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


            InitMenu();

            WireEvent();
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

        void ShowPage(string page)
        { 
        
        }

        void WireEvent()
        {
            ///btnHideOrderEntry.Click += new EventHandler(btnHideOrderEntry_Click);
        }

        void btnHideOrderEntry_Click(object sender, EventArgs e)
        {
            //ctrlOrderEntry1.Visible = !ctrlOrderEntry1.Visible;
        }

        public void OnInit()
        { 
        
        }

        public void OnDisposed()
        { 
        
        }


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
