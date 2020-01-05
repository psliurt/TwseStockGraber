using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DLend_Rsp
    {
        public string DATE { get; set; }
        public string STK_CODE { get; set; }
        public string BRK_NAME { get; set; }
        public string LOANQTY { get; set; }
        public string HIGHPR { get; set; }
        public string MTHQTY { get; set; }
        public string LtoH { get; set; }
        public string SHORTQTY { get; set; }

        public string reportDateStart { get; set; }
        public string reportDateEnd { get; set; }
        public int iTotalRecords { get; set; }
        public List<List<string>> aaData { get; set; }

        
    }
}
