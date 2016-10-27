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

            InitControl();

            WireEvent();
            
        }

        void WireEvent()
        {
            //btnHide.Click += new EventHandler(btnHide_Click);
        }
        ctrlListBox priceBox;
        void InitControl()
        {
            ListBox f = new ListBox();
            
            
            priceBox = new ctrlListBox();
            priceBox.ItemSelected += new Action<string>(box_ItemSelected);
            priceBox.Height = 80;
            priceBox.Width = 0;//默认使用DropDownContrl尺寸,设置Width为0后 则使用触发控件的Width

            priceBox.Items.Add("对手价");
            priceBox.Items.Add("对手价超一");
            priceBox.Items.Add("对手价超二");
            priceBox.Items.Add("挂单价");
            priceBox.Items.Add("最新价");
            priceBox.Items.Add("市价");
            priceBox.Items.Add("涨停价");
            priceBox.Items.Add("跌停价");
           

            inputPrice.DropDownSizeMode = SizeMode.UseControlSize;
            inputPrice.DropDownControl = priceBox;

            TextBox tb = new TextBox();
            inputSize.DropDownControl = tb;
        }

        void box_ItemSelected(string obj)
        {
            inputPrice.SetTxtVal(obj);
            inputPrice.HideDropDown();
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
