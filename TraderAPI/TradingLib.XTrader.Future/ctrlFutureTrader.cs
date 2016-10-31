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
    public partial class ctrlFutureTrader : UserControl, TradingLib.API.IEventBinder
    {
        public event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

        ILog logger = LogManager.GetLogger("ctrlFutureTrader");

        public ctrlFutureTrader()
        {
            InitializeComponent();

            InitPage();

            InitMenu();

            WireEvent();

            InitMessageBW();
        }


        void InitMenu()
        {
            ctrlListMenu1.AddMenu(new MenuItem("交易", Properties.Resources.f1, true, delegate() { ShowPage(PageTypes.PAGE_TRADING); }));
            ctrlListMenu1.AddMenu(new MenuItem("当日委托", Properties.Resources.f2, true, delegate() { ShowPage(PageTypes.PAGE_ORDER); }));
            ctrlListMenu1.AddMenu(new MenuItem("当日成交", Properties.Resources.f3, true, delegate() { ShowPage(PageTypes.PAGE_TRADE); }));
            ctrlListMenu1.AddMenu(new MenuItem("持仓", Properties.Resources.f4, true, delegate() { ShowPage(PageTypes.PAGE_POSITION); }));
            ctrlListMenu1.AddMenu(new MenuItem("条件单", Properties.Resources.f5, false, delegate() { ShowPage(""); }));
            ctrlListMenu1.AddMenu(new MenuItem("查询", Properties.Resources.f6, true, delegate() { ShowPage(PageTypes.PAGE_QRY); }));
            ctrlListMenu1.AddMenu(new MenuItem("行权", Properties.Resources.f7, false, delegate() { ShowPage(""); }));
            ctrlListMenu1.AddMenu(new MenuItem("参数设置", Properties.Resources.f8, false, delegate() {  }));
            ctrlListMenu1.AddMenu(new MenuItem("帮助及说明", Properties.Resources.f9, true, delegate() { ShowPage(PageTypes.PAGE_HELP); }));
            ctrlListMenu1.AddMenu(new MenuItem("银期转账", Properties.Resources.zj, true, delegate() { ShowPage(PageTypes.PAGE_BANK); }));
            ctrlListMenu1.AddMenu(new MenuItem("交易统计", Properties.Resources.tj, true, delegate() { ShowPage(PageTypes.PAGE_STATISTIC); }));
            ctrlListMenu1.AddMenu(new MenuItem("密码", Properties.Resources.pass, true, delegate() { ShowPage(PageTypes.PAGE_PASS); }));

        }


        #region Page部分
        Dictionary<string, IPage> pagemap = new Dictionary<string, IPage>();
        void InitPage()
        {
            pagemap.Add(PageTypes.PAGE_TRADING, new PageTrading());
            pagemap.Add(PageTypes.PAGE_ORDER, new PageOrder());
            pagemap.Add(PageTypes.PAGE_TRADE, new PageTrade());
            pagemap.Add(PageTypes.PAGE_POSITION, new PagePosition());

            pagemap.Add(PageTypes.PAGE_HELP, new PageHelp());
            pagemap.Add(PageTypes.PAGE_BANK, new PageBank());
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


        void WireEvent()
        {
            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);

            CoreService.EventUI.OnSymbolUnSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolUnSelectedEvent);
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, TradingLib.API.Symbol>(EventUI_OnSymbolSelectedEvent);


            ///btnHideOrderEntry.Click += new EventHandler(btnHideOrderEntry_Click);
            btnHide.Click += new EventHandler(btnHide_Click);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            CoreService.EventCore.RegIEventHandler(this);
        }

        void EventUI_OnSymbolUnSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 != null)
            {
                //非常驻合约 则需要取消 避免不必要的订阅
                if (!CoreService.TradingInfoTracker.HotSymbols.Contains(arg2))
                {
                    CoreService.TLClient.ReqUnRegisterSymbol(arg2.Symbol);
                }
            }
        }

        void EventUI_OnSymbolSelectedEvent(object arg1, TradingLib.API.Symbol arg2)
        {
            if (arg2 != null)
            {
                CoreService.TLClient.ReqXQryTickSnapShot(arg2.Exchange, arg2.Symbol);
                CoreService.TLClient.ReqRegisterSymbol(arg2.Symbol);
                
            }
        }



        void EventOther_OnResumeDataEnd()
        {
            btnRefresh.Enabled = true;

            //数据恢复完毕后 订阅常驻合约 放入持仓列表中注册
            //foreach (var sym in CoreService.TradingInfoTracker.HotSymbols)
            //{
            //    CoreService.TLClient.ReqRegisterSymbol(sym.Symbol);
            //}
        }

        void EventOther_OnResumeDataStart()
        {
            btnRefresh.Enabled = false;
        }



        DateTime lastRefresh = DateTime.Now;
        bool refreshed = false;

        /// <summary>
        /// 重新请求日内交易记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefresh_Click(object sender, EventArgs e)
        {
            if (refreshed && DateTime.Now.Subtract(lastRefresh).TotalSeconds < 1)
            {
                MessageBox.Show("请勿频繁刷新数据");
                return;
            }
            CoreService.TradingInfoTracker.ResumeData();
            lastRefresh = DateTime.Now;
            refreshed = true;
        }

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

        void btnHideOrderEntry_Click(object sender, EventArgs e)
        {
            //ctrlOrderEntry1.Visible = !ctrlOrderEntry1.Visible;
        }

        public void OnInit()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnInit), new object[] { });
            }
            else
            {
                
                lbAccount.Text = string.Format("{0},您好！", (string.IsNullOrEmpty(CoreService.TradingInfoTracker.Account.Name) ? CoreService.TradingInfoTracker.Account.Account : CoreService.TradingInfoTracker.Account.Name));

                //cbAccount.Items.Add(string.Format("{0}-{1}", CoreService.TradingInfoTracker.Account.Name, CoreService.TradingInfoTracker.Account.Account));
                //cbAccount.SelectedIndex = 0;
            }
        }

        public void OnDisposed()
        { 
        
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

        public void PageSTKOrderEntry_SetSymbol(string exchange, string symbol)
        {
            //IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            //if (page != null)
            //{
            //    PageSTKOrderEntry p = (page as PageSTKOrderEntry);
            //    if (p != null)
            //    {
            //        p.SetSymbol(exchange, symbol);
            //    }
            //}
        }

        public void EntryBuyPage()
        {
            //IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            //if (page != null)
            //{
            //    (page as PageSTKOrderEntry).Mode = 0;
            //    ShowPage(PageTypes.PAGE_ORDER_ENTRY);
            //    btnBuy.Checked = true;
            //    btnBuy.ForeColor = Color.Red;
            //}
        }

        public void EntrySellPage()
        {
            //IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            //if (page != null)
            //{
            //    (page as PageSTKOrderEntry).Mode = 1;
            //    ShowPage(PageTypes.PAGE_ORDER_ENTRY);
            //    btnSell.Checked = true;
            //    btnSell.ForeColor = Color.Red;
            //}
        }


        #endregion



    }
}
