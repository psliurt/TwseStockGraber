using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Query
{
    public class DateStockTypeParams : BasicWebContentParams
    {
        public DateTime DataDate { get; set; }

        public string SelectType { get; set; }
    }
}
