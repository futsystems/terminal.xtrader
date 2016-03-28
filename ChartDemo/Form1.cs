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
using TradingLib.Chart;
using TradingLib.MDClient;

namespace ChartDemo
{
    public partial class Form1 : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        TradingLib.MDClient.MDClient mdclient = null;
        MDHandler handler = null;

        public Form1()
        {
            InitializeComponent();

            ctlChart1.LoadChart();
            ctlChart1.ApplyChartStyle(new ChartStyle());

            handler = new MDHandler();
            handler.BarsRspEvent += new Action<List<BarImpl>, RspInfo, int, bool>(handler_BarsRspEvent);
            mdclient = new TradingLib.MDClient.MDClient("114.55.72.206", 5060, 5060);
            mdclient.RegisterHandler(handler);
            mdclient.Start();
        }

        void handler_BarsRspEvent(List<BarImpl> arg1, RspInfo arg2, int arg3, bool arg4)
        {
            foreach (var b in arg1)
            {
                ctlChart1.UpdateBar(b, true);
            }
            if (arg4)
            {
                //ctlChart1.UpdateView();
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            ctlChart1.ResetStockChartX();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            mdclient.QryBar("IF1604",60, DateTime.MinValue, DateTime.MaxValue, 10000);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ctlChart1.UpdateView();
        }
    }
}
