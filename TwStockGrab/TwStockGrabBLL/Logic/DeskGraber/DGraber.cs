using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    public abstract class DGraber
    {
        protected int _graberFrequency { get; set; }
        protected string _graberClassName { get; set; }

        public DGraber()
        {
            
        }            

        /// <summary>
        /// 執行抓資料跟存資料的工作
        /// </summary>
        /// <param name="grabeDate">執行抓取資料的日期</param>
        public abstract void DoJob(DateTime grabeDate);

        protected bool GetOrCreateWorkRecord(DateTime dataDate, out work_record record)
        {
            bool complete = false;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                record = context.Set<work_record>().Where(x => x.graber_freq == this._graberFrequency && x.graber_name == this._graberClassName && x.work_data_date == dataDate).FirstOrDefault();
                if (record == null)
                {
                    record = new work_record
                    {
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        is_complete = false,

                        graber_freq = this._graberFrequency,
                        graber_name = this._graberClassName,
                        note = "",
                        start_time = DateTime.Now,
                        work_data_date = dataDate,
                    };
                    context.Set<work_record>().Add(record);

                    context.SaveChanges();

                    complete = false;
                }
                else
                {
                    complete = record.is_complete.Value;
                }
            }
            return complete;
        }

        protected void WriteEndRecord(work_record record)
        {
            record.update_at = DateTime.Now;
            record.end_time = DateTime.Now;
            record.is_complete = true;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.Entry<work_record>(record).State = System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }
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
        protected string GetTimeStamp()
        {
            return DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 休息幾秒鐘後，再繼續抓資料，防止IP被ban掉
        /// </summary>
        protected void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(6500);
            } while (rnd < 3500);
            Thread.Sleep(rnd);
        }

        protected string ParseADDateToRocString(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            return string.Format("{0}/{1}/{2}",
                (year - 1911).ToString(),
                month.ToString().PadLeft(2, '0'),
                day.ToString().PadLeft(2, '0'));
        }

        protected decimal? ToDecimalQ(string data)
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

            if (data.Trim() == "N/A")
            {
                return null;
            }

            string noPercentSymbolSring = data.Replace("%", "");

            string noCommaString = noPercentSymbolSring.Replace(",", "");

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

        protected decimal? ToSignDecimalQ(string data)
        {
            if (data.StartsWith("+"))
            {
                return ToDecimalQ(data.TrimStart('+'));
            }
            if (data.StartsWith("-"))
            {
                return (-1) * ToDecimalQ(data.TrimStart('-'));
            }


            return ToDecimalQ(data);
        }

        /// <summary>
        /// 把可能是Int的字串轉成Int?
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
            data = data.Replace("(", "");
            data = data.Replace(")", "");
            return Convert.ToInt32(data);
        }

        /// <summary>
        /// 把數字字串轉成數字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int ToInt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 0;
            }

            data = data.Replace(",", "").Trim();
            return Convert.ToInt32(data);
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

        protected DateTime? ToTimeQ(DateTime today, string timeStr)
        {
            if (string.IsNullOrEmpty(timeStr))
            {
                return null;
            }

            string[] timeParts = timeStr.Split(':');
            if (timeParts.Count() != 2)
            {
                return null;
            }

            DateTime time = today.AddHours(Convert.ToInt32(timeParts[0])).AddMinutes(Convert.ToInt32(timeParts[1]));
            return time;
        }

        /// <summary>
        /// 把格式為 yyyMM 或是 yyMM 的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyyMM或yyMM民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateMonthFromRocStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string rocYear = "";
            string rocMonth = "";


            if (rocString.Length == 4)
            {
                rocYear = rocString.Substring(0, 2);
                rocMonth = rocString.Substring(2, 2);
            }
            else
            {
                rocYear = rocString.Substring(0, 3);
                rocMonth = rocString.Substring(3, 2);
            }


            int year = 1911 + Convert.ToInt32(rocYear);
            int month = Convert.ToInt32(rocMonth);

            return new DateTime(year, month, 1);
        }

        ///// <summary>
        ///// 把格式為 yyy 或是 yy 的民國年字串轉為DateTime物件
        ///// </summary>
        ///// <param name="rocString">yyy或yy民國年日期字串</param>
        ///// <returns></returns>
        //protected DateTime? GetDateYearFromRocStringQ(string rocString)
        //{
        //    if (string.IsNullOrEmpty(rocString))
        //    {
        //        return null;
        //    }

        //    int year = 1911 + Convert.ToInt32(rocString);

        //    return new DateTime(year, 1, 1);
        //}

        /// <summary>
        /// 把格式為 yyy/MM/dd 或是 yy/MM/dd 的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyy/MM/dd或yy/MM/dd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocSlashStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string[] dateParts = rocString.Split('/');

            if (dateParts.Length != 3)
            {
                return null;
            }

            int year = 1911 + Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 把格式為 yyyMMdd 或是 yyMMdd 的民國年字串轉為DateTime物件
        /// </summary>
        /// <param name="rocString">yyyMMdd或yyMMdd民國年日期字串</param>
        /// <returns></returns>
        protected DateTime? GetDateFromRocStringQ(string rocString)
        {
            if (string.IsNullOrEmpty(rocString))
            {
                return null;
            }

            string rocYear = "";
            string rocMonth = "";
            string rocDay = "";

            if (rocString.Length == 6)
            {
                rocYear = rocString.Substring(0, 2);
                rocMonth = rocString.Substring(2, 2);
                rocDay = rocString.Substring(4, 2);
            }
            else
            {
                rocYear = rocString.Substring(0, 3);
                rocMonth = rocString.Substring(3, 2);
                rocDay = rocString.Substring(5, 2);
            }


            int year = 1911 + Convert.ToInt32(rocYear);
            int month = Convert.ToInt32(rocMonth);
            int day = Convert.ToInt32(rocDay);

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 根據日期計算當月第一天的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DateTime GetMonthFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// 根據傳入的日期，計算該週星期一的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DateTime GetWeekMondayDate(DateTime dt)
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

        /// <summary>
        /// 計算當年的1月1日的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DateTime GetYearFirstDay(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        protected void ParseFloorAndCeil(string srcData, ref decimal floor, ref decimal ceil)
        {
            if (string.IsNullOrEmpty(srcData))
            {
                return;
            }

            string[] dataParts = srcData.Split('~');
            floor = decimal.Parse(dataParts[0].Trim());
            ceil = decimal.Parse(dataParts[1].Trim());
        }


        protected short TransBuySellType(string t)
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
    }
}
