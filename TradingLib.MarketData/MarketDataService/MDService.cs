using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using TradingLib.MarketData;

namespace TradingLib.MarketData
{

    /// <summary>
    /// 行情核心服务
    /// 底层通过插件或配置来调用不同的行情源API实现 可以实现不同的数据源处理
    /// 行情源API接口 引用TradingLib.MarketData将不同的行情源转换成内部使用的MarketData格式，同时将事件抽象后通过EventHub对外发送
    /// </summary>
    public class MDService
    {
        static MDService defaultInstance = null;

        static MDService()
        {
            defaultInstance = new MDService();
        }

        bool _isinited = false;
        public static bool Initialized
        {
            get
            {
                return defaultInstance._isinited;
            }
        }

        /// <summary>
        /// 初始化完毕
        /// </summary>
        internal static void Initialize()
        {
            defaultInstance._isinited = true;
            MDService.EventHub.FireInitializedEvent();
        }



        EventHub _eventHub = null;
        /// <summary>
        /// 底层核心事件
        /// </summary>
        public static EventHub EventHub
        {
            get
            {
                if (defaultInstance._eventHub == null)
                    defaultInstance._eventHub = new EventHub();
                return defaultInstance._eventHub;
            }
        }

        IMarketDataAPI _dataAPI = null;
        public static IMarketDataAPI DataAPI
        {
            get
            {
                return defaultInstance._dataAPI;
            }
        }

        /// <summary>
        /// 初始化行情API
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public static void InitDataAPI(IMarketDataAPI api)
        {
            if (defaultInstance._dataAPI == null)
            {
                defaultInstance._dataAPI = api;
            }
        }

        
    }
}
