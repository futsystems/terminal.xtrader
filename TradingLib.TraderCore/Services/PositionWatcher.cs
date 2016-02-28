using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;



namespace TradingLib.TraderCore
{

    public class PositionOffsetArgs
    {
        public PositionOffsetArgs(Position pos)
        {
            this.Position = pos;
            this.ProfitArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.PROFIT);
            this.LossArg = new PositionOffsetArg(pos.Account, pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false, QSEnumPositionOffsetDirection.LOSS);
        }


        /// <summary>
        /// 对应持仓对象
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 止盈参数
        /// </summary>
        public PositionOffsetArg ProfitArg { get; set; }

        /// <summary>
        /// 止损参数
        /// </summary>
        public PositionOffsetArg LossArg { get; set; }

        public long ProfitTakeOrder { get; set; }//是否已经发送止盈委托
        public int ProfitFireCount { get; set; }//止盈委托触发次数
        public bool NeedTakeProfit { get; set; }//是否需要出发止盈委托
        public DateTime ProfitTakeTime { get; set; }//止盈委托发出时间

        public decimal ProfitTakePrice
        {
            get
            {
                return this.ProfitArg.TargetPrice(this.Position);
            }
        }

        public long LossTakeOrder { get; set; }
        public int LossFireCount { get; set; }
        public bool NeedTakeLoss { get; set; }
        public DateTime LossTakeTime { get; set; }
        public decimal LossTakePrice
        {
            get
            {
                return this.LossArg.TargetPrice(this.Position);
            }
        }

        /// <summary>
        /// 是否设置了有效的止盈止损参数
        /// 如果设置了止盈止损参数则系统需要检查是否触发了止盈或止损
        /// </summary>
        public bool NeedCheck
        {
            get
            {
                if ((this.ProfitArg != null && this.ProfitArg.Enable) || (this.LossArg != null && this.LossArg.Enable))
                    return true;
                else
                    return false;
            }
        }


        /// <summary>
        /// 检查止损
        /// </summary>
        public bool CheckTakeLoss(Tick k)
        {
            if (!this.LossArg.Enable) return false;

            decimal hitprice = LossTakePrice;
            bool side = this.Position.DirectionType == QSEnumPositionDirectionType.Long;
            if (side)
            {
                if (k.Trade <= hitprice)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (k.Trade >= hitprice)
                {
                    return true;
                }
                return false;
            }
            
            
        }

        /// <summary>
        /// 检查止盈
        /// </summary>
        /// <returns></returns>
        public bool CheckTakeProfit(Tick k)
        {
            if (!this.ProfitArg.Enable) return false;//止盈未启用

            bool side = this.Position.DirectionType == QSEnumPositionDirectionType.Long;

            if (ProfitArg.OffsetType == QSEnumPositionOffsetType.TRAILING)
            {
                decimal hitprice = ProfitTakePrice;
                if (hitprice > 0)
                {
                    if (k.Trade <= hitprice)
                    {
                        return true;//执行止盈
                    }
                }
            }
            else
            {
                Util.Debug("profittakeprice:" + ProfitTakePrice.ToString() + " profit arg enable:" + this.ProfitArg.Enable.ToString());
                decimal hitprice = ProfitTakePrice;
                if (side)
                {
                    if (k.Trade >= hitprice)
                    {
                        return true;//执行止盈
                    }
                    return false;
                }
                else//空
                {
                    if (k.Trade <= hitprice)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;//不止盈
        }


        /// <summary>
        /// 重置positionoffset 参数
        /// </summary>
        public void Reset()
        {
            ProfitArg.Enable = false;
            NeedTakeProfit = false;
            ProfitTakeOrder = -1;
            ProfitFireCount = 0;
            ProfitTakeTime = DateTime.Now;

            LossArg.Enable = false;
            NeedTakeLoss = false;
            LossTakeOrder = -1;
            LossFireCount = 0;
            LossTakeTime = DateTime.Now;
        }

    }


    /// <summary>
    /// 持仓检查中心
    /// 用于设定持仓止盈止损参数
    /// </summary>
    public class PositionWatcher
    {

        ILog logger = LogManager.GetLogger("PositionCheckCentre");

        Dictionary<string, PositionOffsetArgs> argsmap = new Dictionary<string, PositionOffsetArgs>();

        Dictionary<string, ThreadSafeList<PositionOffsetArgs>> symbolargsmap = new Dictionary<string, ThreadSafeList<PositionOffsetArgs>>();


        public PositionWatcher()
        {
            //有持仓生成时 创建该持仓对应的止盈止损数据集
            CoreService.TradingInfoTracker.GotPositionEvent += new Action<Position>(NewPosition);
            //响应行情数据 监控持仓止盈止损状态
            CoreService.EventIndicator.GotTickEvent +=new Action<Tick>(GotTick);

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
            ThreadSafeList<PositionOffsetArgs> target = null;
            if (symbolargsmap.TryGetValue(k.Symbol, out target))
            {
                foreach (var arg in target)
                {
                    ProcessTick(arg, k);
                }
            }
        }

        void ProcessTick(PositionOffsetArgs args,Tick tick)
        {
            //非持仓对应合约 直接返回
            if (tick.Symbol != args.Position.Symbol) return;

            //1.是否设置有效止盈止损
            bool needcheck = args.NeedCheck;

            if (!needcheck) return;//没有有效止盈与止损参数 直接返回

            //是否触发平仓操作
            bool flatsend = args.NeedTakeProfit || args.NeedTakeLoss;


            //如果触发平仓操作并 仓位已经 flat,则重置参数 ->  触发了止盈与止损并平仓了 则表明是止盈或止损平仓
            if (flatsend && args.Position.isFlat)
            {
                logger.Info("触发平仓操作成功,重置止盈止损参数");
                args.Reset();
                return;
            }

            //如果设置了止盈止损,但是没有持仓了 则重置止盈止损 -> 手工或者其他方式平仓
            if (args.Position.isFlat)
            {
                logger.Info("持仓被平仓,止盈止损重置");
                args.Reset();
                return;
            }

            //没有触发平仓,并且有持仓 则我们进行检查
            if (!flatsend)
            {
                args.NeedTakeLoss = args.CheckTakeLoss(tick);
                if (args.NeedTakeLoss)
                {
                    args.LossFireCount++;
                    args.LossTakeTime = DateTime.Now;
                    args.LossTakeOrder = FlatPosition(args.Position,args.LossArg.Size,"客户端止损");
                    
                    logger.Info("触发客户端止损:" + args.LossTakePrice.ToString() + " " + args.LossTakeOrder.ToString());
                }

                args.NeedTakeProfit = args.CheckTakeProfit(tick);//检查是否需要止盈
                //如果需要止盈则发送委托
                if (args.NeedTakeProfit)
                {
                    args.ProfitFireCount++;//累加触发次数
                    args.ProfitTakeTime = DateTime.Now;//记录触发时间
                    args.ProfitTakeOrder = FlatPosition(args.Position,args.ProfitArg.Size, "客户端止盈");//平仓
                    logger.Info("触发客户端止盈" + args.ProfitTakePrice.ToString() + " " + args.ProfitTakeOrder.ToString());

                }
                return;
            }

            //如果标记需要执行止盈操作 并且仍然有持仓 则检查是否需要重新发送委托
            //if (args.NeedTakeProfit)
            //{
            //    //如果时间超过重发间隔,重新发送委托
            //    if (DateTime.Now.Subtract(args.ProfitTakeTime).TotalSeconds > PositionOffset.SendOrderDelay && po.ProfitFireCount < PositionOffset.SendRetry)
            //    {
            //        if (po.ProfitTakeOrder > 0)//表明发送过委托 但是由于某些原因没有被成交,则取消该委托后重新发送委托
            //        {
            //            CancelOrder(po.ProfitTakeOrder);
            //            if (WaitAfterCancel) return;
            //        }
            //        else
            //        {
            //            po.ProfitFireCount++;//累加触发次数
            //            po.ProfitTakeTime = DateTime.Now;//记录触发时间
            //            po.ProfitTakeOrder = FlatPosition(po.Position, "服务端止盈");//平仓
            //        }
            //    }
            //    if (po.ProfitFireCount == PositionOffset.SendRetry)
            //    {
            //        if (po.ProfitTakeOrder > 0)//表明发送过委托 但是由于某些原因没有被成交,则取消该委托后重新发送委托
            //        {
            //            CancelOrder(po.ProfitTakeOrder);
            //        }
            //        //达到最大触发次数 记录错误并报警
            //        debug(po.ToString() + "多次触发 异常", QSEnumDebugLevel.ERROR);
            //        po.Reset();
            //    }

            //}
            ////如果标记需要执行止损操作 并且仍然有持仓 则检查是否需要重新发送委托
            //if (po.NeedTakeLoss)
            //{
            //    if (DateTime.Now.Subtract(po.LossTakeTime).TotalSeconds > PositionOffset.SendOrderDelay && po.LossFireCount < PositionOffset.SendRetry)
            //    {
            //        if (po.LossTakeOrder > 0)
            //        {
            //            CancelOrder(po.LossTakeOrder);
            //            if (WaitAfterCancel) return;
            //        }
            //        else
            //        {
            //            po.LossFireCount++;
            //            po.LossTakeTime = DateTime.Now;
            //            po.LossTakeOrder = FlatPosition(po.Position, "服务端止损");
            //        }
            //    }
            //    if (po.LossFireCount == PositionOffset.SendRetry)
            //    {
            //        //达到最大触发次数 记录错误并报警
            //        if (po.LossTakeOrder > 0)
            //        {
            //            CancelOrder(po.LossTakeOrder);
            //        }
            //        debug(po.ToString() + "多次触发 异常", QSEnumDebugLevel.ERROR);
            //        po.Reset();
            //    }
            //}


        }


        long FlatPosition(Position pos,int size,string comment)
        {
            logger.Info("FlatPositon:" + pos.GetPositionKey());
            if (pos == null || pos.isFlat) return -1;

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
                    oyd.OffsetFlag = QSEnumOffsetFlag.CLOSE;

                    CoreService.TLClient.ReqOrderInsert(oyd);
                }
                if (voltd != 0)
                {
                    Order otd = new OrderImpl(pos.Symbol, voltd * (side ? 1 : -1) * -1);
                    otd.OffsetFlag = QSEnumOffsetFlag.CLOSETODAY;

                    CoreService.TLClient.ReqOrderInsert(otd);
                }
            }
            else
            {
                Order o = new MarketOrderFlat(pos);
                size = Math.Abs(size);
                if (size > 0 && size<o.UnsignedSize)//如果size有设定(不为0且小于持仓数量);
                {

                    o.Size = size * (o.Side ? 1 : -1);
                }
                CoreService.TLClient.ReqOrderInsert(o);
            }

            return 0;
        }
    }
}
