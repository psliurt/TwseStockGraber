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
    public class DVolRankYearlyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股成交量排行(年)
        /// d_vol_rank_yearly
        /// 本資訊自民國96年1月起開始提供 實際上由 2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            DateTime yearFirstDay = GetYearFirstDay(dataDate);

            string responseContent = GetWebContent(yearFirstDay);
            DVolRankYearly_Rsp rsp = JsonConvert.DeserializeObject<DVolRankYearly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, yearFirstDay);
                Sleep();
            }

        }

        private void SaveToDatabase(DVolRankYearly_Rsp rsp, DateTime dataDate)
        {
            List<d_vol_rank_yearly> tmpAddList = new List<d_vol_rank_yearly>();
            List<d_vol_rank_yearly> tmpUpdateList = new List<d_vol_rank_yearly>();
            List<d_vol_rank_yearly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_vol_rank_yearly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_vol_rank_yearly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_vol_rank_yearly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_sheet_count = ToLongQ(data.ElementAt(3).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {
                    existItem.deal_sheet_count = ToLongQ(data.ElementAt(3).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_vol_rank_yearly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_vol_rank_yearly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "Y"; //yearly
            string adYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank_result.php?l=zh-tw&t=Y&d=2019&_=1578381681479
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/trading_volume/vol_rank_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, adYear, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
