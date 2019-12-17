using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic
{
    public abstract class CabGraber
    {
        /// <summary>
        /// 上櫃的Graber?
        /// </summary>
        public CabGraber()
        {

        }

        /// <summary>
        /// 執行抓資料跟存資料的工作
        /// </summary>
        /// <param name="grabeDate">執行抓取資料的日期</param>
        public abstract void DoJob(DateTime grabeDate);

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
        protected string GetTimeStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 休息幾秒鐘後，再繼續抓資料，防止IP被ban掉
        /// </summary>
        protected void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(5000);
            } while (rnd < 2500);
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
