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


namespace XTraderLite
{
    public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm,TradingLib.MarketData.IEventBinder
    {
        ILog logger = LogManager.GetLogger("MainForm");

        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);


        DataAPI.TDX.TDXDataAPI _dataAPI = null;
        public MainForm()
        {
            InitializeComponent();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(debug);

            WireEvent();

            InitControls();

            InitQuoteList();

            InitKChart();

            InitDataAPI();
        }

       

        void InitControls()
        {
            this.KeyPreview = true;//Gets or sets a value indicating whether the form will receive key events before the event is passed to the control that has focus.
            splitContainer.Panel2Collapsed = true;
            debugControl1.Dock = DockStyle.Fill;

            viewList.Add(panelQuoteList);
            viewList.Add(panelKChart);


            panelKChart.Dock = DockStyle.Fill;
            panelQuoteList.Dock = DockStyle.Fill;

        }
        void WireEvent()
        {

            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
            this.SizeChanged += new EventHandler(MainForm_SizeChanged);
            this.Load += new EventHandler(MainForm_Load);


            MDService.EventHub.RegIEventHandler(this);



            btnClose.Click += new EventHandler(btnClose_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnMin.Click += new EventHandler(btnMin_Click);

            btnDemo3.Click += new EventHandler(btnDemo3_Click);
            btnDemo2.Click += new EventHandler(btnDemo2_Click);
            btnDemo1.Click += new EventHandler(btnDemo1_Click);

            topMenuPanel.MouseDown += new MouseEventHandler(TopMenuPanel_MouseDown);
            topMenuPanel.MouseUp += new MouseEventHandler(TopMenuPanel_MouseUp);
            topMenuPanel.MouseMove += new MouseEventHandler(TopMenuPanel_MouseMove);
            topMenuPanel.DoubleClick += new EventHandler(TopMenuPanel_DoubleClick);


            //toolbar
            btnQuoteView.Click += new EventHandler(btnQuoteView_Click);
            btnIntraView.Click += new EventHandler(btnIntraView_Click);
            btnBarView.Click += new EventHandler(btnBarView_Click);



            menuTrading.Click += new EventHandler(menuTrading_Click);
            menuSwitchKchart.Click += new EventHandler(menuSwitchKchart_Click);

            splitContainer.SplitterMoved += new SplitterEventHandler(splitContainer_SplitterMoved);
            
        }






        void MainForm_Load(object sender, EventArgs e)
        {
            SetViewType(EnumTraderViewType.QuoteList);
        }

        /// <summary>
        /// 底层基础数据初始化完毕后被调用
        /// </summary>
        public void OnInit()
        {
            quoteView.Symbols = MDService.DataAPI.Symbols;
            quoteView.SelectTab(0);

            MDService.DataAPI.OnRspQryMinuteData += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQryMinuteData);
            MDService.DataAPI.OnRspQrySecurityBar += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQrySecurityBar);
        }

        

        public void OnDisposed()
        { 
            
        }
        

        

        void debug(string msg)
        {
            debugControl1.GotDebug(msg);
        }

        void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //调节交易面板为最小值 如果移动的splitter则设置为当前值
            if (!_splitterMoved)
            {
                splitContainer.SplitterDistance = this.Height;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }


        #region 顶部Panel移动窗体
        private bool m_isMouseDown = false;
        private Point m_mousePos = new Point();
        void TopMenuPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        void TopMenuPanel_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
        }

        void TopMenuPanel_MouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;

            
        }

        void TopMenuPanel_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //max11.Image = stock2.Properties.Resources.C2;

            }
            else
            {
                WindowState = FormWindowState.Normal;
                //max11.Image = stock2.Properties.Resources.C3;
            }
        }
        #endregion




        #region 最大 最小 关闭操作
        void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //this.btnMin.Enabled = false;
        }

        void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //max11.Image = stock2.Properties.Resources.C2;

            }
            else
            {
                WindowState = FormWindowState.Normal;
                //max11.Image = stock2.Properties.Resources.C3;
               
            }
            
            //this.max11.Enabled = false;
            //this.max11.Enabled = true;

           
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }
        #endregion


    }
}
