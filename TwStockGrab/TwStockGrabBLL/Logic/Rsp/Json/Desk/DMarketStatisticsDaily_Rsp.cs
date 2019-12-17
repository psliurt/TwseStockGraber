using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DMarketStatisticsDaily_Rsp
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string csvFile { get; set; }
        public List<StaticsDetail> detail { get; set; }
    }

    public class StaticsDetail
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string volumn { get; set; }
        public string count { get; set; }
    }
}
