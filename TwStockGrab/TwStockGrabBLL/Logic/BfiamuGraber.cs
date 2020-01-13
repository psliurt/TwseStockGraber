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
    ///  交易資訊->盤後資訊->各類指數日成交量值
    ///  bfiamu
    /// 本資訊自民國93年7月9日起開始提供
    /// </summary>
    public class BfiamuGraber : Graber
    {
        public BfiamuGraber() : base()
        {
            this._graberClassName = typeof(BfiamuGraber).Name;
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
            BFIAMU_Rsp rsp = JsonConvert.DeserializeObject<BFIAMU_Rsp>(responseContent);

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

        private void SaveToDatabase(BFIAMU_Rsp rsp, DateTime dataDate)
        {
            List<bfiamu> tmpAddList = new List<bfiamu>();
            List<bfiamu> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<bfiamu>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string indexName = data.ElementAt(0).Trim();

                bfiamu obj = tmpDataList.Where(x => x.index_name == indexName).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new bfiamu
                    {
                        data_date = dataDate,
                        index_name = data.ElementAt(0),
                        deal_stock_cnt = ToLongQ(data.ElementAt(1)),
                        deal_money = ToDecimalQ(data.ElementAt(2)),
                        deal_trade_cnt = ToIntQ(data.ElementAt(3)),
                        up_down_price = ToDecimalQ(data.ElementAt(4)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.bfiamu.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/exchangeReport/BFIAMU?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
