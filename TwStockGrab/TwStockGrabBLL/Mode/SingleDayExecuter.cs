using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic;
using TwStockGrabBLL.Logic.DeskGraber;

namespace TwStockGrabBLL.Mode
{
    public class SingleDayExecuter : IModeExecute
    {
        public void Execute(DateParams dtParam, Graber gbr)
        {
            gbr.DoJob(dtParam.SingleDate.Value.Date);
        }

        public void Execute(DateParams dtParam, List<Graber> gbrList)
        {
            foreach (var gbr in gbrList)
            {
                gbr.DoJob(dtParam.SingleDate.Value.Date);
            }
        }

        public void Execute(DateParams dtParam, DGraber gbr)
        {
            
        }

        public void Execute(DateParams dtParam, List<DGraber> gbrList)
        {
            
        }
    }
}
