using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic.Rsp.Json.Desk;
using TwStockGrabBLL.DAL;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    /// <summary>
    /// 首頁 > 上櫃 > 三大法人 > 三大法人買賣金額彙總表(月)
    /// d_3itrdsum_monthly
    /// 本資訊自民國96年1月起開始提供
    /// 民國93年6月至95年12月資訊由下面的網址查詢
    /// https://hist.tpex.org.tw/Hist/STOCK/3INSTI/3INSTISUM.HTML
    /// 
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum.php?l=zh-tw
    /// </summary>
    public class D3itrdsumMonthlyGraber : DGraber
    {
        public D3itrdsumMonthlyGraber() : base()
        {
            this._graberClassName = typeof(D3itrdsumMonthlyGraber).Name;
            this._graberFrequency = 30;
        }
        
        public override void DoJob(DateTime dataDate)
        {
            DateTime monthFirstDate = GetMonthFirstDay(dataDate);

            work_record record = null;
            if (GetOrCreateWorkRecord(monthFirstDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(monthFirstDate);
            D3itrdsumMonthly_Rsp rsp = JsonConvert.DeserializeObject<D3itrdsumMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, monthFirstDate);
                WriteEndRecord(record);
                Sleep();
            }
        }

        private void SaveToDatabase(D3itrdsumMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_3itrdsum_monthly> tmpAddList = new List<d_3itrdsum_monthly>();
            List<d_3itrdsum_monthly> tmpUpdateList = new List<d_3itrdsum_monthly>();
            List<d_3itrdsum_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_3itrdsum_monthly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string itemName = data.ElementAt(0).Trim();

                d_3itrdsum_monthly existItem = tmpDataList.Where(x => x.item_name == itemName && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_3itrdsum_monthly
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
                else
                {
                    existItem.buy_in_money = ToDecimalQ(data.ElementAt(1));
                    existItem.sell_out_money = ToDecimalQ(data.ElementAt(2));
                    existItem.diff_money = ToDecimalQ(data.ElementAt(3));
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_3itrdsum_monthly.AddRange(tmpAddList);
                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_3itrdsum_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "M"; //monthly
            string stockSelectType = "1";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            ///https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l=zh-tw&t=M&p=1&d=108/12/01&_=1578804004561
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/3insti_summary/3itrdsum_result.php?l={0}&t={1}&p={2}&d={3}&_={4}",
                lang, dataType, stockSelectType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
