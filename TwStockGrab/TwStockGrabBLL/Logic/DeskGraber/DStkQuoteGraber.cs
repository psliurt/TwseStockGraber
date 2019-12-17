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
    public class DStkQuoteGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 > 上櫃股票行情
        /// d_stk_quote
        /// 本資訊自民國96年1月起開始提供，但實際上由 2007/04/23 開始提供       
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/daily_close_quotes/stk_quote.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DStkQuote_Rsp rsp = JsonConvert.DeserializeObject<DStkQuote_Rsp>(responseContent);
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

        private void SaveToDatabase(DStkQuote_Rsp rsp, DateTime dataDate)
        {
            List<d_stk_quote> tmpAddList = new List<d_stk_quote>();
            List<d_stk_quote> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_stk_quote>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_stk_quote existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_quote
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        close_p = ToDecimalQ(data.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(data.ElementAt(3).Trim()),
                        open_p = ToDecimalQ(data.ElementAt(4)),
                        high_p = ToDecimalQ(data.ElementAt(5)),
                        low_p = ToDecimalQ(data.ElementAt(6)),
                        avg_p = ToDecimalQ(data.ElementAt(7)),
                        deal_stock_cnt = ToLongQ(data.ElementAt(8)),
                        deal_money = ToDecimalQ(data.ElementAt(9)),
                        deal_cnt = ToLongQ(data.ElementAt(10)),
                        last_buy_price = ToDecimalQ(data.ElementAt(11)),
                        last_sell_price = ToDecimalQ(data.ElementAt(12)),
                        issue_stock_cnt = ToLongQ(data.ElementAt(13)),
                        next_ref_price = ToDecimalQ(data.ElementAt(14)),
                        next_max_price = ToDecimalQ(data.ElementAt(15)),
                        next_min_price = ToDecimalQ(data.ElementAt(16)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }


            foreach (var mmData in rsp.mmData)
            {
                string stockNo = mmData.ElementAt(0).Trim();

                d_stk_quote existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_quote
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = mmData.ElementAt(1).Trim(),
                        close_p = ToDecimalQ(mmData.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(mmData.ElementAt(3).Trim()),
                        open_p = ToDecimalQ(mmData.ElementAt(4)),
                        high_p = ToDecimalQ(mmData.ElementAt(5)),
                        low_p = ToDecimalQ(mmData.ElementAt(6)),
                        avg_p = ToDecimalQ(mmData.ElementAt(7)),
                        deal_stock_cnt = ToLongQ(mmData.ElementAt(8)),
                        deal_money = ToDecimalQ(mmData.ElementAt(9)),
                        deal_cnt = ToLongQ(mmData.ElementAt(10)),
                        last_buy_price = ToDecimalQ(mmData.ElementAt(11)),
                        last_sell_price = ToDecimalQ(mmData.ElementAt(12)),
                        issue_stock_cnt = ToLongQ(mmData.ElementAt(13)),
                        next_ref_price = ToDecimalQ(mmData.ElementAt(14)),
                        next_max_price = ToDecimalQ(mmData.ElementAt(15)),
                        next_min_price = ToDecimalQ(mmData.ElementAt(16)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_stk_quote.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_close_quotes/stk_quote_result.php?l=zh-tw&d=108/12/04&_=1575560995627
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_close_quotes/stk_quote_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

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
            } while (rnd < 3500);
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

            if (data.Trim() == "除權息")
            {
                return null;
            }

            if (data.Trim() == "除息")
            {
                return null;
            }

            if (data.Trim() == "除權")
            {
                return null;
            }

            string noCommaString = data.Replace(",", "");

            decimal d = 0;
            if (decimal.TryParse(noCommaString, out d))
            {
                return d;
            }
            else
            {
                return null;
            }
        }

        private long? ToLongQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            data = data.Replace(",", "");
            return Convert.ToInt64(data);
        }
    }
}
