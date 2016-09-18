using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    /// <summary>
    /// 合约对象
    /// </summary>
    [Serializable]
    public class MDSymbol
    {

        public MDSymbol()
        {
            this.Symbol = string.Empty;
            this.Name = string.Empty;
            this.SizeRate = 1;
            this.Multiple = 1;
            this.Precision = 2;
            this.PreClose = 0;
            this.SecurityType = MDSecurityType.STK;
            this.Currency = MDCurrency.RMB;
            this.FinanceData = new FinanceData();
            this.TickSnapshot = new TDX();
        }

        string _symbol = string.Empty;
        /// <summary>
        /// 合约代码
        /// </summary>
        public string Symbol 
        {
            get { return _symbol; }
            set {
                _symbol = value;
                _uniquekey = string.Format("{0}-{1}", this.Exchange, this.Symbol);
            }
        }

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

        string _exch = string.Empty;
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange
        {
            get { return _exch; }
            set
            { 
                _exch = value;
                _uniquekey = string.Format("{0}-{1}", this.Exchange, this.Symbol);
            }
        }


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
        /// 财务数据
        /// </summary>
        public FinanceData FinanceData;

        /// <summary>
        /// 除权数据
        /// </summary>
        public PowerData PowerData;

        /// <summary>
        /// 行情快照
        /// </summary>
        public TDX TickSnapshot;


        public double PreClose { get; set; }

        string _uniquekey = string.Empty;
        /// <summary>
        /// 通过交易所-合约 组成唯一Key
        /// </summary>
        public string UniqueKey { get { return _uniquekey; } }

    }
}
