using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        public AccountItem Account { get; set; }

        /// <summary>
        /// 合约
        /// 委托 持仓 合约需要 记录 用于保持行情订阅
        /// </summary>
        public IEnumerable<Symbol> HotSymbols { get { return hashSetSymbol; } }

        public TradingInfoTracker()
        {
            ResumeEnd = false;
            OrderTracker = new OrderTracker();
            PositionTracker = new LSPositionTracker("");
            HoldPositionTracker = new LSPositionTracker("");
            TradeTracker = new ThreadSafeList<Trade>();
            TickTracker = new TickTracker();
            Account = new AccountItem();

            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(EventIndicator_GotFillEvent);
            CoreService.EventIndicator.GotPositionNotifyEvent += new Action<PositionEx>(EventIndicator_GotPositionNotifyEvent);
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(EventIndicator_GotTickEvent);

            CoreService.EventHub.OnRspXQryOrderResponse += new Action<Order, RspInfo, int, bool>(EventQry_OnRspXQryOrderResponse);
            CoreService.EventHub.OnRspXQryYDPositionResponse += new Action<PositionDetail, RspInfo, int, bool>(EventQry_OnRspXQryYDPositionResponse);
            CoreService.EventHub.OnRspXQryFillResponese += new Action<Trade, RspInfo, int, bool>(EventQry_OnRspXQryFillResponese);
            CoreService.EventHub.OnRspXQryAccountResponse += new Action<AccountItem, RspInfo, int, bool>(EventQry_OnRspXQryAccountResponse);
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
            if (accept)
            {
                OrderTracker.GotFill(obj);
                TradeTracker.Add(obj);
            }
        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            OrderTracker.GotOrder(obj);
        }

        void EventIndicator_GotPositionNotifyEvent(PositionEx obj)
        {
            Position pos = this.PositionTracker[obj.Symbol, obj.Account, obj.Side];
            BookPositionHoldSymbols(pos);
        }


        public bool ResumeEnd { get; set; }
        int _qrypositionid = 0;
        /// <summary>
        /// 请求恢复日内交易记录
        /// </summary>
        public void ResumeData()
        {
            CoreService.EventHub.FireResumeDataStart();
            this.ResumeEnd = false;
            CoreService.PositionWatcher.Enable = false;
            //重置维护期
            Reset();
            
            //执行隔夜持仓查询 并按序触发后续查询
            Status("查询隔夜持仓");
            _qrypositionid = CoreService.TLClient.ReqXQryYDPositon();
        }


        public void Reset()
        {
            //清空当前交易记录维护器
            OrderTracker.Clear();
            PositionTracker.Clear();
            HoldPositionTracker.Clear();
            TradeTracker.Clear();
            
            Account = null;
        }



        /// <summary>
        /// 持仓合约维护器
        /// 持仓不为零时 记录合约 用于订阅实时行情
        /// </summary>
        ConcurrentDictionary<string, Symbol> posHoldSymbols = new ConcurrentDictionary<string, Symbol>();
        HashSet<Symbol> hashSetSymbol = new HashSet<Symbol>();
        /// <summary>
        /// 记录持仓合约
        /// </summary>
        /// <param name="pos"></param>
        void BookPositionHoldSymbols(Position pos)
        {
            if (pos == null)
            {
                logger.Warn("Pos is null");
                return;
            }

            string key = pos.GetPositionKey();
            Symbol target = null;
            if (!pos.isFlat)
            {
                //有持仓,添加合约到字典
                posHoldSymbols.TryAdd(key, pos.oSymbol);
            }
            else
            {
                //无持仓,字典中删除合约
                posHoldSymbols.TryRemove(key, out target);
            }
            hashSetSymbol.Clear();
            foreach(var symbol in posHoldSymbols.Values)
            {
                hashSetSymbol.Add(symbol);
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
                //KeepSymbol(arg1.oSymbol);
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
                //KeepSymbol(arg1.oSymbol);
                if (accept)
                {
                    OrderTracker.GotFill(arg1);
                    TradeTracker.Add(arg1);
                }
            }
            if (arg4)
            {
                Status("成交查询完毕,查询账户");
                _qryaccountinfoid = CoreService.TLClient.ReqXQryAccount();
            }
        }

        void EventQry_OnRspXQryAccountResponse(AccountItem arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryaccountinfoid) return;
            CoreService.TradingInfoTracker.Account = arg1;
            
            //执行数据初始化
            //隔夜持仓查询完毕后 将隔夜持仓对应合约放入列表
            foreach (var pos in PositionTracker)
            {
                BookPositionHoldSymbols(pos);
            }


            logger.Info("交易信息查询完毕");
            CoreService.PositionWatcher.Enable = true;
            this.ResumeEnd = true;
            CoreService.EventHub.FireResumeDataEnd();

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
                    CoreService.Initialize();
                    Status("触发初始化完毕事件");
                }
            }

            
        }

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
