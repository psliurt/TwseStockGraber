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
    public class DMarginRankMonthlyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 融資融券增減排行表 (月)
        /// d_margin_rank_monthly
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/1日那個月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank.php?l=zh-tw
        /// * dataDate 請一律用每個月1號
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            List<string> marginTypeList = new List<string>();
            marginTypeList.Add("MargGain"); //融資增加
            marginTypeList.Add("MargLose"); //融資減少
            marginTypeList.Add("ShrtGain"); //融券增加
            marginTypeList.Add("ShrtLose"); //融券減少

            foreach (var marginType in marginTypeList)
            {
                string responseContent = GetWebContent(dataDate, marginType);
                DMarginRankMonthly_Rsp rsp = JsonConvert.DeserializeObject<DMarginRankMonthly_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, marginType);
                    Sleep();
                }
            }

        }

        private void SaveToDatabase(DMarginRankMonthly_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_margin_rank_monthly> tmpAddList = new List<d_margin_rank_monthly>();
            List<d_margin_rank_monthly> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_rank_monthly>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_margin_rank_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {

                    tmpAddList.Add(new d_margin_rank_monthly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        yesterday_balance = ToDecimalQ(data.ElementAt(3).Trim()),
                        today_balance = ToDecimalQ(data.ElementAt(4).Trim()),
                        total_used = ToLongQ(data.ElementAt(5).Trim()),
                        mg_type = marginType,
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_margin_rank_monthly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string marginType)
        {
            string lang = "zh-tw";
            string dataType = "M"; //weekly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();
            //https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l=zh-tw&t=M&type=MargGain&d=108/12/01&_=1578131180044
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, marginType, rocDate, paramUnderLine);

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
        /// 把格式為 yyy/MM/dd 或是 yy/MM/dd 的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyy/MM/dd或yy/MM/dd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string[] dateParts = rocString.Split('/');

            if (dateParts.Length != 3)
            {
                return null;
            }

            int year = 1911 + Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

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

        private short TransBuySellType(string t)
        {
            if (t.Trim().ToLower() == "sell")
            {
                return -1;
            }

            if (t.Trim().ToLower() == "buy")
            {
                return 1;
            }

            return 0;

        }

        private int ToInt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 0;
            }

            data = data.Replace(",", "").Trim();
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

            string noPercentSymbolSring = data.Replace("%", "");

            string noCommaString = noPercentSymbolSring.Replace(",", "");

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

        protected long? ToLongQ(string data)
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
