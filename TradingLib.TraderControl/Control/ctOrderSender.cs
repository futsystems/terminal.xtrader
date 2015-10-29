using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.Primitives;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using System.Threading;
using Common.Logging;


namespace TradingLib.TraderControl
{
    public partial class ctOrderSender : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctOrderSender");
        const string PROGRAM = "ctOrderSender";

        string _dispdecpointformat = "N" + ((int)2).ToString();

        QSEnumOrderType _ordertype = QSEnumOrderType.Market;
        public QSEnumOrderType OrderType { get { return _ordertype; } set { _ordertype = value; } }

        bool _pricetouch = false;//价格框更新标识
        PriceFollow _pricefollow = PriceFollow.TRADE;

        bool ispostionmode = false;//是否处于持仓平仓模式


        Symbol _currentSymbol = null;
        /// <summary>
        /// 事件绑定
        /// 1.核心初始化事件 该事件通过RegIEventHandler进行接口注入绑定，当底层初始化完毕后 通过OnInit进行初始化
        /// 2.在load事件中 进行界面事件绑定，这样可以避免过早绑定事件然后在控件进行第一次显示时触发相关操作
        /// </summary>
        public ctOrderSender()
        {
            InitializeComponent();

            CoreService.EventCore.RegIEventHandler(this);

            this.Load +=new EventHandler(ctOrderSender_Load);

            isLossSet.Enabled = false;
            isProfitSet.Enabled = false;
            lossType.Enabled = false;
            profitType.Enabled = false;
            lossValue.Enabled = false;
            profitValue.Enabled = false;
        }

        bool load = false;
        private void ctOrderSender_Load(object sender, EventArgs e)
        {
            if (!load)
            {
                load = true;
                WireEvent();
                InitOrderType();
                InitOffsetType();

                //如果合约列表有合约 则设定默认合约
                if (cbSymbolList.SelectedItems.Count > 0)
                {
                    cbSymbolList_SelectedValueChanged(null, null);
                }
            }
        }


        void WireEvent()
        {
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotTrade);
            CoreService.EventIndicator.GotOrderEvent +=new Action<Order>(GotOrder);
            CoreService.EventUI.OnSymbolSelectedEvent += new Action<Object,Symbol>(EventUI_OnSymbolSelectedEvent);
            CoreService.EventOther.OnRspQryMaxOrderVolResponse += new Action<RspQryMaxOrderVolResponse>(EventOther_OnRspQryMaxOrderVolResponse);
            this.cbSymbolList.SelectedValueChanged += new EventHandler(cbSymbolList_SelectedValueChanged);

            
            this.btnBuy.Click +=new EventHandler(btnBuy_Click);
            this.btnSell.Click +=new EventHandler(btnSell_Click);
            this.btnQueryMaxVol.Click += new EventHandler(btnQueryMaxVol_Click);
        }

        void btnQueryMaxVol_Click(object sender, EventArgs e)
        {
            QryMaxOrderVal();
        }

        /// <summary>
        /// 查询最大可开数量
        /// </summary>
        /// <param name="obj"></param>
        void EventOther_OnRspQryMaxOrderVolResponse(RspQryMaxOrderVolResponse obj)
        {
            UpdateMaxOpenSize(obj.MaxVol);
        }

        /// <summary>
        /// 响应其他空间的合约选择事件
        /// </summary>
        /// <param name="obj"></param>
        void EventUI_OnSymbolSelectedEvent(Object sender,Symbol obj)
        {
            //如果是自己触发的事件 则直接返回
            if (sender == this) return;

            /////更新合约下拉列表
            if (cbSymbolList.SelectedValue != obj)
            {
                cbSymbolList.SelectedValue = obj;
            }

            

        }


        

        

        

        public void OnInit()
        { 
            //设定合约列表
            TraderHelper.AdapterToIDataSource(cbSymbolList).BindDataSource(GetSymbolCombList());
        }

        public void OnDisposed()
        { 
            
        }

        #region 辅助函数
        /// <summary>
        /// 当前选中的合约
        /// </summary>
        Symbol CurrentSymbol
        {
            get
            {
                return _currentSymbol;
            }
        }
        ArrayList GetSymbolCombList()
        {
            ArrayList list = new ArrayList();

            foreach (var sym in CoreService.BasicInfoTracker.Symbols)
            {
                ValueObject<Symbol> vo = new ValueObject<Symbol>();
                vo.Name = sym.SecurityFamily.Name +"-" + sym.Symbol;
                vo.Value = sym;
                list.Add(vo);
            }
            return list;
        }

        #endregion




        /*
        void SetUI()
        {
            askprice.ForeColor = UIGlobals.ShortSideColor;
            bidprice.ForeColor = UIGlobals.LongSideColor;
        }**/


        //初始化委托类型
        void InitOrderType()
        {
            ArrayList l = new ArrayList();
            ValueObject<QSEnumOrderType> vo1 = new ValueObject<QSEnumOrderType>();
            vo1.Name = "市价";
            vo1.Value = QSEnumOrderType.Market;
            l.Add(vo1);

            ValueObject<QSEnumOrderType> vo2 = new ValueObject<QSEnumOrderType>();
            vo2.Name = "限价";
            vo2.Value = QSEnumOrderType.Limit;
            l.Add(vo2);

            ValueObject<QSEnumOrderType> vo3 = new ValueObject<QSEnumOrderType>();
            vo3.Name = "Stop";
            vo3.Value = QSEnumOrderType.Stop;
            l.Add(vo3);

            orderType.DisplayMember = "Name";
            orderType.ValueMember = "Value";

            Factory.IDataSourceFactory(orderType).BindDataSource(l);
        }


        QSEnumOrderType CurrentOrderType {
            get {
                return (QSEnumOrderType)orderType.SelectedValue;
            }
        }



        #region 设定合约

        //当前合约symbol
        //string CurrentSymbol { get { if (_sec != null && _sec.isValid) { return _sec.Symbol; } else { return ""; } } }
        //当前合约对象
        //public Security CurrentSecurity { get { return _sec; } }
        //当前持仓
       // Position CurrentPosition { get { if (_sec != null && _sec.isValid) { return _pt[_sec.Symbol]; } else { return null; } } }

        /// <summary>
        /// 当默认合约列表发生变化时候,我们触发 用于更新下拉列表
        /// </summary>
        public void OnBasketChange()
        {
            //securityList.DataSource = null;
            ////securityList.Items.Clear();
            //ArrayList l = new ArrayList();
            
            //foreach (Security s in DefaultBasket)
            //{
            //    ValueObject<string> vo = new ValueObject<string>();
            //    vo.Name = s.Symbol + "  " + s.Description + "";
            //    vo.Value = s.Symbol;
            //    l.Add(vo);
            //    //m.Add(vo.Name);
            //}
            ////debug("binding security to here");
            ////MessageBox.Show("binding security to here:" + DefaultBasket.Count.ToString());
            //securityList.DisplayMember = "Name";
            //securityList.ValueMember = "Value";

            //Factory.IDataSourceFactory(securityList).BindDataSource(l);
            //securityList.DataSource = m;
        }


        

        /// <summary>
        /// 通过合约symbol设定合约
        /// 用于快捷设置合约
        /// </summary>
        /// <param name="symbol"></param>
        public void SetSecurity(string symbol)
        {
            //Security sec = DefaultBasket[symbol];
            //if (sec == null)
            //{
            //    MessageForm.Show("自选合约不存在");
            //    return;
            //}
            //SetSecurity(sec);
            //UpdatePosSize();//更新合约持仓
            //updateoffsettype();//更新止损 设置对应的默认值
        }

        /// <summary>
        /// 设定当前交易合约
        /// </summary>
        ///// <param name="sec"></param>
        //public void SetSecurity(Security sec)
        //{
        //    SetSecurity(sec, false);
        //}


        /// <summary>
        /// 行情面板双击合约 选定合约,合约列表下拉选择合约,持仓双击 选定合约
        /// 设定合约包含了一些底层的数据的处理
        /// </summary>
        /// <param name="sec"></param>
        void SetSecurity(Symbol sec, bool setposition = false)
        {
            ////Reset();
            //if (sec == null || !sec.isValid) return;
            ////设置合约,并调整price对应的参数
            //_sec = sec;
            

            ////生成对应的委托
            //work = new OrderImpl(_sec.Symbol, 0);
            //work.ex = _sec.DestEx;
            //work.LocalSymbol = _sec.Symbol;


            ////更新下拉列表的选中合约
            //if (securityList.SelectedValue != sec.Symbol)
            //    securityList.SelectedValue = sec.Symbol;

            //debug("Set security to here："+setposition.ToString());
            ////如果是平仓设定则不查询可开数量/选择合约，双击报价 则查询可开数量
            //if (!setposition)
            //    UpdateMaxOpenSize();

            ////默认手数为1，通过手工调节或者快速调节设定手数 查询默认设置,并设置手数
            //int s = GetSize(_sec.Symbol);//获得默认手数
            //UpdateSize(s);

            ////更新持仓信息
            //UpdatePositionSize();
            //UpdateUnRealizedPL();
            //UpdateRealizedPL();
            //UpdateCanFlat();
        }

        /// <summary>
        /// 设定合约
        /// </summary>
        /// <param name="sym"></param>
        void SetSymbol(Symbol sym)
        {
            logger.Debug("set symbol ~~~~~~~~~~~~");
            if (sym == null) return;
            //检查合约是否一致
            //if (sym.Symbol == _currentSymbol.Symbol) return;
            //设定当前合约
            _currentSymbol = sym;


            //设置界面显示参数
            _dispdecpointformat = TraderHelper.GetDisplayFormat(sym.SecurityFamily.PriceTick);//获得价格显示格式
            price.Increment = sym.SecurityFamily.PriceTick;
            price.DecimalPlaces = TraderHelper.GetDecimalPlace(sym.SecurityFamily.PriceTick);

            logger.Debug("price increment:" + price.Increment.ToString() + " decimalplaces:" + price.DecimalPlaces.ToString());
            lossValue.Increment = sym.SecurityFamily.PriceTick;
            lossValue.DecimalPlaces = TraderHelper.GetDecimalPlace(sym.SecurityFamily.PriceTick);

            profitValue.Increment = sym.SecurityFamily.PriceTick;
            profitValue.DecimalPlaces = TraderHelper.GetDecimalPlace(sym.SecurityFamily.PriceTick);

            //初始化合约行情快照
            Tick k = CoreService.TradingInfoTracker.TickTracker[_currentSymbol.Symbol];
            this.GotTick(k);

            ////更新持仓信息
            UpdatePositionSize();
            UpdateUnRealizedPL();
            UpdateRealizedPL();
            UpdateCanFlat();

            //查询可开手数
            QryMaxOrderVal();

        }

        


        #endregion



        #region 获得Trade Tick order cancel数据

        public void GotOrder(Order o)
        {
            if (this.InvokeRequired)
            {
                Invoke(new OrderDelegate(GotOrder), new object[] { o });
            }
            else
            {
                if(_currentSymbol == null) return;
                if (o.Symbol.Equals(CurrentSymbol.Symbol) && o.Status == QSEnumOrderStatus.Opened)
                {

                    UpdateCanFlat();
                }
                //{
                //    UpdateMaxOpenSize();
                //    UpdateCanFlat();
                //}
            }
            
        }
        //获得成交数据时候更新对应的持仓信息
        public void GotTrade(Trade t)
        {
            if (this.InvokeRequired)
            {
                Invoke(new FillDelegate(GotTrade), new object[] { t });
            }
            else
            {
                if (_currentSymbol == null) return;
                if (t.Symbol.Equals(CurrentSymbol.Symbol))
                {
                    UpdatePositionSize();
                    if (!t.IsEntryPosition)
                    {
                        UpdateRealizedPL();
                        UpdateCanFlat();
                    }
                }
                //if (t.symbol.Equals(CurrentSymbol))
                //{
                //    UpdateMaxOpenSize();
                //    UpdatePositionSize();
                //    if(t.PositionOperation == QSEnumPosOperation.DelPosition || t.PositionOperation == QSEnumPosOperation.ExitPosition || t.PositionOperation == QSEnumPosOperation.UNKNOWN)
                //        UpdateRealizedPL();
                //    UpdateCanFlat();
                //}
            }
        }
        //public void GotCancel(long id)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        Invoke(new LongDelegate(GotCancel), new object[] { id });
        //    }
        //    else
        //    {
        //        //Order o = _ot.SentOrder(id);
        //        //if (o != null && o.isValid && o.symbol.Equals(CurrentSymbol))
        //        //{
        //        //    UpdateMaxOpenSize();
        //        //    UpdateCanFlat();
        //        //}
        //    }
        //}

        Dictionary<string, string> secFromatMap = new Dictionary<string, string>();
        string GetDisplayFormat(string sym)
        {
            if (secFromatMap.Keys.Contains(sym))
            {
                return secFromatMap[sym];
            }
            else
            {
                Symbol symbol = CoreService.BasicInfoTracker.GetSymbol(sym);
                if (symbol == null) return UIConstant.DefaultDecimalFormat;
                string format = TraderHelper.GetDisplayFormat(symbol);
                secFromatMap.Add(sym, format);
                return format;
            }
        }


        public void GotTick(Tick tick)
        {
            if (this.InvokeRequired)
            {
                Invoke(new TickDelegate(GotTick), new object[] { tick });
            }
            else
            {
                //更新价格
                try
                {
                    if (_currentSymbol == null) return;
                    if (tick == null) return;
                    if (_currentSymbol.Symbol != tick.Symbol) return;

                    string format = GetDisplayFormat(tick.Symbol);


                    if (tick.hasAsk) askprice.Text = string.Format(format, tick.AskPrice) + " /" + tick.AskSize.ToString();
                    if (tick.hasBid) bidprice.Text = string.Format(format, tick.BidPrice) + " /" + tick.BidSize.ToString();
               
                    if (!_pricetouch)
                    {
                        switch (_pricefollow)
                        {
                            case PriceFollow.TRADE:
                                if (tick.isTrade) price.Value = tick.Trade;
                                break;
                            case PriceFollow.ASK:
                                if (tick.hasAsk) price.Value = tick.AskPrice;
                                break;
                            case PriceFollow.BID:
                                if (tick.hasBid) price.Value = tick.BidPrice;
                                break;
                            default:
                                break;
                        }
                    }

                    //更新当前持仓的浮动盈亏
                    UpdateUnRealizedPL();

                }
                catch (Exception e)
                {
                    logger.Error(e);
                }

            }


        }

        #endregion

        #region 更新持仓信息

        void UpdatePositionSize()
        {
            if (InvokeRequired)
                Invoke(new VoidDelegate(UpdatePositionSize), new object[] { });
            else
            {
                try
                {

                    if (_currentSymbol == null) return;
                    Position longside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol,CoreService.Account, true];
                    Position shortside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol,CoreService.Account, false];
                    posSize.Text = string.Format("{0}/{1}", longside.UnsignedSize, shortside.UnsignedSize);
                }
                catch(Exception ex)
                {
                    logger.Error("UpdatePositionSize error:" + ex.ToString());
                }
            }
        }
        void UpdateRealizedPL()
        {
            if (InvokeRequired)
                Invoke(new VoidDelegate(UpdateRealizedPL), new object[] { });
            else
            {
                try
                {
                    if (_currentSymbol == null) return;
                    Position longside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, true];
                    Position shortside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, false];
                    decimal realized = longside.ClosedPL + shortside.ClosedPL;
                    posRealizedPL.Text = string.Format(GetDisplayFormat(_currentSymbol.Symbol), realized);
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateRealizedPL error:" + ex.ToString());
                }
            }
        }
        void UpdateUnRealizedPL()
        {
            if (InvokeRequired)
                Invoke(new VoidDelegate(UpdateUnRealizedPL), new object[] { });
            else
            {
                try
                {
                    if (_currentSymbol == null) return;
                    Position longside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol,CoreService.Account, true];
                    Position shortside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account,false];
                    decimal unrealized = longside.UnRealizedPL + shortside.UnRealizedPL;
                    posUnrealizedPL.Text = string.Format(GetDisplayFormat(_currentSymbol.Symbol), unrealized);
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateUnRealizedPL error:" + ex.ToString());
                }
            }
        }
        void UpdateCanFlat()
        {
            if (InvokeRequired)
                Invoke(new VoidDelegate(UpdateCanFlat), new object[] { });
            else
            {
                try
                {

                    if (_currentSymbol == null) return;
                    Position longside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, true];
                    Position shortside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, false];
                    OrderTracker ot = CoreService.TradingInfoTracker.OrderTracker;

                    int longflat = longside.isFlat ? 0 : (longside.UnsignedSize - ot.GetPendingExitSize(_currentSymbol.Symbol, !longside.isLong));
                    int shortflat = shortside.isFlat ? 0 : (shortside.UnsignedSize - ot.GetPendingExitSize(_currentSymbol.Symbol, !shortside.isLong));
                    posCanFlat.Text = string.Format("{0}/{1}", longflat, shortflat);

                }
                catch (Exception ex)
                {
                    logger.Error("UpdateCanFlat error:" + ex.ToString());
                }
            }
        }
        #endregion

        #region 手数
        void UpdateSizeCtlMaximum(int num)
        {
            //canopensize = num;
            if (InvokeRequired)
                Invoke(new Action<int>(UpdateSizeCtlMaximum), new object[] { num });
            else
            {
                try
                {
                    maxOpenSize.Text = num.ToString();
                    //sizeTrack.Maximum = num;
                    size.Maximum = num;
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateSizeCtlMaximum error:" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 调节器数量到数字空间
        /// </summary>
        //void UpdateSizeCtlValue()
        //{
        //    if (InvokeRequired)
        //        Invoke(new VoidDelegate(UpdateSizeCtlValue), new object[] { });
        //    else
        //    {
        //        try
        //        {
        //            int csize = (int)sizeTrack.Value;
        //            csize = csize < canopensize ? csize : canopensize;
        //            size.Value = csize;
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error("UpdateSizeCtlValue error:" + ex.ToString());
        //        }
        //    }
        //}

        ///// <summary>
        ///// 更新数量调节器
        ///// </summary>
        //void UpdateSizeTrackValue()
        //{
        //    if (InvokeRequired)
        //        Invoke(new VoidDelegate(UpdateSizeTrackValue), new object[] { });
        //    else
        //    {
        //        try
        //        {
        //            float fs = (float)size.Value;
        //            if (fs > sizeTrack.Maximum)
        //                fs = sizeTrack.Maximum;
        //            sizeTrack.Value = fs;
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error("UpdateSizeTrackValue error:" + ex.ToString());
        //        }
        //    }
        //}

        void QryMaxOrderVal()
        { 
            if(_currentSymbol == null) return;
            CoreService.TLClient.ReqQryMaxOrderVol(_currentSymbol.Symbol);

        }
       
        
        /// <summary>
        /// 更新可开数量
        /// 1.服务端查询 
        /// 2.本地计算
        /// </summary>
        //public void UpdateMaxOpenSize()
        //{
        //    ////本地计算可开手数
        //    //if (QryCanOpenPositionLocalEvent != null && CurrentSecurity != null)
        //    //{
        //    //    int r = QryCanOpenPositionLocalEvent(CurrentSecurity.Symbol);
        //    //    UpdateMaxOpenSize(r);
        //    //    return;
        //    //}
        //    ////服务端查询
        //    //if (QryCanOpenPositionEvent != null && CurrentSecurity !=null)
        //    //    QryCanOpenPositionEvent(CurrentSymbol);
        //}

        //总可开数量 服务端获得最大可开数量 则通过回调事件调用该函数
        int canopensize = 0;
        public void UpdateMaxOpenSize(int psize)
        {
            canopensize = psize;
            UpdateSizeCtlMaximum(psize);
        }

        //更新当前设定的手数
        void UpdateSize(int nsize)
        {
            int v = nsize <= canopensize ? nsize : canopensize;
            size.Value = v;
        }
        /// <summary>
        /// 设定总可开数量,用于保存可开手数
        /// -1 默认手数
        /// >0 设定对应的手数
        /// </summary>
        /// <param name="psize"></param>
        public void SetOrderSize(int size=-1)
        {
            if (size != -1)
            {
                if (size > canopensize)
                    size = canopensize;
                this.size.Value = size;
            }
            else
            {
                if (this.size.Value > canopensize)
                    this.size.Value = canopensize;
                if (this.size.Value == 0 && canopensize > 0)
                    this.size.Value = 1;
            }
        }

        #endregion

        #region 界面操作 事件
        /// <summary>
        /// 合约列表选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbSymbolList_SelectedValueChanged(object sender, EventArgs e)
        {
            Symbol sym = cbSymbolList.SelectedValue as Symbol;
            if (sym == null)
            {
                TraderHelper.WindowMessage("无效合约对象");
                return;
            }
            
            //设定
            SetSymbol(sym);

            //触发合约选择事件
            CoreService.EventUI.FireSymbolselectedEvent(this,sym);

            //TraderHelper.WindowMessage("选择合约:" + sym.Symbol);
        }


        //买入按钮
        private void btnBuy_Click(object sender, EventArgs e)
        {
            //TraderHelper.WindowMessage("买入按钮操作");
            genOrder(true);
        }
        //卖出按钮
        private void btnSell_Click(object sender, EventArgs e)
        {
            genOrder(false);
        }


        //合约手数
        private void size_ValueChanged(object sender, EventArgs e)
        {
            //UpdateSizeTrackValue();
        }
        //合约手数调节器
        private void sizeTrack_ValueChanged(object sender, EventArgs e)
        {
            //UpdateSizeCtlValue();
        }

        //委托类别选择触发事件
        private void orderType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentOrderType == QSEnumOrderType.Market)
            {
                price.Enabled = false;
            }
            else
            {
                price.Enabled = true;
            }
            _pricetouch = false;
        }

        #region 调整跟价设置
        private void bidlabel_DoubleClick(object sender, EventArgs e)
        {
            _pricefollow = PriceFollow.BID;
            _pricetouch = false;
            followtype.Text = "买";
        }

        private void bidlabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void asklabel_DoubleClick(object sender, EventArgs e)
        {
            _pricefollow = PriceFollow.ASK;
            _pricetouch = false;
            followtype.Text = "卖";
            
        }

        private void bidprice_DoubleClick(object sender, EventArgs e)
        {
            _pricefollow = PriceFollow.BID;
            _pricetouch = false;
            followtype.Text = "买";
        }

        private void askprice_DoubleClick(object sender, EventArgs e)
        {
            _pricefollow = PriceFollow.ASK;
            _pricetouch = false;
            followtype.Text = "卖";
        }

        private void asklabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void bidlabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void asklabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void followtype_DoubleClick(object sender, EventArgs e)
        {
            _pricefollow = PriceFollow.TRADE;
            _pricetouch = false;
            followtype.Text = "价";
        }

        private void followtype_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void followtype_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        //价格输入框 操作事件
        private void price_Enter(object sender, EventArgs e)
        {
            //MessageBox.Show("price enter");
            logger.Debug("price enter ");
            _pricetouch = true;
        }

        private void price_Leave(object sender, EventArgs e)
        {
            logger.Debug("price leave ");
            //_pricetouch = false;
        }


        #endregion

        //全平按钮
        private void btnFall_Click(object sender, EventArgs e)
        {
            if (_currentSymbol == null) return;
            Position longside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, true];
            Position shortside = CoreService.TradingInfoTracker.PositionTracker[_currentSymbol.Symbol, CoreService.Account, false];

            if (longside != null && !longside.isFlat)
            {
                FlatPosition(longside);
            }
            if (longside != null && !shortside.isFlat)
            {
                FlatPosition(shortside);
            }
        }
        void FlatPosition(Position pos)
        {
            if (pos == null || pos.isFlat) return;
            Order o = new MarketOrderFlat(pos);
            SendOrder(o);
        }

        //全撤按钮
        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            if (_currentSymbol == null) return;
            foreach (Order o in CoreService.TradingInfoTracker.OrderTracker)
            {
                if ((o.Symbol == _currentSymbol.Symbol) && o.IsPending())
                {
                    CoreService.TLClient.ReqCancelOrder(o.id);
                }
            }
        }
        //反手按钮
        private void btnReserve_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 功能函数

        /// <summary>
        /// 生成对应的买 卖委托并发送出去
        /// </summary>
        /// <param name="f"></param>
        private void genOrder(bool f)
        {
            if (!ValidSymbol()) return;
            if (!ValidSize()) return;
            if (!ValidPrice()) return;


            Order o = new OrderImpl(CurrentSymbol.Symbol, 0);
            o.Exchange = CurrentSymbol.Exchange;
            o.LocalSymbol = CurrentSymbol.Symbol;
            o.Side = f;
            o.Size = Math.Abs((int)size.Value);
            if (ismarket)
            {
                o.LimitPrice = 0;
                o.StopPrice = 0;
            }
            else
            {
                bool islimit = this.islimit;
                decimal limit = islimit ? (decimal)(price.Value) : 0;
                decimal stop = !islimit ? (decimal)(price.Value) : 0;
                o.LimitPrice = limit;
                o.StopPrice = stop;
            }

            SendOrder(o);
        }

        /// <summary>
        /// 检查当前设置手数
        /// </summary>
        /// <returns></returns>
        bool ValidSize()
        {
            if ((int)(size.Value) == 0)
            {
                TraderHelper.WindowMessage("请设置下单手数");
                return false;
            }
            else
            {
                return true;
            }
        }
        bool ismarket { get { return CurrentOrderType == QSEnumOrderType.Market; } }
        bool islimit { get { return CurrentOrderType == QSEnumOrderType.Limit; } }

        bool ValidPrice()
        {
            if (ismarket|| (decimal)(price.Value)>0)
            {
                return true;
            }
            else
            {
                TraderHelper.WindowMessage("请设定下单价格");
                return false;
            }
        }
        /// <summary>
        /// 检查当前是否选中合约
        /// </summary>
        /// <returns></returns>
        bool ValidSymbol()
        {
            if (CurrentSymbol == null)
            {
                TraderHelper.WindowMessage("请选择合约");
                return false;
            }
            return true;
          
        }


        void SendOrder(Order order)
        {
            logger.Info("send new order:" + order.GetOrderInfo());
            //发送委托时,保存当前设置的止损止盈参数,用于持仓列表调用 并自动设置止损 止盈
            //updateSymProfitLossArgs();
            //调用底层接口发送委托
            CoreService.TLClient.ReqOrderInsert(order);

            //将附带的止盈止损设置 发送到服务端
            //SendOffsetAttached();
            //当重置 或者 在平仓模式下发送过为头后，平仓模式切换
            if (ispostionmode)
            {
                //Reset();
            }
        }

        #endregion

        #region 止盈 止损 区域
        //在下单时 如果没有止盈止损则不对服务端的止盈止损参数进行更新
        //如果有止盈止损设置 则对服务端的止盈止损参数进行更新
        //初始化委托类型
        void InitOffsetType()
        {
            ArrayList l1 = new ArrayList();
            ValueObject<QSEnumPositionOffsetType> vo11 = new ValueObject<QSEnumPositionOffsetType>();
            vo11.Name = "价格";
            vo11.Value = QSEnumPositionOffsetType.PRICE;
            l1.Add(vo11);

            ValueObject<QSEnumPositionOffsetType> vo12 = new ValueObject<QSEnumPositionOffsetType>();
            vo12.Name = "点数";
            vo12.Value = QSEnumPositionOffsetType.POINTS;
            l1.Add(vo12);

            lossType.DisplayMember = "Name";
            lossType.ValueMember = "Value";


            ArrayList l2 = new ArrayList();
            ValueObject<QSEnumPositionOffsetType> vo21 = new ValueObject<QSEnumPositionOffsetType>();
            vo21.Name = "价格";
            vo21.Value = QSEnumPositionOffsetType.PRICE;
            l2.Add(vo21);

            ValueObject<QSEnumPositionOffsetType> vo22 = new ValueObject<QSEnumPositionOffsetType>();
            vo22.Name = "点数";
            vo22.Value = QSEnumPositionOffsetType.POINTS;
            l2.Add(vo22);
            profitType.DisplayMember = "Name";
            profitType.ValueMember = "Value";
            

            Factory.IDataSourceFactory(lossType).BindDataSource(l1);
            Factory.IDataSourceFactory(profitType).BindDataSource(l2);
        }
        QSEnumPositionOffsetType LossOffsetType { get { return (QSEnumPositionOffsetType)lossType.SelectedValue; } }
        QSEnumPositionOffsetType ProfitOffsetType { get { return (QSEnumPositionOffsetType)profitType.SelectedValue; } }
        
        void UpdateLossOffsetUI()
        {
            switch (LossOffsetType)
            {
                case QSEnumPositionOffsetType.PRICE:
                    {
                        lossValue.Value = price.Value;
                    }
                    break;
                case QSEnumPositionOffsetType.POINTS:
                    {
                        //if (CurrentSecurity != null)
                        //    lossValue.Value = CurrentSecurity.PriceTick * 100;
                    }
                    break;
                default:
                    break;

            }
            
        }

        void UpdateProfitOffsetUI()
        {
            switch (ProfitOffsetType)
            {
                case QSEnumPositionOffsetType.PRICE:
                    {
                        profitValue.Value = price.Value;
                    }
                    break;
                case QSEnumPositionOffsetType.POINTS:
                    {
                        //if (CurrentSecurity != null)
                        //    profitValue.Value = CurrentSecurity.PriceTick * 100;
                    }
                    break;
                default:
                    break;
            }
        }

        private void lossType_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateLossOffsetUI();

        }

        private void profitType_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateProfitOffsetUI();

        }

        //下单的同时 进行服务端止盈止损参数更新
        //下单时同时设置服务端止盈止损,则我们发送委托时,检查当前的设置 如果有止盈 止损设置 则向服务端提交止盈止损参数
        void SendOffsetAttached()
        {
            //debug("下单面板附带止盈止损");
            //如果下单面板没有设定Account参数 则直接返回 止盈止损需要有Account进行标识
            //if (string.IsNullOrEmpty(_account)) return;
            //止损有效 发送止损参数
            if (isLossSet.Checked)
            {
                //debug("下单面板 附带止损设置");
                //PositionOffsetArgs lossarg = new PositionOffsetArgs(_account, CurrentSymbol, QSEnumPositionOffsetDirection.LOSS);
                //lossarg.Enable = true;
                //lossarg.OffsetType = LossOffsetType;
                //lossarg.Value = lossValue.Value;
                //lossarg.Size = 0;
                //if (UpdatePostionOffsetEvent != null)
                //    UpdatePostionOffsetEvent(lossarg);
            }
            //止盈有效  发送止盈参数
            if (isProfitSet.Checked)
            {
                //debug("下单面板 附带止盈设置");
                //PositionOffsetArgs profitarg = new PositionOffsetArgs(_account, CurrentSymbol, QSEnumPositionOffsetDirection.PROFIT);
                //profitarg.Enable = true;
                //profitarg.OffsetType = ProfitOffsetType;
                //profitarg.Value = profitValue.Value;
                //profitarg.Size = 0;
                //if (UpdatePostionOffsetEvent != null)
                //    UpdatePostionOffsetEvent(profitarg);
            }

        
        }
        #endregion


        #region 快捷 平 撤 反 区域
        private void btnFall_MouseEnter(object sender, EventArgs e)
        {
            FillPrimitive f = ((FillPrimitive)btnFall.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
            f.BackColor = Color.LightGray;
        }

        private void btnFall_MouseLeave(object sender, EventArgs e)
        {
            FillPrimitive f = ((FillPrimitive)btnFall.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
            f.BackColor = Color.Crimson;
        }

        private void btnCancelAll_MouseEnter(object sender, EventArgs e)
        {
            FillPrimitive f = ((FillPrimitive)btnCancelAll.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
            f.BackColor = Color.LightGray;
        }

        private void btnCancelAll_MouseLeave(object sender, EventArgs e)
        {
            FillPrimitive f = ((FillPrimitive)btnCancelAll.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
            f.BackColor = Color.Orange;
        }

       

        

        //private void btnReserve_MouseEnter(object sender, EventArgs e)
        //{

        //    FillPrimitive f = ((FillPrimitive)btnReserve.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
        //    f.BackColor = Color.LightGray;
        //}

        //private void btnReserve_MouseLeave(object sender, EventArgs e)
        //{
        //    FillPrimitive f = ((FillPrimitive)btnReserve.ButtonElement.GetChildrenByType(typeof(FillPrimitive))[0]);
        //    f.BackColor = Color.Crimson;
        //}
        #endregion

        




        






















    }
    //价格框显示价格类别
    internal enum PriceFollow
    { 
        TRADE,
        ASK,
        BID,
    }
   
}
