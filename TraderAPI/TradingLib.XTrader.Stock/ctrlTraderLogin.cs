using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Stock
{
    public partial class ctrlTraderLogin : UserControl
    {

        public event Action ExitTrader;

        public event Action EntryTrader;

        ILog logger = LogManager.GetLogger("ctrlTraderLogin");

        public ctrlTraderLogin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SizeChanged += new EventHandler(ctrlTraderLogin_SizeChanged);
            //this.Paint += new PaintEventHandler(ctrlTraderLogin_Paint);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //this.Update();
            //this.Resize += new EventHandler(ctrlTraderLogin_Resize);

            InitControl();

            WireEvent();
        }

        void WireEvent()
        {
            this.Load +=new EventHandler(ctrlTraderLogin_Load);

            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnExit.Click += new EventHandler(btnExit_Click);

            CoreService.EventCore.OnConnectedEvent += new VoidDelegate(EventCore_OnConnectedEvent);
            CoreService.EventCore.OnDisconnectedEvent += new VoidDelegate(EventCore_OnDisconnectedEvent);
            CoreService.EventCore.OnLoginEvent += new Action<LoginResponse>(EventCore_OnLoginEvent);
            CoreService.EventCore.OnInitializeStatusEvent += new Action<string>(EventCore_OnInitializeStatusEvent);
            CoreService.EventCore.OnInitializedEvent += new VoidDelegate(EventCore_OnInitializedEvent);
            
            //this.AcceptButton = this.btnLogin;
        }


        void ctrlTraderLogin_Load(object sender, EventArgs e)
        {
            //加载交易服务器地址
            foreach (var v in (new ServerConfig("broker.cfg")).GetServerNodes())
            {
                serverList.Items.Add(v);
            }
            if (serverList.Items.Count > 0)
            {
                serverList.SelectedIndex = 0;
            }

            //加载席位
            foreach (var v in (new ServerConfig("seat.cfg")).GetServerNodes())
            {
                seat.Items.Add(v);
            }
            if (seat.Items.Count > 0)
            {
                seat.SelectedIndex = 0;
            }

            InitBW();
        }


        void InitControl()
        {

            encrypt.SelectedIndex = 0;
        }



        void btnExit_Click(object sender, EventArgs e)
        {
            new Thread(delegate()
            {
                StopTrader();
            }).Start();
        }


        /// <summary>
        /// 停止交易系统
        /// </summary>
        public void StopTrader()
        {
            _bkgo = false;

            Reset();

            CoreService.Reset();

            if (ExitTrader != null)
            {
                ExitTrader();
            }
        }

        //加载交易插件并执行初始化过程
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

        ctrlStockTrader CreateTraderControl()
        {
            if (InvokeRequired)
            {
                logger.Info("trader control created invokerequired");
                return Invoke(new Func<ctrlStockTrader>(CreateTraderControl), new object[] { }) as ctrlStockTrader;
;
            }
            else
            {
                ctrlStockTrader tmp = new ctrlStockTrader();
                ShowStatus("交易插件初始化完毕");
                return tmp;
            }
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
            if (_bkgo == false)
            {
                InitBW();
            }
            ServerNode node = serverList.SelectedItem as ServerNode;
            if (node == null)
            {
                ShowStatus("选择可用交易节点");
            }

            //登入过程开始
            _connectstart = true;
            _connecttime = DateTime.Now;
            //
            logger.Info("client try to connec to:" + node.Address + " port:" + node.Port.ToString());
            CoreService.InitClient(node.Address, node.Port);
            CoreService.TLClient.Start();
        }

        bool _loginstart = false;
        DateTime _logintime = DateTime.Now;
        void EventCore_OnConnectedEvent()
        {
            _connected = true;
            ShowStatus("服务端连接成功,请求登入");

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
                ShowStatus("登入成功");
                _loggedin = true;
                _qrybasicinfo = true;
                qrybasicinfoTime = DateTime.Now;
            }
            else
            {
                ShowStatus("登入失败:" + response.RspInfo.ErrorMessage);
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
            initsuccess = true;
        }

        void EventCore_OnInitializeStatusEvent(string obj)
        {
            ShowStatus(obj);
        }


        void EventCore_OnDisconnectedEvent()
        {
            _connected = false;
            _loggedin = false;
        }


        #endregion



        public void ShowStatus(string msg)
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
            ShowStatus("停止交易系统...");
            _connectstart = false;
            _connected = false;

            _loginstart = false;
            _gotloginrep = false;
            _loggedin = false;

            _qrybasicinfo = false;
            initsuccess = false;
            if (CoreService.TLClient != null)
            {
                CoreService.TLClient.Stop();
            }
            this.btnLogin.Enabled = true;

            _msg.Text = "电信、联通用户请分别登入电信、联通站点";
            //new Thread(delegate()
            //{
            //    _connectstart = false;
            //    _connected = false;

            //    _loginstart = false;
            //    _gotloginrep = false;
            //    _loggedin = false;

            //    _qrybasicinfo = false;
            //    initsuccess = false;

            //    CoreService.TLClient.Stop();
            //    this.btnLogin.Enabled = true;
            //    //lbLoginStatus.Text = "请登入";
            //}).Start();
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



        #region 线程内处理消息并触发显示
        /// <summary>
        /// 登入窗口建立线程循环检查全局变量
        /// 登入窗口点击按钮,启动后台登入线程,后台登入线程动态的更新登入窗口的界面,同时将登入的消息实时的写入到系统中
        /// 如果登入窗口的线程检测到该信息,则执行弹窗提醒或者是进入主界面
        /// </summary>
        private System.ComponentModel.BackgroundWorker bg;
        bool _bkgo = false;
        //private PopMessage pmsg = new PopMessage();
        private void bgDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            logger.Info("TraderLogin BW Started");
            while (_bkgo)
            {
                //数据初始化完毕
                if (CoreService.Initialized &&initsuccess)
                {
                    logger.Info("关闭登入窗体");
                    if (EntryTrader != null)
                    {
                        ShowStatus("初始化交易界面");
                        EntryTrader();
                        _bkgo = false;
                    }
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
                if (_qrybasicinfo && (DateTime.Now - qrybasicinfoTime).TotalSeconds > 10 && (!initsuccess))
                {
                    logger.Info("获取基础数据超过10秒没有成功");
                    ShowStatus("获取基础数据失败,请重新登入");
                    Reset();
                    this.EnableLogin();
                }
                Thread.Sleep(100);
            }
            logger.Info("TraderLogin BW Stopped");
        }

        //启动后台工作进程 用于检查信息并调用弹出窗口
        private void InitBW()
        {
            _bkgo = true;
            bg = new System.ComponentModel.BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new System.ComponentModel.DoWorkEventHandler(bgDoWork);
            //bg.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bg_ProgressChanged);
            bg.RunWorkerAsync();
        }
        #endregion



        void ctrlTraderLogin_SizeChanged(object sender, EventArgs e)
        {
            holder.Location = new Point((this.Width - holder.Width) / 2, holder.Location.Y);
        }
    }
}
