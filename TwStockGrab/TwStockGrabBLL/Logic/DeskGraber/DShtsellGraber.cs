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
    /// 首頁 > 上櫃 > 盤後資訊 > 當日融券賣出與借券賣出成交量值
    /// d_shtsell
    /// 本資訊自民國97年10月起開始提供 從2008/10/1提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/short/shtsell.php?l=zh-tw
    /// </summary>
    public class DShtsellGraber : DGraber
    {
        public DShtsellGraber() : base()
        {
            this._graberClassName = typeof(DShtsellGraber).Name;
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
            DShtsell_Rsp rsp = JsonConvert.DeserializeObject<DShtsell_Rsp>(responseContent);
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

        private void SaveToDatabase(DShtsell_Rsp rsp, DateTime dataDate)
        {            
            List<d_shtsell> tmpAddList = new List<d_shtsell>();
            List<d_shtsell> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_shtsell>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(0).Trim();

                d_shtsell existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_shtsell
                    {
                        data_date = dataDate,
                        stock_no = stockNo,                        
                        stock_name = data.ElementAt(1).Trim(),
                        margin_count = ToIntQ(data.ElementAt(2).Trim()),
                        margin_money = ToDecimalQ(data.ElementAt(3).Trim()),
                        lend_count = ToIntQ(data.ElementAt(4).Trim()),
                        lend_money = ToDecimalQ(data.ElementAt(5).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_shtsell.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/short/shtsell_result.php?l=zh-tw&d=108/12/04&_=1575811243181
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/short/shtsell_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
