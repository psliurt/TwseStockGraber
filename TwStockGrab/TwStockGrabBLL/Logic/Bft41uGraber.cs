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
    /// 交易資訊->盤後資訊->盤後定價交易
    /// bft41u
    /// 本資訊自民國93年12月24日起提供
    /// </summary>
    public class Bft41uGraber  : Graber
    {
        public Bft41uGraber() : base()
        {
            this._graberClassName = typeof(Bft41uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALL");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                BFT41U_Rsp rsp = JsonConvert.DeserializeObject<BFT41U_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, type);
                    Sleep();
                }
            }
        }


        private void SaveToDatabase(BFT41U_Rsp rsp, DateTime dataDate, string selectType)
        {
            int year = dataDate.Year;
            int month = dataDate.Month;

            List<bft41u> tmpAddList = new List<bft41u>();            
            List<bft41u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<bft41u>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {                
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();
                //this._stockBag.CheckStock(stockNo, stockName);

                bft41u obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new bft41u
                    {
                        data_date = dataDate,
                        select_type = selectType,
                        stock_no = stockNo,
                        stock_name = stockName,
                        deal_qty = ToLongQ(data.ElementAt(2)),
                        deal_trade_cnt = ToLongQ(data.ElementAt(3)),
                        deal_amount = ToLongQ(data.ElementAt(4)),
                        deal_price = ToDecimalQ(data.ElementAt(5)),
                        last_buy_qty = ToLongQ(data.ElementAt(6)),
                        last_sell_qty = ToLongQ(data.ElementAt(7)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });

                }               

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bft41u.AddRange(tmpAddList);               

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/BFT41U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
