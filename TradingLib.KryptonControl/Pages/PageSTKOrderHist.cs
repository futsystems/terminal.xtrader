﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using TradingLib.KryptonControl;

namespace TradingLib.KryptonControl
{
    public partial class PageSTKOrderHist : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ORDER_HIST;
        public string PageName { get { return _pageName; } }

        ILog logger = LogManager.GetLogger("PageSTKOrderHist");
      
        public PageSTKOrderHist()
        {
            InitializeComponent();

            CoreService.EventQry.OnRspXQryOrderResponse += new Action<Order, RspInfo, int, bool>(EventQry_OnRspXQryOrderResponse);
            btnQry.Click += new EventHandler(btnQry_Click);
        }

        /// <summary>
        /// 响应查询回报
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void EventQry_OnRspXQryOrderResponse(Order arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryid) return;//查询RequestID不一致表面非当前控件查询 直接返回
            //返回委托对象不为空则调用OrderView进行输出显示
            if (arg1 != null)
            {
                ctOrderViewSTK1.GotOrder(arg1);
            }
            //如果是最后一条查询则重置查询ID和按钮可用
            if (arg4)
            {
                _qryid = 0;
                btnQry.Enabled = true;
            }
        }

        int _qryid = 0;
        DateTime _lastqrytime = DateTime.Now;
        
        void btnQry_Click(object sender, EventArgs e)
        {
            //查询频率过滤
            if (DateTime.Now.Subtract(_lastqrytime).TotalSeconds < UIConstant.QRYINTERVAL)
            {
                TraderHelper.WindowMessage("请勿频繁查询");
                return;
            }

            logger.Info("Qry Hist Order");
            ctOrderViewSTK1.Clear();
            _lastqrytime = DateTime.Now;
            btnQry.Enabled = false;
            _qryid = CoreService.TLClient.ReqXQryOrder(Util.ToTLDate(start.Value), Util.ToTLDate(end.Value));
        }


    }
}