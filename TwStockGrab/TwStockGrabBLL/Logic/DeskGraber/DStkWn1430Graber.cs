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
    /// 首頁 > 上櫃 > 盤後資訊 > 上櫃證券成交統計
    /// d_stk_wn1430
    /// 本資訊自民國96年7月起開始提供 2007/7/2開始有資料
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430.php?l=zh-tw
    /// </summary>
    public class DStkWn1430Graber : DGraber
    {
        public DStkWn1430Graber() : base()
        {
            this._graberClassName = typeof(DStkWn1430Graber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }

            List<string> selectTypeList = new List<string>();

            //selectTypeList.Add("02");    //食品工業
            //selectTypeList.Add("03");    //塑膠工業
            //selectTypeList.Add("04");    //紡織纖維
            //selectTypeList.Add("05");    //電機機械
            //selectTypeList.Add("06");    //電器電纜
            //selectTypeList.Add("21");    //化學工業
            //selectTypeList.Add("08");    //玻璃陶瓷
            //selectTypeList.Add("10");    //鋼鐵工業
            //selectTypeList.Add("11");    //橡膠工業
            //selectTypeList.Add("14");    //建材營造
            //selectTypeList.Add("15");    //航運業
            //selectTypeList.Add("16");    //觀光事業
            //selectTypeList.Add("17");    //金融業
            //selectTypeList.Add("18");    //貿易百貨
            //selectTypeList.Add("20");    //其他
            //selectTypeList.Add("22");    //生技醫療類
            //selectTypeList.Add("23");    //油電燃氣類
            //selectTypeList.Add("24");    //半導體類
            //selectTypeList.Add("25");    //電腦及週邊類
            //selectTypeList.Add("26");    //光電業類
            //selectTypeList.Add("27");    //通信網路類
            //selectTypeList.Add("28");    //電子零組件類
            //selectTypeList.Add("29");    //電子通路類
            //selectTypeList.Add("30");    //資訊服務類
            //selectTypeList.Add("31");    //其他電子類
            //selectTypeList.Add("32");    //文化創意業
            //selectTypeList.Add("33");    //農業科技業
            //selectTypeList.Add("34");    //電子商務業
            //selectTypeList.Add("80");    //管理股票
            //selectTypeList.Add("AA");    //受益證券
            //selectTypeList.Add("EE");    //上櫃指數股票型基金(ETF)
            //selectTypeList.Add("EN");    //指數投資證券(ETN)
            //selectTypeList.Add("TD");    //台灣存託憑證(TDR)
            //selectTypeList.Add("WW");    //認購售權證
            //selectTypeList.Add("GG");    //認股權憑證
            //selectTypeList.Add("BC");    //牛熊證(不含展延型牛熊證)
            //selectTypeList.Add("XY");    //展延型牛熊證
            selectTypeList.Add("EW");    //所有證券(不含權證、牛熊證) 
            //selectTypeList.Add("AL");    //所有證券 
            //selectTypeList.Add("OR");    //委託及成交資訊(16:05提供) 
            foreach (var selectType in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, selectType);
                DStkWn1430_Rsp rsp = JsonConvert.DeserializeObject<DStkWn1430_Rsp>(responseContent);
                if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
                {
                    WriteEndRecord(record);
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate, selectType);
                    WriteEndRecord(record);
                    Sleep();
                }
            }

            
        }

        private void SaveToDatabase(DStkWn1430_Rsp rsp, DateTime dataDate, string st)
        {
            List<d_stk_wn1430> tmpAddList = new List<d_stk_wn1430>();
            List<d_stk_wn1430> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_stk_wn1430>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == st).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_stk_wn1430 existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.select_type == st && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_stk_wn1430
                    {
                        data_date = dataDate,
                        select_type = st,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        close_price = ToDecimalQ(data.ElementAt(2).Trim()),
                        up_down_price = ToSignDecimalQ(data.ElementAt(3).Trim()),
                        open_price = ToDecimalQ(data.ElementAt(4).Trim()),
                        high_price = ToDecimalQ(data.ElementAt(5).Trim()),
                        low_price = ToDecimalQ(data.ElementAt(6).Trim()),
                        deal_stock_count = ToIntQ(data.ElementAt(7).Trim()),
                        deal_money = ToDecimalQ(data.ElementAt(8).Trim()),
                        deal_trade_count = ToIntQ(data.ElementAt(9).Trim()),
                        last_buy_price = ToDecimalQ(data.ElementAt(10).Trim()),
                        last_sell_price = ToDecimalQ(data.ElementAt(11).Trim()),
                        issue_stock_count = ToLongQ(data.ElementAt(12).Trim()),
                        next_up_limit = ToDecimalQ(data.ElementAt(13).Trim()),
                        next_down_limit = ToDecimalQ(data.ElementAt(14).Trim()),
                        title = rsp.iTotalRecords.ToString(),                        
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
            }




            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_stk_wn1430.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string st)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l=zh-tw&d=108/12/04&se=EW&_=1575728505209
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l={0}&d={1}&se={2}&_={3}",
                lang, rocDate, st, paramUnderLine);

            return GetHttpResponse(url);
        }

        

        

        
    }
}
