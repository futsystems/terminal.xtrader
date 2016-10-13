using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;
using TradingLib.MarketData;

namespace DataAPI.Futs
{
    public partial class FutsDataAPI : IMarketDataAPI
    {

        #region 实时行情数据
        public event Action<List<MDSymbol>, TradingLib.MarketData.RspInfo, int, int> OnRspQryTickSnapshot;
        /// <summary>
        /// 查询行情快照
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public int QryTickSnapshot(MDSymbol[] symbols)
        {
            logger.Info("QryTickSnapshot not supported");   
            return 0;
        }



        public event Action<MDSymbol> OnRtnTick;
        /// <summary>
        /// 注册合约行情
        /// </summary>
        /// <param name="symbols"></param>
        public void RegisterSymbol(MDSymbol[] symbols)
        {
            if (symbols == null || symbols.Length == 0) return;
            foreach (var g in symbols.GroupBy(sym => sym.Exchange))
            {
                DataCoreService.DataClient.RegisterSymbol(g.Key, g.Select(sym => sym.Symbol).ToArray());
            }
        }

        /// <summary>
        /// 注销合约行情
        /// </summary>
        /// <param name="symbols"></param>
        public void UnregisterSymbol(MDSymbol[] symbols)
        {
            if (symbols == null || symbols.Length == 0) return;
            foreach (var g in symbols.GroupBy(sym => sym.Exchange))
            {
                DataCoreService.DataClient.UnRegisterSymbol(g.Key, g.Select(sym => sym.Symbol).ToArray());
            }
        }

        #endregion



        #region 增加分时数据查询接口
        /// <summary>
        /// 返回当日分时数据
        /// </summary>
        public event Action<Dictionary<string, double[]>, TradingLib.MarketData.RspInfo, int, int> OnRspQryMinuteData;

        public event Action<Dictionary<string, double[]>, TradingLib.MarketData.RspInfo, int, int> OnRspQryHistMinuteData;
        /// <summary>
        /// 查询分时数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QryMinuteDate(string exchange, string symbol, int date)
        {
            return DataCoreService.DataClient.QryMinuteData(exchange, symbol, 20161013);
        }

        Dictionary<int, Dictionary<string, List<double>>> minuteDataResponseMap = new Dictionary<int, Dictionary<string, List<double>>>();
        void EventHub_OnRspMinuteDataEvent(RspXQryMinuteDataResponse obj)
        {
            Dictionary<string, List<double>> target = null;
            if (!minuteDataResponseMap.TryGetValue(obj.RequestID, out target))
            {
                target = new Dictionary<string, List<double>>();
                minuteDataResponseMap.Add(obj.RequestID, target);
                target.Add("date", new List<double>());
                target.Add("time", new List<double>());
                target.Add("close", new List<double>());
                target.Add("vol", new List<double>());
                target.Add("avg", new List<double>());
            }
            List<double> date = target["date"];
            List<double> time = target["time"];
            List<double> close = target["close"];
            List<double> vol = target["vol"];
            List<double> avg = target["avg"];

            foreach (var md in obj.MinuteDataList)
            {
                date.Add(md.Date);
                time.Add(md.Time);
                close.Add(md.Close);
                vol.Add(md.Vol);
                avg.Add(md.AvgPrice);
            }
            if (obj.IsLast)
            {

                if (OnRspQryMinuteData != null)
                {
                    Dictionary<string, double[]> data = new Dictionary<string, double[]>();
                    data["date"] = date.ToArray();
                    data["time"] = time.ToArray();
                    data["close"] = close.ToArray();
                    data["vol"] = vol.ToArray();
                    data["avg"] = avg.ToArray();

                    minuteDataResponseMap.Remove(obj.RequestID);

                    OnRspQryMinuteData(data, null, date.Count, obj.RequestID);
                }
            }
        }
        #endregion




        #region Bar数据查询
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
        public int QrySecurityBars(string exchange, string symbol, string freqStr, int start, int count)
        {
            return DataCoreService.DataClient.QryBar(exchange,symbol,GetInterval(freqStr),start,count);
        }

        /// <summary>
        /// 查询某个时间之后的Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="freqStr"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int QrySecurityBars(string exchange, string symbol, string freqStr, DateTime start, DateTime end)
        {
            return DataCoreService.DataClient.QryBar(exchange, symbol, GetInterval(freqStr), start,end);
        }



        Dictionary<int, Dictionary<string, List<double>>> barResponseMap = new Dictionary<int, Dictionary<string, List<double>>>();
        void EventHub_OnRspBarEvent(RspQryBarResponseBin obj)
        {
            Dictionary<string, List<double>> target = null;
            if (!barResponseMap.TryGetValue(obj.RequestID, out target))
            {
                target = new Dictionary<string, List<double>>();
                barResponseMap.Add(obj.RequestID,target);
                target.Add("date", new List<double>());
                target.Add("time", new List<double>());
                target.Add("open", new List<double>());
                target.Add("high", new List<double>());
                target.Add("low", new List<double>());
                target.Add("close", new List<double>());
                target.Add("vol", new List<double>());
                target.Add("amount", new List<double>());
            }
            List<double> date = target["date"];
            List<double> time = target["time"];
            List<double> open = target["open"];
            List<double> high = target["high"];
            List<double> low = target["low"];
            List<double> close = target["close"];
            List<double> vol = target["vol"];
            List<double> amount = target["amount"];

            foreach (var b in obj.Bars)
            {
                date.Add(b.EndTime.ToTLDate());
                time.Add(b.EndTime.ToTLTime());
                open.Add(b.Open);
                high.Add(b.High);
                low.Add(b.Low);
                close.Add(b.Close);
                vol.Add(b.Volume);
                amount.Add(0);
            }

            if (obj.IsLast)
            {
                if (OnRspQrySecurityBar != null)
                { 
                    Dictionary<string,double[]> data = new Dictionary<string,double[]>();
                    data["date"] = date.ToArray();
                    data["time"] = time.ToArray();
                    data["open"] = open.ToArray();
                    data["high"] = high.ToArray();
                    data["low"] = low.ToArray();
                    data["close"] = close.ToArray();
                    data["vol"] = vol.ToArray();
                    data["amount"] = amount.ToArray();
                    barResponseMap.Remove(obj.RequestID);
                    OnRspQrySecurityBar(data, null, date.Count, obj.RequestID);

                }
            }
        }


        BarFrequency GetInterval(string freqStr)
        {
            switch (freqStr)
            {
                case ConstFreq.Freq_M1: return new BarFrequency(BarInterval.Minute,1);
                case ConstFreq.Freq_M5: return new BarFrequency(BarInterval.FiveMin,1);
                case ConstFreq.Freq_M15: return new BarFrequency(BarInterval.FifteenMin, 1);
                case ConstFreq.Freq_M30: return new BarFrequency(BarInterval.ThirtyMin, 1);
                case ConstFreq.Freq_M60: return new BarFrequency(BarInterval.Hour, 1);
                case ConstFreq.Freq_Day: return new BarFrequency(BarInterval.Day, 1);
                default:
                    return new BarFrequency(BarInterval.Minute,1);
            }
        }
        #endregion


        #region 价格成交量
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
            return DataCoreService.DataClient.QryPriceVol(exchange, symbol, 0);
        }

        Dictionary<int, List<PriceVolPair>> priceVolResponseMap = new Dictionary<int, List<PriceVolPair>>();
        void EventHub_OnRspPriceVolEvent(RspXQryPriceVolResponse obj)
        {
            List<PriceVolPair> target = null;
            if (!priceVolResponseMap.TryGetValue(obj.RequestID, out target))
            {
                target = new List<PriceVolPair>();
                priceVolResponseMap.Add(obj.RequestID, target);
            }
            foreach (var pv in obj.PriceVols)
            {
                target.Add(new PriceVolPair((double)pv.Price, pv.Vol));
            }
            if (obj.IsLast)
            {

                if (OnRspQryPriceVolPair != null)
                {
                    OnRspQryPriceVolPair(target, null, target.Count, obj.RequestID);
                }
            }
        }
        #endregion


        #region 分笔成交数据
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
            return DataCoreService.DataClient.QryTrade(exchange, symbol, start, Count, 0);
        }

        Dictionary<int, List<TradeSplit>> tradeSplitResponseMap = new Dictionary<int, List<TradeSplit>>();
        void EventHub_OnRspTradeSplitEvent(RspXQryTradeSplitResponse obj)
        {
            List<TradeSplit> target = null;
            if(!tradeSplitResponseMap.TryGetValue(obj.RequestID,out target))
            {
                target = new List<TradeSplit>();
                tradeSplitResponseMap.Add(obj.RequestID,target);
            }
            foreach(var trade in obj.Trades)
            {
                target.Add(new TradeSplit(trade.Time, (double)trade.Trade, trade.Size, 0, 1));
            }
            if (obj.IsLast)
            {

                if (OnRspQryTradeSplit != null)
                { 
                    OnRspQryTradeSplit(target,null,target.Count,obj.RequestID);
                }
            }
        }
        #endregion


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

    }
}
