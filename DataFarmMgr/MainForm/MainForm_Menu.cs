using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {

        void InitMenu()
        {

            menuSecurity.Click += new EventHandler(menuSecurity_Click);
            menuSymbol.Click += new EventHandler(menuSymbol_Click);
            menuMarketTime.Click += new EventHandler(menuMarketTime_Click);
            menuExchange.Click += new EventHandler(menuExchange_Click);

            menuBarData.Click += new EventHandler(menuBarData_Click);
            menuRestoreTask.Click += new EventHandler(menuRestoreTask_Click);
        }

        void menuRestoreTask_Click(object sender, EventArgs e)
        {
            fmRestoreTask fm = new fmRestoreTask();
            fm.ShowDialog();
            fm.Close();
        }

        void menuBarData_Click(object sender, EventArgs e)
        {
            fmBarData fm = new fmBarData();
            fm.ShowDialog();
            fm.Close();
        }

        void menuMarketTime_Click(object sender, EventArgs e)
        {
            fmMarketTimeList fm = new fmMarketTimeList();
            fm.ShowDialog();
            fm.Close();
        }

        void menuExchange_Click(object sender, EventArgs e)
        {
            fmExchangeList fm = new fmExchangeList();
            fm.ShowDialog();
            fm.Close();
        }

        void menuSecurity_Click(object sender, EventArgs e)
        {
            fmSecurityList fm = new fmSecurityList();
            fm.ShowDialog();
            fm.Close();
        }

        void menuSymbol_Click(object sender, EventArgs e)
        {
            fmSymbolList fm = new fmSymbolList();
            fm.ShowDialog();
            fm.Close();
        }
    }
}
