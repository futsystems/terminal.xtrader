using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.MarketData;
using TradingLib.XTrader.Control;

namespace XTraderLite
{
    public partial class LoginForm : Form
    {
        ILog logger = LogManager.GetLogger("LoginForm");
        Starter mStarter = null;
        public LoginForm(Starter start)
        {
            //允许线程间调用控件属性 否则无法本地调试
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            InitContrl();

            if (Global.IsXGJStyle)
            {
                XGJLogin();
            }
            else
            {
                ClassicLogin();
            }

            mStarter = start;
            btnLogin.Enabled = false;
            _msg.Visible = false;
            WireEvent();

            InitBW();

            
        }

        void ClassicLogin()
        {
            this.Height = 372;
            this.Width = 562;
            holder.Height = 370;
            holder.Width = 560;
            panel_Classic.Dock = DockStyle.Fill;
            panel_XGJ.Visible = false;

        }

        void XGJLogin()
        {
            this.Height = 412;
            this.Width = 732;
            holder.Height = 410;
            holder.Width = 730;
            panel_XGJ.Dock = DockStyle.Fill;
            panel_Classic.Visible = false;
            this.Icon = Properties.Resources.xgj;
            this.Text = "信管家";

        }

        void InitContrl()
        {
            foreach (var v in (new ServerConfig("market.cfg")).GetServerNodes())
            {
                cbServer.Items.Add(v);
            }
            if (cbServer.Items.Count > 0)
            {
                cbServer.SelectedIndex = 0;
            }

            this.AcceptButton = btnLogin;

            updateBasic = Properties.Settings.Default.updateBasic;
            saveAccout = Properties.Settings.Default.saveAccount;
            cbUpdateBasic.Image = (updateBasic ? Properties.Resources.cb_yes : Properties.Resources.cb_no);
            cbSaveAccount.Image = (saveAccout ? Properties.Resources.cb_yes : Properties.Resources.cb_no);

            username2.Text = Properties.Settings.Default.Account;
            password2.Text = Properties.Settings.Default.Pass;


        }

        void WireEvent()
        {
            this.Load += new EventHandler(LoginForm_Load);
            this.Activated += new EventHandler(LoginForm_Activated);
            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnLogin2.Click += new EventHandler(btnLogin_Click);


            btnCancel.Click += new EventHandler(btnCancel_Click);

            holder.MouseDown += new MouseEventHandler(move_MouseDown);
            holder.MouseUp += new MouseEventHandler(move_MouseUp);
            holder.MouseMove += new MouseEventHandler(move_MouseMove);

            topImage.MouseDown += new MouseEventHandler(move_MouseDown);
            topImage.MouseUp += new MouseEventHandler(move_MouseUp);
            topImage.MouseMove += new MouseEventHandler(move_MouseMove);

            panel_XGJ.MouseDown += new MouseEventHandler(move_MouseDown);
            panel_XGJ.MouseUp += new MouseEventHandler(move_MouseUp);
            panel_XGJ.MouseMove += new MouseEventHandler(move_MouseMove);

            MDService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            MDService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);
            MDService.EventHub.OnLoginEvent += new Action<MDLoginResponse>(EventHub_OnLoginEvent);
            MDService.EventHub.OnInitializedEvent += new Action(EventHub_OnInitializedEvent);
            MDService.EventHub.OnInitializeStatusEvent += new Action<string>(EventHub_OnInitializeStatusEvent);

            btnLogin2.MouseEnter += new EventHandler(btnLogin2_MouseEnter);
            btnLogin2.MouseLeave += new EventHandler(btnLogin2_MouseLeave);
            btnLogin2.MouseDown += new MouseEventHandler(btnLogin2_MouseDown);
            btnLogin2.MouseUp += new MouseEventHandler(btnLogin2_MouseUp);

            btnMin2.Click += new EventHandler(btnMin2_Click);
            btnClose2.Click += new EventHandler(btnCancel_Click);
            cbSaveAccount.Click += new EventHandler(cbSaveAccount_Click);
            cbUpdateBasic.Click += new EventHandler(cbUpdateBasic_Click);
            
        }

        bool updateBasic = true;
        void cbUpdateBasic_Click(object sender, EventArgs e)
        {
            updateBasic = !updateBasic;
            cbUpdateBasic.Image = (updateBasic ? Properties.Resources.cb_yes : Properties.Resources.cb_no);
        }

        bool saveAccout = true;
        void cbSaveAccount_Click(object sender, EventArgs e)
        {
            saveAccout = !saveAccout;
            cbSaveAccount.Image = (saveAccout ? Properties.Resources.cb_yes : Properties.Resources.cb_no);
        }


        string GetAccount()
        {
            if (Global.IsXGJStyle) return username2.Text;
            return username.Text;
        }

        string GetPass()
        {
            if (Global.IsXGJStyle) return password2.Text;
            return password.Text;
        }

        void SavePropertiesConfig()
        {

            Properties.Settings.Default.saveAccount = saveAccout;
            Properties.Settings.Default.updateBasic = updateBasic;
            if (saveAccout)
            {
                Properties.Settings.Default.Account = GetAccount();
                Properties.Settings.Default.Pass = GetPass();
            }
            else
            {
                Properties.Settings.Default.Account = string.Empty;
                Properties.Settings.Default.Pass = string.Empty;
            }

            Properties.Settings.Default.Save();
        }


        void btnMin2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void btnLogin2_MouseUp(object sender, MouseEventArgs e)
        {
            btnLogin2.BackgroundImage = Properties.Resources.login_normal;
        }

        void btnLogin2_MouseDown(object sender, MouseEventArgs e)
        {
            btnLogin2.BackgroundImage = Properties.Resources.login_click;
        }

        void btnLogin2_MouseLeave(object sender, EventArgs e)
        {
            btnLogin2.BackgroundImage = Properties.Resources.login_normal ;
        }

        void btnLogin2_MouseEnter(object sender, EventArgs e)
        {
            btnLogin2.BackgroundImage = Properties.Resources.login_over;
        }



        void LoginForm_Activated(object sender, EventArgs e)
        {
            username.Focus();
            username.SelectionLength = 0;
            username.SelectionStart = username.Text.Length;
        }

        void LoginForm_Load(object sender, EventArgs e)
        {
            
        }



        

        void EventHub_OnInitializeStatusEvent(string obj)
        {
            ShowStatus(obj);
        }

        void EventHub_OnDisconnectedEvent()
        {
            
        }


        #region 登入过程
        bool _connected = false;
        bool _loggedin = false;
        bool _gotloginrep = false;

        bool _connectstart = false;
        DateTime _connecttime = DateTime.Now;
        /// <summary>
        /// 执行连接请求
        /// 连接动作
        /// </summary>
        void Connect()
        {
            //登入过程开始
            _connectstart = true;
            _connecttime = DateTime.Now;
            //从配置文件设定的dll初始化行情插件
            string dllname = new ConfigFileBase("apimarket.cfg").GetFirstLine();
            MDService.InitDataAPI(dllname);

            if (MDService.DataAPI == null)
            {
                MessageBox.Show("行情插件加载异常");
                Reset();
                return;
            }

            //ServerNode node = cbServer.SelectedItem as ServerNode;
            //if (node != null)
            //{
            //    MDService.DataAPI.Connect(new string[] { node.Address }, node.Port);
            //}

            List<string> serverList = new List<string>();
            int port = 0;
            foreach (var v in (new ServerConfig("market.cfg")).GetServerNodes())
            {
                if(port ==0) port = v.Port;
                serverList.Add(v.Address);
            }

            MDService.DataAPI.Connect(serverList.ToArray(), port);

        }

        bool _loginstart = false;
        DateTime _logintime = DateTime.Now;

        void EventHub_OnConnectedEvent()
        {
            _connected = true;
            ShowStatus("服务端连接成功,请求登入");

            _loginstart = true;
            _logintime = DateTime.Now;

            //执行登入请求
            MDService.DataAPI.Login(GetAccount(),GetPass());
        }

        bool _qrybasicinfo = false;
        DateTime qrybasicinfoTime = DateTime.Now;

        void EventHub_OnLoginEvent(MDLoginResponse response)
        {
            _gotloginrep = true;
            if (response.LoginSuccess)
            {
                ShowStatus("登入成功");
                _loggedin = true;
                _qrybasicinfo = true;
                qrybasicinfoTime = DateTime.Now;

                
            }
            else
            {
                ShowStatus(string.Format("登入失败:{0} Code:{1}", response.ErrorMessage, response.ErrorCode));
                _loggedin = false;
                Reset();
            }
        }

        bool initsuccess = false;
        void EventHub_OnInitializedEvent()
        {
            initsuccess = true;
        }

        #endregion



        #region 线程内处理消息并触发显示
        /// <summary>
        /// 登入窗口建立线程循环检查全局变量
        /// 登入窗口点击按钮,启动后台登入线程,后台登入线程动态的更新登入窗口的界面,同时将登入的消息实时的写入到系统中
        /// 如果登入窗口的线程检测到该信息,则执行弹窗提醒或者是进入主界面
        /// </summary>
        private System.ComponentModel.BackgroundWorker bg;
        //private PopMessage pmsg = new PopMessage();
        private void bgDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                //数据初始化完毕
                if (MDService.Initialized && !mStarter.IsSplashScreenClosed && initsuccess)
                {
                    logger.Info("关闭登入窗体");
                    mStarter.CloseSplashScreen();
                }

                if (_connectstart && (DateTime.Now - _connecttime).TotalSeconds > 5 && (!_connected))
                {
                    logger.Info("连接服务器超过5秒没有连接事件回报");
                    ShowStatus("连接超时,无法连接到服务器");
                    Reset();
                    this.EnableLogin();
                }

                //5秒内没有登入回报
                if (_loginstart && (DateTime.Now - _logintime).TotalSeconds > 5 && (!_gotloginrep))
                {
                    logger.Info("登入服务器超过5秒没有连接事件回报");
                    ShowStatus("登入超时,无法登入到服务器");
                    Reset();
                    this.EnableLogin();
                }
                if (_qrybasicinfo && (DateTime.Now - qrybasicinfoTime).TotalSeconds > 30 && (!initsuccess))
                {
                    logger.Info("获取基础数据超过10秒没有成功");
                    ShowStatus("获取基础数据失败,请重新登入");
                    Reset();
                    this.EnableLogin();
                }
                Thread.Sleep(100);
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
        //显示服务端返回过来的信息窗口
        //private void bg_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{
        //    //MessageBox.Show(Globals.LoginStatus.InitMessage);
        //    //fmConfirm.Show(Globals.LoginStatus.InitMessage);
        //}
        #endregion



        #region 移动窗体
        private bool m_isMouseDown = false;
        private Point m_mousePos = new Point();

        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        private void move_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
        }

        private void move_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }
        #endregion

        #region 辅助函数
        void ShowStatus(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowStatus), new object[] { msg });
            }
            else
            {
                _msg.Text = msg;
                __msg2.Text = msg;
            }
        }
        /// <summary>
        /// 重置异常连接并恢复界面状态
        /// </summary>
        void Reset()
        {
            new Thread(delegate()
            {
                _connectstart = false;
                _connected = false;

                _loginstart = false;
                _gotloginrep = false;
                _loggedin = false;

                _qrybasicinfo = false;
                initsuccess = false;

                //CoreService.TLClient.Stop();
                this.btnLogin.Enabled = true;
                //lbLoginStatus.Text = "请登入";
            }).Start();
        }

        public void EnableLogin()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableLogin), new object[] { });
            }
            else
            {
                btnLogin.Enabled = true;
            }
        }
        #endregion



        void btnCancel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            logger.Info("登入-------------------");
            _msg.Visible = true;
            SavePropertiesConfig();
            System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(o => Connect()));
            this.btnLogin.Enabled = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }



    }
}
