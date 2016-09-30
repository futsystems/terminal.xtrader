using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.DataCore
{
    /// <summary>
    /// 管理事件
    /// </summary>
    public class EventManager
    {
        public event Action<RspMGRUpdateSymbolResponse> OnMGRUpdateSymbolResponse;

        internal void FireOnMGRUpdateSymbolResponse(RspMGRUpdateSymbolResponse response)
        {
            if (OnMGRUpdateSymbolResponse != null)
            {
                OnMGRUpdateSymbolResponse(response);
            }
        }
    }
}
