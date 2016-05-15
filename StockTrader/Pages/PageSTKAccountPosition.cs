using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;
using StockTrader.API;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using TradingLib.KryptonControl;

namespace StockTrader
{
    public partial class PageSTKAccountPosition : UserControl,IPage
    {
        string _pageName = "KCHART";
        public string PageName { get { return _pageName; } }

        ILog logger = LogManager.GetLogger("PageSTKAccountPosition");
        public EnumPageType PageType { get { return EnumPageType.AccountPage; } }
        public PageSTKAccountPosition()
        {
            InitializeComponent();

            CoreService.EventQry.OnRspQryAccountInfoResponse += new Action<AccountInfo, RspInfo, int, bool>(EventQry_OnRspQryAccountInfoResponse);
        }

        public void QryAccountInfo()
        {
            _qryid = CoreService.TLClient.ReqQryAccountInfo();
        }
        int _qryid = 0;
        void EventQry_OnRspQryAccountInfoResponse(AccountInfo arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryid) return;//查询RequestID不一致表面非当前控件查询 直接返回
            //返回委托对象不为空则调用OrderView进行输出显示
            if (arg1 != null)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action<AccountInfo, RspInfo, int, bool>(EventQry_OnRspQryAccountInfoResponse), new object[] { arg1, arg2, arg3, arg4 });
                }
                else
                {
                    lbCash.Text = "";
                    lbFrozenFund.Text = arg1.StkMarginFrozen.ToFormatStr();
                    lbAvabileFund.Text = arg1.StkAvabileFunds.ToFormatStr();
                    lbSTKMarketValue.Text = arg1.StkMarketValue.ToFormatStr();
                    lbTotalEquity.Text = arg1.StkLiquidation.ToFormatStr();
                }
            }
            //如果是最后一条查询则重置查询ID和按钮可用
            if (arg4)
            {
                _qryid = 0;
            }
            
        }


    }
}
