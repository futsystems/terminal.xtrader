using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    /// <summary>
    /// 交易类信息事件中继
    /// 底层接口通过CoreService单例进行事件暴露，需要订阅事件的组件通过CoreService进行订阅
    /// 实现解耦
    /// </summary>
    public class EventIndicator
    {
        /// <summary>
        /// 行情事件
        /// </summary>
        public event Action<Tick> GotTickEvent;

        /// <summary>
        /// 委托事件
        /// </summary>
        public event Action<Order> GotOrderEvent;

        /// <summary>
        /// 委托错误事件
        /// </summary>
        public event Action<Order,RspInfo> GotErrorOrderEvent;


        /// <summary>
        /// 委托操作错误事件
        /// </summary>
        public event Action<OrderAction, RspInfo> GotErrorOrderActionEvent;

        /// <summary>
        /// 成交事件
        /// </summary>
        public event Action<Trade> GotFillEvent;

        /// <summary>
        /// 持仓明细事件
        /// </summary>
        public event Action<PositionDetail> GotPositionDetailEvent;

        internal void FireTick(Tick k)
        {
            if (GotTickEvent != null)
                GotTickEvent(k);
        }

        internal void FireOrder(Order o)
        {
            if (GotOrderEvent != null)
                GotOrderEvent(o);
        }

        internal void FireFill(Trade f)
        {
            if (GotFillEvent != null)
                GotFillEvent(f);
        }

        internal void FirePositionDetail(PositionDetail p)
        {
            if (GotPositionDetailEvent != null)
                GotPositionDetailEvent(p);
        }

        internal void FireErrorOrder(Order o,RspInfo e)
        {
            if (GotErrorOrderEvent != null)
                GotErrorOrderEvent(o, e);
        }

        internal void FireErrorOrderAction(OrderAction a, RspInfo e)
        {
            if (GotErrorOrderActionEvent != null)
                GotErrorOrderActionEvent(a, e);
        }


        public event Action<Order, bool> OnHistOrderEvent;
        internal void FireHistOrderEvent(Order o, bool islast)
        {
            if (OnHistOrderEvent != null)
                OnHistOrderEvent(o, islast);
        }
        public event Action<Trade, bool> OnHistTradeEvent;
        internal void FireHistTradeEvent(Trade f, bool islast)
        {
            if (OnHistTradeEvent != null)
                OnHistTradeEvent(f, islast);
        }
        public event Action<PositionDetail, bool> OnHistPositionEvent;
        internal void FireHistPositionEvent(PositionDetail p, bool islast)
        {
            if (OnHistPositionEvent != null)
                OnHistPositionEvent(p, islast);
        }

        public event Action<RspMGRQrySettleResponse> OnSettlementEvent;
        internal void FireSettlementEvent(RspMGRQrySettleResponse resp)
        {
            if (OnSettlementEvent != null)
                OnSettlementEvent(resp);
        }

    }
}
