using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
namespace TwStockGrabBLL.Filter.AfterMarket
{
    public class FilterFactory
    {
        public static AfterMarketFilter Create(string filterCode)
        {
            string className = "";
            p_filter_stg stg = null;
            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                stg = ctx.Set<p_filter_stg>().Where(x => x.stg_code == filterCode).FirstOrDefault();
                className = stg.stg_class_name;
            }

            //className = "FilterNo1";

            Type instanceType  =Type.GetType(string.Format("TwStockGrabBLL.Filter.AfterMarket.{0}", className));

            object instance = Activator.CreateInstance(instanceType, new object[] { stg });
            if (instance != null)
            {
                return (AfterMarketFilter)instance;
            }
            return null;
        }
    }
}
