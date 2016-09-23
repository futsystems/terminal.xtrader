using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStock
{
    public enum DetailBoardTabType
    {
        /// <summary>
        /// 分笔成交
        /// </summary>
        TradeDetails,
        /// <summary>
        /// 价格分布
        /// </summary>
        PriceDistribution,

        /// <summary>
        /// 未知面板
        /// </summary>
        Unknown,
    }
}
