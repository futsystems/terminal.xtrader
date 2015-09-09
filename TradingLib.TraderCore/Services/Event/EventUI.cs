using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    public class EventUI
    {
        public event Action<Object,Symbol> OnSymbolSelectedEvent;

        /// <summary>
        /// 触发合约选择事件
        /// </summary>
        /// <param name="symbol"></param>
        public void FireSymbolselectedEvent(Object sender,Symbol symbol)
        {
            if (OnSymbolSelectedEvent != null)
                OnSymbolSelectedEvent(sender,symbol);
        }
    }
}
