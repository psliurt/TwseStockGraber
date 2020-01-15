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
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->三大法人->三大法人買賣金額統計
    /// bfi82u_daily
    /// bfi82u_week
    /// bfi82u_month
    /// 本資訊自民國93年4月7日起提供
    /// TODO:這個抓取資料的類別，有三個不同的資料頻率 day, week, month
    /// </summary>
    public class Bfi82uGraber : Graber
    {
        public Bfi82uGraber() : base()
        {
            this._graberClassName = typeof(Bfi82uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {

            List<string> typeStringList = new List<string>();
            typeStringList.Add("day");
            typeStringList.Add("week");
            typeStringList.Add("month");

            foreach (string type in typeStringList)
            {
                work_record record = null;
                if (GetOrCreateWorkRecord(dataDate, type, out record) == false)
                {
                    string responseContent = GetWebContent(dataDate, type);
                    BFI82U_Rsp rsp = JsonConvert.DeserializeObject<BFI82U_Rsp>(responseContent);

                    if (rsp.data == null)
                    {
                        WriteEndRecord(record);
                        Sleep();
                    }
                    else
                    {
                        SaveToDatabase(rsp, dataDate, type);
                        WriteEndRecord(record);
                        Sleep();
                    }
                }                                
            }            
        }

        

        private void SaveToDatabase(BFI82U_Rsp rsp, DateTime dataDate, string type)
        {
            if (dataDate.DayOfWeek == DayOfWeek.Saturday || dataDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return;
            }

            if (type == "day")
            {
                List<bfi82u_daily> tmpAddDailyList = new List<bfi82u_daily>();
                List<bfi82u_daily> tmpDataDailyList = null;

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    tmpDataDailyList = context.Set<bfi82u_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                }

                foreach (var data in rsp.data)
                {
                    string unitName = data.ElementAt(0).Trim();
                    bfi82u_daily obj = tmpDataDailyList.Where(x => x.unit_name == unitName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddDailyList.Add(new bfi82u_daily
                        {
                            data_date = dataDate,
                            unit_name = unitName,
                            buy_money = ToDecimalQ(data.ElementAt(1)),
                            sell_money = ToDecimalQ(data.ElementAt(2)),
                            money_diff = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }                    
                }

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    context.bfi82u_daily.AddRange(tmpAddDailyList);

                    context.SaveChanges();
                }
            }
            else if (type == "week")
            {
                DateTime start;
                DateTime end;
                GetWeekStartAndEnd(dataDate, out start, out end);

                List<bfi82u_week> tmpAddWeekList = new List<bfi82u_week>();
                List<bfi82u_week> tmpUpdateWeekList = new List<bfi82u_week>();
                List<bfi82u_week> tmpDataWeekList = null;

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    tmpDataWeekList = context.Set<bfi82u_week>().Where(x => x.week_start == start && x.week_end == end).ToList();
                }

                foreach (var data in rsp.data)
                {
                    string unitName = data.ElementAt(0).Trim();

                    var obj =
                        tmpDataWeekList.Where(x => x.week_start == start && x.week_end == end && x.unit_name == unitName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddWeekList.Add(new bfi82u_week
                        {
                            week_start = start,
                            week_end = end,
                            last_update = dataDate,
                            unit_name = unitName,
                            buy_money = ToDecimalQ(data.ElementAt(1)),
                            sell_money = ToDecimalQ(data.ElementAt(2)),
                            money_diff = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                    else
                    {
                        obj.last_update = dataDate;
                        obj.buy_money = ToDecimalQ(data.ElementAt(1));
                        obj.sell_money = ToDecimalQ(data.ElementAt(2));
                        obj.money_diff = ToDecimalQ(data.ElementAt(3));
                        obj.update_at = DateTime.Now;

                        tmpUpdateWeekList.Add(obj);
                    }
                }

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    context.bfi82u_week.AddRange(tmpAddWeekList);
                    foreach (var item in tmpUpdateWeekList)
                    {
                        context.Entry<bfi82u_week>(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    context.SaveChanges();
                }
            }
            else if (type == "month")
            {
                int year = dataDate.Year;
                int month = dataDate.Month;

                List<bfi82u_month> tmpAddMonthList = new List<bfi82u_month>();
                List<bfi82u_month> tmpUpdateMonthList = new List<bfi82u_month>();
                List<bfi82u_month> tmpDataMonthList = null;

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    tmpDataMonthList = context.Set<bfi82u_month>().Where(x => x.data_year == year && x.data_month == month).ToList();
                }

                foreach (var data in rsp.data)
                {
                    string unitName = data.ElementAt(0).Trim();                    
                    var obj =
                        tmpDataMonthList.Where(x => x.data_year == year && x.data_month == month && x.unit_name == unitName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddMonthList.Add(new bfi82u_month
                        {
                            data_year = dataDate.Year,
                            data_month = dataDate.Month,
                            last_update = dataDate,
                            unit_name = data.ElementAt(0),
                            buy_money = ToDecimalQ(data.ElementAt(1)),
                            sell_money = ToDecimalQ(data.ElementAt(2)),
                            money_diff = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                    else
                    {
                        obj.last_update = dataDate;
                        obj.buy_money = ToDecimalQ(data.ElementAt(1));
                        obj.sell_money = ToDecimalQ(data.ElementAt(2));
                        obj.money_diff = ToDecimalQ(data.ElementAt(3));
                        obj.update_at = DateTime.Now;

                        tmpUpdateMonthList.Add(obj);
                    }


                }


                using (TwStockDataContext context = new TwStockDataContext())
                {
                    context.bfi82u_month.AddRange(tmpAddMonthList);
                    foreach (var item in tmpUpdateMonthList)
                    {
                        context.Entry<bfi82u_month>(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    context.SaveChanges();
                }
            }            
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDayDate = GetWorkAdDateString(date);
            string paramWeekDate = GetWeekMondayAdDateString(date);
            string paramMonthDate = GetWorkAdDateString(date); //GetMonthDate(date)
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/BFI82U?response={0}&dayDate={1}&weekDate={2}&monthDate={3}&type={4}&_={5}",
                paramResponse, paramDayDate, paramWeekDate, paramMonthDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        

        //private string GetMonthDate(DateTime today)
        //{
        //    if (today.DayOfWeek == DayOfWeek.Saturday)
        //    {
        //        DateTime friday = today.AddDays(-1);
        //        return friday.ToString("yyyyMMdd");
        //    }

        //    if (today.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        DateTime friday = today.AddDays(-2);
        //        return friday.ToString("yyyyMMdd");
        //    }
        //    return today.ToString("yyyyMMdd");
        //}

        

        

       
    }
}
