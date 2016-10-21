using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Control
{

    /// <summary>
    /// 根据不同的报价类型 设置需要显示的列与顺序
    /// </summary>
    public enum EnumQuoteListType
    {
        /// <summary>
        /// 显示所有合约
        /// </summary>
        ALL=1,
        /// <summary>
        /// 国外期货
        /// 这里需要针对不同版面执行不同的算法 有些取昨日收盘价 有些取结算价
        /// </summary>
        FUTURE_IQFeed=2,
        /// <summary>
        /// 国内期货
        /// </summary>
        FUTURE_CN=3,
        /// <summary>
        /// 国内股票
        /// </summary>
        STOCK_CN=4,
    }
}
