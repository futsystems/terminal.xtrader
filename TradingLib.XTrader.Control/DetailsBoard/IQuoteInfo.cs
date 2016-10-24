using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CStock;
using TradingLib.MarketData;

namespace TradingLib.XTrader
{
    public interface IQuoteInfo
    {
        EnumQuoteInfoType QuoteInfoType { get; }

        void OnTick(MDSymbol symbol);

        void SetSymbol(MDSymbol symbol);


        bool Visible { get; set; }

        int DefaultHeight { get; }

        int Height { get; set; }
    }
}
