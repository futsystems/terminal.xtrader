using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Stock
{
    public partial class fmConfirm : System.Windows.Forms.Form
    {
        public fmConfirm(string title, string message)
        {
            InitializeComponent();

            this.Text = string.IsNullOrEmpty(title) ? "提示" :title;
            this._label.Text = string.IsNullOrEmpty(message) ? "内容" :message;
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }



        public static DialogResult Show(PromptMessage message)
        {
            return Show(message.Title, message.Message);
        }

        public static DialogResult Show(string title, string message)
        {
            DialogResult result;
            using (fmConfirm fm = new fmConfirm(title, message))
            {

                fm.StartPosition = FormStartPosition.CenterParent;
                result = fm.ShowDialog();
            }
            return result;
        }
    }
}
