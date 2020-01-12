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
    public class DBlockMthMonthlyGraber : DGraber
    {
        /// <summary>
        /// TODO:可能有辦法調整有關於日期的計算方式，還有資料庫資料可能也可以加上update的模式
        /// 首頁 > 上櫃 > 鉅額交易 > 鉅額交易月成交量值統計
        /// d_block_mth_monthly
        /// 本資訊自民國96年1月起開始提供 實際由 2007/1月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/block_trade/monthly_trade_sum/block_mth.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            DBlockMthMonthly_Rsp rsp = JsonConvert.DeserializeObject<DBlockMthMonthly_Rsp>(responseContent);
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

        private void SaveToDatabase(DBlockMthMonthly_Rsp rsp, DateTime dataDate)
        {
            DateTime firstMonth = new DateTime(dataDate.Year, 1, 1);
            DateTime lastMonth = firstMonth.AddYears(1).AddMonths(-1);

            List<d_block_mth_monthly> tmpAddList = new List<d_block_mth_monthly>();
            List<d_block_mth_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_block_mth_monthly>().AsNoTracking().Where(x => x.data_date >= firstMonth && x.data_date <= lastMonth).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string recordMonthStr = data.ElementAt(0).Trim();
                DateTime recordDataDate = GetDateMonthFromRocStringQ(recordMonthStr).Value;
                string tradeType = data.ElementAt(1).Trim();

                d_block_mth_monthly existItem = tmpDataList.Where(x => x.data_date == recordDataDate && x.trade_type == tradeType).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_block_mth_monthly
                    {
                        data_date = recordDataDate,
                        trade_type = tradeType,
                        deal_cnt = ToIntQ(data.ElementAt(2).Trim()),
                        deal_stock_cnt = ToDecimalQ(data.ElementAt(3).Trim()),
                        deal_percent = ToDecimalQ(data.ElementAt(4).Trim()),
                        deal_money = ToDecimalQ(data.ElementAt(5).Trim()),
                        deal_money_percent = ToDecimalQ(data.ElementAt(6).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_block_mth_monthly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dateYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/block_trade/monthly_trade_sum/block_mth_result.php?l=zh-tw&d=2019&_=1578014596461
            string url = string.Format("https://www.tpex.org.tw/web/stock/block_trade/monthly_trade_sum/block_mth_result.php?l={0}&d={1}&_={2}",
                lang, dateYear, paramUnderLine);

            return GetHttpResponse(url);
        }
    }
}
