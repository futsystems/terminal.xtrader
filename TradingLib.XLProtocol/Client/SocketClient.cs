using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Common.Logging;

namespace TradingLib.XLProtocol.Client
{


    public class SocketClient
    {

        ILog logger = LogManager.GetLogger("SocketClient");

        Socket _socket = null;
        Thread _thread = null;
        int _bufferSize = 65535;

        public event Action ThreadBegin = delegate { };
        public event Action ThreadExit = delegate { };
        public event Action<XLProtocolHeader, byte[], int> DataReceived = delegate { };
        public event Action ConnectionDropped = delegate { };
        public event Action ConnectionError = delegate { };

        ManualResetEvent manualReset = new ManualResetEvent(false);

        public bool IsOpen
        {
            get
            {
                return _socket != null;
            }
        }

        public void Close()
        {
            SafeCloseSocket();
        }

        public bool StartClient(string serverIP,int port)
        {
            if (this.IsOpen) return false;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint server = new IPEndPoint(IPAddress.Parse(serverIP), port);
            socket.Connect(server);

            if (socket.Connected)
            {

                _socket = socket;

            }
            else
            {
                _socket = null;
                return false;
            }
            _thread = new Thread(SocketClientProc);
            _thread.IsBackground = true;
            _thread.Start();
            manualReset.Reset();
            return true;
        }

        public int Send(byte[] data, int count)
        {
            if (!IsOpen) return -1;
            int ret = _socket.Send(data, 0, count, SocketFlags.None);
            return ret >= 0 ? ret : -1;
        }

        void SocketClientProc()
        {
            byte[] buffer = new byte[_bufferSize];
            int bufferOffset = 0;

            //Notify
            ThreadBegin();

            try
            {
                while (this.IsOpen)
                {
                    int ret = _socket.Receive(buffer, bufferOffset, buffer.Length - bufferOffset, SocketFlags.None);
                    if (ret > 0)
                    {
                        int dataLen = ret + bufferOffset;
                        int offset = 0;
                        bool parseFlag = true;
                        int pktLen = 0;
                        while (parseFlag)
                        {
                            if (dataLen - offset >= XLConstants.PROTO_HEADER_LEN)
                            {
                                XLProtocolHeader header = XLStructHelp.BytesToStruct<XLProtocolHeader>(buffer, offset);
                                //buffer包含了一个完整的协议数据包 
                                pktLen = XLConstants.PROTO_HEADER_LEN + header.XLMessageLength;
                                if (dataLen - offset >= pktLen)
                                {
                                    //byte[] pdata = new byte[pktLen];
                                    //Array.Copy(buffer, offset, pdata, 0, pktLen);
                                    DataReceived(header, buffer, offset + XLConstants.PROTO_HEADER_LEN);
                                    offset += pktLen;
                                }
                                else //当前数据没有完整的包含一个协议数据包 则不进行解析
                                {
                                    parseFlag = false;
                                }
                            }
                            else
                            {
                                //如果当前可用数据小于协议头长度 则不进行解析
                                parseFlag = false;
                            }

                            //将剩余数据复制到缓存中
                            if (!parseFlag)
                            {
                                byte[] pdata = new byte[dataLen - offset];
                                Array.Copy(buffer, offset, pdata, 0, dataLen - offset);
                                Array.Copy(pdata, 0, buffer, 0, dataLen - offset);
                                bufferOffset = dataLen - offset;
                            }
                        }
                    }
                    else if (ret <= 0)
                    {
                        SafeCloseSocket();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("SocketClientProc Error:{0}", ex.ToString()));
            }
            //logger.Info("SocketClientProc Terminated");
            manualReset.Set();
            ThreadExit();
        }

        /// <summary>
        /// 等待线程终止
        /// </summary>
        public void Wait()
        {
            manualReset.WaitOne();
        }

        

        void SafeCloseSocket()
        {
            if (_socket == null)
                return;

            //if (!_socket.Connected)
            //{
            //    _socket = null;
            //    return;
            //}
            //try
            //{
            //    _socket.Shutdown(SocketShutdown.Both);
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("Socket Shutdown error:" + ex.ToString());
            //}

            try
            {
                _socket.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Socket Close Error:" + ex.ToString());
            }
            _socket = null;
        }  


        bool IsSocketConnected(Socket client, out int errorcode)
        {
            errorcode = 0;
            if (client == null) return false;
            bool blockingState = client.Blocking;

            try
            {
                byte[] tmp = new byte[1];

                client.Blocking = false;
                client.Send(tmp, 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    logger.Info("connected but send blocked.");
                    return true;
                }
                else
                {
                    errorcode = e.NativeErrorCode;
                    logger.Info("disconnected, error: " + errorcode);
                    return false;
                }
            }
            finally
            {
                client.Blocking = blockingState;
            }
            return client.Connected;
        }
    }




}
