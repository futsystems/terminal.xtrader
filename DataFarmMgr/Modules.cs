using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.DataFarmManager
{
    public class Modules
    {
        public const string DATACORE = "DataCore";
    }

    public class Method_DataCore
    {
        /// <summary>
        /// 查询交易所交易日历列表
        /// </summary>
        public const string QRY_INFO_CALENDARLIST = "QryCalendarList";

        /// <summary>
        /// 更新交易时间段
        /// </summary>
        public const string UPDATE_INFO_MARKETTIME = "UpdateMarketTime";

        /// <summary>
        /// 更新交易所
        /// </summary>
        public const string UPDATE_INFO_EXCHANGE = "UpdateExchange";

        /// <summary>
        /// 更新品种
        /// </summary>
        public const string UPDATE_INFO_SECURITY = "UpdateSecurity";

        /// <summary>
        /// 更新合约
        /// </summary>
        public const string UPDATE_INFO_SYMBOL = "UpdateSymbol";

        /// <summary>
        /// 查询当前所连实时行情服务器
        /// </summary>
        public const string QRY_CURRENT_TICK_SERVER = "QryTickSrv";

        /// <summary>
        /// 切换实时行情服务器
        /// </summary>
        public const string SWITCH_TICK_SERVER = "SwitchTickSrv";

        /// <summary>
        /// 接受实时行情
        /// </summary>
        public const string REQ_ACCEPT_TICK = "AcceptFeedTick";

        /// <summary>
        /// 拒绝实时行情
        /// </summary>
        public const string REQ_REJECT_TICK = "RejectFeedTick";


        /// <summary>
        /// 显示查询日志
        /// </summary>
        public const string REQ_VERBOSE_ON = "EnableVerbose";

        /// <summary>
        /// 禁止查询日志
        /// </summary>
        public const string REQ_VERBOSE_OFF = "DisableVerbose";

    }
}
