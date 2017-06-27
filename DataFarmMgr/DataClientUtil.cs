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
        public static int ReqQryCalendarList(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.QRY_INFO_CALENDARLIST,"");
        }
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

        /// <summary>
        /// 查询当前所连实时行情服务器
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqQryCurrentTickSrv(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.QRY_CURRENT_TICK_SERVER,string.Empty);
        }

        /// <summary>
        /// 切换实时行情服务器
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqSwitchTickServer(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.SWITCH_TICK_SERVER, string.Empty);
        }

        /// <summary>
        /// 接受实时行情
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqAcceptTick(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.REQ_ACCEPT_TICK, string.Empty);
        }

        /// <summary>
        /// 拒绝实时行情
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqRejectTick(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.REQ_REJECT_TICK, string.Empty);
        }

        /// <summary>
        /// 显示Verbose日志
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqEnableVerbose(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.REQ_VERBOSE_ON, string.Empty);
        }

        /// <summary>
        /// 禁止Verbose日志
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int ReqDisableVerbose(this DataClient client)
        {
            return client.ReqContribRequest(Modules.DATACORE, Method_DataCore.REQ_VERBOSE_OFF, string.Empty);
        }
    }
}
