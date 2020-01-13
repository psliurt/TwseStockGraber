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
    /// 交易資訊->盤後資訊->每日收盤行情->委託及成交統計資訊
    /// mi_index_deal_stat
    /// mi_index_intrust_stat
    /// 這資料從 2012年4月2日開始有
    /// </summary>
    public class MiIndexTradeStatisticGraber :Graber
    {
        public MiIndexTradeStatisticGraber() : base()
        {
            this._graberClassName = typeof(MiIndexTradeStatisticGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            
            string responseContent = GetWebContent(dataDate, "MS2");
            MI_INDEX_Rsp rsp = JsonConvert.DeserializeObject<MI_INDEX_Rsp>(responseContent);

            if (rsp.data1 == null)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }            
        }

        

        private void SaveToDatabase(MI_INDEX_Rsp rsp, DateTime grabeDate)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);

            List<mi_index_deal_stat> tmpAddDealList = new List<mi_index_deal_stat>();
            List<mi_index_deal_stat> tmpDealDataList = null;

            List<mi_index_intrust_stat> tmpAddIntrustList = new List<mi_index_intrust_stat>();
            List<mi_index_intrust_stat> tmpIntrustList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDealDataList = context.Set<mi_index_deal_stat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpIntrustList = context.Set<mi_index_intrust_stat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }


            foreach (var dealData in rsp.data1)
            {
                string dealItem = dealData.ElementAt(0).Trim();

                mi_index_deal_stat obj =
                    tmpDealDataList.Where(x => x.data_date == dataDate && x.deal_item == dealItem).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddDealList.Add(new mi_index_deal_stat
                    {
                        data_date = dataDate,
                        deal_item = dealItem,
                        all_market = ToLongQ(dealData.ElementAt(1)),                        
                        stock_market = ToLongQ(dealData.ElementAt(2)),
                        fund_market = ToLongQ(dealData.ElementAt(3)),
                        warrant_percent = ToDecimalQ(dealData.ElementAt(4)),
                        warrant_market = ToLongQ(dealData.ElementAt(5)),                        
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}-{1}", rsp.title, rsp.subtitle1)
                    });

                }

            }

            foreach (var intrustData in rsp.data2)
            {
                string intrustItem = intrustData.ElementAt(0).Trim();

                mi_index_intrust_stat obj =
                    tmpIntrustList.Where(x => x.data_date == dataDate && x.intrust_item == intrustItem).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddIntrustList.Add(new mi_index_intrust_stat {

                        data_date = dataDate,
                        intrust_item = intrustItem,
                        all_market = ToLongQ(intrustData.ElementAt(1)),
                        stock_market = ToLongQ(intrustData.ElementAt(2)),
                        fund_market = ToLongQ(intrustData.ElementAt(3)),
                        warrant_percent = ToDecimalQ(intrustData.ElementAt(4)),
                        warrant_market = ToLongQ(intrustData.ElementAt(5)),                        
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}-{1}", rsp.title, rsp.subtitle2)
                    });
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_index_deal_stat.AddRange(tmpAddDealList);
                context.mi_index_intrust_stat.AddRange(tmpAddIntrustList);
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_INDEX?response={0}&date={1}&type={2}&_={3}",                                        
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
