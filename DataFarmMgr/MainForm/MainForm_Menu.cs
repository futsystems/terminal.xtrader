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

        void InitMenu()
        {

            menuSecurity.Click += new EventHandler(menuSecurity_Click);
            menuSymbol.Click += new EventHandler(menuSymbol_Click);

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
