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

            string brand = string.IsNullOrEmpty(Global.BrandName) ? "交易大师" : Global.BrandName;
            brandName.Text = brand;
            productName.Text = string.Format("{0}投资分析系统",brand);
            brandCompany.Text = string.IsNullOrEmpty(Global.BrandCompany) ? "上海大师网络数据信息咨询有限公司" : Global.BrandCompany;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
