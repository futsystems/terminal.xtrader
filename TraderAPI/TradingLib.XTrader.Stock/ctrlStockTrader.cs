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


namespace TradingLib.XTrader.Stock
{
    public partial class ctrlStockTrader : UserControl,TradingLib.API.IEventBinder
    {
        public event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

        ILog logger = LogManager.GetLogger("ctlStockTrader");
        public ctrlStockTrader()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, mainPanel, new object[] { true });

            InitPage();

            InitMenuTree();

            WireEvent();

            InitMessageBW();
        }


        void WireEvent()
        {
            menuTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(menuTree_NodeMouseClick);

            btnBuy.Click += new EventHandler(btnBuy_Click);
            btnSell.Click += new EventHandler(btnSell_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnTodayTrade.Click += new EventHandler(btnTodayTrade_Click);
            btnPosition.Click += new EventHandler(btnPosition_Click);

            btnRefresh.Click += new EventHandler(btnRefresh_Click);

            btnMin.Click += new EventHandler(btnMin_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnClose.Click += new EventHandler(btnClose_Click);


            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);

            CoreService.EventUI.OnSymbolUnSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolUnSelectedEvent);
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolSelectedEvent);

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
                CoreService.TLClient.ReqRegisterSymbol(arg2.Symbol);
            }
        }


        

        void EventOther_OnResumeDataEnd()
        {
            btnRefresh.Enabled = true;

            //数据恢复完毕后 订阅常驻合约
            foreach (var sym in CoreService.TradingInfoTracker.HotSymbols)
            {
                CoreService.TLClient.ReqRegisterSymbol(sym.Symbol);
            }
        }

        void EventOther_OnResumeDataStart()
        {
            btnRefresh.Enabled = false;
        }


        public void OnInit()
        {

            if (InvokeRequired)
            {
                Invoke(new Action(OnInit), new object[] { });
            }
            else
            {
                cbAccount.Items.Add(string.Format("{0}-{1}", CoreService.TradingInfoTracker.Account.Name,CoreService.TradingInfoTracker.Account.Account));
                cbAccount.SelectedIndex = 0;
            }
        }

        public void OnDisposed()
        {
        
        }


        void InitPage()
        {
            pagemap.Add(PageTypes.PAGE_ORDER_ENTRY, new PageSTKOrderEntry());
            pagemap.Add(PageTypes.PAGE_ORDER_CANCEL, new PageSTKOrderCancel());

            pagemap.Add(PageTypes.PAGE_ORDER_TODAY, new PageSTKOrderToday());
            pagemap.Add(PageTypes.PAGE_TRADE_TODAY, new PageSTKTradeToday());
            pagemap.Add(PageTypes.PAGE_ORDER_HIST, new PageSTKOrderHist());
            pagemap.Add(PageTypes.PAGE_TRADE_HIST, new PageSTKTradeHist());

            pagemap.Add(PageTypes.PAGE_ACCOUNT_POSITION, new PageSTKAccountPosition());
            pagemap.Add(PageTypes.PAGE_DELIVERY, new PageSTKDelivery());

            pagemap.Add(PageTypes.PAGE_CHANGE_PASS, new PageSTKChangePass());
            foreach (var page in pagemap.Values)
            {
                Control c = page as Control;
                if (c == null) continue;
                mainPanel.Controls.Add(c);
                c.Dock = DockStyle.Fill;
            }

        }

        /// <summary>
        /// 隐藏所有页面
        /// </summary>
        void HideAllPage()
        {
            foreach (var page in pagemap.Values)
            {
                page.Hide();
            }
        }

        /// <summary>
        /// 取消所有按钮选中状态
        /// </summary>
        void UnCheckAllButton()
        {
            btnBuy.Checked = false;
            btnBuy.ForeColor = Color.Black;
            btnSell.Checked = false;
            btnSell.ForeColor = Color.Black;
            btnCancel.Checked = false;
            btnCancel.ForeColor = Color.Black;
            btnTodayTrade.Checked = false;
            btnTodayTrade.ForeColor = Color.Black;
            btnPosition.Checked = false;
            btnPosition.ForeColor = Color.Black;
        }

        /// <summary>
        /// 显示某个类别的页面
        /// </summary>
        /// <param name="type"></param>
        void ShowPage(string type)
        {
            IPage page = null;
            if (pagemap.TryGetValue(type, out page))
            {
                HideAllPage();
                UnCheckAllButton();
                page.Show();
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


        #region 打开页面
        void OpenPageBuy(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 0;
            ShowPage(PageTypes.PAGE_ORDER_ENTRY);
            btnBuy.Checked = true;
            btnBuy.ForeColor = Color.Red;
        }

        void OpenPageSell(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 1;
            ShowPage(PageTypes.PAGE_ORDER_ENTRY);
            btnSell.Checked = true;
            btnSell.ForeColor = Color.Red;
        }
        void OpenPageBuySell(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 2;
            ShowPage(PageTypes.PAGE_ORDER_ENTRY);
        }

        void OpenPageCancel(TreeNode node)
        {
            PageSTKOrderCancel p = node.Tag as PageSTKOrderCancel;
            ShowPage(PageTypes.PAGE_ORDER_CANCEL);
            btnCancel.Checked = true;
            btnCancel.ForeColor = Color.Red;
        }

        void OpenPageOrderToday(TreeNode node)
        {
            PageSTKOrderToday p = node.Tag as PageSTKOrderToday;
            ShowPage(PageTypes.PAGE_ORDER_TODAY);
            
        }

        void OpenPageTradeToday(TreeNode node)
        {
            PageSTKTradeToday p = node.Tag as PageSTKTradeToday;
            ShowPage(PageTypes.PAGE_TRADE_TODAY);
            btnTodayTrade.Checked = true;
            btnTodayTrade.ForeColor = Color.Red;
        }

        void OpenPageOrderHist(TreeNode node)
        {
            PageSTKOrderHist p = node.Tag as PageSTKOrderHist;
            ShowPage(PageTypes.PAGE_ORDER_HIST);
        }

        void OpenPageTradeHist(TreeNode node)
        {
            PageSTKTradeHist p = node.Tag as PageSTKTradeHist;
            ShowPage(PageTypes.PAGE_TRADE_HIST);
        }

        void OpenPageAccount(TreeNode node)
        {
            PageSTKAccountPosition p = node.Tag as PageSTKAccountPosition;
            ShowPage(PageTypes.PAGE_ACCOUNT_POSITION);
            p.QryAccountFinance();
            btnPosition.Checked = true;
            btnPosition.ForeColor = Color.Red;
        }
        void OpenPageDelivery(TreeNode node)
        {
            PageSTKDelivery p = node.Tag as PageSTKDelivery;
            ShowPage(PageTypes.PAGE_DELIVERY);
        }

        void OpenChangePass(TreeNode node)
        {
            PageSTKChangePass p = node.Tag as PageSTKChangePass;
            ShowPage(PageTypes.PAGE_CHANGE_PASS);
        }
        #endregion

        Dictionary<string, IPage> pagemap = new Dictionary<string, IPage>();
        /// <summary>
        /// 初始化菜单
        /// </summary>
        void InitMenuTree()
        {
            menuTree.ImageList = imageList1;
            TreeNode node_buy = new TreeNode("买入[F1]");
            node_buy.ImageIndex = 1;
            node_buy.SelectedImageIndex = 1;
            node_buy.Tag = GetPage(PageTypes.PAGE_ORDER_ENTRY);

            menuTree.Nodes.Add(node_buy);

            TreeNode node_sell = new TreeNode("卖出[F2]");
            node_sell.ImageIndex = 2;
            node_sell.SelectedImageIndex = 2;
            node_sell.Tag = GetPage(PageTypes.PAGE_ORDER_ENTRY);
            menuTree.Nodes.Add(node_sell);

            TreeNode node_cancel = new TreeNode("撤单[F3]");
            node_cancel.ImageIndex = 3;
            node_cancel.SelectedImageIndex = 3;
            node_cancel.Tag = GetPage(PageTypes.PAGE_ORDER_CANCEL);
            menuTree.Nodes.Add(node_cancel);

            //TreeNode node_buysell = new TreeNode("双向委托");
            //node_buysell.ImageIndex = 4;
            //node_buysell.SelectedImageIndex = 4;
            //node_buysell.Tag = GetPage(PageTypes.PAGE_ORDER_ENTRY);
            //menuTree.Nodes.Add(node_buysell);


            TreeNode node_search = new TreeNode("查询[F4]");
            node_search.ImageIndex = 5;
            node_search.SelectedImageIndex = 5;
            menuTree.Nodes.Add(node_search);


            TreeNode node_pass = new TreeNode("修改密码");
            node_pass.ImageIndex = 7;
            node_pass.SelectedImageIndex = 7;
            node_pass.Tag = GetPage(PageTypes.PAGE_CHANGE_PASS);
            menuTree.Nodes.Add(node_pass);


            TreeNode node_search_account = new TreeNode("资金股票");
            node_search_account.ImageIndex = 6;
            node_search_account.SelectedImageIndex = 6;
            node_search_account.Tag = GetPage(PageTypes.PAGE_ACCOUNT_POSITION);
            node_search.Nodes.Add(node_search_account);

            TreeNode node_search_todayorder = new TreeNode("当日委托");
            node_search_todayorder.ImageIndex = 6;
            node_search_todayorder.SelectedImageIndex = 6;
            node_search_todayorder.Tag = GetPage(PageTypes.PAGE_ORDER_TODAY);
            node_search.Nodes.Add(node_search_todayorder);


            TreeNode node_search_todaytrade = new TreeNode("当日成交");
            node_search_todaytrade.ImageIndex = 6;
            node_search_todaytrade.SelectedImageIndex = 6;
            node_search_todaytrade.Tag = GetPage(PageTypes.PAGE_TRADE_TODAY);
            node_search.Nodes.Add(node_search_todaytrade);

            TreeNode node_search_historder = new TreeNode("历史委托");
            node_search_historder.ImageIndex = 6;
            node_search_historder.SelectedImageIndex = 6;
            node_search_historder.Tag = GetPage(PageTypes.PAGE_ORDER_HIST);
            node_search.Nodes.Add(node_search_historder);

            TreeNode node_search_histtrade = new TreeNode("历史成交");
            node_search_histtrade.ImageIndex = 6;
            node_search_histtrade.SelectedImageIndex = 6;
            node_search_histtrade.Tag = GetPage(PageTypes.PAGE_TRADE_HIST);
            node_search.Nodes.Add(node_search_histtrade);

            


            TreeNode node_search_delivery = new TreeNode("交割单");
            node_search_delivery.ImageIndex = 6;
            node_search_delivery.SelectedImageIndex = 6;
            node_search_delivery.Tag = GetPage(PageTypes.PAGE_DELIVERY);
            node_search.Nodes.Add(node_search_delivery);
            node_search.Expand();
        }
        void menuTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                //MessageBox.Show(e.Node.Index.ToString());
                switch (e.Node.Index)
                {
                    case 0:
                        OpenPageBuy(e.Node);
                        return;
                    case 1:
                        OpenPageSell(e.Node);
                        return;
                    case 2:
                        OpenPageCancel(e.Node);
                        return;
                    //case 3:
                    //    OpenPageBuySell(e.Node);
                        //return;
                    case 4:
                        OpenChangePass(e.Node);
                        return;
                    default:
                        return;
                }
            }
            else if (e.Node.Parent.Index == 3)
            {
                switch (e.Node.Index)
                {
                    case 0:
                        OpenPageAccount(e.Node);
                        return;
                    case 1:
                        OpenPageOrderToday(e.Node);
                        return;
                    case 2:
                        OpenPageTradeToday(e.Node);
                        return;
                    case 3:
                        OpenPageOrderHist(e.Node);
                        return;
                    case 4:
                        OpenPageTradeHist(e.Node);
                        return;
                    case 5:
                        OpenPageDelivery(e.Node);
                        return;
                    default:
                        return;
                }
            }

        }




        #region 按钮事件


        public void EntryBuyPage()
        {
            IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            if (page != null)
            {
                (page as PageSTKOrderEntry).Mode = 0;
                ShowPage(PageTypes.PAGE_ORDER_ENTRY);
                btnBuy.Checked = true;
                btnBuy.ForeColor = Color.Red;
            }
        }

        public void EntrySellPage()
        {
            IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            if (page != null)
            {
                (page as PageSTKOrderEntry).Mode = 1;
                ShowPage(PageTypes.PAGE_ORDER_ENTRY);
                btnSell.Checked = true;
                btnSell.ForeColor = Color.Red;
            }
        }
        void btnBuy_Click(object sender, EventArgs e)
        {
            EntryBuyPage();
        }

        void btnSell_Click(object sender, EventArgs e)
        {

            EntrySellPage();
        }

        void btnPosition_Click(object sender, EventArgs e)
        {
            IPage page = pagemap[PageTypes.PAGE_ACCOUNT_POSITION];
            if (page != null)
            {
                ShowPage(PageTypes.PAGE_ACCOUNT_POSITION);
                PageSTKAccountPosition p = page as PageSTKAccountPosition;
                p.QryAccountFinance();
                btnPosition.Checked = true;
                btnPosition.ForeColor = Color.Red;
            }
        }

        void btnTodayTrade_Click(object sender, EventArgs e)
        {
            IPage page = pagemap[PageTypes.PAGE_TRADE_TODAY];
            if (page != null)
            {
                ShowPage(PageTypes.PAGE_TRADE_TODAY);
                btnTodayTrade.Checked = true;
                btnTodayTrade.ForeColor = Color.Red;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            IPage page = pagemap[PageTypes.PAGE_ORDER_CANCEL];
            if (page != null)
            {
                ShowPage(PageTypes.PAGE_ORDER_CANCEL);
                btnCancel.Checked = true;
                btnCancel.ForeColor = Color.Red;
            }
        }


        DateTime lastRefresh = DateTime.Now;
        bool refreshed = false;
        void btnRefresh_Click(object sender, EventArgs e)
        {
            if (refreshed &&DateTime.Now.Subtract(lastRefresh).TotalSeconds < 10)
            {
                MessageBox.Show("请勿频繁刷新数据");
                return;
            }
            CoreService.TradingInfoTracker.ResumeData();
            lastRefresh = DateTime.Now;
            refreshed = true;
        }



        void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认退出交易系统?", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                if (TraderWindowOpeartion != null)
                {
                    TraderWindowOpeartion(EnumTraderWindowOperation.Close);
                }
            }
        }

        void btnMax_Click(object sender, EventArgs e)
        {
            if (TraderWindowOpeartion != null)
            {
                TraderWindowOpeartion(EnumTraderWindowOperation.Max);
            }
        }

        void btnMin_Click(object sender, EventArgs e)
        {
            if (TraderWindowOpeartion != null)
            {
                TraderWindowOpeartion(EnumTraderWindowOperation.Min);
            }
        }



        #endregion


        #region 内部控件暴露到MainContainer的操作

        public void PageSTKOrderEntry_SetSymbol(string exchange, string symbol)
        {
            IPage page = pagemap[PageTypes.PAGE_ORDER_ENTRY];
            if (page != null)
            {
                PageSTKOrderEntry p = (page as PageSTKOrderEntry);
                if (p != null)
                {
                    p.SetSymbol(exchange, symbol);
                }
            }
        }


        #endregion


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
                                MessageBox.Show(info.Message,"信息:"+info.Title,MessageBoxButtons.OK,MessageBoxIcon.Information); }
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

    }
}
