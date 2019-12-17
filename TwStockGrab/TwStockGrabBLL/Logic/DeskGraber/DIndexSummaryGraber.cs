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
    public class DIndexSummaryGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 > 上櫃股價指數收盤行情
        /// d_index_summary
        /// 本資訊自民國105年01月起開始提供   由2016/01/04開始提供    
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DIndexSummary_Rsp rsp = JsonConvert.DeserializeObject<DIndexSummary_Rsp>(responseContent);
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

        private void SaveToDatabase(DIndexSummary_Rsp rsp, DateTime dataDate)
        {
            List<d_index_summary> tmpAddList = new List<d_index_summary>();
            List<d_index_summary> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_index_summary>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string indexName = data.ElementAt(0).Trim();

                d_index_summary existItem = tmpDataList.Where(x => x.index_name == indexName && x.data_date == dataDate && x.index_cls == 1).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_index_summary
                    {
                        data_date = dataDate,
                        index_name = indexName,
                        index_cls = 1,
                        index_price = ToDecimalQ(data.ElementAt(1).Trim()),
                        up_down_price = ToDecimalQ(data.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(data.ElementAt(3)),                        
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }


            foreach (var mmData in rsp.mmData)
            {
                string indexName = mmData.ElementAt(0).Trim();

                d_index_summary existItem = tmpDataList.Where(x => x.index_name == indexName && x.data_date == dataDate && x.index_cls == 2).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_index_summary
                    {
                        data_date = dataDate,
                        index_name = indexName,
                        index_cls = 2,
                        index_price = ToDecimalQ(mmData.ElementAt(1).Trim()),
                        up_down_price = ToDecimalQ(mmData.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(mmData.ElementAt(3)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_index_summary.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary_result.php?l=zh-tw&d=108/12/05&_=1575722730242
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary_result.php?l={0}&d={1}&_={2}",
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

            string noCommaString = data.Replace(",", "");

            return Convert.ToDecimal(noCommaString);
        }
    }
}
