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
    public class DLendGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 標借
        /// d_lend
        /// 本資訊自民國92年8月起開始提供 2007/1月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/lend/lend.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DLend_Rsp rsp = JsonConvert.DeserializeObject<DLend_Rsp>(responseContent);
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

        private void SaveToDatabase(DLend_Rsp rsp, DateTime dataDate)
        {
            List<d_lend> tmpAddList = new List<d_lend>();
            List<d_lend> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_lend>().AsNoTracking().ToList();
            }

            foreach (var data in rsp.aaData)
            {
                DateTime lendDate = GetDateFromRocStringQ(data.ElementAt(0).Trim()).Value.Date;
                string stockNo = data.ElementAt(1).Trim();

                d_lend existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == lendDate).FirstOrDefault();
                if (existItem == null)
                {
                    decimal lendFloor = 0;
                    decimal lendCeil = 0;
                    ParseFloorAndCeil(data.ElementAt(7).Trim(), ref lendFloor, ref lendCeil);
                    tmpAddList.Add(new d_lend
                    {
                        data_date = lendDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        stock_agent = data.ElementAt(3).Trim(),
                        lend_cnt = ToIntQ(data.ElementAt(4).Trim()),
                        lend_max_price = ToDecimalQ(data.ElementAt(5).Trim()),
                        lend_success_cnt = ToIntQ(data.ElementAt(6).Trim()),
                        lend_floor = lendFloor,
                        lend_ceil = lendCeil,
                        lend_fail_cnt = ToIntQ(data.ElementAt(8).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_lend.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";            
            DateTime endDate = date.AddMonths(1).AddDays(-1);
            string rocStartDate = ParseADDateToRocString(date);
            string rocEndDate = ParseADDateToRocString(endDate);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/lend/lend_result.php?l=zh-tw&sd=99/01/01&ed=109/01/04&stkno=&_=1578116199212
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/lend/lend_result.php?l={0}&sd={1}&ed={2}&stkno=&_={3}",
                lang,  rocStartDate, rocEndDate, paramUnderLine);

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

        private void ParseFloorAndCeil(string srcData, ref decimal floor, ref decimal ceil)
        {
            if (string.IsNullOrEmpty(srcData))
            {
                return;
            }

            string[] dataParts = srcData.Split('~');
            floor = decimal.Parse(dataParts[0].Trim());
            ceil = decimal.Parse(dataParts[1].Trim());
        }
    }
}
