using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StockTrader.API;
using TradingLib.KryptonControl;

namespace StockTrader
{
    public partial class PageSTKTradeToday : UserControl,IPage
    {
        string _pageName = "KCHART";
        public string PageName { get { return _pageName; } }

        public EnumPageType PageType { get { return EnumPageType.TradeTodayPage; } }
        public PageSTKTradeToday()
        {
            InitializeComponent();
        }
    }
}
