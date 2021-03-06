﻿using Newtonsoft.Json;
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
    /// 首頁 > 上櫃 > 歷史熱門資料 > 個股週轉率排行(年)
    /// d_trn_monthly
    /// 本資訊自民國96年1月起開始提供 實際由2007/4/23開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn.php?l=zh-tw
    /// </summary>
    public class DTrnYearlyGraber : DGraber
    {
        public DTrnYearlyGraber() : base()
        {
            this._graberClassName = typeof(DTrnYearlyGraber).Name;
            this._graberFrequency = 365;
        }

        public override void DoJob(DateTime dataDate)
        {
            DateTime yearDate = new DateTime(dataDate.Year, 1, 1);
            work_record record = null;
            if (GetOrCreateWorkRecord(yearDate, out record))
            {
                return;
            }

            string responseContent = GetWebContent(yearDate);
            DTrnYearly_Rsp rsp = JsonConvert.DeserializeObject<DTrnYearly_Rsp>(responseContent);
            if (rsp.iTotalRecords == 0 || rsp.aaData == null || rsp.aaData.Count() == 0)
            {
                WriteEndRecord(record);
                Sleep();
            }
            else
            {
                SaveToDatabase(rsp, yearDate);
                WriteEndRecord(record);
                Sleep();
            }
        }

        /// <summary>
        /// 這邊的dataDate在傳進來function已經做過處理，都會是每個月的第一天
        /// </summary>
        /// <param name="rsp"></param>
        /// <param name="dataDate"></param>
        private void SaveToDatabase(DTrnYearly_Rsp rsp, DateTime dataDate)
        {
            List<d_trn_yearly> tmpAddList = new List<d_trn_yearly>();
            List<d_trn_yearly> tmpUpdateList = new List<d_trn_yearly>();
            List<d_trn_yearly> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_trn_yearly>().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                int rankOrder = ToInt(data.ElementAt(0).Trim());
                string stockNo = data.ElementAt(1).Trim();

                d_trn_yearly existItem = tmpDataList.Where(x => x.stock_no == stockNo && x.data_date == dataDate && x.rank_order == rankOrder).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_trn_yearly
                    {
                        data_date = dataDate,
                        rank_order = rankOrder,
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2).Trim(),
                        deal_stock_count = ToLongQ(data.ElementAt(3).Trim()),
                        issue_stock_count = ToLongQ(data.ElementAt(4).Trim()),
                        turnover_rate = ToDecimalQ(data.ElementAt(5).Trim()),
                        title = rsp.reportTitle.Trim(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });
                }
                else //已經存在，就要用update
                {
                    existItem.deal_stock_count = ToLongQ(data.ElementAt(3).Trim());
                    existItem.issue_stock_count = ToLongQ(data.ElementAt(4).Trim());
                    existItem.turnover_rate = ToDecimalQ(data.ElementAt(5).Trim());
                    existItem.update_at = DateTime.Now;

                    tmpUpdateList.Add(existItem);
                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_trn_yearly.AddRange(tmpAddList);

                foreach (var item in tmpUpdateList)
                {
                    context.Entry<d_trn_yearly>(item).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string dataType = "Y"; //Monthly            

            string adYear = date.Year.ToString();
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l=zh-tw&t=Y&d=2019&_=1578370079134
            string url = string.Format("https://www.tpex.org.tw/web/stock/aftertrading/daily_turnover/trn_result.php?l={0}&t={1}&d={2}&_={3}",
                lang, dataType, adYear, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
