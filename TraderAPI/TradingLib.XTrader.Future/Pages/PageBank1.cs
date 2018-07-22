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
    public partial class PageBank1 : UserControl,IPage,IEventBinder
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBank1()
        {
            InitializeComponent();

            cbCurrency.Items.Add("RMB");
            cbCurrency.SelectedIndex = 0;

            cbCurrency2.Items.Add(CoreService.TradingInfoTracker.Account.Currency.ToString());
            if (CoreService.TradingInfoTracker.Account.Currency != CurrencyType.RMB)
                cbCurrency2.Items.Add("RMB");
            cbCurrency2.SelectedIndex = 0;

            cbCurrency2.SelectedIndexChanged += new EventHandler(cbCurrency2_SelectedIndexChanged);
            
            cbCurrency.VisibleChanged += new EventHandler(cbCurrency_VisibleChanged);
            cbCurrency2.VisibleChanged += new EventHandler(cbCurrency_VisibleChanged);

            //depositNormal.Checked = true;
            depositLeverageDeposit.Checked = true;
            withdrawNormal.Checked = true;

            btnSetBankInfo.Click += new EventHandler(btnSetBankInfo_Click);
            btnDeposit.Click += new EventHandler(btnDeposit_Click);
            btnWithdraw.Click += new EventHandler(btnWithdraw_Click);
            this.Load += new EventHandler(PageBank1_Load);
            CoreService.EventCore.RegIEventHandler(this);
        }

        void PageBank1_Load(object sender, EventArgs e)
        {
            cbCurrency2_SelectedIndexChanged(null, null);
        }

       

        

        public void OnInit()
        {
            CoreService.EventCore.RegisterCallback("APIService", "Deposit2", OnDeposit);
            CoreService.EventCore.RegisterCallback("APIService", "Withdraw2", OnWithdraw);
            btnSetBankInfo.Visible = Constants.EnableConfigBank;
        }

        public void OnDisposed()
        {
            CoreService.EventCore.UnRegisterCallback("APIService", "Deposit2", OnDeposit);
            CoreService.EventCore.UnRegisterCallback("APIService", "Withdraw2", OnWithdraw);
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
            if (amountDeposit.Value <= 0)
            {
                MessageBox.Show("金额需大于零");
                return;
            }
            string msg = string.Empty;
            bool isex = CoreService.TradingInfoTracker.Account.Currency != CurrencyType.RMB;
            EnumBusinessType type = EnumBusinessType.Normal;
            if (depositLeverageDeposit.Checked) type = EnumBusinessType.LeverageDeposit;
            if (depositNormal.Checked) type = EnumBusinessType.Normal;

            if(isex)
            {
                var rate = CoreService.TradingInfoTracker.Account.GetExchangeRate(CurrencyType.RMB);

                msg = string.Format("确认入金人民币:{0}元 ({1}{2}) 类别:{3}", amountDeposit.Value.ToFormatStr(), (rate * amountDeposit.Value).ToFormatStr(), Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency), Util.GetEnumDescription(type));
            }
            else
            {
                msg = string.Format("确认入金人民币:{0}元 类别:{1}", amountDeposit.Value,Util.GetEnumDescription(type));
            }
            if (MessageBox.Show(msg,"确认入金", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                CoreService.TLClient.ReqDeposit2(amountDeposit.Value,type);
                btnDeposit.Enabled = false;
            }
        }

        /// <summary>
        /// 当前选定出金
        /// </summary>
        CurrencyType SelectedWithdrawCurrency
        {
            get
            {
                return cbCurrency2.SelectedItem.ToString().ParseEnum<CurrencyType>();
            }
        }

        void cbCurrency2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CoreService.Initialized)
            {
                //选定币种与账户币种一致 
                if (SelectedWithdrawCurrency == CoreService.TradingInfoTracker.Account.Currency)
                {
                    withdrawAvabile.Text = CoreService.TradingInfoTracker.Account.NowEquity.ToFormatStr();
                }
                else
                {
                    //执行汇率换算
                    var rate = CoreService.TradingInfoTracker.Account.GetExchangeRate(SelectedWithdrawCurrency);
                    withdrawAvabile.Text = (CoreService.TradingInfoTracker.Account.NowEquity / rate).ToFormatStr();
                }
            }
        }

        /// <summary>
        /// 出金逻辑为货币币种或账户币种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (amountWithdraw.Value <= 0)
            {
                MessageBox.Show("金额需大于零");
                return;
            }
            
            string msg = string.Empty;
            bool isex = SelectedWithdrawCurrency  != CurrencyType.RMB;//选择出金货币不为人名币
            EnumBusinessType type = EnumBusinessType.Normal;
            if (withdrawNormal.Checked) type = EnumBusinessType.Normal;
            if (withdrawCreditWithdraw.Checked) type = EnumBusinessType.CreditWithdraw;

            decimal rate = 1;
            if(isex)
            {
                //将出金货币转换成RMB
                rate = Util_Account.GetExchangeRate(CurrencyType.RMB,SelectedWithdrawCurrency);


                msg = string.Format("确认出金人民币:{0}元 ({1}{2}) 类别:{3}", (amountWithdraw.Value * rate).ToFormatStr(), (amountWithdraw.Value).ToFormatStr(), Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency), Util.GetEnumDescription(type));
            }
            else
            {
                msg = string.Format("确认出金人民币:{0}元 类别:{1}", amountWithdraw.Value, Util.GetEnumDescription(type));
            }
            if (MessageBox.Show(msg, "确认出金", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                CoreService.TLClient.ReqWithdraw2(amountWithdraw.Value * rate, type);
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
