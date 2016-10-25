using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.XTrader.Future.Control
{
    public partial class ctrlOrderEntry : UserControl
    {
        //bool _expand = true;
        public ctrlOrderEntry()
        {
            InitializeComponent();
            WireEvent();
            
        }

        void WireEvent()
        {
            //btnHide.Click += new EventHandler(btnHide_Click);
        }


        //void btnHide_Click(object sender, EventArgs e)
        //{
        //    //_expand = !_expand;
        //    //if (_expand)
        //    //{
        //    //    this.Width = 343;
        //    //    btnHide.Text = "<";
        //    //    tabControl1.Visible = true;
        //    //}
        //    //else
        //    //{
        //    //    this.Width = 10;
        //    //    btnHide.Text = ">";
        //    //    tabControl1.Visible = false;
        //    //}
            
        //}
    }
}
