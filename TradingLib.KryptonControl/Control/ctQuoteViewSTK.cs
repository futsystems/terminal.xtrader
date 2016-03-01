using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using ComponentFactory.Krypton.Toolkit;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.KryptonControl
{
    public partial class ctQuoteViewSTK : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctQuoteViewSTK");
        public ctQuoteViewSTK()
        {
            InitializeComponent();
            //label1.TextAlign = ContentAlignment.

            this.Load += new EventHandler(ctQuoteViewSTK_Load);
            
        }

        Symbol _symbol = null;
        public Symbol SymbolSelected { get { return _symbol; } }

        void ctQuoteViewSTK_Load(object sender, EventArgs e)
        {
            CoreService.EventCore.RegIEventHandler(this);
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolSelectedEvent);

            //响应实时行情
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
        }



        public void OnInit()
        {
            
        }



        public void OnDisposed()
        {

        }

        /// <summary>
        /// 响应合约选择事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventUI_OnSymbolSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 == null) return;
            logger.Info(string.Format("Will show quote for symbol:{0}", arg2.Symbol));
            _symbol = arg2;
            Tick k = CoreService.TradingInfoTracker.TickTracker[_symbol.Symbol];
            if (k != null)
            {
                GotTick(k);
            }
        }


        public void GotTick(Tick k)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Tick>(GotTick), new object[] { k });
            }
            else
            {
                //合约过滤
                if (_symbol == null) return;
                if (_symbol.Symbol != k.Symbol) return;

                string _format = "{0:F2}";
                if (k.IsTrade())
                {
                    lbTrade.Text = k.Trade.ToFormatStr(_format);
                }

                lbAsk1.Text = k.AskPrice.ToFormatStr(_format);
                lbAskSize1.Text = k.AskSize.ToString();

                lbBid1.Text = k.BidPrice.ToFormatStr(_format);
                lbBidSize1.Text = k.BidSize.ToString();

            }
        }
    }
}
