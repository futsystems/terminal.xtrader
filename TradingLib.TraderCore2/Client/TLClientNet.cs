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
        const string PROGRAME = "TLClient";
        ILog logger = LogManager.GetLogger(PROGRAME);

        /// <summary>
        /// 服务器地址列表
        /// </summary>
        string[] _servers = new string[] { };
        /// <summary>
        /// 通讯端口
        /// </summary>
        int _port = 5570;

        TLClient<TCPSocket> connecton = null;
       
        string _account = "";

        string _username = string.Empty;
        string _password = string.Empty;

        public string UserName { get { return _username; } }

        public string Password { get { return _password; } }


        int _tradingday = 0;
        string _clientID = string.Empty;
        int _frontID = 0;
        int _sessionID = 0;

        /// <summary>
        /// 客户端UUID
        /// </summary>
        public string ClientID { get { return _clientID; } }

        /// <summary>
        /// 前置ID
        /// </summary>
        public int FrontID { get { return _frontID; } }

        /// <summary>
        /// 会话ID
        /// </summary>
        public int SessionID { get { return _sessionID; } }

        /// <summary>
        /// 当前交易日
        /// </summary>
        public int TradingDay { get { return _tradingday; } }
        
        /// <summary>
        /// 是否处于连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (connecton == null) return false;
                return connecton.IsConnected;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="port"></param>
        public TLClientNet(string[] servers, int port)
        {
            _servers = servers;
            _port = port;
        }

        /// <summary>
        /// 启动TLClient
        /// </summary>
        public void Start()
        {
            if (IsConnected) return;

            logger.Info("Starting......");
            connecton = new TLClient<TCPSocket>(_servers, _port, "TraderCore");
            BindConnectionEvent();
            connecton.Start();
        }

        /// <summary>
        /// 停止TLClient
        /// </summary>
        public void Stop()
        {
            if (!IsConnected) return;

            logger.Info("Stopping......");
            connecton.Stop();
            connecton = null;
            
        }

        /// <summary>
        /// 绑定连接事件
        /// </summary>
        void BindConnectionEvent()
        {
            connecton.OnConnectEvent += new ConnectDel(connecton_OnConnectEvent);
            connecton.OnDisconnectEvent += new DisconnectDel(connecton_OnDisconnectEvent);
            connecton.OnPacketEvent +=new Action<IPacket>(connecton_OnPacketEvent);
            connecton.OnNegotiationEvent += new Action<TLNegotiation, string, string>(connecton_OnNegotiationEvent);
        }

        void connecton_OnNegotiationEvent(TLNegotiation arg1, string arg2, string arg3)
        {
            if(connecton == null) return;
            if (arg1 == null)
            {
                connecton.Stop();
                return;
            }
            string rawstr = string.Empty;
            try
            {
                rawstr = StringCipher.Decrypt(arg1.NegoResponse, arg2);
            }
            catch (Exception ex)
            {
                logger.Error("Negotiation Error");
            }

            if (rawstr != arg3)
            {
                connecton.Stop();
            }

        }



        public void Reset()
        {
            _account = string.Empty;
            _tradingday = 0;
            requestid = 0;
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
