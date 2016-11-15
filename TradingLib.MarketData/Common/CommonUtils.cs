using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public static class CommonUtils
    {

        public static string GetFormat(this MDSymbol symbol)
        {
            return "{0:F" + symbol.Precision.ToString() + "}";
        }

        public static DateTime GetLocalDateTime(this MDSymbol symbol, int date, int time)
        {
            DateTime dt = Utils.ToDateTime(date, time);
            if (symbol.TimeZoneOffset == 0) return dt;
            return dt + symbol.TimeSpanOffset;
        }

        public static bool IsValid(this TDX ticksanpshot)
        {
            if (ticksanpshot.Price > 0) return true;
            return false;
        }

        
    }
}
