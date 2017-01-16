using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public static class DataClientUtil
    {
        /// <summary>
        /// 更新合约数据
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static int ReqUpdateSymbol(this DataClient client,SymbolImpl sym)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.UPDATE_INFO_SYMBOL, SymbolImpl.Serialize(sym), true);
        }

        /// <summary>
        /// 更新品种数据
        /// </summary>
        /// <param name="sec"></param>
        public static int ReqUpdateSecurity(this DataClient client, SecurityFamilyImpl sec)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.UPDATE_INFO_SECURITY, SecurityFamilyImpl.Serialize(sec), true);
        }

        /// <summary>
        /// 更新交易所
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static int ReqUpdateExchange(this DataClient client, ExchangeImpl ex)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.UPDATE_INFO_EXCHANGE, ExchangeImpl.Serialize(ex), true);
        }

        /// <summary>
        /// 更新交易小节
        /// </summary>
        /// <param name="mt"></param>
        public static int ReqUpdateMarketTime(this DataClient client, MarketTimeImpl mt)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.UPDATE_INFO_MARKETTIME, MarketTimeImpl.Serialize(mt), true);
        }

    }
}
