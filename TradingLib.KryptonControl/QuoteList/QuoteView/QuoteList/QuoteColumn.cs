using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.KryptonControl
{
    /// <summary>
    /// 报价表列信息
    /// </summary>
    internal class QuoteColumn
    {
        /// <summary>
        /// 标体头
        /// </summary>
        public string Title { get; set;}

        /// <summary>
        /// 起点X坐标
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public int Width { get; set; }


        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }
    }
}
