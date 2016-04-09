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

using TradingLib.KryptonControl;
using TradingLib.Chart;
using Easychart.Finance.DataProvider;
using TradingLib.DataProvider;



namespace DataFarmMgr
{
    public partial class fmDataMgr : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        MDClient client = null;
        ILog logger = LogManager.GetLogger("DataFarmMgr");
        
        PageQuoteMoniter quoteMoniter=null;
        ctlEasyChart easyChart = null;

        PageHolder pageHolder = new PageHolder();

        TLMemoryDataManager mdmIntraday = null;
        TLMemoryDataManager mdmHist = null;

        
        public fmDataMgr()
        {
            InitializeComponent();
            //
            quoteMoniter = new PageQuoteMoniter();
            quoteMoniter.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(quoteMoniter);
            pageHolder.AddPage(quoteMoniter);

            easyChart = new ctlEasyChart();
            easyChart.Dock = DockStyle.Fill;
            
            mainPanel.Controls.Add(easyChart);
            pageHolder.AddPage(easyChart);



            



            
            WireEvent();
        }

        void WireEvent()
        {
            menuConnect.Click += new EventHandler(menuConnect_Click);

            
        }


        void menuConnect_Click(object sender, EventArgs e)
        {
            //client = new MDClient("114.55.72.206", 5060, 5060);
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
                //初始化QuoteMoniter
                foreach (var exchange in client.Exchanges)
                {
                    quoteMoniter.AddExchange(exchange);
                    IEnumerable<Symbol> symbols = client.Symbols.Where(sym => sym.SecurityFamily.Exchange.EXCode == exchange.EXCode).OrderBy(sym => sym.Symbol);
                    quoteMoniter.AddSymbols(exchange, symbols);
                }
                //初始化完成后 绑定事件
                quoteMoniter.ExchangeChangedEvent += new Action<IExchange>(ctQuoteMoniter_ExchangeChangedEvent);
                quoteMoniter.OpenChartEvent += new Action<Symbol>(quoteMoniter_OpenChartEvent);
                //
                

                mdmIntraday = new TLMemoryDataManager(client, true);
                easyChart.BindDataManager(mdmIntraday);
                easyChart.OpenQuoteEvent += new Action<Symbol>(easyChart_OpenQuoteEvent);

            }
        }

        void easyChart_OpenQuoteEvent(Symbol obj)
        {
            pageHolder.ShowPage(PageConstants.PAGE_QUOTE);
            quoteMoniter.ShowQuote(obj);
        }


        /// <summary>
        /// 报价列表控件交易所切换
        /// </summary>
        /// <param name="obj"></param>
        void ctQuoteMoniter_ExchangeChangedEvent(IExchange obj)
        {
            client.UnRegisterAllSymbols();

            string[] symbols = client.Symbols.Where(sym => sym.SecurityFamily.Exchange.EXCode == obj.EXCode).Select(sym => sym.Symbol).ToArray();
            client.RegisterSymbol(symbols);
        }

        /// <summary>
        /// 报价列表触发打开KChart事件
        /// </summary>
        /// <param name="obj"></param>
        void quoteMoniter_OpenChartEvent(Symbol obj)
        {
            pageHolder.ShowPage(PageConstants.PAGE_CHART);
            easyChart.ShowChart(obj);

        }



        void client_OnRtnTickEvent(Tick obj)
        {
            Symbol symbol = client.GetSymbol(obj.Symbol);
            if (symbol != null)
            {
                quoteMoniter.GotTick(symbol, obj);
            }
        }
    }
}
