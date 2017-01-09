﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.TraderCore
{
    public partial class BasicInfoTracker
    {
        ILog logger = LogManager.GetLogger("BasicInfoTracker");

        bool _inited = false;

        void Status(string msg)
        {
            CoreService.EventCore.FireInitializeStatusEvent(msg);
            logger.Info(msg);
        }


        public void Reset()
        {
            _inited = false;

            markettimemap.Clear();
            exchangemap.Clear();
            securitymap.Clear();
            symbolmap.Clear();
            symbolkeyemap.Clear();
        }
        /// <summary>
        /// 市场时间段map
        /// </summary>
        Dictionary<int, MarketTimeImpl> markettimemap = new Dictionary<int, MarketTimeImpl>();

        /// <summary>
        /// 交易所map
        /// </summary>
        Dictionary<int, ExchangeImpl> exchangemap = new Dictionary<int, ExchangeImpl>();

        /// <summary>
        /// 品种map
        /// </summary>
        Dictionary<int, SecurityFamilyImpl> securitymap = new Dictionary<int, SecurityFamilyImpl>();

        /// <summary>
        /// 合约map
        /// </summary>
        Dictionary<int, SymbolImpl> symbolmap = new Dictionary<int, SymbolImpl>();

        /// <summary>
        /// 合约名称map
        /// </summary>
        Dictionary<string, SymbolImpl> symbolkeyemap = new Dictionary<string, SymbolImpl>();


        /// <summary>
        /// 汇率数据
        /// </summary>
        Dictionary<CurrencyType, ExchangeRate> exchangRateCurrencyMap = new Dictionary<CurrencyType, ExchangeRate>();
        Dictionary<int, ExchangeRate> exchangeRateMap = new Dictionary<int, ExchangeRate>();
    }
}
