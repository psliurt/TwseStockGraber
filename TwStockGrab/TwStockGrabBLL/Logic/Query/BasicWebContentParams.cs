using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwStockGrabBLL.Logic.Query
{
    public class BasicWebContentParams
    {
        /// <summary>
        /// underline timestamp
        /// </summary>
        public string UnderLine { get; set; }
        /// <summary>
        /// the response type, almost be "json"
        /// </summary>
        public string RspContentType { get; set; }
    }
}