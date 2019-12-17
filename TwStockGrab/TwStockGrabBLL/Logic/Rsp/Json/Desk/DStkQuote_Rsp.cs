using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DStkQuote_Rsp
    {
        public string reportDate { get; set; }
        public string reportTitle { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public string listNum { get; set; }
        public string totalAmount { get; set; }
        public string totalVolumn { get; set; }
        public string totalCount { get; set; }
        public List<List<string>> mmData { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}