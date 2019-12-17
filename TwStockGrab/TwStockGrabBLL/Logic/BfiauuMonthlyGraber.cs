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
    /// 交易資訊->鉅額交易->鉅額交易月成交量值統計
    ///  bfiauu_monthly
    ///  本資訊自民國94年04月04日起開始提供
    /// </summary>
    public class BfiauuMonthlyGraber :Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            BFIAUU_M_Rsp rsp = JsonConvert.DeserializeObject<BFIAUU_M_Rsp>(responseContent);

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
               
        private void SaveToDatabase(BFIAUU_M_Rsp rsp, DateTime dataDate)
        {
            int year = dataDate.Year;

            DateTime secondTypeDataStart = new DateTime(2007, 1, 1);
            //DateTime thirdTypeDataStart = new DateTime(2012, 1, 1);

            List<bfiauu_monthly> tmpAddList = new List<bfiauu_monthly>();
            List<bfiauu_monthly> tmpUpdateList = new List<bfiauu_monthly>();
            List<bfiauu_monthly> thisYearData = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                thisYearData = context.Set<bfiauu_monthly>().AsNoTracking().Where(x => x.deal_year == year).ToList();
            }

            foreach (var data in rsp.data)
            {
                int? month = GetMonthFromRocYearMonthString(data.ElementAt(0).Trim());
                if (month.HasValue)
                {
                    if (dataDate < secondTypeDataStart)
                    {
                        string tradeType = data.ElementAt(1).Trim();
                        bfiauu_monthly existItem =
                            thisYearData.Where(x => x.deal_year == year && x.deal_month == month && x.trade_type == tradeType).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_monthly
                            {
                                deal_year = year,
                                deal_month = month.Value,
                                trade_type = tradeType,
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
                        else
                        {
                            existItem.deal_cnt = ToLongQ(data.ElementAt(2));
                            existItem.deal_stock_cnt = ToLongQ(data.ElementAt(3));
                            existItem.deal_stock_rate = ToDecimalQ(data.ElementAt(4));
                            existItem.deal_money = ToDecimalQ(data.ElementAt(5));
                            existItem.deal_money_rate = ToDecimalQ(data.ElementAt(6));
                            existItem.update_at = DateTime.Now;
                            tmpUpdateList.Add(existItem);
                        }
                    }
                    else
                    {
                        string tradeType = data.ElementAt(1).Trim();
                        bfiauu_monthly existItem =
                            thisYearData.Where(x => x.deal_year == year && x.deal_month == month && x.trade_type == tradeType).FirstOrDefault();

                        if (existItem == null)
                        {
                            tmpAddList.Add(new bfiauu_monthly
                            {
                                deal_year = year,
                                deal_month = month.Value,
                                trade_type = tradeType,
                                deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                                deal_stock_rate = ToDecimalQ(data.ElementAt(3)),
                                deal_money = ToDecimalQ(data.ElementAt(4)),
                                deal_money_rate = ToDecimalQ(data.ElementAt(5)),
                                create_at = DateTime.Now,
                                update_at = DateTime.Now,
                                title = string.Format("{0}", rsp.title)
                            });
                        }
                        else
                        {
                            existItem.deal_stock_cnt = ToLongQ(data.ElementAt(2));
                            existItem.deal_stock_rate = ToDecimalQ(data.ElementAt(3));
                            existItem.deal_money = ToDecimalQ(data.ElementAt(4));
                            existItem.deal_money_rate = ToDecimalQ(data.ElementAt(5));
                            existItem.update_at = DateTime.Now;
                            tmpUpdateList.Add(existItem);
                        }
                    }                    
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiauu_monthly.AddRange(tmpAddList);
                foreach (var item in tmpUpdateList)
                {
                    context.Entry<bfiauu_monthly>(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/block/BFIAUU_m?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
