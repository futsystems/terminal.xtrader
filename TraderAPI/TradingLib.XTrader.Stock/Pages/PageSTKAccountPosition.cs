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

            CoreService.EventQry.OnRspXQryAccountFinanceEvent += new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent);

            btnQry.Click += new EventHandler(btnQry_Click);
        }

        void btnQry_Click(object sender, EventArgs e)
        {
            QryAccountFinance();
        }

        public void QryAccountFinance()
        {
            _qryid = CoreService.TLClient.ReqXQryAccountFinance();
        }

        int _qryid = 0;
        void EventQry_OnRspXQryAccountFinanceEvent(RspXQryAccountFinanceResponse response)
        {
            if (response.RequestID != _qryid) return;//查询RequestID不一致表面非当前控件查询 直接返回
            //返回委托对象不为空则调用OrderView进行输出显示
            if (response.Report != null)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent), new object[] {response});
                }
                else
                {
                    lbCash.Text = response.Report.NowEquity.ToFormatStr();//当前资金余额
                    lbMoneyFrozen.Text = response.Report.StkMoneyFronzen.ToFormatStr();//股票资金冻结
                    lbSTKMarketValue.Text = response.Report.StkPositionValue.ToFormatStr();//股票市值
                    lbAvabileFund.Text = response.Report.StkAvabileFunds.ToFormatStr();//可用资金

                    lbTotalEquity.Text = (response.Report.NowEquity + response.Report.StkPositionValue).ToFormatStr();//总资产

                    lbStkPositionCost.Text = response.Report.StkPositionCost.ToFormatStr();//股票成本
                    lbStkRealizedPL.Text = response.Report.StkRealizedPL.ToFormatStr();//股票平仓盈亏

                    lbStkCommission.Text = response.Report.StkCommission.ToFormatStr();//股票手续费

                    lbBuyAmount.Text = response.Report.StkBuyAmount.ToFormatStr();//股票买入金额
                    lbSellAmount.Text = response.Report.StkSellAmount.ToFormatStr();//股票卖出金额

                    lbLastEquity.Text = response.Report.LastEquity.ToFormatStr();//账户昨日权益
                    lbCredit.Text = response.Report.Credit.ToFormatStr();//账户昨日信用

                    //当前资金 = 昨日资金 - 今日买入金额 + 今日卖出金额 - 手续费
                    //lbNowEquityC.Text = (arg1.LastEquity - arg1.StkBuyAmount + arg1.StkSellAmount - arg1.Commission).ToFormatStr();
                    //可用资金 = 当前资金 - 冻结资金
                }
            }
            //如果是最后一条查询则重置查询ID和按钮可用
            if (response.IsLast)
            {
                _qryid = 0;
            }
            
        }


    }
}
