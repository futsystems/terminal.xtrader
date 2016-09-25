using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.MarketData;
using TradingLib.XTrader.Control;
using TradingLib.MDClient;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {
        ILog logger = LogManager.GetLogger("MainForm");

        TradingLib.MDClient.MDClient mdClient = null;

        DebugForm debugForm = null;
        public MainForm()
        {
            InitializeComponent();
            debugForm = new DebugForm();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(Debug);

            WireEvent();

            InitControl();

            InitQuoteLists();

        }

        void WireEvent()
        {
            btnConnect.Click += new EventHandler(btnConnect_Click);

            btnDebugForm.Click += new EventHandler(btnDebugForm_Click);
        }


        void InitControl()
        {
            ctrlQuoteList.Dock = DockStyle.Fill;

        }
        void btnDebugForm_Click(object sender, EventArgs e)
        {
            debugForm.Show();
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            mdClient = new TradingLib.MDClient.MDClient("127.0.0.1", 5060, 5060);
            mdClient.OnInitializedEvent += new Action(mdClient_OnInitializedEvent);
            

            mdClient.Start();
        }


        
        void mdClient_OnInitializedEvent()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(mdClient_OnInitializedEvent), new object[] { });
            }
            else
            {
                logger.Info("MDClient Inited");

                ctrlQuoteList.SetSymbols(mdClient.MDSymbols);
                ctrlQuoteList.SelectTab(0);

                foreach (var exchange in mdClient.Exchanges)
                {
                    string k = exchange.EXCode;

                    ctrlQuoteList.AddBlock(k, new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                        =>
                    {
                        if (symbol.Exchange == k)
                        {
                            return true;
                        }
                        return false;
                    }), EnumQuoteListType.STOCK_CN);
                }
            }
        }


        void Debug(string msg)
        {
            debugForm.Debug(msg);
        }
    }
}
