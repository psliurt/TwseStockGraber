using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwStockGrabBLL
{
    public static class Kit
    {
        public static int ToInt(int? original)
        {
            if (original.HasValue == false)
            {
                return 0;
            }
            return original.Value;
        }

        public static long ToLong(long? original)
        {
            if (original.HasValue == false)
            {
                return 0;
            }
            return original.Value;
        }

        public static decimal ToDecimal(decimal? original)
        {
            if (original.HasValue == false)
            {
                return 0;
            }
            return original.Value;
        }

        public static decimal ToWavePercent(decimal? fraction, decimal? denominator)
        {
            if (denominator.HasValue == false)
            {
                return 0;
            }

            if (denominator.Value == 0)
            {
                return 0;
            }

            if (fraction.HasValue == false)
            {
                return 0;
            }

            if (fraction.Value == 0)
            {
                return 0;
            }

            return (fraction.Value / denominator.Value) * 100;
        }

        private static HashSet<string> _snBag = new HashSet<string>();

        public static string GetSN()
        {
            string sn = null;
            if(_snBag.Count() < 1000)
            {
                while (_snBag.Count() < 1024)
                {
                    _snBag.Add(GenerateSN());
                }
            }
            sn = _snBag.FirstOrDefault();
            _snBag.Remove(sn);
            return sn;
        }

        private static string GenerateSN()
        {
            DateTime dt = DateTime.Now;
            Random rnd = new Random((int)dt.Ticks);
            string yyyyHex = NumberStrToHexStr(dt.Year).PadLeft(3, '0');
            int monthDayNumber = Convert.ToInt32(dt.ToString("MMdd"));
            string monthDayHex = NumberStrToHexStr(monthDayNumber).PadLeft(3, '0');
            int hourMinuteNumber = Convert.ToInt32(dt.ToString("HHmm"));
            string hourMinuteHex = NumberStrToHexStr(hourMinuteNumber).PadLeft(3, '0');
            int secNumber = Convert.ToInt32(dt.ToString("ssfff"));
            string secHex = NumberStrToHexStr(secNumber).PadLeft(4, '0');
            string rndHex1 = NumberStrToHexStr(rnd.Next(1000)).PadLeft(3, '0');
            string rndHex2 = NumberStrToHexStr(rnd.Next(100)).PadLeft(2, '0');
            string rndHex3 = NumberStrToHexStr(rnd.Next(100)).PadLeft(2, '0');
            return string.Format("{0}{1}{2}{3}{4}{5}{6}", yyyyHex, monthDayHex, hourMinuteHex, secHex, rndHex1, rndHex2, rndHex3);

        }

        private static string NumberStrToHexStr(int number)
        {
            return number.ToString("x");
        }
    }
}
