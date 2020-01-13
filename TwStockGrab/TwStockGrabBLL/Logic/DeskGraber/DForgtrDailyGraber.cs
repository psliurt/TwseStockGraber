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
    /// 首頁 > 上櫃 > 三大法人 > 外資及陸資買賣超彙總表
    /// d_forgtr_daily
    /// 本資訊自民國96年1月起開始提供, 實際上由 2007/04/23開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr.php?l=zh-tw
    /// </summary>
    public class DForgtrDailyGraber : DGraber
    {
        public DForgtrDailyGraber() : base()
        {
            this._graberClassName = typeof(DForgtrDailyGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }

            List<string> typeList = new List<string>();
            typeList.Add("buy");
            typeList.Add("sell");
            foreach (var t in typeList)
            {
                string responseContent = GetWebContent(dataDate, t);
                DForgtrDaily_Rsp rsp = JsonConvert.DeserializeObject<DForgtrDaily_Rsp>(responseContent);
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

            WriteEndRecord(record);
        }

        private void SaveToDatabase(DForgtrDaily_Rsp rsp, DateTime dataDate, string t)
        {
            DateTime period2Start = new DateTime(2018, 1, 15);
            short buySellType = TransBuySellType(t);

            List<d_forgtr_daily> tmpAddList = new List<d_forgtr_daily>();
            List<d_forgtr_daily> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_forgtr_daily>().AsNoTracking().Where(x => x.data_date == dataDate && x.buy_sell_type == buySellType).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_forgtr_daily existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.buy_sell_type == buySellType).FirstOrDefault();
                if (existItem == null)
                {
                    if (dataDate < period2Start)
                    {
                        tmpAddList.Add(new d_forgtr_daily
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            buy_sell_type = buySellType,
                            rank = ToInt(data.ElementAt(0).Trim()),
                            stock_name = data.ElementAt(2).Trim(),                            
                            total_buy_in = ToIntQ(data.ElementAt(3)),
                            total_sell_out = ToIntQ(data.ElementAt(4)),
                            total_diff = ToDecimalQ(data.ElementAt(5)),
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new d_forgtr_daily
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            buy_sell_type = buySellType,
                            rank = ToInt(data.ElementAt(0).Trim()),
                            stock_name = data.ElementAt(2).Trim(),
                            buy_in = ToIntQ(data.ElementAt(3)),
                            sell_out = ToIntQ(data.ElementAt(4)),
                            diff = ToIntQ(data.ElementAt(5)),
                            self_buy_in = ToIntQ(data.ElementAt(6)),
                            self_sell_out = ToIntQ(data.ElementAt(7)),
                            self_diff = ToIntQ(data.ElementAt(8)),
                            total_buy_in = ToIntQ(data.ElementAt(9)),
                            total_sell_out = ToIntQ(data.ElementAt(10)),
                            total_diff = ToDecimalQ(data.ElementAt(11)),
                            title = rsp.reportTitle.Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_forgtr_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string t)
        {
            string lang = "zh-tw";
            string dataType = "D"; //daily

            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr_result.php?l=zh-tw&t=D&type=buy&d=108/12/04&_=1575788642019
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/qfii_trading/forgtr_result.php?l={0}&t={1}&type={2}&d={3}&_={4}",
                lang, dataType, t, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

       
    }
}
