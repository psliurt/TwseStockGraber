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
    /// 首頁 > 上櫃 > 歷史熱門資料 > 類股成交價量比重
    /// d_sectr
    /// 本資訊自民國96年1月起開始提供 實際由 2007/4/23提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr.php?l=zh-tw
    /// </summary>
    public class DSectrGraber : DGraber
    {
        public DSectrGraber() : base()
        {
            this._graberClassName = typeof(DSectrGraber).Name;
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
            DSectr_Rsp rsp = JsonConvert.DeserializeObject<DSectr_Rsp>(responseContent);
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

        private void SaveToDatabase(DSectr_Rsp rsp, DateTime dataDate)
        {
            List<d_sectr> tmpAddList = new List<d_sectr>();
            List<d_sectr> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_sectr>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string sectorName = data.ElementAt(0).Trim();

                d_sectr existItem = tmpDataList.Where(x => x.sector_name == sectorName && x.data_date == dataDate).FirstOrDefault();


                if (existItem == null)
                {
                    tmpAddList.Add(new d_sectr
                    {
                        data_date = dataDate,
                        sector_name = sectorName,
                        deal_money = ToDecimalQ(data.ElementAt(1).Trim()),
                        deal_percent = ToDecimalQ(data.ElementAt(2).Trim()),
                        deal_stock_count = ToLongQ( data.ElementAt(3).Trim()),
                        deal_stock_percent = ToDecimalQ(data.ElementAt(4).Trim()),
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_sectr.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr_result.php?l=zh-tw&d=108/12/04&_=1575800265005
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/trading_vol_ratio/sectr_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
