using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;

namespace TwStockGrabBLL.Logic
{
    public class StockBag
    {
        private static StockBag _instance;
        public static StockBag GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StockBag();
            }
            return _instance;
        }

        private List<stock_item> _stockList { get; set; }

        private StockBag()
        {
            this._stockList = new List<stock_item>();

            using (TwStockDataContext context = new TwStockDataContext())
            {
                this._stockList = context.Set<stock_item>().AsNoTracking().ToList();
            }
        }

        public void CheckStock(string stockNo, string stockName, string category)
        {
            string pureStockNo = stockNo.Trim();
            string pureStockName = stockName.Trim();
            
            stock_item stock = this._stockList.Where(x => x.stock_no == pureStockNo).FirstOrDefault();
            if (stock == null)
            {
                stock_item newStock = new stock_item
                {
                    create_at = DateTime.Now,
                    stock_name = pureStockName,
                    stock_no = pureStockNo,
                    update_at = DateTime.Now,
                    memo = "",
                    stock_type = SelectTypeToDbStockType(category),
                    category = category
                };

                using (TwStockDataContext context = new TwStockDataContext())
                {
                    context.Set<stock_item>().Add(newStock);

                    context.SaveChanges();
                }
                this._stockList.Add(newStock);
            }            
        }

        public List<stock_item> GetCompleteList()
        {
            return this._stockList;
        }

        public List<stock_item> GetListByStockType(int stockType)
        {
            return this._stockList.Where(x => x.stock_type == stockType).ToList();
        }

        public List<stock_item> GetListByStockTypes(List<int> stockTypes)
        {
            return this._stockList.Where(x => stockTypes.Contains(x.stock_type)).ToList();
        }

        public List<stock_item> GetListByCategory(string selectType)
        {
            return this._stockList.Where(x => x.category == selectType).ToList();
        }

        public List<stock_item> GetListByCategorys(List<string> selectTypeList)
        {
            return this._stockList.Where(x => selectTypeList.Contains(x.category)).ToList();
        }

        private int SelectTypeToDbStockType(string typeStr)
        {
            switch (typeStr.Trim())
            {
                case "0049":       //封閉式基金
                    return 16;
                case "0099P":       //ETF
                    return 11;
                case "019919T":       //受益證券
                    return 12;
                case "0999GA":       //附認股權特別股
                    return 13;
                case "0999GD":       //附認股權公司債
                    return 30;
                case "0999G9":       //認股權憑證
                    return 14;
                case "0999":       //認購權證 (國內標的)                    
                case "0999F":       //認購權證 (外國標的)
                    return 21;
                case "0999P":       //認售權證 (國內標的)                    
                case "0999Q":       //認售權證 (外國標的)	
                    return 22;
                case "CB":       //可轉換公司債
                    return 31;
                case "0999C":    //牛證                    
                case "0999X":    //可展延牛證
                    return 23;
                case "0999B":    //熊證                    
                case "0999Y":    //可展延熊證
                    return 24;
                case "9299":     //存託憑證
                    return 15;
                case "01":       //水泥工業
                case "02":       //食品工業
                case "03":       //塑膠工業
                case "04":       //紡織纖維
                case "05":       //電機機械
                case "06":       //電器電纜
                case "07":       //化學生技醫療
                case "21":       //化學工業
                case "22":       //生技醫療業
                case "08":       //玻璃陶瓷
                case "09":       //造紙工業
                case "10":       //鋼鐵工業
                case "11":       //橡膠工業
                case "12":       //汽車工業
                case "13":       //電子工業
                case "24":       //半導體業
                case "25":       //電腦及週邊設備業
                case "26":       //光電業
                case "27":       //通信網路業
                case "28":       //電子零組件業
                case "29":       //電子通路業
                case "30":       //資訊服務業
                case "31":       //其他電子業
                case "14":       //建材營造
                case "15":       //航運業
                case "16":       //觀光事業
                case "17":       //金融保險
                case "18":       //貿易百貨
                case "23":       //油電燃氣業
                case "19":       //綜合
                case "20":       //其他
                    return 10;
                case "ALL":
                case "All":
                    return 0; //代表尚未指定
                default:
                    return 40; //其他
            }
        }
    }
}
