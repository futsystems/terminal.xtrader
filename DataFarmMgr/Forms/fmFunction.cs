﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;


namespace TradingLib.DataFarmManager
{
    public partial class fmFunction : Form
    {
        public fmFunction()
        {
            InitializeComponent();

            btnResetAllSnapshot.Click += new EventHandler(btnResetAllSnapshot_Click);
        }

        void btnResetAllSnapshot_Click(object sender, EventArgs e)
        {
            DataCoreService.DataClient.ReqContribRequest("DataFarm", "ResetSnapshot", "");
        }
    }
}
