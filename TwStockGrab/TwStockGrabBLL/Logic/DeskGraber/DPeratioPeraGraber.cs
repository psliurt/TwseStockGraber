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
using TwStockGrabBLL.Logic.Rsp.Json.Desk;

namespace TwStockGrabBLL.Logic.DeskGraber
{
    /// <summary>
    /// 首頁 > 上櫃 > 盤後資訊 > 個股本益比、殖利率及股價淨值比(依日期查詢)
    /// d_peratio_pera
    /// 本資訊自民國96年1月起開始提供 2007/1/2開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/peratio_analysis/pera.php?l=zh-tw
    /// </summary>
    public class DPeratioPeraGraber : DGraber
    {
        public DPeratioPeraGraber() : base()
        {
            this._graberClassName = typeof(DPeratioPeraGraber).Name;
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
            DPeratioPera_Rsp rsp = JsonConvert.DeserializeObject<DPeratioPera_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
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

        private void SaveToDatabase(DPeratioPera_Rsp rsp, DateTime dataDate)
        {
            List<d_peratio_pera> tmpAddList = new List<d_peratio_pera>();
            List<d_peratio_pera> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_peratio_pera>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_peratio_pera existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_peratio_pera
                    {
                        data_date = dataDate,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(1).Trim(),
                        pe_ratio = ToDecimalQ(data.ElementAt(2).Trim()),
                        dividend = ToDecimalQ(data.ElementAt(3).Trim()),
                        dividend_year = ToIntQ(data.ElementAt(4).Trim()),
                        yield = ToDecimalQ(data.ElementAt(5).Trim()),
                        net_value_ratio = ToDecimalQ(data.ElementAt(6).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_peratio_pera.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/peratio_analysis/pera_result.php?l=zh-tw&d=108/12/04&c=&_=1575812822073
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/peratio_analysis/pera_result.php?l={0}&d={1}&c=&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
