using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;



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

    /// <summary>
    /// 行情服务器地址配置
    /// 1.DataFarm整体配置
    /// 2.部署单独配置
    /// 如果部署单独配置包含有行情服务器 则优先使用独享行情服务器否则共享DataFarm整体配置
    /// 
    /// 行情服务器地址获取后 随机排序后导出到DataClient 用于随机进行链接 避免按序过度链接到第一个服务器
    /// </summary>
    [DataContract]
    public class DataFarmConfig
    {
        //[DataMember(Name = "unit")]
        //public string Unit { get; set; }
        /// <summary>
        /// 行情服务器地址列表
        /// </summary>
        [DataMember(Name = "market_server")]
        public string MarketServer { get; set; }

        /// <summary>
        /// 行情服务器端口
        /// </summary>
        [DataMember(Name = "market_port")]
        public int MarketPort { get; set; }
    }


    [DataContract]
    public class HistServerConfig
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "port")]
        public int Port { get; set; }
    }


    [DataContract]
    public class DeployConfig
    {
        /// <summary>
        /// 行情服务器地址列表
        /// </summary>
        [DataMember(Name = "deploy")]
        public string DeployID { get; set; }

        [DataMember(Name = "is_exist")]
        public bool IsExist { get; set; }

        [DataMember(Name = "is_avabile")]
        public bool IsAvabile { get; set; }

        [DataMember(Name = "hist_server")]
        public HistServerConfig HistServerConfig { get; set; }

    }

}
