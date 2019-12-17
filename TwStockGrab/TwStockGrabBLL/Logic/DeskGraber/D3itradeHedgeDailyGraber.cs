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
    /// <summary>
    /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣明細資訊
    /// d_3itrade_hedge_daily
    /// 本資訊自民國96年4月20日起開始提供，實際上由103年12月1日起才有   
    /// 
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge.php?l=zh-tw
    /// </summary>
    public class D3itradeHedgeDailyGraber
    {
        public void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            D3itradeHedgeDaily_Rsp rsp = JsonConvert.DeserializeObject<D3itradeHedgeDaily_Rsp>(responseContent);
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

        private void SaveToDatabase(D3itradeHedgeDaily_Rsp rsp, DateTime dataDate)
        {
            DateTime period1Start = new DateTime(2018, 1, 15);
            List<d_3itrade_hedge_daily> tmpAddList = new List<d_3itrade_hedge_daily>();
            List<d_3itrade_hedge_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrade_hedge_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_3itrade_hedge_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    if (dataDate < period1Start)
                    {
                        tmpAddList.Add(new d_3itrade_hedge_daily
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
                        tmpAddList.Add(new d_3itrade_hedge_daily
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
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrade_hedge_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily
            string stockSelectType = "EW";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l=zh-tw&se=EW&t=D&d=108/11/06&_=1573291033995
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
    }
}
