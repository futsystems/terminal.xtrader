using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.TraderCore;

namespace TradingLib.KryptonControl
{
    public partial class fmMessage : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public fmMessage(string title,string message)
        {
            InitializeComponent();

            this.Text = string.IsNullOrEmpty(title) ? "提示" :title;
            this._label.Text = string.IsNullOrEmpty(message) ? "内容" :message;
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);

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
            using (fmMessage fm = new fmMessage(title,message))
            {

                fm.StartPosition = FormStartPosition.CenterParent;
                result = fm.ShowDialog();
            }
            return result;
        }
    }
}
