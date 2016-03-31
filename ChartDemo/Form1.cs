using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MDClient;

namespace ChartDemo
{
    public partial class Form1 : Form
    {
        MDClient client = null;
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
            
        }

        void Form1_Load(object sender, EventArgs e)
        {
            client = new MDClient("114.55.72.206", 5060, 5060);
            client.Start();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {


            ctlChartEasyChart1.Display(client as IDataClient);
        }
    }
}
