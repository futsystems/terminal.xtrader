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
                System.Threading.ThreadPool.QueueUserWorkItem(o => MDService.DataAPI.Disconnect());
            }
            else
            {
                List<string> serverList = new List<string>();
                int port = 0;
                foreach (var v in (new ServerConfig("market.cfg")).GetServerNodes())
                {
                    if (port == 0) port = v.Port;
                    serverList.Add(v.Address);
                }
                System.Threading.ThreadPool.QueueUserWorkItem(o => MDService.DataAPI.Connect(serverList.ToArray(), port));
               
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
            foreach (var hightlight in MDService.DataAPI.HightLightSymbols)
            {
                ctrlSymbolHighLight.AddSymbol(hightlight);
            }
        }

        void InitUserSetting()
        {
            if (Global.IsXGJStyle)
            {
                topHeader.Text = "信管家";
            }
            else
            {
                topHeader.Text = "交易大师-机构版";
            }
        }

        void UpdateServerInfo()
        {
            if (MDService.DataAPI.Connected)
            {
                if (MDService.DataAPI.CurrentServer != null)
                {
                    //MessageBox.Show(MDService.DataAPI.CurrentServer.Address.ToString());
                    string address = MDService.DataAPI.CurrentServer.Address.ToString();
                    ServerNode srv = srvList.Where(node => node.Address == address).FirstOrDefault();
                    if (srv != null)
                    {
                        lbCurrentServer.Text = srv.Title;

                    }
                }
            }
            else
            {
                lbCurrentServer.Text = "断开";
            }
        }
    }
}
