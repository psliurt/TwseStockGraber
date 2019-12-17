using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    /// <summary>
    /// 這個類別之後可能再也不會用到了
    /// </summary>
    public class MI_INDEX_01_20_Rsp
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

    public class Group
    {
        public int start { get; set; }
        public int span { get; set; }
        public string title { get; set; }
    }

    public class Param01
    {
        public string response { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string _ { get; set; }
        public string controller { get; set; }
        public string format { get; set; }
        public string action { get; set; }
        public string lang { get; set; }
    }

    


}
