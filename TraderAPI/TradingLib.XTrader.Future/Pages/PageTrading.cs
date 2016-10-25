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
    public partial class PageTrading : UserControl, IPage
    {
        string _pageName = PageTypes.PAGE_TRADING;
        public string PageName { get { return _pageName; } }

        public PageTrading()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(PageTrading_SizeChanged);
        }

        void PageTrading_SizeChanged(object sender, EventArgs e)
        {
            this.ctrlPosition1.Height = (this.Height - 2) / 2;
            this.ctrlOrder1.Location = new Point(0, this.ctrlPosition1.Height + 2);
            this.ctrlOrder1.Height = this.Height - this.ctrlPosition1.Height - 2;
        }
    }
}
