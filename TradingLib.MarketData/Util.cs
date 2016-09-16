using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public static class Util
    {

        public static string GetFormat(this MDSymbol symbol)
        {
            return "{0:F" + symbol.Precision.ToString() + "}";
        }
    }
}
