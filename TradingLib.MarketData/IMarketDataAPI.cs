using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    /// <summary>
    /// 报价面板信息
    /// 用于初始化报价列表
    /// 形成不同Tab显示不同的合约组合
    /// </summary>
    public class BlockInfo
    {
        public BlockInfo(string name, Predicate<MDSymbol> filter, int viewType)
        {
            this.Title = name;
            this.Filter = filter;
            this.QuoteViewType = viewType;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 合约过滤器
        /// </summary>
        public Predicate<MDSymbol> Filter { get; set; }

        /// <summary>
        /// 报价显示类别
        /// </summary>
        public int QuoteViewType { get; set; }
    }


    public enum EnumMDTickMode
    {
        /// <summary>
        /// 定时查询
        /// </summary>
        FreqQry,
        /// <summary>
        /// 注册
        /// </summary>
        Register,
    
    }

    /// <summary>
    /// 行情接口工作参数
    /// </summary>
    public class MarketDataAPISetting
    {
        /// <summary>
        /// 实时行情类别
        /// </summary>
        public EnumMDTickMode TickMode { get; set; }

        /// <summary>
        /// 是否支持通过时间来请求Bar数据
        /// </summary>
        public bool QryBarTimeSupport { get; set; }
    }



    public interface IMarketDataAPI
    {
        /// <summary>
        /// 合约列表
        /// 用于报价列表显示的报价合约
        /// </summary>
        IEnumerable<MDSymbol> Symbols { get; }

        /// <summary>
        /// 底部亮显合约
        /// </summary>
        IEnumerable<SymbolHighLight> HightLightSymbols { get; }

        /// <summary>
        /// 板块列表
        /// 用于报价列表显示的Tab页
        /// </summary>
        IEnumerable<BlockInfo> BlockInfos { get; }

        /// <summary>
        /// 工作参数
        /// </summary>
        MarketDataAPISetting APISetting { get; }

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


        
        
        #region 查询操作

        //行情获取方式1 查询
        event Action<List<MDSymbol>, RspInfo, int, int> OnRspQryTickSnapshot;
        /// <summary>
        /// 查询行情快照
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        int QryTickSnapshot(MDSymbol[] symbols);



        event Action<MDSymbol> OnRtnTick;
        //行情获取方式2 订阅/注销
        /// <summary>
        /// 注册合约行情
        /// </summary>
        /// <param name="symbols"></param>
        void RegisterSymbol(MDSymbol[] symbols);

        /// <summary>
        /// 注销合约行情
        /// </summary>
        /// <param name="symbols"></param>
        void UnregisterSymbol(MDSymbol[] symbols);


        /// <summary>
        /// 返回当日分时数据
        /// </summary>
        event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQryMinuteData;

        /// <summary>
        /// 查询分时数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int QryMinuteDate(string exchange, string symbol,int date);


        


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
        int QrySecurityBars(string exchange, string symbol, string freqStr, int start, int count);

        /// <summary>
        /// 查询某个时间之后的Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="freqStr"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        int QrySecurityBars(string exchange, string symbol, string freqStr, DateTime start, DateTime end);




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
