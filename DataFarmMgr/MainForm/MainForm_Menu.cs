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
            menuSymbol.Click += new EventHandler(menuSymbol_Click);
        }

        void menuSymbol_Click(object sender, EventArgs e)
        {
            fmSymbolList fm = new fmSymbolList();
            fm.ShowDialog();
            fm.Close();
        }
    }
}
