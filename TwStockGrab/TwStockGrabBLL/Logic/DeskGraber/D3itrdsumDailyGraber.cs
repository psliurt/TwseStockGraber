using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic.Rsp.Json.Desk;
using TwStockGrabBLL.DAL;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    /// <summary>
    /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣金額彙總表
    /// d_3itrdsum_daily
    /// 本資訊自民國96年1月起開始提供，但實際上 從106/1/3日才有資料
    /// 民國93年6月至95年12月資訊由下面的網址查詢
    /// https://hist.tpex.org.tw/Hist/STOCK/3INSTI/3INSTISUM.HTML
    /// 
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum.php?l=zh-tw
    /// </summary>
    public class D3itrdsumDailyGraber : DGraber
    {
        public D3itrdsumDailyGraber() : base()
        {
            this._graberClassName = typeof(D3itrdsumDailyGraber).Name;
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
            D3itrdsumDaily_Rsp rsp = JsonConvert.DeserializeObject<D3itrdsumDaily_Rsp>(responseContent);
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

        private void SaveToDatabase(D3itrdsumDaily_Rsp rsp, DateTime dataDate)
        {
            List<d_3itrdsum_daily> tmpAddList = new List<d_3itrdsum_daily>();
            List<d_3itrdsum_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrdsum_daily>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string itemName = data.ElementAt(0).Trim();

                d_3itrdsum_daily existItem = tmpDataList.Where(x => x.item_name == itemName && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_3itrdsum_daily
                    {
                        data_date = dataDate,
                        item_name = itemName,
                        buy_in_money = ToDecimalQ(data.ElementAt(1)),
                        sell_out_money = ToDecimalQ(data.ElementAt(2)),
                        diff_money = ToDecimalQ(data.ElementAt(3)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrdsum_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily
            string stockSelectType = "1";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l=zh-tw&t=D&p=1&d=108/11/07&_=1573283083866
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l={0}&t={1}&p={2}&d={3}&_={4}",
                lang, dataType, stockSelectType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
