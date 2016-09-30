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
    public partial class ctOrderSenderSTK : UserControl
    {
        Font TITLEFONT = new Font("宋体", 14, FontStyle.Bold);

        ILog logger = LogManager.GetLogger("ctOrderSenderSTK");
        public ctOrderSenderSTK()
        {
            InitializeComponent();
            this.Side = true;


            cbStockAccount.Items.Add("沪A-00001");
            cbStockAccount.Items.Add("深A-00001");
            cbStockAccount.SelectedIndex = 0;

            WireEvent();

        }

        void WireEvent()
        {
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(EventIndicator_GotTickEvent);//响应实时行情
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolSelectedEvent);//响应合约选择
            CoreService.EventQry.OnRspXQrySymbolResponse += new Action<Symbol, RspInfo, int, bool>(EventQry_OnRspXQrySymbolResponse);//响应合约查询回报
            CoreService.EventQry.OnRspXQryMaxOrderVolResponse += new Action<RspXQryMaxOrderVolResponse>(EventOther_OnRspXQryMaxOrderVolResponse);//响应最大下单量回报
            CoreService.EventQry.OnRspXQryAccountFinanceEvent += new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent);
            //用于检查委托提交返回 并设置界面提交委托按钮有效 提交委托后 需要等对应委托回报到达后才可以再次提交委托 避免多次提交产生错误
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotErrorOrderEvent += new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(EventIndicator_GotFillEvent);

            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);

            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            symbol.TextChanged += new EventHandler(symbol_TextChanged);
            
        }

       

       

        string  GetExchangeSelected()
        {
            if (cbStockAccount.SelectedIndex == 0) return "SSE";
            if (cbStockAccount.SelectedIndex == 1) return "SZE";
            return "";
        }


        void EventOther_OnResumeDataEnd()
        {
            btnSubmit.Enabled = true;
        }

        void EventOther_OnResumeDataStart()
        {
            btnSubmit.Enabled = false;
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

                if (obj.Status == QSEnumOrderStatus.Opened)
                {
                    QryMaxOrderVol();
                    QryAccountFinance();
                }
            }
        }

        /// <summary>
        /// 委托成交 发生资金变化重新查询最大可下单数量和账户财务信息
        /// </summary>
        /// <param name="obj"></param>
        void EventIndicator_GotFillEvent(Trade obj)
        {
            QryMaxOrderVol();
            QryAccountFinance();
        }



        int _orderInesertId = 0;
        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_symbol == null)
            {
                MessageBox.Show("请输入交易股票代码","委托参数错误",MessageBoxButtons.OK,MessageBoxIcon.Warning );
                return;
            }
            if (size.Value == 0)
            {
                MessageBox.Show("请输入交易股票数量", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Order order = new OrderImpl();
            order.Symbol = _symbol.Symbol;
            order.Exchange = _symbol.Exchange;

            order.Size = Math.Abs((int)size.Value);
            order.Side = _side;
            order.LimitPrice = price.Value;

            string msg = "以价格:{0} {1}{2}股票:{3}".Put(order.LimitPrice.ToFormatStr(), order.Side ? "买入" : "卖出", order.UnsignedSize, _symbol.GetName());
            if (MessageBox.Show(msg, "确认提交委托?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
            if (_symbol == null || _symbol.Symbol != obj.Symbol || _symbol.Exchange != obj.Exchange) return;

            //如果价格为0 则根据方向自动填写当前最优盘口价格
            if (price.Value == 0)
            {
                price.Value = _side ? obj.AskPrice : obj.BidPrice;
            }
            
        }

        


        Symbol _symbol = null;
        /// <summary>
        /// 响应合约选择事件
        /// 设置合约名称 并设置当前价格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventUI_OnSymbolSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2!=null && (_symbol == null || _symbol.Symbol != arg2.Symbol))
            {

                _symbol = arg2;
                lbSymbolName.Text = _symbol.GetName();
                SetSymbol(arg2.Exchange, arg2.Symbol,false);//1.合约输入框输入代码 触发自动查询并返回合约 2.行情联动直接设定下单面板合约(需要执行查询) 3.持仓面板双击持仓 设定下单面板合约

                Tick k = CoreService.TradingInfoTracker.TickTracker[_symbol.Exchange,_symbol.Symbol];
                if (k != null)
                {
                    price.Value = _side ? k.AskPrice : k.BidPrice;
                }

                btnSubmit.Enabled = true;
                //查询最大可开委托数量
                QryMaxOrderVol();
                //查询可用资金
                QryAccountFinance();
            }
        }

       
        


        int qryMaxOrderId = 0;
        /// <summary>
        /// 查询最大交易数量
        /// </summary>
        void QryMaxOrderVol()
        {
            if (_symbol == null) return;
            qryMaxOrderId = CoreService.TLClient.ReqXQryMaxOrderVol(_symbol.Exchange,_symbol.Symbol,_side);
        }
        void EventOther_OnRspXQryMaxOrderVolResponse(RspXQryMaxOrderVolResponse obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RspXQryMaxOrderVolResponse>(EventOther_OnRspXQryMaxOrderVolResponse), new object[] { obj });
            }
            else
            {
                if (obj.RequestID != qryMaxOrderId) return;
                lbMaxOrderVol.Text = obj.MaxVol.ToString();
            }
        }


        int qryAccountFinanceId = 0;
        /// <summary>
        /// 查询交易账户
        /// </summary>
        void QryAccountFinance()
        {
            qryAccountFinanceId = CoreService.TLClient.ReqXQryAccountFinance();
        }
        void EventQry_OnRspXQryAccountFinanceEvent(RspXQryAccountFinanceResponse response)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent), new object[] { response });
            }
            else
            {
                if (response.RequestID != qryAccountFinanceId) return;
                lbMoneyAvabile.Text = response.Report.StkAvabileFunds.ToFormatStr();
            }
        }




        

        bool _symChangedFired = true;
        /// <summary>
        /// 设定合约
        /// 行情买卖列双击 联动时需要设定当前下单面板的合约
        /// 双击持仓 响应合约选中时 设定当前下单面板的合约
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="sym"></param>
        public void SetSymbol(string exchange, string sym,bool symChangeFired = true)
        {
            _symChangedFired = symChangeFired;
            if (exchange == "SSE") cbStockAccount.SelectedIndex = 0;
            if (exchange == "SZE") cbStockAccount.SelectedIndex = 1;
            symbol.Text = sym;
            _symChangedFired = true;
        }

        
        /// <summary>
        /// 代码输入框文字变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void symbol_TextChanged(object sender, EventArgs e)
        {
            if (_symChangedFired)
            {
                bool needsearch = NeedSearchSymbol(symbol.Text);
                if (needsearch)
                {
                    string exchange = GetExchangeSelected();
                    TrySelectSymbol(exchange, symbol.Text);
                }
                else //置空合约
                {
                    CoreService.EventUI.FireSymbolSelectedEvent(this, null);
                    Reset();
                }
            }
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


        int _qrySymId = 0;
        /// <summary>
        /// 查询选择某个合约
        /// 1.缓存存在的合约 直接出发合约选择事件
        /// 2.缓存当前不存在的合约 向服务端请求查询合约
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        void TrySelectSymbol(string exchange, string symbol)
        {
            Symbol sym = CoreService.BasicInfoTracker.GetSymbol(exchange,symbol);
            if (sym == null)
            {
                if (_qrySymId == 0)
                {
                    logger.Info(string.Format("Symbol:{0} do not exist in cache, will qry from server", symbol));
                    _qrySymId = CoreService.TLClient.ReqXQrySymbol(exchange, symbol);
                }
            }
            else
            {
                //触发合约选择事件
                CoreService.EventUI.FireSymbolSelectedEvent(this, sym);
            }
        }
        /// <summary>
        /// 响应输入合约事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void EventQry_OnRspXQrySymbolResponse(Symbol arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qrySymId) return;
            if (arg1 != null)
            {
                CoreService.EventUI.FireSymbolSelectedEvent(this, arg1);
            }
            if (arg4)
            {
                _qrySymId = 0;
            }
        }



        void Reset()
        {
            _symbol = null;
            lbSymbolName.Text = "--";
            lbMoneyAvabile.Text = "0";
            lbMaxOrderVol.Text = "0";
            price.Value = 0;
            btnSubmit.Enabled = false;
        }



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
                lbPrice.Text = _side ? "买入价格" : "卖出价格";
                lbSize.Text = _side ? "买入数量" : "买出数量";
                lbMaxSize.Text = _side ? "最大可买" : "最大可卖";
                btnSubmit.Text = _side ? "买入下单" : "卖出下单";

                lbPrice.ForeColor = LabelColor;
                lbSize.ForeColor = LabelColor;
                lbMaxSize.ForeColor = LabelColor;
                btnSubmit.ForeColor = LabelColor;

                //查询最大可开委托数量
                QryMaxOrderVol();
                //查询可用资金
                QryAccountFinance();

                Invalidate();
            }
        }

        Color LabelColor { get { return _side ? UIConstant.LongLabelColor : UIConstant.ShortLabelColor; } }
    }
}
