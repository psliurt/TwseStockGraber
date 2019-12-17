using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;

namespace TwStockGrabBLL.Filter.AfterMarket
{
    public abstract class AfterMarketFilter
    {
        protected p_filter_stg _stgSetting { get; set; }
        public AfterMarketFilter(p_filter_stg stg)
        {
            this._stgSetting = stg;
        }

        public abstract List<FilterResultData> ExecFilter();

        protected DateTime GetFilterDate()
        {
            DateTime current = DateTime.Now;

            if (current.Hour >= 17)
            {
                return DateTime.Today;
            }
            else
            {
                return DateTime.Today.AddDays(-1);
            }
        }
    }
}
