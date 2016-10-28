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
            this.splitContainer1.SplitterMoved += new SplitterEventHandler(splitContainer1_SplitterMoved);
        }

        void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            
        }

        void PageTrading_SizeChanged(object sender, EventArgs e)
        {
            double pect = (double)this.ctrlPosition1.Height / (double)(this.ctrlPosition1.Height + this.ctrlOrder1.Height);
            int newHeight = (int)(this.Height * pect);

            this.splitContainer1.SplitterDistance = newHeight;
        }
    }
}
