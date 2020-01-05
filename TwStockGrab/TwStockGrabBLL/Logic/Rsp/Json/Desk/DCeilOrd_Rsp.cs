using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DCeilOrd_Rsp
    {
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}