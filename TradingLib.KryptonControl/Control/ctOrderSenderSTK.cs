using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.NevronControl
{
    public partial class ctOrderSenderSTK : UserControl
    {
        Font TITLEFONT = new Font("宋体", 14, FontStyle.Bold);
        public ctOrderSenderSTK()
        {
            InitializeComponent();
            label1.Font = TITLEFONT;
            this.Side = true;

            //设置空间为背景透明
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor|ControlStyles.Opaque,true);
            //this.BackColor = Color.Transparent;  
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle = 0x20;
        //        return cp;
        //    }
        //}  


        [DefaultValue(true)]
        bool _side = true;
        public bool Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
                label1.Text = _side ? "买入股票" : "卖出股票";
                label6.Text = _side ? "可买(股):" : "可卖(股):";
                label8.Text = _side ? "买入数量:" : "买出数量:";
                //btnSubmit.Text = _side ? "买 入" : "卖 出";

                label1.ForeColor = LabelColor;
                label2.ForeColor = LabelColor;
                label3.ForeColor = LabelColor;
                //label4.ForeColor = LabelColor;
                label5.ForeColor = LabelColor;
                label6.ForeColor = LabelColor;
                //label7.ForeColor = LabelColor;
                label8.ForeColor = LabelColor;

                Invalidate();
            }
        }

        Color LabelColor { get { return _side ? UIConstant.LongLabelColor : UIConstant.ShortLabelColor; } }
    }
}
