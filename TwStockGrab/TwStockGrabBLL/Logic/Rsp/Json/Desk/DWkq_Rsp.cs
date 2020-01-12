using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DWkq_Rsp
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string foot1 { get; set; }
        public string foot2 { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}
