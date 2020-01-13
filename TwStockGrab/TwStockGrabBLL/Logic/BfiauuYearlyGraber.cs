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
    /// 交易資訊->鉅額交易->鉅額交易年成交量值統計
    /// bfiauu_yearly
    /// 這個資料基本上是以年度為單位，可以久久抓一次即可
    /// </summary>
    public class BfiauuYearlyGraber : Graber
    {
        public BfiauuYearlyGraber() : base()
        {
            this._graberClassName = typeof(BfiauuYearlyGraber).Name;
            this._graberFrequency = 365;
        }

        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            BFIAUU_Y_Rsp rsp = JsonConvert.DeserializeObject<BFIAUU_Y_Rsp>(responseContent);

            if (rsp.data == null)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }
        }       

        private void SaveToDatabase(BFIAUU_Y_Rsp rsp, DateTime dataDate)
        {         
            List<bfiauu_yearly> tmpAddList = new List<bfiauu_yearly>();
            List<bfiauu_yearly> tmpUpdateList = new List<bfiauu_yearly>();
            List<bfiauu_yearly> allData = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                allData = context.Set<bfiauu_yearly>().AsNoTracking().ToList();
            }

            foreach (var data in rsp.data)
            {
                int? year = GetAdYearFromRocYearWordString(data.ElementAt(0));
                if (year.HasValue)
                {
                    bfiauu_yearly existItem =
                            allData.Where(x => x.year == year).FirstOrDefault();

                    if (existItem == null)
                    {
                        tmpAddList.Add(new bfiauu_yearly
                        {
                            year = year.Value,                                
                            deal_stock_cnt = ToLongQ(data.ElementAt(1)),
                            deal_stock_rate = ToDecimalQ(data.ElementAt(2)),
                            deal_money = ToDecimalQ(data.ElementAt(3)),
                            deal_money_rate = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            note = data.ElementAt(0).Trim(),
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                    else
                    {                            
                        existItem.deal_stock_cnt = ToLongQ(data.ElementAt(1));
                        existItem.deal_stock_rate = ToDecimalQ(data.ElementAt(2));
                        existItem.deal_money = ToDecimalQ(data.ElementAt(3));
                        existItem.deal_money_rate = ToDecimalQ(data.ElementAt(4));
                        existItem.update_at = DateTime.Now;
                        tmpUpdateList.Add(existItem);
                    }
                    
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiauu_yearly.AddRange(tmpAddList);
                foreach (var item in tmpUpdateList)
                {
                    context.Entry<bfiauu_yearly>(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/block/BFIAUU_y?response={0}&_={1}",
                paramResponse, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
