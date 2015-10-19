using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using TradingLib.TraderControl;
using Common.Logging;



namespace FutsTrader
{
    public partial class TradeLite : Telerik.WinControls.UI.RadForm,IEventBinder
    {
        ILog logger = LogManager.GetLogger("TradeLite");
        public TradeLite()
        {
            InitializeComponent();
            //this.Text = Globals.CompanyName;
            //ThemeResolutionService.ApplicationThemeName = "Office2010Black";
            //ThemeResolutionService.ApplicationThemeName = "TelerikMetro";
            ThemeResolutionService.ApplicationThemeName = "Windows8";
            //ThemeResolutionService.ApplicationThemeName = "Office2010Silver";
            //ThemeResolutionService.ApplicationThemeName = "TelerikMetroBlue";

            //_runner = new LogicRunner(this,viewQuoteList1, ctOrderView1, ctTradeView1, ctPositionView1, ctOrderSender1,ctAccountInfo1);
            //_runner.SendDebugEvent +=new TradingLib.API.DebugDelegate(debug);
            //_runner.Init();
            //new Thread(_runner.Start).Start();

           // UIGlobals.MainForm = this;
            viewQuoteList1.SelectedColor = System.Drawing.ColorTranslator.FromHtml("#569DE5");
            btnCashIn.Click += new EventHandler(btnCashIn_Click);
            btnCashOut.Click += new EventHandler(btnCashOut_Click);
            btnChangePass.Click += new EventHandler(btnChangePass_Click);

            if (string.IsNullOrEmpty(Globals.CashIn))
            {
                btnCashIn.Visible = false;
            }
            if (string.IsNullOrEmpty(Globals.CashOut))
            {
                btnCashOut.Visible = false;
            }
            this.Load += new EventHandler(TradeLite_Load);
            this.FormClosing += new FormClosingEventHandler(TradeLite_FormClosing);
            this.SizeChanged += new EventHandler(TradeLite_SizeChanged);
        }

        void btnCashOut_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Globals.CashOut))
            {
                TraderHelper.OpenURL(Globals.CashOut);
            }
        }

        void btnChangePass_Click(object sender, EventArgs e)
        {
            fmChangePassword fm = new fmChangePassword();
            fm.ShowDialog();
            fm.Close();
        }

        void btnCashIn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Globals.CashIn))
            {
                TraderHelper.OpenURL(Globals.CashIn);
            }
        }
    

        void TradeLite_SizeChanged(object sender, EventArgs e)
        {
            logger.Info("form size changed, windowstat:" + this.WindowState.ToString());
        }

        void TradeLite_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!(TraderHelper.ConfirmWindow("确认退出交易客户端?") == System.Windows.Forms.DialogResult.Yes))
            {
                e.Cancel = true;
            }
        }

        void TradeLite_Load(object sender, EventArgs e)
        {
            InitBW();

            WireEvent();
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        void WireEvent()
        {

            CoreService.EventCore.OnConnectedEvent += new VoidDelegate(EventCore_OnConnectedEvent);
            CoreService.EventCore.OnDisconnectedEvent += new VoidDelegate(EventCore_OnDisconnectedEvent);
            CoreService.EventCore.OnDataConnectedEvent += new VoidDelegate(EventCore_OnDataConnectedEvent);
            CoreService.EventCore.OnDataDisconnectedEvent += new VoidDelegate(EventCore_OnDataDisconnectedEvent);

            //将相关操作类的Rsp暴露到RspInfoEvent用于同一对外显示
            CoreService.EventCore.OnRspInfoEvent += new Action<RspInfo>(EventCore_OnRspInfoEvent);
            //绑定行情到Quote控件
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(OnTick);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(OnFill);
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(OnOrder);

            CoreService.EventIndicator.GotErrorOrderEvent += new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent);
            CoreService.EventIndicator.GotErrorOrderActionEvent += new Action<OrderAction, RspInfo>(EventIndicator_GotErrorOrderActionEvent);
            
            CoreService.EventCore.RegIEventHandler(this);


            this.menuAboutUS.Click += new EventHandler(menuAboutUS_Click);
            this.msgbtn.MouseEnter += new EventHandler(msgbtn_MouseEnter);
            this.msgbtn.MouseLeave += new EventHandler(msgbtn_MouseLeave);
        }

        void EventCore_OnRspInfoEvent(RspInfo obj)
        {
            ShowMessage(MsgSource.SYSTEM, obj.ErrorMessage, obj.ErrorID == 0 ? MsgType.MESSAGE : MsgType.ERROR);
        }

        

        void msgbtn_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void msgbtn_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        void menuAboutUS_Click(object sender, EventArgs e)
        {
            AboutUS fm = new AboutUS();
            fm.ShowDialog();
        }

        void EventIndicator_GotErrorOrderEvent(Order arg1, RspInfo arg2)
        {
            ShowMessage(MsgSource.ORDER, arg2.ErrorMessage,MsgType.ERROR);
        }

        void EventIndicator_GotErrorOrderActionEvent(OrderAction arg1, RspInfo arg2)
        {
            ShowMessage(MsgSource.ORDER, string.Format("{0}-{1}", arg2.ErrorID, arg2.ErrorMessage), MsgType.ERROR);
        }


        void OnOrder(Order o)
        {
            if(o.Status == QSEnumOrderStatus.Opened)
            {
                ShowMessage(MsgSource.ORDER, string.Format("{0}-{1} {2} {3}手 {4} 提交成功",o.OrderSeq, GetOrderPrice(o), o.Side ? "买入" : "卖出", o.UnsignedSize, o.Symbol));
            }
            else if (o.Status == QSEnumOrderStatus.Canceled)
            {
                ShowMessage(MsgSource.ORDER, string.Format("委托:{0} 撤单成功", o.OrderSeq));
            }
        }

        string GetOrderPrice(Order o)
        {
            if (o.isMarket)
            {
                return "市价";
            }
            else
            {
                return "限价:" + Util.FormatDecimal(o.LimitPrice, TraderHelper.GetDisplayFormat(o.oSymbol));
            }
        }

        void OnFill(Trade f)
        {
            ShowMessage(MsgSource.TRADE, string.Format("{0}-{1} {2} {3} 成交价格:{4}",f.TradeID, f.Side ? "买入" : "卖出", f.UnsignedSize, f.Symbol, Util.FormatDecimal(f.xPrice, TraderHelper.GetDisplayFormat(f.oSymbol))));
        }

        void OnTick(Tick k)
        {
            try
            {
                //viewQuoteList1.GotTick(k);
                //ctPositionView1.GotTick(k);
                //ctOrderSender1.GotTick(k);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void OnInit()
        {
            this.Text = "欢迎使用XTrader";
            //设定行情列表
            foreach (var symbol in CoreService.BasicInfoTracker.Symbols.OrderBy(sym => sym.SecurityFamily.Code))
            {
                viewQuoteList1.addSecurity(symbol);
            }

            //查询登入状态
            exStatus.Image = (System.Drawing.Image)(CoreService.TLClient.IsConnected ? Properties.Resources.connected : Properties.Resources.disconnected);
            mdStatus.Image = (System.Drawing.Image)(CoreService.TLClient.IsTickConnected ? Properties.Resources.connected : Properties.Resources.disconnected);
        }

        public void OnDisposed()
        { 
            
        }

        void EventCore_OnDataDisconnectedEvent()
        {
            mdStatus.Image = (System.Drawing.Image)Properties.Resources.disconnected;
        }

        void EventCore_OnDataConnectedEvent()
        {
            mdStatus.Image = (System.Drawing.Image)Properties.Resources.connected;
        }

        void EventCore_OnDisconnectedEvent()
        {
            exStatus.Image = (System.Drawing.Image)Properties.Resources.disconnected;
            mdStatus.Image = (System.Drawing.Image)Properties.Resources.disconnected;
        }

        void EventCore_OnConnectedEvent()
        {
            exStatus.Image = (System.Drawing.Image)Properties.Resources.connected;
        }




        #region windows事件截获
        const int WM_SYSCOMMAND = 0x112;
        const int SC_CLOSE = 0xF060;//关闭
        const int SC_MINIMIZE = 0xF020;//最小化
        const int SC_MAXIMIZE = 0xF030;//最大化
        const int WM_NCLBUTTONDBLCLK = 0x00A3;//双击标题栏
        int _lastSplitPanel1Height = 0;
        bool IsSmallPanel { get { return _lastSplitPanel1Height != 0; } }
        void SwitchFormHeight()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                if (_lastSplitPanel1Height == 0)
                {   //变小
                    _lastSplitPanel1Height = this.splitPanel1.Height;
                    this.splitPanel1.Height = 0;
                    this.Height = this.Height - _lastSplitPanel1Height;
                    this.Location = new Point(this.Location.X, this.Location.Y + _lastSplitPanel1Height);
                    //this.Opacity = 0.8;
                    
                }
                else
                {   //变大
                    this.splitPanel1.Height = _lastSplitPanel1Height;
                    this.Height = this.Height + _lastSplitPanel1Height;
                    this.Location = new Point(this.Location.X, this.Location.Y - _lastSplitPanel1Height);
                    _lastSplitPanel1Height = 0;
                    //this.Opacity = 1;

                }
            }
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
               //MessageBox.Show("syscommand");
            }
            //标题栏双击事件
            //if (m.Msg == WM_NCLBUTTONDBLCLK && m.WParam.ToInt32() == 2)
            {
                //SwitchFormHeight();
            }
            //else
                base.WndProc(ref m);
        }
        #endregion

        #region 界面事件处理
        private void SplitContainer_Click(object sender, EventArgs e)
        {
            SwitchFormHeight();
        }

        MessageHistoryForm msgHistForm = new MessageHistoryForm();

        private void msgbtn_DoubleClick(object sender, EventArgs e)
        {

            
            msgHistForm.Show();
            msgHistForm.Location = new Point(this.Location.X - msgHistForm.Size.Width, this.Location.Y);
        }

        #endregion





        #region 辅助函数
        void ShowMessage(MsgSource source, string content,MsgType type = MsgType.MESSAGE)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MsgSource, string, MsgType>(ShowMessage), new object[] { source,content,type });
            }
            else
            {

                if (type == MsgType.ERROR)
                {
                    this.message.Text = string.Format("【{0}-{1}】 {2}", Util.GetEnumDescription(source),Util.GetEnumDescription(type),content);
                }
                else
                {
                    this.message.Text = string.Format("【{0}】 {1}", Util.GetEnumDescription(source), content);
                }
                this.message.Opacity = 1;//触发消息时候为不透明 事件推移透明

                msgHistForm.GotMessage(this.message.Text);

            }
        }
        #endregion
        #region 后台worker用于更新界面
        private System.ComponentModel.BackgroundWorker bg;
        private void bgDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                if (message.Opacity != 0)
                {
                    double targetOpacity =message.Opacity-0.02;
                    message.Opacity = targetOpacity >= 0 ? targetOpacity : 0;
                }
                Thread.Sleep(50);
            }
        }
        //启动后台工作进程 用于检查信息并调用弹出窗口
        private void InitBW()
        {
            bg = new System.ComponentModel.BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bgDoWork);
            //bg.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bg_ProgressChanged);
            bg.RunWorkerAsync();
        }
        #endregion





    }

    /// <summary>
    /// 消息来源
    /// </summary>
    enum MsgSource
    { 
        [Description("委托")]
        ORDER,
        [Description("成交")]
        TRADE,
        [Description("系统")]
        SYSTEM,
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    enum MsgType
    {
        [Description("错误")]
        ERROR,
        [Description("提示")]
        MESSAGE,
    }
}

//winform default size 612, 354