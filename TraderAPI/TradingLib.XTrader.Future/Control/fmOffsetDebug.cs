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
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public partial class fmOffsetDebug : Form
    {
        public fmOffsetDebug()
        {
            InitializeComponent();


            foreach (var arg in CoreService.PositionWatcher.ArgMap)
            {
                posOffsetArgList.Items.Add(string.Format("{0}-{1}", arg.Key, arg.Value));
            }
        }
    }
}
