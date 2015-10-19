using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {
        void CliOnMaxOrderVol(RspQryMaxOrderVolResponse response)
        {
            CoreService.EventOther.FireRspQryMaxOrderVolResponse(response);
        }
    }
}
