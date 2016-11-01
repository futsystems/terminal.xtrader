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
        static Symbol _symbolSelected = null;
        public event Action<Object,Symbol> OnSymbolSelectedEvent;
        /// <summary>
        /// 触发合约选择事件
        /// </summary>
        /// <param name="symbol"></param>
        public void FireSymbolSelectedEvent(Object sender,Symbol symbol)
        {
            if (_symbolSelected != null && symbol != null)
            {
                FireSymbolUnSelectedEvent(sender, _symbolSelected);
                _symbolSelected = symbol;
            }
            
            if (OnSymbolSelectedEvent != null)
                OnSymbolSelectedEvent(sender,symbol);
        }

        public event Action<object, Symbol> OnSymbolUnSelectedEvent;
        /// <summary>
        /// 触发合约取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="symbol"></param>
        public void FireSymbolUnSelectedEvent(Object sender, Symbol symbol)
        {
            if (OnSymbolUnSelectedEvent != null)
                OnSymbolUnSelectedEvent(sender, symbol);
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


        public event Action<object,Position> OnPositionSelectedEvent = delegate { };

        /// <summary>
        /// 触发持仓选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="position"></param>
        public void FirePositionSelectedEvent(object sender, Position position)
        {
            OnPositionSelectedEvent(sender, position);
        }
    }
}
