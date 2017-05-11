using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class AppConfig
    {
        /// <summary>
        /// 配置单元编号
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 行情服务器地址列表
        /// </summary>
        public string MarketAddress { get; set; }

        /// <summary>
        /// 行情服务器端口
        /// </summary>
        public int MarketPort { get; set; }

        /// <summary>
        /// 交易服务器地址
        /// </summary>
        //public string BrokerAddress { get; set; }

        /// <summary>
        /// 交易服务器端口
        /// </summary>
        //public int BrokerPort { get; set; }
    }
}
