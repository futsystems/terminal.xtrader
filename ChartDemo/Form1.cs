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

        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    MessageBox.Show("ProcessDialogKey:" + keyData.ToString());
        //    switch (keyData)
        //    {
        //        case Keys.Up:
        //            MessageBox.Show("up");
        //            break;
        //        default:
        //            break;
        //    }


        //    if (keyData == Keys.Up || keyData == Keys.Down ||
        //        keyData == Keys.Left || keyData == Keys.Right)
        //        return true;
        //    else
        //        return base.ProcessDialogKey(keyData);
        //}


        void Form1_Load(object sender, EventArgs e)
        {
            client = new MDClient("127.0.0.1", 5060, 5060);
            client.Start();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            TradingLib.DataProvider.TLMemoryDataManager dmd = new TradingLib.DataProvider.TLMemoryDataManager(client,true);


            ctlChartEasyChart1.BindDataManager(dmd);

            Symbol symbol = client.GetSymbol("rb1610");

            ctlChartEasyChart1.ShowChart(symbol);
        }
    }
}
