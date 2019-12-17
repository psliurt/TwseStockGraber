using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    /// <summary>
    /// 上櫃股票三大法人日彙總交易資訊
    /// </summary>
    public class D3itrdsumDaily_Rsp
    {
        public string reportTitle { get; set; }
        public string reportDate { get; set; }
        public int iTotalRecords { get; set; }
        public string csvContentNote { get; set; }
        public string csvFileTime { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}