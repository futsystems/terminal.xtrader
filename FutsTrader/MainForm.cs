using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using TradingLib.API;
using TradingLib.Common;


namespace FutsTrader
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        //交易数据记录核心
        //private TradingTrackerCentre _tradingtrackerCentre = null;
        //数据通讯核心
        //private CoreCentre2 _coreCentre = null;
        //仓位检查策略核心
        //private PositionCheckCentre _poscheckCentre = null;
        //系统默认证券列表
        //Basket _defaultBasket;


        public MainForm(DebugDelegate showinfo)
        {
            try
            {
                ShowInfoHandel = showinfo;
                //Telerik.WinControls.UI.Docking.
                InitializeComponent();
                

                this.Load += new EventHandler(MainForm_Load);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        void Init()
        {
            //ThemeResolutionService.ApplicationThemeName = "Windows8";//"TelerikMetro";
            //ThemeResolutionService.ApplicationThemeName = "Office2010Black";

            ShowInfo("加载交易信息记录组件");
            InitTradingTrackerCentre();//初始化交易信息记录中心

            ShowInfo("加载交易连接组件");
            InitCoreCentre();//初始化QSTradingSession的扩展

            ShowInfo("加载持仓策略管理组件");
            InitPositionCheckCentre();//初始化策略管i中心

            ShowInfo("初始化显示控件");
            InitControl();
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            //ThemeResolutionService.ApplicationThemeName = "Windows8";//"TelerikMetro";
            //ThemeResolutionService.ApplicationThemeName = "Office2010Black";

           
        }

        string _loginuser = "";
        string _loginpass = "";
        /// <summary>
        /// 登入服务器,登入界面调用该函数进行服务器登入
        /// </summary>
        public bool TryLogin(string server, string user, string pass)
        {
            //获得登入窗口传递过来的服务器地址与登入信息
            string[] str = server.Split(':');
            //mAddress = str[0];
            //mPort = Convert.ToInt16(str[1]);
            _loginuser = user;
            _loginpass = pass;

            new Thread(loginproc).Start();
            //在后台线程执行登入任务,登入过程中向登入窗口给出提示信息并根据登入结果更新全局登入状态
            //登入窗口有一个单独的后台线程用于检查登入状态并给出对话框提示
            return true;
        }


        void loginproc()
        {
            ////1.初始化组件
            //ShowInfo("初始化客户端组件");
            //Init();
            ////2.启动连接
            //ShowInfo("尝试与服务端建立连接");
            //new Thread(_coreCentre.Start).Start();

            //string s = ".";
            //DateTime now = DateTime.Now;
            //while (!_coreCentre.Connected && (DateTime.Now - now).TotalSeconds < 5)
            //{
            //    ShowInfo("连接中" + s);
            //    Thread.Sleep(500);
            //    s += ".";
            //}
            //if (!_coreCentre.Connected)
            //{
            //    ShowInfo("连接超时,无法连接到服务器");
            //    Globals.LoginStatus.LoginFaliedReason = "连接超时,无法连接到服务器";
            //    Globals.LoginStatus.IsLoginFailed = true;
            //    return;// false;
            //}
            //else //如果连接服务器成功,则我们请求登入系统
            //{
            //    _coreCentre.ReqLogin(_loginuser, _loginpass);
            //}

            //s = ".";
            //now = DateTime.Now;
            //while (!_coreCentre.GotLoginRep && (DateTime.Now - now).TotalSeconds < 5)
            //{
            //    ShowInfo("登入中" + s);
            //    Thread.Sleep(500);
            //    s += ".";
            //}
            //if (!_coreCentre.GotLoginRep)
            //{
            //    ShowInfo("登入超时,无法登入到服务器");
            //    Globals.LoginStatus.LoginFaliedReason = "登入超时,无法登入到服务器";
            //    Globals.LoginStatus.IsLoginFailed = true;
            //    return;// false;
            //}

            //else
            //{
            //    if (_coreCentre.IsLoggedIn)
            //    {
            //        ShowInfo("登入成功!");
            //        Globals.LoginStatus.LoginFaliedReason = "登入成功";
            //        Globals.LoginStatus.IsLoginSuccess = true;
            //        return;//true;
            //    }
            //    else
            //    {
            //        ShowInfo("登入失败!");
            //        Globals.LoginStatus.LoginFaliedReason = "登入失败!";
            //        Globals.LoginStatus.IsLoginFailed = true;
            //        return;// false;
            //    }
            //}
        }

        public event DebugDelegate ShowInfoHandel;
        //显示加载信息
        void ShowInfo(string msg)
        {
            if (ShowInfoHandel != null)
                ShowInfoHandel(msg);
        }

        void debug(string msg)
        {
            Globals.Debug(msg);
            ctDebug1.GotDebug(msg);
        }


        private void SetPreferences()
        {
            //mTradeWindow.DocumentButtons = null;
            //mTradeWindow.ContextMenu = null;
            //mTradeWindow.ContextMenuStrip = null;
           // mTradeWindow.SizeChanged += new EventHandler(mTradeWindow_SizeChanged);
            //mSendOrderWindow.SizeChanged += new EventHandler(mSendOrderWindow_SizeChanged);
            
        }

        void mSendOrderWindow_SizeChanged(object sender, EventArgs e)
        {
            //debug(mSendOrderWindow.Height.ToString() + " " + mSendOrderWindow.Width.ToString());
        }

        void mTradeWindow_SizeChanged(object sender, EventArgs e)
        {
            //debug(mTradeWindow.Height.ToString() + " " + mTradeWindow.Width.ToString());
        }


        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            TradeImpl t = new TradeImpl("IF1301",2300.2M,20,DateTime.Now);
            t.Account = "168001";
            ctTradeView1.GotFill(t);
            debug("add trade:" + t.ToString());
        }

        private void menu_theme_win8_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeName = "Windows8";
        }

        private void menu_theme_win7_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeName = "Windows7";
        }

        private void menu_theme_metro_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeName = "TelerikMetro";
        }

        private void menu_theme_officesliver_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeName = "Office2010Silver";
        }

        private void menu_theme_officeblack_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeName = "Office2010Black";
        }
    }
}
