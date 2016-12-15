using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    /// <summary>
    /// 全局服务单例
    /// 
    /// </summary>
    public class CoreService
    {
        static CoreService defaultinstance = null;

        static CoreService()
        {
            defaultinstance = new CoreService();
        }

        
        bool _isinited = false;
        public static bool Initialized
        {
            get
            {
                return defaultinstance._isinited;
            }
        }

        /// <summary>
        /// 初始化完毕
        /// </summary>
        internal static void Initialize()
        {
            defaultinstance._isinited = true;
            CoreService.EventCore.FireInitializedEvent();
        }

        EventCore _eventCore = null;
        /// <summary>
        /// 底层核心事件
        /// </summary>
        public static EventCore EventCore
        {
            get
            {
                if (defaultinstance._eventCore == null)
                    defaultinstance._eventCore = new EventCore();
                return defaultinstance._eventCore;
            }
        }


        EventHub _eventHub = null;
        /// <summary>
        /// 查询事件中继
        /// </summary>
        public static EventHub EventHub
        {
            get
            {
                if (defaultinstance._eventHub == null)
                    defaultinstance._eventHub = new EventHub();
                return defaultinstance._eventHub;
            }
        }

        EventIndicator _eventIndicator = null;

        /// <summary>
        /// 交易类事件
        /// </summary>
        public static EventIndicator EventIndicator
        {
            get
            {
                if (defaultinstance._eventIndicator == null)
                    defaultinstance._eventIndicator = new EventIndicator();
                return defaultinstance._eventIndicator;
            }
        }


        BasicInfoTracker _basicinfortracker = null;

        /// <summary>
        /// 基础数据维护器
        /// 通过该维护器可以访问到基本数据 比如品种 合约 交易所等基础数据
        /// </summary>
        public static BasicInfoTracker BasicInfoTracker
        {
            get
            {
                if (defaultinstance._basicinfortracker == null)
                    defaultinstance._basicinfortracker = new BasicInfoTracker();
                return defaultinstance._basicinfortracker;
            }
        }


        TradingInfoTracker _tradinginfotracker = null;

        /// <summary>
        /// 交易数据维护器
        /// </summary>
        public static TradingInfoTracker TradingInfoTracker
        {
            get
            {
                if (defaultinstance._tradinginfotracker == null)
                    defaultinstance._tradinginfotracker = new TradingInfoTracker();
                return defaultinstance._tradinginfotracker;
            }
        }


        PositionWatcher _positionwatcher = null;

        /// <summary>
        /// 持仓监控器
        /// </summary>
        public static PositionWatcher PositionWatcher
        {
            get
            {
                if (defaultinstance._positionwatcher == null)
                    defaultinstance._positionwatcher = new PositionWatcher();
                return defaultinstance._positionwatcher;
            }
        }

        TLClientNet _tlclient = null;
        public static TLClientNet TLClient
        {
            get
            {
                return defaultinstance._tlclient;
            }
        }

        static string _address = string.Empty;
        static int _port = 0;
        public static void InitClient(string address, int port)
        {
            if (defaultinstance._tlclient == null ||( _address != address || _port != port))
            {
                _address = address;
                _port = port;
                TLClientNet tlclient = new TLClientNet(new string[] { address }, port);

                defaultinstance._tlclient = tlclient;
                //执行初始化 如果TradingInfo延迟调用会导致 ctrlPosition订阅成交在TradingInfo之前
                if (defaultinstance._tradinginfotracker == null)
                    defaultinstance._tradinginfotracker = new TradingInfoTracker();

                if (defaultinstance._positionwatcher == null)
                    defaultinstance._positionwatcher = new PositionWatcher();

            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset()
        {
            if (TLClient != null)
            {
                TLClient.Reset();
            }
            BasicInfoTracker.Reset();
            TradingInfoTracker.Reset();
            defaultinstance._isinited = false;
        }

    }
}
