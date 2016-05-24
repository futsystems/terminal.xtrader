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
using StockTrader.API;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace StockTrader
{
    public partial class StockTrader : KryptonForm
    {
        Dictionary<EnumPageType, IPage> pagemap = new Dictionary<EnumPageType, IPage>();

        public StockTrader()
        {
            InitializeComponent();

            //初始化页面
            InitPage();

            //初始化左侧树状菜单
            InitMenuTree();

            WireEvent();

            ShowPage(EnumPageType.OrderEntryPage);
        }

        void WireEvent()
        {
            menuTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(menuTree_NodeMouseClick);

            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            //
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, TradingLib.API.Symbol>(OnSymbolSelectedEvent);
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            fmMessage fm = new fmMessage("测试", "多朋友都会在开发WinForm中遇到Label要显示的内容太长,但却不能换行的问题.这里我总结了几种方法,供大家参考:");
            fm.ShowDialog();
            fm.Close();
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
            pagemap.Add(EnumPageType.OrderEntryPage, new PageSTKOrderEntry());
            pagemap.Add(EnumPageType.CancelPage, new PageSTKOrderCancel());

            pagemap.Add(EnumPageType.OrderTodayPage, new PageSTKOrderToday());
            pagemap.Add(EnumPageType.TradeTodayPage, new PageSTKTradeToday());
            pagemap.Add(EnumPageType.OrderHistPage, new PageSTKOrderHist());
            pagemap.Add(EnumPageType.TradeHistPage, new PageSTKTradeHist());

            pagemap.Add(EnumPageType.AccountPage, new PageSTKAccountPosition());
            pagemap.Add(EnumPageType.DeliveryPage, new PageSTKDelivery());

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
        void ShowPage(EnumPageType type)
        {
            IPage page = null;
            if (pagemap.TryGetValue(type, out page))
            {
                HideAllPage();
                page.Show();
            }
        }

        IPage GetPage(EnumPageType pagetype)
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
            ShowPage(EnumPageType.OrderEntryPage);
        }

        void OpenPageSell(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 1;
            ShowPage(EnumPageType.OrderEntryPage);
        }
        void OpenPageBuySell(TreeNode node)
        {
            PageSTKOrderEntry p = node.Tag as PageSTKOrderEntry;
            p.Mode = 2;
            ShowPage(EnumPageType.OrderEntryPage);
        }

        void OpenPageCancel(TreeNode node)
        {
            PageSTKOrderCancel p = node.Tag as PageSTKOrderCancel;
            ShowPage(EnumPageType.CancelPage);
        }

        void OpenPageOrderToday(TreeNode node)
        {
            PageSTKOrderToday p = node.Tag as PageSTKOrderToday;
            ShowPage(EnumPageType.OrderTodayPage);
        }

        void OpenPageTradeToday(TreeNode node)
        {
            PageSTKTradeToday p = node.Tag as PageSTKTradeToday;
            ShowPage(EnumPageType.TradeTodayPage);
        }

        void OpenPageOrderHist(TreeNode node)
        {
            PageSTKOrderHist p = node.Tag as PageSTKOrderHist;
            ShowPage(EnumPageType.OrderHistPage);
        }

        void OpenPageTradeHist(TreeNode node)
        {
            PageSTKTradeHist p = node.Tag as PageSTKTradeHist;
            ShowPage(EnumPageType.TradeHistPage);
        }

        void OpenPageAccount(TreeNode node)
        {
            PageSTKAccountPosition p = node.Tag as PageSTKAccountPosition;
            ShowPage(EnumPageType.AccountPage);
            p.QryAccountInfo();
        }
        void OpenPageDelivery(TreeNode node)
        {
            PageSTKDelivery p = node.Tag as PageSTKDelivery;
            ShowPage(EnumPageType.DeliveryPage);
        }
        #endregion


        #region 初始化树状菜单与菜单点击事件
        /// <summary>
        /// 初始化菜单
        /// </summary>
        void InitMenuTree()
        {
            TreeNode node_buy = new TreeNode("买入[F1]");
            node_buy.ImageIndex = 1;
            node_buy.SelectedImageIndex = 1;
            node_buy.Tag = GetPage(EnumPageType.OrderEntryPage);
            
            menuTree.Nodes.Add(node_buy);

            TreeNode node_sell = new TreeNode("卖出[F2]");
            node_sell.ImageIndex = 2;
            node_sell.SelectedImageIndex = 2;
            node_sell.Tag = GetPage(EnumPageType.OrderEntryPage);
            menuTree.Nodes.Add(node_sell);

            TreeNode node_cancel = new TreeNode("撤单[F3]");
            node_cancel.ImageIndex = 3;
            node_cancel.SelectedImageIndex = 3;
            node_cancel.Tag = GetPage(EnumPageType.CancelPage);
            menuTree.Nodes.Add(node_cancel);

            TreeNode node_buysell = new TreeNode("双向委托");
            node_buysell.ImageIndex = 4;
            node_buysell.SelectedImageIndex = 4;
            node_buysell.Tag = GetPage(EnumPageType.OrderEntryPage);
            menuTree.Nodes.Add(node_buysell);


            TreeNode node_search = new TreeNode("查询[F4]");
            node_search.ImageIndex = 5;
            node_search.SelectedImageIndex = 5;
            menuTree.Nodes.Add(node_search);


            TreeNode node_search_todayorder = new TreeNode("当日委托");
            node_search_todayorder.ImageIndex = 5;
            node_search_todayorder.SelectedImageIndex = 5;
            node_search_todayorder.Tag = GetPage(EnumPageType.OrderTodayPage);
            node_search.Nodes.Add(node_search_todayorder);


            TreeNode node_search_todaytrade = new TreeNode("当日成交");
            node_search_todaytrade.ImageIndex = 5;
            node_search_todaytrade.SelectedImageIndex = 5;
            node_search_todaytrade.Tag = GetPage(EnumPageType.TradeTodayPage);
            node_search.Nodes.Add(node_search_todaytrade);

            TreeNode node_search_historder = new TreeNode("历史委托");
            node_search_historder.ImageIndex = 5;
            node_search_historder.SelectedImageIndex = 5;
            node_search_historder.Tag = GetPage(EnumPageType.OrderHistPage);
            node_search.Nodes.Add(node_search_historder);

            TreeNode node_search_histtrade = new TreeNode("历史成交");
            node_search_histtrade.ImageIndex = 5;
            node_search_histtrade.SelectedImageIndex = 5;
            node_search_histtrade.Tag = GetPage(EnumPageType.TradeHistPage);
            node_search.Nodes.Add(node_search_histtrade);

            TreeNode node_search_account = new TreeNode("资金股票");
            node_search_account.ImageIndex = 5;
            node_search_account.SelectedImageIndex = 5;
            node_search_account.Tag = GetPage(EnumPageType.AccountPage);
            node_search.Nodes.Add(node_search_account);


            TreeNode node_search_delivery = new TreeNode("交割单");
            node_search_delivery.ImageIndex = 5;
            node_search_delivery.SelectedImageIndex = 5;
            node_search_delivery.Tag = GetPage(EnumPageType.DeliveryPage);
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

        RingBuffer<RspInfo> infobuffer = new RingBuffer<RspInfo>(1000);


        /// <summary>
        /// 将需要弹出的消息放入缓存
        /// </summary>
        /// <param name="info"></param>
        void OnRspInfo(TradingLib.API.RspInfo info)
        {
            //将RspInfo写入缓存 等待后台线程进行处理
            infobuffer.Write(info);
        }

        void InitPopBW()
        {
            bg = new BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);
            bg.RunWorkerAsync();
            MessageBox.Show("eee");
            CoreService.EventCore.OnRspInfoEvent += new Action<RspInfo>(OnRspInfo);
        }

        /// <summary>
        /// 当后台线程有触发时 调用显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RspInfo info = e.UserState as RspInfo;
            //System.Drawing.Point p = PointToScreen(status.Location);
            //p = new System.Drawing.Point(p.X, p.Y - popwindow.Height + status.Height);

            //popwindow.Location = p;
            //popwindow.PopMessage(info);
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
                //检查变量 然后对外触发 
                while (infobuffer.hasItems)
                {
                    RspInfo info = infobuffer.Read();
                    bg.ReportProgress(1, info);
                    Util.sleep(1000);
                }
                Util.sleep(50);
            }
        }


        #endregion


        private void ctOrderViewSTK1_Load(object sender, EventArgs e)
        {

        }
    }
}
