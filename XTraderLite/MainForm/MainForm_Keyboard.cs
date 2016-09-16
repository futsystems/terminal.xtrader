using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    public partial class MainForm
    {
        public override bool PreProcessMessage(ref Message msg)
        {
            if ((Keys)msg.WParam.ToInt32() == Keys.F12)
            {
                SwitchTradingBox();
            }
            return base.PreProcessMessage(ref msg);
        }
    }
}
