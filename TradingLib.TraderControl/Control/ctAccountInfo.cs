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

namespace TradingLib.TraderControl
{
    public partial class ctAccountInfo : UserControl,IEventBinder
    {
        public event VoidDelegate QueryAccountInfo;
        public event VoidDelegate QueryRaceInfo;
        public ctAccountInfo()
        {
            InitializeComponent();
            CoreService.EventCore.RegIEventHandler(this);
            WireEvent();
            
        }

        void WireEvent()
        {
            CoreService.EventOther.OnAccountInfoEvent += new Action<AccountInfo>(GotAccountInfo);
        }
        public void OnInit()
        {
            this.GotAccountInfo(CoreService.AccountInfo);
        }

        public void OnDisposed()
        { 
            
        }

        public void GotAccountInfo(AccountInfo info)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<AccountInfo>(GotAccountInfo), new object[] { info });
            }
            else
            {
                if (info == null) return;
                account.Text = info.Account;
                execution.Text = info.Execute ? "允许" : "禁止";
                accountCategory.Text = Util.GetEnumDescription(info.Category);
                interday.Text = info.IntraDay ? "日内" : "隔夜";

                name.Text = info.Name;
                lastequity.Text = Util.FormatDecimal(info.LastEquity);
                nowequity.Text = Util.FormatDecimal(info.NowEquity);

                realizedpl.Text = Util.FormatDecimal(info.RealizedPL);
                unrealizedpl.Text = Util.FormatDecimal(info.UnRealizedPL);

                commission.Text = Util.FormatDecimal(info.Commission);
                profit.Text = Util.FormatDecimal(info.Profit);

                marginUsed.Text = Util.FormatDecimal(info.Margin);
                marginFrozen.Text = Util.FormatDecimal(info.MarginFrozen);


                buypower.Text = Util.FormatDecimal(info.AvabileFunds);
                cashin.Text = Util.FormatDecimal(info.CashIn);
                cashout.Text = Util.FormatDecimal(info.CashOut);

            }

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            CoreService.TLClient.ReqQryAccountInfo();   
        }
    }

    

}
