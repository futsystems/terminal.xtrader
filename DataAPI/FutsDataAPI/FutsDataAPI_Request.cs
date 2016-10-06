using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;

namespace DataAPI.Futs
{
    public partial class FutsDataAPI : IMarketDataAPI
    {
        #region 查询操作


        public event Action<List<MDSymbol>, TradingLib.MarketData.RspInfo, int, int> OnRspQryTickSnapshot;
        /// <summary>
        /// 查询行情快照
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public int QryTickSnapshot(MDSymbol[] symbols)
        {
            return 0;
        }


        public event Action<Dictionary<string, double[]>, TradingLib.MarketData.RspInfo, int, int> OnRspQryHistMinuteData;
        /// <summary>
        /// 查询分时数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QryMinuteDate(string exchange, string symbol, int date)
        {
            return 0;
        }


        /// <summary>
        /// 返回当日分时数据
        /// </summary>
        public event Action<Dictionary<string, double[]>, TradingLib.MarketData.RspInfo, int, int> OnRspQryMinuteData;


        /// <summary>
        /// Bar数据回报
        /// </summary>
        public event Action<Dictionary<string, double[]>, TradingLib.MarketData.RspInfo, int, int> OnRspQrySecurityBar;

        /// <summary>
        /// 查询Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="freqStr"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int QrySeurityBars(string exchange, string symbol, string freqStr, int start, int count)
        {
            return 0;
        }




        public event Action<List<PriceVolPair>, TradingLib.MarketData.RspInfo, int, int> OnRspQryPriceVolPair;
        /// <summary>
        /// 查询价量信息
        /// 在什么价位 成交多少数量
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QryPriceVol(string exchange, string symbol)
        {
            return 0;
        }


        /// <summary>
        /// 分笔数据回报
        /// </summary>
        public event Action<List<TradeSplit>, TradingLib.MarketData.RspInfo, int, int> OnRspQryTradeSplit;
        /// <summary>
        /// 查询分笔数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="start"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public int QryTradeSplitData(string exchange, string symbol, int start, int Count)
        {
            return 0;
        }



        /// <summary>
        /// 查询合约信息类别回报
        /// </summary>
        public event Action<List<SymbolInfoType>, TradingLib.MarketData.RspInfo, int, int> OnRspQrySymbolInfoType;
        /// <summary>
        /// 查询合约信息类别
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QrySymbolInfoType(string exchange, string symbol)
        {
            return 0;
        }


        /// <summary>
        /// 查询合约信息回报
        /// </summary>
        public event Action<string, TradingLib.MarketData.RspInfo, int, int> OnRspQrySymbolInfo;
        /// <summary>
        /// 查询合约信息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int QrySymbolInfo(string exchange, string symbol, SymbolInfoType type)
        {
            return 0;
        }
        #endregion
    }
}
