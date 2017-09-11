using System;
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
    public partial class PageBankWebSite : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBankWebSite()
        {
            InitializeComponent();
            btnDeposit.Click += new EventHandler(btnDeposit_Click);
            btnWithdraw.Click += new EventHandler(btnWithdraw_Click);

            btnDeposit.Visible = false;
            btnWithdraw.Visible = false;

            if (!string.IsNullOrEmpty(Constants.CashURL1) && !string.IsNullOrEmpty(Constants.CashURL2))
            {
                btnDeposit.Visible = true;
                btnWithdraw.Visible = true;
            }
            else
            {

                if (!string.IsNullOrEmpty(Constants.CashURL1))
                {
                    btnDeposit.Visible = true;
                    btnDeposit.Text = "在线出入金";
                }
            }

        }


        void btnDeposit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Constants.CashURL1))
            {
                System.Diagnostics.Process.Start("iexplore.exe", Constants.CashURL1);
            }
        }

        void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Constants.CashURL1))
            {
                System.Diagnostics.Process.Start("iexplore.exe", Constants.CashURL2);
            }
        }

    }
}
