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
    /// 交易資訊->盤後資訊->每日成交量前20名證券
    /// mi_index_top20
    /// 本資訊自民國93年2月11日起開始提供
    /// </summary>
    public class MiIndexTop20Graber : Graber
    {
        public MiIndexTop20Graber() : base()
        {
            this._graberClassName = typeof(MiIndexTop20Graber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            MI_INDEX_Top20_Rsp rsp = JsonConvert.DeserializeObject<MI_INDEX_Top20_Rsp>(responseContent);
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

        private void SaveToDatabase(MI_INDEX_Top20_Rsp rsp, DateTime dataDate)
        {
            List<mi_index_top20> tmpAddList = new List<mi_index_top20>();
            List<mi_index_top20> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_index_top20>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                int order = ToIntQ(data.ElementAt(0)).Value;
                string stockNo = data.ElementAt(1).Trim();

                mi_index_top20 obj = tmpDataList.Where(x => x.data_date == dataDate && x.day_order == order && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new mi_index_top20
                    {
                        data_date = dataDate,
                        day_order = order,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_stock_num = ToIntQ(data.ElementAt(3)),
                        deal_trade_num = ToIntQ(data.ElementAt(4)),
                        open_price = ToDecimalQ(data.ElementAt(5)),
                        high_price = ToDecimalQ(data.ElementAt(6)),
                        low_price = ToDecimalQ(data.ElementAt(7)),
                        close_price = ToDecimalQ(data.ElementAt(8)),
                        up_down = SignToByteQ(data.ElementAt(9)),
                        up_down_price = ToDecimalQ(data.ElementAt(10)),
                        last_show_buy_price = ToDecimalQ(data.ElementAt(11)),
                        last_show_sell_price = ToDecimalQ(data.ElementAt(12)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_index_top20.AddRange(tmpAddList);
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_INDEX20?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }


    }
}
