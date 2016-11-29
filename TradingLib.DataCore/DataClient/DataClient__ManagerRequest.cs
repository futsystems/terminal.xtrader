using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.DataCore
{

    public partial class DataClient
    {
        /// <summary>
        /// 更新合约数据
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public int ReqUpdateSymbol(SymbolImpl sym)
        {
            logger.Info("请求更新合约");
            MGRUpdateSymbolRequest request = RequestTemplate<MGRUpdateSymbolRequest>.CliSendRequest(++requestid);
            request.Symbol = sym;

            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 更新品种数据
        /// </summary>
        /// <param name="sec"></param>
        public int ReqUpdateSecurity(SecurityFamilyImpl sec)
        {
            logger.Info("请求更新品种信息");
            MGRUpdateSecurityRequest request = RequestTemplate<MGRUpdateSecurityRequest>.CliSendRequest(++requestid);
            request.SecurityFaimly = sec;
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 更新交易所
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public int ReqUpdateExchange(Exchange ex)
        {
            logger.Info("请求更新交易所");
            MGRUpdateExchangeRequest request = RequestTemplate<MGRUpdateExchangeRequest>.CliSendRequest(++requestid);
            request.Exchange = ex;

            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 更新交易小节
        /// </summary>
        /// <param name="mt"></param>
        public int ReqUpdateMarketTime(MarketTime mt)
        {
            logger.Info("请求更新交易时间段");
            MGRUpdateMarketTimeRequest request = RequestTemplate<MGRUpdateMarketTimeRequest>.CliSendRequest(++requestid);
            request.MarketTime = mt;

            SendPacket(request);
            return requestid;
        }

        

    }
}
