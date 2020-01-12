using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic.Rsp.Json.Desk;
using TwStockGrabBLL.DAL;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    public class D3itrdsumMonthlyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣金額彙總表(月)
        /// d_3itrdsum_monthly
        /// 本資訊自民國96年1月起開始提供
        /// 民國93年6月至95年12月資訊由下面的網址查詢
        /// https://hist.tpex.org.tw/Hist/STOCK/3INSTI/3INSTISUM.HTML
        /// 
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime monthFirstDate = GetMonthFirstDay(dataDate);

            string responseContent = GetWebContent(monthFirstDate);
            D3itrdsumMonthly_Rsp rsp = JsonConvert.DeserializeObject<D3itrdsumMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, monthFirstDate);
                Sleep();
            }
        }

        private void SaveToDatabase(D3itrdsumMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_3itrdsum_monthly> tmpAddList = new List<d_3itrdsum_monthly>();
            List<d_3itrdsum_monthly> tmpUpdateList = new List<d_3itrdsum_monthly>();
            List<d_3itrdsum_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrdsum_monthly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string itemName = data.ElementAt(0).Trim();

                d_3itrdsum_monthly existItem = tmpDataList.Where(x => x.item_name == itemName && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_3itrdsum_monthly
                    {
                        data_date = dataDate,
                        item_name = itemName,
                        buy_in_money = ToDecimalQ(data.ElementAt(1)),
                        sell_out_money = ToDecimalQ(data.ElementAt(2)),
                        diff_money = ToDecimalQ(data.ElementAt(3)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {
                    existItem.buy_in_money = ToDecimalQ(data.ElementAt(1));
                    existItem.sell_out_money = ToDecimalQ(data.ElementAt(2));
                    existItem.diff_money = ToDecimalQ(data.ElementAt(3));
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrdsum_monthly.AddRange(tmpAddList);
                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_3itrdsum_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "M"; //monthly
            string stockSelectType = "1";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l=zh-tw&t=M&p=1&d=108/12/01&_=1578804004561
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l={0}&t={1}&p={2}&d={3}&_={4}",
                lang, dataType, stockSelectType, rocDate, paramUnderLine);

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

        private DateTime GetMonthFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
    }
}
