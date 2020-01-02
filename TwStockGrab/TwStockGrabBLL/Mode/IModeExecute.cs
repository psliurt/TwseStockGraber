using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.Logic;
using TwStockGrabBLL.Logic.DeskGraber;

namespace TwStockGrabBLL.Mode
{
    public interface IModeExecute
    {
        void Execute(DateParams dtParam, Graber gbr);
        void Execute(DateParams dtParam, List<Graber> gbrList);

        void Execute(DateParams dtParam, DGraber gbr);
        void Execute(DateParams dtParam, List<DGraber> gbrList);
    }

    public class DateParams
    {
        public DateTime? SingleDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? BackFromDate { get; set; }
    }
}
