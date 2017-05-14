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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            lbVersion.Text = "2.0.2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
