using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Common.Logging;
using TradingLib.KryptonControl;
using TradingLib.MarketData;
using System.Reflection;


namespace XTraderLite
{
    public partial class MainForm : Form,TradingLib.MarketData.IEventBinder
    {
        ILog logger = LogManager.GetLogger("MainForm");

        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);


        //API声明：获取当前焦点控件句柄      

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]

        internal static extern IntPtr GetFocus();


        List<ServerNode> srvList = new List<ServerNode>();

        WatchList watchList = new WatchList();

        ///获取 当前拥有焦点的控件
        private Control GetFocusedControl()
        {

            Control focusedControl = null;

            // To get hold of the focused control:

            IntPtr focusedHandle = GetFocus();

            if (focusedHandle != IntPtr.Zero)

                //focusedControl = Control.FromHandle(focusedHandle);

                focusedControl = Control.FromChildHandle(focusedHandle);

            return focusedControl;

        }

        frmDebug _debugform = null;
        public MainForm()
        {
            InitializeComponent();
            //默认窗口最大化
            //this.WindowState = FormWindowState.Maximized;
            btnMax_Click(null, null);
            _debugform = new frmDebug();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(Debug);
            this.DoubleBuffered = true;

            //绑定界面事件
            WireEvent();
            //初始化控件
            InitControls();
            //初始化报价列表
            InitQuoteList();
            //初始化绘图控件
            InitKChart();
            //初始化其他视图
            InitOtherView();
            //初始化键盘精灵
            InitSearchBox();

            //初始化交易插件
            //InitTrader();
        }

        void Debug(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Debug), new object[] { msg });
            }
            else
            {
                _debugform.Debug(msg);
            }
        }

        LinkedList<string> freqLink = new LinkedList<string>();//频率双向链表 用于F8循环切换

        void InitControls()
        {
            this.KeyPreview = true;//Gets or sets a value indicating whether the form will receive key events before the event is passed to the control that has focus.
            //this.panelHolder.Width = this.Width - 2;
            //this.panelHolder.Dock = DockStyle.Fill;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //_curView = ctrlQuoteList;//设置默认当前视图控件

            #region 设置频率切换按钮的Tag 并放入list方便访问
            btnFreqDay.Tag = ConstFreq.Freq_Day;
            btnFreqWeek.Tag = ConstFreq.Freq_Week;
            btnFreqMonth.Tag = ConstFreq.Freq_Month;
            btnFreqQuarter.Tag = ConstFreq.Freq_Quarter;
            btnFreqYear.Tag = ConstFreq.Freq_Year;
            btnFreqM1.Tag = ConstFreq.Freq_M1;
            btnFreqM3.Tag = ConstFreq.Freq_M3;
            btnFreqM5.Tag = ConstFreq.Freq_M5;
            btnFreqM10.Tag = ConstFreq.Freq_M10;
            btnFreqM15.Tag = ConstFreq.Freq_M15;
            btnFreqM30.Tag = ConstFreq.Freq_M30;
            btnFreqM60.Tag = ConstFreq.Freq_M60;
            btnFreqH2.Tag = ConstFreq.Freq_H2;

            freqLink.AddLast(ConstFreq.Freq_M1);
            freqLink.AddLast(ConstFreq.Freq_M3);
            freqLink.AddLast(ConstFreq.Freq_M5);
            freqLink.AddLast(ConstFreq.Freq_M10);
            freqLink.AddLast(ConstFreq.Freq_M15);
            freqLink.AddLast(ConstFreq.Freq_M30);
            freqLink.AddLast(ConstFreq.Freq_M60);
            freqLink.AddLast(ConstFreq.Freq_H2);
            freqLink.AddLast(ConstFreq.Freq_Day);
            freqLink.AddLast(ConstFreq.Freq_Week);
            freqLink.AddLast(ConstFreq.Freq_Month);
            freqLink.AddLast(ConstFreq.Freq_Quarter);
            freqLink.AddLast(ConstFreq.Freq_Year);
            
            freqButtons.Add(btnFreqDay);
            freqButtons.Add(btnFreqWeek);
            freqButtons.Add(btnFreqMonth);
            freqButtons.Add(btnFreqQuarter);
            freqButtons.Add(btnFreqYear);
            freqButtons.Add(btnFreqM1);
            freqButtons.Add(btnFreqM3);
            freqButtons.Add(btnFreqM5);
            freqButtons.Add(btnFreqM10);
            freqButtons.Add(btnFreqM15);
            freqButtons.Add(btnFreqM30);
            freqButtons.Add(btnFreqM60);
            freqButtons.Add(btnFreqH2);
            #endregion



            panelMarket.BackColor = Color.Black;
            panelBroker.Visible = false;
            //debugControl1.Dock = DockStyle.Fill;

            viewMap.Add(ctrlQuoteList.ViewType, ctrlQuoteList);
            viewMap.Add(ctrlKChart.ViewType, ctrlKChart);
            viewMap.Add(ctrlTickList.ViewType, ctrlTickList);
            viewMap.Add(ctrlPriceVolList.ViewType, ctrlPriceVolList);
            viewMap.Add(ctrlSymbolInfo.ViewType, ctrlSymbolInfo);

            ctrlKChart.Dock = DockStyle.Fill;
            ctrlQuoteList.Dock = DockStyle.Fill;
            ctrlTickList.Dock = DockStyle.Fill;
            ctrlPriceVolList.Dock = DockStyle.Fill;
            ctrlSymbolInfo.Dock = DockStyle.Fill;

            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //this.Update();
            //typeof(Panel).InvokeMember("DoubleBuffered",BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,null, panelBroker, new object[] { true });

            if (Global.IsXGJStyle)
            {
                btnBack.Image = Properties.Resources.xqj_back;
                btnHome.Image = Properties.Resources.xqj_home;

                btnF10.Image = Properties.Resources.xqj_f10;


                btnBarView.Image = Properties.Resources.xqj_bar;
                btnIntraView.Image = Properties.Resources.xqj_timeview;

                btnQuoteView.Image = Properties.Resources.xqj_quote;
                btnFreqDay.Image = Properties.Resources.xgj_freq_day;
                btnFreqWeek.Image = Properties.Resources.xgj_freq_week;
                btnFreqMonth.Image = Properties.Resources.xgj_freq_month;
                btnFreqQuarter.Image = Properties.Resources.xgj_freq_quarter;
                btnFreqYear.Image = Properties.Resources.xgj_freq_x;
                btnRefresh.Image = Properties.Resources.xqj_refresh;
                btnDrawBox.Image = Properties.Resources.xqj_draw;


                btnFreqM1.Image = Properties.Resources.xgj_freq_1min;
                btnFreqM3.Image = Properties.Resources.xqj_freq_3min;
                btnFreqM5.Image = Properties.Resources.xqj_freq_5min;
                btnFreqM10.Image = Properties.Resources.xqj_freq_10min;
                btnFreqM15.Image = Properties.Resources.xqj_freq_15min;
                btnFreqM30.Image = Properties.Resources.xqj_freq_30min;
                btnFreqM60.Image = Properties.Resources.xqj_freq_60min;
                btnFreqH2.Image = Properties.Resources.xqj_freq_2hour;

                //this.Icon = Properties.Resources.xgj;
                //btnTickList.Visible = false;
                //btnPriceVolList.Visible = false;
                //btnWatchList.Visible = false;
                //toolStripSeparator5.Visible = false;
                this.panelSiteInfo.Visible = false;

            }

            cornerImg.Image = Image.FromHbitmap(Icon.ExtractAssociatedIcon(Application.ExecutablePath).ToBitmap().GetHbitmap());
            cornerImg.Visible = Global.ShowCorner;
            if (!Global.ShowCorner)
            {
                panelMenu.Location = new Point(panelMenu.Location.X - cornerImg.Width, panelMenu.Location.Y);
            }
            this.Text = Global.TaskBarTitle;
            //使用全局图标
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            //if (!Global.ClassicLogin)
            //{
            //    this.Icon = Properties.Resources.xgj;
            //}

            //在线出入金菜单
            toolStripSeparatorPay.Visible = false;
            menuPay.Visible = false;
            //if(!string.IsNullOrEmpty(Global.PayUrl))
            //{
            //    toolStripSeparatorPay.Visible = true;
            //    menuPay.Visible = true;
            //}

            #region 加载行情服务站点
            srvList =  (new ServerConfig("market.cfg")).GetServerNodes();
            #endregion

            UpdateTime();


        }

        void WireEvent()
        {

            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.SizeChanged += new EventHandler(MainForm_SizeChanged);
            this.Load += new EventHandler(MainForm_Load);

            MDService.EventHub.RegIEventHandler(this);
            MDService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            MDService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);


            splitter.SplitterMoved += new SplitterEventHandler(splitter_SplitterMoved);
            WireFormOperation();

            WireToolBar();

            WireMenu();

            WireUI();
            
        }

        void splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            logger.Info(string.Format("splitter move X:{0} Y:{1}", e.SplitX, e.SplitY));
        }

        void EventHub_OnDisconnectedEvent()
        {
            menuConnect.Enabled = true;
            menuDisconnect.Enabled = false;
            UpdateConnImg(false);

            UpdateServerInfo();
        }

        void EventHub_OnConnectedEvent()
        {
            menuConnect.Enabled = false;
            menuDisconnect.Enabled = true;
            UpdateConnImg(true);

            UpdateServerInfo();

            //连接完毕后 执行数据订阅
            MDService.DataAPI.RegisterSymbol(symbolRegister.ToArray());
        }


        void MainForm_Load(object sender, EventArgs e)
        {
            //初始视图为报价列表
            ViewQuoteList();

            //new System.Threading.Thread(LoadTrader).Start(); 
            InitTrader();
        }

        /// <summary>
        /// 底层基础数据初始化完毕后被调用
        /// </summary>
        public void OnInit()
        {
            BindDataAPICallBack();

            //初始化自选数据
            watchList.Load();
            watchList.WatchListChanged += new Action(watchList_WatchListChanged);

            //初始化板块信息
            if (Global.QuoteBlockList.Count == 0)
            {
                ctrlQuoteList.AddBlock("所有品种", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    return true;
                })
                , TradingLib.XTrader.Control.EnumQuoteListType.FUTURE_IQFeed);


                foreach (var b in MDService.DataAPI.BlockInfos)
                {
                    ctrlQuoteList.AddBlock(b.Title, b.Filter, (TradingLib.XTrader.Control.EnumQuoteListType)b.QuoteViewType);
                }

                ctrlQuoteList.AddBlock("自选", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    return watchList.IsWatched(symbol.Symbol);
                })
                , TradingLib.XTrader.Control.EnumQuoteListType.FUTURE_IQFeed, watchList.GetWatchSymbols);
            }
            else
            {
                ctrlQuoteList.AddBlock("所有品种", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    return Global.QuoteBlockList.Contains(symbol.Exchange);
                })
                , TradingLib.XTrader.Control.EnumQuoteListType.FUTURE_IQFeed);

                //按顺序添加到报价列表
                foreach (var key in Global.QuoteBlockList)
                {
                    var b = MDService.DataAPI.BlockInfos.Where(item => item.Key == key).FirstOrDefault();
                    if (b == null) continue;
                    ctrlQuoteList.AddBlock(b.Title, b.Filter, (TradingLib.XTrader.Control.EnumQuoteListType)b.QuoteViewType);
                }
                ctrlQuoteList.AddBlock("自选", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    return watchList.IsWatched(symbol.Symbol);
                })
                , TradingLib.XTrader.Control.EnumQuoteListType.FUTURE_IQFeed, watchList.GetWatchSymbols);
            }
            //数据完毕后 初始化报价面板并以第一个tab为默认视图
            ctrlQuoteList.SetSymbols(MDService.DataAPI.Symbols);

            int idx = -1;
            if (!string.IsNullOrEmpty(Global.DefaultBlock))
            {
                var b = MDService.DataAPI.BlockInfos.Where(item => item.Key == Global.DefaultBlock).FirstOrDefault();
                if (b != null)
                {
                    idx = ctrlQuoteList.GetIndex(b.Title);
                }
            }

            ctrlQuoteList.SelectTab(idx>=0?idx:0);

            //设置底部跑马灯
            InitHightLight();

            InitUserSetting();


            UpdateServerInfo();

            //启动定时任务
            InitTimer();
        }

        void watchList_WatchListChanged()
        {
            if (ctrlQuoteList.IsWatchMode)
            {
                ctrlQuoteList.Reload();
            }
        }






        

        public void OnDisposed()
        { 
            
        }

        void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //调节标题logo位置
            if (this.Width < 250 + 150 + topHeader.Width + 260)
            {
                topHeader.Visible = false;
            }
            else
            {
                topHeader.Visible = true;
                //250 菜单宽度 75 ControlBox宽度
                topHeader.Location = new Point(250 + (this.Width - 250 - 260 - topHeader.Width) / 2, topHeader.Location.Y);
            }

            //调整panelBroker高度
            //if (panelHolder.Height < panelBroker.Height)
            //{
            //    panelBroker.Height = panelHolder.Height;
            //}
            //else
            //{
            //    if (_panelBrokerMax)
            //    {
            //        panelBroker.Height = panelHolder.Height;
            //    }
            //}
            //调节交易面板为最小值 如果移动的splitter则设置为当前值
            //if (!_splitterMoved)
            //{
            //    splitContainer.SplitterDistance = this.Height;
            //}
        }


        void WireFormOperation()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnMin.Click += new EventHandler(btnMin_Click);

            btnDemo3.Click += new EventHandler(btnDemo3_Click);
            btnDemo2.Click += new EventHandler(btnDemo2_Click);
            btnDemo1.Click += new EventHandler(btnDemo1_Click);

            panelTop.MouseDown += new MouseEventHandler(move_MouseDown);
            panelTop.MouseUp += new MouseEventHandler(move_MouseUp);
            panelTop.MouseMove += new MouseEventHandler(move_MouseMove);
            panelTop.DoubleClick += new EventHandler(Form_DoubleClick);
            topHeader.MouseDown += new MouseEventHandler(move_MouseDown);
            topHeader.MouseUp += new MouseEventHandler(move_MouseUp);
            topHeader.MouseMove += new MouseEventHandler(move_MouseMove);
            topHeader.DoubleClick += new EventHandler(Form_DoubleClick);
            panelMenu.MouseDown += new MouseEventHandler(move_MouseDown);
            panelMenu.MouseUp += new MouseEventHandler(move_MouseUp);
            panelMenu.MouseMove += new MouseEventHandler(move_MouseMove);
            panelMenu.DoubleClick += new EventHandler(Form_DoubleClick);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | 0x20000;//允许最小化操作
                return cp;
            }
        }


        #region 顶部Panel移动窗体
        private bool m_isMouseDown = false;
        private Point m_mousePos = new Point();
        void move_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        void move_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
        }

        void move_MouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        void Form_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }
        #endregion

        #region 最大 最小 关闭操作
        void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("退出金融投资分析系统?","确认",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                //this.Close();
                //try
                //{
                //    //timer.Stop();
                //    //System.Threading.ThreadPool.QueueUserWorkItem(o => MDService.DataAPI.Disconnect());

                //    //MDService.DataAPI.Disconnect();
                //    //Application.ExitThread();
                //    System.Environment.Exit(0);
                //}
                //catch (Exception ex)
                //{
                //    System.Diagnostics.Process.GetCurrentProcess().Kill();
                //}
            }
        }
        #endregion


    }
}
