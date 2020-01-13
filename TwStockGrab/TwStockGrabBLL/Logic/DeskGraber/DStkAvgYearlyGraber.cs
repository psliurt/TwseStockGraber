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
    /// d_stk_avg_yearly
    /// 本資訊自民國96年1月起開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg.php?l=zh-tw
    /// </summary>
    public class DStkAvgYearlyGraber : DGraber
    {
        public DStkAvgYearlyGraber() : base()
        {
            this._graberClassName = typeof(DStkAvgYearlyGraber).Name;
            this._graberFrequency = 365;
        }

        public override void DoJob(DateTime dataDate)
        {
            DateTime yearDate = GetYearFirstDay(dataDate);

            work_record record = null;
            if (GetOrCreateWorkRecord(yearDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(yearDate);
            DStkAvgMonthly_Rsp rsp = JsonConvert.DeserializeObject<DStkAvgMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, yearDate);
                WriteEndRecord(record);
                Sleep();
            }

        }

        private void SaveToDatabase(DStkAvgMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_stk_avg_yearly> tmpAddList = new List<d_stk_avg_yearly>();
            List<d_stk_avg_yearly> tmpUpdateList = new List<d_stk_avg_yearly>();
            List<d_stk_avg_yearly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_stk_avg_yearly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_stk_avg_yearly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.rank_order == rankOrder && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_avg_yearly
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
                context.d_stk_avg_yearly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_stk_avg_yearly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "Y"; //yearly
            string adYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg_result.php?l=zh-tw&t=Y&d=2019&_=1578492490096
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/trading_volumes_avg/stk_avg_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, adYear, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
