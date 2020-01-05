using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    public class HolidayDataFetcher
    {
        public void Fetch()
        {
            string url = string.Format("http://data.ntpc.gov.tw/api/v1/rest/datastore/382000000A-000077-002");
            string holidayData = GetHttpResponse(url);
            HolidayData_Rsp rsp = JsonConvert.DeserializeObject<HolidayData_Rsp>(holidayData);
            SaveToDatabase(rsp);

        }

        private void SaveToDatabase(HolidayData_Rsp rsp)
        {
            List<c_holiday> tmpAddList = new List<c_holiday>();
            List<c_holiday> tmpDataList = null;
            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpDataList = context.Set<c_holiday>().AsNoTracking().ToList();
            }

            foreach (var data in rsp.result.records)
            {
                DateTime holidayDate = ParseRocSlashDate(data.date);

                c_holiday obj =
                    tmpDataList.Where(x => x.holiday_date == holidayDate).FirstOrDefault();

                if (obj == null)
                {

                    tmpAddList.Add(new c_holiday
                    {
                        create_at = DateTime.Now,
                        update_at = DateTime.Now,
                        holiday_date = holidayDate,
                        holiday_name = data.name,
                        holiday_category = data.holidayCategory,
                        holiday_desc = data.description,
                        is_holiday = ParseIsHoliday(data.isHoliday)
                    });

                }

            }


            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.c_holiday.AddRange(tmpAddList);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// 送出http GET 請求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string GetHttpResponse(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream inputResponseStream = null;
            string responseContent = "";

            inputResponseStream = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(inputResponseStream))
            {
                responseContent = sr.ReadToEnd();
            }

            return responseContent;
        }

        private DateTime ParseRocSlashDate(string rocString)
        {
            string[] dateParts = rocString.Split('/');

            return new DateTime(Convert.ToInt32(dateParts[0]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[2]));
        }

        private bool ParseIsHoliday(string isHolidayString)
        {
            if (string.IsNullOrEmpty(isHolidayString))
            {
                return false;
            }

            if (isHolidayString.Trim() == "是")
            {
                return true;
            }

            return false;
        }
    }
}
