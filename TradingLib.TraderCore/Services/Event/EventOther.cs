using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.TraderCore
{
    public class EventOther
    {
        public event Action<AccountInfo> OnAccountInfoEvent;

        internal void FireAccountInfoEvent(AccountInfo info)
        {
            if (OnAccountInfoEvent != null)
                OnAccountInfoEvent(info);
        }

        public event Action<RspQryMaxOrderVolResponse> OnRspQryMaxOrderVolResponse;

        internal void FireRspQryMaxOrderVolResponse(RspQryMaxOrderVolResponse response)
        {
            if (OnRspQryMaxOrderVolResponse != null)
                OnRspQryMaxOrderVolResponse(response);
            
        }
    }
}
