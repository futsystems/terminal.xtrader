using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.DataCore
{
    /// <summary>
    /// 行情客户端 底层TLClient服务 用于维护连接等基础工作
    /// TLSocket_TCP 基于TCP通讯实现的数据收发组件
    /// 实现业务层的逻辑封装 并且与DataCoreService实现数据绑定
    /// </summary>
    public partial class DataClient
    {
        ILog logger = LogManager.GetLogger("MDClient");

        TLClient<TLSocket_TCP> mktClient = null;

        int requestid = 0;
        object _reqidobj = new object();
        protected int NextRequestID
        {
            get
            {
                lock (_reqidobj)
                {
                    return ++requestid;
                }
            }
        }


        /// <summary>
        /// 单台服务器同时提供行情与地址
        /// </summary>
        /// <param name="address"></param>
        /// <param name="realport"></param>
        /// <param name="histport"></param>
        public DataClient(string address, int dataPort)
            : this(new string[] { address }, dataPort)
        {

        }

        /// <summary>
        /// 提供一组实时行情地址与历史行情地址
        /// </summary>
        /// <param name="realservers"></param>
        /// <param name="realport"></param>
        /// <param name="histservers"></param>
        /// <param name="histport"></param>
        public DataClient(string[] dataServers, int dataPort)
        {
            mktClient = new TLClient<TLSocket_TCP>(dataServers, dataPort, "MDClient");
            //绑定事件
            WireEvent();
        }


        public void Start()
        {
            logger.Info("Start MDClient");
            mktClient.Start();
        }

        public void Stop()
        {
            logger.Info("Stop MDClient");
            mktClient.Stop();
        }
        /// <summary>
        /// 绑定事件
        /// </summary>
        void WireEvent()
        {
            mktClient.OnConnectEvent += new ConnectDel(OnConnectEvent);
            mktClient.OnDisconnectEvent += new DisconnectDel(OnDisconnectEvent);
            mktClient.OnPacketEvent += new Action<IPacket>(OnPacketEvent);
        }

        /// <summary>
        /// 解绑事件
        /// </summary>
        void UnwireEvent()
        {
            mktClient.OnConnectEvent -= new ConnectDel(OnConnectEvent);
            mktClient.OnDisconnectEvent -= new DisconnectDel(OnDisconnectEvent);
            mktClient.OnPacketEvent -= new Action<IPacket>(OnPacketEvent);
        }


        void OnPacketEvent(IPacket obj)
        {
            //logger.Debug(string.Format("Hist Packet Type:{0} Content:{1}", obj.Type, obj.Content));
            switch (obj.Type)
            {
                case MessageTypes.TICKNOTIFY:
                    {
                        TickNotify response = obj as TickNotify;
                        DataCoreService.EventHub.FireRtnTickEvent(response.Tick);
                        return;
                    }
                case MessageTypes.XTICKSNAPSHOTRESPONSE:
                    {
                        RspXQryTickSnapShotResponse response = obj as RspXQryTickSnapShotResponse;
                        DataCoreService.EventHub.FireRtnTickEvent(response.Tick);
                        return;
                    }
                case MessageTypes.XMARKETTIMERESPONSE:
                    {
                        RspXQryMarketTimeResponse response = obj as RspXQryMarketTimeResponse;
                        OnXQryMarketTimeResponse(response);
                        return;
                    }
                case MessageTypes.XEXCHANGERESPNSE:
                    {
                        RspXQryExchangeResponse response = obj as RspXQryExchangeResponse;
                        OnXQryExchangeResponse(response);
                        return;
                    }
                case MessageTypes.XSECURITYRESPONSE:
                    {
                        RspXQrySecurityResponse response = obj as RspXQrySecurityResponse;
                        OnXQrySecurityResponse(response);
                        return;
                    }
                case MessageTypes.XSYMBOLRESPONSE:
                    {
                        RspXQrySymbolResponse response = obj as RspXQrySymbolResponse;
                        OnXQrySymbolResponse(response);
                        return;
                    }
                //case MessageTypes.BARRESPONSE:
                //    {
                //        RspQryBarResponse response = obj as RspQryBarResponse;
                        
                        
                //        return;
                //    }
                case MessageTypes.LOGINRESPONSE:
                    {
                        LoginResponse response = obj as LoginResponse;
                        DataCoreService.EventHub.FireLoginEvent(response);
                        return;
                    }
                //查询Bar数据回报
                case MessageTypes.BIN_BARRESPONSE:
                    {
                        RspQryBarResponseBin response = obj as RspQryBarResponseBin;
                        DataCoreService.EventHub.FireOnRspBarEvent(response);
                        return;
                    }
                //查询分笔成交数据
                case MessageTypes.XQRYTRADSPLITRESPONSE:
                    {
                        RspXQryTradeSplitResponse response = obj as RspXQryTradeSplitResponse;
                        DataCoreService.EventHub.FireOnRspTradeSplitEvent(response);
                        return;
                    }
                //查询价格成交量分布
                case MessageTypes.XQRYPRICEVOLRESPONSE:
                    {
                        RspXQryPriceVolResponse response = obj as RspXQryPriceVolResponse;
                        DataCoreService.EventHub.FireOnRspPriceVolEvent(response);
                        return;
                    }
                //查询分时数据
                case MessageTypes.XQRYMINUTEDATARESPONSE:
                    {
                        RspXQryMinuteDataResponse response = obj as RspXQryMinuteDataResponse;
                        DataCoreService.EventHub.FireOnRspMinuteDataEvent(response);
                        return;
                    }

                
                #region 管理操作
                case MessageTypes.MGRCONTRIBRESPONSE:
                    { 
                        RspMGRContribResponse response = obj as RspMGRContribResponse;
                        DataCoreService.EventContrib.OnMGRContribResponse(response);
                        return;
                    }
                case MessageTypes.MGRCONTRIBRNOTIFY:
                    {
                        NotifyMGRContribNotify notify = obj as NotifyMGRContribNotify;
                        DataCoreService.EventContrib.OnMGRContribNotifyResponse(notify);
                        return;
                    }


                //合约更新回报
                case MessageTypes.MGRUPDATESYMBOLRESPONSE:
                    {
                        RspMGRUpdateSymbolResponse response = obj as RspMGRUpdateSymbolResponse;
                        this.OnMGRUpdateSymbol(response);
                        DataCoreService.EventManager.FireOnMGRUpdateSymbolResponse(response);

                        break;
                    }
                //品种更新回报
                case MessageTypes.MGRUPDATESECURITYRESPONSE:
                    {
                        RspMGRUpdateSecurityResponse response = obj as RspMGRUpdateSecurityResponse;
                        this.OnMGRUpdateSecurity(response);
                        DataCoreService.EventManager.FireOnMGRUpdateSecurityResponse(response);
                        break;
                    }
                //更新交易所回报
                case MessageTypes.MGRUPDATEEXCHANGERESPONSE:
                    {
                        RspMGRUpdateExchangeResponse response = obj as RspMGRUpdateExchangeResponse;
                        this.OnMGRUpdateExchange(response);
                        DataCoreService.EventManager.FireOnMGRUpdateExchangeResponse(response);
                        break;
                    }
                case MessageTypes.MGRUPDATEMARKETTIMERESPONSE:
                    {
                        RspMGRUpdateMarketTimeResponse response = obj as RspMGRUpdateMarketTimeResponse;
                        this.OnMGRUpdateMarketTime(response);
                        DataCoreService.EventManager.FireOnMGRUpdateMarketTimeResponse(response);
                        break;
                    }
                #endregion


                default:
                    logger.Warn(string.Format("Message Type:{0} not supported", obj.Type));
                    return;
            }
        }

        void OnDisconnectEvent()
        {
            logger.Info(string.Format("Hist Socket Disconnected"));
            DataCoreService.EventHub.FireDisconnectedEvent();
        }

        void OnConnectEvent()
        {
            logger.Info(string.Format("Hist Socket Connected Server:{0} Port:{1}", mktClient.CurrentServer.Address, mktClient.CurrentServer.Port));
            DataCoreService.EventHub.FireConnectedEvent();
            //执行登入

            //执行查询
            //QryMarketTime();
        }

    }
}
