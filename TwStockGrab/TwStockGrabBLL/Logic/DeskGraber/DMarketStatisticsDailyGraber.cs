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
    public class DMarketStatisticsDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 > 上櫃證券成交統計(日)
        /// d_market_statistics_daily
        /// 本資訊自民國98年1月起開始提供, 實際上由 2009/01/05開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/market_statistics/statistics.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DMarketStatisticsDaily_Rsp rsp = JsonConvert.DeserializeObject<DMarketStatisticsDaily_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.detail == null || rsp.detail.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }
        }

        private void SaveToDatabase(DMarketStatisticsDaily_Rsp rsp, DateTime dataDate)
        {
            List<d_market_statistics_daily> tmpAddList = new List<d_market_statistics_daily>();
            List<d_market_statistics_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_market_statistics_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.detail)
            {
                string itemName = data.name.Trim();

                d_market_statistics_daily existItem = tmpDataList.Where(x => x.item_name == itemName && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_market_statistics_daily
                    {
                        data_date = dataDate,
                        item_name = itemName,
                        trade_money = ToDecimalQ(data.amount.Trim()),
                        trade_stock_count = ToIntQ(data.volumn.Trim()),
                        deal_count = ToIntQ(data.count.Trim()),                        
                        title = rsp.csvFile.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }


            

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_market_statistics_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string dataType = "D";

            //https://www.tpex.org.tw/web/stock/aftertrading/market_statistics/statistics_result.php?l=zh-tw&t=D&d=108/12/04
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/market_statistics/statistics_result.php?l={0}&t={1}&d={2}",
                lang, dataType, rocDate);

            return GetHttpResponse(url);
        }

        
    }
}
