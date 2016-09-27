using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.MarketData;
using TradingLib.XTrader.Control;
using TradingLib.MDClient;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {
        void InitQuoteLists()
        {
            ctrlQuoteList.SymbolVisibleChanged += new EventHandler<SymbolVisibleChangeEventArgs>(ctrlQuoteList_SymbolVisibleChanged);
            
        }

        void ctrlQuoteList_SymbolVisibleChanged(object sender, SymbolVisibleChangeEventArgs e)
        {
            logger.Info("QuoteSymbol List Changed");

            foreach (var g in e.Symbols.GroupBy(s => s.Exchange))
            {
                List<string> symlist = new List<string>();
                string exchange = g.Key;
                foreach(var val in g)
                {
                    symlist.Add(val.Symbol);
                }
                mdClient.RegisterSymbol(exchange, symlist.ToArray());
            }

            
        }
    }
}
