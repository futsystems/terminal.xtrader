using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;
using TradingLib.MarketData;


namespace TradingLib.DataCore
{
    /// <summary>
    /// 维护基础数据
    /// </summary>
    public partial class MDClient
    {
        Dictionary<string, MDSymbol> mdSymbolMap = new Dictionary<string, MDSymbol>();
        /// <summary>
        /// 市场时间段map
        /// </summary>
        Dictionary<int, MarketTime> markettimemap = new Dictionary<int, MarketTime>();

        /// <summary>
        /// 交易所map
        /// </summary>
        Dictionary<int, Exchange> exchangemap = new Dictionary<int, Exchange>();

        /// <summary>
        /// 品种map
        /// </summary>
        Dictionary<int, SecurityFamilyImpl> securitymap = new Dictionary<int, SecurityFamilyImpl>();

        /// <summary>
        /// 品种名称map
        /// </summary>
        Dictionary<string, SecurityFamilyImpl> securitynamemap = new Dictionary<string, SecurityFamilyImpl>();

        /// <summary>
        /// 合约map
        /// </summary>
        Dictionary<int, SymbolImpl> symbolmap = new Dictionary<int, SymbolImpl>();

        /// <summary>
        /// 合约名称map
        /// </summary>
        Dictionary<string, SymbolImpl> symbolnamemap = new Dictionary<string, SymbolImpl>();


        void OnXQryMarketTimeResponse(RspXQryMarketTimeResponse response)
        {
            if (response.MarketTime!= null)
            {
                MarketTime target = null;
                if (markettimemap.TryGetValue(response.MarketTime.ID, out target))
                {
                    //更新
                    target.Name = response.MarketTime.Name;
                    target.Description = response.MarketTime.Description;
                    target.CloseTime = response.MarketTime.CloseTime;
                    target.DeserializeTradingRange(response.MarketTime.SerializeTradingRange());//将时间小节解析到对象中
                }
                else
                {
                    markettimemap.Add(response.MarketTime.ID, response.MarketTime);
                }
            }
            if (response.IsLast)
            {
                logger.Info("交易时间查询完毕,查询交易所信息");
                QryExchange();
            }
        }

        #region 交易数数据处理
        void OnXQryExchangeResponse(RspXQryExchangeResponse response)
        {
            if (response.Exchange != null)
            {
                GotExchange(response.Exchange);
            }
            if (response.IsLast)
            {
                logger.Info("交易所查询完毕,查询品种信息");
                QrySecurity();
            }
        }

        void OnMGRUpdateExchange(RspMGRUpdateExchangeResponse response)
        {
            if (response.Exchange != null)
            {
                GotExchange(response.Exchange);
            }
        }

        void GotExchange(Exchange exchange)
        {
            Exchange target = null;
            if (exchangemap.TryGetValue(exchange.ID, out target))
            {
                //更新
                target.Name = exchange.Name;
                target.EXCode = exchange.EXCode;
                target.Country = exchange.Country;
                target.Title = exchange.Title;
                target.Calendar = exchange.Calendar;
                target.CloseTime = exchange.CloseTime;
            }
            else
            {

                exchangemap.Add(exchange.ID, exchange);
            }
        }
        #endregion


        #region 品种数据处理
        void OnXQrySecurityResponse(RspXQrySecurityResponse response)
        {
            if (response.SecurityFaimly != null)
            {
                GotSecurity(response.SecurityFaimly);
            }

            if (response.IsLast)
            {
                logger.Info("品种查询完毕,查询合约信息");
                QrySymbol();
            }
        }

        void OnMGRUpdateSecurity(RspMGRUpdateSecurityResponse response)
        {
            GotSecurity(response.SecurityFaimly);
        }
        void GotSecurity(SecurityFamilyImpl sec)
        {
            SecurityFamilyImpl target = null;
            if (securitymap.TryGetValue(sec.ID, out target))
            {
                //更新
                target.Code = sec.Code;
                target.Name = sec.Name;
                target.Currency = sec.Currency;
                target.Type = sec.Type;

                target.exchange_fk = sec.exchange_fk;
                //target.Exchange = this.GetExchange(target.exchange_fk);

                target.mkttime_fk = sec.mkttime_fk;
                //target.MarketTime = this.GetMarketTime(target.mkttime_fk);

                target.underlaying_fk = sec.underlaying_fk;
                //target.UnderLaying = this.GetSecurity(target.underlaying_fk);

                target.Multiple = sec.Multiple;
                target.PriceTick = sec.PriceTick;
                target.EntryCommission = sec.EntryCommission;
                target.ExitCommission = sec.ExitCommission;
                target.Margin = sec.Margin;
                target.ExtraMargin = sec.ExtraMargin;
                target.MaintanceMargin = sec.MaintanceMargin;
                target.Tradeable = sec.Tradeable;
            }
            else
            {
                target = sec;
                securitymap.Add(target.ID, target);
                securitynamemap.Add(target.Code, target);
            }
            //如果已经初始化 则执行对象关系绑定
            if (_inited)
            {
                target.Exchange = this.GetExchange(target.exchange_fk);
                target.MarketTime = this.GetMarketTime(target.mkttime_fk);
                target.UnderLaying = this.GetSecurity(target.underlaying_fk);
            }
        }
        #endregion


        #region 合约数据处理
        void OnXQrySymbolResponse(RspXQrySymbolResponse response)
        {
            if (response.Symbol != null)
            {
                GotSymbol(response.Symbol);
            }
            if (response.IsLast)
            {
                if (!_inited)
                {
                    logger.Info("合约查询完毕,查询隔夜持仓");
                    BindData();

                    _inited = true;
                    if (OnInitializedEvent != null)
                    {
                        OnInitializedEvent();
                    }
                }
            }

        }

        void OnMGRUpdateSymbol(RspMGRUpdateSymbolResponse response)
        {
            GotSymbol(response.Symbol);

        }

        void GotSymbol(SymbolImpl symbol)
        {
            if (symbol != null)
            {
                SymbolImpl target = null;
                if (symbolmap.TryGetValue(symbol.ID, out target))
                {
                    //更新
                    target.Symbol = symbol.Symbol;
                    target.EntryCommission = symbol._entrycommission;
                    target.ExitCommission = symbol._exitcommission;
                    target.Margin = symbol._margin;
                    target.ExtraMargin = symbol._extramargin;
                    target.MaintanceMargin = symbol._maintancemargin;
                    target.Strike = symbol.Strike;
                    target.OptionSide = symbol.OptionSide;
                    target.ExpireDate = symbol.ExpireDate;

                    target.security_fk = symbol.security_fk;
                    target.SecurityFamily = this.GetSecurity(target.security_fk);

                    target.underlaying_fk = symbol.underlaying_fk;
                    target.ULSymbol = this.GetSymbol(target.underlaying_fk);

                    target.underlayingsymbol_fk = symbol.underlayingsymbol_fk;
                    target.UnderlayingSymbol = this.GetSymbol(target.underlayingsymbol_fk);
                    target.Tradeable = symbol.Tradeable;
                }
                else //添加
                {
                    symbolmap.Add(symbol.ID, symbol);
                    symbol.SecurityFamily = this.GetSecurity(symbol.security_fk);
                    symbol.ULSymbol = this.GetSymbol(symbol.underlaying_fk);
                    symbol.UnderlayingSymbol = this.GetSymbol(symbol.underlayingsymbol_fk);

                    if (_inited)
                    {
                        symbolnamemap[symbol.UniqueKey] = symbol;
                    }

                }
            }
        }

        #endregion 

        /// <summary>
        /// 绑定对象数据
        /// </summary>
        void BindData()
        {
            foreach (SecurityFamilyImpl target in securitymap.Values)
            {
                target.Exchange = this.GetExchange(target.exchange_fk);
                target.MarketTime = this.GetMarketTime(target.mkttime_fk);
                target.UnderLaying = this.GetSecurity(target.underlaying_fk);
            }

            foreach (SymbolImpl target in symbolmap.Values)
            {
                target.SecurityFamily = this.GetSecurity(target.security_fk);
                target.ULSymbol = this.GetSymbol(target.underlaying_fk);
                target.UnderlayingSymbol = this.GetSymbol(target.underlayingsymbol_fk);

                symbolnamemap[target.UniqueKey] = target;
            }

            foreach (var target in symbolmap.Values)
            {
                MDSymbol symbol = new MDSymbol();
                symbol.Symbol = target.Symbol;
                symbol.SecCode = target.SecurityFamily.Code;
                symbol.Name = target.GetName();
                symbol.Currency = MDCurrency.RMB;
                symbol.Exchange = target.Exchange;
                symbol.Multiple = target.Multiple;
                symbol.SecurityType = MDSecurityType.FUT;
                symbol.SizeRate = 1;
                symbol.NCode = 0;
                symbol.SortKey = target.Month;
                mdSymbolMap.Add(symbol.UniqueKey, symbol);
                
            }
            logger.Info("MarketTime     Num:" + markettimemap.Count.ToString());
            logger.Info("Exchange       Num:" + exchangemap.Count.ToString());
            logger.Info("Security       Num:" + securitymap.Count.ToString());
            logger.Info("Symbol         Num:" + symbolmap.Count.ToString());
        }



        #region 外部访问接口
        /// <summary>
        /// 市场时间段
        /// </summary>
        public IEnumerable<IMarketTime> MarketTimes
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


        public IEnumerable<MDSymbol> MDSymbols
        {
            get
            {
                return mdSymbolMap.Values;
            }
        }


        public MDSymbol GetMDSymbol(string key)
        {
            MDSymbol target = null;
            if (mdSymbolMap.TryGetValue(key, out target))
            {
                return target;
            }
            return null;
        }


        private MarketTime GetMarketTime(int id)
        {
            MarketTime mt = null;
            if (markettimemap.TryGetValue(id, out mt))
            {
                return mt;
            }
            return null;
        }

        private Exchange GetExchange(int id)
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

        public SecurityFamilyImpl GetSecurity(string code)
        {
            SecurityFamilyImpl sec = null;
            if (securitynamemap.TryGetValue(code, out sec))
            {
                return sec;
            }
            return null;
        }

        private SymbolImpl GetSymbol(int id)
        {
            SymbolImpl sym = null;
            if (symbolmap.TryGetValue(id, out sym))
            {
                return sym;
            }
            return null;
        }



        public SymbolImpl GetSymbol(string exchange,string symbol)
        {
            string key = string.Format("{0}-{1}", exchange, symbol);
            SymbolImpl sym = null;
            if (symbolnamemap.TryGetValue(key, out sym))
            {
                return sym;
            }
            return null;
        }




        #endregion



    }
}
