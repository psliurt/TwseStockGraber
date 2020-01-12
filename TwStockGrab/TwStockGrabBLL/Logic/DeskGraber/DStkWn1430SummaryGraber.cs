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
    public class DStkWn1430SummaryGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 盤後資訊 > 上櫃證券成交統計
        /// d_stk_wn1430_summary
        /// 本資訊自民國96年7月起開始提供 2012/05/29開始有資料，這是把select type為OR的資料選項特別獨立出來，因為他的資料結構跟其他選項不同
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            
            string responseContent = GetWebContent(dataDate);
            DStkWn1430Summary_Rsp rsp = JsonConvert.DeserializeObject<DStkWn1430Summary_Rsp>(responseContent);
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

        private void SaveToDatabase(DStkWn1430Summary_Rsp rsp, DateTime dataDate)
        {
            List<d_stk_wn1430_summary> tmpAddList = new List<d_stk_wn1430_summary>();
            List<d_stk_wn1430_summary> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_stk_wn1430_summary>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string summaryItem = data.ElementAt(0).Trim();

                d_stk_wn1430_summary existItem = tmpDataList.Where(x => x.sum_item == summaryItem && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_wn1430_summary
                    {
                        data_date = dataDate,
                        sum_item = summaryItem,                        
                        all_market = ToDecimalQ(data.ElementAt(1).Trim()),
                        stock = ToDecimalQ(data.ElementAt(2).Trim()),
                        fund = ToDecimalQ(data.ElementAt(3).Trim()),
                        buy_warrant = ToDecimalQ(data.ElementAt(4).Trim()),
                        sale_warrant = ToDecimalQ(data.ElementAt(5).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_stk_wn1430_summary.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string st = "OR";
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l=zh-tw&d=108/12/04&se=OR&_=1575784680087
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l={0}&d={1}&se={2}&_={3}",
                lang, rocDate, st, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
