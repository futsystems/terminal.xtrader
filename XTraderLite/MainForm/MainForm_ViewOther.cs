﻿using System;
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

        void InitOtherView()
        {
            ctrlTickList.ExitView += new EventHandler(ctrlTickList_ExitView);
        }

        void ctrlTickList_ExitView(object sender, EventArgs e)
        {
            ViewQuoteList();
            
        }
    }
}
