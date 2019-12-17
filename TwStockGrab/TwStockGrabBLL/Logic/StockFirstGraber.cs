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
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->盤後資訊->每日第一上市外國股票成交量值
    /// stock_first
    /// 本資訊自民國93年2月11日起開始提供
    /// </summary>
    public class StockFirstGraber : Graber
    {
        
        public override void DoJob(DateTime dataDate)
        {            
            string responseContent = GetWebContent(dataDate);
            STOCK_FIRST_Rsp rsp = JsonConvert.DeserializeObject<STOCK_FIRST_Rsp>(responseContent);

            if (rsp.data == null)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }            
        }       

        private void SaveToDatabase(STOCK_FIRST_Rsp rsp, DateTime grabeDate)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);            

            List<stock_first> tmpAddList = new List<stock_first>();
            List<stock_first> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<stock_first>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                if (stockNo.ToLower() != "total")
                {
                    stock_first obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.stock_no == stockNo).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddList.Add(new stock_first
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = stockName,
                            deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                            deal_cnt = ToLongQ(data.ElementAt(3)),
                            deal_money = ToDecimalQ(data.ElementAt(4)),
                            open_price = ToDecimalQ(data.ElementAt(5)),
                            high_price = ToDecimalQ(data.ElementAt(6)),
                            low_price = ToDecimalQ(data.ElementAt(7)),
                            close_price = ToDecimalQ(data.ElementAt(8)),
                            up_down_price = ToUpDownDecimal(data.ElementAt(9).Trim(), data.ElementAt(10)),
                            last_buy_in_price = ToDecimalQ(data.ElementAt(11)),
                            last_buy_ask_qty = ToLongQ(data.ElementAt(12)),
                            last_sell_out_price = ToDecimalQ(data.ElementAt(13)),
                            last_sell_ask_qty = ToLongQ(data.ElementAt(14)),
                            pe_rate = ToDecimalQ(data.ElementAt(15)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.stock_first.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/STOCK_FIRST?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
