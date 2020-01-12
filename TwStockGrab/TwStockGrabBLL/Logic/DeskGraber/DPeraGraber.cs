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
    public class DPeraGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 歷史熱門資料 > 個股本益比排行
        /// d_pera
        /// 本資訊自民國96年1月起開始提供 從 2007/1/2開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/historical/pe_ratio_top10/pera.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DPera_Rsp rsp = JsonConvert.DeserializeObject<DPera_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, dataDate);
                Sleep();
            }

        }

        private void SaveToDatabase(DPera_Rsp rsp, DateTime dataDate)
        {

            List<d_pera> tmpAddList = new List<d_pera>();
            List<d_pera> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_pera>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();
                string stockName = data.ElementAt(2).Trim();

                d_pera existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_pera
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,                        
                        stock_no = stockNo,
                        stock_name = stockName,
                        close_price = ToDecimalQ(data.ElementAt(3).Trim()),
                        surplus = ToDecimalQ(data.ElementAt(4).Trim()),
                        ep_ratio = ToDecimalQ(data.ElementAt(5).Trim()),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });



                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_pera.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/historical/pe_ratio_top10/pera_result.php?l=zh-tw&d=108/12/03&_=1575801835905
            string url = string.Format("https://www.tpex.org.tw/web/stock/historical/pe_ratio_top10/pera_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
