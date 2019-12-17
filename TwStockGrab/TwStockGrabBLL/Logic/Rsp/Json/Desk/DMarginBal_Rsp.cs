using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DMarginBal_Rsp
    {        
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public List<List<string>> aaData { get; set; }
        public List<string> tfootData_one { get; set; }
        public List<string> tfootData_two { get; set; }
    }
}
