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
    /// 交易資訊 > 盤後資訊 > 每5秒委託成交統計
    /// mi_5mins
    /// 本資訊自民國93年10月15日起開始提供
    /// </summary>
    public class Mi5MinsGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            MI_5MINS_Rsp rsp = JsonConvert.DeserializeObject<MI_5MINS_Rsp>(responseContent);
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

        private void SaveToDatabase(MI_5MINS_Rsp rsp, DateTime dataDate)
        {
            DateTime tomorrow = dataDate.AddDays(1);
            List<mi_5mins> tmpAddList = new List<mi_5mins>();
            List<mi_5mins> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_5mins>().AsNoTracking().Where(x => x.data_time >= dataDate && x.data_time < tomorrow).ToList();
            }

            foreach (var data in rsp.data)
            {
                DateTime dataTime = AddTimeToDateFromTimeString(dataDate, data.ElementAt(0));

                mi_5mins obj = tmpDataList.Where(x => x.data_time == dataTime).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new mi_5mins
                    {
                        data_time = dataTime,
                        ask_buy_in_cnt = ToIntQ(data.ElementAt(1)),
                        ask_buy_in_qty = ToLongQ(data.ElementAt(2)),
                        ask_sell_out_cnt = ToIntQ(data.ElementAt(3)),
                        ask_sell_out_qty = ToLongQ(data.ElementAt(4)),
                        deal_cnt = ToIntQ(data.ElementAt(5)),
                        deal_qty = ToLongQ(data.ElementAt(6)),
                        deal_money = ToDecimalQ(data.ElementAt(7)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }                
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_5mins.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_5MINS?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
