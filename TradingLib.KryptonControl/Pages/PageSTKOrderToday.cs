using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.KryptonControl;

namespace TradingLib.KryptonControl
{
    public partial class PageSTKOrderToday : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ORDER_TODAY;
        public string PageName { get { return _pageName; } }


        public PageSTKOrderToday()
        {
            InitializeComponent();
        }
    }
}
