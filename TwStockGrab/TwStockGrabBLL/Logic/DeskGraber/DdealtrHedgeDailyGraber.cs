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
    /// 首頁 > 上櫃 > 三大法人 > 自營商買賣超彙總表
    /// d_dealtr_hedge_daily
    /// 本資訊自民國103年12月起開始提供 
    /// 
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/dealer_trading/dealtr_hedge.php?l=zh-tw
    /// </summary>
    public class DDealtrHedgeDailyGraber : DGraber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> typeList = new List<string>();
            typeList.Add("buy");
            typeList.Add("sell");
            foreach (var t in typeList)
            {
                string responseContent = GetWebContent(dataDate, t);
                DDealtrHedgeDaily_Rsp rsp = JsonConvert.DeserializeObject<DDealtrHedgeDaily_Rsp>(responseContent);
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

        private void SaveToDatabase(DDealtrHedgeDaily_Rsp rsp, DateTime dataDate, string t)
        {
            short typeByte = TransBuySellType(t);
            
            List<d_dealtr_hedge_daily> tmpAddList = new List<d_dealtr_hedge_daily>();
            List<d_dealtr_hedge_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_dealtr_hedge_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.buy_sell == typeByte).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_dealtr_hedge_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.buy_sell == typeByte).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_dealtr_hedge_daily
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        buy_sell = typeByte,
                        day_order = ToIntQ(data.ElementAt(0).Trim()),
                        self_buy_in = ToIntQ(data.ElementAt(3)),
                        self_sell_out = ToIntQ(data.ElementAt(4)),
                        self_diff = ToIntQ(data.ElementAt(5)),
                        risk_buy_in = ToIntQ(data.ElementAt(6)),
                        risk_sell_out = ToIntQ(data.ElementAt(7)),
                        risk_diff = ToIntQ(data.ElementAt(8)),                        
                        total_diff = ToIntQ(data.ElementAt(9)),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                    
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_dealtr_hedge_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string t)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily
            
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();
            
            //https://www.tpex.org.tw/web/stock/3insti/dealer_trading/dealtr_hedge_result.php?l=zh-tw&t=D&type=sell&d=108/11/07&_=1573306741344
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/dealer_trading/dealtr_hedge_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, t, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

       
    }
}
