using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class SymbolHighLight
    {
        public SymbolHighLight(string title, MDSymbol symbol)
        {
            this.Title = title;
            this.Symbol = symbol;
        }
        public string Title { get; set; }

        public MDSymbol Symbol { get; set; }
    }

    /// <summary>
    /// 实时行情注册
    /// </summary>
    public class SymbolRegister
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 合约代码
        /// </summary>
        public string Symbol { get; set; }
    }

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
            //this.PreClose = 0;
            this.SecurityType = MDSecurityType.STK;
            this.Currency = MDCurrency.RMB;
            
            this.SortKey = string.Empty;
            this.Session = string.Empty;

            this.FinanceData = new FinanceData();
            this.TickSnapshot = new TDX();
            this.LastTickSnapshot = new TDX();

            this.LongPosition = new MDPosition();
            this.ShortPosition = new MDPosition();
            this.TimeZoneOffset = 0;
            this.OpenTime = null;
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
                _keyTitle = string.Format("{0} {1}", this.Symbol, this.Name);
            }
        }


        string _name = string.Empty;
        /// <summary>
        /// 合约名称
        /// </summary>
        public string Name 
        {
            get { return _name; }
            set
            {
                _name = value;
                _keyTitle = string.Format("{0} {1}", this.Symbol, this.Name);
            }
        }

        /// <summary>
        /// 交易小节
        /// 用于绘制分时图
        /// </summary>
        public string Session { get; set; }

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

        public string SecCode {get;set;}

        /// <summary>
        /// 排序键
        /// </summary>
        public string SortKey { get; set; }

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

        /// <summary>
        /// 上次行情快照
        /// </summary>
        public TDX LastTickSnapshot;


        /// <summary>
        /// 多头持仓
        /// </summary>
        public MDPosition LongPosition;

        /// <summary>
        /// 空头持仓
        /// </summary>
        public MDPosition ShortPosition;


        int _offset =0;
        /// <summary>
        /// 时区转换偏移秒数
        /// 用于将时间转换成本地时间
        /// </summary>
        public int TimeZoneOffset 
        { get {return _offset;}
            
            set{
                _offset = value;
                timeSpanOffset = TimeSpan.FromSeconds(_offset);
            } }

        TimeSpan timeSpanOffset;
        public TimeSpan TimeSpanOffset { get { return timeSpanOffset; } }

        /// <summary>
        /// 开盘时间
        /// </summary>
        public int? OpenTime { get; set; }

        /// <summary>
        /// 获得昨日收盘/结算价格
        /// </summary>
        /// <returns></returns>
        public double GetYdPrice()
        {
            switch (this.SecurityType)
            { 
                case MDSecurityType.STK:
                    return this.TickSnapshot.PreClose;
                case MDSecurityType.FUT:
                    return this.TickSnapshot.PreSettlement;
                default:
                    return this.TickSnapshot.PreClose;
            }
        }
        string _uniquekey = string.Empty;
        /// <summary>
        /// 通过交易所-合约 组成唯一Key
        /// </summary>
        public string UniqueKey { get { return _uniquekey; } }


        string _keyTitle = string.Empty;
        /// <summary>
        /// 搜索 显示标题
        /// </summary>
        public string KeyTitle { get { return _keyTitle; } }

    }
}
