using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TradingLib.XTrader.Stock
{
    public partial class PageSTKTradeToday : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_TRADE_TODAY;
        public string PageName { get { return _pageName; } }

       
        public PageSTKTradeToday()
        {
            InitializeComponent();
        }
    }
}
