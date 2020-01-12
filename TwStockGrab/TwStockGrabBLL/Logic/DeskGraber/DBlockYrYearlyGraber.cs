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
    /// 首頁 > 上櫃 > 鉅額交易 > 鉅額交易月成交量值統計
    /// d_block_yr_yearly
    /// 本資訊自民國96年1月起開始提供 實際由2007開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/block_trade/yearly_trade_sum/block_yr.php?l=zh-tw
    /// </summary>
    public class DBlockYrYearlyGraber : DGraber
    {
        public DBlockYrYearlyGraber() : base()
        {
            this._graberClassName = typeof(DBlockYrYearlyGraber).Name;
            this._graberFrequency = 365;
        }

        public override void DoJob(DateTime dataDate)
        {
            DateTime yearFirstDay = GetYearFirstDay(dataDate);

            work_record record = null;
            if (GetOrCreateWorkRecord(yearFirstDay, out record))
            {
                return;
            }

            string responseContent = GetWebContent(yearFirstDay);
            DBlockMthMonthly_Rsp rsp = JsonConvert.DeserializeObject<DBlockMthMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, yearFirstDay);
                WriteEndRecord(record);
                Sleep();
            }
        }

        /// <summary>
        /// 這邊的dataDate已經是被處理過的日期資料，只會是每年的1月1日的日期
        /// ex 2019/01/01
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="dataDate"></param>
        private void SaveToDatabase(DBlockMthMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_block_yr_yearly> tmpAddList = new List<d_block_yr_yearly>();
            List<d_block_yr_yearly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_block_yr_yearly>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string recordYearStr = data.ElementAt(0).Trim();                
                string tradeType = data.ElementAt(1).Trim();

                d_block_yr_yearly existItem = tmpDataList.Where(x => x.data_date == dataDate && x.trade_type == tradeType).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_block_yr_yearly
                    {
                        data_date = dataDate,
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
                context.d_block_yr_yearly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dateYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/block_trade/yearly_trade_sum/block_yr_result.php?l=zh-tw&d=2019&_=1578014880251
            string url = string.Format("https://www.tpex.org.tw/web/stock/block_trade/yearly_trade_sum/block_yr_result.php?l={0}&d={1}&_={2}",
                lang, dateYear, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
