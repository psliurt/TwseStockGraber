using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL.Mode
{
    public class ModeFactory
    {
        public static IModeExecute GetExecuter(ExecuterType t)
        {
            switch (t)
            {
                case ExecuterType.SingleDay:
                    return new SingleDayExecuter();
                case ExecuterType.Period:
                    return new PeriodExecuter();
                case ExecuterType.ReverseBack:
                    return new ReverseBackExecuter();
                default:
                    return null;
            }
        }
    }

    public enum ExecuterType
    {
        SingleDay,
        Period,
        ReverseBack
    }
}
