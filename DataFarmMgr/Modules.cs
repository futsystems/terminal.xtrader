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
        /// 更新交易时间段
        /// </summary>
        public const string UPDATE_INFO_MARKETTIME = "";

        /// <summary>
        /// 更新交易所
        /// </summary>
        public const string UPDATE_INFO_EXCHANGE = "";

        /// <summary>
        /// 更新品种
        /// </summary>
        public const string UPDATE_INFO_SECURITY = "";

        /// <summary>
        /// 更新合约
        /// </summary>
        public const string UPDATE_INFO_SYMBOL = "";
    }
}
