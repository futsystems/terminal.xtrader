using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;



namespace TradingLib.TraderCore
{

    /// <summary>
    /// 持仓 止盈止损参数
    /// </summary>
    public class PositionOffsetArgs
    {
        public PositionOffsetArgs(Position pos)
        {
            this.Position = pos;
            this.ProfitArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.PROFIT);
            this.LossArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.LOSS);

            argList.Add(this.ProfitArg);
            argList.Add(this.LossArg);
        }


        /// <summary>
        /// 对应持仓对象
        /// </summary>
        public Position Position { get; set; }

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

        //public long ProfitTakeOrder { get; set; }//是否已经发送止盈委托
        //public int ProfitFireCount { get; set; }//止盈委托触发次数
        //public bool NeedTakeProfit { get; set; }//是否需要出发止盈委托
        //public DateTime ProfitTakeTime { get; set; }//止盈委托发出时间

        //public decimal ProfitTakePrice
        //{
        //    get
        //    {
        //        return this.ProfitArg.TargetPrice(this.Position);
        //    }
        //}

        //public long LossTakeOrder { get; set; }
        //public int LossFireCount { get; set; }
        //public bool NeedTakeLoss { get; set; }
        //public DateTime LossTakeTime { get; set; }
        //public decimal LossTakePrice
        //{
        //    get
        //    {
        //        return this.LossArg.TargetPrice(this.Position);
        //    }
        //}

        ///// <summary>
        ///// 是否设置了有效的止盈止损参数
        ///// 如果设置了止盈止损参数则系统需要检查是否触发了止盈或止损
        ///// </summary>
        //public bool NeedCheck
        //{
        //    get
        //    {
        //        if ((this.ProfitArg != null && this.ProfitArg.Enable) || (this.LossArg != null && this.LossArg.Enable))
        //            return true;
        //        else
        //            return false;
        //    }
        //}


        ///// <summary>
        ///// 检查止损
        ///// </summary>
        //public bool CheckTakeLoss(Tick k)
        //{
        //    if (!this.LossArg.Enable) return false;

        //    decimal hitprice = LossTakePrice;
        //    bool side = this.Position.DirectionType == QSEnumPositionDirectionType.Long;
        //    if (side)
        //    {
        //        if (k.Trade <= hitprice)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        if (k.Trade >= hitprice)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 检查止盈
        ///// </summary>
        ///// <returns></returns>
        //public bool CheckTakeProfit(Tick k)
        //{
        //    if (!this.ProfitArg.Enable) return false;//止盈未启用

        //    bool side = this.Position.DirectionType == QSEnumPositionDirectionType.Long;

        //    if (ProfitArg.OffsetType == QSEnumPositionOffsetType.TRAILING)
        //    {
        //        decimal hitprice = ProfitTakePrice;
        //        if (hitprice > 0)
        //        {
        //            if (k.Trade <= hitprice)
        //            {
        //                return true;//执行止盈
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Util.Debug("profittakeprice:" + ProfitTakePrice.ToString() + " profit arg enable:" + this.ProfitArg.Enable.ToString());
        //        decimal hitprice = ProfitTakePrice;
        //        if (side)
        //        {
        //            if (k.Trade >= hitprice)
        //            {
        //                return true;//执行止盈
        //            }
        //            return false;
        //        }
        //        else//空
        //        {
        //            if (k.Trade <= hitprice)
        //            {
        //                return true;
        //            }
        //            return false;
        //        }
        //    }
        //    return false;//不止盈
        //}


        ///// <summary>
        ///// 重置positionoffset 参数
        ///// </summary>
        //public void Reset()
        //{
        //    ProfitArg.Enable = false;
        //    NeedTakeProfit = false;
        //    ProfitTakeOrder = -1;
        //    ProfitFireCount = 0;
        //    ProfitTakeTime = DateTime.Now;

        //    LossArg.Enable = false;
        //    NeedTakeLoss = false;
        //    LossTakeOrder = -1;
        //    LossFireCount = 0;
        //    LossTakeTime = DateTime.Now;
        //}

    }


    /// <summary>
    /// 持仓检查中心
    /// 用于设定持仓止盈止损参数
    /// </summary>
    public class PositionWatcher
    {

        ILog logger = LogManager.GetLogger("PositionWatcher");

        /// <summary>
        /// 持仓 与 止盈止损参数映射 一个持仓有一个PositionOffsetArgs与之对应
        /// </summary>
        Dictionary<string, PositionOffsetArgs> argsmap = new Dictionary<string, PositionOffsetArgs>();

        /// <summary>
        /// 将止盈止损参数 按合约进行分类
        /// </summary>
        Dictionary<string, ThreadSafeList<PositionOffsetArgs>> symbolargsmap = new Dictionary<string, ThreadSafeList<PositionOffsetArgs>>();


        public PositionWatcher()
        {
            //有持仓生成时 创建该持仓对应的止盈止损数据集
            CoreService.TradingInfoTracker.GotPositionEvent += new Action<Position>(NewPosition);
            //响应行情数据 监控持仓止盈止损状态
            CoreService.EventIndicator.GotTickEvent +=new Action<Tick>(GotTick);

            //响应委托回报数据
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);

        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得某个持仓的止盈止损数据
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public PositionOffsetArgs GetPositionOffsetArgs(Position pos)
        {
            PositionOffsetArgs target = null;
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
                PositionOffsetArgs args = new PositionOffsetArgs(pos);
                argsmap.Add(pos.GetPositionKey(), args);


                if (!symbolargsmap.Keys.Contains(pos.Symbol))
                {
                    symbolargsmap.Add(pos.Symbol, new ThreadSafeList<PositionOffsetArgs>());
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
            if (k == null || !k.IsValid()) return;
            if (k.UpdateType != "X") return;
            ThreadSafeList<PositionOffsetArgs> target = null;
            if (symbolargsmap.TryGetValue(k.Symbol, out target))
            {
                foreach (var arg in target)
                {
                    ProcessTick(arg, k);
                }
            }
        }



        void GotOrder(Order o)
        {
            if (o.Status == QSEnumOrderStatus.Filled)
            {
                ThreadSafeList<PositionOffsetArgs> target = null;
                if (symbolargsmap.TryGetValue(o.Symbol, out target))
                {
                    foreach (var arg in target)
                    {
                        ProcessOrder(arg, o);
                    }
                }
            }
        }

        void ProcessOrder(PositionOffsetArgs args, Order o)
        {
            //非持仓对应合约 直接返回
            if (o.Symbol != args.Position.Symbol) return;

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

        void ProcessTick(PositionOffsetArgs args,Tick tick)
        {
            //非持仓对应合约 直接返回
            if (tick.Symbol != args.Position.Symbol) return;

            //无持仓 重置所有止盈止损设置
            //if (args.Position.isFlat)
            //{
            //    foreach (var arg in args.PositionOffsetArgList)
            //    {
            //        //重置
            //        arg.Enable = false;
            //        arg.Fired = false;
            //        arg.FlatOrderRefList.Clear();
            //    }
            //}

            foreach (var arg in args.PositionOffsetArgList)
            {
                //没有处于Enable状态 pass
                if (!arg.Enable) continue;

                //参数已经Fired委托 pass
                if (arg.Fired) continue;

                //检查
                arg.Fired = arg.NeedSendOrder(args.Position, tick);
                if (arg.Fired)
                {
                    arg.FlatOrderRefList = FlatPosition(args.Position, arg.Size);
                }
            }

            ////非持仓对应合约 直接返回
            //if (tick.Symbol != args.Position.Symbol) return;

            ////1.是否设置有效止盈止损
            //bool needcheck = args.NeedCheck;

            //if (!needcheck) return;//没有有效止盈与止损参数 直接返回

            ////是否触发平仓操作
            //bool flatsend = args.NeedTakeProfit || args.NeedTakeLoss;


            ////如果触发平仓操作并 仓位已经 flat,则重置参数 ->  触发了止盈与止损并平仓了 则表明是止盈或止损平仓
            //if (flatsend && args.Position.isFlat)
            //{
            //    logger.Info("触发平仓操作成功,重置止盈止损参数");
            //    args.Reset();
            //    return;
            //}

            ////如果设置了止盈止损,但是没有持仓了 则重置止盈止损 -> 手工或者其他方式平仓
            //if (args.Position.isFlat)
            //{
            //    logger.Info("持仓被平仓,止盈止损重置");
            //    args.Reset();
            //    return;
            //}

            ////没有触发平仓,并且有持仓 则我们进行检查
            //if (!flatsend)
            //{
            //    args.NeedTakeLoss = args.CheckTakeLoss(tick);
            //    if (args.NeedTakeLoss)
            //    {
            //        args.LossFireCount++;
            //        args.LossTakeTime = DateTime.Now;
            //        args.LossTakeOrder = FlatPosition(args.Position,args.LossArg.Size,"客户端止损");
                    
            //        logger.Info("触发客户端止损:" + args.LossTakePrice.ToString() + " " + args.LossTakeOrder.ToString());
            //    }

            //    args.NeedTakeProfit = args.CheckTakeProfit(tick);//检查是否需要止盈
            //    //如果需要止盈则发送委托
            //    if (args.NeedTakeProfit)
            //    {
            //        args.ProfitFireCount++;//累加触发次数
            //        args.ProfitTakeTime = DateTime.Now;//记录触发时间
            //        args.ProfitTakeOrder = FlatPosition(args.Position,args.ProfitArg.Size, "客户端止盈");//平仓
            //        logger.Info("触发客户端止盈" + args.ProfitTakePrice.ToString() + " " + args.ProfitTakeOrder.ToString());

            //    }
            //    return;
            //}
        }


        List<string> FlatPosition(Position pos,int size)
        {
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
