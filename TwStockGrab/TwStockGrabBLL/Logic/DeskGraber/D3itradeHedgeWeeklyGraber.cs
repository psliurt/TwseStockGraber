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
using TwStockGrabBLL.Logic.Rsp.Json.Desk;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    /// <summary>
    /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣明細資訊(周)
    /// d_3itrade_hedge_weekly
    /// 本資訊自民國96年4月20日起開始提供
    /// 
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge.php?l=zh-tw
    /// </summary>
    public class D3itradeHedgeWeeklyGraber : DGraber
    {
        public D3itradeHedgeWeeklyGraber() : base()
        {
            this._graberClassName = typeof(D3itradeHedgeWeeklyGraber).Name;
            this._graberFrequency = 7;
        }
        
        public override void DoJob(DateTime dataDate)
        {
            DateTime weekFirstDay = GetWeekMondayDate(dataDate);
            work_record record = null;
            if (GetOrCreateWorkRecord(weekFirstDay, out record))
            {
                return;
            }

            string responseContent = GetWebContent(weekFirstDay);
            D3itradeHedgeWeekly_Rsp rsp = JsonConvert.DeserializeObject<D3itradeHedgeWeekly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, weekFirstDay);
                WriteEndRecord(record);
                Sleep();
            }
        }

        private void SaveToDatabase(D3itradeHedgeWeekly_Rsp rsp, DateTime dataDate)
        {
            DateTime period1Start = new DateTime(2018, 1, 15);
            List<d_3itrade_hedge_weekly> tmpAddList = new List<d_3itrade_hedge_weekly>();
            List<d_3itrade_hedge_weekly> tmpUpdateList = new List<d_3itrade_hedge_weekly>();
            List<d_3itrade_hedge_weekly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrade_hedge_weekly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_3itrade_hedge_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    if (dataDate < period1Start)
                    {
                        tmpAddList.Add(new d_3itrade_hedge_weekly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            foreign_all_buy_in = ToIntQ(data.ElementAt(2)), //外資及陸資
                            foreign_all_sell_out = ToIntQ(data.ElementAt(3)),
                            foreign_all_diff = ToIntQ(data.ElementAt(4)),
                            invest_buy_in = ToIntQ(data.ElementAt(5)), //投信
                            invest_sell_out = ToIntQ(data.ElementAt(6)),
                            invest_diff = ToIntQ(data.ElementAt(7)),
                            dealer_all_diff = ToIntQ(data.ElementAt(8)), //自營商
                            dealer_self_buy_in = ToIntQ(data.ElementAt(9)),//自營商(自行買賣)
                            dealer_self_sell_out = ToIntQ(data.ElementAt(10)),
                            dealer_self_diff = ToIntQ(data.ElementAt(11)),
                            dealer_risk_buy_in = ToIntQ(data.ElementAt(12)),//自營商(避險)
                            dealer_risk_sell_out = ToIntQ(data.ElementAt(13)),
                            dealer_risk_diff = ToIntQ(data.ElementAt(14)),
                            total_diff = ToIntQ(data.ElementAt(15)),//三大法人買賣合計                            
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new d_3itrade_hedge_weekly
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            foreign_buy_in = ToIntQ(data.ElementAt(2)), //外資不含外資自營商
                            foreign_sell_out = ToIntQ(data.ElementAt(3)),
                            foreign_diff = ToIntQ(data.ElementAt(4)),
                            foreign_self_buy_in = ToIntQ(data.ElementAt(5)), //外資自營商
                            foreign_self_sell_out = ToIntQ(data.ElementAt(6)),
                            foreign_self_diff = ToIntQ(data.ElementAt(7)),
                            foreign_all_buy_in = ToIntQ(data.ElementAt(8)), //外資及陸資
                            foreign_all_sell_out = ToIntQ(data.ElementAt(9)),
                            foreign_all_diff = ToIntQ(data.ElementAt(10)),
                            invest_buy_in = ToIntQ(data.ElementAt(11)), //投信
                            invest_sell_out = ToIntQ(data.ElementAt(12)),
                            invest_diff = ToIntQ(data.ElementAt(13)),
                            dealer_self_buy_in = ToIntQ(data.ElementAt(14)), //自營商(自行買賣)
                            dealer_self_sell_out = ToIntQ(data.ElementAt(15)),
                            dealer_self_diff = ToIntQ(data.ElementAt(16)),
                            dealer_risk_buy_in = ToIntQ(data.ElementAt(17)),//自營商(避險)
                            dealer_risk_sell_out = ToIntQ(data.ElementAt(18)),
                            dealer_risk_diff = ToIntQ(data.ElementAt(19)),
                            dealer_all_buy_in = ToIntQ(data.ElementAt(20)), //自營商
                            dealer_all_sell_out = ToIntQ(data.ElementAt(21)),
                            dealer_all_diff = ToIntQ(data.ElementAt(22)),
                            total_diff = ToIntQ(data.ElementAt(23)), //三大法人買賣合計
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                }
                else
                {
                    if (dataDate < period1Start)
                    {
                        existItem.foreign_all_buy_in = ToIntQ(data.ElementAt(2)); //外資及陸資
                        existItem.foreign_all_sell_out = ToIntQ(data.ElementAt(3));
                        existItem.foreign_all_diff = ToIntQ(data.ElementAt(4));
                        existItem.invest_buy_in = ToIntQ(data.ElementAt(5)); //投信
                        existItem.invest_sell_out = ToIntQ(data.ElementAt(6));
                        existItem.invest_diff = ToIntQ(data.ElementAt(7));
                        existItem.dealer_all_diff = ToIntQ(data.ElementAt(8)); //自營商
                        existItem.dealer_self_buy_in = ToIntQ(data.ElementAt(9));//自營商(自行買賣)
                        existItem.dealer_self_sell_out = ToIntQ(data.ElementAt(10));
                        existItem.dealer_self_diff = ToIntQ(data.ElementAt(11));
                        existItem.dealer_risk_buy_in = ToIntQ(data.ElementAt(12));//自營商(避險)
                        existItem.dealer_risk_sell_out = ToIntQ(data.ElementAt(13));
                        existItem.dealer_risk_diff = ToIntQ(data.ElementAt(14));
                        existItem.total_diff = ToIntQ(data.ElementAt(15));//三大法人買賣合計                            
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;

                        tmpUpdateList.Add(existItem);                            
                        
                    }
                    else
                    {


                        existItem.foreign_buy_in = ToIntQ(data.ElementAt(2)); //外資不含外資自營商
                        existItem.foreign_sell_out = ToIntQ(data.ElementAt(3));
                        existItem.foreign_diff = ToIntQ(data.ElementAt(4));
                        existItem.foreign_self_buy_in = ToIntQ(data.ElementAt(5)); //外資自營商
                        existItem.foreign_self_sell_out = ToIntQ(data.ElementAt(6));
                        existItem.foreign_self_diff = ToIntQ(data.ElementAt(7));
                        existItem.foreign_all_buy_in = ToIntQ(data.ElementAt(8)); //外資及陸資
                        existItem.foreign_all_sell_out = ToIntQ(data.ElementAt(9));
                        existItem.foreign_all_diff = ToIntQ(data.ElementAt(10));
                        existItem.invest_buy_in = ToIntQ(data.ElementAt(11)); //投信
                        existItem.invest_sell_out = ToIntQ(data.ElementAt(12));
                        existItem.invest_diff = ToIntQ(data.ElementAt(13));
                        existItem.dealer_self_buy_in = ToIntQ(data.ElementAt(14)); //自營商(自行買賣)
                        existItem.dealer_self_sell_out = ToIntQ(data.ElementAt(15));
                        existItem.dealer_self_diff = ToIntQ(data.ElementAt(16));
                        existItem.dealer_risk_buy_in = ToIntQ(data.ElementAt(17));//自營商(避險)
                        existItem.dealer_risk_sell_out = ToIntQ(data.ElementAt(18));
                        existItem.dealer_risk_diff = ToIntQ(data.ElementAt(19));
                        existItem.dealer_all_buy_in = ToIntQ(data.ElementAt(20)); //自營商
                        existItem.dealer_all_sell_out = ToIntQ(data.ElementAt(21));
                        existItem.dealer_all_diff = ToIntQ(data.ElementAt(22));
                        existItem.total_diff = ToIntQ(data.ElementAt(23)); //三大法人買賣合計
                        existItem.title = rsp.reportTitle.Trim();
                        existItem.update_at = DateTime.Now;

                        tmpUpdateList.Add(existItem);
                        
                    }
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrade_hedge_weekly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_3itrade_hedge_weekly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "W"; //weekly
            string stockSelectType = "EW";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l=zh-tw&se=EW&t=W&d=109/01/04&_=1578809981470
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l={0}&se={1}&t={2}&d={3}&_={4}",
                lang, stockSelectType, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
