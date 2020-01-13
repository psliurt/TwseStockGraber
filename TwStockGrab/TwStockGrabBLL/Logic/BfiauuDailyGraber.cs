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
    /// 交易資訊  鉅額交易  鉅額交易日成交量值統計
    /// bfiauu_daily
    /// 本資訊自民國94年04月04日起開始提供
    /// </summary>
    public class BfiauuDailyGraber : Graber
    {
        public BfiauuDailyGraber() : base()
        {
            this._graberClassName = typeof(BfiauuDailyGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {

            work_record record = null;
            if (GetOrCreateWorkRecord(dataDate, out record))
            {
                return;
            }
            string responseContent = GetWebContent(dataDate);
            BFIAUU_D_Rsp rsp = JsonConvert.DeserializeObject<BFIAUU_D_Rsp>(responseContent);

            if (rsp.data == null)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                WriteEndRecord(record);
                Sleep();
            }
         
        }        

        private void SaveToDatabase(BFIAUU_D_Rsp rsp, DateTime dataDate)
        {
            DateTime startDate = new DateTime(dataDate.Year, dataDate.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            DateTime secondTypeDataStart = new DateTime(2007, 5, 1);
            DateTime thirdTypeDataStart = new DateTime(2012, 1, 1);

            List<bfiauu_daily> tmpAddList = new List<bfiauu_daily>();
            List<bfiauu_daily> thisMonthData = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                 thisMonthData = context.Set<bfiauu_daily>().AsNoTracking().Where(x => x.data_date >= startDate && x.data_date <= endDate).ToList();                
            }

            foreach (var data in rsp.data)
            {
                DateTime? tradeDate = GetDateFromRocSlashStringQ(data.ElementAt(0));

                if (tradeDate.HasValue)
                {
                    if (dataDate < secondTypeDataStart)
                    {
                        string typeClass = data.ElementAt(1).Trim();
                        bfiauu_daily existItem =
                            thisMonthData.Where(x => x.data_date == tradeDate && x.type_class == typeClass).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_daily
                            {
                                data_date = tradeDate.Value,
                                type_class = typeClass,
                                trade_type = "",
                                settle_type = "",
                                deal_cnt = ToLongQ(data.ElementAt(2)),
                                deal_stock_cnt = ToLongQ(data.ElementAt(3)),
                                deal_stock_rate = ToDecimalQ(data.ElementAt(4)),
                                deal_money = ToDecimalQ(data.ElementAt(5)),
                                deal_money_rate = ToDecimalQ(data.ElementAt(6)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });

                        }

                    }
                    else if (dataDate >= secondTypeDataStart && dataDate < thirdTypeDataStart)
                    {
                        string tradeType = data.ElementAt(1).Trim();
                        string settleType = data.ElementAt(2).Trim();
                        string typeClass = data.ElementAt(3).Trim();
                        bfiauu_daily existItem =
                            thisMonthData.Where(x => x.data_date == tradeDate && x.trade_type == tradeType && x.type_class == typeClass && x.settle_type == settleType).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_daily
                            {
                                data_date = tradeDate.Value,
                                trade_type = tradeType,
                                settle_type = settleType,
                                type_class = typeClass,
                                deal_stock_cnt = ToLongQ(data.ElementAt(4)),
                                deal_stock_rate = ToDecimalQ(data.ElementAt(5)),
                                deal_money = ToDecimalQ(data.ElementAt(6)),
                                deal_money_rate = ToDecimalQ(data.ElementAt(7)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });

                        }
                    }
                    else if (dataDate >= thirdTypeDataStart)
                    {
                        string tradeType = data.ElementAt(1).Trim();
                        string typeClass = data.ElementAt(2).Trim();
                        bfiauu_daily existItem =
                            thisMonthData.Where(x => x.data_date == tradeDate && x.trade_type == tradeType && x.type_class == typeClass).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_daily
                            {
                                data_date = tradeDate.Value,
                                trade_type = tradeType,
                                type_class = typeClass,
                                settle_type = "",
                                deal_stock_cnt = ToLongQ(data.ElementAt(3)),
                                deal_stock_rate = ToDecimalQ(data.ElementAt(4)),
                                deal_money = ToDecimalQ(data.ElementAt(5)),
                                deal_money_rate = ToDecimalQ(data.ElementAt(6)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });

                        }
                    }
                    else
                    {

                    }
                }               
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiauu_daily.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/block/BFIAUU_d?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
