using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.DataCore
{
    public class DataCoreService
    {
        static DataCoreService defaultinstance = null;

        static DataCoreService()
        {
            defaultinstance = new DataCoreService();
        }

        private DataCoreService()
        { 
        
        }

        EventHub _eventHub;

        /// <summary>
        /// 数据事件集合
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


        EventManager _eventManager;

        /// <summary>
        /// 管理事件集合
        /// </summary>
        public static EventManager EventManager
        {
            get
            {
                if (defaultinstance._eventManager == null)
                    defaultinstance._eventManager = new EventManager();
                return defaultinstance._eventManager;
            }
        }


        EventContrib _eventContrib;
        /// <summary>
        /// 扩展事件
        /// </summary>
        public static EventContrib EventContrib
        {
            get
            {
                if (defaultinstance._eventContrib == null)
                    defaultinstance._eventContrib = new EventContrib();
                return defaultinstance._eventContrib;
            }
        }


        bool _isinited = false;
        public static bool Initialized
        {
            get
            {
                return defaultinstance._isinited;
            }
        }

        //public static event Action OnInitializedEvent;

        /// <summary>
        /// 初始化完毕
        /// </summary>
        internal static void Initialize()
        {
            defaultinstance._isinited = true;
            //if (OnInitializedEvent != null)
            //{
            //    OnInitializedEvent();
            //}
            EventHub.FireOnInitializedEvent();
        }

        DataClient _client = null;

        public static DataClient DataClient
        {
            get
            {
                return defaultinstance._client;
            }
        }

        public static void InitClient(string address, int port)
        {
            if (defaultinstance._client == null)
            {
                DataClient tlclient = new DataClient(address, port);

                defaultinstance._client = tlclient;
            }
        }

    }
}
