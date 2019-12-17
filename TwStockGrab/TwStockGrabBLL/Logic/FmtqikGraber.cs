using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->盤後資訊->每日市場成交資訊
    /// fmtqik
    /// 本資訊自民國79年1月4日起開始提供
    /// </summary>
    public class FmtqikGraber : Graber
    {
        /// <summary>
        /// 如果要抓資料這個可以每七天跑一次就好
        /// 每天跑則是抓完歷史資料後可以考慮的
        /// </summary>
        /// <param name="dataDate"></param>
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            FMTQIK_Rsp rsp = JsonConvert.DeserializeObject<FMTQIK_Rsp>(responseContent);
            if (rsp.data == null)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }            
        }        

        private void SaveToDatabase(FMTQIK_Rsp rsp, DateTime dataDate)
        {
            DateTime thisMonthStart = new DateTime(dataDate.Year, dataDate.Month, 1);
            DateTime nextMonthStart = thisMonthStart.AddMonths(1);

            List<fmtqik> tmpAddList = new List<fmtqik>();
            List<fmtqik> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<fmtqik>().AsNoTracking().Where(x => x.data_date >= thisMonthStart && x.data_date < nextMonthStart).ToList();
            }

            
            foreach (var data in rsp.data)
            {
                DateTime? realDataDate = GetDateFromRocSlashStringQ(data.ElementAt(0));

                fmtqik obj = tmpDataList.Where(x => x.data_date == realDataDate).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new fmtqik
                    {
                        data_date = realDataDate.Value,
                        deal_stock_num = ToDecimalQ(data.ElementAt(1)),
                        deal_money = ToDecimalQ(data.ElementAt(2)),
                        deal_trade_num = ToDecimalQ(data.ElementAt(3)),
                        issue_weight = ToDecimalQ(data.ElementAt(4)),
                        up_down_point = ToDecimalQ(data.ElementAt(5)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }

            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.fmtqik.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();



            string url = string.Format("https://www.twse.com.tw/exchangeReport/FMTQIK?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
