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
    /// 首頁 > 上櫃 > 融資融券 > 上櫃股票融資融券餘額
    /// d_margin_bal
    /// 本資訊自民國96年1月起開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal.php?l=zh-tw
    /// </summary>
    public class DMarginBalGraber : DGraber
    {
        public DMarginBalGraber() : base()
        {
            this._graberClassName = typeof(DMarginBalGraber).Name;
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
            DMarginBal_Rsp rsp = JsonConvert.DeserializeObject<DMarginBal_Rsp>(responseContent);
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

        private void SaveToDatabase(DMarginBal_Rsp rsp, DateTime dataDate)
        {
            List<d_margin_bal> tmpAddList = new List<d_margin_bal>();
            List<d_margin_bal> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_bal>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_margin_bal existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_margin_bal
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        yesterday_lend_balance = ToIntQ(data.ElementAt(2).Trim()),                        
                        lend_buy = ToIntQ(data.ElementAt(3)),
                        lend_sell = ToIntQ(data.ElementAt(4)),
                        lend_back = ToIntQ(data.ElementAt(5)),
                        lend_balance = ToIntQ(data.ElementAt(6)),
                        lend_margin = ToIntQ(data.ElementAt(7)),
                        lend_percent = ToDecimalQ(data.ElementAt(8)),
                        lend_limit = ToDecimalQ(data.ElementAt(9)),
                        yesterday_borrow_balance = ToIntQ(data.ElementAt(10)),
                        borrow_sell = ToIntQ(data.ElementAt(11)),
                        borrow_buy = ToIntQ(data.ElementAt(12)),
                        borrow_back = ToIntQ(data.ElementAt(13)),
                        borrow_balance = ToIntQ(data.ElementAt(14)),
                        borrow_margin = ToIntQ(data.ElementAt(15)),
                        borrow_percent = ToDecimalQ(data.ElementAt(16)),
                        borrow_limit = ToDecimalQ(data.ElementAt(17)),
                        offset  =  ToIntQ(data.ElementAt(18)),
                        note =  data.ElementAt(19).Trim(),
                        title = string.Format("{0} 融資融券餘額表", rsp.reportDate),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });


                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_margin_bal.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string responseDataType = "json";            

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal_result.php?l=zh-tw&o=json&d=108/11/07&_=1573477027219
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/margin_balance/margin_bal_result.php?l={0}&o={1}&d={2}&_={3}",
                lang, responseDataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
