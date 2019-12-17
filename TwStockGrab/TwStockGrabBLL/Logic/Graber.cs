using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 每個Graber最好可以標明以下三個資訊
    /// 1. 證交所的資料點選的步驟
    /// 2. 影響的資料表名稱
    /// 3. 網頁上寫明的資料提供的起始日期資訊
    /// *可參考MiIndexTop20Graber的註解寫法
    /// </summary>
    public abstract class Graber
    {
        public Graber()
        {

        }

        /// <summary>
        /// 執行抓資料跟存資料的工作
        /// </summary>
        /// <param name="grabeDate">執行抓取資料的日期</param>
        public abstract void DoJob(DateTime grabeDate);
        
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
        protected string GetTimeStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 轉為Decimal?型態
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected decimal? ToDecimalQ(string data)
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

            return Convert.ToDecimal(data);
        }

        /// <summary>
        /// 把資料轉為Int
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int? ToIntQ(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            data = data.Replace(",", "");
            return Convert.ToInt32(data);
        }

        protected sbyte? SignToByteQ(string data)
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

        /// <summary>
        /// 把資料轉為Long型態
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected long? ToLongQ(string data)
        {
            data = data.Replace(",", "");
            return Convert.ToInt64(data);
        }

        protected void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(5000);
            } while (rnd < 2500);
            Thread.Sleep(rnd);
        }

        /// <summary>
        /// 把格式為 yyy/MM/dd的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyy.MM.dd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocPointStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string[] dateParts = rocString.Split('.');
            if (dateParts.Count() != 3)
            {
                return null;
            }
            int year = 1911 + Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 把格式為 yyy/MM/dd的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyy/MM/dd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocSlashStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string[] dateParts = rocString.Split('/');

            if (dateParts.Count() != 3)
            {
                return null;
            }

            int year = 1911 + Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 把yyyyMMdd字串轉成DateTime物件
        /// </summary>
        /// <param name="adDateString">yyyyMMdd的日期字串</param>
        /// <returns></returns>
        protected DateTime GetDateFromAdDateString(string adDateString)
        {
            int year = Convert.ToInt32(adDateString.Substring(0, 4));
            int month = Convert.ToInt32(adDateString.Substring(4, 2));
            int day = Convert.ToInt32(adDateString.Substring(6, 2));

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 把格式為 yyyy/MM/dd 的西元年日期字串轉為DateTime物件
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        protected DateTime? GetDateFromAdSlashDateString(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            string[] dateParts = dateString.Split('/');

            if (dateParts.Count() != 3)
            {
                return null;
            }

            return new DateTime(Convert.ToInt32(dateParts[0]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[2]));
        }

        /// <summary>
        /// 取得格式為yyyyMMdd的西元年日期字串
        /// </summary>
        /// <param name="dt">時間</param>
        /// <returns></returns>
        protected string GetyyyyMMddDateString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 根據dataString內是不是有星型符號來決定是否轉為true
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        protected bool StarSignToBool(string dataString)
        {
            if (string.IsNullOrEmpty(dataString))
            {
                return false;
            }

            if (string.IsNullOrEmpty(dataString.Trim()))
            {
                return false;
            }

            if (dataString.Trim().Contains('*'))
            {
                return true;
            }

            return false;
        }

        protected decimal ToUpDownDecimal(string sign, string price)
        {
            if (sign.Contains(">+<"))
            {
                return Convert.ToDecimal(price);
            }
            else if (sign.Contains(">-<"))
            {
                return Convert.ToDecimal(price) * (-1);
            }
            else
            {
                if (string.IsNullOrEmpty(price))
                {
                    return 0;
                }
                else if (price.Trim() == "--")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(price);
                }
                
            }
        }

        protected DateTime AddTimeToDateFromTimeString(DateTime date, string timeString)
        {
            string[] timeParts = timeString.Trim().Split(':');
            return date.AddHours(Convert.ToInt32(timeParts[0]))
                       .AddMinutes(Convert.ToInt32(timeParts[1]))
                       .AddSeconds(Convert.ToInt32(timeParts[2]));
        }

        /// <summary>
        /// 由今天的日期計算這一周的星期一的日期，並轉為yyyyMMdd的日期格式
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        protected string GetMondayAdDateString(DateTime today)
        {
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                DateTime monday = today.AddDays(-6);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                DateTime monday = today.AddDays(-5);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                DateTime monday = today.AddDays(-4);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                DateTime monday = today.AddDays(-3);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                DateTime monday = today.AddDays(-2);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                DateTime monday = today.AddDays(-1);
                return monday.ToString("yyyyMMdd");
            }

            return today.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 用今天的日期計算這一周的起訖日期
        /// </summary>
        /// <param name="today"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void CountWeekStartAndEndDate(DateTime today, out DateTime start, out DateTime end)
        {
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                start = today.AddDays(-6);
                end = today.AddDays(-2);
            }
            else if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                start = today.AddDays(-5);
                end = today.AddDays(-1);
            }
            else if (today.DayOfWeek == DayOfWeek.Friday)
            {
                start = today.AddDays(-4);
                end = today;
            }
            else if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                start = today.AddDays(-3);
                end = today.AddDays(1);
            }
            else if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                start = today.AddDays(-2);
                end = today.AddDays(2);
            }
            else if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                start = today.AddDays(-1);
                end = today.AddDays(3);
            }
            else
            {
                start = today;
                end = today.AddDays(4);
            }
        }

        /// <summary>
        /// 根據今天日期取得這個月第一天的日期
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        protected DateTime GetMonthFirstDate(DateTime today)
        {
            return new DateTime(today.Year, today.Month, 1);
        }

        /// <summary>
        /// 取得民國年日期(yyy/QQ)中的 年 並轉為 西元年數字
        /// </summary>
        /// <param name="dataString">yyy/QQ</param>
        /// <returns></returns>
        protected int GetAdYearFromRocYearSeasonDataString(string dataString)
        {
            string[] datas = dataString.Split('/');
            if (datas.Count() != 2)
            {
                return 0;
            }
            return 1911 + Convert.ToInt32(datas[0]);
        }
        
        /// <summary>
        /// 取得民國年與季數(yyy/QQ)中的 季數 並轉為 數字
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        protected int GetSeasonNoFromRocYearSeasonDataString(string dataString)
        {
            string[] datas = dataString.Split('/');
            if (datas.Count() != 2)
            {
                return 0;
            }
            return Convert.ToInt32(datas[1]);
        }

        /// <summary>
        /// 從ROC加中文字年的字串中取得西元年數字
        /// ex. 98年->2009
        /// </summary>
        /// <param name="yearString">ex.100年</param>
        /// <returns></returns>
        protected int? GetAdYearFromRocYearWordString(string yearString)
        {
            for (int i = 0; i < yearString.Length; i++)
            {
                if (char.IsNumber(yearString, i) == false)
                {
                    if (i == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return Convert.ToInt32(yearString.Substring(0, i)) + 1911;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 從民國年月的字串中取得月份數字
        /// ex. 108/01 -> 取得1月
        /// </summary>
        /// <param name="dateString">yyy/MM ex.108/02</param>
        /// <returns></returns>
        protected int? GetMonthFromRocYearMonthString(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            string[] dateParts = dateString.Split('/');
            if (dateParts.Count() != 2)
            {
                return null;
            }
            return Convert.ToInt32(dateParts[1]);
        }

        /// <summary>
        /// 把select_type轉為DB裡面要看的StockType
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        protected int SelectTypeToDbStockType(string typeStr)
        {
            switch (typeStr.Trim())
            {
                case "0049":       //封閉式基金
                    return 16;
                case "0099P":       //ETF
                    return 11;
                case "019919T":       //受益證券
                    return 12;
                case "0999GA":       //附認股權特別股
                    return 13;
                case "0999GD":       //附認股權公司債
                    return 30;
                case "0999G9":       //認股權憑證
                    return 14;
                case "0999":       //認購權證 (國內標的)                    
                case "0999F":       //認購權證 (外國標的)
                    return 21;
                case "0999P":       //認售權證 (國內標的)                    
                case "0999Q":       //認售權證 (外國標的)	
                    return 22;
                case "CB":       //可轉換公司債
                    return 31;
                case "0999C":    //牛證                    
                case "0999X":    //可展延牛證
                    return 23;
                case "0999B":    //熊證                    
                case "0999Y":    //可展延熊證
                    return 24;
                case "9299":     //存託憑證
                    return 15;
                case "01":       //水泥工業
                case "02":       //食品工業
                case "03":       //塑膠工業
                case "04":       //紡織纖維
                case "05":       //電機機械
                case "06":       //電器電纜
                case "07":       //化學生技醫療
                case "21":       //化學工業
                case "22":       //生技醫療業
                case "08":       //玻璃陶瓷
                case "09":       //造紙工業
                case "10":       //鋼鐵工業
                case "11":       //橡膠工業
                case "12":       //汽車工業
                case "13":       //電子工業
                case "24":       //半導體業
                case "25":       //電腦及週邊設備業
                case "26":       //光電業
                case "27":       //通信網路業
                case "28":       //電子零組件業
                case "29":       //電子通路業
                case "30":       //資訊服務業
                case "31":       //其他電子業
                case "14":       //建材營造
                case "15":       //航運業
                case "16":       //觀光事業
                case "17":       //金融保險
                case "18":       //貿易百貨
                case "23":       //油電燃氣業
                case "19":       //綜合
                case "20":       //其他
                    return 10;
                case "ALL":
                case "All":
                    return 0; //代表尚未指定
                default:
                    return 40; //其他
            }
        }

        /// <summary>
        /// 把帶有正負符號的數字轉為decimal型態
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected decimal? ToDecimalFromSignDataString(string data)
        {
            if (data.StartsWith("-"))
            {
                return Convert.ToDecimal(data);
            }
            else if (data.StartsWith("+"))
            {
                return Convert.ToDecimal(data.Substring(1));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得西元年yyyyMMdd字串，並自動判斷輸入的日期是否為工作日
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        protected string GetWorkAdDateString(DateTime today)
        {
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                DateTime friday = today.AddDays(-1);
                return friday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                DateTime friday = today.AddDays(-2);
                return friday.ToString("yyyyMMdd");
            }
            return today.ToString("yyyyMMdd");
        }

        protected string GetWeekMondayAdDateString(DateTime today)
        {
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                DateTime monday = today.AddDays(-6);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                DateTime monday = today.AddDays(-5);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                DateTime monday = today.AddDays(-4);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                DateTime monday = today.AddDays(-3);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                DateTime monday = today.AddDays(-2);
                return monday.ToString("yyyyMMdd");
            }

            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                DateTime monday = today.AddDays(-1);
                return monday.ToString("yyyyMMdd");
            }

            return today.ToString("yyyyMMdd");
        }

        protected void GetWeekStartAndEnd(DateTime today, out DateTime start, out DateTime end)
        {
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                start = today.AddDays(-6);
                end = today.AddDays(-2);
            }
            else if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                start = today.AddDays(-5);
                end = today.AddDays(-1);
            }
            else if (today.DayOfWeek == DayOfWeek.Friday)
            {
                start = today.AddDays(-4);
                end = today;
            }
            else if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                start = today.AddDays(-3);
                end = today.AddDays(1);
            }
            else if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                start = today.AddDays(-2);
                end = today.AddDays(2);
            }
            else if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                start = today.AddDays(-1);
                end = today.AddDays(3);
            }
            else
            {
                start = today;
                end = today.AddDays(4);
            }
        }

        protected DateTime? GetDateFromYearAndDateStringQ(int year, string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            string[] dateParts = dateString.Split('/');
            if (dateParts.Count() != 2)
            {
                return null;
            }

            return new DateTime(year,
                Convert.ToInt32(dateParts[0]),
                Convert.ToInt32(dateParts[1]));
        }
    }
}
