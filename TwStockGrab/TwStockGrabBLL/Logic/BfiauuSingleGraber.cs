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
    /// 交易資訊->鉅額交易->鉅額交易日成交資訊 (單一證券選項)
    /// bfiauu_single
    /// 本資訊自民國94年04月04日起開始提供
    /// </summary>
    public class BfiauuSingleGraber : Graber
    {      

        public override void DoJob(DateTime dataDate)
        {            
            string responseContent = GetWebContent(dataDate);
            BFIAUU_Rsp rsp = JsonConvert.DeserializeObject<BFIAUU_Rsp>(responseContent);

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

        private void SaveToDatabase(BFIAUU_Rsp rsp, DateTime dataDate)
        {
            DateTime secondTypeDataStart = new DateTime(2007, 5, 28);
            DateTime thirdTypeDataStart = new DateTime(2011, 12, 19);

            List<bfiauu_single> todayDataList = null;
            List<bfiauu_single> tmpAddList = new List<bfiauu_single>();
            using (TwStockDataContext context = new TwStockDataContext())
            {
                todayDataList = context.Set<bfiauu_single>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();

                if (stockNo != "總計")
                {
                    if (dataDate < secondTypeDataStart)
                    {
                        bfiauu_single existItem =
                                todayDataList.Where(x => x.stock_no == stockNo).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_single
                            {
                                data_date = dataDate,
                                stock_no = stockNo,
                                stock_name = data.ElementAt(1).Trim(),
                                trade_type = "",
                                settle_period = "",
                                deal_price = ToDecimalQ(data.ElementAt(2)),
                                deal_qty = ToLongQ(data.ElementAt(3)),
                                deal_stock_cnt = ToLongQ(data.ElementAt(4)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                    }
                    else if (dataDate >= secondTypeDataStart && dataDate < thirdTypeDataStart)
                    {
                        string tradeType = data.ElementAt(2).Trim();
                        string settlePeriod = data.ElementAt(3).Trim();

                        bfiauu_single existItem =
                                todayDataList.Where(x => x.stock_no == stockNo && x.trade_type == tradeType && x.settle_period == settlePeriod).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_single
                            {
                                data_date = dataDate,
                                stock_no = stockNo,
                                stock_name = data.ElementAt(1).Trim(),
                                trade_type = tradeType,
                                settle_period = settlePeriod,
                                deal_price = ToDecimalQ(data.ElementAt(4)),
                                deal_stock_cnt = ToLongQ(data.ElementAt(5)),
                                deal_money = ToDecimalQ(data.ElementAt(6)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                    }
                    else
                    {
                        string tradeType = data.ElementAt(2).Trim();

                        bfiauu_single existItem =
                                todayDataList.Where(x => x.stock_no == stockNo && x.trade_type == tradeType).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_single
                            {
                                data_date = dataDate,
                                stock_no = stockNo,
                                stock_name = data.ElementAt(1).Trim(),
                                trade_type = tradeType,
                                settle_period = "",
                                deal_price = ToDecimalQ(data.ElementAt(3)),
                                deal_stock_cnt = ToLongQ(data.ElementAt(4)),
                                deal_money = ToDecimalQ(data.ElementAt(5)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                    }

                }
            }
                

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiauu_single.AddRange(tmpAddList);
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/block/BFIAUU?response={0}&date={1}&selectType=S&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
