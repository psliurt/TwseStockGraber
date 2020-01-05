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
    public class DMgUsedWeeklyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 融資融券使用率報表(週)
        /// d_mgused_weekly
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23那一週開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused.php?l=zh-tw
        /// 日期請用星期六的日期
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            List<string> sideList = new List<string>();
            sideList.Add("Marg"); //融資
            sideList.Add("Shrt"); //融券

            foreach (var side in sideList)
            {
                string responseContent = GetWebContent(dataDate, side);
                DMgUsedWeekly_Rsp rsp = JsonConvert.DeserializeObject<DMgUsedWeekly_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, side);
                    Sleep();
                }
            }
        }

        private void SaveToDatabase(DMgUsedWeekly_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_mgused_weekly> tmpAddList = new List<d_mgused_weekly>();
            List<d_mgused_weekly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_mgused_weekly>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_mgused_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.mg_type == marginType && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_mgused_weekly
                    {
                        data_date = dataDate,
                        mg_type = marginType,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        used_percent = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_mgused_weekly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string sideType)
        {
            string lang = "zh-tw";
            string freq = "W";

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused_result.php?l=zh-tw&t=W&type=Marg&d=108/12/21&_=1578156277917
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, freq, sideType, rocDate, paramUnderLine);

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

        private int? ToIntQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            data = data.Replace(",", "");
            data = data.Replace("(", "");
            data = data.Replace(")", "");
            return Convert.ToInt32(data);
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
    }
}
