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
    /// 交易資訊->融資融券與可借券賣出額度->平盤下得融(借)券賣出之證券名單
    /// twt92u
    /// 本資料自民國102年9月23日開始提供
    /// </summary>
    public class Twt92uGraber : Graber
    {
        public Twt92uGraber() : base()
        {
            this._graberClassName = typeof(Twt92uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            TWT92U_Rsp rsp = JsonConvert.DeserializeObject<TWT92U_Rsp>(responseContent);

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

        private void SaveToDatabase(TWT92U_Rsp rsp, DateTime grabeDate)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);            

            List<twt92u> tmpAddList = new List<twt92u>();
            List<twt92u> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt92u>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                bool isStopMargin = StarSignToBool(data.ElementAt(2));
                bool isStopLend = StarSignToBool(data.ElementAt(3));
                bool isYesterdayDownLimitPriceStopAllToady = StarSignToBool(data.ElementAt(4));
                //this._stockBag.CheckStock(stockNo, stockName);

                twt92u obj =
                    tmpDataList.Where(x => x.data_date == dataDate && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    if (isStopMargin || isStopLend)
                    {
                        tmpAddList.Add(new twt92u
                        {
                            data_date = dataDate,
                            stock_no = stockNo,
                            stock_name = stockName,
                            stop_margin_sell = isStopMargin,
                            stop_lend_sell = isStopLend,
                            yesterday_down_limit_stop_all = isYesterdayDownLimitPriceStopAllToady,                            
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.title)
                        });
                    }
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt92u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWT92U?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }

    }
}
