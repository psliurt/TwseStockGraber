using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Logic.Rsp.Json.Desk
{
    public class DPeratioPera_Rsp
    {
        public string datetime { get; set; }
        public int iTotalRecords { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}
