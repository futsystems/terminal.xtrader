using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class AddFenLei : Form
    {
        public AddFenLei()
        {
            InitializeComponent();
        }

        private void AddFenLei_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (leiname.Text.Length == 0)
                {
                    e.Cancel = true;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (leiname.Text.Length == 0)
            {
                MessageBox.Show("请输入分类名称!");
                leiname.Focus();
                return;
            }

        }
    }
}