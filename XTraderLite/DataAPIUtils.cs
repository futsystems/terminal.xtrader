using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.MarketData;

namespace XTraderLite
{
    public static class DataAPIUtils
    {
        public static MDSymbol GetSymbol(this IMarketDataAPI api,string exchange, string symbol)
        {
            return api.Symbols.FirstOrDefault(sym => sym.Exchange == exchange && sym.Symbol == symbol);
        }
    }
}
