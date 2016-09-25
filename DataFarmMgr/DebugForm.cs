using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.DataFarmManager
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(DebugForm_FormClosing);
        }

        void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public void Debug(string msg)
        {
            debugControl1.GotDebug(msg);
        }
    }
}
