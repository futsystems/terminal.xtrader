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
        void btnQuoteView_Click(object sender, EventArgs e)
        {
            SetViewType(EnumTraderViewType.QuoteList);

        }

        void btnBarView_Click(object sender, EventArgs e)
        {
            SetViewType(EnumTraderViewType.KChart);
            if (kChartView.IsIntraView)
            {
                kChartView.ViewType = CStock.KChartViewType.KView;
            }
        }

        void btnIntraView_Click(object sender, EventArgs e)
        {
            SetViewType(EnumTraderViewType.KChart);
            if (kChartView.IsBarView)
            {
                kChartView.ViewType = CStock.KChartViewType.TimeView;
            }
        }

    }
}
