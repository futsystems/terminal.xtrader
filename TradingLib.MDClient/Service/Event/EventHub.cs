using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.DataCore
{
    public class EventHub
    {

        /// <summary>
        /// 通讯连接建立事件
        /// </summary>
        public event Action OnConnectedEvent;
        internal void FireConnectedEvent()
        {
            if (OnConnectedEvent != null)
                OnConnectedEvent();
        }

        /// <summary>
        /// 通讯连接断开事件
        /// </summary>
        public event Action OnDisconnectedEvent;
        internal void FireDisconnectedEvent()
        {
            if (OnDisconnectedEvent != null)
                OnDisconnectedEvent();
        }

        public event Action<LoginResponse> OnLoginEvent;
        internal void FireLoginEvent(LoginResponse response)
        {
            if (OnLoginEvent != null)
                OnLoginEvent(response);
        }

        public event Action OnInitializedEvent;

        internal void FireOnInitializedEvent()
        {
            if (OnInitializedEvent != null)
                OnInitializedEvent();
        }
        /// <summary>
        /// 响应实时行情数据
        /// </summary>
        public event Action<Tick> OnRtnTickEvent;


        

        
        internal void FireRtnTickEvent(Tick k)
        {
            if (OnRtnTickEvent != null)
                OnRtnTickEvent(k);
        }


        /// <summary>
        /// 响应Bar数据查询
        /// </summary>
        public event Action<RspQryBarResponseBin> OnRspBarEvent;
        internal void FireOnRspBarEvent(RspQryBarResponseBin barResponse)
        {
            if (OnRspBarEvent != null)
                OnRspBarEvent(barResponse);
        }

        /// <summary>
        /// 响应成交数据查询
        /// </summary>
        public event Action<RspXQryTradeSplitResponse> OnRspTradeSplitEvent;
        internal void FireOnRspTradeSplitEvent(RspXQryTradeSplitResponse tradeResponse)
        {
            if (OnRspTradeSplitEvent != null)
                OnRspTradeSplitEvent(tradeResponse);
        }
        
    }
}
