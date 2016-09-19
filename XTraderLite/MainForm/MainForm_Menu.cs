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
        void SwitchTradingBox()
        {
            panelBroker.Visible ^= true;
        }


        bool  SwitchMainView(bool backQuote)
        {
            //当前为报价表状态 则进入分时
            if (ctrlQuoteList.Visible)
            {
                ViewIntraChart();
                return true;
            }

            if (ctrlKChart.Visible)
            {
                if (ctrlKChart.IsIntraView)
                {
                    ViewBarChart();
                    return true;
                }
                if (ctrlKChart.IsBarView)
                {
                    if (backQuote)
                    {
                        ViewQuoteList();
                        return false;
                    }
                    else
                    {
                        ViewIntraChart();
                        return true;
                    }
                  
                }
            }
            return false;
        }


        
        void menuTrading_Click(object sender, EventArgs e)
        {
            SwitchTradingBox();
        }

        void menuSwitchKchart_Click(object sender, EventArgs e)
        {
            SwitchMainView(false);
        }
    }
}
