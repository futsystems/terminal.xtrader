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
using Easychart.Finance.DataProvider;
using TradingLib.DataProvider;



namespace DataFarmMgr
{
    public partial class fmDataMgr : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        MDClient client = null;
        ILog logger = LogManager.GetLogger("DataFarmMgr");
        
        PageQuoteMoniter quoteMoniter=null;
        PageEasyChart easyChart = null;

        PageHolder pageHolder = new PageHolder();

        TLMemoryDataManager mdmIntraday = null;
        TLMemoryDataManager mdmHist = null;

        ToolStrip toolbar = null;
        ComponentFactory.Krypton.Toolkit.KryptonPanel mainPanel;
        public fmDataMgr()
        {
            InitializeComponent();

            this.toolbar = new ToolStrip();
            // 
            // toolStrip1
            // 
            this.toolbar.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            //this.toolStrip1.Size = new System.Drawing.Size(835, 25);
            this.toolbar.TabIndex = 0;
            //this.toolStrip1.Text = "toolStrip1";
            

            this.mainPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.mainPanel.Location = new System.Drawing.Point(0,this.toolbar.Height);
            this.mainPanel.Dock = DockStyle.Fill;

            this.Controls.Add(mainPanel);
            this.Controls.Add(toolbar);
            


            this.quoteMoniter = new PageQuoteMoniter();
            this.quoteMoniter.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Add(quoteMoniter);
            this.pageHolder.AddPage(quoteMoniter);

            this.easyChart = new PageEasyChart();
            this.easyChart.Dock = DockStyle.Fill;

            this.mainPanel.Controls.Add(easyChart);
            this.pageHolder.AddPage(easyChart);


            //初始化工具栏
            InitTooBar();

            WireEvent();
        }


        #region 工具栏
        void InitTooBar()
        {
            ToolStripButton btn = new ToolStripButton("连接");
            btn.Click += new EventHandler(BTN_CONNECT_Click);
            this.toolbar.Items.Add(btn);

            btn = new ToolStripButton("BTN_HOME",Properties.Resources.img_home);
            btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn.ToolTipText = "起始页";
            btn.Click += new EventHandler(BTN_HOME_Click);
            this.toolbar.Items.Add(btn);

            btn = new ToolStripButton(Properties.Resources.img_refresh);
            btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn.ToolTipText = "数据刷新";
            this.toolbar.Items.Add(btn);

            this.toolbar.Items.Add(new ToolStripSeparator());

            btn = new ToolStripButton("BTN_KCHART_1M", Properties.Resources.img_1M);
            btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn.ToolTipText = "1分钟";
            btn.Click += new EventHandler(BTN_KCHART_1M_Click);
            this.toolbar.Items.Add(btn);


            btn = new ToolStripButton("BTN_KCHART_3M", Properties.Resources.img_3M);
            btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn.ToolTipText = "3分钟";
            btn.Click += new EventHandler(BTN_KCHART_3M_Click);
            this.toolbar.Items.Add(btn);

            btn = new ToolStripButton("BTN_KCHART_5M", Properties.Resources.img_5M);
            btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn.ToolTipText = "5分钟";
            btn.Click += new EventHandler(BTN_KCHART_5M_Click);
            this.toolbar.Items.Add(btn);
        }



        void BTN_CONNECT_Click(object sender, EventArgs e)
        {
            //client = new MDClient("114.55.72.206", 5060, 5060);
            client = new MDClient("127.0.0.1", 5060, 5060);
            client.OnRtnTickEvent += new Action<Tick>(client_OnRtnTickEvent);
            client.OnInitializedEvent += new Action(OnInitializedEvent);
            client.Start();
        }

        /// <summary>
        /// 返回报价页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BTN_HOME_Click(object sender, EventArgs e)
        {
            pageHolder.ShowPage(PageConstants.PAGE_QUOTE);
        }

        /// <summary>
        /// 打开1分钟K线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BTN_KCHART_1M_Click(object sender, EventArgs e)
        {
            logger.Info("Open 1M Chart");
            pageHolder.ShowPage(PageConstants.PAGE_CHART);
            Symbol symbol = quoteMoniter.SelectedSymbol;
            if (symbol != null)
            {
                easyChart.ShowChart(symbol, new BarFrequency(BarInterval.CustomTime, 60));
            }
            else
            {
                logger.Warn("quote moniter have no symbol selected");
            }
        }

        /// <summary>
        /// 打开3分钟K线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BTN_KCHART_3M_Click(object sender, EventArgs e)
        {
            logger.Info("Open 3M Chart");
            pageHolder.ShowPage(PageConstants.PAGE_CHART);
            Symbol symbol = quoteMoniter.SelectedSymbol;
            if (symbol != null)
            {
                easyChart.ShowChart(symbol, new BarFrequency(BarInterval.CustomTime, 180));
            }
            else
            {
                logger.Warn("quote moniter have no symbol selected");
            }
        }
        /// <summary>
        /// 打开3分钟K线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BTN_KCHART_5M_Click(object sender, EventArgs e)
        {
            logger.Info("Open 5M Chart");
            pageHolder.ShowPage(PageConstants.PAGE_CHART);
            Symbol symbol = quoteMoniter.SelectedSymbol;
            if (symbol != null)
            {
                easyChart.ShowChart(symbol, new BarFrequency(BarInterval.CustomTime, 300));
            }
            else
            {
                logger.Warn("quote moniter have no symbol selected");
            }
        }




        #endregion



        void WireEvent()
        {
            //menuConnect.Click += new EventHandler(menuConnect_Click);

            
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }
    }
}
