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
    /// 首頁 > 上櫃 > 三大法人 > 各類股僑外資及陸資持股比例表
    /// d_qfiisect
    /// 本資訊自民國96年1月起開始提供 實際由 2007/4/23 開始提供
    /// 網頁位置
    /// https://www.tpex.org.tw/web/stock/3insti/qfii_sect/qfiisect.php?l=zh-tw
    /// </summary>
    public class DQfiisectGraber : DGraber
    {
        public DQfiisectGraber() : base()
        {
            this._graberClassName = typeof(DQfiisectGraber).Name;
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
            DQfiisect_Rsp rsp = JsonConvert.DeserializeObject<DQfiisect_Rsp>(responseContent);
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

        private void SaveToDatabase(DQfiisect_Rsp rsp, DateTime dataDate)
        {   
            List<d_qfiisect> tmpAddList = new List<d_qfiisect>();
            List<d_qfiisect> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<d_qfiisect>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.aaData)
            {
                string sectorItem = data.ElementAt(0).Trim();

                d_qfiisect existItem = tmpDataList.Where(x => x.sector_item == sectorItem && x.data_date == dataDate).FirstOrDefault();
                if (existItem == null)
                {
                    tmpAddList.Add(new d_qfiisect
                    {
                        data_date = dataDate,
                        sector_item = sectorItem,
                        compony_count = ToIntQ(data.ElementAt(1)),
                        all_issue_stock_count = ToLongQ(data.ElementAt(2).Trim()),
                        hold_stock_count = ToLongQ(data.ElementAt(3).Trim()),                        
                        hold_percent = ToDecimalQ(data.ElementAt(4)),                        
                        title = rsp.iTotalRecords.ToString(),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now
                    });

                }
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.d_qfiisect.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string lang = "zh-tw";
            string rocDate = ParseADDateToRocString(date);
            string paramUnderLine = GetTimeStamp();

            //https://www.tpex.org.tw/web/stock/3insti/qfii_sect/qfiisect_result.php?l=zh-tw&d=108/12/04&_=1575790353411
            string url = string.Format("https://www.tpex.org.tw/web/stock/3insti/qfii_sect/qfiisect_result.php?l={0}&d={1}&_={2}",
                lang, rocDate, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
