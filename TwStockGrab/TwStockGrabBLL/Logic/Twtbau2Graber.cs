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
    /// 交易資訊->當日沖銷交易標的->暫停先賣後買當日沖銷交易歷史查詢
    /// twtbau2
    /// 本資料自民國103年6月30日起提供
    /// https://www.twse.com.tw/zh/page/trading/exchange/TWTBAU2.html
    /// </summary>
    public class Twtbau2Graber : Graber
    {
        public Twtbau2Graber() : base()
        {
            this._graberClassName = typeof(Twtbau2Graber).Name;
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
            TWTBAU2_Rsp rsp = JsonConvert.DeserializeObject<TWTBAU2_Rsp>(responseContent);

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

        private void SaveToDatabase(TWTBAU2_Rsp rsp, DateTime grabeDate)
        {
            List<twtbau2> tmpAddList = new List<twtbau2>();
            List<twtbau2> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twtbau2>().AsNoTracking().Where(x => grabeDate >= x.start_date && grabeDate <= x.end_date).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                DateTime? stopStartDate = GetDateFromRocSlashStringQ(data.ElementAt(2).Trim());
                DateTime? stopEndDate = GetDateFromRocSlashStringQ(data.ElementAt(3).Trim());
                string reason = data.ElementAt(4).Trim();
                twtbau2 obj =
                    tmpDataList.Where(x => x.stock_no == stockNo && x.start_date == stopStartDate && x.end_date == stopEndDate && x.reason == reason).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twtbau2
                    {   
                        stock_no = stockNo,
                        stock_name = stockName,
                        start_date = stopStartDate.Value,
                        end_date = stopEndDate.Value,
                        reason = reason,                        
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twtbau2.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime startDate)
        {
            string paramResponse = "json";
            string paramStartDate = GetyyyyMMddDateString(startDate);
            string paramEndDate = GetyyyyMMddDateString(startDate);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/TWTBAU2?response={0}&strDate={1}&endDate={2}&stockNo=&_={3}",
                paramResponse, paramStartDate, paramEndDate, paramUnderLine);

            return GetHttpResponse(url);
        }

     }
}
