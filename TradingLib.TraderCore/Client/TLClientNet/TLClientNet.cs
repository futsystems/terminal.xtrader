using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {
        const string PROGRAME = "TLClientNet";
        ILog logger = LogManager.GetLogger(PROGRAME);
        string[] _servers = new string[] { };
        int _port = 5570;

        TLClient_MQ connecton = null;
        bool _firstlogin = true;
        string _account = "";
        int requestid = 0;
        int _tradingday = 0;

        public int TradingDay { get { return _tradingday; } }
        public bool IsConnected
        {
            get
            {
                if (connecton == null) return false;
                return connecton.IsConnected;
            }
        }

        public bool IsTickConnected
        {
            get
            {
                if (connecton == null) return false;
                return connecton.IsTickConnected;
            }
        }

        
        public TLClientNet(string[] servers, int port)
        {
            _servers = servers;
            _port = port;
        }


        ///// <summary>
        ///// 启动行情连接
        ///// </summary>
        //public void StartTick()
        //{
        //    if (connecton != null && connecton.IsConnected)
        //    {
        //        connecton.StartTick();
        //    }
        //}


        public void Start()
        {
            if (!IsConnected)
            {
                logger.Info("TLClientNet Starting......");
                connecton = new TLClient_MQ(_servers, _port, "Trader");
                //connecton.ProviderType = QSEnumProviderType.Both;
                BindConnectionEvent();
                connecton.Start();

            }
        }

        public void Stop()
        {
            if (IsConnected)
            {
                logger.Info("TLClientNet Stopping......");
                if (connecton != null && connecton.IsConnected)
                {
                    connecton.Stop();
                }
                connecton = null;
            }
        }

        void BindConnectionEvent()
        {
            connecton.OnConnectEvent += new ConnectDel(connecton_OnConnectEvent);
            connecton.OnDisconnectEvent += new DisconnectDel(connecton_OnDisconnectEvent);
            connecton.OnDataPubConnectEvent += new DataPubConnectDel(connecton_OnDataPubConnectEvent);
            connecton.OnDataPubDisconnectEvent += new DataPubDisconnectDel(connecton_OnDataPubDisconnectEvent);
            connecton.OnLoginResponse += new LoginResponseDel(connecton_OnLoginResponse);
            connecton.OnPacketEvent += new IPacketDelegate(connecton_OnPacketEvent);
        }



        public void Reset()
        {
            _firstlogin = true;
            _account = string.Empty;
            _tradingday = 0;
            requestid = 0;
        }


        void SendPacket(IPacket packet)
        {
            //权限或者登入状态检查
            if (connecton != null && connecton.IsConnected)
            {
                connecton.TLSend(packet);
            }
        }


        /// <summary>
        /// 判断某个回报消息是否是错误消息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool IsRspInfoError(RspInfo info)
        {
            if (info != null && info.ErrorID != 0) return true;
            return false;
        }
    }
}
