using HtmlAgilityPack;
using Newtonsoft.Json;
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
using TwStockGrabBLL.Logic.Rsp.Json.Desk;

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
            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(dataDate);

            DT13Sa150Otc_Rsp rsp = JsonConvert.DeserializeObject<DT13Sa150Otc_Rsp>(responseContent);

            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                WriteEndRecord(record);
                Sleep();
            }
        }

        private void SaveToDatabase(DT13Sa150Otc_Rsp rsp, DateTime dataDate)
        {            
            List<d_t13sa150_otc> tmpAddList = new List<d_t13sa150_otc>();
            List<d_t13sa150_otc> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_t13sa150_otc>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_t13sa150_otc existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_t13sa150_otc
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        issue_cnt = ToLongQ(data.ElementAt(2).Trim()), //發行股數
                        remain_investable = ToLongQ(data.ElementAt(3).Trim()), //外資及陸資尚可投資股數
                        total_hold_cnt = ToLongQ(data.ElementAt(4).Trim()), //全體外資及陸資持有股數
                        remain_invest_percent = ToDecimalQ(data.ElementAt(5).Trim()), //外資及陸資尚可投資比率
                        total_hold_percent = ToDecimalQ(data.ElementAt(6).Trim()), //全體外資及陸資持股比率
                        invest_ceil_limit = ToDecimalQ(data.ElementAt(7).Trim()), //外資及陸資共用法令投資上限比率
                        china_invest_ceil_limit = ToDecimalQ(data.ElementAt(8)), //陸資法令投資上限比率
                        change_reason = data.ElementAt(9).Trim(), //與前日異動原因
                        last_comp_change = GetDateFromRocSlashStringQ(data.ElementAt(10)),//最近一次上櫃公司申報外資持股異動日期                        
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_t13sa150_otc.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

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
            HtmlNodeCollection tableNodes = doc.DocumentNode.SelectNodes("//table[@border='1']");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");

            string reportTitle = "";
            string reportDate = "";
            int count = 0;

            bool titleSet = false;
            bool tdDataRow = false;

            if (tableNodes != null && tableNodes.Count() > 0)
            {
                foreach (var tableNode in tableNodes)
                {
                    sb.AppendFormat("'aaData':[")
                        .AppendLine();

                    foreach (var child in tableNode.ChildNodes)
                    {
                        string c = child.Name;
                        string a = child.InnerHtml;
                        string b = child.InnerText;
                        if (child.Name.Trim().ToLower() == "tr")
                        {
                            if (child.ChildNodes.Where(x => x.Name.Trim().ToLower() == "td").Any())
                            {
                                sb.AppendFormat("[")
                                    .AppendLine();
                            }

                            foreach (var thOrTdNodes in child.ChildNodes)
                            {
                                if (thOrTdNodes.Name.Trim().ToLower() == "th")
                                {
                                    if (titleSet == false)
                                    {
                                        reportTitle = thOrTdNodes.InnerText;
                                        string[] titleParts = reportTitle.Split('　');
                                        reportDate = titleParts[0].Trim();
                                        titleSet = true;
                                    }
                                    tdDataRow = false;
                                }

                                if (thOrTdNodes.Name.Trim().ToLower() == "td")
                                {
                                    string tdText = thOrTdNodes.InnerText.Trim();

                                    if (tdText.Contains("&nbsp;"))
                                    {
                                        tdText = tdText.Replace("&nbsp;", "");
                                    }

                                    sb.AppendFormat("'{0}',", tdText)
                                        .AppendLine();

                                    tdDataRow = true;
                                }
                            }

                            if (tdDataRow)
                            {
                                //sb.Remove(sb.Length - 1, 1);
                                sb.AppendFormat("],")
                                    .AppendLine();

                                count += 1;
                            }
                            tdDataRow = false;
                        }

                    }

                    sb.AppendFormat("],")
                        .AppendLine();
                }
            }
            else
            {
                sb.AppendFormat("'aaData':[[]],").AppendLine();
            }

            sb.AppendFormat("'reportTitle':\"{0}\",", reportTitle)
                .AppendLine();
            sb.AppendFormat("'reportDate':\"{0}\",", reportDate)
                .AppendLine();
            sb.AppendFormat("'iTotalRecords':{0}", count)
                .AppendLine();


            sb.AppendLine("}");

            return sb.ToString();
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
            request.Host = "mops.twse.com.tw";
            request.Referer = "https://mops.twse.com.tw/server-java/t13sa150_otc?&step=1";
            request.Headers.Add("Origin", "https://mops.twse.com.tw");
            request.KeepAlive = true;
            request.Timeout = 1000 * 60 * 30;
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
            using (StreamReader sr = new StreamReader(inputResponseStream, Encoding.Default))
            {
                responseContent = sr.ReadToEnd();
            }

            return responseContent;
        }
    }
}
