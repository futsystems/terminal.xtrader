using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    /// <summary>
    /// 合约对象
    /// </summary>
    public class MDSymbol
    {

        public MDSymbol()
        {
            this.Symbol = string.Empty;
            this.Name = string.Empty;
            this.SizeRate = 1;
            this.Multiple = 1;
            this.Precision = 2;

            this.SecurityType = MDSecurityType.STK;
            this.Currency = MDCurrency.RMB;
        }

        /// <summary>
        /// 合约代码
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 合约名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 品种
        /// </summary>
        public MDSecurityType SecurityType { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        public MDCurrency Currency { get; set; }

        /// <summary>
        /// 数量乘数 股票1手 代表100股
        /// </summary>
        public int SizeRate { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 乘数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }


        /// <summary>
        /// 板块类别
        /// </summary>
        public string BlockType { get; set; }


        /// <summary>
        /// 中文Name gb2312中的汉字编码
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        /// 数字code
        /// </summary>
        public long NCode { get; set; }

        /// <summary>
        /// 通过交易所-合约 组成唯一Key
        /// </summary>
        public string UniqueKey { get { return string.Concat(new { this.Exchange, this.Symbol }); } }

    }
}
