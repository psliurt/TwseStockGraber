using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class MI_INDEX_ETF_Rsp
    {
        public List<Group> groups1 { get; set; }
        public List<string> notes1 { get; set; }
        public string title { get; set; }
        public Param01 @params { get; set; }
        public string stat { get; set; }
        public List<string> fields1 { get; set; }
        public string subtitle1 { get; set; }
        public List<List<string>> data1 { get; set; }
        public string date { get; set; }
        public List<List<string>> alignsStyle1 { get; set; }
    }
}
