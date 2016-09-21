using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Control
{
    public enum EnumViewType
    {
        /// <summary>
        /// 报价面板
        /// </summary>
        QuoteList=0,
        /// <summary>
        /// K线绘图面板
        /// </summary>
        KChart=1,

        /// <summary>
        /// 分笔明细
        /// </summary>
        TradeSplit=2,

        /// <summary>
        /// 分价明细
        /// </summary>
        PriceVol=3,

        /// <summary>
        /// 基础信息
        /// </summary>
        BasicInfo=4,
    }
}
