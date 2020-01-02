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

        /// <summary>
        /// TODO:看看有沒有辦法得知哪一天有放假
        /// 目前 https://data.gov.tw/dataset/26557 新北市政府有資料
        /// 台北市政府也有 https://data.taipei/#/dataset/detail?id=9cfba4c6-3caa-48ff-a926-f903c74c5736
        /// </summary>
        /// <returns></returns>
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
