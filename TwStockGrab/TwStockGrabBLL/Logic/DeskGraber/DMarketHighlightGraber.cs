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
    /// 首頁 > 上櫃 > 盤後資訊 > 上櫃股票市場現況
    /// d_market_highlight
    /// 本資訊自民國96年01月起開始提供  實際上從 2007/04/23開始有資料
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight.php?l=zh-tw
    /// </summary>
    public class DMarketHighlightGraber : DGraber
    {
        public DMarketHighlightGraber() : base()
        {
            this._graberClassName = typeof(DMarketHighlightGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(dataDate);
            DMarketHighlight_Rsp rsp = JsonConvert.DeserializeObject<DMarketHighlight_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                WriteEndRecord(record);
                Sleep();
            }
        }

        private void SaveToDatabase(DMarketHighlight_Rsp rsp, DateTime dataDate)
        {
            List<d_market_highlight> tmpAddList = new List<d_market_highlight>();
            List<d_market_highlight> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_market_highlight>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            if (tmpDataList.Count() <= 0)
            {
                tmpAddList.Add(new d_market_highlight
                {
                    data_date = dataDate,
                    list_stock_count = ToIntQ(rsp.listedNum),
                    total_capital = ToDecimalQ(rsp.capital),
                    total_value = ToDecimalQ(rsp.companyValue),
                    today_trade_value = ToDecimalQ(rsp.tradeAmount),
                    today_trade_stock_count = ToDecimalQ(rsp.tradeVolumn),
                    close_index_price = ToDecimalQ(rsp.close),
                    up_count = ToIntQ(rsp.upNum),
                    down_count = ToIntQ(rsp.downNum),
                    no_change_count = ToIntQ(rsp.noChangeNum),
                    index_up_down_price = ToDecimalQ(rsp.change),
                    up_limit_count = ToIntQ(rsp.upStopNum),
                    down_limit_count = ToIntQ(rsp.downStopNum),
                    no_trade_count = ToIntQ(rsp.noTradeNum),
                    title = rsp.rptNote.Trim(),
                    create_at = DateTime.Now,
                    update_at = DateTime.Now
                });
            }

            

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_market_highlight.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);           

            //https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight_result.php?l=zh-tw&d=108/12/04
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/market_highlight/highlight_result.php?l={0}&d={1}",
                lang, rocDate);

            return GetHttpResponse(url);
        }

    }
}
