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
        //#region Event

        ///// <summary>
        ///// 行情回调
        ///// </summary>
        //public event TickDelegate OnTickEvent;
        ///// <summary>
        ///// 委托回报回调
        ///// </summary>
        //public event OrderDelegate OnOrderEvent;

        ///// <summary>
        ///// 成交回调
        ///// </summary>
        //public event FillDelegate OnTradeEvent;


        //#endregion

        TLClient_MQ connecton = null;

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

        string[] _servers = new string[] { };
        int _port = 5570;


        string _account = "";
        public TLClientNet(string[] servers, int port)
        {
            _servers = servers;
            _port = port;
            //_noverb = !verb;

        }

        /// <summary>
        /// 启动行情连接
        /// </summary>
        public void StartTick()
        {
            if (connecton != null && connecton.IsConnected)
            {
                connecton.StartTick();
            }
        }


        public void Start()
        {
            logger.Info("TLClientNet Starting......");
            connecton = new TLClient_MQ(_servers, _port, "Trader");
            connecton.ProviderType = QSEnumProviderType.Both;
            BindConnectionEvent();

            connecton.Start();


        
        }

        public void Stop()
        {
            logger.Info("TLClientNet Stopping......");
            if (connecton != null && connecton.IsConnected)
            {
                connecton.Stop();
            }
            connecton = null;
            
        }

        void BindConnectionEvent()
        {
            //connecton.OnDebugEvent += new DebugDelegate(connecton_OnDebugEvent);
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
        }

        int requestid = 0;


        #region 底层连接暴露上来的通道





        void CliOnPositionUpdateNotify(PositionNotify response)
        {
            logger.Info("got postion notify:" + response.Position.ToString());
            //if (OnPositionUpdateEvent != null)
            //    OnPositionUpdateEvent(response.Position);
        }

        void CliOnSettleInfoConfirm(RspQrySettleInfoConfirmResponse response)
        {
            logger.Info("got confirmsettleconfirm data:" + response.ConfirmDay.ToString() + " time:" + response.ConfirmTime.ToString());

        }

        void CliOnSettleInfo(RspQrySettleInfoResponse response)
        {
            logger.Info("got settleinfo:");
            string[] rec = response.Content.Split('\n');
            foreach (string s in rec)
            {
                logger.Info(s);
            }
        }


        void CliOnOrderAction(OrderActionNotify response)
        {
            logger.Info("got order action:" + response.ToString());
        }

        #region 查询




        /// <summary>
        /// 查询委托回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnRspQryOrderResponse(RspQryOrderResponse response)
        {
            logger.Info("##Order:" + response.OrderToSend.ToString());
        }
        /// <summary>
        /// 查询成交回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnRspQryTradeResponse(RspQryTradeResponse response)
        { 
        
        }

        /// <summary>
        /// 查询持仓回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnRspQryPositionResponse(RspQryPositionResponse response)
        { 
        
        }

        void CliOnRspQryInvestorResponse(RspQryInvestorResponse response)
        { 
            
        }
        #endregion



        

        int _tradingday = 0;

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

        #endregion



    }
}
