using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {
        void CliOnXMarketTime(RspXQryMarketTimeResponse response)
        {
            logger.Debug("got markettime response:" + response.ToString());
            CoreService.BasicInfoTracker.GotMarketTime(response.MarketTime, response.IsLast);
        }

        void CliOnXExchange(RspXQryExchangeResponse response)
        {
            logger.Debug("got exchange response:" + response.ToString());
            CoreService.BasicInfoTracker.GotExchange(response.Exchange, response.IsLast);
        }

        void CliOnXSecurity(RspXQrySecurityResponse response)
        {
            logger.Debug("got security response:" + response.ToString());
            CoreService.BasicInfoTracker.GotSecurity(response.SecurityFaimly, response.IsLast);
        }

        void CliOnXSymbol(RspXQrySymbolResponse response)
        {
            logger.Debug("got symbol response:" + response.ToString());
            CoreService.BasicInfoTracker.GotSymbol(response.Symbol, response.IsLast);

            //触发查询回调
            Symbol target = null;
            if (response.Symbol != null)
            {
                target = CoreService.BasicInfoTracker.GetSymbol(response.Symbol.Symbol);
            }
            CoreService.EventQry.FireRspXQrySymbolResponse(target, response.RspInfo, response.RequestID, response.IsLast);

        }
    }
}
