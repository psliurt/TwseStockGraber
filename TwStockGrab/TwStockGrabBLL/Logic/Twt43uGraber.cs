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
    /// 交易資訊->三大法人->自營商買賣超彙總表
    /// twt43u
    /// 本資訊自民國93年12月17日起開始提供
    /// </summary>
    public class Twt43uGraber : Graber
    {
        public Twt43uGraber() : base()
        {
            this._graberClassName = typeof(Twt43uGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            string responseContent = GetWebContent(dataDate);
            TWT43U_Rsp rsp = JsonConvert.DeserializeObject<TWT43U_Rsp>(responseContent);
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

        private void SaveToDatabase(TWT43U_Rsp rsp, DateTime dataDate)
        {
            List<twt43u> tmpAddList = new List<twt43u>();
            List<twt43u> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt43u>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();

                twt43u obj = tmpDataList.Where(x => x.data_date == dataDate && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twt43u
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        self_buy_cnt = ToLongQ(data.ElementAt(2)),
                        self_sell_cnt = ToLongQ(data.ElementAt(3)),
                        self_cnt_diff = ToLongQ(data.ElementAt(4)),
                        risk_buy_cnt = ToLongQ(data.ElementAt(5)),
                        risk_sell_cnt = ToLongQ(data.ElementAt(6)),
                        risk_cnt_diff = ToLongQ(data.ElementAt(7)),
                        total_buy_cnt = ToLongQ(data.ElementAt(8)),
                        total_sell_cnt = ToLongQ(data.ElementAt(9)),
                        total_cnt_diff = ToLongQ(data.ElementAt(10)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }                
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt43u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/TWT43U?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
