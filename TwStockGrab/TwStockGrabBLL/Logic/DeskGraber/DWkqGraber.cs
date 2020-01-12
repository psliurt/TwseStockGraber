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
    public class DWkqGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 證券行情週報表
        /// d_wkq
        /// 本資訊自民國96年1月起開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/weekly_report/wkq.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime weekFirstDay = GetWeekMondayDate(dataDate);

            string responseContent = GetWebContent(weekFirstDay);
            DWkq_Rsp rsp = JsonConvert.DeserializeObject<DWkq_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, weekFirstDay);
                Sleep();
            }

        }

        private void SaveToDatabase(DWkq_Rsp rsp, DateTime dataDate)
        {
            List<d_wkq> tmpAddList = new List<d_wkq>();
            List<d_wkq> tmpUpdateList = new List<d_wkq>();
            List<d_wkq> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_wkq>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {                
                string stockNo = data.ElementAt(0).Trim();

                d_wkq existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_wkq
                    {
                        data_date = dataDate,                        
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        ref_price = ToDecimalQ( data.ElementAt(2).Trim()),
                        high_max_price = ToDecimalQ(data.ElementAt(3).Trim()),
                        low_min_price = ToDecimalQ(data.ElementAt(4).Trim()),
                        close_price = ToDecimalQ(data.ElementAt(5).Trim()),
                        open_price = ToDecimalQ(data.ElementAt(6).Trim()),
                        high_price = ToDecimalQ(data.ElementAt(7).Trim()),
                        low_price = ToDecimalQ(data.ElementAt(8).Trim()),
                        deal_sheet_cnt = ToLongQ(data.ElementAt(9).Trim()),
                        deal_money = ToDecimalQ(data.ElementAt(10).Trim()),
                        deal_cnt = ToLongQ(data.ElementAt(11).Trim()),
                        pe_ratio = ToDecimalQ(data.ElementAt(12).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {
                    existItem.ref_price = ToDecimalQ(data.ElementAt(2).Trim());
                    existItem.high_max_price = ToDecimalQ(data.ElementAt(3).Trim());
                    existItem.low_min_price = ToDecimalQ(data.ElementAt(4).Trim());
                    existItem.close_price = ToDecimalQ(data.ElementAt(5).Trim());
                    existItem.open_price = ToDecimalQ(data.ElementAt(6).Trim());
                    existItem.high_price = ToDecimalQ(data.ElementAt(7).Trim());
                    existItem.low_price = ToDecimalQ(data.ElementAt(8).Trim());
                    existItem.deal_sheet_cnt = ToLongQ(data.ElementAt(9).Trim());
                    existItem.deal_money = ToDecimalQ(data.ElementAt(10).Trim());
                    existItem.deal_cnt = ToLongQ(data.ElementAt(11).Trim());
                    existItem.pe_ratio = ToDecimalQ(data.ElementAt(12).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_wkq.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_wkq>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {

            string lang = "zh-tw";
            string dataType = "json";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/weekly_report/wkq_result.php?o=json&l=zh-tw&d=108/12/26&_=1578797076863
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/weekly_report/wkq_result.php?o={0}&l={1}&d={2}&_={3}",
                dataType, lang, rocDate, paramUnderLine);

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

            if (data == "null")
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

            if (data.Trim() == "N/A")
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

        private DateTime GetWeekMondayDate(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    return dt.AddDays(-1);
                case DayOfWeek.Wednesday:
                    return dt.AddDays(-2);
                case DayOfWeek.Thursday:
                    return dt.AddDays(-3);
                case DayOfWeek.Friday:
                    return dt.AddDays(-4);
                case DayOfWeek.Saturday:
                    return dt.AddDays(-5);
                case DayOfWeek.Sunday:
                    return dt.AddDays(-6);
                default:
                    return dt;
            }
        }
    }
}
