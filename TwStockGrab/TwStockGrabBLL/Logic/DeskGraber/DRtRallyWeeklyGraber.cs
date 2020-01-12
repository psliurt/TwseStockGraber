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
    public class DRtRallyWeeklyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 >歷史熱門資料 > 個股漲幅排行
        /// d_rt_rally_weekly
        /// 本資訊自民國96年1月起開始提供 
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/active_advanced/rt_rally.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            DateTime weekDate = GetWeekMondayDate(dataDate);

            string responseContent = GetWebContent(weekDate);
            DRtRallyWeekly_Rsp rsp = JsonConvert.DeserializeObject<DRtRallyWeekly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, weekDate);
                Sleep();
            }

        }

        private void SaveToDatabase(DRtRallyWeekly_Rsp rsp, DateTime dataDate)
        {
            List<d_rt_rally_weekly> tmpAddList = new List<d_rt_rally_weekly>();
            List<d_rt_rally_weekly> tmpUpdateList = new List<d_rt_rally_weekly>();
            List<d_rt_rally_weekly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_rt_rally_weekly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_rt_rally_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_rt_rally_weekly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),                        
                        up_percent = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
                else
                {
                    existItem.update_at = DateTime.Now;                    
                    existItem.up_percent = ToDecimalQ(data.ElementAt(3).Trim());

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_rt_rally_weekly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_rt_rally_weekly>(item).State = System.Data.Entity.EntityState.Modified;
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

            //https://www.tpex.org.tw/web/stock/historical/active_advanced/rt_rally_result.php?l=zh-tw&t=W&d=109/01/02&_=1578494559092
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/active_advanced/rt_rally_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
