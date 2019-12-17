using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class BFIAUU_Rsp
    {
        public string stat { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<List<string>> data { get; set; }
        public string selectType { get; set; }
        public List<string> notes { get; set; }
        public string sub { get; set; }
        public string buyNo { get; set; }
        public string stockType { get; set; }
        public string sideL { get; set; }
        public string sideR { get; set; }



    }
}
