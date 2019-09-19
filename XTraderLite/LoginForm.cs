using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
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
        ConfigFile _cfgfile;
        
        public LoginForm(Starter start)
        {
            //允许线程间调用控件属性 否则无法本地调试
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            InitContrl();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);


            _cfgfile = ConfigFile.GetConfigFile("Setting.cfg");
            Global.ClassicLogin = _cfgfile["ClassicLogin"].AsBool();
            Global.HeadTitle = _cfgfile["HeadTitle"].AsString();
            Global.ShowCorner = _cfgfile["ShowCorner"].AsBool();
            Global.TaskBarTitle = _cfgfile["TaskBarTitle"].AsString();
            Global.BrandName = _cfgfile["BrandName"].AsString();
            Global.BrandCompany = _cfgfile["BrandCompany"].AsString();
            Global.RiskPrompt = _cfgfile["RiskPrompt"].AsBool();
            Global.NewsUrl = _cfgfile["NewsUrl"].AsString();
            Global.NewsBtn = _cfgfile["NewsBtn"].AsInt();

            Global.DataFarmGroup = _cfgfile["DataFarmGroup"].AsInt();
            Global.DefaultMarketUser = _cfgfile["DefaultMarketUser"].AsString();
            Global.DefaultBlock = _cfgfile["DefaultBlock"].AsString();
            Global.ShowMDIP = _cfgfile["ShowMDIP"].AsBool();
            Global.XGJCTRL_R = _cfgfile["LOGINXGJCTRLCOLOR_R"].AsInt();
            Global.XGJCTRL_B = _cfgfile["LOGINXGJCTRLCOLOR_B"].AsInt();
            Global.XGJCTRL_G = _cfgfile["LOGINXGJCTRLCOLOR_G"].AsInt();




            UIConstant.QuoteViewStdSumbolHidden = _cfgfile["QuoteViewStdSumbolHidden"].AsBool();//报价列表 标准合约隐藏
            UIConstant.QuoteSymbolNameStyle = _cfgfile["QuoteSymbolNameStyle"].AsInt();//报价列表 合约名类型

            UIConstant.BoardSymbolTitleStyle = _cfgfile["BoardSymbolTitleStyle"].AsInt();//盘口明细顶部合约Title类被
            UIConstant.BoardSymbolNameStyle = _cfgfile["BoardSymbolNameStyle"].AsInt();
            


            var blockStr = _cfgfile["Block"].AsString();
            if(!string.IsNullOrEmpty(blockStr))
            {
                foreach(var block in blockStr.Split(','))
                {
                    Global.QuoteBlockList.Add(block);
                }
            }

            TradingLib.XTrader.Future.Constants.SymbolNameStyle = _cfgfile["BrokerSymbolNameStyle"].AsInt();
            TradingLib.XTrader.Future.Constants.SymbolTitleStyle = _cfgfile["BrokerSymbolTitleStyle"].AsInt();
            TradingLib.XTrader.Future.Constants.PageBankStyle = _cfgfile["PageBankStyle"].AsInt();

            TradingLib.XTrader.Future.Constants.CashURL1 = _cfgfile["CashURL1"].AsString();
            TradingLib.XTrader.Future.Constants.CashURL2 = _cfgfile["CashURL2"].AsString();
            TradingLib.XTrader.Future.Constants.QRDescription = _cfgfile["QRDescription"].AsString();
            TradingLib.XTrader.Future.Constants.EnableConfigBank = _cfgfile["EnableConfigBank"].AsBool();
            TradingLib.XTrader.Future.Constants.BranName = Global.BrandName;

            TradingLib.XTrader.Future.Constants.CompanyTitle = _cfgfile["CompanyTitle"].AsString();
            TradingLib.XTrader.Future.Constants.CompanyUrl = _cfgfile["CompanyUrl"].AsString();


            slogen.Text = _cfgfile["Slogen"].AsString();

            
            //判断是否有UNIT编号
            if (_cfgfile.ContainsKey("UNIT"))
            {
                Global.DeployID = _cfgfile["UNIT"].AsString();
            }
            else
            {
                MessageBox.Show("配置文件缺少部署编号");
            }

            if (!string.IsNullOrEmpty(_cfgfile["APPServer"].AsString()))
            { 

                Global.AppServer =  _cfgfile["APPServer"].AsString();
            }
            else
            {
                MessageBox.Show("配置文件缺少APPServer字段");
            }

            if (!string.IsNullOrEmpty(_cfgfile["APPPort"].AsString()))
            {

                Global.AppPort = _cfgfile["APPPort"].AsInt();
            }

            TradingLib.XTrader.Future.Constants.AppServer = Global.AppServer;
            


            //设置样式
            if (!Global.ClassicLogin)
            {
                XGJLogin();
            }
            else
            {
                //设置自定义登入界面
                if (File.Exists("Config/login.png"))
                {
                    topImage.Image = Image.FromFile("Config/login.png");

                }
                else
                {
                    //设置登入界面图片
                    topImage.Image = Properties.Resources.login;
                }
                ClassicLogin();
            }

            this.Text = string.Format("登入{0}", Global.TaskBarTitle);

            //如果设定了默认用户名 则不允许修改
            if (!string.IsNullOrEmpty(Global.DefaultMarketUser))
            {
                username.Text = Global.DefaultMarketUser;
                password.Text = "123456";
                username.Enabled = false;
                password.Enabled = false;

                username2.Text = Global.DefaultMarketUser;
                password2.Text = "123456";
                username2.Enabled = false;
                password2.Enabled = false;
                cbSaveAccount.Enabled = false;
                cbUpdateBasic.Enabled = false;
            }

            mStarter = start;
            btnLogin.Enabled = false;
            btnLogin2.Enabled = false;
            //_msg.Visible = false;

            panel_risk.Visible = Global.RiskPrompt;

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
            topImage.Height = 230;
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

            if (File.Exists("Config/login_xgj.png"))
            {
                panel_XGJ.BackgroundImage = Image.FromFile("Config/login_xgj.png");
                var color = Color.FromArgb(Global.XGJCTRL_R, Global.XGJCTRL_G, Global.XGJCTRL_B);
                username2.BackColor = color;
                password2.BackColor = color;
                cbSaveAccount.BackColor = color;
                cbUpdateBasic.BackColor = color;

            }
            
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

            //username2.Text = Properties.Settings.Default.Account;
            //password2.Text = Properties.Settings.Default.Pass;
            //password2.Text = "12345678";


        }

        void WireEvent()
        {
            this.Load += new EventHandler(LoginForm_Load);
            this.Activated += new EventHandler(LoginForm_Activated);
            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnLogin2.Click += new EventHandler(btnLogin_Click);
            btnLogin2.Paint += new PaintEventHandler(btnLogin2_Paint);


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


            readRisk.Click += new EventHandler(readRisk_Click);
        }

        void readRisk_Click(object sender, EventArgs e)
        {
            frmRisk fm = new frmRisk();
            fm.ShowDialog();
            fm.Close();
        }

        void btnLogin2_Paint(object sender, PaintEventArgs e)
        {
            //throw new NotImplementedException();
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
            this.lbFocus.Focus();//避免按钮获得焦点 有黑色边框
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
            try
            {
                //登入过程开始
                _connectstart = true;
                _connecttime = DateTime.Now;
                
                MDService.InitDataAPI(new DataAPI.Futs.FutsDataAPI());

                if (MDService.DataAPI == null)
                {
                    MessageBox.Show("行情插件加载异常");
                    Reset();
                    return;
                }

                if (string.IsNullOrEmpty(_cfgfile["MDServer"].AsString()) || string.IsNullOrEmpty(_cfgfile["MDPort"].AsString()))
                {
                    MDService.DataAPI.Connect(new string[] { Global.Config.HistServerConfig.Address }, Global.Config.HistServerConfig.Port);
                }
                else
                {

                    MDService.DataAPI.Connect(new string[] { _cfgfile["MDServer"].AsString() }, _cfgfile["MDPort"].AsInt());
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("程序异常,联系程序开发人员:" + ex.ToString());
            }

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
                this.btnLogin2.Enabled = true;
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
                btnLogin2.Enabled = true;
            }
        }
        #endregion



        void btnCancel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            if (Global.RiskPrompt)
            {
                if (!riskConfirm.Checked)
                {
                    MessageBox.Show("请阅读并确认风险提示");
                    return;
                }
            }
            this.btnLogin.Enabled = false;
            this.btnLogin2.Enabled = false;

           //第二代配置服务器
           InvokeQryDeployConfig();
            
        }


        

       

        static int ToTLDate(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }


        #region Ver2 行情服务器分配

        void InvokeQryDeployConfig()
        {
            Action del = new Action(() => {
                string url = string.Format("http://{0}:{1}/config/desktop/{2}/", Global.AppServer,Global.AppPort, Global.DeployID);
                Global.Config = NetHelper.GetHttpJsonResponse<DeployConfig>(url);
            });
            del.BeginInvoke(QryDeployConfigCallback, null);
        }

        void QryDeployConfigCallback(IAsyncResult result)
        {
            Action proc = ((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate as Action;
            proc.EndInvoke(result);
            //查询配置信息结束

            //组设定与部署设定均不存在
            if (Global.Config == null)
            {
                ShowStatus("获取配置信息异常,请检查网络并重启交易端");
                return;
            }

            if (Global.Config.IsExist == false)
            {
                ShowStatus("部署环境不存在");
                return;
            }

            if (Global.Config.IsAvabile == false)
            {
                ShowStatus("部署环境不可用");
                return;
            }

            bool set_server = (!string.IsNullOrEmpty(_cfgfile["MDServer"].AsString()) && !string.IsNullOrEmpty(_cfgfile["MDPort"].AsString()));

            if (Global.Config.HistServerConfig == null && (!set_server))
            {
                ShowStatus("无可用行情服务器");
                return;
            }

            //本地调试行情服务器
            //Global.Config.HistServerConfig.Address = "127.0.0.1";


            ShowStatus("配置信息加载完毕");

            if (Global.Config.HistServerConfig != null)
            {
                Properties.Settings.Default.UpdateTime = ToTLDate(DateTime.Now);
                Properties.Settings.Default.MarketAddress = Global.Config.HistServerConfig.Address;
                Properties.Settings.Default.MarketPort = Global.Config.HistServerConfig.Port;
                Properties.Settings.Default.Save();
            }

            logger.Info("登入-------------------");
            _msg.Visible = true;
            SavePropertiesConfig();
            System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(o => Connect()));
            
        }

        #endregion


    }
}
