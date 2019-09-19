﻿using System;
using System.Collections;
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
    public partial class PageBankAPI : UserControl,IPage,IEventBinder
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBankAPI()
        {
            InitializeComponent();

            cbCurrency.Items.Add("RMB");
            cbCurrency.SelectedIndex = 0;
            cbCurrency.VisibleChanged += new EventHandler(cbCurrency_VisibleChanged);

            cbBank.DataSource = this.GetBankSelection();
            cbBank.ValueMember = "Value";
            cbBank.DisplayMember = "Name";
            

            btnSetBankInfo.Click += new EventHandler(btnSetBankInfo_Click);
            btnDeposit.Click += new EventHandler(btnDeposit_Click);
            btnWithdraw.Click += new EventHandler(btnWithdraw_Click);
            CoreService.EventCore.RegIEventHandler(this);
        }


        public ArrayList GetBankSelection()
        {
            ArrayList list = new ArrayList();
            list.Add(new ValueObject<string>() { Value = "01020000", Name = "工商银行" });
            list.Add(new ValueObject<string>() { Value = "01030000", Name = "农业银行" });
            list.Add(new ValueObject<string>() { Value = "01050000", Name = "建设银行" });
            list.Add(new ValueObject<string>() { Value = "01040000", Name = "中国银行" });
            list.Add(new ValueObject<string>() { Value = "03080000", Name = "招商银行" });
            list.Add(new ValueObject<string>() { Value = "03010000", Name = "交通银行" });
            list.Add(new ValueObject<string>() { Value = "01000000", Name = "中国邮政" });
            list.Add(new ValueObject<string>() { Value = "03020000", Name = "中信银行" });
            list.Add(new ValueObject<string>() { Value = "03030000", Name = "光大银行" });
            list.Add(new ValueObject<string>() { Value = "03050000", Name = "民生银行" });


            return list;
        }
        

        public void OnInit()
        {
            //CoreService.EventCore.RegisterCallback("APIService", "DepositFZ", OnDeposit);
            CoreService.EventCore.RegisterCallback("APIService", "Withdraw", OnWithdraw);
            btnSetBankInfo.Visible = Constants.EnableConfigBank;
        }

        public void OnDisposed()
        {
            //CoreService.EventCore.UnRegisterCallback("APIService", "DepositFZ", OnDeposit);
            CoreService.EventCore.UnRegisterCallback("APIService", "Withdraw", OnWithdraw);
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
            string msg = string.Empty;
            bool isex = CoreService.TradingInfoTracker.Account.Currency != CurrencyType.RMB;
            if(isex)
            {
                var rate = CoreService.TradingInfoTracker.Account.GetExchangeRate(CurrencyType.RMB);

                msg = string.Format("确认入金人民币:{0}元 ({1}{2})", amount.Value.ToFormatStr(), (rate * amount.Value).ToFormatStr(), Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency));
            }
            else
            {
                msg = string.Format("确认入金人民币:{0}元",amount.Value);
            }
            if (MessageBox.Show(msg,"确认入金", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                //CoreService.TLClient.ReqDepositFZ(amount.Value,(string)cbBank.SelectedValue);
                //btnDeposit.Enabled = false;
                string url = string.Format("{0}?account={1}&amount={2}&bank={3}", Constants.CashURL1, CoreService.TLClient.UserName, amount.Value, (string)cbBank.SelectedValue);
                //MessageBox.Show(url);
                System.Diagnostics.Process.Start(url);
            }
        }

        void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (amount.Value <= 0)
            {
                MessageBox.Show("金额需大于零");
                return;
            }
            
            string msg = string.Empty;
            bool isex = CoreService.TradingInfoTracker.Account.Currency != CurrencyType.RMB;
            if(isex)
            {
                var rate = CoreService.TradingInfoTracker.Account.GetExchangeRate(CurrencyType.RMB);

                msg = string.Format("确认出金人民币:{0}元 ({1}{2})", amount.Value.ToFormatStr(), (rate * amount.Value).ToFormatStr(), Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency));
            }
            else
            {
                msg = string.Format("确认出金人民币:{0}元",amount.Value);
            }
            if (MessageBox.Show(msg, "确认出金", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                CoreService.TLClient.ReqWithdraw(amount.Value);
                btnWithdraw.Enabled = false;
            }
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