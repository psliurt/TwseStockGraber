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
    /// 交易資訊->盤後資訊->個股年成交資訊
    /// fmnptk
    /// fmnptk_stat
    /// 本資訊自民國80年起提供
    /// 這類別抓一天的資料要花很多時間
    /// </summary>
    public class FmnptkGraber : Graber
    {
        private StockBag _stockBag { get; set; }
        /// <summary>
        /// 交易資訊->盤後資訊->個股年成交資訊
        /// </summary>
        public FmnptkGraber() : base()
        {
            _stockBag = StockBag.GetInstance();

            this._graberClassName = typeof(FmnptkGraber).Name;
            this._graberFrequency = 1;
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
                FMNPTK_Rsp rsp = JsonConvert.DeserializeObject<FMNPTK_Rsp>(responseContent);

                if (rsp.data == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, stock.stock_no);
                    Sleep();
                }
            }
        }

        
        private void SaveToDatabase(FMNPTK_Rsp rsp, string stockNo)
        {
            List<fmnptk> tmpAddList = new List<fmnptk>();
            List<fmnptk> tmpDataList = null;

            List<fmnptk_stat> tmpAddStatList = new List<fmnptk_stat>();
            List<fmnptk_stat> tmpUpdateStatList = new List<fmnptk_stat>();
            fmnptk_stat tmpExistStatItem = null;


            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<fmnptk>().AsNoTracking().Where(x => x.stock_no == stockNo).ToList();
                tmpExistStatItem = context.Set<fmnptk_stat>().Where(x => x.stock_no == stockNo).FirstOrDefault();
            }

            foreach (var data in rsp.data)
            {
                int dbYear = 1911 + Convert.ToInt32(data.ElementAt(0).Trim());

                fmnptk existItem =
                    tmpDataList.Where(x => x.year == dbYear && x.stock_no == stockNo).FirstOrDefault();

                if (existItem == null)
                {
                    tmpAddList.Add(new fmnptk
                    {
                        stock_no = stockNo,
                        year = dbYear,
                        deal_stock_cnt = ToLongQ(data.ElementAt(1)),
                        deal_money = ToDecimalQ(data.ElementAt(2)),
                        deal_cnt = ToLongQ(data.ElementAt(3)),
                        high_price = ToDecimalQ(data.ElementAt(4)),
                        high_date = GetDateFromYearAndDateStringQ(dbYear, data.ElementAt(5)),
                        low_price = ToDecimalQ(data.ElementAt(6)),
                        low_date = GetDateFromYearAndDateStringQ(dbYear, data.ElementAt(7)),
                        close_avg = ToDecimalQ(data.ElementAt(8)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }

            foreach (var data2 in rsp.data2)
            {
                if (tmpExistStatItem == null)
                {
                    tmpAddStatList.Add(new fmnptk_stat
                    {
                        stock_no = stockNo,
                        last_high = ToDecimalQ(data2.ElementAt(0)),
                        last_high_date = GetDateFromRocSlashStringQ(data2.ElementAt(1)),
                        last_low = ToDecimalQ(data2.ElementAt(2)),
                        last_low_date = GetDateFromRocSlashStringQ(data2.ElementAt(3)),
                        title = string.Format("{0}", rsp.title),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else
                {
                    tmpExistStatItem.last_high = ToDecimalQ(data2.ElementAt(0));
                    tmpExistStatItem.last_high_date = GetDateFromRocSlashStringQ(data2.ElementAt(1));
                    tmpExistStatItem.last_low = ToDecimalQ(data2.ElementAt(2));
                    tmpExistStatItem.last_low_date = GetDateFromRocSlashStringQ(data2.ElementAt(3));
                    tmpExistStatItem.update_at = DateTime.Now;

                    tmpUpdateStatList.Add(tmpExistStatItem);

                    
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.fmnptk.AddRange(tmpAddList);
                context.fmnptk_stat.AddRange(tmpAddStatList);
                foreach (var item in tmpUpdateStatList)
                {
                    context.Entry<fmnptk_stat>(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string stockNo)
        {
            string paramResponse = "json";            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/FMNPTK?response={0}&stockNo={1}&_={2}",
                paramResponse, stockNo, paramUnderLine);

            return GetHttpResponse(url);
        }       

        
    }
}
