using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class AddString : Form
    {
        public AddString()
        {
            InitializeComponent();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            cdg.Color = Color1.BackColor;
            if (cdg.ShowDialog() == DialogResult.OK)
            {
                Color1.BackColor = cdg.Color;
            }
        }

        private void AddString_Load(object sender, EventArgs e)
        {

        }
    }
}