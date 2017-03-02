﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;



namespace TradingLib.TraderCore
{

    /// <summary>
    /// 持仓 止盈止损参数
    /// </summary>
    public class PositionOffsetArgSet
    {
        public PositionOffsetArgSet(Position pos)
        {
            this.Symbol = pos.Symbol;
            this.Account = pos.Account;
            this.Side = pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false;
            this.ProfitArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.PROFIT);
            this.LossArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.LOSS);

            argList.Add(this.ProfitArg);
            argList.Add(this.LossArg);
        }

        public void Reset()
        {
            foreach (var offset in this.argList)
            {

                offset.Enable = false;
                offset.Fired = false;
                offset.FlatOrderRefList.Clear();

            }
        }

        public string Account { get; set; }

        public string Symbol { get; set; }

        public bool Side { get; set; }

        /// <summary>
        /// 止盈止损参数列表
        /// </summary>
        ThreadSafeList<PositionOffsetArg> argList = new ThreadSafeList<PositionOffsetArg>();

        public IEnumerable<PositionOffsetArg> PositionOffsetArgList { get { return argList; } }

        /// <summary>
        /// 止盈参数
        /// </summary>
        public PositionOffsetArg ProfitArg { get; set; }

        /// <summary>
        /// 止损参数
        /// </summary>
        public PositionOffsetArg LossArg { get; set; }

        public override string ToString()
        {
            return string.Format("Profit:{0} Loss:{1}", this.ProfitArg != null ? this.ProfitArg.ToString() : "null", this.LossArg != null ? this.LossArg.ToString() : "null");
        }

    }


    /// <summary>
    /// 持仓检查中心
    /// 用于设定持仓止盈止损参数
    /// </summary>
    public class PositionWatcher
    {

        ILog logger = LogManager.GetLogger("PositionWatcher");

        public ConcurrentDictionary<string, PositionOffsetArgSet> ArgMap { get { return argsmap; } }

        /// <summary>
        /// 持仓 与 止盈止损参数映射 一个持仓有一个PositionOffsetArgs与之对应
        /// 行情线程与界面UI线程都会对止盈止损参数进行获取或者跟新操作为了避免多线程访问数据问题，这里使用线程安全字典类型
        /// </summary>
        ConcurrentDictionary<string, PositionOffsetArgSet> argsmap = new ConcurrentDictionary<string, PositionOffsetArgSet>();

        /// <summary>
        /// 将止盈止损参数 按合约进行分类 方便行情驱动
        /// </summary>
        ConcurrentDictionary<string, ThreadSafeList<PositionOffsetArgSet>> symbolargsmap = new ConcurrentDictionary<string, ThreadSafeList<PositionOffsetArgSet>>();

        
        public PositionWatcher()
        {
            //有持仓生成时 创建该持仓对应的止盈止损数据集
            CoreService.TradingInfoTracker.GotPositionEvent += new Action<Position>(NewPosition);

            //响应行情数据 监控持仓止盈止损状态
            CoreService.EventIndicator.GotTickEvent +=new Action<Tick>(GotTick);

            //响应行情数据
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFillEvent);

            //响应委托回报数据
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);

            Start();
            this.Enable = false;
        }

        public bool Enable { get; set; }
        void Start()
        {
            if (processgo) return;
            processgo = true;
            processThread = new Thread(ProcessArg);
            processThread.IsBackground = true;
            processThread.Start();
        }
        Thread processThread = null;
        bool processgo = false;

        void ProcessArg()
        {
            while (processgo)
            {
                if (this.Enable)
                {
                    foreach (var argset in argsmap.Values)
                    {
                        foreach (var arg in argset.PositionOffsetArgList)
                        {
                            try
                            {
                                //止损参数启用 且已经触发 同时超过5秒
                                if (arg.Enable && arg.Fired && DateTime.Now.Subtract(arg.SentTime).TotalSeconds > 5)
                                {
                                    var pos = CoreService.TradingInfoTracker.PositionTracker[argset.Symbol, argset.Account, argset.Side];
                                    if (pos != null)
                                    {
                                        if (!pos.isFlat)
                                        {
                                            logger.Info("~~~~~~~~~~~~~ ReFlat Postion");
                                            arg.FlatOrderRefList = FlatPosition(pos, arg.Size);
                                            arg.SentTime = DateTime.Now;
                                        }
                                        else
                                        {   //重置止损 止盈参数
                                            arg.Enable = false;
                                            arg.Fired = false;
                                            arg.FlatOrderRefList.Clear();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Error("ReFlat position error:" + ex.ToString());
                            }
                        }
                    }
                }
                Thread.Sleep(300);
            }
        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获得某个持仓的止盈止损数据
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public PositionOffsetArgSet GetPositionOffsetArgs(Position pos)
        {
            PositionOffsetArgSet target = null;
            if (argsmap.TryGetValue(pos.GetPositionKey(), out target))
            {
                return target;
            }
            return null;
        }

        /// <summary>
        /// 当有新的Position产生时 生成对应的止盈止损参数
        /// </summary>
        /// <param name="pos"></param>
        void NewPosition(Position pos)
        {
            //持仓没有对应的止盈止损数据则新增
            if (!argsmap.Keys.Contains(pos.GetPositionKey()))
            {
                PositionOffsetArgSet args = new PositionOffsetArgSet(pos);
                argsmap.TryAdd(pos.GetPositionKey(), args);

                if (!symbolargsmap.Keys.Contains(pos.Symbol))
                {
                    symbolargsmap.TryAdd(pos.Symbol, new ThreadSafeList<PositionOffsetArgSet>());
                }
                symbolargsmap[pos.Symbol].Add(args);

            }
        }


        /// <summary>
        /// 响应外部行情
        /// </summary>
        /// <param name="k"></param>
        void GotTick(Tick k)
        {
            if (!this.Enable) return;
            if (k == null || !k.IsValid()) return;
            if (k.UpdateType != "X" && k.UpdateType !="S") return;
            ThreadSafeList<PositionOffsetArgSet> target = null;
            if (symbolargsmap.TryGetValue(k.Symbol, out target))
            {
                foreach (var arg in target)
                {
                    ProcessTick(arg, k);
                }
            }
        }

        void ProcessTick(PositionOffsetArgSet args, Tick tick)
        {
            //非持仓对应合约 直接返回
            if (tick.Symbol != args.Symbol) return;

            foreach (var arg in args.PositionOffsetArgList)
            {
                //没有处于Enable状态 pass
                if (!arg.Enable) continue;

                //参数已经Fired委托 pass
                if (arg.Fired) continue;
                //检查
                var tpos = CoreService.TradingInfoTracker.PositionTracker[args.Symbol, args.Account, args.Side];
                if (tpos == null) return;
                arg.Fired = arg.NeedSendOrder(tpos, tick);

#if DEBUG
                logger.Info(string.Format("Arg:{0} TargetPrice:{1} Trade:{2} Fired:{3}", arg, arg.TargetPrice(tpos), tick.Trade, arg.Fired));
#endif

                if (arg.Fired)
                {
                    arg.FlatOrderRefList = FlatPosition(tpos, arg.Size);
                    arg.SentTime = DateTime.Now;
                }
            }
        }

        void GotOrder(Order o)
        {
            if (!this.Enable) return;
            if (o.Status == QSEnumOrderStatus.Filled)
            {
                ThreadSafeList<PositionOffsetArgSet> target = null;
                if (symbolargsmap.TryGetValue(o.Symbol, out target))
                {
                    foreach (var arg in target)
                    {
                        ProcessOrder(arg, o);
                    }
                }
            }
        }

        void ProcessOrder(PositionOffsetArgSet args, Order o)
        {
            
            var tpos = CoreService.TradingInfoTracker.PositionTracker[args.Symbol, args.Account, args.Side];
            if (tpos == null) return;
            //非持仓对应合约 直接返回
            if (o.Symbol != tpos.Symbol) return;

            List<string> removelsit = new List<string>();
            foreach (var arg in args.PositionOffsetArgList)
            {
                //未触发 不执行委托完成检查
                if (!arg.Fired) continue;
                
                string key = string.Format("{0}-{1}-{2}", CoreService.TLClient.FrontID, CoreService.TLClient.SessionID, o.OrderRef);
                logger.Info("******** got order ref:" + key);
                if (arg.FlatOrderRefList.Contains(key))
                {
                    arg.FlatOrderRefList.Remove(key);
                }
                //所有委托已经执行完毕 重置arg
                if (arg.FlatOrderRefList.Count == 0)
                {
                    arg.Enable = false;
                    arg.Fired = false;
                }
            }
        }

        void GotFillEvent(Trade obj)
        {
            if (!this.Enable) return;
            Position pos = CoreService.TradingInfoTracker.PositionTracker[obj.Symbol,obj.Account,obj.PositionSide];
            //当持仓归零时重置止盈止损参数
            if(pos!=null && pos.isFlat)
            {
                var arg = GetPositionOffsetArgs(pos);
                arg.Reset();
            }
        }

        


        List<string> FlatPosition(Position pos,int size)
        {
            if (!CoreService.TLClient.LastOrderNotified)
            {
                CoreService.TLClient.Stop();
                CoreService.TLClient.Start();
                return new List<string>();
            }

            logger.Info("FlatPositon:" + pos.GetPositionKey());
            List<string> reflist = new List<string>();
            if (pos == null || pos.isFlat) return reflist;

            bool side = pos.isLong ? true : false;
            //上期所区分平今平昨
            if (pos.oSymbol.SecurityFamily.Exchange.EXCode == "SHFE")
            {
                int voltd = pos.PositionDetailTodayNew.Sum(p => p.Volume);//今日持仓
                int volyd = pos.PositionDetailYdNew.Sum(p => p.Volume);//昨日持仓
                //Tick snapshot = TLCtxHelper.ModuleDataRouter.GetTickSnapshot(pos.Account);
                if (volyd != 0)
                {
                    Order oyd = new OrderImpl(pos.Symbol, volyd * (side ? 1 : -1) * -1);
                    oyd.Exchange = pos.oSymbol.Exchange;
                    oyd.OffsetFlag = QSEnumOffsetFlag.CLOSE;

                    CoreService.TLClient.ReqOrderInsert(oyd);
                    reflist.Add(string.Format("{0}-{1}-{2}", CoreService.TLClient.FrontID, CoreService.TLClient.SessionID, oyd.OrderRef));
                }
                if (voltd != 0)
                {
                    Order otd = new OrderImpl(pos.Symbol, voltd * (side ? 1 : -1) * -1);
                    otd.Exchange = pos.oSymbol.Exchange;
                    otd.OffsetFlag = QSEnumOffsetFlag.CLOSETODAY;

                    CoreService.TLClient.ReqOrderInsert(otd);
                    reflist.Add(string.Format("{0}-{1}-{2}", CoreService.TLClient.FrontID, CoreService.TLClient.SessionID, otd.OrderRef));
                }
            }
            else
            {
                Order o = new MarketOrderFlat(pos);
                o.Exchange = pos.oSymbol.Exchange;
                size = Math.Abs(size);
                if (size > 0 && size<o.UnsignedSize)//如果size有设定(不为0且小于持仓数量);
                {
                    o.Size = size * (o.Side ? 1 : -1);
                }
                CoreService.TLClient.ReqOrderInsert(o);
                reflist.Add(string.Format("{0}-{1}-{2}", CoreService.TLClient.FrontID, CoreService.TLClient.SessionID, o.OrderRef));
            }

            return reflist;
        }
    }
}
