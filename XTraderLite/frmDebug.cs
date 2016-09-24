using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

namespace XTraderLite
{
    public partial class frmDebug : Form
    {
        
        public frmDebug()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(frmDebug_FormClosing);

        }

        void frmDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// DebugControl做了Invoke判断，但是在DebugForm被调用 仍然需要做调用判断
        /// 因为他们是2个对象
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg)
        {
            if (InvokeRequired)
            {

                Invoke(new Action<string>(Debug), new object[] { msg });
            }
            else
            {
                debugControl1.GotDebug(msg);
            }
        }
    }
}
