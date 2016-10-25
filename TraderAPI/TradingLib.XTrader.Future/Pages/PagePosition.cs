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
    public partial class PagePosition : UserControl, IPage
    {
        string _pageName = PageTypes.PAGE_POSITION;
        public string PageName { get { return _pageName; } }

        public PagePosition()
        {
            InitializeComponent();
        }
    }
}
