using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;

namespace TwStockGrabBLL.Filter.AfterMarket
{
    public class FilterLongStep2 : AfterMarketFilter
    {
        public FilterLongStep2(p_filter_stg stg) : base(stg)
        {

        }

        public override List<FilterResultData> ExecFilter()
        {
            DateTime dataDate = GetFilterDate();
            //找到最近四天(有開市)的日期

            //融資資料表
            //外資賣超資料表
            //借券資料表
            //融券資料表

            return null;
        }
    }
}
