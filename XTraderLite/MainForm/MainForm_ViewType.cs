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

        List<Panel> viewList = new List<Panel>();


        void SetViewType(EnumTraderViewType type)
        {
            int index = (int)type;
            if (viewList[index].Visible) return;
            foreach (var v in viewList)
            {
                v.Visible = false;
            }
            viewList[index].Visible = true;
        }
    }
}
