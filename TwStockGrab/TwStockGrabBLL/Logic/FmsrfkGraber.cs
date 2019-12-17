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
    /// 交易資訊->盤後資訊->個股月成交資訊
    /// fmsrfk
    /// 本資訊自民國81年1月1日起開始提供
    /// 這個類別抓一天的資料要很久
    /// </summary>
    public class FmsrfkGraber : Graber
    {
        private StockBag _stockBag { get; set; }
        /// <summary>
        /// 交易資訊->盤後資訊->個股月成交資訊
        /// </summary>
        public FmsrfkGraber() : base()
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

            List<stock_item> stockList = this._stockBag.GetListByCategorys(selectTypeList);

            foreach (stock_item stock in stockList)
            {
                string responseContent = GetWebContent(dataDate, stock.stock_no);
                FMSRFK_Rsp rsp = JsonConvert.DeserializeObject<FMSRFK_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, stock.stock_no);
                    Sleep();
                }
            }
        }        

        private void SaveToDatabase(FMSRFK_Rsp rsp, DateTime dataDate, string stockNo)
        {
            int year = dataDate.Year;

            List<fmsrfk> tmpAddList = new List<fmsrfk>();
            List<fmsrfk> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<fmsrfk>().AsNoTracking().Where(x => x.year == year && x.stock_no == stockNo).ToList();
            }

            foreach (var data in rsp.data)
            {
                int dbYear = 1911 + Convert.ToInt32(data.ElementAt(0).Trim());
                int dbMonth = Convert.ToInt32(data.ElementAt(1).Trim());

                fmsrfk existItem = tmpDataList.Where(x => x.year == dbYear && x.month == dbMonth && x.stock_no == stockNo).FirstOrDefault();

                if (existItem == null)
                {
                    tmpAddList.Add(new fmsrfk
                    {
                        stock_no = stockNo,
                        year = dbYear,
                        month = dbMonth,
                        high_price = ToDecimalQ(data.ElementAt(2)),
                        low_price = ToDecimalQ(data.ElementAt(3)),
                        weight_avg = ToDecimalQ(data.ElementAt(4)),
                        deal_cnt = ToLongQ(data.ElementAt(5)),
                        deal_money = ToDecimalQ(data.ElementAt(6)),
                        deal_stock_cnt = ToLongQ(data.ElementAt(7)),
                        turnover_rate = ToDecimalQ(data.ElementAt(8)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }

            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.fmsrfk.AddRange(tmpAddList);               

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string stockNo)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/FMSRFK?response={0}&date={1}&stockNo={2}&_={3}",
                paramResponse, paramDate, stockNo, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
