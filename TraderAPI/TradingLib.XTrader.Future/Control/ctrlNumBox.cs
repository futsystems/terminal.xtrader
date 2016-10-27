using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlNumBox : UserControl
    {
        public event Action<int> NumSelected = delegate { };

        public ctrlNumBox()
        {
            InitializeComponent();

            fButton1.Click += new EventHandler(fButton1_Click);
            fButton2.Click += new EventHandler(fButton2_Click);
            fButton3.Click += new EventHandler(fButton3_Click);
            fButton4.Click += new EventHandler(fButton4_Click);
            fButton5.Click += new EventHandler(fButton5_Click);
            fButton6.Click += new EventHandler(fButton6_Click);
            fButton7.Click += new EventHandler(fButton7_Click);
            fButton8.Click += new EventHandler(fButton8_Click);
            fButton9.Click += new EventHandler(fButton9_Click);
        }

        void fButton9_Click(object sender, EventArgs e)
        {
            NumSelected(50);
        }

        void fButton8_Click(object sender, EventArgs e)
        {
            NumSelected(100);
        }

        void fButton7_Click(object sender, EventArgs e)
        {
            NumSelected(200);
        }

        void fButton6_Click(object sender, EventArgs e)
        {
            NumSelected(5);
        }

        void fButton5_Click(object sender, EventArgs e)
        {
            NumSelected(10);
        }

        void fButton4_Click(object sender, EventArgs e)
        {
            NumSelected(20);
        }

        void fButton3_Click(object sender, EventArgs e)
        {
            NumSelected(3);
        }

        void fButton2_Click(object sender, EventArgs e)
        {
            NumSelected(2);
        }

        void fButton1_Click(object sender, EventArgs e)
        {
            NumSelected(1);
        }
    }
}
