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
    public class DOddDailyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 鉅額交易 > 鉅額交易日成交量值統計
        /// d_odd_daily
        /// 本資訊自民國96年1月起開始提供 實際由 2007/4 月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/block_trade/daily_trade_sum/odd.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
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

        /// <summary>
        /// 把格式為 yyyMMdd 或是 yyMMdd 的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyyMMdd或yyMMdd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string rocYear = "";
            string rocMonth = "";
            string rocDay = "";

            if (rocString.Length == 6)
            {
                rocYear = rocString.Substring(0, 2);
                rocMonth = rocString.Substring(2, 2);
                rocDay = rocString.Substring(4, 2);
            }
            else
            {
                rocYear = rocString.Substring(0, 3);
                rocMonth = rocString.Substring(3, 2);
                rocDay = rocString.Substring(5, 2);
            }

            
            int year = 1911 + Convert.ToInt32(rocYear);
            int month = Convert.ToInt32(rocMonth);
            int day = Convert.ToInt32(rocDay);

            return new DateTime(year, month, day);
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

    }
}
