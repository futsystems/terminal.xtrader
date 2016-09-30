using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.DataCore
{
    public class MDService
    {
        static MDService defaultinstance = null;

        static MDService()
        {
            defaultinstance = new MDService();
        }

        private MDService()
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
    }
}
