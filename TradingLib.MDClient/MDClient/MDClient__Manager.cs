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
        public void ReqUpdateSecurity(SecurityFamilyImpl sec)
        {
            logger.Info("请求更新品种信息");
            MGRUpdateSecurityRequest request = RequestTemplate<MGRUpdateSecurityRequest>.CliSendRequest(++requestid);
            request.SecurityFaimly = sec;
            SendPacket(request);
        }
    }
}
