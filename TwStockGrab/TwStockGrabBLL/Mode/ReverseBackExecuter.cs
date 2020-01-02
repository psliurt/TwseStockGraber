using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic;
using TwStockGrabBLL.Logic.DeskGraber;

namespace TwStockGrabBLL.Mode
{
    public class ReverseBackExecuter : IModeExecute
    {
        private DateTime _minDataDate = new DateTime(2000, 1, 1);

        public void Execute(DateParams dtParam, Graber gbr)
        {
            DateTime from = dtParam.BackFromDate.Value.Date;
            
            do
            {
                gbr.DoJob(from);
                from = from.AddDays(-1);
                Sleep();
            } while (from >= _minDataDate);
        }

        public void Execute(DateParams dtParam, List<Graber> gbrList)
        {
            DateTime from = dtParam.BackFromDate.Value.Date;
            

            do
            {
                foreach (var gbr in gbrList)
                {
                    gbr.DoJob(from);
                }
                from = from.AddDays(-1);
                Sleep();
            } while (from >= _minDataDate);
        }

        public void Execute(DateParams dtParam, DGraber gbr)
        {
            
        }

        public void Execute(DateParams dtParam, List<DGraber> gbrList)
        {
            
        }

        private void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(5000);
            } while (rnd < 3000);
            Thread.Sleep(rnd);
        }
    }
}
