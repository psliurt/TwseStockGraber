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
    /// 首頁 > 上櫃 > 歷史熱門資料 > 個股週轉率排行
    /// d_trn_daily
    /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn.php?l=zh-tw
    /// </summary>
    public class DTrnDailyGraber : DGraber
    {
        public DTrnDailyGraber() : base()
        {
            this._graberClassName = typeof(DTrnDailyGraber).Name;
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
            DTrnDaily_Rsp rsp = JsonConvert.DeserializeObject<DTrnDaily_Rsp>(responseContent);
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

        private void SaveToDatabase(DTrnDaily_Rsp rsp, DateTime dataDate)
        {
            List<d_trn_daily> tmpAddList = new List<d_trn_daily>();
            List<d_trn_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_trn_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_trn_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_trn_daily
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        issue_stock_count = ToLongQ(data.ElementAt(4).Trim()),
                        turnover_rate = ToDecimalQ(data.ElementAt(5).Trim()),                        
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_trn_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l=zh-tw&t=D&d=108/12/04&_=1575817311526
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
