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
    /// 交易資訊->融資融券與可借券賣出額度->融券借券賣出餘額
    /// twt93u
    /// 本資訊自民國94年7月1日起開始提供
    /// </summary>
    public class Twt93uGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {        
            string responseContent = GetWebContent(dataDate);
            TWT93U_Rsp rsp = JsonConvert.DeserializeObject<TWT93U_Rsp>(responseContent);

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
                

        private void SaveToDatabase(TWT93U_Rsp rsp, DateTime grabeDate)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);

            List<twt93u> tmpAddList = new List<twt93u>();
            List<twt93u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt93u>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                //this._stockBag.CheckStock(stockNo, stockName);

                twt93u obj =
                    tmpDataList.Where(x => x.data_date == dataDate &&  x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twt93u
                    {
                        data_date = dataDate,                        
                        stock_no = stockNo,
                        stock_name = stockName,
                        margin_yesterday_balance = ToDecimalQ(data.ElementAt(2)),
                        margin_sell_out = ToDecimalQ(data.ElementAt(3)),
                        margin_buy_in = ToDecimalQ(data.ElementAt(4)),
                        margin_current_ticket = ToLongQ(data.ElementAt(5)),
                        margin_today_balance = ToDecimalQ(data.ElementAt(6)),
                        margin_ceiling = ToDecimalQ(data.ElementAt(7)),
                        lend_yesterday_balance = ToDecimalQ(data.ElementAt(8)),
                        lend_sell_out = ToDecimalQ(data.ElementAt(9)),
                        lend_return_back = ToDecimalQ(data.ElementAt(10)),
                        lend_adjust = ToDecimalQ(data.ElementAt(11)),
                        lend_balance = ToDecimalQ(data.ElementAt(12)),
                        lend_next_ceiling = ToDecimalQ(data.ElementAt(13)),                        
                        note = data.ElementAt(14).Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt93u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWT93U?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
