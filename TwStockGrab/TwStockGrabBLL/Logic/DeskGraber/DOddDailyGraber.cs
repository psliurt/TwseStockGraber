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
using TwStockGrabBLL.Logic.Rsp.Json.Desk;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    public class DOddDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 鉅額交易 > 鉅額交易日成交量值統計
        /// d_odd_daily
        /// 本資訊自民國96年1月起開始提供 實際由 2007/4 月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/block_trade/daily_trade_sum/odd.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DOddDaily_Rsp rsp = JsonConvert.DeserializeObject<DOddDaily_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }
        }

        private void SaveToDatabase(DOddDaily_Rsp rsp, DateTime dataDate)
        {
            DateTime firstDay = new DateTime(dataDate.Year, dataDate.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

            List<d_odd_daily> tmpAddList = new List<d_odd_daily>();
            List<d_odd_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_odd_daily>().AsNoTracking().Where(x => x.data_date >= firstDay && x.data_date <= lastDay).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string recordDateStr = data.ElementAt(0).Trim();
                DateTime recordDataDate = GetDateFromRocStringQ(recordDateStr).Value;
                string tradeType = data.ElementAt(1).Trim();                

                d_odd_daily existItem = tmpDataList.Where(x => x.data_date == recordDataDate && x.trade_type == tradeType).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_odd_daily
                    {
                        data_date = recordDataDate,
                        trade_type = tradeType,
                        deal_cnt = ToIntQ(data.ElementAt(2).Trim()),
                        deal_stock_cnt = ToDecimalQ(data.ElementAt(3).Trim()),
                        deal_percent = ToDecimalQ(data.ElementAt(4).Trim()),
                        deal_money = ToDecimalQ(data.ElementAt(5).Trim()),
                        deal_money_percent = ToDecimalQ(data.ElementAt(6).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_odd_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            //日期要用該月第一天
            DateTime mohthFirstDay = new DateTime(date.Year, date.Month, 1);

            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(mohthFirstDay);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/block_trade/daily_trade_sum/odd_result.php?l=zh-tw&d=108/12/01&_=1578014227377
            string url = string.Format("https://www.tpex.org.tw/web/stock/block_trade/daily_trade_sum/odd_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        

    }
}
