using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public void Debug(string msg)
        {
            debugControl1.GotDebug(msg);
        }
    }
}
