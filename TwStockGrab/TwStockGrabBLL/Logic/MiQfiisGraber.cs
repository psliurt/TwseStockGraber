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
    /// 交易資訊->三大法人->外資及陸資投資持股統計
    /// mi_qfiis
    /// 本資訊自民國93年2月11日起提供
    /// https://www.twse.com.tw/zh/page/trading/fund/MI_QFIIS.html
    /// </summary>
    public class MiQfiisGraber :Graber
    {
        public MiQfiisGraber() : base()
        {
            this._graberClassName = typeof(MiQfiisGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {
            List<string> selectTypeList = new List<string>();
            selectTypeList.Add("ALLBUT0999");

            foreach (string selectType in selectTypeList)
            {
                work_record record = null;
                if (GetOrCreateWorkRecord(dataDate,selectType, out record) ==false)
                {
                    string responseContent = GetWebContent(dataDate, selectType);
                    MI_QFIIS_Rsp rsp = JsonConvert.DeserializeObject<MI_QFIIS_Rsp>(responseContent);
                    if (rsp.data == null)
                    {
                        WriteEndRecord(record);
                        Sleep();
                    }
                    else if (rsp.data.Count() == 0)
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
            }
        }        

        private void SaveToDatabase(MI_QFIIS_Rsp rsp, DateTime dataDate)
        {
            List<mi_qfiis> tmpAddList = new List<mi_qfiis>();
            List<mi_qfiis> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<mi_qfiis>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }

            foreach (var data in rsp.data)
            {
                string selectType = rsp.selectType.Trim();
                string stockNo = data.ElementAt(0).Trim();

                mi_qfiis obj = tmpDataList.Where(x => x.select_type == selectType && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new mi_qfiis
                    {
                        data_date = dataDate,
                        select_type = rsp.selectType,
                        stock_no = data.ElementAt(0).Trim(),
                        stock_name = data.ElementAt(1).Trim(),
                        world_stock_no = data.ElementAt(2).Trim(),
                        issue_cnt = ToLongQ(data.ElementAt(3)),
                        investable_cnt = ToLongQ(data.ElementAt(4)),
                        hold_cnt = ToLongQ(data.ElementAt(5)),
                        investable_rate = ToDecimalQ(data.ElementAt(6)),
                        hold_rate = ToDecimalQ(data.ElementAt(7)),
                        law_max_rate = ToDecimalQ(data.ElementAt(8)),
                        law_china_max_rate = ToDecimalQ(data.ElementAt(9)),
                        change_reason = data.ElementAt(10).Trim(),
                        last_report_change_date = GetDateFromRocSlashStringQ(data.ElementAt(11)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }
                
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_qfiis.AddRange(tmpAddList);

                context.SaveChanges();                
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramType = type.Trim();
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/MI_QFIIS?response={0}&date={1}&selectType={2}&_={3}",
                paramResponse, paramDate, paramType, paramUnderLine);

            return GetHttpResponse(url);
        }

        
    }
}
