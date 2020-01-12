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
    public class DTrnMonthlyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股週轉率排行(月)
        /// d_trn_monthly
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            DateTime monthDate = new DateTime(dataDate.Year, dataDate.Month, 1);

            string responseContent = GetWebContent(monthDate);
            DTrnMonthly_Rsp rsp = JsonConvert.DeserializeObject<DTrnMonthly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, monthDate);
                Sleep();
            }
        }

        /// <summary>
        /// 這邊的dataDate在傳進來function已經做過處理，都會是每個月的第一天
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="dataDate"></param>
        private void SaveToDatabase(DTrnMonthly_Rsp rsp, DateTime dataDate)
        {
            List<d_trn_monthly> tmpAddList = new List<d_trn_monthly>();
            List<d_trn_monthly> tmpUpdateList = new List<d_trn_monthly>();
            List<d_trn_monthly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_trn_monthly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_trn_monthly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_trn_monthly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        issue_stock_count = ToLongQ(data.ElementAt(4).Trim()),
                        turnover_rate = ToDecimalQ(data.ElementAt(5).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else //已經存在，就要用update
                {
                    existItem.deal_stock_count = ToLongQ(data.ElementAt(3).Trim());
                    existItem.issue_stock_count = ToLongQ(data.ElementAt(4).Trim());
                    existItem.turnover_rate = ToDecimalQ(data.ElementAt(5).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_trn_monthly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_trn_monthly>(item).State = System.Data.Entity.EntityState.Modified;                    
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "M"; //Monthly            

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l=zh-tw&t=M&d=108/12/01&_=1578368225423
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
