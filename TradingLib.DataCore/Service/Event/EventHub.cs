using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.DataCore
{
    public interface IDataCallback
    {
        void OnConnected();
        void OnDisconnected();
        void OnLogin(LoginResponse response);
        void OnInitialized();

        void OnRtnTick(Tick k);
        void OnRspBar(RspQryBarResponseBin response);
        void OnRspTradeSplit(RspXQryTradeSplitResponse response);
        void OnRspPriceVol(RspXQryPriceVolResponse response);
        void OnRspMinuteData(RspXQryMinuteDataResponse response);
    }

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

        public event Action<TickData> OnRtnTickDataEvent;

        internal void FireRtnTickDataEvent(TickData k)
        {
            if (OnRtnTickDataEvent != null)
            {
                OnRtnTickDataEvent(k);
            }
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


        /// <summary>
        /// 响应价格成交量查询
        /// </summary>
        public event Action<RspXQryPriceVolResponse> OnRspPriceVolEvent;
        internal void FireOnRspPriceVolEvent(RspXQryPriceVolResponse pvResponse)
        {
            if (OnRspPriceVolEvent != null)
            {
                OnRspPriceVolEvent(pvResponse);
            }
        }

        /// <summary>
        /// 分时数据响应
        /// </summary>
        public event Action<RspXQryMinuteDataResponse> OnRspMinuteDataEvent;
        internal void FireOnRspMinuteDataEvent(RspXQryMinuteDataResponse mdResponse)
        {
            if (OnRspMinuteDataEvent != null)
            {
                OnRspMinuteDataEvent(mdResponse);
            }
        }
    }
}
