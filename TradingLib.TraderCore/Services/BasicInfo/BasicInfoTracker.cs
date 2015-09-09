using System;
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

        void Status(string msg)
        {
            CoreService.EventCore.FireInitializeStatusEvent(msg);
            logger.Info(msg);
        }


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
        /// 合约map
        /// </summary>
        Dictionary<int, SymbolImpl> symbolmap = new Dictionary<int, SymbolImpl>();

        /// <summary>
        /// 合约名称map
        /// </summary>
        Dictionary<string, SymbolImpl> symbolnamemap = new Dictionary<string, SymbolImpl>();

    }
}
