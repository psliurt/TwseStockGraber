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
    /// 交易資訊->三大法人->外資及陸資投資類股持股比率表
    /// mi_qfiis_cat
    /// 本資訊自民國89年12月7日起開始提供
    /// https://www.twse.com.tw/zh/page/trading/fund/MI_QFIIS_cat.html
    /// </summary>
    public class MiQfiisCatGraber : Graber
    {
        public MiQfiisCatGraber() : base()
        {
            this._graberClassName = typeof(MiQfiisCatGraber).Name;
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
            MI_QFIIS_CAT_Rsp rsp = JsonConvert.DeserializeObject<MI_QFIIS_CAT_Rsp>(responseContent);
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

        private void SaveToDatabase(MI_QFIIS_CAT_Rsp rsp, DateTime dataDate)
        {
            List<mi_qfiis_cat> tmpAddList = new List<mi_qfiis_cat>();
            List<mi_qfiis_cat> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_qfiis_cat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string category = data.ElementAt(0).Trim();

                mi_qfiis_cat obj = tmpDataList.Where(x => x.industry_cat == category).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new mi_qfiis_cat
                    {
                        data_date = dataDate,
                        industry_cat = data.ElementAt(0).Trim(),
                        hold_comp_cnt = ToIntQ(data.ElementAt(1).Trim()),
                        total_issue_cnt = ToLongQ(data.ElementAt(2).Trim()),
                        hold_stock_cnt = ToLongQ(data.ElementAt(3)),
                        hold_stock_rate = ToDecimalQ(data.ElementAt(4)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)

                    });
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_qfiis_cat.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/MI_QFIIS_cat?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }        
    }
}
