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

            lbAsk1.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;
            lbAsk2.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;
            lbAsk3.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;
            lbAsk4.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;
            lbAsk5.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;

            lbBid1.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;
            lbBid2.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;
            lbBid3.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;
            lbBid4.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;
            lbBid5.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;

            lbUpper.StateCommon.ShortText.Color1 = UIConstant.LongLabelColor;
            lbLower.StateCommon.ShortText.Color1 = UIConstant.ShortLabelColor;

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
            else
            { 
                //如果本地行情快照没有对应合约行情 则查询行情快照
                CoreService.TLClient.ReqXQryTickSnapShot(arg2.Symbol);
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
                if (_symbol.Symbol != k.Symbol) return;

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
