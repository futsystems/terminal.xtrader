using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.MDClient;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {

        void InitToolBar()
        {
            btnConnect.Click += new EventHandler(btnConnect_Click);

            btnDebug1.Click += new EventHandler(btnDebug1_Click);

            btnDebugForm.Click += new EventHandler(btnDebugForm_Click);
        }

        void btnDebug1_Click(object sender, EventArgs e)
        {
            mdClient.DemoRequest("LoadTickFile");
        }

        void btnDebugForm_Click(object sender, EventArgs e)
        {
            debugForm.Show();
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            mdClient = new TradingLib.MDClient.MDClient("127.0.0.1", 5060, 5060);
            mdClient.OnInitializedEvent += new Action(mdClient_OnInitializedEvent);


            mdClient.Start();
        }


    }
}
