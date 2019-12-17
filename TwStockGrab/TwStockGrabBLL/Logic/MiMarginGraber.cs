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
    /// 交易資訊->融資融券與可借券賣出額度->融資融券餘額
    /// mi_margin
    /// mi_margin_stat
    /// 本資訊自民國90年01月01日起提供
    /// </summary>
    public class MiMarginGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALL");        //全部     

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                MI_MARGN_Rsp rsp = JsonConvert.DeserializeObject<MI_MARGN_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, type);
                    Sleep();
                }
            }
        }        

        private void SaveToDatabase(MI_MARGN_Rsp rsp, DateTime grabeDate, string selectType)
        {
            if (rsp.creditFields.Count() == 0)
            { return; }



            DateTime dataDate = GetDateFromAdDateString(rsp.date);            

            List<mi_margin> tmpAddList = new List<mi_margin>();
            List<mi_margin> tmpDataList = null;
            List<mi_margin_stat> tmpStatisticDataList = null;
            List<mi_margin_stat> tmpNewStatisticDataList = new List<mi_margin_stat>();
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_margin>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
                tmpStatisticDataList = context.Set<mi_margin_stat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();


                mi_margin obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new mi_margin
                    {
                        data_date = dataDate,
                        select_type = selectType,
                        stock_no = stockNo,
                        stock_name = stockName,
                        finance_buy_in = ToDecimalQ(data.ElementAt(2)),
                        finance_sell_out = ToDecimalQ(data.ElementAt(3)),
                        finance_cash_back = ToDecimalQ(data.ElementAt(4)),
                        finance_yesterday_balance = ToDecimalQ(data.ElementAt(5)),
                        finance_today_balance = ToDecimalQ(data.ElementAt(6)),
                        finance_ceiling = ToDecimalQ(data.ElementAt(7)),
                        margin_buy_in = ToDecimalQ(data.ElementAt(8)),
                        margin_sell_out = ToDecimalQ(data.ElementAt(9)),
                        margin_cash_back = ToDecimalQ(data.ElementAt(10)),
                        margin_yesterday_balance = ToDecimalQ(data.ElementAt(11)),
                        margin_today_balance = ToDecimalQ(data.ElementAt(12)),
                        margin_ceiling = ToDecimalQ(data.ElementAt(13)),
                        offset = ToLongQ(data.ElementAt(14)),
                        note = data.ElementAt(15).Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });

                }

            }


            foreach (var creditData in rsp.creditList)
            {
                string statisticItem = creditData.ElementAt(0).Trim();

                mi_margin_stat statObj = tmpStatisticDataList.Where(x => x.data_date == dataDate && x.stat_item == statisticItem).FirstOrDefault();
                if (statObj == null)
                {
                    tmpNewStatisticDataList.Add(new mi_margin_stat
                    {                        
                        data_date = dataDate,
                        stat_item = statisticItem,                        
                        buy_in = ToDecimalQ(creditData.ElementAt(1)),
                        sell_out = ToDecimalQ(creditData.ElementAt(2)),
                        return_back = ToDecimalQ(creditData.ElementAt(3)),
                        yesterday_balance = ToDecimalQ(creditData.ElementAt(4)),
                        today_balance = ToDecimalQ(creditData.ElementAt(5)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.creditTitle)
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_margin.AddRange(tmpAddList);
                context.mi_margin_stat.AddRange(tmpNewStatisticDataList);
                
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_MARGN?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        //private string GetDateString(DateTime dt)
        //{
        //    return dt.ToString("yyyyMMdd");
        //}

        //private string GetTimeStamp()
        //{
        //    return DateTime.Now.Ticks.ToString();
        //}

        //private decimal? ToDecimal(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //    { return null; }
        //    if (data == "--")
        //    {
        //        return null;
        //    }
        //    return Convert.ToDecimal(data);
        //}

        //private int? ToInt(string data)
        //{
        //    data = data.Replace(",", "");
        //    return Convert.ToInt32(data);
        //}

        //private sbyte? ToByte(string data)
        //{
        //    if (data.Contains("+"))
        //    {
        //        return 1;
        //    }
        //    if (data.Contains("-"))
        //    {
        //        return -1;
        //    }
        //    return 0;

        //}
        //private long? ToLong(string data)
        //{
        //    data = data.Replace(",", "");
        //    return Convert.ToInt64(data);
        //}

        //private DateTime GetMonthDate(DateTime today)
        //{
        //    return new DateTime(today.Year, today.Month, 1);
        //}

        //private DateTime GetDateFromDataDateString(string dataDateString)
        //{
        //    int year = Convert.ToInt32(dataDateString.Substring(0, 4));
        //    int month = Convert.ToInt32(dataDateString.Substring(4, 2));
        //    int day = Convert.ToInt32(dataDateString.Substring(6, 2));

        //    return new DateTime(year, month, day);
        //}

        //private DateTime GetDateFromRocPointString(string rocString)
        //{
        //    string[] dateParts = rocString.Split('.');
        //    int year = 1911 + Convert.ToInt32(dateParts[0]);
        //    int month = Convert.ToInt32(dateParts[1]);
        //    int day = Convert.ToInt32(dateParts[2]);

        //    return new DateTime(year, month, day);
        //}

    }
}
