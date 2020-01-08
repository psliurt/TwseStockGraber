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
    public class DVolRankMonthlyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股成交量排行(月)
        /// d_vol_rank_monthly
        /// 本資訊自民國96年1月起開始提供 實際上由 2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime monthFirstDay = GetMonthFirstDay(dataDate);

            string responseContent = GetWebContent(monthFirstDay);
            DVolRankMonthly_Rsp rsp = JsonConvert.DeserializeObject<DVolRankMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, monthFirstDay);
                Sleep();
            }

        }

        private void SaveToDatabase(DVolRankMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_vol_rank_monthly> tmpAddList = new List<d_vol_rank_monthly>();
            List<d_vol_rank_monthly> tmpUpdateList = new List<d_vol_rank_monthly>();
            List<d_vol_rank_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_vol_rank_monthly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_vol_rank_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_vol_rank_monthly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_sheet_count = ToLongQ(data.ElementAt(3).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {
                    existItem.deal_sheet_count = ToLongQ(data.ElementAt(3).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_vol_rank_monthly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_vol_rank_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "M"; //monthly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank_result.php?l=zh-tw&t=M&d=104/07/01&_=1578381659645
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

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

        private DateTime GetMonthFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
    }
}
