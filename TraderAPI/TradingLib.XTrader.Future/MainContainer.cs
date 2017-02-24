﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    /// <summary>
    /// 交易插件设计
    /// 交易功能部分全部放到交易插件的dll中去实现,控件实现了接口ITraderAPI,主程序通过加载该接口来实现必须的功能
    /// 1.交易相关的 下单，查询交易记录等。不过这些功能相对设计简单 主要用于图标展现
    /// 2.功能区域的正常最大化，最小化，关闭等功能 相互之间的联动
    /// 3.合约选择等相关操作的联动 
    /// 以上操作需要设计一个双向联动接口 双方都通过该接口进行必要的数据访问。尽量减少操作暴露。实现低耦合，高内聚
    /// 
    /// 1.默认只创建登入控件，该控件简单轻便，在主程序调用时可以快速的加载并显示。
    /// 2.当客户执行登入时，当登入成功并执行初始化操作完毕后 在后台线程创建 交易控件，并对外暴露事件 如果在登入控件内的后台线程创建 则需要使用invoke 否则创建的控件无法在前段显示
    /// 3.在主控件中 获得该时间后 将登入控件隐藏同时将交易控件加入到当前主控件并显示 在接受控件注册，需要再次使用invoke才可以显示传递过来的控件
    /// 
    /// 后来通过实验 在登入后台线程直接触发事件，主控件监听后在函数内通过invoke 生成并显示控件，简化了代码逻辑
    /// 
    /// 
    /// </summary>
    public partial class MainContainer : UserControl,ITraderAPI
    {
        public TraderAPISetting APISetting { get { return apisetting; } }

        TraderAPISetting apisetting = new TraderAPISetting();

        /// <summary>
        /// 交易窗口操作 关闭 最小化 最大化操作
        /// </summary>
        public event Action<EnumTraderWindowOperation> TraderWindowOpeartion = delegate { };

        /// <summary>
        /// 持仓更新事件
        /// </summary>
        public event Action<string, string,bool, int, decimal> PositionNotify = delegate { };

        /// <summary>
        /// 交易数据重置
        /// </summary>
        public event Action TradingInfoRest = delegate { };
        /// <summary>
        /// 查看KChart事件
        /// </summary>
        public event Action<string, string, int> ViewKChart;

        ConcurrentDictionary<string, Symbol> _symbolRegister = new ConcurrentDictionary<string, Symbol>();
        /// <summary>
        /// 交易组件所需合约注册集合
        /// </summary>
        public IEnumerable<string> SymbolRegisters { get { return _symbolRegister.Keys; } }


        /// <summary>
        /// 合约集合变动事件
        /// </summary>
        public event Action SymbolRegisterChanged = delegate { };

        public MainContainer()
        {
            apisetting.TradingBoxMinHeight = 258;


            InitializeComponent();
            ctrlTraderLogin.BackColor = Color.White;

            WireEvent();

            
        }

        void WireEvent()
        {
            ctrlTraderLogin.EntryTrader += new Action(ctrlTraderLogin_EntryTrader);
            ctrlTraderLogin.TradingBoxOpeartion += new Action<EnumTraderWindowOperation>(OnTraderWindowOpeartion);
            //ctrlTraderLogin.ExitTrader += new Action(ctrlTraderLogin_ExitTrader);

            CoreService.EventCore.OnInitializedEvent += new VoidDelegate(EventCore_OnInitializedEvent);

            CoreService.EventIndicator.GotPositionNotifyEvent += new Action<PositionEx>(EventIndicator_GotPositionNotifyEvent);

            //合约选择
            UIService.EventUI.OnSymbolSelectedEvent += new Action<object, API.Symbol>(EventHub_OnSymbolSelectedEvent);
            UIService.EventUI.OnSymbolUnSelectedEvent += new Action<object, Symbol>(EventHub_OnSymbolUnSelectedEvent);

            //数据恢复
            CoreService.EventHub.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            CoreService.EventHub.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            
        }


        void EventIndicator_GotPositionNotifyEvent(PositionEx obj)
        {
            if (TraderConfig.ExPositionLine)
            {
                PositionNotify(obj.Exchange, obj.Symbol, obj.Side, obj.Position, obj.AvgPrice);
            }
        }

        /// <summary>
        /// 初始化完毕事件
        /// 将当前持仓以事件的方式对外发送 向行情组件填充数据
        /// </summary>
        void EventCore_OnInitializedEvent()
        {

        }

        #region 合约选择
        /// <summary>
        /// 非选中合约
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventHub_OnSymbolUnSelectedEvent(object arg1, Symbol arg2)
        {
            if (arg2 == null) return;
            UnRegisterSymbol(arg2);
        }

        /// <summary>
        /// 选中合约
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void EventHub_OnSymbolSelectedEvent(object arg1, API.Symbol arg2)
        {
            if (arg2 == null) return;
            RegisterSymbol(arg2);

            if (ViewKChart != null)
            {
                //同步切换行情窗口中的合约
                if (TraderConfig.ExSwitchSymbolOfMarketDataView)
                {
                    //点击持仓 选择合约
                    if (arg1 is ctrlPosition)
                    {
                        ViewKChart(arg2.Exchange, arg2.Symbol, 0);
                    }
                    if (arg1 is ctrlOrderEntry)
                    {
                        ViewKChart(arg2.Exchange, arg2.Symbol, 0);
                    }
                }
            }
        }
        #endregion

        #region 交易数据恢复
        void EventOther_OnResumeDataStart()
        {
            TradingInfoRest();
        }

        /// <summary>
        /// 交易数据查询完毕
        /// </summary>
        void EventOther_OnResumeDataEnd()
        {
            //1.交易记录查询完毕后将 重新设定合约注册
            _symbolRegister.Clear();
            foreach (var sym in CoreService.TradingInfoTracker.HotSymbols)
            {
                RegisterSymbol(sym, false);
            }

            if(UIService.EventUI.SymbolSelected!= null)
            {
                //注册当前选中合约
                RegisterSymbol(UIService.EventUI.SymbolSelected);
            }
            //触发合约集合变动事件
            SymbolRegisterChanged();

            //2.对外执行持仓更新
            if (TraderConfig.ExPositionLine)
            {
                foreach (var pos in CoreService.TradingInfoTracker.PositionTracker.Where(pos => !pos.isFlat))
                {
                    PositionNotify(pos.oSymbol.Exchange, pos.oSymbol.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, pos.UnsignedSize, pos.AvgPrice);
                }
            }
        }
        #endregion

        #region 订阅/注销 合约实时行情
        /// <summary>
        /// 订阅合约
        /// </summary>
        /// <param name="sym"></param>
        void RegisterSymbol(Symbol sym,bool fireEvent=true)
        {
            bool ret = _symbolRegister.TryAdd(sym.UniqueKey,sym);
            if (ret)
            {
                CoreService.TLClient.ReqXQryTickSnapShot(sym.Exchange, sym.Symbol);
                //触发变动事件
            }
            if (ret && fireEvent)
            {
                SymbolRegisterChanged();
            }
        }


        /// <summary>
        /// 注销合约
        /// </summary>
        /// <param name="sym"></param>
        void UnRegisterSymbol(Symbol sym)//, bool fireEvent = true)
        {
            Symbol target = null;
            bool ret = false;
            if (!CoreService.TradingInfoTracker.HotSymbols.Contains(sym))
            {
                ret = _symbolRegister.TryRemove(sym.UniqueKey, out target);
            }
            //注销合约是在选中合约时 对上一次选中合约的注销 都是伴随着选中合约发生的 因此这里只在订阅合约处 触发事件 避免多次触发
            //if (ret && fireEvent)
            //{
            //    SymbolRegisterChanged();
            //}
        }
        #endregion


        ctrlFutureTrader  _trader = null;

        //void ctrlTraderLogin_ExitTrader()
        //{
        //    if (TraderWindowOpeartion != null)
        //    {
        //        TraderWindowOpeartion(EnumTraderWindowOperation.Close);
        //    }
        //}


        void ctrlTraderLogin_EntryTrader()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ctrlTraderLogin_EntryTrader), new object[] { });
            }
            else
            {
                //进行延迟加载，避免第一次初始化时创建所有控件
                //如果每次退出再登入都重新创建_trader控件 则多次登入后会卡死界面，后来通过复用解决这个问题
                if (_trader == null)
                {
                    _trader = new ctrlFutureTrader();
                    _trader.Dock = DockStyle.Fill;
                    _trader.TradingBoxOpeartion += new Action<EnumTraderWindowOperation>(OnTraderWindowOpeartion);
                    _trader.LockTradingBox += new Action(OnLockTradingBox);
                    this.Controls.Add(_trader);
                }
                ctrlTraderLogin.Visible = false;
                _trader.Show();
            }
        }

        void OnLockTradingBox()
        {
            
            _trader.Visible = false;
            ctrlTraderLogin.LockMode();
            ctrlTraderLogin.Show();

        }

        void OnTraderWindowOpeartion(EnumTraderWindowOperation obj)
        {

                System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                {
                    //重置行情行情窗口交易信息
                    TradingInfoRest();
                    //调用主程序 执行交易窗口操作 最大化 最小化
                    TraderWindowOpeartion(obj);

                    //如果是关闭操作 则调用TraderLogin执行停止操作
                    if (obj == EnumTraderWindowOperation.Close)
                    {
                        //关闭交易系统
                        ctrlTraderLogin.Visible = true;
                        ctrlTraderLogin.StopTrader();
                        if (_trader != null)
                        {
                            _trader.Visible = false;
                            //_trader = null;
                        }
                    }

                });
            
            
        }


        #region 接口操作
        /// <summary>
        /// 选择某合约
        /// 行情切换合约时 交易窗口同步选择对应合约
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        public void SelectSymbol(string exchange, string symbol)
        {
            if (_trader != null)
            {
                Symbol sym = CoreService.BasicInfoTracker.GetSymbol(exchange, symbol);
                if (sym != null)
                {
                    UIService.EventUI.FireSymbolSelectedEvent(this, sym);
                }
                else
                {
                    _trader.OrderEntryClearSymbol();
                }
            }
        }

        Dictionary<string, Tick> snapshotMap = new Dictionary<string, Tick>();
        /// <summary>
        /// 合约更新行情数据
        /// </summary>
        /// <param name="symbol"></param>
        public void NotifyTick(MDSymbol symbol)
        {
            //会出现 合约注册为无的情况 导致无法获得正常的行情数据
            if (!_symbolRegister.Keys.Contains(symbol.UniqueKey)) return;
            int x = CoreService.TradingInfoTracker.HotSymbols.Count();
            Tick k = null;
            if (!snapshotMap.TryGetValue(symbol.UniqueKey, out k))
            {
                k = new TickImpl();
                k.Exchange = symbol.Exchange;
                k.Symbol = symbol.Symbol;
                k.UpdateType = "S";
                k.DataFeed = QSEnumDataFeedTypes.DEFAULT;

            }
            
            k.Date = symbol.TickSnapshot.Date;
            k.Time = symbol.TickSnapshot.Time;
            k.Trade = (decimal)symbol.TickSnapshot.Price;
            k.Size = (int)symbol.TickSnapshot.Size;
            k.BidPrice = (decimal)symbol.TickSnapshot.Buy1;
            k.BidSize = (int)symbol.TickSnapshot.BuyQTY1;
            k.AskPrice = (decimal)symbol.TickSnapshot.Sell1;
            k.AskSize = (int)symbol.TickSnapshot.SellQTY1;
            k.Vol = (int)symbol.TickSnapshot.Volume;
            k.Open = (decimal)symbol.TickSnapshot.Open;
            k.High = (decimal)symbol.TickSnapshot.High;
            k.Low = (decimal)symbol.TickSnapshot.Low;
            k.PreClose = (decimal)symbol.TickSnapshot.PreClose;
            k.PreOpenInterest = symbol.TickSnapshot.PreOI;
            k.PreSettlement = (decimal)symbol.TickSnapshot.PreSettlement;
            k.Settlement = (decimal)symbol.TickSnapshot.Settlement;

            CoreService.EventIndicator.FireTick(k);

        }


        /// <summary>
        /// 进入委托提交状态
        /// </summary>
        /// <param name="side"></param>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        public void EntryOrder(bool side, string exchange, string symbol)
        {
        

        }

        /// <summary>
        /// 提交委托
        /// </summary>
        /// <param name="side"></param>
        /// <param name="exchagne"></param>
        /// <param name="symbol"></param>
        /// <param name="size"></param>
        /// <param name="price"></param>
        public void PlaceOrder(bool side, string exchagne, string symbol, int size, double price)
        {
            if (_trader != null)
            {
                
            }
        }

        #endregion


    }
}
