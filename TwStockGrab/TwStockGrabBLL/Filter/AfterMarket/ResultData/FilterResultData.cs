using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Filter.AfterMarket.ResultData
{
    public class FilterResultData
    {
        public DateTime DataDate { get; set; }
        public string StockNo { get; set; }
        public string StockName { get; set; }
        public string FilterName { get; set; }
        public string Note { get; set; }
    }
}
