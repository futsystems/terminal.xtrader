using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader
{
    public class Util
    {
        public static string GetAmountFormated(double val)
        {
            if (val > 100000000)
            {
                return string.Format("{0:F2}亿", val / 100000000);
            }
            else if (val > 10000)
            {
                return string.Format("{0:F0}万", val / 10000);
            }
            else
            {
                return string.Format("{0:F0}", val / 10000);
            }
        }

    }
}
