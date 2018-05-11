using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class frmDataFarmList : Form
    {
        public frmDataFarmList()
        {
            InitializeComponent();
            this.Load += new EventHandler(fmDataFarmList_Load);
        }

        void fmDataFarmList_Load(object sender, EventArgs e)
        {
            if (MDService.DataAPI.Connected)
            {
                if (MDService.DataAPI.CurrentServer != null)
                {
                    if (MDService.DataAPI.CurrentServer.EndsWith("tickcloud.net"))
                    {
                        lbCurrentServer.Text = MDService.DataAPI.CurrentServer.Split('.')[0];
                    }
                    else
                    {
                        lbCurrentServer.Text = MDService.DataAPI.CurrentServer;
                    }
                }
            }
            else
            {
                lbCurrentServer.Text = "断开";
            }
        }
    }
}
