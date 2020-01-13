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
    /// 首頁 > 上櫃 > 融資融券 > 融券借券賣出餘額
    /// d_margin_sbl
    /// 本資訊自民國95年1月起開始提供 實際由2012/10/02開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/block_trade/daily_qutoes/block_day.php?l=zh-tw
    /// </summary>
    public class DMarginSblGraber : DGraber
    {
        public DMarginSblGraber() : base()
        {
            this._graberClassName = typeof(DMarginSblGraber).Name;
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
            DMarginSbl_Rsp rsp = JsonConvert.DeserializeObject<DMarginSbl_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
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

        private void SaveToDatabase(DMarginSbl_Rsp rsp, DateTime dataDate)
        {
            List<d_margin_sbl> tmpAddList = new List<d_margin_sbl>();
            List<d_margin_sbl> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_sbl>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_margin_sbl existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_margin_sbl
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        margin_yesterday_balance = ToDecimalQ(data.ElementAt(2).Trim()),
                        margin_sell = ToDecimalQ(data.ElementAt(3)),
                        margin_buy = ToDecimalQ(data.ElementAt(4)),
                        margin_back = ToDecimalQ(data.ElementAt(5)),
                        margin_today_balance = ToDecimalQ(data.ElementAt(6)),
                        margin_limit = ToDecimalQ(data.ElementAt(7)),
                        lend_yesterday_balance = ToDecimalQ(data.ElementAt(8)),
                        lend_sell = ToDecimalQ(data.ElementAt(9)),
                        lend_back = ToDecimalQ(data.ElementAt(10)),
                        lend_adjust = ToDecimalQ(data.ElementAt(11)),
                        lend_today_balance = ToDecimalQ(data.ElementAt(12)),
                        lend_next_remain_limit = ToDecimalQ(data.ElementAt(13)),                        
                        note = data.ElementAt(14).Trim(),
                        title = string.Format("{0} 融券借券賣出餘額", rsp.reportDate),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });


                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_margin_sbl.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";           

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/margin_sbl/margin_sbl_result.php?l=zh-tw&d=108/12/20&_=1577157362696
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/margin_sbl/margin_sbl_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
