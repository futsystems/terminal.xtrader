using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class CheckPass : Form
    {
        public CheckPass()
        {
            InitializeComponent();
        }

        private void CheckPass_Shown(object sender, EventArgs e)
        {
            password.Focus();
        }
    }
}