using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader
{
    public class EventUI
    {
        static Symbol _symbolSelected = null;
        /// <summary>
        /// 获得当前选中合约
        /// </summary>
        public Symbol SymbolSelected { get { return _symbolSelected; } }

        public event Action<Object, Symbol> OnSymbolSelectedEvent;
        /// <summary>
        /// 触发合约选择事件
        /// </summary>
        /// <param name="symbol"></param>
        public void FireSymbolSelectedEvent(Object sender, Symbol symbol)
        {
            if (symbol == null) return;
            if (_symbolSelected != null && symbol != _symbolSelected)
            {
                FireSymbolUnSelectedEvent(sender, _symbolSelected);
            }

            _symbolSelected = symbol;
            if (OnSymbolSelectedEvent != null)
                OnSymbolSelectedEvent(sender, symbol);
        }

        public event Action<object, Symbol> OnSymbolUnSelectedEvent;
        /// <summary>
        /// 触发合约取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="symbol"></param>
        private void FireSymbolUnSelectedEvent(Object sender, Symbol symbol)
        {
            if (OnSymbolUnSelectedEvent != null)
                OnSymbolUnSelectedEvent(sender, symbol);
        }


        public event Action<object, Position> OnPositionSelectedEvent = delegate { };

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
