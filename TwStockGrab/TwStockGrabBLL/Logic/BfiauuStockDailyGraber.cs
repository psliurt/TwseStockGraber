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
    /// 交易資訊->鉅額交易->個股單一證券鉅額交易日成交資訊
    /// bfiauu_stock
    /// 本資訊自民國94年04月04日起開始提供
    /// 這個類別抓一天的資料會很久
    /// </summary>
    public class BfiauuStockDailyGraber : Graber
    {
        private StockBag _stockBag { get; set; }

        public BfiauuStockDailyGraber() : 
            base()
        {
            _stockBag = StockBag.GetInstance();
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("01");       //水泥工業
            selectTypeList.Add("02");       //食品工業
            selectTypeList.Add("03");       //塑膠工業
            selectTypeList.Add("04");       //紡織纖維
            selectTypeList.Add("05");       //電機機械
            selectTypeList.Add("06");       //電器電纜
            selectTypeList.Add("07");       //化學生技醫療
            selectTypeList.Add("21");       //化學工業
            selectTypeList.Add("22");       //生技醫療業
            selectTypeList.Add("08");       //玻璃陶瓷
            selectTypeList.Add("09");       //造紙工業
            selectTypeList.Add("10");       //鋼鐵工業
            selectTypeList.Add("11");       //橡膠工業
            selectTypeList.Add("12");       //汽車工業
            selectTypeList.Add("13");       //電子工業
            selectTypeList.Add("24");       //半導體業
            selectTypeList.Add("25");       //電腦及週邊設備業
            selectTypeList.Add("26");       //光電業
            selectTypeList.Add("27");       //通信網路業
            selectTypeList.Add("28");       //電子零組件業
            selectTypeList.Add("29");       //電子通路業
            selectTypeList.Add("30");       //資訊服務業
            selectTypeList.Add("31");       //其他電子業
            selectTypeList.Add("14");       //建材營造
            selectTypeList.Add("15");       //航運業
            selectTypeList.Add("16");       //觀光事業
            selectTypeList.Add("17");       //金融保險
            selectTypeList.Add("18");       //貿易百貨
            selectTypeList.Add("23");       //油電燃氣業
            selectTypeList.Add("19");       //綜合
            selectTypeList.Add("20");       //其他

            List<stock_item> stockList = this._stockBag.GetListByCategorys(selectTypeList).OrderByDescending(x=>x.stock_no).ToList();

            foreach (stock_item stock in stockList)
            {
                string responseContent = GetWebContent(dataDate, stock);
                BFIAUU_SD_Rsp rsp = JsonConvert.DeserializeObject<BFIAUU_SD_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, stock);
                    Sleep();
                }                
            }            
        }        

        private void SaveToDatabase(BFIAUU_SD_Rsp rsp, DateTime dataDate, stock_item stock)
        {
            int year = Convert.ToInt32(rsp.date);
            string stockNo = stock.stock_no;

            List<bfiauu_stock> tmpAddList = new List<bfiauu_stock>();
            List<bfiauu_stock> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<bfiauu_stock>().AsNoTracking().Where(x => x.year == year && x.stock_no == stockNo).ToList();
            }

            foreach (var data in rsp.data)
            {
                DateTime? tradeDate = GetDateFromAdSlashDateString(data.ElementAt(0));

                if (tradeDate.HasValue)
                {
                    bfiauu_stock existItem =
                        tmpDataList.Where(x => x.stock_no == stock.stock_no && x.year == year && x.trade_date == tradeDate).FirstOrDefault();

                    if (existItem == null)
                    {
                        if (year <= 2006)
                        {
                            tmpAddList.Add(new bfiauu_stock
                            {
                                year = year,
                                stock_no = stock.stock_no,
                                stock_name = stock.stock_name,
                                trade_date = tradeDate.Value,
                                deal_stock_cnt = ToLongQ(data.ElementAt(1)),
                                deal_money = ToDecimalQ(data.ElementAt(2)),
                                high_price = ToDecimalQ(data.ElementAt(3)),
                                low_price = ToDecimalQ(data.ElementAt(4)),
                                weight_price = ToDecimalQ(data.ElementAt(5)),
                                deal_cnt = ToLongQ(data.ElementAt(6)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                        else if (year >= 2007 && year <= 2011)
                        {
                            tmpAddList.Add(new bfiauu_stock
                            {
                                year = year,
                                stock_no = stock.stock_no,
                                stock_name = stock.stock_name,
                                trade_date = tradeDate.Value,
                                trade_type = data.ElementAt(1).Trim(),
                                settle_period = data.ElementAt(2).Trim(),
                                deal_stock_cnt = ToLongQ(data.ElementAt(3)),
                                deal_money = ToDecimalQ(data.ElementAt(4)),
                                high_price = ToDecimalQ(data.ElementAt(5)),
                                low_price = ToDecimalQ(data.ElementAt(6)),
                                weight_price = ToDecimalQ(data.ElementAt(7)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                        else if (year >= 2012)
                        {
                            tmpAddList.Add(new bfiauu_stock
                            {
                                year = year,
                                stock_no = stock.stock_no,
                                stock_name = stock.stock_name,
                                trade_date = tradeDate.Value,
                                trade_type = data.ElementAt(1).Trim(),
                                deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                                deal_money = ToDecimalQ(data.ElementAt(3)),
                                high_price = ToDecimalQ(data.ElementAt(4)),
                                low_price = ToDecimalQ(data.ElementAt(5)),
                                weight_price = ToDecimalQ(data.ElementAt(6)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                        else
                        {

                        }
                    }
                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiauu_stock.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, stock_item stockItem)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramStockNo = stockItem.stock_no;
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/block/BFIAUU_sd?response={0}&date={1}&stockNo={2}&_={3}",
                paramResponse, paramDate, paramStockNo, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
