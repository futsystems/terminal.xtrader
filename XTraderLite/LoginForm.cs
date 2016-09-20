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
            mStarter = start;
            btnLogin.Enabled = false;

            WireEvent();

            InitBW();
        }


        void WireEvent()
        {
            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            holder.MouseDown += new MouseEventHandler(move_MouseDown);
            holder.MouseUp += new MouseEventHandler(move_MouseUp);
            holder.MouseMove += new MouseEventHandler(move_MouseMove);

            topImage.MouseDown += new MouseEventHandler(move_MouseDown);
            topImage.MouseUp += new MouseEventHandler(move_MouseUp);
            topImage.MouseMove += new MouseEventHandler(move_MouseMove);

            MDService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            MDService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);
            MDService.EventHub.OnLoginEvent += new Action<MDLoginResponse>(EventHub_OnLoginEvent);
            MDService.EventHub.OnInitializedEvent += new Action(EventHub_OnInitializedEvent);
            MDService.EventHub.OnInitializeStatusEvent += new Action<string>(EventHub_OnInitializeStatusEvent);
            
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
            //string address = serverlist.SelectedValue.ToString();

            //登入过程开始
            _connectstart = true;
            _connecttime = DateTime.Now;
            //
            //logger.Info("client try to connec to:" + address + " port:" + port.ToString());
            MDService.InitDataAPI("TradingLib.MarketData.IMarketDataAPI");
            //MDService.DataAPI.Connect(new string[] { "210.21.198.182" }, 7709);
            MDService.DataAPI.Connect(new string[] { "218.85.137.40" }, 7709);

        }

        bool _loginstart = false;
        DateTime _logintime = DateTime.Now;

        void EventHub_OnConnectedEvent()
        {
            _connected = true;
            ShowStatus("服务端连接成功,请求登入");

            _loginstart = true;
            _logintime = DateTime.Now;

            MDService.DataAPI.Login("", "");
            
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
            //SaveLoginConfig();
            new Thread(delegate()
            {
                Connect();
            }).Start();
            this.btnLogin.Enabled = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }



    }
}
