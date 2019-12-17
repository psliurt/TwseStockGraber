using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DIndexSummary_Rsp
    {
        public string reportTitle { get; set; }
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalRecordsTRI { get; set; }
        
        public List<List<string>> aaData { get; set; }
        public List<List<string>> mmData { get; set; }
    }
}
