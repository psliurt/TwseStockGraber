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
    public class DRtBrkGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 >  熱門股證券商進出排行
        /// d_rt_brk
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/active_BROKER_vol/rt_brk.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DRtBrk_Rsp rsp = JsonConvert.DeserializeObject<DRtBrk_Rsp>(responseContent);
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

        private void SaveToDatabase(DRtBrk_Rsp rsp, DateTime dataDate)
        {
            List<d_rt_brk> tmpAddList = new List<d_rt_brk>();
            List<d_rt_brk> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_rt_brk>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int dealerOrder = ToInt(data.ElementAt(0).Trim());
                int rankOrder = ToInt(data.ElementAt(4).Trim());
                string dealerName = data.ElementAt(1).Trim();
                string stockNo = "";
                string stockName = "";
                ParseOutStockNoAndName(data.ElementAt(5).Trim(), out stockNo, out stockName);

                d_rt_brk existItem = tmpDataList.Where(x => x.stock_name == stockName && x.dealer_order == dealerOrder && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_rt_brk
                    {
                        data_date = dataDate,
                        dealer_order = dealerOrder,
                        dealer_name = dealerName,
                        buy_count = ToIntQ(data.ElementAt(2).Trim()),
                        sell_count = ToIntQ(data.ElementAt(3).Trim()),
                        rank_order = rankOrder,
                        stock_name = stockName,
                        stock_no = stockNo,
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_rt_brk.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/active_BROKER_vol/rt_brk_result.php?l=zh-tw&d=108/12/04&_=1575797814858
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/active_BROKER_vol/rt_brk_result.php?l={0}&d={1}&_={2}",
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

        protected long? ToLongQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            data = data.Replace(",", "");
            return Convert.ToInt64(data);
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

        private int? ToIntQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            data = data.Replace(",", "");
            return Convert.ToInt32(data);
        }

        protected void ParseOutStockNoAndName(string data, out string stockNo, out string stockName)
        {
            string[] stockParts = data.Split('(');
            if (stockParts.Count() > 1)
            {
                stockName = stockParts[0].Trim().TrimEnd('(').Trim();
                stockNo = stockParts[1].Trim().TrimEnd(')').Trim();
            }
            else
            {
                stockName = stockParts[0].Trim().TrimEnd('(').Trim();
                stockNo = "";
            }
            

        }
    }
}
