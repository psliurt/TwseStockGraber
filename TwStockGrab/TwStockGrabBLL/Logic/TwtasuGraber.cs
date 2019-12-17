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
    /// TODO:這個類別還需要再確認抓資料的完整性，確認為什麼會出現A0000B
    /// 交易資訊->盤後資訊->當日融券賣出與借券賣出成交量值
    /// twtasu
    /// 本資訊自民國97年9月26日起開始提供
    /// </summary>
    public class TwtasuGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            TWTASU_Rsp rsp = JsonConvert.DeserializeObject<TWTASU_Rsp>(responseContent);
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

        private void SaveToDatabase(TWTASU_Rsp rsp, DateTime dataDate)
        {
            List<twtasu> tmpAddList = new List<twtasu>();
            List<twtasu> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twtasu>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string[] stockParts = data.ElementAt(0).Split(new string[] { "   " }, StringSplitOptions.None);
                string stockNo = "";
                string stockName = "";
                if (stockParts.Count() >= 2)
                {
                    stockNo = stockParts[0].Trim();
                    stockName = stockParts[1].Trim();
                }
                else
                {
                    stockNo = "A0000B"; //TODO: why?
                    stockName = stockParts[0].Trim();
                }

                twtasu obj = tmpDataList.Where(x => x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twtasu
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = stockName,
                        borrow_sell_deal_cnt = ToLongQ(data.ElementAt(1)),
                        borrow_sell_deal_money = ToDecimalQ(data.ElementAt(2)),
                        lending_sell_deal_cnt = ToLongQ(data.ElementAt(3)),
                        lending_sell_deal_money = ToDecimalQ(data.ElementAt(4)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twtasu.AddRange(tmpAddList);
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWTASU?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
