using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class MI_INDEX_Rsp
    {
        public string title { get; set; }
        
        public List<string> fields1 { get; set; }
        public List<string> fields2 { get; set; }
        public List<string> fields3 { get; set; }
        public List<string> fields4 { get; set; }
        public List<string> fields5 { get; set; }
        public List<string> fields6 { get; set; }
        public List<string> fields7 { get; set; }
        public List<string> fields8 { get; set; }
        public List<string> fields9 { get; set; }


        public string subtitle1 { get; set; }        
        public string subtitle2 { get; set; }
        public string subtitle3 { get; set; }
        public string subtitle4 { get; set; }
        public string subtitle5 { get; set; }
        public string subtitle6 { get; set; }
        public string subtitle7 { get; set; }
        public string subtitle8 { get; set; }
        public string subtitle9 { get; set; }

        public string date { get; set; }

        public List<List<string>> alignsStyle1 { get; set; }
        public List<List<string>> alignsStyle2 { get; set; }
        public List<List<string>> alignsStyle3 { get; set; }
        public List<List<string>> alignsStyle4 { get; set; }
        public List<List<string>> alignsStyle5 { get; set; }
        public List<List<string>> alignsStyle6 { get; set; }
        public List<List<string>> alignsStyle7 { get; set; }
        public List<List<string>> alignsStyle8 { get; set; }
        public List<List<string>> alignsStyle9 { get; set; }



        public List<string> notes2 { get; set; }
        public List<string> notes6 { get; set; }
        public List<string> notes8 { get; set; }
        public List<string> notes9 { get; set; }


        public string stat { get; set; }
        public List<Group> groups9 { get; set; }
        public Param01 @params { get; set; }
        
        public List<List<string>> data1 { get; set; }
        public List<List<string>> data2 { get; set; }
        public List<List<string>> data3 { get; set; }
        public List<List<string>> data4 { get; set; }
        public List<List<string>> data5 { get; set; }
        public List<List<string>> data6 { get; set; }
        public List<List<string>> data7 { get; set; }
        public List<List<string>> data8 { get; set; }
        public List<List<string>> data9 { get; set; }
    }
}
