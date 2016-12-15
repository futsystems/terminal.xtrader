using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.TraderCore
{
    /// <summary>
    /// 基础事件处理
    /// </summary>
    public partial class TLClientNet
    {

        void connecton_OnDataPubDisconnectEvent()
        {
            logger.Info("Data Disconnected");
            CoreService.EventCore.FireDataDisconnectedEvent();
        }

        void connecton_OnDataPubConnectEvent()
        {
            logger.Info("Data Connected");
            CoreService.EventCore.FireDataConnectedEvent();
        }

        void connecton_OnDisconnectEvent()
        {
            logger.Info("Server Disconnected");
            CoreService.EventCore.FireDisconnectedEvent();
        }

        void connecton_OnConnectEvent()
        {
            logger.Info("Server Connected");
            CoreService.EventCore.FireConnectedEvent();
        }

        string _clientID = string.Empty;
        /// <summary>
        /// 客户端UUID 每次物理连接创建后唯一
        /// </summary>
        public string ClientID { get { return _clientID; } }

        int _frontID = 0;
        public int FrontID { get { return _frontID; } }

        int _sessionID = 0;
        public int SessionID { get { return _sessionID; } }
        /// <summary>
        /// 响应底层暴露上来的登入回报事件
        /// </summary>
        /// <param name="response"></param>
        
        

    }
}
