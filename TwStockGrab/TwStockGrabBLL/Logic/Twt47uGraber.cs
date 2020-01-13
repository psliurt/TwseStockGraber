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
    /// 交易資訊->三大法人->三大法人買賣超月報
    /// twt47u
    /// 本資訊自民國91年12月16日起提供
    /// 這個類別一天資料抓取的時間會稍微多一點
    /// </summary>
    public class Twt47uGraber : Graber
    {
        public Twt47uGraber() : base()
        {
            this._graberClassName = typeof(Twt47uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("ALL");        //全部                        

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                TWT47U_Rsp rsp = JsonConvert.DeserializeObject<TWT47U_Rsp>(responseContent);

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

        
        private void SaveToDatabase(TWT47U_Rsp rsp, DateTime dataDate, string selectType)
        {
            int year = dataDate.Year;
            int month = dataDate.Month;

            List<twt47u> tmpAddList = new List<twt47u>();
            List<twt47u> tmpUptList = new List<twt47u>();
            List<twt47u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt47u>().AsNoTracking().Where(x => x.data_year == year && x.data_month == month && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                if (data.Count() >= 19)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    //this._stockBag.CheckStock(stockNo, data.ElementAt(1).Trim());

                    twt47u obj =
                        tmpDataList.Where(x => x.data_year == year && x.data_month == month && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddList.Add(new twt47u
                        {
                            data_year = year,
                            data_month = month,
                            last_update = dataDate,
                            select_type = selectType,
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            foreign_buy_in = ToLongQ(data.ElementAt(2)),
                            foreign_sell_out = ToLongQ(data.ElementAt(3)),
                            foreign_diff = ToLongQ(data.ElementAt(4)),
                            foreign_dealer_buy_in = ToLongQ(data.ElementAt(5)),
                            foreign_dealer_sell_out = ToLongQ(data.ElementAt(6)),
                            foreign_dealer_diff = ToLongQ(data.ElementAt(7)),
                            trust_buy_in = ToLongQ(data.ElementAt(8)),
                            trust_sell_out = ToLongQ(data.ElementAt(9)),
                            trust_diff = ToLongQ(data.ElementAt(10)),
                            dealer_diff = ToLongQ(data.ElementAt(11)),
                            dealer_self_buy_in = ToLongQ(data.ElementAt(12)),
                            dealer_self_sell_out = ToLongQ(data.ElementAt(13)),
                            dealer_self_diff = ToLongQ(data.ElementAt(14)),
                            dealer_risk_buy_in = ToLongQ(data.ElementAt(15)),
                            dealer_risk_sell_out = ToLongQ(data.ElementAt(16)),
                            dealer_risk_diff = ToLongQ(data.ElementAt(17)),
                            capital3_total_diff = ToLongQ(data.ElementAt(18)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });

                    }
                    else // exists in database
                    {
                        bool needUpdate = false;

                        if (obj.foreign_buy_in != ToLongQ(data.ElementAt(2)))
                        {
                            needUpdate = true;
                            obj.foreign_buy_in = ToLongQ(data.ElementAt(2));
                        }
                        if (obj.foreign_sell_out != ToLongQ(data.ElementAt(3)))
                        {
                            needUpdate = true;
                            obj.foreign_sell_out = ToLongQ(data.ElementAt(3));
                        }
                        if (obj.foreign_diff != ToLongQ(data.ElementAt(4)))
                        {
                            needUpdate = true;
                            obj.foreign_diff = ToLongQ(data.ElementAt(4));
                        }
                        if (obj.foreign_dealer_buy_in != ToLongQ(data.ElementAt(5)))
                        {
                            needUpdate = true;
                            obj.foreign_dealer_buy_in = ToLongQ(data.ElementAt(5));
                        }
                        if (obj.foreign_dealer_sell_out != ToLongQ(data.ElementAt(6)))
                        {
                            needUpdate = true;
                            obj.foreign_dealer_sell_out = ToLongQ(data.ElementAt(6));
                        }
                        if (obj.foreign_dealer_diff != ToLongQ(data.ElementAt(7)))
                        {
                            needUpdate = true;
                            obj.foreign_dealer_diff = ToLongQ(data.ElementAt(7));
                        }
                        if (obj.trust_buy_in != ToLongQ(data.ElementAt(8)))
                        {
                            needUpdate = true;
                            obj.trust_buy_in = ToLongQ(data.ElementAt(8));
                        }
                        if (obj.trust_sell_out != ToLongQ(data.ElementAt(9)))
                        {
                            needUpdate = true;
                            obj.trust_sell_out = ToLongQ(data.ElementAt(9));
                        }
                        if (obj.trust_diff != ToLongQ(data.ElementAt(10)))
                        {
                            needUpdate = true;
                            obj.trust_diff = ToLongQ(data.ElementAt(10));
                        }
                        if (obj.dealer_diff != ToLongQ(data.ElementAt(11)))
                        {
                            needUpdate = true;
                            obj.dealer_diff = ToLongQ(data.ElementAt(11));
                        }
                        if (obj.dealer_self_buy_in != ToLongQ(data.ElementAt(12)))
                        {
                            needUpdate = true;
                            obj.dealer_self_buy_in = ToLongQ(data.ElementAt(12));
                        }
                        if (obj.dealer_self_sell_out != ToLongQ(data.ElementAt(13)))
                        {
                            needUpdate = true;
                            obj.dealer_self_sell_out = ToLongQ(data.ElementAt(13));
                        }
                        if (obj.dealer_self_diff != ToLongQ(data.ElementAt(14)))
                        {
                            needUpdate = true;
                            obj.dealer_self_diff = ToLongQ(data.ElementAt(14));
                        }
                        if (obj.dealer_risk_buy_in != ToLongQ(data.ElementAt(15)))
                        {
                            needUpdate = true;
                            obj.dealer_risk_buy_in = ToLongQ(data.ElementAt(15));
                        }
                        if (obj.dealer_risk_sell_out != ToLongQ(data.ElementAt(16)))
                        {
                            needUpdate = true;
                            obj.dealer_risk_sell_out = ToLongQ(data.ElementAt(16));
                        }
                        if (obj.dealer_risk_diff != ToLongQ(data.ElementAt(17)))
                        {
                            needUpdate = true;
                            obj.dealer_risk_diff = ToLongQ(data.ElementAt(17));
                        }
                        if (obj.capital3_total_diff != ToLongQ(data.ElementAt(18)))
                        {
                            needUpdate = true;
                            obj.capital3_total_diff = ToLongQ(data.ElementAt(18));
                        }

                        if (needUpdate)
                        {
                            obj.last_update = dataDate;
                            obj.update_at = DateTime.Now;
                            //context.Entry<twt54u>(obj).State = System.Data.Entity.EntityState.Modified;
                            tmpUptList.Add(obj);
                        }

                    }


                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt47u.AddRange(tmpAddList);
                foreach (var uptObj in tmpUptList)
                {
                    context.Entry<twt47u>(uptObj).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramMonthDate = GetMonthFirstDate(date).ToString("yyyyMMdd");
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/TWT47U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramMonthDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        

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
    }
}
