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
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->三大法人->外資及陸資買賣超彙總表
    /// twt38u
    /// 本資訊自民國93年12月17日起開始提供
    /// https://www.twse.com.tw/zh/page/trading/fund/TWT38U.html
    /// </summary>
    public class Twt38uGraber : Graber
    {
        public Twt38uGraber() : base()
        {
            this._graberClassName = typeof(Twt38uGraber).Name;
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
            TWT38U_Rsp rsp = JsonConvert.DeserializeObject<TWT38U_Rsp>(responseContent);
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
                

        private void SaveToDatabase(TWT38U_Rsp rsp, DateTime dataDate)
        {
            List<twt38u> tmpAddList = new List<twt38u>();
            List<twt38u> tmpDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<twt38u>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
            }
                       
            foreach (var data in rsp.data)
            {
                string stockNo = data.ElementAt(1).Trim();

                twt38u obj = tmpDataList.Where(x => x.data_date == dataDate && x.stock_no == stockNo).FirstOrDefault();

                if (obj == null)
                {
                    tmpAddList.Add(new twt38u
                    {
                        data_date = dataDate,
                        is_big_trade = StarSignToBool(data.ElementAt(0).Trim()),
                        stock_no = stockNo,
                        stock_name = data.ElementAt(2),
                        buy_cnt = ToLongQ(data.ElementAt(3)),
                        sell_cnt = ToLongQ(data.ElementAt(4)),
                        cnt_diff = ToLongQ(data.ElementAt(5)),
                        self_buy_cnt = ToLongQ(data.ElementAt(6)),
                        self_sell_cnt = ToLongQ(data.ElementAt(7)),
                        self_cnt_diff = ToLongQ(data.ElementAt(8)),
                        total_buy_cnt = ToLongQ(data.ElementAt(9)),
                        total_sell_cnt = ToLongQ(data.ElementAt(10)),
                        total_cnt_diff = ToLongQ(data.ElementAt(11)),
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        title = string.Format("{0}", rsp.title)
                    });
                }               
            }

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.twt38u.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramUnderLine = GetTimeStamp();

            string url = string.Format("https://www.twse.com.tw/fund/TWT38U?response={0}&date={1}&_={2}",
                paramResponse, paramDate, paramUnderLine);

            return GetHttpResponse(url);
        }
    }
}
