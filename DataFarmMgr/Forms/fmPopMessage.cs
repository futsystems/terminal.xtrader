using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.DataFarmManager
{
    public partial class fmPopMessage : Form
    {
        Timer _timer = new Timer();

        public fmPopMessage()
        {
            InitializeComponent();
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = 200;
            this.FormClosing += new FormClosingEventHandler(fmPopMessage_FormClosing);
        }

        void fmPopMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        void ResetMessage()
        {
            lbTitle.Text = "标题";
            lbMessage.Text = "内容";
            //picbox.Image = null;
        }

        int num = 0;
        int totalnum = 20;
        void _timer_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            //到达计数隐藏窗口
            if (num == totalnum)
            {
                _timer.Stop();
                this.Hide();
                ResetMessage();
            }
            num++;
        }

        public void PopMessage(RspInfo info = null)
        {
            if (info != null)
            {
                lbTitle.Text = info.ErrorID == 0 ? "操作成功" : "操作失败(" + info.ErrorID.ToString() + ")";
                lbMessage.Text = info.ErrorMessage;
                picbox.Image = info.ErrorID == 0 ? Properties.Resources.success_24 : Properties.Resources.error_24;
            }
            //初始化状态
            this.Opacity = 1;
            num = 0;
            //显示窗口
            this.Show();
            //定时开始
            _timer.Start();
        }
    }
}
