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
    /// 首頁 > 上櫃 > 融資融券 > 融資融券增減排行表 (月)
    /// d_margin_rank_monthly
    /// 本資訊自民國96年1月起開始提供 實際由2007/4/1日那個月開始有資料
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank.php?l=zh-tw
    /// * dataDate 請一律用每個月1號
    /// </summary>
    public class DMarginRankMonthlyGraber : DGraber
    {

        public DMarginRankMonthlyGraber() : base()
        {
            this._graberClassName = typeof(DMarginRankMonthlyGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }

            List<string> marginTypeList = new List<string>();
            marginTypeList.Add("MargGain"); //融資增加
            marginTypeList.Add("MargLose"); //融資減少
            marginTypeList.Add("ShrtGain"); //融券增加
            marginTypeList.Add("ShrtLose"); //融券減少

            foreach (var marginType in marginTypeList)
            {
                string responseContent = GetWebContent(dataDate, marginType);
                DMarginRankMonthly_Rsp rsp = JsonConvert.DeserializeObject<DMarginRankMonthly_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    WriteEndRecord(record);
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, marginType);
                    WriteEndRecord(record);
                    Sleep();
                }
            }

        }

        private void SaveToDatabase(DMarginRankMonthly_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_margin_rank_monthly> tmpAddList = new List<d_margin_rank_monthly>();
            List<d_margin_rank_monthly> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_rank_monthly>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_margin_rank_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {

                    tmpAddList.Add(new d_margin_rank_monthly
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
                context.d_margin_rank_monthly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string marginType)
        {
            string lang = "zh-tw";
            string dataType = "M"; //weekly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();
            //https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l=zh-tw&t=M&type=MargGain&d=108/12/01&_=1578131180044
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, marginType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
