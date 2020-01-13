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
    /// 首頁 > 上櫃 > 盤後資訊 >   歷史熱門資料 > 個股日均量排行
    /// d_stk_avg_monthly
    /// 本資訊自民國96年1月起開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg.php?l=zh-tw
    /// </summary>
    public class DStkAvgMonthlyGraber : DGraber
    {
        public DStkAvgMonthlyGraber() : base()
        {
            this._graberClassName = typeof(DStkAvgMonthlyGraber).Name;
            this._graberFrequency = 30;
        }

        public override void DoJob(DateTime dataDate)
        {
            DateTime monthDate = GetMonthFirstDay(dataDate);

            work_record record = null;
            if (GetOrCreateWorkRecord(monthDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(monthDate);
            DStkAvgMonthly_Rsp rsp = JsonConvert.DeserializeObject<DStkAvgMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, monthDate);
                WriteEndRecord(record);
                Sleep();
            }

        }

        private void SaveToDatabase(DStkAvgMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_stk_avg_monthly> tmpAddList = new List<d_stk_avg_monthly>();
            List<d_stk_avg_monthly> tmpUpdateList = new List<d_stk_avg_monthly>();
            List<d_stk_avg_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_stk_avg_monthly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_stk_avg_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_avg_monthly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        avg_count = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
                else
                {
                    existItem.avg_count = ToDecimalQ(data.ElementAt(3).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_stk_avg_monthly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_stk_avg_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "M";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg_result.php?l=zh-tw&t=M&d=108/12/01&_=1578491806737
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
