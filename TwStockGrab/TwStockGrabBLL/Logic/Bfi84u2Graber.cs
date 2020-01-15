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
    /// 交易資訊->融資融券與可借券賣出額度->停券歷史查詢
    /// bfi84u2
    /// 自99年11月8日起提供停資券原因
    /// https://www.twse.com.tw/zh/page/trading/exchange/BFI84U2.html
    /// </summary>
    public class Bfi84u2Graber : Graber
    {
        public Bfi84u2Graber() : base()
        {
            this._graberClassName = typeof(Bfi84u2Graber).Name;
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
            BFI84U2_Rsp rsp = JsonConvert.DeserializeObject<BFI84U2_Rsp>(responseContent);

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
        

        private void SaveToDatabase(BFI84U2_Rsp rsp, DateTime grabeDate)
        {
            List<bfi84u2> tmpAddList = new List<bfi84u2>();
            List<bfi84u2> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<bfi84u2>().AsNoTracking().Where(x => grabeDate >= x.stop_start && grabeDate <= x.stop_end).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(0).Trim();
                string stockName = data.ElementAt(1).Trim();

                DateTime? stopStartDate = GetDateFromRocPointStringQ(data.ElementAt(2).Trim());
                DateTime? stopEndDate = GetDateFromRocPointStringQ(data.ElementAt(3).Trim());
                string reason = data.ElementAt(4).Trim();
                bfi84u2 obj =
                    tmpDataList.Where(x => x.stock_no == stockNo && x.stop_start == stopStartDate && x.stop_end == stopEndDate && x.reason == reason).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new bfi84u2
                    {
                        stock_no = stockNo,
                        stock_name = stockName,
                        stop_start = stopStartDate,
                        stop_end = stopEndDate,
                        reason = reason,
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfi84u2.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime startDate)
        {
            string paramResponse = "json";
            string paramStartDate = GetyyyyMMddDateString(startDate);
            string paramEndDate = GetyyyyMMddDateString(startDate);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/BFI84U2?response={0}&startDate={1}&endDate={2}&stockNo=&_={3}",
                paramResponse, paramStartDate, paramEndDate, paramUnderLine);

            return GetHttpResponse(url);
        }


    }
}
