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
    public class DForgtrMonthlyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 三大法人 > 外資及陸資買賣超彙總表(月)
        /// d_forgtr_monthly
        /// 本資訊自民國96年1月起開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime monthFirstDay = GetMonthFirstDay(dataDate);

            List<string> typeList = new List<string>();
            typeList.Add("buy");
            typeList.Add("sell");
            foreach (var t in typeList)
            {
                string responseContent = GetWebContent(monthFirstDay, t);
                DForgtrMonthly_Rsp rsp = JsonConvert.DeserializeObject<DForgtrMonthly_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, monthFirstDay, t);
                    Sleep();
                }
            }
        }

        private void SaveToDatabase(DForgtrMonthly_Rsp rsp, DateTime dataDate, string t)
        {
            DateTime period2Start = new DateTime(2018, 1, 1);
            short buySellType = TransBuySellType(t);

            List<d_forgtr_monthly> tmpAddList = new List<d_forgtr_monthly>();
            List<d_forgtr_monthly> tmpUpdateList = new List<d_forgtr_monthly>();
            List<d_forgtr_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_forgtr_monthly>().Where(x => x.data_date == dataDate && x.buy_sell_type == buySellType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_forgtr_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.buy_sell_type == buySellType).FirstOrDefault();
                if (existItem == null)
                {
                    if (dataDate < period2Start)
                    {
                        tmpAddList.Add(new d_forgtr_monthly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            buy_sell_type = buySellType,
                            rank = ToInt(data.ElementAt(0).Trim()),
                            stock_name = data.ElementAt(2).Trim(),
                            total_buy_in = ToIntQ(data.ElementAt(3)),
                            total_sell_out = ToIntQ(data.ElementAt(4)),
                            total_diff = ToDecimalQ(data.ElementAt(5)),
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new d_forgtr_monthly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            buy_sell_type = buySellType,
                            rank = ToInt(data.ElementAt(0).Trim()),
                            stock_name = data.ElementAt(2).Trim(),
                            buy_in = ToIntQ(data.ElementAt(3)),
                            sell_out = ToIntQ(data.ElementAt(4)),
                            diff = ToIntQ(data.ElementAt(5)),
                            self_buy_in = ToIntQ(data.ElementAt(6)),
                            self_sell_out = ToIntQ(data.ElementAt(7)),
                            self_diff = ToIntQ(data.ElementAt(8)),
                            total_buy_in = ToIntQ(data.ElementAt(9)),
                            total_sell_out = ToIntQ(data.ElementAt(10)),
                            total_diff = ToDecimalQ(data.ElementAt(11)),
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                }
                else
                {
                    if (dataDate < period2Start)
                    {
                        existItem.rank = ToInt(data.ElementAt(0).Trim());
                        existItem.stock_name = data.ElementAt(2).Trim();
                        existItem.total_buy_in = ToIntQ(data.ElementAt(3));
                        existItem.total_sell_out = ToIntQ(data.ElementAt(4));
                        existItem.total_diff = ToDecimalQ(data.ElementAt(5));
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;

                        tmpUpdateList.Add(existItem);

                    }
                    else
                    {
                        existItem.rank = ToInt(data.ElementAt(0).Trim());
                        existItem.stock_name = data.ElementAt(2).Trim();
                        existItem.buy_in = ToIntQ(data.ElementAt(3));
                        existItem.sell_out = ToIntQ(data.ElementAt(4));
                        existItem.diff = ToIntQ(data.ElementAt(5));
                        existItem.self_buy_in = ToIntQ(data.ElementAt(6));
                        existItem.self_sell_out = ToIntQ(data.ElementAt(7));
                        existItem.self_diff = ToIntQ(data.ElementAt(8));
                        existItem.total_buy_in = ToIntQ(data.ElementAt(9));
                        existItem.total_sell_out = ToIntQ(data.ElementAt(10));
                        existItem.total_diff = ToDecimalQ(data.ElementAt(11));
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;

                        tmpUpdateList.Add(existItem);
                    }
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_forgtr_monthly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_forgtr_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string t)
        {
            string lang = "zh-tw";
            string dataType = "M"; //monthly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr_result.php?l=zh-tw&t=M&type=buy&d=107/12/01&_=1578818642519
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, t, rocDate, paramUnderLine);

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

        private DateTime GetMonthFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
    }
}
