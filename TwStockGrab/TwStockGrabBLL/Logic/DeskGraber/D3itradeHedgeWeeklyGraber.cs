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
    public class D3itradeHedgeWeeklyGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣明細資訊(周)
        /// d_3itrade_hedge_weekly
        /// 本資訊自民國96年4月20日起開始提供
        /// 
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge.php?l=zh-tw
        /// </summary>
        public void DoJob(DateTime dataDate)
        {
            DateTime weekFirstDay = GetWeekMondayDate(dataDate);

            string responseContent = GetWebContent(weekFirstDay);
            D3itradeHedgeWeekly_Rsp rsp = JsonConvert.DeserializeObject<D3itradeHedgeWeekly_Rsp>(responseContent);
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

        private void SaveToDatabase(D3itradeHedgeWeekly_Rsp rsp, DateTime dataDate)
        {
            DateTime period1Start = new DateTime(2018, 1, 15);
            List<d_3itrade_hedge_weekly> tmpAddList = new List<d_3itrade_hedge_weekly>();
            List<d_3itrade_hedge_weekly> tmpUpdateList = new List<d_3itrade_hedge_weekly>();
            List<d_3itrade_hedge_weekly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrade_hedge_weekly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_3itrade_hedge_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    if (dataDate < period1Start)
                    {
                        tmpAddList.Add(new d_3itrade_hedge_weekly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            foreign_all_buy_in = ToIntQ(data.ElementAt(2)), //外資及陸資
                            foreign_all_sell_out = ToIntQ(data.ElementAt(3)),
                            foreign_all_diff = ToIntQ(data.ElementAt(4)),
                            invest_buy_in = ToIntQ(data.ElementAt(5)), //投信
                            invest_sell_out = ToIntQ(data.ElementAt(6)),
                            invest_diff = ToIntQ(data.ElementAt(7)),
                            dealer_all_diff = ToIntQ(data.ElementAt(8)), //自營商
                            dealer_self_buy_in = ToIntQ(data.ElementAt(9)),//自營商(自行買賣)
                            dealer_self_sell_out = ToIntQ(data.ElementAt(10)),
                            dealer_self_diff = ToIntQ(data.ElementAt(11)),
                            dealer_risk_buy_in = ToIntQ(data.ElementAt(12)),//自營商(避險)
                            dealer_risk_sell_out = ToIntQ(data.ElementAt(13)),
                            dealer_risk_diff = ToIntQ(data.ElementAt(14)),
                            total_diff = ToIntQ(data.ElementAt(15)),//三大法人買賣合計                            
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new d_3itrade_hedge_weekly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            foreign_buy_in = ToIntQ(data.ElementAt(2)), //外資不含外資自營商
                            foreign_sell_out = ToIntQ(data.ElementAt(3)),
                            foreign_diff = ToIntQ(data.ElementAt(4)),
                            foreign_self_buy_in = ToIntQ(data.ElementAt(5)), //外資自營商
                            foreign_self_sell_out = ToIntQ(data.ElementAt(6)),
                            foreign_self_diff = ToIntQ(data.ElementAt(7)),
                            foreign_all_buy_in = ToIntQ(data.ElementAt(8)), //外資及陸資
                            foreign_all_sell_out = ToIntQ(data.ElementAt(9)),
                            foreign_all_diff = ToIntQ(data.ElementAt(10)),
                            invest_buy_in = ToIntQ(data.ElementAt(11)), //投信
                            invest_sell_out = ToIntQ(data.ElementAt(12)),
                            invest_diff = ToIntQ(data.ElementAt(13)),
                            dealer_self_buy_in = ToIntQ(data.ElementAt(14)), //自營商(自行買賣)
                            dealer_self_sell_out = ToIntQ(data.ElementAt(15)),
                            dealer_self_diff = ToIntQ(data.ElementAt(16)),
                            dealer_risk_buy_in = ToIntQ(data.ElementAt(17)),//自營商(避險)
                            dealer_risk_sell_out = ToIntQ(data.ElementAt(18)),
                            dealer_risk_diff = ToIntQ(data.ElementAt(19)),
                            dealer_all_buy_in = ToIntQ(data.ElementAt(20)), //自營商
                            dealer_all_sell_out = ToIntQ(data.ElementAt(21)),
                            dealer_all_diff = ToIntQ(data.ElementAt(22)),
                            total_diff = ToIntQ(data.ElementAt(23)), //三大法人買賣合計
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                }
                else
                {
                    if (dataDate < period1Start)
                    {
                        existItem.foreign_all_buy_in = ToIntQ(data.ElementAt(2)); //外資及陸資
                        existItem.foreign_all_sell_out = ToIntQ(data.ElementAt(3));
                        existItem.foreign_all_diff = ToIntQ(data.ElementAt(4));
                        existItem.invest_buy_in = ToIntQ(data.ElementAt(5)); //投信
                        existItem.invest_sell_out = ToIntQ(data.ElementAt(6));
                        existItem.invest_diff = ToIntQ(data.ElementAt(7));
                        existItem.dealer_all_diff = ToIntQ(data.ElementAt(8)); //自營商
                        existItem.dealer_self_buy_in = ToIntQ(data.ElementAt(9));//自營商(自行買賣)
                        existItem.dealer_self_sell_out = ToIntQ(data.ElementAt(10));
                        existItem.dealer_self_diff = ToIntQ(data.ElementAt(11));
                        existItem.dealer_risk_buy_in = ToIntQ(data.ElementAt(12));//自營商(避險)
                        existItem.dealer_risk_sell_out = ToIntQ(data.ElementAt(13));
                        existItem.dealer_risk_diff = ToIntQ(data.ElementAt(14));
                        existItem.total_diff = ToIntQ(data.ElementAt(15));//三大法人買賣合計                            
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;
                        tmpUpdateList.Add(existItem);                            
                        
                    }
                    else
                    {


                        existItem.foreign_buy_in = ToIntQ(data.ElementAt(2)); //外資不含外資自營商
                        existItem.foreign_sell_out = ToIntQ(data.ElementAt(3));
                        existItem.foreign_diff = ToIntQ(data.ElementAt(4));
                        existItem.foreign_self_buy_in = ToIntQ(data.ElementAt(5)); //外資自營商
                        existItem.foreign_self_sell_out = ToIntQ(data.ElementAt(6));
                        existItem.foreign_self_diff = ToIntQ(data.ElementAt(7));
                        existItem.foreign_all_buy_in = ToIntQ(data.ElementAt(8)); //外資及陸資
                        existItem.foreign_all_sell_out = ToIntQ(data.ElementAt(9));
                        existItem.foreign_all_diff = ToIntQ(data.ElementAt(10));
                        existItem.invest_buy_in = ToIntQ(data.ElementAt(11)); //投信
                        existItem.invest_sell_out = ToIntQ(data.ElementAt(12));
                        existItem.invest_diff = ToIntQ(data.ElementAt(13));
                        existItem.dealer_self_buy_in = ToIntQ(data.ElementAt(14)); //自營商(自行買賣)
                        existItem.dealer_self_sell_out = ToIntQ(data.ElementAt(15));
                        existItem.dealer_self_diff = ToIntQ(data.ElementAt(16));
                        existItem.dealer_risk_buy_in = ToIntQ(data.ElementAt(17));//自營商(避險)
                        existItem.dealer_risk_sell_out = ToIntQ(data.ElementAt(18));
                        existItem.dealer_risk_diff = ToIntQ(data.ElementAt(19));
                        existItem.dealer_all_buy_in = ToIntQ(data.ElementAt(20)); //自營商
                        existItem.dealer_all_sell_out = ToIntQ(data.ElementAt(21));
                        existItem.dealer_all_diff = ToIntQ(data.ElementAt(22));
                        existItem.total_diff = ToIntQ(data.ElementAt(23)); //三大法人買賣合計
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;

                        tmpUpdateList.Add(existItem);
                        
                    }
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrade_hedge_weekly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_3itrade_hedge_weekly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "W"; //weekly
            string stockSelectType = "EW";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l=zh-tw&se=EW&t=W&d=109/01/04&_=1578809981470
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l={0}&se={1}&t={2}&d={3}&_={4}",
                lang, stockSelectType, dataType, rocDate, paramUnderLine);

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
                rnd = r.Next(7000);
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
