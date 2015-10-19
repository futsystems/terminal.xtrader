using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace TradingLib.TraderControl
{
    public partial class fmChangePassword : Telerik.WinControls.UI.RadForm
    {
        public fmChangePassword()
        {
            InitializeComponent();
            btnConfirm.Click += new EventHandler(btnConfirm_Click);
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(oldpass.Text))
            {
                TraderHelper.WindowMessage("请输入旧密码");
                return;
            }
            if (string.IsNullOrEmpty(pass1.Text) || string.IsNullOrEmpty(pass2.Text))
            {
                TraderHelper.WindowMessage("请输入新密码");
                return;
            }

            if (pass1.Text != pass2.Text)
            {
                TraderHelper.WindowMessage("两次密码输入不一致");
                return;
            }
            TraderCore.CoreService.TLClient.ReqChangePassowrd(oldpass.Text, pass1.Text);
        }
    }
}
