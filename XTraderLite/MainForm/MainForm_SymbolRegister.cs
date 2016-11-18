using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class MainForm
    {
        /// <summary>
        /// 订阅中的合约列表
        /// </summary>
        List<MDSymbol> symbolRegister = new List<MDSymbol>();

        /// <summary>
        /// 获得需要订阅的合约列表
        /// </summary>
        IEnumerable<MDSymbol> GetSymbolsNeeded()
        {
            IEnumerable<MDSymbol> symlist = new List<MDSymbol>();
            //1.底部高亮合约
            symlist = symlist.Union(ctrlSymbolHighLight.Symbols);

            //2.当前K线图合约
            symlist = symlist.Union(new MDSymbol[] { ctrlKChart.Symbol });

            //3.如果合约报价列表可见 合并对应可见合约
            if (ctrlQuoteList.Visible)
            {
                symlist = symlist.Union(ctrlQuoteList.SymbolVisible);
            }

            return symlist;
        }
    }
}
