using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        string _account = string.Empty;
        /// <summary>
        /// 交易账户
        /// </summary>
        public static string Account
        {
            get
            {
                return defaultinstance._account;
            }
            set
            {
                defaultinstance._account = value;
            }
        }


        EventUI _eventUI = null;
        /// <summary>
        /// 界面操作事件中继
        /// 通过中继触发相关事件，这样可以接触组件之间的耦合与复杂的事件绑定
        /// </summary>
        public static EventUI EventUI
        {
            get
            {
                if (defaultinstance._eventUI == null)
                    defaultinstance._eventUI = new EventUI();
                return defaultinstance._eventUI;
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


        TLClientNet _tlclient = null;
        public static TLClientNet TLClient
        {
            get
            {
                return defaultinstance._tlclient;
            }
        }

        public static void InitClient(string address, int port)
        {
            if (defaultinstance._tlclient == null)
            {
                TLClientNet tlclient = new TLClientNet(new string[] { address }, port);

                defaultinstance._tlclient = tlclient;
                
            }
        }

    }
}
