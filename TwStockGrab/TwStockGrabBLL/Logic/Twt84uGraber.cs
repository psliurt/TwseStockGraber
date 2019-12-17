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
    /// 交易資訊->升降幅度/首五日無漲跌幅->股價升降幅度
    /// twt84u
    /// 本資訊自民國94年12月19日起開始提供
    /// </summary>
    public class Twt84uGraber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();

            selectTypeList.Add("ALL");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                TWT84U_Rsp rsp = JsonConvert.DeserializeObject<TWT84U_Rsp>(responseContent);

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

        private void SaveToDatabase(TWT84U_Rsp rsp, DateTime grabeDate, string selectType)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);
            DateTime newTypeDataDate = new DateTime(2011, 3, 28);

            List<twt84u> tmpAddList = new List<twt84u>();
            List<twt84u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt84u>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();
                //this._stockBag.CheckStock(stockNo, stockName);

                twt84u obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    if (dataDate < newTypeDataDate)
                    {
                        tmpAddList.Add(new twt84u
                        {
                            data_date = dataDate,
                            select_type = selectType,
                            stock_no = stockNo,
                            stock_name = stockName,
                            today_high_limit = ToDecimalQ(data.ElementAt(2)),
                            yesterday_close = ToDecimalQ(data.ElementAt(3)),
                            today_low_limit = ToDecimalQ(data.ElementAt(4)),
                            recent_deal_date = GetDateFromRocPointStringQ(data.ElementAt(5).Trim()),
                            can_odd_lot = data.ElementAt(6).Trim(),                            
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                    else
                    {
                        tmpAddList.Add(new twt84u
                        {
                            data_date = dataDate,
                            select_type = selectType,
                            stock_no = stockNo,
                            stock_name = stockName,
                            today_high_limit = ToDecimalQ(data.ElementAt(2)),
                            today_open_base = ToDecimalQ(data.ElementAt(3)),
                            today_low_limit = ToDecimalQ(data.ElementAt(4)),
                            yesterday_open_base = ToDecimalQ(data.ElementAt(5)),
                            yesterday_close = ToDecimalQ(data.ElementAt(6)),
                            yesterday_buy_in = ToDecimalQ(data.ElementAt(7)),
                            yesterday_sell_out = ToDecimalQ(data.ElementAt(8)),
                            recent_deal_date = GetDateFromRocPointStringQ(data.ElementAt(9).Trim()),
                            can_odd_lot = data.ElementAt(10).Trim(),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }

                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt84u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWT84U?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
