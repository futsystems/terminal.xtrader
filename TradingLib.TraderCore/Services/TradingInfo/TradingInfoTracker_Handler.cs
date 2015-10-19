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

        void Status(string msg)
        {
            CoreService.EventCore.FireInitializeStatusEvent(msg);
            logger.Info(msg);
        }

        public TickTracker TickTracker { get { return _tickTracker; } }

        TickTracker _tickTracker = new TickTracker();

        #region 实时数据处理 实时数据需要通过事件中继对外触发
        /// <summary>
        /// 响应行情
        /// </summary>
        /// <param name="k"></param>
        public void NotifyTick(Tick k)
        {
            _tickTracker.GotTick(k);
            PositionTracker.GotTick(k);
            //CoreService.EventIndicator.FireTick(k);
        }


        /// <summary>
        /// 获得持仓回报
        /// </summary>
        /// <param name="o"></param>
        public void NotifyOrder(Order o)
        {
            OrderTracker.GotOrder(o);
            //CoreService.EventIndicator.FireOrder(o);
        }


        /// <summary>
        /// 获得成交回报
        /// </summary>
        /// <param name="f"></param>
        public void NotifyFill(Trade f)
        {
            OrderTracker.GotFill(f);
            PositionTracker.GotFill(f);
            TradeTracker.Add(f);
            //CoreService.EventIndicator.FireFill(f);
        }
        #endregion


        #region 历史数据处理 历史数据用于恢复交易帐户当前状态
        /// <summary>
        /// 获得隔夜持仓数据
        /// </summary>
        /// <param name="pos"></param>
        public void GotYDPosition(PositionDetail pos,bool islast)
        {
            if (pos != null)
            {
                PositionTracker.GotPosition(pos);
                HoldPositionTracker.GotPosition(pos);
            }
            if (islast)
            {
                Status("隔夜持仓查询完毕,查询委托");
                CoreService.TLClient.ReqXQryOrder();
            }
        }

        public void GotOrder(Order o, bool islast)
        {
            if (o != null)
            {
                OrderTracker.GotOrder(o);
            }
            if (islast)
            {
                Status("委托查询完毕,查询成交");
                CoreService.TLClient.ReqXQryTrade();
            }
        }

        public void GotTrade(Trade f, bool islast)
        {
            if (f != null)
            {
                OrderTracker.GotFill(f);
                PositionTracker.GotFill(f);
                TradeTracker.Add(f);
            }
            if (islast)
            {
                Status("成交查询完毕,查询帐户信息");

                CoreService.TLClient.ReqQryAccountInfo();
            }
        }

        public void GotAccountInfo(AccountInfo info,bool islast)
        {
            CoreService.AccountInfo = info;
            if (!CoreService.Initialized)
            {
                if (info == null)
                {
                    Status("帐户信息查询异常");
                    return;
                }

                if (islast) //没有初始化完毕则需要触发一下操作
                {
                    Status("帐户信息查询完毕");

                    CoreService.TLClient.StartTick();
                    //核心服务完成初始化
                    CoreService.Initialize();
                    Status("触发初始化完毕事件");
                }
            }
            else
            {
                CoreService.EventOther.FireAccountInfoEvent(info);  
            }
        }

        #endregion

    }
}
