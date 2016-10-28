using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.TraderCore
{
    public partial class TradingInfoTracker
    {
        ILog logger = LogManager.GetLogger("TradingInfoTracker");

        /// <summary>
        /// 行情快照维护器
        /// </summary>
        public TickTracker TickTracker {get;set;}

        /// <summary>
        /// 委托维护器
        /// </summary>
        public OrderTracker OrderTracker { get; set; }

        /// <summary>
        /// 当日持仓维护器
        /// </summary>
        public LSPositionTracker PositionTracker { get; set; }

        /// <summary>
        /// 昨日持仓维护器
        /// </summary>
        public LSPositionTracker HoldPositionTracker { get; set; }

        /// <summary>
        /// 成交维护器
        /// </summary>
        public ThreadSafeList<Trade> TradeTracker { get; set; }

        /// <summary>
        /// 新的持仓生成事件
        /// </summary>
        public event Action<Position> GotPositionEvent;

        /// <summary>
        /// 交易账户对象
        /// </summary>
        public AccountLite Account { get; set; }

        /// <summary>
        /// 合约
        /// 委托 持仓 合约需要 记录 用于保持行情订阅
        /// </summary>
        public List<Symbol> HotSymbols { get; set; }

        public TradingInfoTracker()
        {
            OrderTracker = new OrderTracker();
            PositionTracker = new LSPositionTracker("");
            HoldPositionTracker = new LSPositionTracker("");
            TradeTracker = new ThreadSafeList<Trade>();
            TickTracker = new TickTracker();
            Account = new AccountLite();
            HotSymbols = new List<Symbol>();

            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(EventIndicator_GotFillEvent);
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(EventIndicator_GotTickEvent);

            CoreService.EventQry.OnRspXQryOrderResponse += new Action<Order, RspInfo, int, bool>(EventQry_OnRspXQryOrderResponse);
            CoreService.EventQry.OnRspXQryYDPositionResponse += new Action<PositionDetail, RspInfo, int, bool>(EventQry_OnRspXQryYDPositionResponse);
            CoreService.EventQry.OnRspXQryFillResponese += new Action<Trade, RspInfo, int, bool>(EventQry_OnRspXQryFillResponese);
            //CoreService.EventQry.OnRspQryAccountInfoResponse += new Action<AccountInfo, RspInfo, int, bool>(EventQry_OnRspQryAccountInfoResponse);
            CoreService.EventQry.OnRspXQryAccountResponse += new Action<AccountLite, RspInfo, int, bool>(EventQry_OnRspXQryAccountResponse);
            PositionTracker.NewPositionEvent += new Action<Position>(PositionTracker_NewPositionEvent);
        }



        void EventIndicator_GotTickEvent(Tick obj)
        {
            TickTracker.GotTick(obj);

            //持仓处理成交数据
            if (obj.UpdateType == "X" || obj.UpdateType == "S")
            {
                PositionTracker.GotTick(obj);
            }
        }

        void EventIndicator_GotFillEvent(Trade obj)
        {
            bool accept = false;
            PositionTracker.GotFill(obj, out accept);
            KeepSymbol(obj.oSymbol);
            if (accept)
            {
                OrderTracker.GotFill(obj);
                TradeTracker.Add(obj);
            }
        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            OrderTracker.GotOrder(obj);
            KeepSymbol(obj.oSymbol);
        }


        int _qrypositionid = 0;
        /// <summary>
        /// 请求恢复日内交易记录
        /// </summary>
        public void ResumeData()
        {
            logger.Info("Start to resueme trading data");
            CoreService.EventOther.FireResumeDataStart();

            //清空当前交易记录维护器
            OrderTracker.Clear();
            PositionTracker.Clear();
            HoldPositionTracker.Clear();
            TradeTracker.Clear();
            HotSymbols.Clear();

            //执行隔夜持仓查询 并按序触发后续查询
            _qrypositionid = CoreService.TLClient.ReqXQryYDPositon();
        }


        public void Reset()
        {
            //清空当前交易记录维护器
            OrderTracker.Clear();
            PositionTracker.Clear();
            HoldPositionTracker.Clear();
            TradeTracker.Clear();
            HotSymbols.Clear();
            Account = null;
        }


        void KeepSymbol(Symbol symbol)
        {
            if (symbol == null) return;
            if (!HotSymbols.Contains(symbol))
            {
                HotSymbols.Add(symbol);
            }
        }
        int _qryorderid = 0;
        void EventQry_OnRspXQryYDPositionResponse(PositionDetail arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qrypositionid) return;
            if (arg1 != null)
            {
                PositionTracker.GotPosition(arg1);
                HoldPositionTracker.GotPosition(arg1);
                KeepSymbol(arg1.oSymbol);
            }
            if (arg4)
            {
                Status("隔夜持仓查询完毕,查询委托");
                _qryorderid =  CoreService.TLClient.ReqXQryOrder();
            }
        }

        int _qrytradeid = 0;
        void EventQry_OnRspXQryOrderResponse(Order arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryorderid) return;
            if (arg1 != null)
            {
                OrderTracker.GotOrder(arg1);
                KeepSymbol(arg1.oSymbol);
            }
            if (arg4)
            {
                Status("委托查询完毕,查询成交");
                _qrytradeid =  CoreService.TLClient.ReqXQryTrade();
            }
        }

        int _qryaccountinfoid = 0;
        void EventQry_OnRspXQryFillResponese(Trade arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qrytradeid) return;
            if (arg1 != null)
            {
                bool accept = false;
                PositionTracker.GotFill(arg1, out accept);
                KeepSymbol(arg1.oSymbol);
                if (accept)
                {
                    OrderTracker.GotFill(arg1);
                    TradeTracker.Add(arg1);
                }
            }
            if (arg4)
            {
                Status("成交查询完毕,查询帐户信息");
                _qryaccountinfoid = CoreService.TLClient.ReqXQryAccount();
            }
        }

        void EventQry_OnRspXQryAccountResponse(AccountLite arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryaccountinfoid) return;
            CoreService.TradingInfoTracker.Account = arg1;
            //登入第一次初始化过程中 查询完毕后需要启动行情连接并执行初始化事件
            if (!CoreService.Initialized)
            {
                if (arg1 == null)
                {
                    Status("帐户信息查询异常");
                    return;
                }

                if (arg4)
                {
                    Status("帐户信息查询完毕");
                    CoreService.TLClient.StartTick();
                    //核心服务完成初始化
                    CoreService.Initialize();
                    Status("触发初始化完毕事件");
                }
            }

            logger.Info("trading data resume finished");
            CoreService.EventOther.FireResumeDataEnd();
        }

        //void EventQry_OnRspQryAccountInfoResponse(AccountInfo arg1, RspInfo arg2, int arg3, bool arg4)
        //{
        //    if (arg3 != _qryaccountinfoid) return;

        //    //CoreService.AccountInfo = arg1;
        //    //登入第一次初始化过程中 查询完毕后需要启动行情连接并执行初始化事件
        //    if (!CoreService.Initialized)
        //    {
        //        if (arg1 == null)
        //        {
        //            Status("帐户信息查询异常");
        //            return;
        //        }

        //        if (arg4)
        //        {
        //            Status("帐户信息查询完毕");
        //            CoreService.TLClient.StartTick();
        //            //核心服务完成初始化
        //            CoreService.Initialize();
        //            Status("触发初始化完毕事件");
        //        }
        //    }

        //    logger.Info("trading data resume finished");
        //    CoreService.EventOther.FireResumeDataEnd();
            
        //}



        void PositionTracker_NewPositionEvent(Position obj)
        {
            if (GotPositionEvent != null)
                GotPositionEvent(obj);
        }



        void Status(string msg)
        {
            CoreService.EventCore.FireInitializeStatusEvent(msg);
            logger.Info(msg);
        }


    }
}
