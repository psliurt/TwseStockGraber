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
    /// 首頁 > 上櫃 > 盤後資訊 > 漲跌停未成交資訊
    /// d_ceil_ord
    /// 本資訊自民國96年1月起開始提供  實際上由 2007/04/23開始有資料
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/ceil_non_trading/ceil_ord.php?l=zh-tw
    /// </summary>
    public class DCeilOrdGraber : DGraber
    {
        public DCeilOrdGraber() : base()
        {
            this._graberClassName = typeof(DCeilOrdGraber).Name;
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
            DCeilOrd_Rsp rsp = JsonConvert.DeserializeObject<DCeilOrd_Rsp>(responseContent);
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

        private void SaveToDatabase(DCeilOrd_Rsp rsp, DateTime dataDate)
        {
            List<d_ceil_ord> tmpAddList = new List<d_ceil_ord>();
            List<d_ceil_ord> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_ceil_ord>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_ceil_ord existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_ceil_ord
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        close_price = ToDecimalQ(data.ElementAt(2).Trim()),
                        up_down_price = ToDecimalQ(data.ElementAt(3).Trim()),
                        deal_cnt = ToLongQ(data.ElementAt(4).Trim()),
                        ceil_floor_deal_cnt = ToLongQ(data.ElementAt(5).Trim()),
                        ceil_floor_ask_cnt = ToLongQ(data.ElementAt(6).Trim()),
                        ceil_floor_no_deal_cnt = ToLongQ(data.ElementAt(7).Trim()),                        
                        title = string.Format("Date: {0}, RecordCount: {1}", rsp.reportDate.Trim(), rsp.iTotalRecords),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_ceil_ord.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/ceil_non_trading/ceil_ord_result.php?l=zh-tw&d=109/01/02&_=1578222442345
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/ceil_non_trading/ceil_ord_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }
    }
}
