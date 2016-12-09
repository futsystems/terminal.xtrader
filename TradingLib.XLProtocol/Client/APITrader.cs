﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.XLProtocol;
using Common.Logging;
using TradingLib.XLProtocol.V1;


namespace TradingLib.XLProtocol.Client
{
    public class APITrader
    {

        #region 对外暴露的事件
        public event Action<ErrorField> OnRspError = delegate { };
        /// <summary>
        /// 服务端连接建立
        /// </summary>
        public event Action OnServerConnected = delegate { };

        /// <summary>
        /// 服务端连接断开
        /// </summary>
        public event Action<int> OnServerDisconnected = delegate { };


        /// <summary>
        /// 服务端登入回报
        /// </summary>
        public event Action<XLRspLoginField, ErrorField, uint, bool> OnRspUserLogin = delegate { };

        #endregion
        string _serverIP = string.Empty;
        int _port = 0;
        SocketClient _socketClient = null;
        ILog logger = LogManager.GetLogger("APITtrader");

        public APITrader(string serverIP, int port)
        {
            _serverIP = serverIP;
            _port = port;
            _socketClient = new SocketClient();
            _socketClient.ThreadBegin += new Action(_socketClient_ThreadBegin);
            _socketClient.ThreadExit += new Action(_socketClient_ThreadExit);
            _socketClient.DataReceived += new Action<XLProtocolHeader, byte[], int>(_socketClient_DataReceived);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        { 
            if(_socketClient.StartClient(_serverIP,_port))
            {
                OnServerConnected();
            }
            else
            {
                ErrorField rsp = new ErrorField();
                rsp.ErrorID = 2;
                rsp.ErrorMsg = "连接建立失败";
                OnRspError(rsp);
            }
        }

        /// <summary>
        /// 等待接口线程结束运行
        /// </summary>
        public void Join()
        {
            _socketClient.Wait();
        }

        /// <summary>
        /// 停止接口线程
        /// </summary>
        public void Release()
        {
            _socketClient.Close();
        }

        void _socketClient_DataReceived(XLProtocolHeader header, byte[] data, int offset)
        {
            XLMessageType msgType = (XLMessageType)header.XLMessageType;
            XLDataHeader dataHeader;
            XLPacketData pkt = XLPacketData.Deserialize(msgType, data, offset,out dataHeader);
            logger.Info(string.Format("Recv Pkt Type:{0}", msgType));
            switch (msgType)
            {
                case XLMessageType.T_RSP_LOGIN:
                    { 
                        ErrorField rsp = (ErrorField)pkt.FieldList[0].FieldData;
                        XLRspLoginField response = (XLRspLoginField)pkt.FieldList[1].FieldData;
                        OnRspUserLogin(response, rsp, dataHeader.RequestID, (int)dataHeader.IsLast==1?true:false);
                        break;
                    }
                default:
                    logger.Info(string.Format("Unhandled Pkt:{0}", msgType));
                    break;
            }
        }

        void _socketClient_ThreadExit()
        {
            logger.Info("SocketClient Thred Exit");
        }

        void _socketClient_ThreadBegin()
        {
            logger.Info("SocketClient Thred Begin");
        }



        /// <summary>
        /// 请求登入
        /// </summary>
        /// <param name="req"></param>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public bool ReqUserLogin(XLReqLoginField req,uint requestID)
        {
            XLPacketData pktData = new XLPacketData(XLMessageType.T_REQ_LOGIN);
            pktData.AddField(req);

            return SendPktData(pktData, XLEnumSeqType.SeqReq, requestID);
        }



        bool SendPktData(XLPacketData pktData, XLEnumSeqType seqType,uint requestID)
        {
            byte[] data = XLPacketData.PackToBytes(pktData, XLEnumSeqType.SeqReq, 0, requestID, true);
            logger.Debug(string.Format("PktData Send,Type:{0} Size:{1}", pktData.MessageType, data.Length));
            return SendData(data, data.Length);
        }
        bool SendData(byte[] data, int count)
        {
            if (_socketClient.IsOpen)
            {
                int size = _socketClient.Send(data, count);
                if (size == count)
                {
                    return true;
                }
                else
                {
                    OnServerDisconnected(0x1002);
                    return false;
                }
            }
            return false;
        }
    }
}
