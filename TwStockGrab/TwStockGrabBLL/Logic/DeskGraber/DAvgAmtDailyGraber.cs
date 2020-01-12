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
    /// 首頁 > 上櫃 > 盤後資訊 >歷史熱門資料 > 個股日均值排行
    /// d_avg_amt_daily
    /// 本資訊自民國96年1月起開始提供 實際上由 2007/1/2  開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/trading_val_avg/avg_amt.php?l=zh-tw
    /// </summary>
    public class DAvgAmtDailyGraber : DGraber
    {
        public DAvgAmtDailyGraber() : base()
        {
            this._graberClassName = typeof(DAvgAmtDailyGraber).Name;
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
            DAvgAmtDaily_Rsp rsp = JsonConvert.DeserializeObject<DAvgAmtDaily_Rsp>(responseContent);
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

        private void SaveToDatabase(DAvgAmtDaily_Rsp rsp, DateTime dataDate)
        {
            List<d_avg_amt_daily> tmpAddList = new List<d_avg_amt_daily>();
            List<d_avg_amt_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_avg_amt_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_avg_amt_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_avg_amt_daily
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        avg_value = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_avg_amt_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "D";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/trading_val_avg/avg_amt_result.php?l=zh-tw&t=D&d=108/12/04&_=1575881095833v
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/trading_val_avg/avg_amt_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
