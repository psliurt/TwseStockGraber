using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class BFI82U_Rsp
    {
        public string stat { get; set; }

        public string date { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<List<string>> data { get; set; }
        public Param02 @params { get; set; }
        public List<string> notes { get; set; }

    }

    public class Param02
    {
        public string response { get; set; }
        public string dayDate { get; set; }
        public string weekDate { get; set; }
        public string monthDate { get; set; }
        public string type { get; set; }
        public string _ { get; set; }
        public string controller { get; set; }
        public string format { get; set; }
        public string action { get; set; }
        public string lang { get; set; }
    }
}
