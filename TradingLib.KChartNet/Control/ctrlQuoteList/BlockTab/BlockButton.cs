using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.MarketData;

namespace TradingLib.XTrader.Control
{
    public class BlockButton
    {
        public BlockButton(string title, Predicate<MDSymbol> filter,EnumQuoteListType quoteType)
        {
            this.Title = title;
            this.SymbolFilter = filter;

            this.StartX = 0;
            this.EndX = 0;
            this.Index = 0;
            this.MouseOver = false;
            this.Selected = false;
            this.QuoteType = quoteType;
        }

        public Predicate<MDSymbol> SymbolFilter { get; set; }
        /// <summary>
        /// 按钮标题
        /// </summary>
        public string Title { get; set; }


        internal EnumQuoteListType QuoteType { get; set; }
        /// <summary>
        /// 按钮宽度
        /// </summary>
        internal int StartX { get; set; }


        internal int EndX { get; set; }

        /// <summary>
        /// 按钮序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 鼠标停留
        /// </summary>
        internal bool MouseOver { get; set; }

        /// <summary>
        /// 选中
        /// </summary>
        internal bool Selected { get; set; }


    }
}
