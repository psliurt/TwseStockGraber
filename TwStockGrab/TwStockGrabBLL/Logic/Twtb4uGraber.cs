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
    ///  交易資訊->當日沖銷交易標的->每日當日沖銷交易標的
    ///  twtb4u
    ///  twtb4u_stat
    ///  本資訊自民國103年1月6日起開始提供
    /// </summary>
    public class Twtb4uGraber : Graber
    {
        public Twtb4uGraber() : base()
        {
            this._graberClassName = typeof(Twtb4uGraber).Name;
            this._graberFrequency = 1;
        }


        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("All");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                TWTB4U_Rsp rsp = JsonConvert.DeserializeObject<TWTB4U_Rsp>(responseContent);

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

        private void SaveToDatabase(TWTB4U_Rsp rsp, DateTime grabeDate, string selectType)
        {
            if (rsp.creditFields.Count() == 0)
            { return; }

            DateTime dataDate = GetDateFromAdDateString(rsp.date);            

            List<twtb4u> tmpAddList = new List<twtb4u>();
            List<twtb4u> tmpDataList = null;
            twtb4u_stat tmpStatisticData = null;
            twtb4u_stat tmpNewStatisticData = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twtb4u>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
                tmpStatisticData = context.Set<twtb4u_stat>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).FirstOrDefault();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();
                //this._stockBag.CheckStock(stockNo, stockName);

                twtb4u obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                   
                    tmpAddList.Add(new twtb4u
                    {
                        data_date = dataDate,
                        select_type = selectType,
                        stock_no = stockNo,
                        stock_name = stockName,
                        mark = data.ElementAt(2).Trim(),
                        daytrade_deal_stock_cnt = ToLongQ(data.ElementAt(3)),
                        daytrade_buy_in_money = ToDecimalQ(data.ElementAt(4)),
                        daytrade_sell_out_money = ToDecimalQ(data.ElementAt(5)),                            
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });

                }

            }


            foreach (var creditData in rsp.creditList)
            {
                if (tmpStatisticData == null)
                {
                    tmpNewStatisticData = new twtb4u_stat
                    {
                        select_type = selectType,
                        data_date = dataDate,
                        all_deal_stock_cnt = ToLongQ(creditData.ElementAt(0)),
                        all_deal_stock_rate = ToDecimalQ(creditData.ElementAt(1)),
                        all_buy_in_money = ToDecimalQ(creditData.ElementAt(2)),
                        all_buy_in_money_rate = ToDecimalQ(creditData.ElementAt(3)),
                        all_sell_out_money = ToDecimalQ(creditData.ElementAt(4)),
                        all_sell_out_money_rate = ToDecimalQ(creditData.ElementAt(5)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.creditTitle)
                    };
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twtb4u.AddRange(tmpAddList);
                if (tmpNewStatisticData != null)
                {
                    context.Set<twtb4u_stat>().Add(tmpNewStatisticData);
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWTB4U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }        
        
        

        
    }
}
