using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    /// <summary>
    /// 首頁 > 上櫃 > 三大法人 > 外資及陸資投資持股統計
    /// d_t13sa150_otc
    /// 
    /// 網頁位置
    /// https://mops.twse.com.tw/server-java/t13sa150_otc?step=0
    /// </summary>
    public class DT13Sa150OtcGraber : DGraber
    {
        public DT13Sa150OtcGraber() : base()
        {
            this._graberClassName = typeof(DT13Sa150OtcGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            GetWebContent(dataDate);
        }

        //private void SaveToDatabase(D3itradeHedgeDaily_Rsp rsp, DateTime dataDate)
        //{
        //    DateTime period1Start = new DateTime(2018, 1, 15);
        //    List<d_3itrade_hedge_daily> tmpAddList = new List<d_3itrade_hedge_daily>();
        //    List<d_3itrade_hedge_daily> tmpDataList = null;
        //    using (TwStockDataContext context = new TwStockDataContext())
        //    {
        //        tmpDataList = context.Set<d_3itrade_hedge_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
        //    }

        //    foreach (var data in rsp.aaData)
        //    {
        //        string stockNo = data.ElementAt(0).Trim();

        //        d_3itrade_hedge_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
        //        if (existItem == null)
        //        {
        //            if (dataDate < period1Start)
        //            {
        //                tmpAddList.Add(new d_3itrade_hedge_daily
        //                {
        //                    data_date = dataDate,
        //                    stock_no = stockNo,
        //                    stock_name = data.ElementAt(1).Trim(),
        //                    foreign_all_buy_in = ToIntQ(data.ElementAt(2)), //外資及陸資
        //                    foreign_all_sell_out = ToIntQ(data.ElementAt(3)),
        //                    foreign_all_diff = ToIntQ(data.ElementAt(4)),
        //                    invest_buy_in = ToIntQ(data.ElementAt(5)), //投信
        //                    invest_sell_out = ToIntQ(data.ElementAt(6)),
        //                    invest_diff = ToIntQ(data.ElementAt(7)),
        //                    dealer_all_diff = ToIntQ(data.ElementAt(8)), //自營商
        //                    dealer_self_buy_in = ToIntQ(data.ElementAt(9)),//自營商(自行買賣)
        //                    dealer_self_sell_out = ToIntQ(data.ElementAt(10)),
        //                    dealer_self_diff = ToIntQ(data.ElementAt(11)),
        //                    dealer_risk_buy_in = ToIntQ(data.ElementAt(12)),//自營商(避險)
        //                    dealer_risk_sell_out = ToIntQ(data.ElementAt(13)),
        //                    dealer_risk_diff = ToIntQ(data.ElementAt(14)),
        //                    total_diff = ToIntQ(data.ElementAt(15)),//三大法人買賣合計                            
        //                    title = rsp.reportTitle.Trim(),
        //                    create_at = DateTime.Now,
        //                    update_at = DateTime.Now
        //                });
        //            }
        //            else
        //            {
        //                tmpAddList.Add(new d_3itrade_hedge_daily
        //                {
        //                    data_date = dataDate,
        //                    stock_no = stockNo,
        //                    stock_name = data.ElementAt(1).Trim(),
        //                    foreign_buy_in = ToIntQ(data.ElementAt(2)), //外資不含外資自營商
        //                    foreign_sell_out = ToIntQ(data.ElementAt(3)),
        //                    foreign_diff = ToIntQ(data.ElementAt(4)),
        //                    foreign_self_buy_in = ToIntQ(data.ElementAt(5)), //外資自營商
        //                    foreign_self_sell_out = ToIntQ(data.ElementAt(6)),
        //                    foreign_self_diff = ToIntQ(data.ElementAt(7)),
        //                    foreign_all_buy_in = ToIntQ(data.ElementAt(8)), //外資及陸資
        //                    foreign_all_sell_out = ToIntQ(data.ElementAt(9)),
        //                    foreign_all_diff = ToIntQ(data.ElementAt(10)),
        //                    invest_buy_in = ToIntQ(data.ElementAt(11)), //投信
        //                    invest_sell_out = ToIntQ(data.ElementAt(12)),
        //                    invest_diff = ToIntQ(data.ElementAt(13)),
        //                    dealer_self_buy_in = ToIntQ(data.ElementAt(14)), //自營商(自行買賣)
        //                    dealer_self_sell_out = ToIntQ(data.ElementAt(15)),
        //                    dealer_self_diff = ToIntQ(data.ElementAt(16)),
        //                    dealer_risk_buy_in = ToIntQ(data.ElementAt(17)),//自營商(避險)
        //                    dealer_risk_sell_out = ToIntQ(data.ElementAt(18)),
        //                    dealer_risk_diff = ToIntQ(data.ElementAt(19)),
        //                    dealer_all_buy_in = ToIntQ(data.ElementAt(20)), //自營商
        //                    dealer_all_sell_out = ToIntQ(data.ElementAt(21)),
        //                    dealer_all_diff = ToIntQ(data.ElementAt(22)),
        //                    total_diff = ToIntQ(data.ElementAt(23)), //三大法人買賣合計
        //                    title = rsp.reportTitle.Trim(),
        //                    create_at = DateTime.Now,
        //                    update_at = DateTime.Now
        //                });
        //            }
        //        }
        //    }

        //    using (TwStockDataContext context = new TwStockDataContext())
        //    {
        //        context.d_3itrade_hedge_daily.AddRange(tmpAddList);

        //        context.SaveChanges();
        //    }
        //}

        private string GetWebContent(DateTime date)
        {
            ///https://mops.twse.com.tw/server-java/t13sa150_otc
            string url = string.Format("https://mops.twse.com.tw/server-java/t13sa150_otc");

            Dictionary<string, string> paramVals = new Dictionary<string, string>();
            paramVals.Add("years", date.Year.ToString());
            paramVals.Add("months", date.Month.ToString().PadLeft(2, '0'));
            paramVals.Add("days", date.Day.ToString().PadLeft(2, '0'));
            paramVals.Add("bcode", "");
            paramVals.Add("step", "2");


            string response = PostHttpResponse(url, paramVals);
            //要先把html response裡面的<center>拿掉，因為這不符合xhtml規定
            string clearText = Regex.Replace(response, "<center>", "", RegexOptions.IgnoreCase);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            

            return null;
        }

        /// <summary>
        /// 送出http POST 請求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string PostHttpResponse(string url, Dictionary<string, string> paramVals)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //要把資料寫出去

            StringBuilder sb = new StringBuilder();
            foreach (var kv in paramVals)
            {
                sb.AppendFormat("{0}={1}&", kv.Key, kv.Value);
            }
            sb.Remove(sb.Length - 1, 1);

            Stream outStream = null;
            outStream = request.GetRequestStream();
            byte[] outData = Encoding.UTF8.GetBytes(sb.ToString());
            outStream.Write(outData, 0, outData.Length);
            

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
    }
}
