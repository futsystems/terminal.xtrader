using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

using TradingLib.KryptonControl;

namespace TradingLib.KryptonControl
{
    public partial class PageSTKOrderEntry : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ORDER_ENTRY;
        public string PageName { get { return _pageName; } }

       

        public PageSTKOrderEntry()
        {
            InitializeComponent();
            this.Mode = 0;

            //绑定按钮事件
            WireEvent();
        }

        void WireEvent()
        {
            //btnCancelAll.Click += new EventHandler(btnCancelAll_Click);
            //btnCancelBuy.Click += new EventHandler(btnCancelBuy_Click);
            //btnCancelSell.Click += new EventHandler(btnCancelSell_Click);
            //btnRefresh.Click += new EventHandler(btnRefresh_Click);

            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            CoreService.TradingInfoTracker.ResumeData();
        }

        void EventOther_OnResumeDataEnd()
        {
            //btnCancelAll.Enabled = true;
            //btnCancelBuy.Enabled = true;
            //btnCancelSell.Enabled = true;

            //btnRefresh.Enabled = true;
        }

        void EventOther_OnResumeDataStart()
        {
            //btnCancelAll.Enabled = false;
            //btnCancelBuy.Enabled = false;
            //btnCancelSell.Enabled = false;

            //btnRefresh.Enabled = false;
        }

        #region 撤单按钮事件操作

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

        #endregion

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
                    //ctOrderSenderSTK2.Visible = false;
                }
                //卖出
                else if (_mode == 1)
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = false;
                    //ctOrderSenderSTK2.Visible = false;
                }
                //双向
                else
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = true;
                   //ctOrderSenderSTK2.Visible = true;
                    //ctOrderSenderSTK2.Side = false;
                    
                }

                Invalidate();
            }
        }
    }
}
