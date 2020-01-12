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
    public class DMktGraber  : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股市值排行
        /// d_mkt
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/aftertrading/daily_mktval/mkt.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            
            string responseContent = GetWebContent(dataDate);
            DMkt_Rsp rsp = JsonConvert.DeserializeObject<DMkt_Rsp>(responseContent);
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

        private void SaveToDatabase(DMkt_Rsp rsp, DateTime dataDate)
        {
            List<d_mkt> tmpAddList = new List<d_mkt>();
            List<d_mkt> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_mkt>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_mkt existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_mkt
                    {
                        data_date = dataDate,
                        rank = ToInt(data.ElementAt(0).Trim()),
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        issue_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        close_price = ToDecimalQ(data.ElementAt(4).Trim()),
                        market_value = ToDecimalQ(data.ElementAt(5).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_mkt.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_mktval/mkt_result.php?l=zh-tw&d=108/12/04&_=1575796575034
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_mktval/mkt_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
