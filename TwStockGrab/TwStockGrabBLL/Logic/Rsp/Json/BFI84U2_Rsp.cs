using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class BFI84U2_Rsp
    {
        public string stat { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public Param03 @params { get; set; }
        public List<List<string>> data { get; set; }
        public List<string> notes { get; set; }        
    }

    public class Param03
    {
        public string response { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string stockNo { get; set; }        
        public string _ { get; set; }
        public string controller { get; set; }
        public string format { get; set; }
        public string action { get; set; }
        public string lang { get; set; }
    }
}