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
    public class PeriodExecuter : IModeExecute
    {
        public void Execute(DateParams dtParam, Graber gbr)
        {
            DateTime from = dtParam.FromDate.Value.Date;
            DateTime to = dtParam.ToDate.Value.Date;            

            do
            {
                gbr.DoJob(from);                
                from = from.AddDays(1);
                Sleep();
            } while (from <= to);
        }

        public void Execute(DateParams dtParam, List<Graber> gbrList)
        {
            DateTime from = dtParam.FromDate.Value.Date;
            DateTime to = dtParam.ToDate.Value.Date;            

            do
            {
                foreach (var gbr in gbrList)
                {
                    gbr.DoJob(from);
                }                
                from = from.AddDays(1);
                Sleep();
            } while (from <= to);
        }

        public void Execute(DateParams dtParam, DGraber gbr)
        {
            DateTime from = dtParam.FromDate.Value.Date;
            DateTime to = dtParam.ToDate.Value.Date;

            do
            {
                gbr.DoJob(from);
                from = from.AddDays(1);
                Sleep();
            } while (from <= to);
        }

        public void Execute(DateParams dtParam, List<DGraber> gbrList)
        {
            DateTime from = dtParam.FromDate.Value.Date;
            DateTime to = dtParam.ToDate.Value.Date;

            do
            {
                foreach (var gbr in gbrList)
                {
                    gbr.DoJob(from);
                }
                from = from.AddDays(1);
                Sleep();
            } while (from <= to);
        }

        private void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(4500);
            } while (rnd < 2500);
            Thread.Sleep(rnd);
        }
    }
}
