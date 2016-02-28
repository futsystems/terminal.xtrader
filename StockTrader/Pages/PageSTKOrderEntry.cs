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
    public partial class PageSTKOrderEntry : UserControl,API.IPage
    {
        public EnumPageType PageType { get { return EnumPageType.OrderEntryPage; } }

        public PageSTKOrderEntry()
        {
            InitializeComponent();
            this.Mode = 0;
        }

        [DefaultValue(0)]
        int _mode = 0;
        public int Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode < 0 || _mode > 2) return;
                _mode = value;
                //买入
                if (_mode == 0)
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = true;
                    ctOrderSenderSTK2.Visible = false;
                }
                //卖出
                else if (_mode == 1)
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = false;
                    ctOrderSenderSTK2.Visible = false;
                }
                //双向
                else
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = true;
                    ctOrderSenderSTK2.Visible = true;
                    ctOrderSenderSTK2.Side = false;
                    
                }

                Invalidate();
            }
        }
    }
}
