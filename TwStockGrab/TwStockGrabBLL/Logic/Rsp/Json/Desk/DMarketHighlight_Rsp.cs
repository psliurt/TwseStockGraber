using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DMarketHighlight_Rsp
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string listedNum { get; set; }
        public string capital { get; set; }
        public string companyValue { get; set; }
        public string tradeAmount { get; set; }
        public string tradeVolumn { get; set; }
        public string close { get; set; }
        public string change { get; set; }
        public string upNum { get; set; }
        public string upStopNum { get; set; }
        public string downNum { get; set; }
        public string downStopNum { get; set; }
        public string noChangeNum { get; set; }
        public string noTradeNum { get; set; }
        public string rptNote { get; set; }
    }
}
