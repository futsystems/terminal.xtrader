using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    /// <summary>
    /// 持仓信息
    /// 用于绑定到对应合约
    /// 在KChart上绘制持仓线
    /// </summary>
    public class MDPosition
    {

        public MDPosition()
        {
            this.Size = 0;
            this.PositionCost = 0;
            this.UnRealizedPL = 0;
        }
        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 持仓均价
        /// </summary>
        public double PositionCost { get; set; }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        public double UnRealizedPL { get; set; }

        public void Reset()
        {
            this.Size = 0;
            this.PositionCost = 0;
            this.UnRealizedPL = 0;
        }
    }
}
