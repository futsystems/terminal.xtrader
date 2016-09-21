using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.KryptonControl
{

    /// <summary>
    /// 根据不同的报价类型 设置需要显示的列与顺序
    /// </summary>
    public enum EnumQuoteListType
    {
        /// <summary>
        /// 显示所有合约
        /// </summary>
        ALL,
        /// <summary>
        /// 国内期货
        /// </summary>
        FUTURE_CN,
        /// <summary>
        /// 国内股票
        /// </summary>
        STOCK_CN,
    }
}
