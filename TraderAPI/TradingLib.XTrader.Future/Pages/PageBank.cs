using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.XTrader.Future
{
    public partial class PageBank : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBank()
        {
            InitializeComponent();
        }
    }
}
