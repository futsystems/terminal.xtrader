﻿using System;
using System.Collections.Generic;
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
        public event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

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
            //ctrlTraderLogin.ExitTrader += new Action(ctrlTraderLogin_ExitTrader);

            CoreService.EventUI.OnSymbolSelectedEvent += new Action<object, API.Symbol>(EventUI_OnSymbolSelectedEvent);
            CoreService.EventCore.OnInitializedEvent += new VoidDelegate(EventCore_OnInitializedEvent);
            CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
            CoreService.EventIndicator.GotPositionNotifyEvent += new Action<PositionEx>(EventIndicator_GotPositionNotifyEvent);
        }

        void EventIndicator_GotPositionNotifyEvent(PositionEx obj)
        {
            if (TraderConfig.ExPositionLine)
            {
                PositionNotify(obj.Exchange, obj.Symbol, obj.Side, obj.Position, obj.AvgPrice);
            }
        }

        void EventOther_OnResumeDataStart()
        {
            TradingInfoRest();
        }

        void EventOther_OnResumeDataEnd()
        {
            if (TraderConfig.ExPositionLine)
            {
                foreach (var pos in CoreService.TradingInfoTracker.PositionTracker.Where(pos => !pos.isFlat))
                {
                    PositionNotify(pos.oSymbol.Exchange, pos.oSymbol.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, pos.UnsignedSize, pos.AvgPrice);
                }
            }
        }

        /// <summary>
        /// 初始化完毕事件
        /// 将当前持仓以事件的方式对外发送 向行情组件填充数据
        /// </summary>
        void EventCore_OnInitializedEvent()
        {
            if (TraderConfig.ExPositionLine)
            {
                foreach (var pos in CoreService.TradingInfoTracker.PositionTracker.Where(pos => !pos.isFlat))
                {
                    PositionNotify(pos.oSymbol.Exchange, pos.oSymbol.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, pos.UnsignedSize, pos.AvgPrice);
                }
            }
        }



        void EventUI_OnSymbolSelectedEvent(object arg1, API.Symbol arg2)
        {
            if(arg2 == null) return;
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
                    _trader.TraderWindowOpeartion += new Action<EnumTraderWindowOperation>(tmp_TraderWindowOpeartion);
                    this.Controls.Add(_trader);
                }
                ctrlTraderLogin.Visible = false;
                _trader.Show();
            }
        }

        void tmp_TraderWindowOpeartion(EnumTraderWindowOperation obj)
        {
            if (TraderWindowOpeartion != null)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                {
                    TradingInfoRest();

                    TraderWindowOpeartion(obj);

                    if (obj == EnumTraderWindowOperation.Close)
                    {
                        //关闭交易系统
                        ctrlTraderLogin.Visible = true;
                        ctrlTraderLogin.StopTrader();
                        _trader.Visible = false;
                        _trader = null;
                    }

                });
            
            }
        }


        #region 操作
        public void SelectSymbol(string exchange, string symbol)
        {
            if (_trader != null)
            {
                Symbol sym = CoreService.BasicInfoTracker.GetSymbol(exchange, symbol);
                if (sym != null)
                {
                    CoreService.EventUI.FireSymbolSelectedEvent(this, sym);
                }
                else
                {
                    _trader.OrderEntryClearSymbol();
                }
            }
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
