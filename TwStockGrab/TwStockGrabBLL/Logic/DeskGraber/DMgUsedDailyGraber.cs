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
    public class DMgUsedDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 融資融券 > 融資融券使用率報表
        /// d_mgused_daily
        /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            List<string> sideList = new List<string>();
            sideList.Add("Marg"); //融資
            sideList.Add("Shrt"); //融券

            foreach (var side in sideList)
            {
                string responseContent = GetWebContent(dataDate, side);
                DMgUsedDaily_Rsp rsp = JsonConvert.DeserializeObject<DMgUsedDaily_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, side);
                    Sleep();
                }
            }            
        }

        private void SaveToDatabase(DMgUsedDaily_Rsp rsp, DateTime dataDate, string marginType)
        {
            List<d_mgused_daily> tmpAddList = new List<d_mgused_daily>();
            List<d_mgused_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_mgused_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.mg_type == marginType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_mgused_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.mg_type == marginType && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_mgused_daily
                    {
                        data_date = dataDate,
                        mg_type = marginType,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        used_percent = ToDecimalQ(data.ElementAt(3).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_mgused_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string sideType)
        {
            string lang = "zh-tw";
            string freq = "D";

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused_result.php?l=zh-tw&t=D&type=Shrt&d=108/12/20&_=1577164318712
            string url = string.Format("https://www.tpex.org.tw/web/stock/margin_trading/margin_used/mgused_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, freq, sideType, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
