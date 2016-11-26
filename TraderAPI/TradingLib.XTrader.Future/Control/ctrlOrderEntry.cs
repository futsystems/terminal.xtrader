using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlOrderEntry : UserControl, TradingLib.API.IEventBinder
    {
        ILog logger = LogManager.GetLogger("OrderEntry");

        ctrlListBox priceBox;
        ctrlNumBox sizeBox;

        QSEnumOffsetFlag _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;

        Symbol _symbol = null;
        /// <summary>
        /// 当前选中合约
        /// </summary>
        public Symbol SymbolSelected { get { return _symbol; } }
        public ctrlOrderEntry()
        {
            InitializeComponent();

            InitControl();

            WireEvent();

        }

        void WireEvent()
        {

            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

            //合约选择控件
            inputSymbol.SymbolSelected += new Action<Symbol>(inputSymbol_SymbolSelected);//下拉选择
            inputSymbol.TextChanged += new EventHandler(inputSymbol_TextChanged);//合约输入
            //inputSymbol.DropDown += new EventHandler(inputSymbol_DropDown);
            inputSymbol.SecuritySelected += new Action(inputSymbol_SecuritySelected);
            
            inputFlagClose.Click += new EventHandler(inputFlagClose_Click);
            inputFlagCloseToday.Click += new EventHandler(inputFlagCloseToday_Click);
            inputFlagOpen.Click += new EventHandler(inputFlagOpen_Click);
            inputFlagAuto.CheckedChanged += new EventHandler(inputFlagAuto_CheckedChanged);

            inputPrice.NumTxtValSelected += new Action<string>(inputPrice_NumTxtValSelected);
            inputPrice.ValueChanged += new Action<decimal>(inputPrice_ValueChanged);

            btnBuy.Click += new EventHandler(btnBuy_Click);
            btnSell.Click += new EventHandler(btnSell_Click);

            btnQryMaxVol.Click += new EventHandler(btnQryMaxVol_Click);
            btnReset.Click += new EventHandler(btnReset_Click);

            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(EventIndicator_GotTickEvent);//响应实时行情
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, Symbol>(EventUI_OnSymbolSelectedEvent);
            CoreService.EventUI.OnPositionSelectedEvent += new Action<object, Position>(EventUI_OnPositionSelectedEvent);
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(EventIndicator_GotFillEvent);
            CoreService.EventIndicator.GotErrorOrderEvent += new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent);

            CoreService.EventQry.OnRspXQryMaxOrderVolResponse +=new Action<RspXQryMaxOrderVolResponse>(EventQry_OnRspXQryMaxOrderVolResponse);
            CoreService.EventCore.RegIEventHandler(this);
        }

        void btnQryMaxVol_Click(object sender, EventArgs e)
        {
            QryMaxOrderVol();
        }

        

        void btnReset_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

       
        

        /// <summary>
        /// 价格输入框价格变化后 更新按钮价格
        /// </summary>
        /// <param name="obj"></param>
        void inputPrice_ValueChanged(decimal obj)
        {
            UpdatePriceButton();
        }

        /// <summary>
        /// 取价模式切换 重新更新按钮当前对应显示价格
        /// 同时需要根据当前设定的取价模式 设定上下价格。用于上下调节按钮进行调节
        /// </summary>
        /// <param name="obj"></param>
        void inputPrice_NumTxtValSelected(string obj)
        {
            UpdatePriceButton();
        }

        /// <summary>
        /// 下拉选择品种后 重置价格输入控件
        /// </summary>
        void inputSymbol_SecuritySelected()
        {
            this.ResetInputPrice();
            this.ResetPriceButton();
        }

        


        /// <summary>
        /// 响应实时行情
        /// </summary>
        /// <param name="obj"></param>
        void EventIndicator_GotTickEvent(Tick obj)
        {
            if (obj == null) return;
            if (_symbol == null || _symbol.Symbol != obj.Symbol || _symbol.Exchange != obj.Exchange) return;

            if(obj.UpdateType=="X" || obj.UpdateType == "Q" || obj.UpdateType == "S")
            {
                UpdatePriceButton();
            }
        }




        string _priceFormat = "{0:F2}";
        void EventUI_OnSymbolSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 != null)// && (_symbol == null || _symbol.Symbol != arg2.Symbol))
            {

                _symbol = arg2;
                _priceFormat = _symbol.SecurityFamily.GetPriceFormat();
                
                //合约选择框文字
                if (!inputSymbol.Text.StartsWith(_symbol.Symbol))
                {
                    inputSymbol.Text = _symbol.Symbol;
                }

                //下单按钮可以显示盘口价格
                btnBuy.IsPriceOn = true;
                btnSell.IsPriceOn = true;

                //设定priceInput的 相关参数
                inputPrice.PriceFormat = _symbol.SecurityFamily.GetPriceFormat();
                inputPrice.Increment = _symbol.SecurityFamily.PriceTick;
                inputPrice.SymbolSelected = true;

                //重置输入控件
                ResetInput();
            }
        }



        bool _positionSelected = false;
        Position _position = null;
        /// <summary>
        /// 持仓选择事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventUI_OnPositionSelectedEvent(object arg1, Position arg2)
        {
            ResetInput();

            if (arg2.isLong)
            {
                btnBuy.Enabled = false;
                if (!_autoflag)
                {
                    inputFlagClose.Checked = true;
                    _currentOffsetFlag = QSEnumOffsetFlag.CLOSE;
                }
                inputSize.SetValue(Math.Abs(arg2.FlatSize).ToString());
            }
            else
            {
                btnSell.Enabled = false;
                if (!_autoflag)
                {
                    inputFlagClose.Checked = true;
                    _currentOffsetFlag = QSEnumOffsetFlag.CLOSE;
                }
                inputSize.SetValue(Math.Abs(arg2.FlatSize).ToString());
            }
            _positionSelected = true;
            _position = arg2;
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
                    btnBuy.Enabled = true;
                    btnSell.Enabled = true;
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
                    btnBuy.Enabled = true;
                    btnSell.Enabled = true;
                }

                //委托处于Open状态 则查询一次财务信息与可开
                if (obj.Status == QSEnumOrderStatus.Opened || obj.Status == QSEnumOrderStatus.Canceled)
                {
                    QryMaxOrderVol();
                    QryAccountFinance();
                }
                
            }
        }
        /// <summary>
        /// 获得成交回报后查询财务信息
        /// </summary>
        /// <param name="obj"></param>
        void EventIndicator_GotFillEvent(Trade obj)
        {
            if (_positionSelected && _position!= null)
            {
                if (obj.Symbol == _position.Symbol)
                {
                    ResetInput(false);
                }
            }
            else
            {
                QryMaxOrderVol();
                QryAccountFinance();
            }
            
        }

        

        void InitControl()
        {
            if (Constants.HedgeFieldVisible)
            {
                inputArbFlag.Items.Add("套保");
            }
            inputArbFlag.Items.Add("投机");
            inputArbFlag.SelectedIndex = 0;
            //inputArbFlag.DrawMode = DrawMode.OwnerDrawVariable;
            //inputArbFlag.ItemHeight = 16;
            //inputArbFlag.IntegralHeight = false;
            inputSymbol.DropDownSizeMode = SizeMode.UseControlSize;

            ListBox f = new ListBox();
            priceBox = new ctrlListBox();
            priceBox.ItemSelected += new Action<string>(box_ItemSelected);
            priceBox.Height = 80;
            priceBox.Width = 0;//默认使用DropDownContrl尺寸,设置Width为0后 则使用触发控件的Width

            priceBox.Items.Add("对手价");
            priceBox.Items.Add("对手价超一");
            priceBox.Items.Add("对手价超二");
            priceBox.Items.Add("挂单价");
            priceBox.Items.Add("最新价");
            priceBox.Items.Add("市价");
            //priceBox.Items.Add("涨停价");
            //priceBox.Items.Add("跌停价");

            inputPrice.DropDownSizeMode = SizeMode.UseControlSize;
            inputPrice.DropDownControl = priceBox;

            sizeBox = new ctrlNumBox();
            sizeBox.NumSelected += new Action<int>(sizeBox_NumSelected);
            inputSize.DropDownControl = sizeBox;
            inputSize.DropDownSizeMode = SizeMode.UseControlSize;

            ResetInputPrice();
            ResetPriceButton();

            if (TraderConfig.ExFlagAuto)
            {
                _autoflag = true;
                inputFlagAuto.Checked = true;
                _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;//服务端自动判定
                inputFlagClose.Enabled = false;
                inputFlagOpen.Enabled = false;
                inputFlagCloseToday.Enabled = false;
                inputFlagClose.Checked = false;
                inputFlagOpen.Checked = false;
                inputFlagCloseToday.Checked = false;
            }
            else
            {
                _autoflag = false;
                inputFlagOpen.Checked = true;
                _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
            }
        }

        #region 合约选择控件 选择合约
        void inputSymbol_TextChanged(object sender, EventArgs e)
        {
            string s = inputSymbol.Text;
            if (inputSymbol.Text.Contains("|"))
            {
                s = inputSymbol.Text.Split('|')[0];
                
            }
            Symbol symbol = CoreService.BasicInfoTracker.Symbols.Where(sym => sym.Symbol == s).FirstOrDefault();
            if (symbol != null)
            {
                logger.Info(string.Format("Symbol:{0} Selected", symbol.Symbol));
                CoreService.EventUI.FireSymbolSelectedEvent(this, symbol);
            }
            else //当前没有选中任何合约
            {

                ClearSymbol();
            }
            
        }

        void inputSymbol_SymbolSelected(Symbol obj)
        {
            logger.Info(string.Format("Symbol:{0} Selected", obj.Symbol));
            CoreService.EventUI.FireSymbolSelectedEvent(this, obj);
        }

        /// <summary>
        /// 清空选中的合约
        /// </summary>
        public void ClearSymbol()
        {
            logger.Info("clear selected symbol");
            _symbol = null;

            inputPrice.SymbolSelected = false;
            ResetPriceButton();
            ResetInput();

            lbLongOpenVol.Visible = false;
            lbLongCloseVol.Visible = false;
            lbShortOpenVol.Visible = false;
            lbShortCloseVol.Visible = false;

            inputSymbol.Text = string.Empty;

        }
        #endregion




        void sizeBox_NumSelected(int obj)
        {
            inputSize.SetValue(obj.ToString());
            inputSize.HideDropDown();
        }

        void box_ItemSelected(string obj)
        {
            inputPrice.SetTxtVal(obj);
            inputPrice.HideDropDown();
        }
        /// <summary>
        /// 禁止切换Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = true;
        }


        void ResetInputPrice()
        {
            inputPrice.SetTxtVal("对手价");
        }

        void ResetPriceButton()
        {
            btnBuy.IsPriceOn = false;
            btnSell.IsPriceOn = false;
            btnBuy.PriceStr = string.Empty;
            btnSell.PriceStr = string.Empty;
        }

        void ResetInput(bool resetprice = true)
        {
            logger.Info("重置输入按钮");
            _positionSelected = false;
            btnBuy.Enabled = true;
            btnSell.Enabled = true;
            inputSize.SetValue("1");
            if (!_autoflag)
            {
                _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
                inputFlagOpen.Checked = true;
            }
            if (resetprice)
            {
                inputPrice.SetTxtVal("对手价");
            }

            inputArbFlag.SelectedIndex = 0;

            if (_symbol != null)
            {
                QryMaxOrderVol();
            }
            //查询账户财务信息
            QryAccountFinance();
        }

        

        /// <summary>
        /// 更新价格信息
        /// </summary>
        /// <param name="tick"></param>
        void UpdatePriceButton()
        {
            if (_symbol == null) return;
            if (inputPrice.IsTxtMode)
            {
                Tick snapshot = CoreService.TradingInfoTracker.TickTracker[_symbol.Exchange, _symbol.Symbol];
                if (snapshot == null) return;
                switch (inputPrice.TxtValue)
                {
                    case "对手价":
                        {
                            btnBuy.PriceStr = string.Format(_priceFormat, snapshot.AskPrice);
                            btnSell.PriceStr = string.Format(_priceFormat, snapshot.BidPrice);
                            inputPrice.SetBenchPrice(snapshot.AskPrice, snapshot.BidPrice);
                            return;
                        }
                    case "对手价超一":
                        {
                            btnBuy.PriceStr = string.Format(_priceFormat, snapshot.AskPrice + _symbol.SecurityFamily.PriceTick);
                            btnSell.PriceStr = string.Format(_priceFormat, snapshot.BidPrice - _symbol.SecurityFamily.PriceTick);
                            inputPrice.SetBenchPrice(snapshot.AskPrice + _symbol.SecurityFamily.PriceTick, snapshot.BidPrice - _symbol.SecurityFamily.PriceTick);
                            return;
                        }
                    case "对手价超二":
                        {
                            btnBuy.PriceStr = string.Format(_priceFormat, snapshot.AskPrice + 2 * _symbol.SecurityFamily.PriceTick);
                            btnSell.PriceStr = string.Format(_priceFormat, snapshot.BidPrice - 2 * _symbol.SecurityFamily.PriceTick);
                            inputPrice.SetBenchPrice(snapshot.AskPrice + 2*_symbol.SecurityFamily.PriceTick, snapshot.BidPrice - 2*_symbol.SecurityFamily.PriceTick);
                            return;
                        }
                    case "挂单价":
                        {
                            btnBuy.PriceStr = string.Format(_priceFormat, snapshot.BidPrice);
                            btnSell.PriceStr = string.Format(_priceFormat, snapshot.AskPrice);
                            inputPrice.SetBenchPrice(snapshot.AskPrice, snapshot.BidPrice);
                            return;
                        }
                    case "最新价":
                        {
                            btnBuy.PriceStr = string.Format(_priceFormat, snapshot.Trade);
                            btnSell.PriceStr = string.Format(_priceFormat, snapshot.Trade);
                            inputPrice.SetBenchPrice(snapshot.Trade, snapshot.Trade);
                            return;
                        }
                    case "市价":
                        {
                            btnBuy.PriceStr = string.Empty;
                            btnSell.PriceStr = string.Empty;
                            inputPrice.SetBenchPrice(-1,-1);
                            return;
                        }
                    //case "涨停价":
                    //    {
                    //        price = snapshot.UpperLimit;
                    //        return true;
                    //    }
                    //case "跌停价":
                    //    {
                    //        price = snapshot.LowerLimit;
                    //        return true;
                    //    }
                    default:
                        return;
                }
            }
            else
            {
                decimal price = 0;
                //根据输入的价格返回
                if (!decimal.TryParse(inputPrice.TxtValue, out price)) price = 0;

                btnBuy.PriceStr = string.Format(_priceFormat, price);
                btnSell.PriceStr = string.Format(_priceFormat, price);
                return;
            }
        }

        #region 提交委托
        int _orderInesertId = 0;

        void btnSell_Click(object sender, EventArgs e)
        {
            SendOrder(false);
            //发送平仓委托后 自动切换回开仓状态
            if (TraderConfig.ExSwitchToOpenWhenCloseOrderSubmit)
            {
                if (!_autoflag && _currentOffsetFlag != QSEnumOffsetFlag.OPEN)
                {
                    _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
                    inputFlagOpen.Checked = true;
                }
            }

        }

        void btnBuy_Click(object sender, EventArgs e)
        {
            SendOrder(true);
            if (TraderConfig.ExSwitchToOpenWhenCloseOrderSubmit)
            {
                if (!_autoflag && _currentOffsetFlag != QSEnumOffsetFlag.OPEN)
                {
                    _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
                    inputFlagOpen.Checked = true;
                }
            }
        }

        /// <summary>
        /// 从加入输入控件获得价格
        /// </summary>
        /// <param name="side"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        bool GetInputPrice(bool side,out decimal price)
        {
            price = 0;
            
            if (inputPrice.IsTxtMode)
            {
                Tick snapshot = CoreService.TradingInfoTracker.TickTracker[_symbol.Exchange, _symbol.Symbol];
                if (snapshot == null) return false;
                switch (inputPrice.TxtValue)
                {
                    case "对手价":
                        {
                            price = side ? snapshot.AskPrice : snapshot.BidPrice;
                            return true;
                        }
                    case "对手价超一":
                        {
                            price = side ? (snapshot.AskPrice + _symbol.SecurityFamily.PriceTick) : (snapshot.BidPrice - _symbol.SecurityFamily.PriceTick);
                            return true;
                        }
                    case "对手价超二":
                        {
                            price = side ? (snapshot.AskPrice + 2 * _symbol.SecurityFamily.PriceTick) : (snapshot.BidPrice - 2 * _symbol.SecurityFamily.PriceTick);
                            return true;
                        }
                    case "挂单价":
                        {
                            price = side ? snapshot.BidPrice : snapshot.AskPrice;
                            return true;
                        }
                    case "最新价":
                        {
                            price = snapshot.Trade;
                            return true;
                        }
                    case "市价":
                        {
                            price = 0;
                            return true;
                        }
                    case "涨停价":
                        {
                            price = snapshot.UpperLimit;
                            return true;
                        }
                    case "跌停价":
                        {
                            price = snapshot.LowerLimit;
                            return true;
                        }
                    default:
                        return false;
                }
            }
            else
            {
                //根据输入的价格返回
                if (!decimal.TryParse(inputPrice.TxtValue, out price)) return false;
                return true;
            }
        }

        string GetOffsetString()
        {
            switch (_currentOffsetFlag)
            {
                case QSEnumOffsetFlag.OPEN: return "开仓";
                case QSEnumOffsetFlag.CLOSE: return "平仓";
                case QSEnumOffsetFlag.CLOSETODAY: return "平今";
                default:
                    return "";
            }
        }
        string GetPriceString(decimal price)
        {
            if (price == 0) return "市价";
            return "限价:" + price.ToFormatStr(_symbol.SecurityFamily.GetPriceFormat());
        }
        void SendOrder(bool side)
        {
            if (_symbol == null)
            {
                MessageBox.Show("请选择交易合约", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int size = 0;
            if (!int.TryParse(inputSize.TxtValue, out size)) size = 0;
            if (size == 0)
            {
                MessageBox.Show("委托数量需大于零", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            

            decimal price=0;
            bool priceok = GetInputPrice(side, out price);
            if (!priceok)
            {
                MessageBox.Show("委托价格异常", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Order order = new OrderImpl();
            order.Symbol = _symbol.Symbol;
            order.Exchange = _symbol.Exchange;

            order.Size = size;
            order.Side = side;
            order.LimitPrice = price;
            order.OffsetFlag = _currentOffsetFlag;

            //一键下单
            if (TraderConfig.ExSendOrderDirect)
            {
                _orderInesertId = CoreService.TLClient.ReqOrderInsert(order);
                btnBuy.Enabled = false;
                btnSell.Enabled = false;
            }
            else
            {
                //以价格:00 买入1手 
                string msg = "以价格:{0} {1}{4}{2}手 {3}".Put(GetPriceString(order.LimitPrice), order.Side ? "买入" : "卖出", order.UnsignedSize, _symbol.GetName(), GetOffsetString());
                if (MessageBox.Show(msg, "确认提交委托?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _orderInesertId = CoreService.TLClient.ReqOrderInsert(order);
                    btnBuy.Enabled = false;
                    btnSell.Enabled = false;
                }
            }
        }
        #endregion


        #region OrderOffsetFlag
        bool _autoflag = false;
        void inputFlagAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (inputFlagAuto.Checked)
            {
                _autoflag = true;
                _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;//服务端自动判定
                inputFlagClose.Enabled = false;
                inputFlagOpen.Enabled = false;
                inputFlagCloseToday.Enabled = false;
                inputFlagClose.Checked = false;
                inputFlagOpen.Checked = false;
                inputFlagCloseToday.Checked = false;

            }
            else
            {
                _autoflag = false;
                _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
                inputFlagClose.Enabled = true;
                inputFlagOpen.Enabled = true;
                inputFlagCloseToday.Enabled = true;
                inputFlagClose.Checked = false;
                inputFlagOpen.Checked = true;
                inputFlagCloseToday.Checked = false;
            }
        }
        void inputFlagOpen_Click(object sender, EventArgs e)
        {
            _autoflag = false;
            _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
        }

        void inputFlagCloseToday_Click(object sender, EventArgs e)
        {
            _autoflag = false;
            _currentOffsetFlag = QSEnumOffsetFlag.CLOSETODAY;
        }

        void inputFlagClose_Click(object sender, EventArgs e)
        {
            _autoflag = false;
            _currentOffsetFlag = QSEnumOffsetFlag.CLOSE;
        }
        #endregion




        #region 可开与账户财务查询
        //获得某个持仓的可平数量
        int GetCanFlatSize(Position pos)
        {
            OrderTracker ot = CoreService.TradingInfoTracker.OrderTracker;
            return pos.isFlat ? 0 : (pos.UnsignedSize - ot.GetPendingExitSize(pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false));
        }
        /// <summary>
        /// 查询最大交易数量
        /// </summary>
        void QryMaxOrderVol()
        {
            if (_symbol == null) return;
            
            lbLongOpenVol.Visible = false;
            lbShortOpenVol.Visible = false;
            System.Threading.Thread.Sleep(100);

            CoreService.TLClient.ReqXQryMaxOrderVol(_symbol.Exchange, _symbol.Symbol, true, QSEnumOffsetFlag.OPEN);
            CoreService.TLClient.ReqXQryMaxOrderVol(_symbol.Exchange, _symbol.Symbol, false, QSEnumOffsetFlag.OPEN);
        }
        void EventQry_OnRspXQryMaxOrderVolResponse(RspXQryMaxOrderVolResponse obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RspXQryMaxOrderVolResponse>(EventQry_OnRspXQryMaxOrderVolResponse), new object[] { obj });
            }
            else
            {
                if(obj.Symbol == _symbol.Symbol)
                {

                    string accout = CoreService.TradingInfoTracker.Account.Account;
                    if (obj.Side)
                    {
                        if (lbLongOpenVol.Visible == false)
                        {
                            lbLongOpenVol.Visible = true;
                            lbLongCloseVol.Visible = true;
                        }

                        Position pos = CoreService.TradingInfoTracker.PositionTracker[obj.Symbol,accout, obj.Side];
                        lbLongOpenVol.Text = string.Format("可开<={0}", obj.MaxVol);
                        lbLongCloseVol.Text = string.Format("可平 {0}",pos == null ? 0 : GetCanFlatSize(pos));
                    }
                    else
                    {
                        if (lbShortOpenVol.Visible == false)
                        {
                            lbShortOpenVol.Visible = true;
                            lbShortCloseVol.Visible = true;
                        }
                        Position pos = CoreService.TradingInfoTracker.PositionTracker[obj.Symbol,accout, obj.Side];
                        lbShortOpenVol.Text = string.Format("可开<={0}", obj.MaxVol);
                        lbShortCloseVol.Text = string.Format("可平 {0}",pos == null ? 0 : GetCanFlatSize(pos));
                    }
                }
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

        #endregion






        #region IEventBinder
        public void OnInit()
        {

            //初始化合约选择控件
            foreach (var g in CoreService.BasicInfoTracker.Symbols.GroupBy(sym => sym.SecurityFamily))
            {
                SymbolSet symbolset = new SymbolSet(string.Format("{0}   {1}", g.Key.Code, g.Key.Name));
                foreach (var symbol in g.OrderBy(sym => sym.Month))
                {
                    symbolset.AddSymbol(symbol);
                }
                inputSymbol.AddSymbolSet(symbolset);
            }
            inputSymbol.InitListBox();


        }

        public void OnDisposed()
        {

        }

        #endregion

    }
}
