using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.KryptonControl
{
    public partial class Canvas : System.Windows.Forms.Control
    {
        public Canvas()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
    }
}
