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
    /// 首頁 > 上櫃 > 歷史熱門資料 > 個股跌幅排行
    /// d_rt_declined_weekly
    /// 本資訊自民國96年1月起開始提供 實際上由   開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined.php?l=zh-tw
    /// </summary>
    public class DRtDeclinedWeeklyGraber : DGraber
    {

        public DRtDeclinedWeeklyGraber() : base()
        {
            this._graberClassName = typeof(DRtDeclinedWeeklyGraber).Name;
            this._graberFrequency = 7;
        }
        
        public override void DoJob(DateTime dataDate)
        {
            DateTime weekDate = GetWeekMondayDate(dataDate);

            work_record record = null;
            if (GetOrCreateWorkRecord(weekDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(weekDate);
            DRtDeclinedWeekly_Rsp rsp = JsonConvert.DeserializeObject<DRtDeclinedWeekly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, weekDate);
                WriteEndRecord(record);
                Sleep();
            }

        }

        private void SaveToDatabase(DRtDeclinedWeekly_Rsp rsp, DateTime dataDate)
        {
            List<d_rt_declined_weekly> tmpAddList = new List<d_rt_declined_weekly>();
            List<d_rt_declined_weekly> tmpUpdateList = new List<d_rt_declined_weekly>();
            List<d_rt_declined_weekly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_rt_declined_weekly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_rt_declined_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_rt_declined_weekly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),                        
                        down_percent = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {                    
                    existItem.down_percent = ToDecimalQ(data.ElementAt(3).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_rt_declined_weekly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_rt_declined_weekly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "W"; //weekly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined_result.php?l=zh-tw&t=W&d=109/01/02&_=1578494640947
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
