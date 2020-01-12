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
    public class DQfiiGraber : DGraber
    {
        /// <summary>
        /// 首頁 > 上櫃 > 三大法人 > 僑外資及陸資持股比例排行表
        /// d_qfii
        /// 本資訊自民國96年1月起開始提供 實際上由 2007/4/23開始提供
        /// 網頁位置
        /// https://www.tpex.org.tw/web/stock/3insti/qfii/qfii.php?l=zh-tw
        /// </summary>
        public override void DoJob(DateTime dataDate)
        {

            string responseContent = GetWebContent(dataDate);
            DQfii_Rsp rsp = JsonConvert.DeserializeObject<DQfii_Rsp>(responseContent);
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

        private void SaveToDatabase(DQfii_Rsp rsp, DateTime dataDate)
        {
            List<d_qfii> tmpAddList = new List<d_qfii>();
            List<d_qfii> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_qfii>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string stockNo = data.ElementAt(1).Trim();

                d_qfii existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_qfii
                    {
                        data_date = dataDate,
                        rank = ToInt(data.ElementAt(0).Trim()),
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        issue_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        remain_invest_count = ToLongQ(data.ElementAt(4).Trim()),
                        hold_count = ToLongQ(data.ElementAt(5).Trim()),
                        remain_invest_percent = ToDecimalQ(data.ElementAt(6).Trim()),
                        hold_percent = ToDecimalQ(data.ElementAt(7).Trim()),
                        law_hold_limit = ToDecimalQ(data.ElementAt(8).Trim()),
                        data_note = data.ElementAt(9).Trim(),                        
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_qfii.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/3insti/qfii/qfii_result.php?l=zh-tw&d=108/12/04&_=1575792621192
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/qfii/qfii_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
