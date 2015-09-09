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
            logger.Info("Server disconnected");
            CoreService.EventCore.FireDisconnectedEvent();
        }

        void connecton_OnConnectEvent()
        {
            logger.Info("Server connected");
            CoreService.EventCore.FireConnectedEvent();
        }
    }
}
