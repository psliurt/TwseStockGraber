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
    public class DSectrGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 類股成交價量比重
        /// d_sectr
        /// 本資訊自民國96年1月起開始提供 實際由 2007/4/23提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            
                string responseContent = GetWebContent(dataDate);
                DSectr_Rsp rsp = JsonConvert.DeserializeObject<DSectr_Rsp>(responseContent);
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

        private void SaveToDatabase(DSectr_Rsp rsp, DateTime dataDate)
        {
            List<d_sectr> tmpAddList = new List<d_sectr>();
            List<d_sectr> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_sectr>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string sectorName = data.ElementAt(0).Trim();

                d_sectr existItem = tmpDataList.Where(x => x.sector_name == sectorName && x.data_date == dataDate).FirstOrDefault();


                if (existItem == null)
                {
                    tmpAddList.Add(new d_sectr
                    {
                        data_date = dataDate,
                        sector_name = sectorName,
                        deal_money = ToDecimalQ(data.ElementAt(1).Trim()),
                        deal_percent = ToDecimalQ(data.ElementAt(2).Trim()),
                        deal_stock_count = ToLongQ( data.ElementAt(3).Trim()),
                        deal_stock_percent = ToDecimalQ(data.ElementAt(4).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_sectr.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr_result.php?l=zh-tw&d=108/12/04&_=1575800265005
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr_result.php?l={0}&d={1}&_={2}",
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
