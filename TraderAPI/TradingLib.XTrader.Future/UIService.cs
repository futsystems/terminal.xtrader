using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader
{
    public class UIService
    {
        static UIService defaultinstance = null;

        static UIService()
        {
            defaultinstance = new UIService();
        }

        EventUI _eventUI = null;
        /// <summary>
        /// 底层核心事件
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

        public static event Action<Symbol> OnReqRegisterSymbol = delegate { };
        /// <summary>
        /// 注册某个合约实时行情
        /// </summary>
        /// <param name="sym"></param>
        public static void ReqRegisterSymbol(Symbol sym)
        {
            OnReqRegisterSymbol(sym);
        }

        public static event Action<Symbol> OnReqUnRegisterSymbol = delegate { };
        /// <summary>
        /// 注销某个合约实时行情
        /// </summary>
        /// <param name="sym"></param>
        public static void ReqUnRegisterSymbol(Symbol sym)
        {
            OnReqUnRegisterSymbol(sym);
        }
    }
}
