using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FutsTrader
{
    public class ServerCfg
    {

        public ServerCfg(string ipaddress, string name)
        {
            this.IPAddress = ipaddress;
            this.Name = string.IsNullOrEmpty(name) ? "交易服务器" : name;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string Name { get; set; }
    }
}
