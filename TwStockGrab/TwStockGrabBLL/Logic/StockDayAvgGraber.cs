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
    /// 交易資訊->盤後資訊->個股日收盤價及月平均價
    /// stock_day_avg
    /// 本資訊自民國88年1月5日起開始提供
    /// https://www.twse.com.tw/zh/page/trading/exchange/STOCK_DAY_AVG.html
    /// </summary>
    public class StockDayAvgGraber :Graber
    {
        private StockBag _stockBag { get; set; }
        public StockDayAvgGraber() : base()
        {
            _stockBag = StockBag.GetInstance();
            this._graberClassName = typeof(StockDayAvgGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("01");       //水泥工業
            selectTypeList.Add("02");       //食品工業
            selectTypeList.Add("03");       //塑膠工業
            selectTypeList.Add("04");       //紡織纖維
            selectTypeList.Add("05");       //電機機械
            selectTypeList.Add("06");       //電器電纜
            selectTypeList.Add("07");       //化學生技醫療
            selectTypeList.Add("21");       //化學工業
            selectTypeList.Add("22");       //生技醫療業
            selectTypeList.Add("08");       //玻璃陶瓷
            selectTypeList.Add("09");       //造紙工業
            selectTypeList.Add("10");       //鋼鐵工業
            selectTypeList.Add("11");       //橡膠工業
            selectTypeList.Add("12");       //汽車工業
            selectTypeList.Add("13");       //電子工業
            selectTypeList.Add("24");       //半導體業
            selectTypeList.Add("25");       //電腦及週邊設備業
            selectTypeList.Add("26");       //光電業
            selectTypeList.Add("27");       //通信網路業
            selectTypeList.Add("28");       //電子零組件業
            selectTypeList.Add("29");       //電子通路業
            selectTypeList.Add("30");       //資訊服務業
            selectTypeList.Add("31");       //其他電子業
            selectTypeList.Add("14");       //建材營造
            selectTypeList.Add("15");       //航運業
            selectTypeList.Add("16");       //觀光事業
            selectTypeList.Add("17");       //金融保險
            selectTypeList.Add("18");       //貿易百貨
            selectTypeList.Add("23");       //油電燃氣業
            selectTypeList.Add("19");       //綜合
            selectTypeList.Add("20");       //其他

            List<stock_item> stockList = this._stockBag.GetListByCategorys(selectTypeList);

            foreach (stock_item stock in stockList)
            {
                work_record record = null;
                if (GetOrCreateWorkRecord(dataDate, stock.stock_no, out record) == false)
                {
                    string responseContent = GetWebContent(dataDate, stock.stock_no);
                    STOCK_DAY_AVG_Rsp rsp = JsonConvert.DeserializeObject<STOCK_DAY_AVG_Rsp>(responseContent);

                    if (rsp.data == null)
                    {
                        WriteEndRecord(record);
                        Sleep();
                    }
                    else
                    {
                        SaveToDatabase(rsp, dataDate, stock.stock_no);
                        WriteEndRecord(record);
                        Sleep();
                    }
                }                
            }
        }        

        private void SaveToDatabase(STOCK_DAY_AVG_Rsp rsp, DateTime dataDate, string stockNo)
        {
            DateTime startDay = new DateTime(dataDate.Year, dataDate.Month, 1);
            DateTime endDay = startDay.AddMonths(1).AddDays(-1);

            List<stock_day_avg> tmpAddList = new List<stock_day_avg>();
            List<stock_day_avg> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<stock_day_avg>().AsNoTracking().Where(x => x.stock_no == stockNo && x.data_date >= startDay && x.data_date <= endDay).ToList();
            }

            foreach (var data in rsp.data)
            {
                DateTime? dbDataDate = GetDateFromRocSlashStringQ(data.ElementAt(0).Trim());
                if (dbDataDate.HasValue)
                {
                    stock_day_avg existItem = tmpDataList.Where(x => x.data_date == dbDataDate.Value && x.stock_no == stockNo).FirstOrDefault();

                    if (existItem == null)
                    {
                        tmpAddList.Add(new stock_day_avg
                        {
                            stock_no = stockNo,
                            data_date = dbDataDate.Value,
                            close_price = ToDecimalQ(data.ElementAt(1)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.stock_day_avg.AddRange(tmpAddList);                

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string stockNo)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/STOCK_DAY_AVG?response={0}&date={1}&stockNo={2}&_={3}",
                paramResponse, paramDate, stockNo, paramUnderLine);

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

        //private decimal? ToDiffDecimal(string data)
        //{
        //    if (data.StartsWith("-"))
        //    {
        //        return Convert.ToDecimal(data);
        //    }
        //    else if (data.StartsWith("+"))
        //    {
        //        return Convert.ToDecimal(data.Substring(1));
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //private DateTime? GetDataDate(string dateString)
        //{
        //    string[] dateParts = dateString.Split('/');
        //    if (dateParts.Count() != 3)
        //    {
        //        return null;
        //    }

        //    int year = 1911 + Convert.ToInt32(dateParts[0]);
        //    int month = Convert.ToInt32(dateParts[1]);
        //    int day = Convert.ToInt32(dateParts[2]);
        //    return new DateTime(year, month, day);

        //}
    }
}
