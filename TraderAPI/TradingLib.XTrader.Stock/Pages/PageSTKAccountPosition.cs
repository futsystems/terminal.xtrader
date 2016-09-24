using System;
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


namespace TradingLib.XTrader.Stock
{
    public partial class PageSTKAccountPosition : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ACCOUNT_POSITION;
        public string PageName { get { return _pageName; } }

        ILog logger = LogManager.GetLogger("PageSTKAccountPosition");
        

        public PageSTKAccountPosition()
        {
            InitializeComponent();
            
            CoreService.EventQry.OnRspQryAccountInfoResponse += new Action<AccountInfo, RspInfo, int, bool>(EventQry_OnRspQryAccountInfoResponse);

            btnQry.Click += new EventHandler(btnQry_Click);
        }

        void btnQry_Click(object sender, EventArgs e)
        {
            QryAccountInfo();
        }

        public void QryAccountInfo()
        {
            _qryid = CoreService.TLClient.ReqXQryAccount();
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
                    lbCash.Text = arg1.NowEquity.ToFormatStr();//当前资金余额
                    lbMoneyFrozen.Text = arg1.StkMoneyFronzen.ToFormatStr();//股票资金冻结
                    lbSTKMarketValue.Text = arg1.StkPositionValue.ToFormatStr();//股票市值
                    lbAvabileFund.Text = arg1.StkAvabileFunds.ToFormatStr();//可用资金

                    lbTotalEquity.Text = (arg1.NowEquity + arg1.StkPositionValue).ToFormatStr();//总资产

                    lbStkPositionCost.Text = arg1.StkPositionCost.ToFormatStr();//股票成本
                    lbStkRealizedPL.Text = arg1.StkRealizedPL.ToFormatStr();//股票平仓盈亏

                    lbStkCommission.Text = arg1.StkCommission.ToFormatStr();//股票手续费

                    lbBuyAmount.Text = arg1.StkBuyAmount.ToFormatStr();//股票买入金额
                    lbSellAmount.Text = arg1.StkSellAmount.ToFormatStr();//股票卖出金额

                    lbLastEquity.Text = arg1.LastEquity.ToFormatStr();//账户昨日权益
                    lbCredit.Text = arg1.Credit.ToFormatStr();//账户昨日信用

                    //当前资金 = 昨日资金 - 今日买入金额 + 今日卖出金额 - 手续费
                    //lbNowEquityC.Text = (arg1.LastEquity - arg1.StkBuyAmount + arg1.StkSellAmount - arg1.Commission).ToFormatStr();
                    //可用资金 = 当前资金 - 冻结资金
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
