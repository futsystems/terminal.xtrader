using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class MainForm
    {
        void btnDemo1_Click(object sender, EventArgs e)
        {
            timeGo ^= true;
        }

        void btnDemo3_Click(object sender, EventArgs e)
        {
            
        }

        void btnDemo2_Click(object sender, EventArgs e)
        {
            _debugForm.Show();
        }


    }
}
