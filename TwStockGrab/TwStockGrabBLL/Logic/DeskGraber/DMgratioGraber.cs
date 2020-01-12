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
    public class DMgratioGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 信用交易餘額概況表
        /// d_mgratio
        /// 本資訊自民國96年1月起開始提供  2007/5月實際有資料
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/marginspot/mgratio.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DMgratio_Rsp rsp = JsonConvert.DeserializeObject<DMgratio_Rsp>(responseContent);
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

        private void SaveToDatabase(DMgratio_Rsp rsp, DateTime dataDate)
        {
            DateTime dataMonth = new DateTime(dataDate.Year, dataDate.Month, 1);

            List<d_mgratio> tmpAddList = new List<d_mgratio>();
            List<d_mgratio> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_mgratio>().AsNoTracking().Where(x => x.data_date == dataMonth).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_mgratio existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataMonth && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {

                    tmpAddList.Add(new d_mgratio
                    {
                        data_date = dataMonth,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        margin_avg_money = ToLongQ(data.ElementAt(3).Trim()),
                        margin_market_percent = ToDecimalQ(data.ElementAt(4).Trim()),
                        lend_avg_money = ToLongQ(data.ElementAt(5).Trim()),
                        lend_market_percent = ToDecimalQ(data.ElementAt(6).Trim()),
                        avg_money = ToLongQ(data.ElementAt(7).Trim()),
                        market_percent = ToDecimalQ(data.ElementAt(8).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_mgratio.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";            
            string rocDate = ParseADDateToRocString(date);            
            string paramUnderLine = GetTimeStamp();
            //https://www.tpex.org.tw/web/stock/margin_trading/marginspot/mgratio_result.php?l=zh-tw&d=108/11/01&_=1578125875599
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/marginspot/mgratio_result.php?l={0}&d={1}&_={2}",
                lang, rocDate,  paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
