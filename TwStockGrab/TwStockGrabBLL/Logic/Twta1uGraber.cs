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
    /// 交易資訊->融資融券與可借券賣出額度->借貸款項擔保品管制餘額
    /// twta1u
    /// 本資訊自民國95年10月2日起開始提供
    /// </summary>
    public class Twta1uGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("X");        //上市(櫃)股票(屬得為融資融券交易)           
            selectTypeList.Add("A");        //上市(櫃)股票(非屬得為融資融券交易)
            selectTypeList.Add("F");        //基金受益憑證
            selectTypeList.Add("G");        //黃金現貨
            selectTypeList.Add("B");        //債券

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                TWTA1U_Rsp rsp = JsonConvert.DeserializeObject<TWTA1U_Rsp>(responseContent);

                if (rsp.data == null || rsp.total == 0)
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

        

        private void SaveToDatabase(TWTA1U_Rsp rsp, DateTime grabeDate, string selectType)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);
            int totalDataRow = rsp.total;            

            List<twta1u> tmpAddList = new List<twta1u>();
            List<twta1u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twta1u>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();
                
                //this._stockBag.CheckStock(stockNo, stockName);

                twta1u obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twta1u
                    {
                        data_date = dataDate,
                        select_type = selectType,
                        stock_no = stockNo,
                        stock_name = stockName,
                        finance_yesterday_balance = ToDecimalQ(data.ElementAt(2)),
                        finance_buy_in = ToDecimalQ(data.ElementAt(3)),
                        finance_sell_out = ToDecimalQ(data.ElementAt(4)),
                        finance_cash_back = ToDecimalQ(data.ElementAt(5)),
                        finance_today_balance = ToDecimalQ(data.ElementAt(6)),
                        finance_ceiling = ToDecimalQ(data.ElementAt(7)),
                        dealer_yesterday_balance = ToDecimalQ(data.ElementAt(8)),
                        dealer_buy_in = ToDecimalQ(data.ElementAt(9)),
                        dealer_sell_out = ToDecimalQ(data.ElementAt(10)),
                        dealer_cash_back = ToDecimalQ(data.ElementAt(11)),
                        dealer_change = ToDecimalQ(data.ElementAt(12)),
                        dealer_today_balance = ToDecimalQ(data.ElementAt(13)),
                        dealer_ceiling = ToDecimalQ(data.ElementAt(14)),
                        dealer_unlimit_yesterday_balance = ToDecimalQ(data.ElementAt(15)),
                        dealer_unlimit_buy_in = ToDecimalQ(data.ElementAt(16)),
                        dealer_unlimit_sell_out = ToDecimalQ(data.ElementAt(17)),
                        dealer_unlimit_cash_back = ToDecimalQ(data.ElementAt(18)),
                        dealer_unlimit_change = ToDecimalQ(data.ElementAt(19)),
                        dealer_unlimit_today_balance = ToDecimalQ(data.ElementAt(20)),
                        dealer_unlimit_ceiling = ToDecimalQ(data.ElementAt(21)),
                        margin_loan_yesterday_balance = ToDecimalQ(data.ElementAt(22)),
                        margin_loan_buy_in = ToDecimalQ(data.ElementAt(23)),
                        margin_loan_sell_out = ToDecimalQ(data.ElementAt(24)),
                        margin_loan_cash_back = ToDecimalQ(data.ElementAt(25)),
                        margin_loan_change = ToDecimalQ(data.ElementAt(26)),
                        margin_loan_today_balance = ToDecimalQ(data.ElementAt(27)),
                        margin_loan_ceiling = ToDecimalQ(data.ElementAt(28)),
                        margin_deliver_yesterday_balance = ToDecimalQ(data.ElementAt(29)),
                        margin_deliver_buy_in = ToDecimalQ(data.ElementAt(30)),
                        margin_deliver_sell_out = ToDecimalQ(data.ElementAt(31)),
                        margin_deliver_cash_back = ToDecimalQ(data.ElementAt(32)),
                        margin_deliver_change = ToDecimalQ(data.ElementAt(33)),
                        margin_deliver_today_balance = ToDecimalQ(data.ElementAt(34)),
                        margin_deliver_ceiling = ToDecimalQ(data.ElementAt(35)),                        
                        market_type = data.ElementAt(36).Trim(),
                        note = data.ElementAt(37).Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });

                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twta1u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWTA1U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
