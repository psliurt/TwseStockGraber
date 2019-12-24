using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TwStockGrabBLL.DAL;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 上市股票
    /// 以下所列的交易資訊都還沒有實作     
    /// 1. 鉅額交易日成交資訊 https://www.twse.com.tw/zh/page/trading/block/BFIAUU.html     
    /// 
    /// 2. 零股交易行情單 https://www.twse.com.tw/zh/page/trading/exchange/TWT53U.html 
    /// 3. 應付現股當日沖銷券差借券費率 https://www.twse.com.tw/zh/page/trading/exchange/BFIF8U.html     
    /// 
    /// 6. 停券預告表 https://www.twse.com.tw/zh/page/trading/exchange/BFI84U.html
    /// 3. 暫停先賣後買當日沖銷交易標的預告表 https://www.twse.com.tw/zh/page/trading/exchange/TWTBAU1.html  
    /// 8. 當日可借券賣出股數 https://www.twse.com.tw/zh/page/trading/SBL/TWT96U.html    
    /// </summary>
    public class PerformanceTest
    {
        public double Test1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            IEnumerable<twt43u> enu = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var queryable = context.Set<twt43u>().AsNoTracking().AsQueryable();

                enu = queryable.ToList();
            }

            foreach (var obj in enu)
            {
                string a = obj.stock_name;
            }

            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            return ts.TotalMilliseconds;
        }
            
    }


    #region  上櫃資料api url 清單
    /// # 盤後資訊
    ///   4. 上櫃證券成交統計(週月年) https://www.tpex.org.tw/web/stock/aftertrading/market_statistics/statistics_result.php?l=zh-tw&t=D&d=108/10/02    
    ///   6. 漲跌停未成交資訊 https://www.tpex.org.tw/web/stock/aftertrading/ceil_non_trading/ceil_ord_result.php?l=zh-tw&d=108/10/15&_=1571412875341
    ///   7. 日成交量值指數 https://www.tpex.org.tw/web/stock/aftertrading/daily_trading_index/st41_result.php?l=zh-tw&d=108/07/01&_=1571412851225
    ///   8. 個股日成交資訊 https://www.tpex.org.tw/web/stock/aftertrading/daily_trading_info/st43_result.php?l=zh-tw&d=108/07&stkno=4401&_=1571412797183    
    ///   10. 盤後定價行情 https://www.tpex.org.tw/web/stock/aftertrading/off_market/fixp_result.php?l=zh-tw&d=108/10/16&_=1571412686635
    ///   11. 零股交易資訊(週日月年) https://www.tpex.org.tw/web/stock/aftertrading/odd_stock/odd_result.php?l=zh-tw&t=D&d=108/10/02&_=1571412659113    
    ///   13. 個股本益比、殖利率及股價淨值比(依代碼查詢) https://www.tpex.org.tw/web/stock/aftertrading/peratio_stk/pera_result.php?l=zh-tw&d=108/10&stkno=4401&_=1571412611642
    ///   14. 首五日無漲跌幅資訊 https://www.tpex.org.tw/web/stock/aftertrading/fifth_day_info/nolimit5_result.php?l=zh-tw&d=108/10/02&_=1571412581252
    ///   15. 變更交易、分盤交易、管理股票與停止交易資訊 https://www.tpex.org.tw/web/stock/aftertrading/cmode/chtm_result.php?l=zh-tw&o=json&d=108/10/02&_=1571412559104
    ///   16. 各券商當日營業金額統計表 https://www.tpex.org.tw/web/stock/aftertrading/daily_brk1/brk1_result.php?l=zh-tw&d=108/10/08&_=1571412538831
    ///   17. 各券商總公司當日營業金額統計表 https://www.tpex.org.tw/web/stock/aftertrading/daily_brk2/brk2_result.php?l=zh-tw&d=108/10/02&_=1571412522051
    ///   18. 等價系統成交分價表 https://www.tpex.org.tw/web/stock/aftertrading/prvol/prvol_result.php?l=zh-tw&d=108/10/02&c=02&_=1571412501826
    /// # 盤中指數及熱門資料    
    ///   1. 個股成交金額排行 https://www.tpex.org.tw/web/stock/most_active/active_dollar_vol/rt_amt_result.php?l=zh-tw&_=1571412431372    
    /// # 歷史熱門資料    
    ///   2. 個股週轉率排行(月年) https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l=zh-tw&t=D&d=108/10/01&_=1571412291433
    ///   3. 個股成交量排行(週月年) https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank_result.php?l=zh-tw&t=D&d=108/10/02&_=1571412256081
    ///   4. 個股成交值排行(月年) https://www.tpex.org.tw/web/stock/aftertrading/trading_amount/amt_rank_result.php?l=zh-tw&t=D&d=108/10/02&_=1571412223664
    ///   5. 個股日均量排行(月年) https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg_result.php?l=zh-tw&t=D&d=108/10/01&_=1571411860245
    ///   6. 個股日均值排行(月年) https://www.tpex.org.tw/web/stock/aftertrading/trading_val_avg/avg_amt_result.php?l=zh-tw&t=D&d=108/10/02&_=1571411833362    
    ///   8. 個股漲幅排行(周月) https://www.tpex.org.tw/web/stock/historical/active_advanced/rt_rally_result.php?l=zh-tw&t=D&d=108/10/01&_=1571411784118
    ///   9. 個股跌幅排行(週月) https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined_result.php?l=zh-tw&t=D&d=108/10/01&_=1571411751942        
    ///   12. 證券行情周報表 https://www.tpex.org.tw/web/stock/historical/weekly_report/wkq_result.php?o=json&l=zh-tw&d=108/10/01&_=1571411506459
    /// # 融資融券
    ///   1. 暫停融券賣出預告表 https://www.tpex.org.tw/web/stock/margin_trading/term/term_result.php?l=zh-tw&sd=108/10/01&ed=&stkno=&_=1571411467947
    ///   2. 調整成數 https://www.tpex.org.tw/web/stock/margin_trading/adjust/adjust_result.php?l=zh-tw&stkno=&_=1571411434657
    ///   3. 標借 https://www.tpex.org.tw/web/stock/margin_trading/lend/lend_result.php?l=zh-tw&sd=108/10/02&ed=108/10/18&stkno=&_=1571411388556
    ///   4. 融券借券賣出餘額 https://www.tpex.org.tw/web/stock/margin_trading/margin_sbl/margin_sbl_result.php?l=zh-tw&d=108/10/17&_=1571411355345
    ///   5. 信用交易餘額概況表 https://www.tpex.org.tw/web/stock/margin_trading/marginspot/mgratio_result.php?l=zh-tw&d=108/07/01&_=1571411327806
    ///   6. 平盤下得融借券賣出之證券名單 https://www.tpex.org.tw/web/stock/margin_trading/margin_mark/margin_mark_result.php?l=zh-tw&o=json&d=108/10/16&_=1571411302074
    ///   7. 信用交易公告 ## 以後再做
    ///   *8. 融資融券餘額表 https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal_result.php?l=zh-tw&o=json&d=108/10/17&_=1571411236323
    ///   9. 融資融券使用率報表 https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused_result.php?l=zh-tw&t=D&type=Marg&d=108/10/16&_=1571411184691
    ///   10. 融資融券增減排行表 https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l=zh-tw&t=D&type=MargGain&d=108/10/17&_=1571411135236
    /// # 三大法人
    ///   1. 三大法人買賣金額彙總表(週月年) https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l=zh-tw&t=D&p=1&d=108/10/17&_=1571411011672
    ///   2. 三大法人買賣明細資訊(週月年) https://www.tpex.org.tw/web/stock/3insti/daily_trade/3itrade_hedge_result.php?l=zh-tw&se=EW&t=D&d=108/10/16&_=1571410931472
    ///   3. 自營商買賣超彙總表(週月年) https://www.tpex.org.tw/web/stock/3insti/dealer_trading/dealtr_hedge_result.php?l=zh-tw&t=D&type=buy&d=108/10/17&_=1571410863156
    ///   4. 投信買賣超彙總表(週月年) https://www.tpex.org.tw/web/stock/3insti/sitc_trading/sitctr_result.php?l=zh-tw&t=D&type=buy&d=108/10/17&_=1571410813188
    ///   5. 外資及陸資買賣超彙總表(週月年) https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr_result.php?l=zh-tw&t=D&type=buy&d=108/10/17&_=1571410712340
    ///   6. 外資及陸資投資持股統計 XX    
    /// # 鉅額交易    
    ///   2. 個股單一證券鉅額交易日成交資訊 https://www.tpex.org.tw/web/stock/block_trade/daily_trade_infor/block_day_result.php?l=zh-tw&d=2018&stkno=&_=1571410305730
    ///   3. 鉅額交易日成交量值統計 https://www.tpex.org.tw/web/stock/block_trade/daily_trade_sum/odd_result.php?l=zh-tw&d=108/08/01&_=1571410188854
    ///   4. 鉅額交易月成交量值統計 https://www.tpex.org.tw/web/stock/block_trade/monthly_trade_sum/block_mth_result.php?l=zh-tw&d=2018&_=1571410131656
    ///   5. 鉅額交易年成交量值統計 https://www.tpex.org.tw/web/stock/block_trade/yearly_trade_sum/block_yr_result.php?l=zh-tw&d=2018&_=1571409979651
    ///   
    ///
    #endregion
}
