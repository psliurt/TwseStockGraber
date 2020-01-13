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
    /// 首頁 > 上櫃 > 融資融券 > 融資融券增減排行表 (周)
    /// d_margin_rank_weekly
    /// 本資訊自民國96年1月起開始提供 由2007/4/28日那一周開始有資料(也就是4/23-4/28那周)
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank.php?l=zh-tw
    /// * dataDate 請一律用星期六的日期
    /// </summary>
    public class DMarginRankWeeklyGraber : DGraber
    {
        public DMarginRankWeeklyGraber() : base()
        {
            this._graberClassName = typeof(DMarginRankWeeklyGraber).Name;
            this._graberFrequency = 7;
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
                DMarginRankWeekly_Rsp rsp = JsonConvert.DeserializeObject<DMarginRankWeekly_Rsp>(responseContent);
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

        private void SaveToDatabase(DMarginRankWeekly_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_margin_rank_weekly> tmpAddList = new List<d_margin_rank_weekly>();
            List<d_margin_rank_weekly> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_margin_rank_weekly>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_margin_rank_weekly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {

                    tmpAddList.Add(new d_margin_rank_weekly
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
                context.d_margin_rank_weekly.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string marginType)
        {
            string lang = "zh-tw";
            string dataType = "W"; //weekly
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();
            //https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l=zh-tw&t=W&type=MargGain&d=108/12/23&_=1578129479301
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/short_sell/margin_rank_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, marginType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
