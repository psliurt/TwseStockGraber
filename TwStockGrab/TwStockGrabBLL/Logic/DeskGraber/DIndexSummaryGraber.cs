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
    /// 首頁 > 上櫃 > 盤後資訊 > 上櫃股價指數收盤行情
    /// d_index_summary
    /// 本資訊自民國105年01月起開始提供   由2016/01/04開始提供    
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary.php?l=zh-tw
    /// </summary>
    public class DIndexSummaryGraber : DGraber
    {

        public DIndexSummaryGraber() : base()
        {
            this._graberClassName = typeof(DIndexSummaryGraber).Name;
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
            DIndexSummary_Rsp rsp = JsonConvert.DeserializeObject<DIndexSummary_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                WriteEndRecord(record);
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }
        }

        private void SaveToDatabase(DIndexSummary_Rsp rsp, DateTime dataDate)
        {
            List<d_index_summary> tmpAddList = new List<d_index_summary>();
            List<d_index_summary> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_index_summary>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string indexName = data.ElementAt(0).Trim();

                d_index_summary existItem = tmpDataList.Where(x => x.index_name == indexName && x.data_date == dataDate && x.index_cls == 1).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_index_summary
                    {
                        data_date = dataDate,
                        index_name = indexName,
                        index_cls = 1,
                        index_price = ToDecimalQ(data.ElementAt(1).Trim()),
                        up_down_price = ToDecimalQ(data.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(data.ElementAt(3)),                        
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }


            foreach (var mmData in rsp.mmData)
            {
                string indexName = mmData.ElementAt(0).Trim();

                d_index_summary existItem = tmpDataList.Where(x => x.index_name == indexName && x.data_date == dataDate && x.index_cls == 2).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_index_summary
                    {
                        data_date = dataDate,
                        index_name = indexName,
                        index_cls = 2,
                        index_price = ToDecimalQ(mmData.ElementAt(1).Trim()),
                        up_down_price = ToDecimalQ(mmData.ElementAt(2).Trim()),
                        up_down_percent = ToDecimalQ(mmData.ElementAt(3)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_index_summary.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary_result.php?l=zh-tw&d=108/12/05&_=1575722730242
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/index_summary/summary_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
