using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.MarketData;


namespace TradingLib.XTrader.Control
{
    public enum QuoteMouseEventType
    {
        /// <summary>
        /// 合约双击 用于进入KChart图标
        /// </summary>
        SymbolDoubleClick,

        /// <summary>
        /// 合约买价点击
        /// </summary>
        SymbolBuyClick,

        /// <summary>
        /// 合约卖价点击
        /// </summary>
        SymbolSellClick
    }



    public partial class ViewQuoteList
    {

        /// <summary>
        /// 聚合鼠标事件
        /// </summary>
        public event Action<MDSymbol, QuoteMouseEventType> MouseEvent;


    }
}
