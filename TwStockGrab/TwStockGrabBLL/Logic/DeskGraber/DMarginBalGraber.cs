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
    public class DMarginBalGraber
    {
        public void DoJob(DateTime dataDate)
        {            
            string responseContent = GetWebContent(dataDate);
            DMarginBal_Rsp rsp = JsonConvert.DeserializeObject<DMarginBal_Rsp>(responseContent);
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

        private void SaveToDatabase(DMarginBal_Rsp rsp, DateTime dataDate)
        {
            List<d_margin_bal> tmpAddList = new List<d_margin_bal>();
            List<d_margin_bal> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_bal>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_margin_bal existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_margin_bal
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        yesterday_lend_balance = ToIntQ(data.ElementAt(2).Trim()),                        
                        lend_buy = ToIntQ(data.ElementAt(3)),
                        lend_sell = ToIntQ(data.ElementAt(4)),
                        lend_back = ToIntQ(data.ElementAt(5)),
                        lend_balance = ToIntQ(data.ElementAt(6)),
                        lend_margin = ToIntQ(data.ElementAt(7)),
                        lend_percent = ToDecimalQ(data.ElementAt(8)),
                        lend_limit = ToDecimalQ(data.ElementAt(9)),
                        yesterday_borrow_balance = ToIntQ(data.ElementAt(10)),
                        borrow_sell = ToIntQ(data.ElementAt(11)),
                        borrow_buy = ToIntQ(data.ElementAt(12)),
                        borrow_back = ToIntQ(data.ElementAt(13)),
                        borrow_balance = ToIntQ(data.ElementAt(14)),
                        borrow_margin = ToIntQ(data.ElementAt(15)),
                        borrow_percent = ToDecimalQ(data.ElementAt(16)),
                        borrow_limit = ToDecimalQ(data.ElementAt(17)),
                        offset  =  ToIntQ(data.ElementAt(18)),
                        note =  data.ElementAt(19).Trim(),
                        title = string.Format("{0} 融資融券餘額表", rsp.reportDate),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });


                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_margin_bal.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string responseDataType = "json";            

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal_result.php?l=zh-tw&o=json&d=108/11/07&_=1573477027219
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal_result.php?l={0}&o={1}&d={2}&_={3}",
                lang, responseDataType, rocDate, paramUnderLine);

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
