using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;

namespace TwStockGrabBLL.Filter.AfterMarket
{
    public class FilterBlackShort1 : AfterMarketFilter
    {
        public FilterBlackShort1(p_filter_stg stg) : base(stg)
        { }

        public override List<FilterResultData> ExecFilter()
        {
            DateTime dataDate = GetFilterDate();
            List<FilterResultData> filteredList = new List<FilterResultData>();

            List<DateTime> lastestNDate = new List<DateTime>();
            Dictionary<string, string> marketStockList = new Dictionary<string, string>();
            Dictionary<string, string> deskStockList = new Dictionary<string, string>();
            int day = 2 + 1;

            //連續兩天融資維持在50%以上
            //融資資料
            List<mi_margin> marketMarginList = null; //上市融資資料
            List<d_margin_bal> deskMarginList = null; //上櫃融資資料

            //連續兩天 外資賣超，而且賣得越來越多
            //外資資料
            List<twt38u> marketForeignList = null; //上市外資資料
            List<d_3itrade_hedge_daily> deskForeignList = null; //上櫃外資資料

            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                marketStockList = ctx.Set<mi_index_all>().AsNoTracking().Where(x => x.data_date == dataDate).Distinct().ToDictionary(x => x.stock_no, x => x.stock_name);
                lastestNDate = ctx.Set<mi_margin_stat>().AsNoTracking().Select(x => x.data_date).Distinct().OrderByDescending(x => x).ToList().GetRange(0, day);
                marketMarginList = ctx.Set<mi_margin>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                marketForeignList = ctx.Set<twt38u>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();                

                deskStockList = ctx.Set<d_stk_quote>().AsNoTracking().Where(x => x.data_date == dataDate).Distinct().ToDictionary(x => x.stock_no, x => x.stock_name);
                deskMarginList = ctx.Set<d_margin_bal>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                deskForeignList = ctx.Set<d_3itrade_hedge_daily>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();                

            }

            bool condition1 = false; //連續兩天 融資維持在50%以上
            bool condition2 = false; //外資連續賣超兩天，而且後面一天比前一天賣得多

            //上市股票判斷
            foreach (var stock in marketStockList)
            {
                if (stock.Key.Length == 4)
                {
                    var marketStockMarginData = marketMarginList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();
                    var marketStockCapitalData = marketForeignList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();

                    //判斷融資比例兩天以來都是 >= 50%
                    if (marketStockMarginData.Count() == day)
                    {
                        mi_margin firstDayStandard = marketStockMarginData.ElementAt(0);

                        if (firstDayStandard.finance_today_balance > 0) //第一天的融資必須大於0
                        {
                            bool allDayMoreThen50Percent = false;
                            for (int i = 1; i < day; i++)
                            {
                                mi_margin eachDayData = marketStockMarginData.ElementAt(i);

                                if (eachDayData.finance_ceiling.HasValue == false || eachDayData.finance_today_balance.HasValue == false || eachDayData.finance_ceiling == 0)
                                {
                                    allDayMoreThen50Percent = false;
                                    break;
                                }

                                decimal marginPercent = (eachDayData.finance_today_balance.Value / eachDayData.finance_ceiling.Value) * 100; 

                                if (marginPercent >= 25)
                                {
                                    allDayMoreThen50Percent = true;
                                }
                                else
                                {
                                    allDayMoreThen50Percent = false;
                                    break;
                                }
                            }

                            condition1 = allDayMoreThen50Percent;
                        }
                        else
                        {
                            condition1 = false;
                        }
                    }
                    else
                    {
                        condition1 = false;
                    }

                    //判斷外資賣超連續2天，而且第2天比第一天賣得多
                    if (marketStockCapitalData.Count() == day)
                    {
                        twt38u firstDayStandard = marketStockCapitalData.ElementAt(0);
                        twt38u secondDay = marketStockCapitalData.ElementAt(1);
                        twt38u thirdDay = marketStockCapitalData.ElementAt(2);                        

                        if (firstDayStandard.total_cnt_diff <= 0 &&
                            secondDay.total_cnt_diff < firstDayStandard.total_cnt_diff &&                            
                            thirdDay.total_cnt_diff < secondDay.total_cnt_diff)
                        {
                            condition2 = true;
                        }
                        else
                        {
                            condition2 = false;
                        }
                    }
                    else
                    {
                        condition2 = false;
                    }

                    if (condition1 && condition2)
                    {
                        filteredList.Add(new FilterResultData
                        {
                            DataDate = dataDate,
                            FilterName = this._stgSetting.stg_name,
                            StockNo = stock.Key,
                            StockName = stock.Value,
                            Note = ""
                        });
                    }
                }

                condition1 = false;
                condition2 = false;
            }

            //上櫃股票判斷
            foreach (var stock in deskStockList)
            {
                if (stock.Key.Length == 4)
                {
                    var deskStockMarginData = deskMarginList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();
                    var deskStockCapitalData = deskForeignList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();

                    //上櫃股票，融資都要比第一天增加
                    if (deskStockMarginData.Count() == day)
                    {
                        d_margin_bal firstDayStandard = deskStockMarginData.ElementAt(0);

                        if (firstDayStandard.lend_balance > 0) //第一天有融資
                        {
                            bool allDayMoreThen50Percent = false;
                            for (int i = 1; i < day; i++)
                            {
                                d_margin_bal eachDayData = deskStockMarginData.ElementAt(i);
                                if (eachDayData.lend_percent.HasValue == false)
                                {
                                    allDayMoreThen50Percent = false;
                                    break;
                                }
                                else
                                {
                                    if (eachDayData.lend_percent.Value >= 50)
                                    {
                                        allDayMoreThen50Percent = true;
                                    }
                                    else
                                    {
                                        allDayMoreThen50Percent = false;
                                        break;
                                    }
                                }
                                
                            }

                            condition1 = allDayMoreThen50Percent;
                        }
                        else
                        {
                            condition1 = false;
                        }

                    }
                    else
                    {
                        condition1 = false;
                    }

                    //上櫃股票，外資賣超，每天都要比前一天賣得更多
                    if (deskStockCapitalData.Count() == day)
                    {
                        d_3itrade_hedge_daily firstDayStandard = deskStockCapitalData.ElementAt(0);
                        d_3itrade_hedge_daily secondDay = deskStockCapitalData.ElementAt(1);
                        d_3itrade_hedge_daily thirdDay = deskStockCapitalData.ElementAt(2);                        

                        if (firstDayStandard.foreign_all_diff <= 0 &&
                            secondDay.foreign_all_diff < firstDayStandard.foreign_all_diff &&                            
                            thirdDay.foreign_all_diff < secondDay.foreign_all_diff)
                        {
                            condition2 = true;
                        }
                        else
                        {
                            condition2 = false;
                        }


                    }
                    else
                    {
                        condition2 = false;
                    }

                    if (condition1 && condition2)
                    {
                        filteredList.Add(new FilterResultData
                        {
                            DataDate = dataDate,
                            FilterName = this._stgSetting.stg_name,
                            StockNo = stock.Key,
                            StockName = stock.Value,
                            Note = ""
                        });
                    }
                }

                condition1 = false;
                condition2 = false;
            }

            //外資借券也出現?這個條件以後再說


            return filteredList;
        }
    }
}
