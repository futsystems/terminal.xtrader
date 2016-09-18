﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public interface IMarketDataAPI
    {

        /// <summary>
        /// 连接服务端
        /// </summary>
        void Connect(string[] hosts, int port);

        /// <summary>
        /// 登入服务端
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        void Login(string username, string pass);


        /// <summary>
        /// 获得所有合约
        /// </summary>
        IEnumerable<MDSymbol> Symbols { get; }

        /// <summary>
        /// 启动后台数据处理线程
        /// </summary>
        //void Start();

        #region 查询操作

        /// <summary>
        /// 行情快照查询回报
        /// </summary>
        event Action<List<MDSymbol>, RspInfo, int, int> OnRspQryTickSnapshot;
        /// <summary>
        /// 查询行情快照
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        int QryTickSnapshot(MDSymbol[] symbols);


        /// <summary>
        /// 查询分时数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int QryMinuteDate(string exchange, string symbol,int date);

        event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQryMinuteData;

        /// <summary>
        /// Bar数据回报
        /// </summary>
        event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQrySecurityBar;
        
        /// <summary>
        /// 查询Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="freqStr"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        int QrySeurityBars(string exchange, string symbol, string freqStr, int start, int count);

        #endregion
    }
}
