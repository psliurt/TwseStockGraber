using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class HolidayData_Rsp
    {
        public bool success { get; set; }
        public HolidayDataResult result { get; set; }
    }

    public class HolidayDataResult
    {
        public string resource_id { get; set; }
        public int limit { get; set; }
        public string total { get; set; }
        public List<HolidayDataResultField> fields { get; set; }
        public List<HolidayDataResultRecord> records { get; set; }
    }

    public class HolidayDataResultField
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class HolidayDataResultRecord
    {
        public string date { get; set; }
        public string name { get; set; }
        public string isHoliday { get; set; }
        public string holidayCategory { get; set; }
        public string description { get; set; }
    }
}
