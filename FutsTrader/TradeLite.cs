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
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using TradingLib.TraderControl;


namespace FutsTrader
{
    public partial class TradeLite : Telerik.WinControls.UI.RadForm,IEventBinder
    {
        //LogicRunner _runner;
        string _account="";
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

            this.Load += new EventHandler(TradeLite_Load);
        }

        void TradeLite_Load(object sender, EventArgs e)
        {
            WireEvent();
        }


        void debug(string msg)
        {
            ctDebug1.GotDebug(msg);
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

            //绑定行情到Quote控件
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(OnTick);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(OnFill);
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(OnOrder);


            CoreService.EventCore.RegIEventHandler(this);
            
        }

        void OnOrder(Order o)
        {
            //ctOrderView1.GotOrder(o);
        }

        void OnFill(Trade f)
        {
            //ctOrderView1
            ctTradeView1.GotFill(f);
        }

        void OnTick(Tick k)
        {
            try
            {
                viewQuoteList1.GotTick(k);
                ctPositionView1.GotTick(k);
                ctOrderSender1.GotTick(k);
            }
            catch (Exception ex)
            {
                debug(ex.ToString());
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

        private void SplitContainer_Click(object sender, EventArgs e)
        {
            SwitchFormHeight();
        }

        //自动登入
        void autoLogin()
        {
            /*
            string struser = Properties.Settings.Default.user;
            bool rememberpass = Properties.Settings.Default.RememberPass;
            string strpass = Properties.Settings.Default.pass;
            _loginID = struser;

            if ((!rememberpass) || string.IsNullOrEmpty(struser) || string.IsNullOrEmpty(strpass)) return;
            if (_coreCentre == null || _coreCentre.IsLoggedIn) return;

            _coreCentre.ReqLogin(struser, strpass);
             * **/
            //_account = "170001";
            //_runner.RequestLogin("170001", "123456");
        }


        public void OnConnect()
        {
            
        }
        public void OnDisconnect()
        {
            
            //authStatus.Image = (System.Drawing.Image)Properties.Resources.login_on;
        }
        public void OnDataFeedConnect()
        {
            
            autoLogin();
        }
        public void OnDataFeedDisconnect()
        {
            
        }

        public void OnLoginSuccess()
        {
            //authStatus.Image = (System.Drawing.Image)Properties.Resources.login_on;
            //string title = Globals.CompanyName + " " + Globals.Version + " 帐号:" + _account;
            //this.Text = title;
        }
        //public void OnPopMessage(QSMessageType type, string title, string msg)
        //{
        //    message.Text = msg;
        //}
        private void btnConfig_Click(object sender, EventArgs e)
        {
            //ConfigForm fm = new ConfigForm();
            //fm.Show();
        }

       
    }
}

//winform default size 612, 354