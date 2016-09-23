using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;


namespace TradingLib.KryptonControl
{
    public partial class MainContainer : UserControl,ITraderAPI
    {
        public MainContainer()
        {
            InitializeComponent();
            ctrlTraderLogin.EntryTrader += new Action<ctrlStockTrader>(ctrlTraderLogin_EntryTrader);
        }

        void ctrlTraderLogin_EntryTrader(ctrlStockTrader obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ctrlStockTrader>(ctrlTraderLogin_EntryTrader), new object[] { obj});
            }
            else
            {
                ctrlTraderLogin.Visible = false;
                this.Controls.Add(obj);
                obj.Dock = DockStyle.Fill;
                obj.Show();
            }
        }


    }
}
