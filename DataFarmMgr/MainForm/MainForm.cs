using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;
using TradingLib.XTrader.Control;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {
        ILog logger = LogManager.GetLogger("MainForm");

       

        DebugForm debugForm = null;
        public MainForm()
        {
            InitializeComponent();
            debugForm = new DebugForm();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(Debug);

            WireEvent();

            InitMenu();

            InitControl();

            InitQuoteLists();

            InitToolBar();
        }

        void WireEvent()
        {
           
            
        }


        void InitControl()
        {
            ctrlQuoteList.Dock = DockStyle.Fill;

        }


        Dictionary<string, MDSymbol> mdsymbolmap = new Dictionary<string, MDSymbol>();

        MDSymbol GetMDSymbol(string key)
        {
            MDSymbol target = null;
            if (mdsymbolmap.TryGetValue(key, out target))
                return target;
            return null;
        }


        


        void Debug(string msg)
        {
            debugForm.Debug(msg);
        }
    }
}
