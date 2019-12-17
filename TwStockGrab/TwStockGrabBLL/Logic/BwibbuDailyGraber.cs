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
    /// 交易資訊->盤後資訊->個股日本益比、殖利率及股價淨值比（依日期查詢）
    /// bwibbu_daily
    /// 本資料自民國94年09月02日開始提供
    /// </summary>
    public class BwibbuDailyGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALL");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                BWIBBU_D_Rsp rsp = JsonConvert.DeserializeObject<BWIBBU_D_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, type);
                    Sleep();
                }
            }
        }
        

        private void SaveToDatabase(BWIBBU_D_Rsp rsp, DateTime grabeDate, string selectType)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);
            DateTime newTypeDataDate = new DateTime(2017, 4, 13);

            List<bwibbu_daily> tmpAddList = new List<bwibbu_daily>();
            List<bwibbu_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<bwibbu_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                bwibbu_daily obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {

                    if (dataDate >= newTypeDataDate)
                    {
                        tmpAddList.Add(new bwibbu_daily
                        {
                            data_date = dataDate,
                            select_type = selectType,
                            stock_no = stockNo,
                            stock_name = stockName,
                            yield_rate = ToDecimalQ(data.ElementAt(2)),
                            dividend_yearly = ToIntQ(data.ElementAt(3)),
                            pe_ratio = ToDecimalQ(data.ElementAt(4)),
                            pb_ratio = ToDecimalQ(data.ElementAt(5)),
                            report_year = GetAdYearFromRocYearSeasonDataString(data.ElementAt(6).Trim()),
                            report_season = GetSeasonNoFromRocYearSeasonDataString(data.ElementAt(6).Trim()),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new bwibbu_daily
                        {
                            data_date = dataDate,
                            select_type = selectType,
                            stock_no = stockNo,
                            stock_name = stockName,
                            pe_ratio = ToDecimalQ(data.ElementAt(2)),
                            yield_rate = ToDecimalQ(data.ElementAt(3)),
                            pb_ratio = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }

                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bwibbu_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/BWIBBU_d?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
