﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;

namespace TwStockGrabBLL.Filter.AfterMarket
{
    public class FilterNo2 : AfterMarketFilter
    {
        public FilterNo2(p_filter_stg stg) : base(stg)
        {

        }

        public override List<FilterResultData> ExecFilter()
        {
            DateTime dataDate = GetFilterDate();
            List<mi_index_all> marketCloseList = null;
            List<t86> market3CapitalList = null;
            List<mi_margin> marketMarginList = null;
            List<d_3itrade_hedge_daily> desk3CapitalList = null;
            List<d_margin_bal> deskMarginList = null;
            List<d_stk_quote> deskCloseList = null;

            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                marketCloseList = ctx.Set<mi_index_all>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                market3CapitalList = ctx.Set<t86>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                marketMarginList = ctx.Set<mi_margin>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();

                deskCloseList = ctx.Set<d_stk_quote>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                desk3CapitalList = ctx.Set<d_3itrade_hedge_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                deskMarginList = ctx.Set<d_margin_bal>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            decimal volumnShouleBe = Convert.ToInt32(this._stgSetting.stg_p3) * 1000; // 2000000; //以股為單位，所以這裡是張
            decimal wavePercentShouleBe = Convert.ToInt32(this._stgSetting.stg_p2); // -2;
            decimal foreignDiffShouldBe = Convert.ToInt32(this._stgSetting.stg_p1) * 1000; // -1000000; //以股為單位，所以這裡是賣超1000張
            decimal dealerDiffShouleBe = Convert.ToInt32(this._stgSetting.stg_p4) * 1000; // 0; //這裡賣超0張
            decimal marginShouldBe = Convert.ToInt32(this._stgSetting.stg_p5); // 0; //今日融資0
            List<FilterResultData> filteredList = new List<FilterResultData>();

            bool condition1 = false;
            bool condition2 = false;
            bool condition3 = false;
            bool condition4 = false;
            bool condition5 = false;

            foreach (var marketStock in marketCloseList)
            {
                if (marketStock.stock_no.Length == 4)
                {
                    decimal volumn = Kit.ToDecimal(marketStock.deal_stock_num);

                    if (volumn >= volumnShouleBe)
                    {
                        condition1 = true;
                    }

                    sbyte upDown = marketStock.up_down.HasValue ? marketStock.up_down.Value : (sbyte)0;
                    decimal upDownPrice = marketStock.up_down_price.HasValue ? marketStock.up_down_price.Value : 0;
                    decimal todayClose = marketStock.close_price.HasValue ? marketStock.close_price.Value : 0;

                    decimal yesterdayClose = todayClose - (upDownPrice * (upDown));
                    decimal wavePercent = yesterdayClose != 0 ? ((upDownPrice * (upDown)) / yesterdayClose) * 100 : 0;

                    if (wavePercent <= wavePercentShouleBe)
                    {
                        condition2 = true;
                    }

                    t86 capitalObj = market3CapitalList.Where(x => x.stock_no == marketStock.stock_no).FirstOrDefault();
                    if (capitalObj != null)
                    {
                        decimal foreignDealerDiff = capitalObj.foreign_dealer_diff.HasValue ? capitalObj.foreign_dealer_diff.Value : 0;
                        decimal foreignDiff = capitalObj.foreign_diff.HasValue ? capitalObj.foreign_diff.Value : 0;
                        decimal foreignTotalDiff = foreignDealerDiff + foreignDiff;

                        if (foreignTotalDiff <= foreignDiffShouldBe)
                        {
                            condition3 = true;
                        }

                        decimal dealerSelfDiff = Kit.ToDecimal(capitalObj.dealer_self_diff);
                        decimal dealerRiskDiff = Kit.ToDecimal(capitalObj.dealer_risk_diff);
                        decimal dealerTotalDiff = dealerSelfDiff + dealerRiskDiff;

                        if (dealerTotalDiff < dealerDiffShouleBe)
                        {
                            condition4 = true;
                        }
                    }

                    mi_margin margin = marketMarginList.Where(x => x.stock_no == marketStock.stock_no).FirstOrDefault();
                    if (margin != null)
                    {
                        decimal todayMargin = Kit.ToDecimal(margin.finance_today_balance);

                        if (todayMargin > marginShouldBe)
                        {
                            condition5 = true;
                        }
                        else
                        {

                        }
                    }

                    if (condition1 && condition2 && condition3 && condition4 && condition5)
                    {
                        filteredList.Add(new FilterResultData
                        {
                            DataDate = dataDate,
                            FilterName = this._stgSetting.stg_name,
                            StockNo = marketStock.stock_no,
                            StockName = marketStock.stock_name
                        });
                    }
                }

                condition1 = false;
                condition2 = false;
                condition3 = false;
                condition4 = false;
                condition5 = false;
            }

            foreach (var deskStock in deskCloseList)
            {
                if (deskStock.stock_no.Length == 4)
                {
                    long volumn = Kit.ToLong(deskStock.deal_stock_cnt);
                    if (volumn >= volumnShouleBe)
                    {
                        condition1 = true;
                    }

                    decimal upDownPrice = deskStock.up_down_percent.HasValue ? deskStock.up_down_percent.Value : 0;
                    decimal todayClose = deskStock.close_p.HasValue ? deskStock.close_p.Value : 0;

                    decimal yesterdayClose = todayClose - (upDownPrice);
                    decimal wavePercent = yesterdayClose != 0 ? ((upDownPrice) / yesterdayClose) * 100 : 0;

                    if (wavePercent <= wavePercentShouleBe)
                    {
                        condition2 = true;
                    }

                    d_3itrade_hedge_daily capitalObj = desk3CapitalList.Where(x => x.stock_no == deskStock.stock_no).FirstOrDefault();
                    if (capitalObj != null)
                    {
                        int foreignDiff = capitalObj.foreign_all_diff.HasValue ? capitalObj.foreign_all_diff.Value : 0;
                        if (foreignDiff <= foreignDiffShouldBe)
                        {
                            condition3 = true;
                        }

                        int dealerDiff = Kit.ToInt(capitalObj.dealer_all_diff);
                        if (dealerDiff < dealerDiffShouleBe)
                        {
                            condition4 = true;
                        }
                    }

                    d_margin_bal margin = deskMarginList.Where(x => x.stock_no == deskStock.stock_no).FirstOrDefault();
                    if (margin != null)
                    {

                        int todayBalance = Kit.ToInt(margin.lend_balance);

                        if (todayBalance > marginShouldBe)
                        {
                            condition5 = true;
                        }
                        else
                        {

                        }
                    }

                    if (condition1 && condition2 && condition3 && condition4 && condition5)
                    {
                        filteredList.Add(new FilterResultData
                        {
                            DataDate = dataDate,
                            FilterName = this._stgSetting.stg_name,
                            StockNo = deskStock.stock_no,
                            StockName = deskStock.stock_name
                        });
                    }

                }

                condition1 = false;
                condition2 = false;
                condition3 = false;
                condition4 = false;
                condition5 = false;
            }

            
            
            return filteredList;
        }
    }
}
