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
        void btnDemo1_Click(object sender, EventArgs e)
        {
            //_dataAPI.Connect();//("218.85.137.40", 7709);
            MDSymbol symbol = ctrlQuoteList.SymbolSelected;
            if (symbol != null)
            {
                logger.Info("symbol selected:" + symbol.Symbol);

                MDService.DataAPI.QryTickSnapshot(new MDSymbol[] { symbol });
            }
        }

        void btnDemo3_Click(object sender, EventArgs e)
        {
            //_dataAPI.QrySymbol();

            //quoteList.AddSymbol(_dataAPI.Symbols);
            //quoteView.Symbols = _dataAPI.Symbols;
        }

        void btnDemo2_Click(object sender, EventArgs e)
        {
            
        }


    }
}
