using System;
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
        /// 断开链接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 是否处于链接状态
        /// </summary>
        bool Connected { get; }



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
        /// 返回历史分时数据
        /// </summary>
        event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQryHistMinuteData;
        /// <summary>
        /// 查询分时数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int QryMinuteDate(string exchange, string symbol,int date);

        /// <summary>
        /// 返回当日分时数据
        /// </summary>
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



        /// <summary>
        /// 量价信息回报
        /// </summary>
        event Action<List<PriceVolPair>, RspInfo, int, int> OnRspQryPriceVolPair;
        /// <summary>
        /// 查询价量信息
        /// 在什么价位 成交多少数量
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int QryPriceVol(string exchange, string symbol);


        /// <summary>
        /// 分笔数据回报
        /// </summary>
        event Action<List<TradeSplit>, RspInfo, int, int> OnRspQryTradeSplit;
        /// <summary>
        /// 查询分笔数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="start"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        int QryTradeSplitData(string exchange, string symbol, int start, int Count);


        /// <summary>
        /// 查询合约信息类别回报
        /// </summary>
        event Action<List<SymbolInfoType>, RspInfo, int, int> OnRspQrySymbolInfoType;
        /// <summary>
        /// 查询合约信息类别
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int QrySymbolInfoType(string exchange, string symbol);


        /// <summary>
        /// 查询合约信息回报
        /// </summary>
        event Action<string, RspInfo, int, int> OnRspQrySymbolInfo;
        /// <summary>
        /// 查询合约信息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int QrySymbolInfo(string exchange, string symbol, SymbolInfoType type);
        #endregion
    }
}
