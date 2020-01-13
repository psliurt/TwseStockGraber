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
    /// 交易資訊->三大法人->三大法人買賣超週報
    /// twt54u
    /// 本資訊自民國91年12月16日起提供
    /// </summary>
    public class Twt54uGraber : Graber
    {
        public Twt54uGraber() : base()
        {
            this._graberClassName = typeof(Twt54uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALL");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                TWT54U_Rsp rsp = JsonConvert.DeserializeObject<TWT54U_Rsp>(responseContent);

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

        private void SaveToDatabase(TWT54U_Rsp rsp, DateTime dataDate, string selectType)
        {
            DateTime start;
            DateTime end;
            CountWeekStartAndEndDate(dataDate, out start, out end);

            List<twt54u> tmpAddList = new List<twt54u>();
            List<twt54u> tmpUptList = new List<twt54u>();
            List<twt54u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt54u>().AsNoTracking().Where(x => x.week_start == start && x.week_end == end && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                if (data.Count() >= 19)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    twt54u obj =
                        tmpDataList.Where(x => x.week_start == start && x.week_end == end && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddList.Add(new twt54u
                        {
                            week_start = start,
                            week_end = end,
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
                context.twt54u.AddRange(tmpAddList);
                foreach (var uptObj in tmpUptList)
                {
                    context.Entry<twt54u>(uptObj).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramWeekDate = GetMondayAdDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/TWT54U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramWeekDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
