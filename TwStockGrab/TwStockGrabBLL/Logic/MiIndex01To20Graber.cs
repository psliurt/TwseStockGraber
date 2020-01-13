using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic.Rsp.Json;
using Newtonsoft.Json;
using TwStockGrabBLL.DAL;
using System.Threading;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->盤後資訊->每日收盤行情 水泥工業 到其他  這個類別會由 MiIndexGraber取代
    /// </summary>
    public class MiIndex01To20Graber
    {
        //public MiIndex01To20Graber() : base()
        //{
        //    this._graberClassName = typeof(MiIndex01To20Graber).Name;
        //    this._graberFrequency = 1;
        //}

        public void DoJob(DateTime dataDate)
        {
            List<string> typeList = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                typeList.Add(i.ToString().PadLeft(2, '0'));
            }

            //01 : 水泥工業
            //02 : 食品工業
            //03 : 塑膠工業
            //04 : 紡織纖維
            //05 : 電機機械
            //dataDate = new DateTime(2019, 7, 5);
            foreach(string type in typeList)
            { 
                if (type == "19")
                {
                    continue;
                }
                
                string responseContent = GetWebContent(dataDate, type);
                MI_INDEX_01_20_Rsp rsp = JsonConvert.DeserializeObject<MI_INDEX_01_20_Rsp>(responseContent);
                SaveToDatabase(rsp, type, dataDate);
                Sleep();
                
            }

            
        }

        private void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(5000);
            } while (rnd < 2000);            
            Thread.Sleep(rnd );
        }

        private void SaveToDatabase(MI_INDEX_01_20_Rsp rsp, string type, DateTime dataDate)
        {
            using (TwStockDataContext context = new TwStockDataContext())
            {
                foreach (var data in rsp.data1)
                {
                    context.Set<mi_index>().Add(new mi_index
                    {
                        data_date = dataDate,
                        stock_no = data.ElementAt(0),
                        stock_name = data.ElementAt(1),
                        deal_stock_num = ToInt(data.ElementAt(2)),
                        deal_trade_num = ToInt(data.ElementAt(3)),
                        deal_money = ToDecimal(data.ElementAt(4)),
                        open_price = ToDecimal(data.ElementAt(5)),
                        high_price = ToDecimal(data.ElementAt(6)),
                        low_price = ToDecimal(data.ElementAt(7)),
                        close_price = ToDecimal(data.ElementAt(8)),
                        up_down = ToByte(data.ElementAt(9)),
                        up_down_price = ToDecimal(data.ElementAt(10)),
                        last_show_buy_price = ToDecimal(data.ElementAt(11)),
                        last_show_buy_qty = ToInt(data.ElementAt(12)),
                        last_show_sell_price = ToDecimal(data.ElementAt(13)),
                        last_show_sell_qty = ToInt(data.ElementAt(14)),
                        eps = ToDecimal(data.ElementAt(15)),
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                        title = string.Format("{0}-{1}", rsp.title, type)
                    });


                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetDateString(date);
            string paramType = type;
            string paramUnderLine = GetTimeStamp();



            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_INDEX?response={0}&date={1}&type={2}&_={3}",
                paramResponse, paramDate, paramType, paramUnderLine);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

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

        private string GetDateString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }

        private decimal? ToDecimal(string data)
        {
            if (string.IsNullOrEmpty(data))
            { return null; }
            if (data == "--")
            {
                return null;
            }
            return Convert.ToDecimal(data);
        }

        private int? ToInt(string data)
        {
            data = data.Replace(",", "");
            return Convert.ToInt32(data);
        }

        private sbyte? ToByte(string data)
        {
            if (data.Contains("+"))
            {
                return 1;
            }
            if (data.Contains("-"))
            {
                return -1;
            }
            return 0;

        }
    }
}
