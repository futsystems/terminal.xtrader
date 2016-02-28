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



        /// <summary>
        /// 帐户交易信息刷新事件
        /// 用于重新查询日内交易数据生成本地数据
        /// </summary>
        public event Action OnRefreshEvent;
        internal void FireRefreshEvent()
        {
            if (OnRefreshEvent != null)
            {
                OnRefreshEvent();
            }
        }
    }
}
