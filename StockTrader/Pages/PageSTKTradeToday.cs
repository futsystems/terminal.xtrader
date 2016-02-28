using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StockTrader.API;

namespace StockTrader
{
    public partial class PageSTKTradeToday : UserControl,IPage
    {
        public EnumPageType PageType { get { return EnumPageType.TradeTodayPage; } }
        public PageSTKTradeToday()
        {
            InitializeComponent();
        }
    }
}
