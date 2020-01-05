using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;

namespace TwStockGrabBLL.Filter.AfterMarket
{
    public class FilterShortStep1 : AfterMarketFilter
    {
        public FilterShortStep1(p_filter_stg stg) : base(stg)
        {

        }

        public override List<FilterResultData> ExecFilter()
        {
            DateTime dataDate = GetFilterDate();
            //找到最近四天(有開市)的日期

            List<DateTime> lastestNDate = new List<DateTime>();
            Dictionary<string, string> marketStockList = new Dictionary<string, string>();
            Dictionary<string, string> deskStockList = new Dictionary<string, string>();
            int day = 3 + 1;

            List<mi_margin> marketMarginList = null; //融資資料
            List<twt38u> marketForeignCapitalList = null; //外資買賣超
            //List<twt93u> marketLendList = null;//融券借券資料

            List<d_margin_bal> deskMarginList = null;// 融資資料
            List<d_3itrade_hedge_daily> deskForeignCapitalList = null;//外資買賣超
            //List<d_margin_sbl> deskLendList = null;//融券借券資料

            List<FilterResultData> filteredList = new List<FilterResultData>();

            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                marketStockList = ctx.Set<mi_index_all>().AsNoTracking().Where(x => x.data_date == dataDate).Distinct().ToDictionary(x => x.stock_no, x => x.stock_name);
                lastestNDate = ctx.Set<mi_margin_stat>().AsNoTracking().Select(x => x.data_date).Distinct().OrderByDescending(x => x).ToList().GetRange(0, day);
                marketMarginList = ctx.Set<mi_margin>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                marketForeignCapitalList = ctx.Set<twt38u>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                //marketLendList = ctx.Set<twt93u>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();

                deskStockList = ctx.Set<d_stk_quote>().AsNoTracking().Where(x => x.data_date == dataDate).Distinct().ToDictionary(x => x.stock_no, x => x.stock_name);
                deskMarginList = ctx.Set<d_margin_bal>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                deskForeignCapitalList = ctx.Set<d_3itrade_hedge_daily>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();
                //deskLendList = ctx.Set<d_margin_sbl>().AsNoTracking().Where(x => lastestNDate.Contains(x.data_date)).ToList();

            }

            bool condition1 = false;
            bool condition2 = false;

            foreach (var stock in marketStockList)
            {
                if (stock.Key.Length == 4)
                {
                    var marketStockMarginData = marketMarginList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();
                    var marketStockCapitalData = marketForeignCapitalList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();

                    //判斷融資連續三天都是大於基準的第一天
                    if (marketStockMarginData.Count() == day)
                    {
                        mi_margin firstDayStandard = marketStockMarginData.ElementAt(0);
                        bool allDayMoreThenFirst = false;
                        for (int i = 1; i < day; i++)
                        {
                            mi_margin eachDayData = marketStockMarginData.ElementAt(i);
                            if (eachDayData.finance_today_balance > firstDayStandard.finance_today_balance)
                            {
                                allDayMoreThenFirst = true;
                            }
                            else
                            {
                                allDayMoreThenFirst = false;
                                break;
                            }
                        }

                        condition1 = allDayMoreThenFirst;
                    }
                    else
                    {
                        condition1 = false;
                    }

                    //判斷外資賣超連續三天都是小於基準的第一天
                    if (marketStockCapitalData.Count() == day)
                    {
                        twt38u firstDayStandard = marketStockCapitalData.ElementAt(0);
                        bool allDayLessThenFirst = false;
                        for (int i = 1; i < day; i++)
                        {
                            twt38u eachDayData = marketStockCapitalData.ElementAt(i);
                            if (eachDayData.total_cnt_diff < firstDayStandard.total_cnt_diff)
                            {
                                allDayLessThenFirst = true;
                            }
                            else
                            {
                                allDayLessThenFirst = false;
                                break;
                            }
                        }

                        condition2 = allDayLessThenFirst;
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

            foreach (var stock in deskStockList)
            {
                if (stock.Key.Length == 4)
                {
                    var deskStockMarginData = deskMarginList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();
                    var deskStockCapitalData = deskForeignCapitalList.Where(x => x.stock_no == stock.Key).OrderBy(x => x.data_date).ToList();


                    if (deskStockMarginData.Count() == day)
                    {
                        d_margin_bal firstDayStandard = deskStockMarginData.ElementAt(0);
                        bool allDayMoreThenFirst = false;
                        for (int i = 1; i < day; i++)
                        {
                            d_margin_bal eachDayData = deskStockMarginData.ElementAt(i);
                            if (eachDayData.lend_balance > firstDayStandard.lend_balance)
                            {
                                allDayMoreThenFirst = true;
                            }
                            else
                            {
                                allDayMoreThenFirst = false;
                                break;
                            }
                        }

                        condition1 = allDayMoreThenFirst;
                    }
                    else
                    {
                        condition1 = false;
                    }

                    if (deskStockCapitalData.Count() == day)
                    {
                        d_3itrade_hedge_daily firstDayStandard = deskStockCapitalData.ElementAt(0);
                        bool allDayLessThenFirst = false;
                        for (int i = 1; i < day; i++)
                        {
                            d_3itrade_hedge_daily eachDayData = deskStockCapitalData.ElementAt(i);
                            if (eachDayData.foreign_all_diff < firstDayStandard.foreign_all_diff)
                            {
                                allDayLessThenFirst = true;
                            }
                            else
                            {
                                allDayLessThenFirst = false;
                                break;
                            }
                        }

                        condition2 = allDayLessThenFirst;
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

            //融資資料表
            //外資賣超資料表
            //借券資料表
            //融券資料表

            return filteredList;
        }
    }
}
