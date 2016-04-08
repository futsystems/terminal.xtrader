using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MDClient;
using Common.Logging;

namespace DataFarmMgr
{
    public partial class Form1 : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        MDClient client = null;
        ILog logger = LogManager.GetLogger("DataFarmMgr");

        public Form1()
        {
            InitializeComponent();
            WireEvent();
        }

        void WireEvent()
        {
            menuConnect.Click += new EventHandler(menuConnect_Click);
            
        }

        void ctQuoteMoniter1_ExchangeChangedEvent(IExchange obj)
        {
            client.UnRegisterAllSymbols();

            string[] symbols = client.Symbols.Where(sym => sym.SecurityFamily.Exchange.EXCode == obj.EXCode).Select(sym => sym.Symbol).ToArray();
            client.RegisterSymbol(symbols);
        }

        void menuConnect_Click(object sender, EventArgs e)
        {
            client = new MDClient("127.0.0.1", 5060, 5060);
            client.OnRtnTickEvent += new Action<Tick>(client_OnRtnTickEvent);
            client.OnInitializedEvent += new Action(OnInitializedEvent);
            client.Start();
        }




        void OnInitializedEvent()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnInitializedEvent),new object[]{});
            }
            else
            {
                foreach (var exchange in client.Exchanges)
                {
                    ctQuoteMoniter1.AddExchange(exchange);
                    IEnumerable<Symbol> symbols = client.Symbols.Where(sym => sym.SecurityFamily.Exchange.EXCode == exchange.EXCode).OrderBy(sym => sym.Symbol);
                    ctQuoteMoniter1.AddSymbols(exchange,symbols);
                }

                
                //foreach (var symbol in client.Symbols)
                //{
                //    ctQuoteMoniter1.AddSymbol(symbol);
                //}
                //初始化完成后 绑定事件
                ctQuoteMoniter1.ExchangeChangedEvent += new Action<IExchange>(ctQuoteMoniter1_ExchangeChangedEvent);
                //注册合约
                //client.RegisterSymbol(client.Symbols.Select(t => t.Symbol).ToArray());
            }
        }

        void client_OnRtnTickEvent(Tick obj)
        {
            Symbol symbol = client.GetSymbol(obj.Symbol);
            if (symbol != null)
            {
                ctQuoteMoniter1.GotTick(symbol, obj);
            }
        }
    }
}
