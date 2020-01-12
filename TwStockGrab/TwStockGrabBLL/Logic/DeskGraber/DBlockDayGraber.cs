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
    /// 首頁 > 上櫃 > 鉅額交易 > 鉅額交易日成交資訊
    /// d_block_day
    /// 本資訊自民國96年1月起開始提供 實際由2007/1/8開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/block_trade/daily_qutoes/block_day.php?l=zh-tw
    /// </summary>
    public class DBlockDayGraber : DGraber
    {
        public DBlockDayGraber() : base()
        {
            this._graberClassName = typeof(DBlockDayGraber).Name;
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
            DBlockDay_Rsp rsp = JsonConvert.DeserializeObject<DBlockDay_Rsp>(responseContent);
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

        private void SaveToDatabase(DBlockDay_Rsp rsp, DateTime dataDate)
        {   

            List<d_block_day> tmpAddList = new List<d_block_day>();
            List<d_block_day> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_block_day>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string tradeItem = data.ElementAt(0).Trim();
                string tradePeriod = data.ElementAt(1).Trim();
                string stockNo = data.ElementAt(2).Trim();

                d_block_day existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.trade_item == tradeItem && x.trade_period == tradePeriod).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_block_day
                    {
                        data_date = dataDate,
                        trade_item = tradeItem,
                        trade_period = tradePeriod,
                        stock_no = stockNo,                        
                        stock_name = data.ElementAt(3).Trim(),
                        deal_price = ToDecimalQ(data.ElementAt(4).Trim()),
                        deal_stock_count = ToLongQ(data.ElementAt(5).Trim()),
                        deal_value = ToDecimalQ(data.ElementAt(6).Trim()),
                        deal_time = ToTimeQ(dataDate, data.ElementAt(7).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_block_day.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/block_trade/daily_qutoes/block_day_result.php?l=zh-tw&d=108/12/04&_=1575794828398
            string url = string.Format("https://www.tpex.org.tw/web/stock/block_trade/daily_qutoes/block_day_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
