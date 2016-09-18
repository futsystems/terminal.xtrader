using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.MarketData;


namespace TradingLib.KryptonControl
{
    /// <summary>
    /// 报价列表显示合约变化事件参数
    /// 1.切换板块 合约整体变化
    /// 2.上下翻页或者scroll导致显示的合约变化
    /// </summary>
    public class SymbolVisibleChangeEventArgs:EventArgs
    {

        public SymbolVisibleChangeEventArgs(int start, int end, MDSymbol[] symbols)
        {
            this.StartIndex = start;
            this.EndIndex = end;
            this.Symbols = symbols;
        }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public MDSymbol[] Symbols { get; set; }
    }
}
