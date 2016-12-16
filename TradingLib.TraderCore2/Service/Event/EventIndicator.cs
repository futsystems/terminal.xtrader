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

        public void FireTick(Tick k)
        {
            if (GotTickEvent != null)
                GotTickEvent(k);
        }


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


        public event Action<PositionEx> GotPositionNotifyEvent = delegate { };

        

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

        internal void FirePositionNotify(PositionEx notify)
        {
            GotPositionNotifyEvent(notify);
        }
    }
}
