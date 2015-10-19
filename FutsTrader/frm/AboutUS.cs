using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace FutsTrader
{
    public partial class AboutUS : Telerik.WinControls.UI.RadForm
    {
        public AboutUS()
        {
            InitializeComponent();

            this.btnExit.Click += new EventHandler(btnExit_Click);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
