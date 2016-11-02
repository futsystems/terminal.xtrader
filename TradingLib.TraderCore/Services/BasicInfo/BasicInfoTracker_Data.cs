using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.TraderCore
{
    public partial class BasicInfoTracker
    {

        /// <summary>
        /// 市场时间段
        /// </summary>
        public IEnumerable<MarketTime> MarketTimes
        {
            get
            {
                return markettimemap.Values;
            }
        }

        /// <summary>
        /// 交易所
        /// </summary>
        public IEnumerable<Exchange> Exchanges
        {
            get
            {
                return exchangemap.Values;
            }
        }

        /// <summary>
        /// 品种
        /// </summary>
        public IEnumerable<SecurityFamilyImpl> Securities
        {
            get
            {
                return securitymap.Values;
            }

        }

        /// <summary>
        /// 合约
        /// </summary>
        public IEnumerable<SymbolImpl> Symbols
        {
            get
            {
                return symbolmap.Values.ToArray();
            }
        }






        public MarketTime GetMarketTime(int id)
        {
            MarketTime mt = null;
            if (markettimemap.TryGetValue(id, out mt))
            {
                return mt;
            }
            return null;
        }

        public Exchange GetExchange(int id)
        {
            Exchange ex = null;
            if (exchangemap.TryGetValue(id, out ex))
            {
                return ex;
            }
            return null;
        }
        public SecurityFamilyImpl GetSecurity(int id)
        {
            SecurityFamilyImpl sec = null;
            if (securitymap.TryGetValue(id, out sec))
            {
                return sec;
            }
            return null;
        }

        public SymbolImpl GetSymbol(int id)
        {
            SymbolImpl sym = null;
            if (symbolmap.TryGetValue(id, out sym))
            {
                return sym;
            }
            return null;
        }

        public SecurityFamilyImpl GetSecurity(string code)
        {
            foreach (SecurityFamilyImpl sec in securitymap.Values)
            {
                if (sec.Code.Equals(code))
                    return sec;
            }
            return null;
        }

        public SymbolImpl GetSymbol(string exchange,string symbol)
        {
            string key = string.Format("{0}-{1}", exchange, symbol);
            SymbolImpl sym = null;
            if (symbolkeyemap.TryGetValue(key, out sym))
            {
                return sym;
            }
            return null;
        }


        /// <summary>
        /// 返回所有汇率数据
        /// </summary>
        IEnumerable<ExchangeRate> ExchangeRates { get { return this.exchangRateCurrencyMap.Values; } }

        public ExchangeRate GetExchangeRate(int id)
        {
            ExchangeRate target = null;
            if (this.exchangeRateMap.TryGetValue(id, out target))
            {
                return target;
            }
            return target;
        }


        public ExchangeRate GetExchangeRate(CurrencyType currency)
        {
            ExchangeRate target = null;
            if (this.exchangRateCurrencyMap.TryGetValue(currency, out target))
            {
                return target;
            }
            return null;
        }



    }
}
