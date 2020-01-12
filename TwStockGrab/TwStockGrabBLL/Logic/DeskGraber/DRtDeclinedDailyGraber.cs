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
    public class DRtDeclinedDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股跌幅排行
        /// d_rt_declined_daily
        /// 本資訊自民國96年1月起開始提供 實際上由   開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DRtDeclinedDaily_Rsp rsp = JsonConvert.DeserializeObject<DRtDeclinedDaily_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }

        }

        private void SaveToDatabase(DRtDeclinedDaily_Rsp rsp, DateTime dataDate)
        {
            List<d_rt_declined_daily> tmpAddList = new List<d_rt_declined_daily>();
            List<d_rt_declined_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_rt_declined_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_rt_declined_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_rt_declined_daily
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        yesterday_close = ToDecimalQ(data.ElementAt(3).Trim()),
                        today_close = ToDecimalQ(data.ElementAt(4).Trim()),
                        down_percent = ToDecimalQ(data.ElementAt(5).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_rt_declined_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "D";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined_result.php?l=zh-tw&t=D&d=108/12/04&_=1575884043086
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/active_declined/rt_declined_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
