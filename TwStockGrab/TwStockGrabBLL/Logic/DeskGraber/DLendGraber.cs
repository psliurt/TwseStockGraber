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
    public class DLendGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 標借
        /// d_lend
        /// 本資訊自民國92年8月起開始提供 2007/1月開始有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/lend/lend.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DLend_Rsp rsp = JsonConvert.DeserializeObject<DLend_Rsp>(responseContent);
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

        private void SaveToDatabase(DLend_Rsp rsp, DateTime dataDate)
        {
            List<d_lend> tmpAddList = new List<d_lend>();
            List<d_lend> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_lend>().AsNoTracking().ToList();
            }

            foreach (var data in rsp.aaData)
            {
                DateTime lendDate = GetDateFromRocSlashStringQ(data.ElementAt(0).Trim()).Value.Date;
                string stockNo = data.ElementAt(1).Trim();

                d_lend existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == lendDate).FirstOrDefault();
                if (existItem == null)
                {
                    decimal lendFloor = 0;
                    decimal lendCeil = 0;
                    ParseFloorAndCeil(data.ElementAt(7).Trim(), ref lendFloor, ref lendCeil);
                    tmpAddList.Add(new d_lend
                    {
                        data_date = lendDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        stock_agent = data.ElementAt(3).Trim(),
                        lend_cnt = ToIntQ(data.ElementAt(4).Trim()),
                        lend_max_price = ToDecimalQ(data.ElementAt(5).Trim()),
                        lend_success_cnt = ToIntQ(data.ElementAt(6).Trim()),
                        lend_floor = lendFloor,
                        lend_ceil = lendCeil,
                        lend_fail_cnt = ToIntQ(data.ElementAt(8).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_lend.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";            
            DateTime endDate = date.AddMonths(1).AddDays(-1);
            string rocStartDate = ParseADDateToRocString(date);
            string rocEndDate = ParseADDateToRocString(endDate);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/lend/lend_result.php?l=zh-tw&sd=99/01/01&ed=109/01/04&stkno=&_=1578116199212
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/lend/lend_result.php?l={0}&sd={1}&ed={2}&stkno=&_={3}",
                lang,  rocStartDate, rocEndDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
