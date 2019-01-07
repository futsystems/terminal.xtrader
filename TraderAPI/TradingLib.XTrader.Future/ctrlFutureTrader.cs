using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TradingLib.MarketData;
using System.Drawing.Drawing2D;
using System.Reflection;
using TradingLib.TraderCore;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.XTrader.Future
{
    public partial class ctrlFutureTrader : UserControl
    {
        public event Action<EnumTraderWindowOperation> TradingBoxOpeartion = delegate { };

        /// <summary>
        /// 锁定交易窗口
        /// </summary>
        public event Action LockTradingBox = delegate { };

        ILog logger = LogManager.GetLogger("ctrlFutureTrader");

        public ctrlFutureTrader()
        {
            InitializeComponent();

            InitPage();

            InitMenu();

            WireEvent();

            InitMessageBW();
        }


       

        #region Page部分
        Dictionary<string, IPage> pagemap = new Dictionary<string, IPage>();
        void InitPage()
        {
            pagemap.Add(PageTypes.PAGE_TRADING, new PageTrading());
            pagemap.Add(PageTypes.PAGE_ORDER, new PageOrder());
            pagemap.Add(PageTypes.PAGE_TRADE, new PageTrade());
            pagemap.Add(PageTypes.PAGE_POSITION, new PagePosition());
            pagemap.Add(PageTypes.PAGE_QRY, new PageQry());
            pagemap.Add(PageTypes.PAGE_HELP, new PageHelp());
            switch (Constants.PageBankStyle)
            {
                case 0://常规出入金
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBank());
                        break;
                    }
                case 1://配资业务逻辑
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBank1());
                        break;
                    }
                case 2://分众支付 选择银行与支付宝
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankFZ());
                        break;
                    }
                case 4://七彩支付 选择银行银行
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankSe7());
                        break;
                    }
                case 3://
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankAliPay());
                        break;
                    }
                case 6://
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankAliPay2());
                        break;
                    }
                case 5://网页跳转支付
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankWebSite());
                        break;
                    }
                case 8://配资出入金
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBank8());
                        break;
                    }
                case 9://标准版选择银行三方支付
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankStd());
                        break;
                    }
                case 10://入金跳转到网站处理
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBankAPI());
                        break;
                    }
                default:
                    {
                        pagemap.Add(PageTypes.PAGE_BANK, new PageBank());
                        break;
                    }
            }
            pagemap.Add(PageTypes.PAGE_PASS, new PagePass());
            foreach (var page in pagemap.Values)
            {
                System.Windows.Forms.Control c = page as System.Windows.Forms.Control;
                if (c == null) continue;
                panelPageHolder.Controls.Add(c);
                c.Dock = DockStyle.Fill;
            }
        }

        IPage GetPage(string pagetype)
        {
            IPage page = null;
            if (pagemap.TryGetValue(pagetype, out page))
            {
                return page;
            }
            return null;
        }

        void ShowPage(string type)
        {
            IPage page = null;
            if (type == PageTypes.PAGE_CONFIG)
            {
                frmConfig fm = new frmConfig();
                fm.ShowDialog();
                fm.Close();
                return;
            }
            if (pagemap.TryGetValue(type, out page))
            {
                HideAllPage();
                switch (type)
                {
                    case PageTypes.PAGE_TRADING:
                        {
                            panelOrderEntry.Visible = true;
                            page.Show();
                            return;
                        }
                    case PageTypes.PAGE_ORDER:
                        {
                            panelOrderEntry.Visible = true;
                            page.Show();
                            return;
                        }
                    case PageTypes.PAGE_TRADE:
                        {
                            panelOrderEntry.Visible = true;
                            page.Show();
                            return;
                        }
                    case PageTypes.PAGE_POSITION:
                        {
                            panelOrderEntry.Visible = true;
                            page.Show();
                            return;
                        }
                    case PageTypes.PAGE_HELP:
                        {
                            page.Show();
                            panelOrderEntry.Visible = false;
                            return;
                        }
                    case PageTypes.PAGE_BANK:
                        {
                            page.Show();
                            panelOrderEntry.Visible = false;
                            return;
                        }
                    case PageTypes.PAGE_PASS:
                        {
                            page.Show();
                            panelOrderEntry.Visible = false;
                            return;
                        }
                    case PageTypes.PAGE_QRY:
                        {
                            page.Show();
                            panelOrderEntry.Visible = false;
                            return;
                        }
                    default:
                        return;
                }
            }
        
        }
        void HideAllPage()
        {
            foreach (var page in pagemap.Values)
            {
                page.Hide();
            }
        }
        #endregion


        void InitMenu()
        {
            ctrlListMenu1.AddMenu(new MenuItem("交易", Properties.Resources.f1, true, delegate() { ShowPage(PageTypes.PAGE_TRADING); }));
            ctrlListMenu1.AddMenu(new MenuItem("当日委托", Properties.Resources.f2, true, delegate() { ShowPage(PageTypes.PAGE_ORDER); }));
            ctrlListMenu1.AddMenu(new MenuItem("当日成交", Properties.Resources.f3, true, delegate() { ShowPage(PageTypes.PAGE_TRADE); }));
            ctrlListMenu1.AddMenu(new MenuItem("持仓", Properties.Resources.f4, true, delegate() { ShowPage(PageTypes.PAGE_POSITION); }));
            //ctrlListMenu1.AddMenu(new MenuItem("条件单", Properties.Resources.f5, false, delegate() { ShowPage(""); }));
            ctrlListMenu1.AddMenu(new MenuItem("查询", Properties.Resources.f6, true, delegate() { ShowPage(PageTypes.PAGE_QRY); }));
            //ctrlListMenu1.AddMenu(new MenuItem("行权", Properties.Resources.f7, false, delegate() { ShowPage(""); }));
            ctrlListMenu1.AddMenu(new MenuItem("参数设置", Properties.Resources.f8, true, delegate() { ShowPage(PageTypes.PAGE_CONFIG); }));
            ctrlListMenu1.AddMenu(new MenuItem("帮助及说明", Properties.Resources.f9, true, delegate() { ShowPage(PageTypes.PAGE_HELP); }));
            ctrlListMenu1.AddMenu(new MenuItem("银期转账", Properties.Resources.zj, true, delegate() { ShowPage(PageTypes.PAGE_BANK); }));
            //ctrlListMenu1.AddMenu(new MenuItem("交易统计", Properties.Resources.tj, false, delegate() { ShowPage(PageTypes.PAGE_STATISTIC); }));
            ctrlListMenu1.AddMenu(new MenuItem("修改密码", Properties.Resources.pass, true, delegate() { ShowPage(PageTypes.PAGE_PASS); }));

        }

        void WireEvent()
        {
            CoreService.EventCore.OnConnectedEvent += new VoidDelegate(EventCore_OnConnectedEvent);//连接建立
            CoreService.EventCore.OnDisconnectedEvent += new VoidDelegate(EventCore_OnDisconnectedEvent);//连接断开
            CoreService.EventCore.OnLoginEvent += new Action<LoginResponse>(EventCore_OnLoginEvent);//登入回报

            CoreService.EventHub.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventHub.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            CoreService.EventHub.OnRspXQryAccountFinanceEvent += new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent);

            btnMin.Click += new EventHandler(btnMin_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
            
            btnHide.Click += new EventHandler(btnHide_Click);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            btnLock.Click += new EventHandler(btnLock_Click);
        }

        

        /// <summary>
        /// 工作模式
        /// 工作模式需要响应底层连接断开与连接事件 用于重新登入并刷新数据
        /// </summary>
        bool workMode = false;

        void EventCore_OnDisconnectedEvent()
        {
            if (!workMode) return;
            lbDisconnect.Visible = true;
            lbDisconnect.Text = "交易服务器连接断开，正在重新建立交易连接...";
        }

        void EventCore_OnConnectedEvent()
        {
            if (!workMode) return;
            //System.Threading.Thread.Sleep(500);
            CoreService.TLClient.ReqLogin(CoreService.TLClient.UserName,CoreService.TLClient.Password, Constants.PRODUCT_INFO);
        }

        void EventCore_OnLoginEvent(LoginResponse obj)
        {
            if (!workMode) return;
            if (obj.Authorized)
            {
                //System.Threading.Thread.Sleep(500);
                //登入成功 请求刷新数据
                CoreService.TradingInfoTracker.ResumeData();
                lastRefresh = DateTime.Now;
            }
            else //更新界面提醒
            {
                lbDisconnect.Text = "重新登入交易账号失败，请退出交易客户端重新登入";
            }
        }


        #region 锁定 刷新按钮

        void btnLock_Click(object sender, EventArgs e)
        {
            LockTradingBox();
        }

        DateTime lastRefresh = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));

        /// <summary>
        /// 重新请求日内交易记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefresh_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(lastRefresh).TotalSeconds < 1)
            {
                MessageBox.Show("请勿频繁刷新数据");
                return;
            }
            CoreService.TradingInfoTracker.ResumeData();
            lastRefresh = DateTime.Now;
        }
        #endregion

        #region ControlBox
        void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认退出交易系统?", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                TradingBoxOpeartion(EnumTraderWindowOperation.Close);
            }
        }

        void btnMax_Click(object sender, EventArgs e)
        {
            TradingBoxOpeartion(EnumTraderWindowOperation.Max);
        }

        void btnMin_Click(object sender, EventArgs e)
        {
            TradingBoxOpeartion(EnumTraderWindowOperation.Min);
        }
        #endregion

        #region 事件回报
        /// <summary>
        /// 账户财务信息查询回报 更新可用资金
        /// </summary>
        /// <param name="obj"></param>
        void EventQry_OnRspXQryAccountFinanceEvent(RspXQryAccountFinanceResponse obj)
        {
            lbMoneyAvabile.Text = string.Format("{0:F2} ({1})", obj.Report.AvabileFunds, Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency));
        }

        void EventOther_OnResumeDataStart()
        {
            btnRefresh.Enabled = false;
        }

        void EventOther_OnResumeDataEnd()
        {
            btnRefresh.Enabled = true;
            lbDisconnect.Visible = false;
            //刷新 重新请求交易数据后 查询账户最新财务数据
            CoreService.TLClient.ReqXQryAccountFinance();
        }


        #endregion


        #region 隐藏下单面板
        bool _expandOrderEntry = true;
        /// <summary>
        /// 隐藏下单面板按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnHide_Click(object sender, EventArgs e)
        {
            _expandOrderEntry = !_expandOrderEntry;
            if (!_expandOrderEntry)
            {
                panelOrderEntry.Width = 9;
            }
            else
            {
                panelOrderEntry.Width = 344;
            }
            
        }
        #endregion

        /// <summary>
        /// 登入成功后 显示控件
        /// </summary>
        public void Entry()
        {
            workMode = true;
            lbDisconnect.Visible = false;
            //更新账户名字
            lbAccount.Text = string.Format("{0},您好！", (string.IsNullOrEmpty(CoreService.TradingInfoTracker.Account.Name) ? CoreService.TradingInfoTracker.Account.Account : CoreService.TradingInfoTracker.Account.Name));
            this.Show();

            //查询账户财务信息
            CoreService.TLClient.ReqXQryAccountFinance();
        }

        public void Exit()
        {
            workMode = false;
            this.Hide();
        }

        #region 弹窗提醒
        System.ComponentModel.BackgroundWorker bg;

        TradingLib.MarketData.RingBuffer<PromptMessage> infobuffer = new TradingLib.MarketData.RingBuffer<PromptMessage>(1000);




        void InitMessageBW()
        {
            bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerAsync();
            CoreService.EventCore.OnPromptMessageEvent += new Action<PromptMessage>(EventCore_OnPromptMessageEvent);
        }

        void EventCore_OnPromptMessageEvent(PromptMessage obj)
        {
            infobuffer.Write(obj);
        }

        /// <summary>
        /// 后台工作流程 当缓存中有数据是通过ReportProgress进行触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    //如果消息缓存中有内容则弹窗提醒
                    while (infobuffer.hasItems)
                    {
                        PromptMessage info = infobuffer.Read();
                        if (info != null)
                        {
                            MethodInvoker mi = new MethodInvoker(() =>
                            {
                                MessageBox.Show(info.Message, "信息:" + info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            );
                            IAsyncResult result = this.BeginInvoke(mi);
                            this.EndInvoke(result);
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("bg worker error:" + ex.ToString());
                }
            }
        }


        #endregion


        #region 内部控件暴露到MainContainer的操作

        public void OrderEntryClearSymbol()
        {
            ctrlOrderEntry1.ClearSymbol();
        }
        #endregion



    }
}
