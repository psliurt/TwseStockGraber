using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DMgratio_Rsp
    {
        public string MAvgfinance { get; set; }
        public string MfinancePercent { get; set; }
        public string MAvgbearish { get; set; }
        public string MbearishPercent { get; set; }
        public string MAvgTotal { get; set; }
        public string MAvgPercent { get; set; }        

        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}
