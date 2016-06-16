using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ComponentFactory.Krypton.Toolkit;
using TradingLib.KryptonControl;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

using Common.Logging;

namespace StockTrader
{
    public partial class StockTrader : KryptonForm
    {
        Dictionary<string, IPage> pagemap = new Dictionary<string, IPage>();
        ILog logger = LogManager.GetLogger("StockTrader");
        public StockTrader()
        {
            InitializeComponent();

            //初始化页面
            InitPage();

            //初始化左侧树状菜单
            InitMenuTree();

            InitControl();

            //绑定事件
            WireEvent();


            //启动消息弹窗线程
            InitMessageBW();

            ShowPage(PageTypes.PAGE_ORDER_ENTRY);
        }

        void InitControl()
        {
            lbProgrameName.Text = TraderConstant.PROGRAME_NAME;
        }
        void WireEvent()
        {
            menuTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(menuTree_NodeMouseClick);

            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            btnPass.Click += new EventHandler(btnPass_Click);
            btnExit.Click += new EventHandler(btnExit_Click);
            //
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, TradingLib.API.Symbol>(OnSymbolSelectedEvent);
            CoreService.EventCore.OnConnectedEvent += new VoidDelegate(EventCore_OnConnectedEvent);
            CoreService.EventCore.OnDisconnectedEvent += new VoidDelegate(EventCore_OnDisconnectedEvent);

            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);

            this.Load += new EventHandler(StockTrader_Load);
            CoreService.EventCore.OnLoginEvent += new Action<LoginResponse>(EventCore_OnLoginEvent);
        }

        void EventCore_OnLoginEvent(LoginResponse obj)
        {
            if(obj.RspInfo.ErrorID == 0)
            {
                this.Text = "股票交易终端-{0}".Put(obj.Account);
            }
        }

        void StockTrader_Load(object sender, EventArgs e)
        {
            
        }

        

        void EventOther_OnResumeDataEnd()
        {
            btnRefresh.Enabled = true;
        }

        void EventOther_OnResumeDataStart()
        {
            btnRefresh.Enabled = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            if (fmConfirm.Show("退出", "确认退出交易客户端?") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        void btnPass_Click(object sender, EventArgs e)
        {
            ShowPage(PageTypes.PAGE_CHANGE_PASS);
        }

        void EventCore_OnDisconnectedEvent()
        {
            lbConnectImg.Image = Properties.Resources.disconnected;
        }

        void EventCore_OnConnectedEvent()
        {
            lbConnectImg.Image = Properties.Resources.connected;
        }

        bool conn = false;
        void btnRefresh_Click(object sender, EventArgs e)
        {
            CoreService.TradingInfoTracker.ResumeData();          
        }

        void OnSymbolSelectedEvent(object arg1, TradingLib.API.Symbol arg2)
        {
            if (arg2 != null)
            {
                CoreService.TLClient.ReqRegisterSymbols(new string[] { arg2.Symbol });
            }
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
        /// 显示某个类别的页面
        /// </summary>
        /// <param name="type"></param>
        void ShowPage(string type)
        {
            IPage page = null;
            if (pagemap.TryGetValue(type, out page))
            {
                HideAllPage();
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
        }

        void OpenPageSell(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 1;
            ShowPage(PageTypes.PAGE_ORDER_ENTRY);
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
            p.QryAccountInfo();
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


        #region 初始化树状菜单与菜单点击事件
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

            TreeNode node_buysell = new TreeNode("双向委托");
            node_buysell.ImageIndex = 4;
            node_buysell.SelectedImageIndex = 4;
            node_buysell.Tag = GetPage(PageTypes.PAGE_ORDER_ENTRY);
            menuTree.Nodes.Add(node_buysell);


            TreeNode node_search = new TreeNode("查询[F4]");
            node_search.ImageIndex = 5;
            node_search.SelectedImageIndex = 5;
            menuTree.Nodes.Add(node_search);


            TreeNode node_pass = new TreeNode("修改密码");
            node_pass.ImageIndex = 7;
            node_pass.SelectedImageIndex = 7;
            node_pass.Tag = GetPage(PageTypes.PAGE_CHANGE_PASS);
            menuTree.Nodes.Add(node_pass);



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

            TreeNode node_search_account = new TreeNode("资金股票");
            node_search_account.ImageIndex = 6;
            node_search_account.SelectedImageIndex = 6;
            node_search_account.Tag = GetPage(PageTypes.PAGE_ACCOUNT_POSITION);
            node_search.Nodes.Add(node_search_account);


            TreeNode node_search_delivery = new TreeNode("交割单");
            node_search_delivery.ImageIndex = 6;
            node_search_delivery.SelectedImageIndex = 6;
            node_search_delivery.Tag = GetPage(PageTypes.PAGE_DELIVERY);
            node_search.Nodes.Add(node_search_delivery);
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
                    case 3:
                        OpenPageBuySell(e.Node);
                        return;
                    case 5:
                        OpenChangePass(e.Node);
                        return;
                    default:
                        return;
                }
            }
            else if (e.Node.Parent.Index == 4)
            {
                switch (e.Node.Index)
                {
                    case 0:
                        OpenPageOrderToday(e.Node);
                        return;
                    case 1:
                        OpenPageTradeToday(e.Node);
                        return;
                    case 2:
                        OpenPageOrderHist(e.Node);
                        return;
                    case 3:
                        OpenPageTradeHist(e.Node);
                        return;
                    case 4:
                        OpenPageAccount(e.Node);
                        return;
                    case 5:
                        OpenPageDelivery(e.Node);
                        return;
                    default:
                        return;
                }
            }

        }
        #endregion



        #region 弹窗提醒


        
        System.ComponentModel.BackgroundWorker bg;

        RingBuffer<PromptMessage> infobuffer = new RingBuffer<PromptMessage>(1000);


       

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
                            MethodInvoker mi = new MethodInvoker(() => { fmMessage.Show(info); });
                            IAsyncResult result = this.BeginInvoke(mi);
                            this.EndInvoke(result);
                        }
                        Util.sleep(100);
                    }
                    Util.sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("bg worker error:" + ex.ToString());
                }
            }
        }


        #endregion


        private void ctOrderViewSTK1_Load(object sender, EventArgs e)
        {

        }
    }
}
