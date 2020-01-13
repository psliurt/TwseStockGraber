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
    /// 這個類別主要在於抓出所有可能的股票資料
    /// stock_item
    /// </summary>
    public class StockItemGraber : Graber
    {
        public StockItemGraber() : base()
        {
            this._graberClassName = typeof(StockItemGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> typeStringList = new List<string>();
            typeStringList.Add("0049");     //封閉式基金
            typeStringList.Add("0099P");    //ETF
            typeStringList.Add("019919T");  //受益證券
            typeStringList.Add("0999GA");   //附認股權特別股
            typeStringList.Add("0999GD");   //附認股權公司債
            typeStringList.Add("0999G9");   //認股權憑證
            typeStringList.Add("0999");     //認購權證 (國內標的)
            typeStringList.Add("0999P");    //認售權證 (國內標的)
            typeStringList.Add("0999F");    //認購權證 (外國標的)
            typeStringList.Add("0999Q");    //認售權證 (外國標的)	
            typeStringList.Add("CB");       //可轉換公司債
            typeStringList.Add("0999C");    //牛證
            typeStringList.Add("0999B");    //熊證
            typeStringList.Add("0999X");    //可展延牛證
            typeStringList.Add("0999Y");    //可展延熊證
            typeStringList.Add("9299");     //存託憑證
            typeStringList.Add("01");       //水泥工業
            typeStringList.Add("02");       //食品工業
            typeStringList.Add("03");       //塑膠工業
            typeStringList.Add("04");       //紡織纖維
            typeStringList.Add("05");       //電機機械
            typeStringList.Add("06");       //電器電纜
            typeStringList.Add("07");       //化學生技醫療
            typeStringList.Add("21");       //化學工業
            typeStringList.Add("22");       //生技醫療業
            typeStringList.Add("08");       //玻璃陶瓷
            typeStringList.Add("09");       //造紙工業
            typeStringList.Add("10");       //鋼鐵工業
            typeStringList.Add("11");       //橡膠工業
            typeStringList.Add("12");       //汽車工業
            typeStringList.Add("13");       //電子工業
            typeStringList.Add("24");       //半導體業
            typeStringList.Add("25");       //電腦及週邊設備業
            typeStringList.Add("26");       //光電業
            typeStringList.Add("27");       //通信網路業
            typeStringList.Add("28");       //電子零組件業
            typeStringList.Add("29");       //電子通路業
            typeStringList.Add("30");       //資訊服務業
            typeStringList.Add("31");       //其他電子業
            typeStringList.Add("14");       //建材營造
            typeStringList.Add("15");       //航運業
            typeStringList.Add("16");       //觀光事業
            typeStringList.Add("17");       //金融保險
            typeStringList.Add("18");       //貿易百貨
            typeStringList.Add("23");       //油電燃氣業
            typeStringList.Add("19");       //綜合
            typeStringList.Add("20");       //其他

            foreach (string type in typeStringList)
            {
                string responseContent = GetWebContent(dataDate, type);
                CodeFilters_Rsp rsp = JsonConvert.DeserializeObject<CodeFilters_Rsp>(responseContent);

                if (rsp.resualt == null)
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

        private void SaveToDatabase(CodeFilters_Rsp rsp, DateTime dataDate, string type)
        {
            int stockType = SelectTypeToDbStockType(type);

            List<stock_item> tmpAddList = new List<stock_item>();
            List<stock_item> tmpUpdateList = new List<stock_item>();
            List<stock_item> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<stock_item>().AsNoTracking().Where(x => x.category == type).ToList();
            }

            foreach (var data in rsp.resualt)
            {
                string[] stockParts = data.Split('\t');

                if (stockParts.Count() == 2)
                {
                    string stockNo = stockParts[0].Trim();
                    string stockName = stockParts[1].Trim();

                    stock_item obj = tmpDataList.Where(x => x.stock_no == stockNo).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddList.Add(new stock_item
                        {
                            stock_no = stockNo.Trim(),
                            stock_name = stockName.Trim(),
                            stock_type = stockType,
                            category = type.Trim(),
                            memo = "每日更新檢查",
                            create_at = DateTime.Now,
                            update_at = DateTime.Now
                        });
                    }
                    else
                    {
                        if (obj.stock_type == 0)
                        {
                            obj.stock_type = stockType;
                            obj.update_at = DateTime.Now;
                            obj.memo = string.Format("每日檢查更新-stock_type由[0]變更為[{0}]", stockType);
                            tmpUpdateList.Add(obj);
                        }
                    }
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.stock_item.AddRange(tmpAddList);
                foreach (var item in tmpUpdateList)
                {
                    context.Entry<stock_item>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {                    
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/zh/api/codeFilters?filter={0}&_={1}",
                type, paramUnderLine);

            return GetHttpResponse(url);
        }       

        
    }
}
