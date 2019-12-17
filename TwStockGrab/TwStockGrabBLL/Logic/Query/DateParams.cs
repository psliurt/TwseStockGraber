using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Query
{
    public class DateParams : BasicWebContentParams
    {
        /// <summary>
        /// 查詢的日期
        /// </summary>
        public DateTime DataDate { get; set; }
    }
}
