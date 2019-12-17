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
    public class DMarketHighlightGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 > 上櫃股票市場現況
        /// d_index_summary
        /// 本資訊自民國96年01月起開始提供  實際上從 2007/04/23開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DMarketHighlight_Rsp rsp = JsonConvert.DeserializeObject<DMarketHighlight_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }
        }

        private void SaveToDatabase(DMarketHighlight_Rsp rsp, DateTime dataDate)
        {
            List<d_market_highlight> tmpAddList = new List<d_market_highlight>();
            List<d_market_highlight> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_market_highlight>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            if (tmpDataList.Count() <= 0)
            {
                tmpAddList.Add(new d_market_highlight
                {
                    data_date = dataDate,
                    list_stock_count = ToIntQ(rsp.listedNum),
                    total_capital = ToDecimalQ(rsp.capital),
                    total_value = ToDecimalQ(rsp.companyValue),
                    today_trade_value = ToDecimalQ(rsp.tradeAmount),
                    today_trade_stock_count = ToDecimalQ(rsp.tradeVolumn),
                    close_index_price = ToDecimalQ(rsp.close),
                    up_count = ToIntQ(rsp.upNum),
                    down_count = ToIntQ(rsp.downNum),
                    no_change_count = ToIntQ(rsp.noChangeNum),
                    index_up_down_price = ToDecimalQ(rsp.change),
                    up_limit_count = ToIntQ(rsp.upStopNum),
                    down_limit_count = ToIntQ(rsp.downStopNum),
                    no_trade_count = ToIntQ(rsp.noTradeNum),
                    title = rsp.rptNote.Trim(),
                    create_at = DateTime.Now,
                    update_at = DateTime.Now
                });
            }

            

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_market_highlight.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            

            //https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight_result.php?l=zh-tw&d=108/12/04
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight_result.php?l={0}&d={1}",
                lang, rocDate);

            return GetHttpResponse(url);
        }

        /// <summary>
        /// 送出http GET 請求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string GetHttpResponse(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream inputResponseStream = null;
            string responseContent = "";

            inputResponseStream = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(inputResponseStream))
            {
                responseContent = sr.ReadToEnd();
            }

            return responseContent;
        }

        /// <summary>
        /// 取得時間戳記
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }
        /// <summary>
        /// 休息一段時間避免被上櫃的網站ban
        /// </summary>
        private void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(8000);
            } while (rnd < 4000);
            Thread.Sleep(rnd);
        }

        private string ParseADDateToRocString(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            return string.Format("{0}/{1}/{2}",
                (year - 1911).ToString(),
                month.ToString().PadLeft(2, '0'),
                day.ToString().PadLeft(2, '0'));
        }

        private decimal? ToDecimalQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            { return null; }

            if (data == "---")
            {
                return null;
            }

            if (data == "--")
            {
                return null;
            }

            if (data == "-")
            {
                return null;
            }

            string noCommaString = data.Replace(",", "");

            return Convert.ToDecimal(noCommaString);
        }

        private int? ToIntQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            data = data.Replace(",", "");
            return Convert.ToInt32(data);
        }
    }
}
