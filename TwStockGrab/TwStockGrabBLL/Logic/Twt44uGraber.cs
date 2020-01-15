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
    /// 交易資訊->三大法人->投信買賣超彙總表
    /// twt44u
    /// 本資訊自民國93年12月17日起開始提供
    /// </summary>
    public class Twt44uGraber : Graber
    {
        public Twt44uGraber() : base()
        {
            this._graberClassName = typeof(Twt44uGraber).Name;
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
            TWT44U_Rsp rsp = JsonConvert.DeserializeObject<TWT44U_Rsp>(responseContent);
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

        private void SaveToDatabase(TWT44U_Rsp rsp, DateTime dataDate)
        {
            List<twt44u> tmpAddList = new List<twt44u>();
            List<twt44u> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt44u>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(1).Trim();

                twt44u obj = tmpDataList.Where(x => x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twt44u
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        buy_cnt = ToLongQ(data.ElementAt(3)),
                        sell_cnt = ToLongQ(data.ElementAt(4)),
                        cnt_diff = ToLongQ(data.ElementAt(5)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }                
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt44u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);            
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/TWT44U?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }
    }
}
