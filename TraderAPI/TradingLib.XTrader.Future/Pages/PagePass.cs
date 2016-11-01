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
    public partial class PagePass : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_PASS;
        public string PageName { get { return _pageName; } }

        public PagePass()
        {
            InitializeComponent();
            btnChange.Click += new EventHandler(btnChange_Click);
            cbPassType.SelectedIndex = 0;
        }

        void btnChange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pass.Text))
            {
                MessageBox.Show("请输入旧密码", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(newpass1.Text) || string.IsNullOrEmpty(newpass2.Text))
            {
                MessageBox.Show("请输入新密码", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newpass1.Text != newpass2.Text)
            {
                MessageBox.Show("输入的新密码不一致，请确认新密码", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CoreService.TLClient.ReqChangePassowrd(pass.Text, newpass1.Text);
        }
    }
}
