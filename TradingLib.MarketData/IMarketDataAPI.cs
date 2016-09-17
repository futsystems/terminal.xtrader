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
    }
}
