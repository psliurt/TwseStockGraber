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
    /// 交易資訊->三大法人->三大法人買賣超日報
    /// t86
    /// 本資訊自民國101年5月2日起提供
    /// </summary>
    public class T86Graber : Graber
    {
        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALL");        //全部            

            foreach (string type in selectTypeList)
            {
                string responseContent = GetWebContent(dataDate, type);
                T86_Rsp rsp = JsonConvert.DeserializeObject<T86_Rsp>(responseContent);

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
                

        private void SaveToDatabase(T86_Rsp rsp, DateTime dataDate, string selectType)
        {

            List<t86> tmpAddList = new List<t86>();
            List<t86> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<t86>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == selectType).ToList();
            }

            foreach (var data in rsp.data)
            {
                if (data.Count() >= 19)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    t86 obj =
                        tmpDataList.Where(x => x.stock_no == stockNo).FirstOrDefault();

                    tmpAddList.Add(new t86
                    {
                        data_date = dataDate,
                        select_type = selectType,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        foreign_buy_in = ToLongQ(data.ElementAt(2)),
                        foreign_sell_out = ToLongQ(data.ElementAt(3)),
                        foreign_diff = ToLongQ(data.ElementAt(4)),
                        foreign_dealer_buy_in = ToLongQ(data.ElementAt(5)),
                        foreign_dealer_sell_out = ToLongQ(data.ElementAt(6)),
                        foreign_dealer_diff = ToLongQ(data.ElementAt(7)),
                        trust_buy_in = ToLongQ(data.ElementAt(8)),
                        trust_sell_out = ToLongQ(data.ElementAt(9)),
                        trust_diff = ToLongQ(data.ElementAt(10)),
                        dealer_diff = ToLongQ(data.ElementAt(11)),
                        dealer_self_buy_in = ToLongQ(data.ElementAt(12)),
                        dealer_self_sell_out = ToLongQ(data.ElementAt(13)),
                        dealer_self_diff = ToLongQ(data.ElementAt(14)),
                        dealer_risk_buy_in = ToLongQ(data.ElementAt(15)),
                        dealer_risk_sell_out = ToLongQ(data.ElementAt(16)),
                        dealer_risk_diff = ToLongQ(data.ElementAt(17)),
                        capital3_total_diff = ToLongQ(data.ElementAt(18)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }

            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.t86.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/T86?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, type, paramUnderLine);

            return GetHttpResponse(url);
        }       
    }
}
