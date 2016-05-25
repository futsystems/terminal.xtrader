using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.KryptonControl;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace StockTrader
{
    public partial class PageSTKOrderCancel : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ORDER_CANCEL;
        public string PageName { get { return _pageName; } }

       
        public PageSTKOrderCancel()
        {
            InitializeComponent();

            btnCancelAll.Click += new EventHandler(btnCancelAll_Click);
            btnCancelBuy.Click += new EventHandler(btnCancelBuy_Click);
            btnCancelSell.Click += new EventHandler(btnCancelSell_Click);
        }

        void btnCancelSell_Click(object sender, EventArgs e)
        {
            if (TraderHelper.ConfirmWindow("确认撤掉所有未成交卖出委托?") == DialogResult.Yes)
            {
                foreach (var order in CoreService.TradingInfoTracker.OrderTracker.Where(o => o.IsPending() && (!o.Side)))
                {
                    CoreService.TLClient.ReqCancelOrder(order.id);
                }
            }
        }

        void btnCancelBuy_Click(object sender, EventArgs e)
        {
            if (TraderHelper.ConfirmWindow("确认撤掉所有未成交买入委托?") == DialogResult.Yes)
            {
                foreach (var order in CoreService.TradingInfoTracker.OrderTracker.Where(o => o.IsPending() && (o.Side)))
                {
                    CoreService.TLClient.ReqCancelOrder(order.id);
                }
            }
        }

        void btnCancelAll_Click(object sender, EventArgs e)
        {
            if (TraderHelper.ConfirmWindow("确认撤掉所有未成交委托?") == DialogResult.Yes)
            {
                foreach (var order in CoreService.TradingInfoTracker.OrderTracker.Where(o => o.IsPending()))
                {
                    CoreService.TLClient.ReqCancelOrder(order.id);
                }
            }
        }


    }
}
