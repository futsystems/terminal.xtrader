using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.DataCore
{

    public partial class DataClient
    {


        public void SendPacket(IPacket packet)
        {
            mktClient.TLSend(packet);
        }
        /// <summary>
        /// 查询交易时间段
        /// </summary>
        public void QryMarketTime()
        {
            XQryMarketTimeRequest request = RequestTemplate<XQryMarketTimeRequest>.CliSendRequest(0);
            mktClient.TLSend(request);
        }

        /// <summary>
        /// 查询交易所
        /// </summary>
        public void QryExchange()
        { 
            XQryExchangeRequuest request = RequestTemplate<XQryExchangeRequuest>.CliSendRequest(0);
            mktClient.TLSend(request);
        }

        /// <summary>
        /// 查询品种数据
        /// </summary>
        public void QrySecurity()
        {
            XQrySecurityRequest request = RequestTemplate<XQrySecurityRequest>.CliSendRequest(0);
            mktClient.TLSend(request);
        }

        /// <summary>
        /// 查询合约数据
        /// </summary>
        public void QrySymbol()
        {
            XQrySymbolRequest request = RequestTemplate<XQrySymbolRequest>.CliSendRequest(0);
            mktClient.TLSend(request);
        }

        /// <summary>
        /// 请求登入
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        public void Login(string username, string pass)
        {
            logger.Info(string.Format("Request Login Account:{0} Pass:{1}", username, pass));
            LoginRequest request = RequestTemplate<LoginRequest>.CliSendRequest(++requestid);
            request.LoginID = username;
            request.Passwd = pass;
            mktClient.TLSend(request);
        }



        /// <summary>
        /// 订阅合约实时行情
        /// </summary>
        public void RegisterSymbol(string exchange,string[] symbols)
        {
            logger.Info(string.Format("Subscribe market data for exchange:{0} symbol:{1}",exchange, string.Join(",", symbols)));
            RegisterSymbolTickRequest request = RequestTemplate<RegisterSymbolTickRequest>.CliSendRequest(NextRequestID);
            request.Exchange = exchange;
            foreach (var symbol in symbols)
            {
                Symbol sym = this.GetSymbol(exchange,symbol);
                if (sym == null)
                {
                    logger.Warn(string.Format("Symbol:{0} do not exist", symbol));
                    continue;
                }
                request.SymbolList.Add(symbol);
            }
            mktClient.TLSend(request);
        }

        /// <summary>
        /// 注销合约实时行情
        /// </summary>
        /// <param name="symbol"></param>
        public void UnRegisterSymbol(string exchange,string[] symbols)
        {
            logger.Info(string.Format("Unsubscribe market data for exchange:{0} symbols:{1}", exchange, string.Join(",", symbols)));
            UnregisterSymbolTickRequest request = RequestTemplate<UnregisterSymbolTickRequest>.CliSendRequest(NextRequestID);
            request.Exchange = exchange;
            foreach (var symbol in symbols)
            {
                Symbol sym = this.GetSymbol(exchange, symbol);
                if (sym == null)
                {
                    logger.Warn(string.Format("Symbol:{0} do not exist", symbol));
                    continue;
                }
                request.SymbolList.Add(symbol);
            }
            mktClient.TLSend(request);

            //logger.Info(string.Format("Unsubscribe market data for symbol:{0}", string.Join(",",symbols)));
            //UnregisterSymbolTickRequest request = RequestTemplate<UnregisterSymbolTickRequest>.CliSendRequest(NextRequestID);
                
            //foreach (var symbol in symbols)
            //{
            //    if (symbol != "*")//过滤统配符
            //    {
            //        Symbol sym = this.GetSymbol(symbol);
            //        if (sym == null)
            //        {
            //            logger.Warn(string.Format("Symbol:{0} do not exist", symbol));
            //            continue;
            //        }
            //    }
            //    request.SymbolList.Add(symbol);
            //}
            //histClient.TLSend(request);
        }



        //public int QryBar(string symbol, int interval, DateTime start, DateTime end, int maxcount)
        //{
        //    return QryBar(symbol, interval, start, end, maxcount, true);
        //}

        /// <summary>
        /// 以开始位置和最大返回数量为条件查询Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public int QryBar(string exchange, string symbol, BarFrequency freq, int startIndex, int maxCount)
        {
            return QryBar(exchange, symbol,freq.Type,freq.Interval, DateTime.MinValue, DateTime.MaxValue, startIndex, maxCount);
        }

        /// <summary>
        /// 查询某个时间以来的所有Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int QryBar(string exchange, string symbol, BarFrequency freq, DateTime start,DateTime end)
        {
            return QryBar(exchange, symbol, freq.Type, freq.Interval, start, end, 0, 0);
        }


        /// <summary>
        /// 底层查询Bar数据接口
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval">默认为时间周期的Bar数据</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="maxcount"></param>
        /// <param name="fromend"></param>
        public int QryBar(string exchange,string symbol,BarInterval type, int interval,DateTime start,DateTime end,int startIndex,int maxCount,bool fromend = false,bool havepartial = true)
        {
            int reqid = NextRequestID;
            QryBarRequest request = RequestTemplate<QryBarRequest>.CliSendRequest(reqid);
            request.Exchange = exchange;
            request.FromEnd = fromend;
            request.Symbol = symbol;
            request.MaxCount = maxCount;
            request.StartIndex = startIndex;
            request.Interval = interval;
            request.IntervalType = type;
            request.StartTime = start;
            request.EndTime = end;
            request.BarResponseType = EnumBarResponseType.BINARY;
            request.HavePartial = havepartial;
            mktClient.TLSend(request);
            return reqid;
        }

        /// <summary>
        /// 查询成交明细
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="startIdx"></param>
        /// <param name="macount"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int QryTrade(string exchange, string symbol, int startIdx, int macount, int date)
        {
            int reqid = NextRequestID;
            XQryTradeSplitRequest request = RequestTemplate<XQryTradeSplitRequest>.CliSendRequest(reqid);
            request.Exchange = exchange;
            request.Symbol = symbol;
            request.StartIndex = startIdx;
            request.MaxCount = macount;
            request.Tradingday = date;

            mktClient.TLSend(request);
            return reqid;
        }

        public int DemoTick(int time, decimal price)
        { 
            int reqid = NextRequestID;
            MDDemoTickRequest request = RequestTemplate<MDDemoTickRequest>.CliSendRequest(reqid);
            request.Time = time;
            request.Trade = price;

            mktClient.TLSend(request);
            return reqid;
        }

        /// <summary>
        /// 管理扩展请求
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <param name="cmdStr">命令编号</param>
        /// <param name="args">命令参数</param>
        /// <returns></returns>
        public int ReqContribRequest(string moduleId, string cmdStr, string args)
        {
            int reqid = NextRequestID;
            MGRContribRequest request = RequestTemplate<MGRContribRequest>.CliSendRequest(reqid);
            request.ModuleID = moduleId;
            request.CMDStr = cmdStr;
            request.Parameters = args;

            mktClient.TLSend(request);
            return reqid;
        }
    }
}
