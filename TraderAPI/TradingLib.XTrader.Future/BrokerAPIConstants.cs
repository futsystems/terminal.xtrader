using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Future
{
    public class BrokerAPIConstants
    {
        static BrokerAPIConstants()
        {
            IsLongSymbolName = false;
        }
        public static bool IsLongSymbolName { get; set; }
    }
}
