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


namespace TradingLib.KryptonControl
{
    public partial class ctOrderSenderSTK : UserControl
    {
        Font TITLEFONT = new Font("宋体", 14, FontStyle.Bold);

        ILog logger = LogManager.GetLogger("ctOrderSenderSTK");
        public ctOrderSenderSTK()
        {
            InitializeComponent();
            //kryptonLabel1.Font = TITLEFONT;
            this.Side = true;


            WireEvent();
            //设置空间为背景透明
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor|ControlStyles.Opaque,true);
            //this.BackColor = Color.Transparent;  
        }

        void WireEvent()
        {
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(EventIndicator_GotTickEvent);
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolSelectedEvent);
            CoreService.EventQry.OnRspXQrySymbolResponse += new Action<Symbol, RspInfo, int, bool>(EventQry_OnRspXQrySymbolResponse);
            CoreService.EventOther.OnRspQryMaxOrderVolResponse += new Action<RspQryMaxOrderVolResponse>(EventOther_OnRspQryMaxOrderVolResponse);

            //用于检查委托提交返回 并设置界面提交委托按钮有效 提交委托后 需要等对应委托回报到达后才可以再次提交委托 避免多次提交产生错误
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotErrorOrderEvent += new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent);

            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            symbol.TextChanged += new EventHandler(symbol_TextChanged);
            
        }

        void EventOther_OnResumeDataEnd()
        {
            btnSubmit.Enabled = true;
            btnReset.Enabled = true;
        }

        void EventOther_OnResumeDataStart()
        {
            btnSubmit.Enabled = false;
            btnReset.Enabled = false;
        }

        void EventIndicator_GotErrorOrderEvent(Order arg1, RspInfo arg2)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent), new object[] { arg1, arg2 });
            }
            else
            {
                if (arg1.RequestID == _orderInesertId)
                {
                    btnSubmit.Enabled = true;
                }
            }
        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Order>(EventIndicator_GotOrderEvent), new object[] { obj });
            }
            else
            {
                if (obj.RequestID == _orderInesertId)
                {
                    btnSubmit.Enabled = true;
                }
            }
        }

        int _orderInesertId = 0;
        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_symbol == null)
            {
                fmMessage.Show("委托参数错误", "请输入交易股票代码");
                return;
            }
            if (size.Value == 0)
            {
                fmMessage.Show("委托参数错误", "请输入交易股票数量");
                return;
            }

            Order order = new OrderImpl();
            order.Symbol = _symbol.Symbol;
            order.Size = Math.Abs((int)size.Value);
            order.Side = _side;
            order.LimitPrice = price.Value;

            string msg = "以价格:{0} {1}{2}股票:{3}".Put(order.LimitPrice.ToFormatStr(), order.Side ? "买入" : "卖出", order.UnsignedSize, _symbol.GetName());
            if (fmConfirm.Show("确认提交委托?", msg) == DialogResult.Yes)
            {
                _orderInesertId = CoreService.TLClient.ReqOrderInsert(order);
                btnSubmit.Enabled = false;
            }
            
        }

       

        

        /// <summary>
        /// 响应实时行情
        /// </summary>
        /// <param name="obj"></param>
        void EventIndicator_GotTickEvent(Tick obj)
        {
            if (obj == null) return;
            if (_symbol == null || _symbol.Symbol != obj.Symbol) return;
            AdjustInputControl();
            
        }

        /// <summary>
        /// 响应输入合约事件
        /// 1.如果合约在缓存中可以查找到 则触发合约选中事件
        /// 2.如果合约无法在缓存中找到 则提交服务器查询 如果回报获得合约则触发合约选择事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void EventQry_OnRspXQrySymbolResponse(Symbol arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryid) return;
            logger.Info("got response of symbol from server");
            if (arg1 != null)
            {
                CoreService.EventUI.FireSymbolSelectedEvent(this, arg1);      
            }
            if (arg4)
            {
                _qryid = 0;
            }
        }


        Symbol _symbol = null;
        bool _inputControlAdjuestd = false;
        /// <summary>
        /// 响应合约选择事件
        /// 设置合约名称 并设置当前价格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventUI_OnSymbolSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 == null)
            {
                _symbol = null;
                lbSymbolName.Text = "--";
                lbMoneyAvabile.Text = "0";
                price.Maximum = 0;
                price.Minimum = 0;
                price.DecimalPlaces = 0;
                price.Increment = 0;
                price.Value = 0;
                btnSubmit.Enabled = false;
                return;
            }
            if (_symbol == null || _symbol.Symbol != arg2.Symbol)
            {
                _symbol = arg2;
                _inputControlAdjuestd = false;

                AdjustInputControl();
            }
        }

        void AdjustInputControl()
        {
            if (_symbol == null) return;
            if (_inputControlAdjuestd) return;
            lbSymbolName.Text = _symbol.GetName();
            Tick k = CoreService.TradingInfoTracker.TickTracker[_symbol.Symbol];
            if (k != null)
            {
                price.Maximum = k.UpperLimit;
                price.Minimum = k.LowerLimit;
                price.DecimalPlaces = _symbol.SecurityFamily.GetDecimalPlaces();
                price.Increment = _symbol.SecurityFamily.PriceTick;
                price.Value = _side ? k.AskPrice : k.BidPrice;

                //查询最大可开委托数量
                QryMaxOrderVol();

                _inputControlAdjuestd = true;
                btnSubmit.Enabled = true;
            }
            
        }

        int qryMaxOrderId = 0;
        void QryMaxOrderVol()
        {
            if (_symbol == null) return;
            qryMaxOrderId = CoreService.TLClient.ReqQryMaxOrderVol(_symbol.Symbol);
        }

        void EventOther_OnRspQryMaxOrderVolResponse(RspQryMaxOrderVolResponse obj)
        {
            if (obj.RequestID != qryMaxOrderId) return;
            lbMoneyAvabile.Text = obj.MaxVol.ToString();
        }


        /// <summary>
        /// 判定是否需要查找合约
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool NeedSearchSymbol(string symbol)
        {
            if (symbol.Length == 6) return true;
            return false;

        }

        int _qryid = 0;
        void symbol_TextChanged(object sender, EventArgs e)
        {
            logger.Info(string.Format("Symbol changed:{0}", symbol.Text));

            bool needsearch = NeedSearchSymbol(symbol.Text);
            if (needsearch)
            {
                Symbol sym = CoreService.BasicInfoTracker.GetSymbol(symbol.Text);
                if (sym == null)
                {
                    if (_qryid == 0)
                    {
                        logger.Info(string.Format("Symbol:{0} do not exist in cache, will qry from server", symbol.Text));
                        //logger.Info(string.Format("qry symbol:{0} from server", sym.Symbol));
                        _qryid = CoreService.TLClient.ReqXQrySymbol(symbol.Text);
                    }
                }
                else
                {
                    //触发合约选择事件
                    CoreService.EventUI.FireSymbolSelectedEvent(this, sym);
                }
            }
            else
            {
                if (_symbol != null)//如果当前选择的合约不为空 则出发选空 进行重置，边界出发
                {
                    CoreService.EventUI.FireSymbolSelectedEvent(this, null);
                }
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle = 0x20;
        //        return cp;
        //    }
        //}  


        [DefaultValue(true)]
        bool _side = true;
        public bool Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
                //kryptonLabel1.Text = _side ? "买入股票" : "卖出股票";
                //kryptonLabel6.Text = _side ? "可买(股):" : "可卖(股):";
                //kryptonLabel8.Text = _side ? "买入数量:" : "买出数量:";
                //btnSubmit.Text = _side ? "买 入" : "卖 出";

                //kryptonLabel1.StateCommon.ShortText.Color1 = LabelColor;
                //kryptonLabel2.StateCommon.ShortText.Color1 = LabelColor;
                //kryptonLabel3.StateCommon.ShortText.Color1 = LabelColor;
                ////label4.ForeColor = LabelColor;
                //kryptonLabel5.StateCommon.ShortText.Color1 = LabelColor;
                //kryptonLabel6.StateCommon.ShortText.Color1 = LabelColor;
                ////label7.ForeColor = LabelColor;
                //kryptonLabel8.StateCommon.ShortText.Color1 = LabelColor;

                
                _inputControlAdjuestd = false;
                AdjustInputControl();
                Invalidate();
            }
        }

        Color LabelColor { get { return _side ? UIConstant.LongLabelColor : UIConstant.ShortLabelColor; } }
    }
}
