﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class MI_INDEX_Top20_Rsp
    {
        public string stat { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public List<string> fields1 { get; set; }
        public List<List<string>> data { get; set; }
        public List<string> notes1 { get; set; }        
    }
}
