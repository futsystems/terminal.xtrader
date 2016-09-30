using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.XTrader.Stock
{
    public partial class ctQuoteViewSTK : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctQuoteViewSTK");
        public ctQuoteViewSTK()
        {
            InitializeComponent();
            //label1.TextAlign = ContentAlignment.
            lbAsk1.ForeColor = UIConstant.LongLabelColor;
            lbAsk2.ForeColor = UIConstant.LongLabelColor;
            lbAsk3.ForeColor = UIConstant.LongLabelColor;
            lbAsk4.ForeColor = UIConstant.LongLabelColor;
            lbAsk5.ForeColor = UIConstant.LongLabelColor;

            lbBid1.ForeColor = UIConstant.ShortLabelColor;
            lbBid2.ForeColor = UIConstant.ShortLabelColor;
            lbBid3.ForeColor = UIConstant.ShortLabelColor;
            lbBid4.ForeColor = UIConstant.ShortLabelColor;
            lbBid5.ForeColor = UIConstant.ShortLabelColor;

            lbUpper.ForeColor = UIConstant.LongLabelColor;
            lbLower.ForeColor = UIConstant.ShortLabelColor;

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

        const string lbnull = "--";
        /// <summary>
        /// 响应合约选择事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventUI_OnSymbolSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 == null)
            {
                _symbol = null;
                lbTrade.Text = lbnull;
                lbAsk1.Text = lbnull;
                lbAskSize1.Text = lbnull;

                lbAsk2.Text = lbnull;
                lbAskSize2.Text = lbnull;

                lbAsk3.Text = lbnull;
                lbAskSize3.Text = lbnull;

                lbAsk4.Text = lbnull;
                lbAskSize4.Text = lbnull;

                lbAsk5.Text = lbnull;
                lbAskSize5.Text = lbnull;


                lbBid1.Text = lbnull;
                lbBidSize1.Text = lbnull;

                lbBid2.Text = lbnull;
                lbBidSize2.Text = lbnull;

                lbBid3.Text = lbnull;
                lbBidSize3.Text = lbnull;

                lbBid4.Text = lbnull;
                lbBidSize4.Text = lbnull;

                lbBid5.Text = lbnull;
                lbBidSize5.Text = lbnull;

                lbPect.Text = lbnull;

                lbUpper.Text = lbnull;
                lbLower.Text = lbnull;
                return;

            }
            logger.Info(string.Format("Set QuoteView symbol:{0}", arg2.Symbol));
            _symbol = arg2;
            Tick k = CoreService.TradingInfoTracker.TickTracker[_symbol.Exchange,_symbol.Symbol];
            if (k != null)
            {
                GotTick(k);
            }
            else
            { 
                //如果本地行情快照没有对应合约行情 则查询行情快照
                CoreService.TLClient.ReqXQryTickSnapShot(arg2.Exchange,arg2.Symbol);
            }
        }


        /// <summary>
        /// 响应行情数据
        /// </summary>
        /// <param name="k"></param>
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
                if (_symbol.Exchange!=k.Exchange ||  _symbol.Symbol != k.Symbol) return;

                string _format = "{0:F2}";
                if (k.IsTrade())
                {
                    lbTrade.Text = k.Trade.ToFormatStr(_format);
                }

                lbAsk1.Text = k.AskPrice.ToFormatStr(_format);
                lbAskSize1.Text = k.AskSize.ToString();

                lbAsk2.Text = k.AskPrice2.ToFormatStr(_format);
                lbAskSize2.Text = k.AskSize2.ToString();

                lbAsk3.Text = k.AskPrice3.ToFormatStr(_format);
                lbAskSize3.Text = k.AskSize3.ToString();

                lbAsk4.Text = k.AskPrice4.ToFormatStr(_format);
                lbAskSize4.Text = k.AskSize4.ToString();

                lbAsk5.Text = k.AskPrice5.ToFormatStr(_format);
                lbAskSize5.Text = k.AskSize5.ToString();


                lbBid1.Text = k.BidPrice.ToFormatStr(_format);
                lbBidSize1.Text = k.BidSize.ToString();

                lbBid2.Text = k.BidPrice2.ToFormatStr(_format);
                lbBidSize2.Text = k.BidSize2.ToString();

                lbBid3.Text = k.BidPrice3.ToFormatStr(_format);
                lbBidSize3.Text = k.BidSize3.ToString();

                lbBid4.Text = k.BidPrice4.ToFormatStr(_format);
                lbBidSize4.Text = k.BidSize4.ToString();

                lbBid5.Text = k.BidPrice5.ToFormatStr(_format);
                lbBidSize5.Text = k.BidSize5.ToString();

                lbPect.Text = ((k.Trade - k.PreClose) / k.PreClose * 100).ToFormatStr() + "%";

                lbUpper.Text = k.UpperLimit.ToFormatStr(_format);
                lbLower.Text = k.LowerLimit.ToFormatStr(_format);

            }
        }
    }
}
