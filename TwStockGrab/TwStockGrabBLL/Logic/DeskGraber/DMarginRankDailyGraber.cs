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
    public class DMarginRankDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 融資融券增減排行表
        /// d_margin_rank_daily
        /// 本資訊自民國96年1月起開始提供 2007/04/23實際有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            List<string> marginTypeList = new List<string>();
            marginTypeList.Add("MargGain"); //融資增加
            marginTypeList.Add("MargLose"); //融資減少
            marginTypeList.Add("ShrtGain"); //融券增加
            marginTypeList.Add("ShrtLose"); //融券減少

            foreach (var marginType in marginTypeList)
            {
                string responseContent = GetWebContent(dataDate, marginType);
                DMarginRankDaily_Rsp rsp = JsonConvert.DeserializeObject<DMarginRankDaily_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, marginType);
                    Sleep();
                }
            }

        }

        private void SaveToDatabase(DMarginRankDaily_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_margin_rank_daily> tmpAddList = new List<d_margin_rank_daily>();
            List<d_margin_rank_daily> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_rank_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_margin_rank_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {

                    tmpAddList.Add(new d_margin_rank_daily
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        yesterday_balance = ToDecimalQ(data.ElementAt(3).Trim()),
                        today_balance = ToDecimalQ(data.ElementAt(4).Trim()),
                        total_used = ToLongQ(data.ElementAt(5).Trim()),
                        mg_type = marginType,                        
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_margin_rank_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string marginType)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();
            //https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l=zh-tw&t=D&type=MargGain&d=109/01/02&_=1578129214429
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, marginType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
