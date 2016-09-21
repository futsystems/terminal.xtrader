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
using TradingLib.XTrader.Control;

namespace XTraderLite
{
    public partial class MainForm
    {


        void UpdateConnImg(bool conn)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action<bool>(UpdateConnImg), new object[] { conn });
            }
            else
            {
                imgConn.Image = conn ? Properties.Resources.connected : Properties.Resources.disconnected;
            }
        }
        void UpdateTime()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(UpdateTime), new object[] { });
            }
            else
            {
                lbTime.Text = string.Format("{0:T}", DateTime.Now);
            }
        }

        void InitHightLight()
        {
            MDSymbol sh = MDService.DataAPI.GetSymbol(Exchange.EXCH_SSE, "999999");
            MDSymbol sz = MDService.DataAPI.GetSymbol(Exchange.EXCH_SZE, "399001");

            if (sh != null) ctrlSymbolHighLight.AddSymbol(new SymbolHighLight("沪", sh));
            if (sz != null) ctrlSymbolHighLight.AddSymbol(new SymbolHighLight("深", sz));


        }
    }
}
