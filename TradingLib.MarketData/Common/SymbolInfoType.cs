using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class SymbolInfoType
    {
        /// <summary>
        /// 合约信息类别
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        public SymbolInfoType(string symbol, string title, int start, int len,int typecode)
        {
            this.Symbol = symbol;
            this.Title = title;
            this.Start = start;
            this.Length = len;
            this.TypeCode = typecode;
        }
        /// <summary>
        /// 类别标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }


        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 信息类别编号
        /// 用于同类软件同步
        /// </summary>
        public int TypeCode { get; set; }
    }
}
