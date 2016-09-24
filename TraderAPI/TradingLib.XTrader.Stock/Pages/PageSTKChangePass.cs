﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Stock
{
    public partial class PageSTKChangePass : UserControl,IPage
    {

        string _pageName = PageTypes.PAGE_CHANGE_PASS;
        public string PageName { get { return _pageName; } }


        public PageSTKChangePass()
        {
            InitializeComponent();

            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(newpass1.Text))
            {
                MessageBox.Show("请输入旧密码","修改密码",MessageBoxButtons.OK,MessageBoxIcon.Warning);
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

            CoreService.TLClient.ReqChangePassowrd(newpass1.Text, newpass1.Text);

        }
    }
}
