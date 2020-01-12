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
    public class DSitctrDailyGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 三大法人 > 投信買賣超彙總表
        /// d_sitctr_daily
        /// 本資訊自民國96年1月起開始提供 實際上由2009/2/2日開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/3insti/sitc_trading/sitctr.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {
            List<string> typeList = new List<string>();
            typeList.Add("buy");
            typeList.Add("sell");
            foreach (var t in typeList)
            {
                string responseContent = GetWebContent(dataDate, t);
                DSitctrDaily_Rsp rsp = JsonConvert.DeserializeObject<DSitctrDaily_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, t);
                    Sleep();
                }
            }
        }

        private void SaveToDatabase(DSitctrDaily_Rsp rsp, DateTime dataDate, string t)
        {
            short buySellType = TransBuySellType(t);

            List<d_sitctr_daily> tmpAddList = new List<d_sitctr_daily>();
            List<d_sitctr_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_sitctr_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.buy_sell_type == buySellType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_sitctr_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.buy_sell_type == buySellType).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_sitctr_daily
                    {
                        data_date = dataDate,
                        rank = ToInt(data.ElementAt(0).Trim()),
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        buy_sell_type = buySellType,                        
                        buy_in = ToIntQ(data.ElementAt(3)),
                        sell_out = ToIntQ(data.ElementAt(4)),                        
                        total_diff = ToIntQ(data.ElementAt(5)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });


                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_sitctr_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string t)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/3insti/sitc_trading/sitctr_result.php?l=zh-tw&t=D&type=buy&d=108/12/03&_=1575787424537
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/sitc_trading/sitctr_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, t, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }
       
    }
}
