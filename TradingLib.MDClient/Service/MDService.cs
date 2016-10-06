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

        EventData _eventdata;

        /// <summary>
        /// 数据事件集合
        /// </summary>
        public static EventData EventData
        {
            get
            {
                if (defaultinstance._eventdata == null)
                    defaultinstance._eventdata = new EventData();
                return defaultinstance._eventdata;
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


        MDClient _client = null;

        public static MDClient MDClient
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
                MDClient tlclient = new MDClient(address, port, port);

                defaultinstance._client = tlclient;
            }
        }

    }
}
