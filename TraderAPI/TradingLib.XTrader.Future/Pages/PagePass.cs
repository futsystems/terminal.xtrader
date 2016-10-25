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
    public partial class PagePass : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_PASS;
        public string PageName { get { return _pageName; } }

        public PagePass()
        {
            InitializeComponent();
        }
    }
}
