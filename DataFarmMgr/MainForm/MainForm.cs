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


        void mdClient_OnInitializedEvent()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(mdClient_OnInitializedEvent), new object[] { });
            }
            else
            {
                logger.Info("MDClient Inited");

                foreach (var target in DataCoreService.DataClient.Symbols)
                {
                    MDSymbol symbol = new MDSymbol();
                    symbol.Symbol = target.Symbol;
                    symbol.SecCode = target.SecurityFamily.Code;
                    symbol.Name = target.GetName();
                    symbol.Currency = MDCurrency.RMB;
                    symbol.Exchange = target.Exchange;
                    symbol.Multiple = target.Multiple;
                    symbol.SecurityType = MDSecurityType.FUT;
                    symbol.SizeRate = 1;
                    symbol.NCode = 0;
                    symbol.SortKey = target.Month;
                    mdsymbolmap.Add(symbol.UniqueKey, symbol);

                }

                ctrlQuoteList.SetSymbols(mdsymbolmap.Values);
                ctrlQuoteList.SelectTab(0);

                foreach (var exchange in DataCoreService.DataClient.Exchanges)
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
                    }), EnumQuoteListType.FUTURE_OVERSEA);
                }
            }
        }


        void Debug(string msg)
        {
            debugForm.Debug(msg);
        }
    }
}
