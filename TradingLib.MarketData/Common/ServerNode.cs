using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class ServerNode
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
