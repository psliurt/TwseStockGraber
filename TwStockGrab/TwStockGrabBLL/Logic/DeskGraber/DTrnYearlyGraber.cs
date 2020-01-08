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
    public class DTrnYearlyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股週轉率排行(年)
        /// d_trn_monthly
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime yearDate = new DateTime(dataDate.Year, 1, 1);

            string responseContent = GetWebContent(yearDate);
            DTrnYearly_Rsp rsp = JsonConvert.DeserializeObject<DTrnYearly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, yearDate);
                Sleep();
            }
        }

        /// <summary>
        /// 這邊的dataDate在傳進來function已經做過處理，都會是每個月的第一天
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="dataDate"></param>
        private void SaveToDatabase(DTrnYearly_Rsp rsp, DateTime dataDate)
        {
            List<d_trn_yearly> tmpAddList = new List<d_trn_yearly>();
            List<d_trn_yearly> tmpUpdateList = new List<d_trn_yearly>();
            List<d_trn_yearly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_trn_yearly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_trn_yearly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_trn_yearly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        issue_stock_count = ToLongQ(data.ElementAt(4).Trim()),
                        turnover_rate = ToDecimalQ(data.ElementAt(5).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else //已經存在，就要用update
                {
                    existItem.deal_stock_count = ToLongQ(data.ElementAt(3).Trim());
                    existItem.issue_stock_count = ToLongQ(data.ElementAt(4).Trim());
                    existItem.turnover_rate = ToDecimalQ(data.ElementAt(5).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_trn_yearly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_trn_yearly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "Y"; //Monthly            

            string adYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l=zh-tw&t=Y&d=2019&_=1578370079134
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, adYear, paramUnderLine);

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
            } while (rnd < 4000);
            Thread.Sleep(rnd);
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
    }
}
