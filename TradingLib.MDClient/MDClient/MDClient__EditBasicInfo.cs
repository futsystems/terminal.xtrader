using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.MDClient
{

    public partial class MDClient
    {
        public int ReqUpdateSymbol(SymbolImpl sym)
        {
            logger.Info("请求更新合约");
            MGRUpdateSymbolRequest request = RequestTemplate<MGRUpdateSymbolRequest>.CliSendRequest(++requestid);
            request.Symbol = sym;

            SendPacket(request);
            return requestid;
        }
    }
}
