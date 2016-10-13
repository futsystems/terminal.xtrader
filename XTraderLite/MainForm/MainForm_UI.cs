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


        void WireUI()
        {
            imgConn.DoubleClick += new EventHandler(imgConn_DoubleClick);
            btnTrade.Click += new EventHandler(btnTrade_Click);
            btnBBS.Click += new EventHandler(btnBBS_Click);
            ctrlSymbolHighLight.SymbolDoubleClick += new Action<MDSymbol>(ctrlSymbolHighLight_SymbolDoubleClick);
        }

        //底部跑马灯双击 显示合约分时
        void ctrlSymbolHighLight_SymbolDoubleClick(MDSymbol symbol)
        {
            if (symbol != null)
            {
                ctrlKChart.KChartViewType = CStock.KChartViewType.TimeView;
                ViewKChart(symbol);
            }
        }

        void btnBBS_Click(object sender, EventArgs e)
        {
            MessageBox.Show("欢迎访问");
        }

        void btnTrade_Click(object sender, EventArgs e)
        {
            SwitchTradingBox();
        }

        void imgConn_DoubleClick(object sender, EventArgs e)
        {
            if (MDService.DataAPI.Connected)
            {
                new System.Threading.Thread(delegate()
                {
                    MDService.DataAPI.Disconnect();
                }).Start();
            }
            else
            {
                new System.Threading.Thread(delegate()
                {
                    MDService.DataAPI.Connect(new string[] { "218.85.137.40" }, 7709);
                }).Start();
            }
        }

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
            MDSymbol sh = MDService.DataAPI.GetSymbol(ConstsExchange.EXCH_SSE, "999999");
            MDSymbol sz = MDService.DataAPI.GetSymbol(ConstsExchange.EXCH_SZE, "399001");

            if (sh != null) ctrlSymbolHighLight.AddSymbol(new SymbolHighLight("沪", sh));
            if (sz != null) ctrlSymbolHighLight.AddSymbol(new SymbolHighLight("深", sz));


        }
    }
}
