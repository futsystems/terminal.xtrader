using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using TradingLib.KryptonControl;

namespace StockTrader
{
    public partial class LoginForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        ILog logger = LogManager.GetLogger("LoginForm");
        Starter mStart;

        List<ServerCfg> servers = null;
        int port = 5570;

        public LoginForm(Starter start)
        {
            try
            {
                //允许线程间调用控件属性 否则无法本地调试
                CheckForIllegalCrossThreadCalls = false;
                logger.Info("LoginForm created");
                InitializeComponent();
                mStart = start;
                btnLogin.Enabled = false;

                //从配置文件加载加载服务器参数
                LoadServerCfg();
                TraderHelper.AdapterToIDataSource(serverlist).BindDataSource(GenServerList());
                account.Text = Properties.Settings.Default.account;
                password.Text = Properties.Settings.Default.pass;
                savepassword.Checked = Properties.Settings.Default.savepass;

                this.Load += new EventHandler(LoginForm_Load);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        void LoginForm_Load(object sender, EventArgs e)
        {


            InitBW();

            WireEvent();

            ShowLoginStatus("登入窗口初始化完毕");
        }



        /// <summary>
        /// 绑定事件
        /// </summary>
        void WireEvent()
        {
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.btnExit.Click += new EventHandler(btnExit_Click);
            this.FormClosing += new FormClosingEventHandler(LoginForm_FormClosing);

            CoreService.EventCore.OnConnectedEvent += new VoidDelegate(EventCore_OnConnectedEvent);
            CoreService.EventCore.OnDisconnectedEvent += new VoidDelegate(EventCore_OnDisconnectedEvent);
            CoreService.EventCore.OnLoginEvent += new Action<LoginResponse>(EventCore_OnLoginEvent);
            CoreService.EventCore.OnInitializeStatusEvent += new Action<string>(EventCore_OnInitializeStatusEvent);
            CoreService.EventCore.OnInitializedEvent += new VoidDelegate(EventCore_OnInitializedEvent);

            this.AcceptButton = this.btnLogin;

        }


        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveLoginConfig();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 登入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            logger.Info("登入-------------------");
            SaveLoginConfig();
            new Thread(delegate()
            {
                Connect();
            }).Start();
            this.btnLogin.Enabled = false;
        }


        #region 连接服务端
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
            string address = serverlist.SelectedValue.ToString();

            //登入过程开始
            _connectstart = true;
            _connecttime = DateTime.Now;
            //
            logger.Info("client try to connec to:" + address + " port:" + port.ToString());
            CoreService.InitClient(address, port);
            CoreService.TLClient.Start();
        }

        bool _loginstart = false;
        DateTime _logintime = DateTime.Now;
        void EventCore_OnConnectedEvent()
        {
            _connected = true;
            ShowLoginStatus("服务端连接成功,请求登入");

            _loginstart = true;
            _logintime = DateTime.Now;

            CoreService.TLClient.ReqLogin(account.Text, password.Text);
        }

        bool _qrybasicinfo = false;
        DateTime qrybasicinfoTime = DateTime.Now;
        /// <summary>
        /// 响应登入 请求查询基础信息(底层 无需人工操作)
        /// </summary>
        /// <param name="response"></param>
        void EventCore_OnLoginEvent(LoginResponse response)
        {
            _gotloginrep = true;
            if (response.RspInfo.ErrorID == 0)
            {
                ShowLoginStatus("登入成功");
                _loggedin = true;
                _qrybasicinfo = true;
                qrybasicinfoTime = DateTime.Now;
            }
            else
            {
                ShowLoginStatus("登入失败:" + response.RspInfo.ErrorMessage);
                _loggedin = false;
                Reset();
            }
        }

        bool initsuccess = false;

        /// <summary>
        /// 响应初始化完成事件 用于检查系统是否初始化正常
        /// </summary>
        void EventCore_OnInitializedEvent()
        {
            //if (CoreService.SiteInfo.Manager == null)
            //{
            //    ShowLoginStatus("柜员数据获取异常,请重新登入!");
            //}
            //else
            //执行初始化完毕后的操作 然后标注initsuccess为成功
            {
                initsuccess = true;
            }
        }

        void EventCore_OnInitializeStatusEvent(string obj)
        {
            ShowLoginStatus(obj);
        }


        void EventCore_OnDisconnectedEvent()
        {
            _connected = false;
            _loggedin = false;
        }


        #endregion



        #region 辅助函数
        public void ShowLoginStatus(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowLoginStatus), new object[] { msg });
            }
            else
            {
                lbLoginStatus.Text = msg;
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

                CoreService.TLClient.Stop();
                this.btnLogin.Enabled = true;
                //lbLoginStatus.Text = "请登入";
            }).Start();
        }

        public void EnableLogin()
        {
            if (InvokeRequired)
            {
                Invoke(new VoidDelegate(EnableLogin), new object[] { });
            }
            else
            {
                btnLogin.Enabled = true;
            }
        }


        void LoadServerCfg()
        {
            //加载配置文件
            ConfigFile _config = ConfigFile.GetConfigFile("server.cfg");

            servers = new List<ServerCfg>();
            string[] iplist = _config["SERVERS"].AsString().Split(',');
            string[] namelist = _config["NAMES"].AsString().Split(',');

            //port = _config["PORT"].AsInt();
            for (int i = 0; i < iplist.Length; i++)
            {
                string name = i < namelist.Length ? namelist[i] : iplist[i];
                ServerCfg cfg = new ServerCfg(iplist[i], name);
                servers.Add(cfg);
            }
            //Globals.CashIn = _config["CASHIN"].AsString();
            //Globals.CashOut = _config["CASHOUT"].AsString();

        }

        void SaveLoginConfig()
        {
            bool save = savepassword.Checked;
            Properties.Settings.Default.savepass = save;
            if (save)
            {
                Properties.Settings.Default.pass = password.Text;
            }
            else
            {
                Properties.Settings.Default.pass = "";
            }
            Properties.Settings.Default.savepass = save;
            Properties.Settings.Default.account = account.Text;
            Properties.Settings.Default.Save();
        }
        #endregion

        ArrayList GenServerList()
        {
            ArrayList list = new ArrayList();

            foreach (var srv in servers)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = srv.Name;
                vo.Value = srv.IPAddress;
                list.Add(vo);
            }
            return list;
        }




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
                if (CoreService.Initialized && !mStart.IsSplashScreenClosed && initsuccess)
                {
                    logger.Info("关闭登入窗体");
                    mStart.CloseSplashScreen();
                }

                if (_connectstart && (DateTime.Now - _connecttime).TotalSeconds > 5 && (!_connected))
                {
                    logger.Info("连接服务器超过5秒没有连接事件回报");
                    ShowLoginStatus("连接超时,无法连接到服务器");
                    Reset();
                    this.EnableLogin();
                }

                //5秒内没有登入回报
                if (_loginstart && (DateTime.Now - _logintime).TotalSeconds > 5 && (!_gotloginrep))
                {
                    logger.Info("登入服务器超过5秒没有连接事件回报");
                    ShowLoginStatus("登入超时,无法登入到服务器");
                    Reset();
                    this.EnableLogin();
                }
                if (_qrybasicinfo && (DateTime.Now - qrybasicinfoTime).TotalSeconds > 10 && (!initsuccess))
                {
                    logger.Info("获取基础数据超过10秒没有成功");
                    ShowLoginStatus("获取基础数据失败,请重新登入");
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
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_isMouseDown = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        private void imageheader_MouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        private void imageheader_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
        }

        private void imageheader_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }
        #endregion
    }
}
