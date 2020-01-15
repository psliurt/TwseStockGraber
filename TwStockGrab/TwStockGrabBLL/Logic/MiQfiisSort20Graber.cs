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
    /// 交易資訊->三大法人->外資及陸資持股前20名彙總表
    /// mi_qfiis_sort_20
    /// 本資訊自民國93年02月11日起提供
    /// https://www.twse.com.tw/zh/page/trading/fund/MI_QFIIS_sort_20.html
    /// </summary>
    public class MiQfiisSort20Graber : Graber
    {
        public MiQfiisSort20Graber() : base()
        {
            this._graberClassName = typeof(MiQfiisSort20Graber).Name;
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
            MI_QFIIS_SORT_20_Rsp rsp = JsonConvert.DeserializeObject<MI_QFIIS_SORT_20_Rsp>(responseContent);
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

        private void SaveToDatabase(MI_QFIIS_SORT_20_Rsp rsp, DateTime dataDate)
        {
            List<mi_qfiis_sort_20> tmpAddList = new List<mi_qfiis_sort_20>();
            List<mi_qfiis_sort_20> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_qfiis_sort_20>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                int order = ToIntQ(data.ElementAt(0).Trim()).Value;
                string stockNo = data.ElementAt(1).Trim();

                tmpAddList.Add(new mi_qfiis_sort_20
                {
                    data_date = dataDate,
                    order = order,
                    stock_no = stockNo,
                    stock_name = data.ElementAt(2).Trim(),
                    issue_cnt = ToLongQ(data.ElementAt(3)),
                    investable_cnt = ToLongQ(data.ElementAt(4)),
                    hold_cnt = ToLongQ(data.ElementAt(5)),
                    investable_rate = ToDecimalQ(data.ElementAt(6)),
                    hold_rate = ToDecimalQ(data.ElementAt(7)),
                    law_max_rate = ToDecimalQ(data.ElementAt(8)),
                    create_at = DateTime.Now,
                    update_at = DateTime.Now,
                    title = string.Format("{0}", rsp.title)
                });
            }



            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_qfiis_sort_20.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/MI_QFIIS_sort_20?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }
        
    }
}
