﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json
{
    public class MI_QFIIS_CAT_Rsp
    {
        public string stat { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<List<string>> data { get; set; }
    }
}
