using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader.Future
{
    public static class Util_Symbol
    {
        public static string GetSymbolTitle(this Symbol symbol)
        {
            if (Constants.SymbolTitleStyle == 2)
                return symbol.Symbol;
            return symbol.GetCodeNumSuffix(Constants.SymbolTitleStyle == 0);
        }
    }
}
