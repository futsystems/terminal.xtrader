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


namespace TradingLib.XTrader.Future
{
    public partial class PageBank : UserControl,IPage,IEventBinder
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBank()
        {
            InitializeComponent();

            cbCurrency.Items.Add("RMB");
            cbCurrency.SelectedIndex = 0;
            cbCurrency.VisibleChanged += new EventHandler(cbCurrency_VisibleChanged);

            btnSetBankInfo.Click += new EventHandler(btnSetBankInfo_Click);
            btnDeposit.Click += new EventHandler(btnDeposit_Click);
            btnWithdraw.Click += new EventHandler(btnWithdraw_Click);
            CoreService.EventCore.RegIEventHandler(this);
        }

        

        public void OnInit()
        {
            CoreService.EventCore.RegisterCallback("APIService", "Deposit", OnDeposit);
            CoreService.EventCore.RegisterCallback("APIService", "Withdraw", OnWithdraw);
        }

        public void OnDisposed()
        { 
            
        }

        void OnWithdraw(RspInfo info, string json, bool islast)
        {
            if (islast)
            {
                btnWithdraw.Enabled = true;
            }
            if (info.ErrorID != 0)
            {
                MessageBox.Show(info.ErrorMessage);
                return;
            }
            else
            {
                MessageBox.Show("出金请求已提交,等待工作人员审核");
            }
            
        }

        void OnDeposit(RspInfo info, string json, bool islast)
        {
            if (islast)
            {
                btnDeposit.Enabled = true;
            }

            if (info.ErrorID != 0)
            {
                MessageBox.Show(info.ErrorMessage);
                return;
            }

            var url = json.DeserializeObject<string>();
            if (!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start("iexplore.exe",url);
            }
            
        }

        void btnDeposit_Click(object sender, EventArgs e)
        {
            if (amount.Value <= 0)
            {
                MessageBox.Show("金额需大于零");
                return;
            }
            CoreService.TLClient.ReqDeposit(amount.Value);
            btnDeposit.Enabled = false;
        }

        void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (amount.Value <= 0)
            {
                MessageBox.Show("金额需大于零");
                return;
            }
            CoreService.TLClient.ReqWithdraw(amount.Value);
            btnWithdraw.Enabled = false;
        }

        void btnSetBankInfo_Click(object sender, EventArgs e)
        {
            fmBankInfo fm = new fmBankInfo();
            fm.ShowDialog();
            fm.Close();
        }

        void cbCurrency_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                account.Text = CoreService.TradingInfoTracker.Account.Account;
            }
        }
    }
}
