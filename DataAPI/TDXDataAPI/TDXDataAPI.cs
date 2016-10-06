using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using Common.Logging;

using TradingLib.MarketData;

namespace DataAPI.TDX
{
    internal class RawData
    {
        public TGPNAME TGPNAME;
        public PowerData PowerData;
        public FinanceData FinanceData;
    }

    public partial class TDXDataAPI : IMarketDataAPI
    {
        ILog logger = LogManager.GetLogger("TDXDataAPI");

        Profiler _profiler = new Profiler();
        /// <summary>
        /// 连接建立事件
        /// </summary>
        public event Action OnConnected = delegate() { };

        /// <summary>
        /// 连接断开事件
        /// </summary>
        public event Action OnDisconnectd = delegate() { };


        public event Action OnLoginSuccess = delegate() { };


        public event Action OnLoginFail = delegate() { };

        /// <summary>
        /// 查询价格分布信息回报事件
        /// </summary>
        public event Action<List<PriceVolPair>, RspInfo, int, int> OnRspQryPriceVolPair;

        
        /// <summary>
        /// 查询当日分笔数据回报事件
        /// </summary>
        public event Action<List<TradeSplit>, RspInfo, int, int> OnRspQryTradeSplit;

        /// <summary>
        /// 查询历史分笔数据回报事件
        /// </summary>
        public event Action<TradeSplit, RspInfo, int, bool> OnRspQryHistTradeSplit;


        /// <summary>
        /// 查询Bar数据回报事件
        /// </summary>
        //public event Action<double[][], RspInfo, int> OnRspQrySecurityBar;




        /// <summary>
        /// 行情快照查询回报
        /// </summary>
        public event Action<List<MDSymbol>, RspInfo, int, int> OnRspQryTickSnapshot;


        /// <summary>
        /// 分时数据回报事件
        /// </summary>
        public event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQryMinuteData;


        /// <summary>
        /// 分时数据回报事件
        /// </summary>
        public event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQryHistMinuteData;

        /// <summary>
        /// 查询Bar数据响应
        /// </summary>
        public event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQrySecurityBar;


        /// <summary>
        /// 查询基础信息类别回报
        /// </summary>
        public event Action<List<SymbolInfoType>, RspInfo, int, int> OnRspQrySymbolInfoType;

        /// <summary>
        /// 查询基础信息回报
        /// </summary>
        public event Action<string, RspInfo, int, int> OnRspQrySymbolInfo;


       
        Socket m_hSocket = null;

        /// <summary>
        /// 请求队列
        /// </summary>
        private Queue SendList = new Queue();


        int _requestId = 0;
        /// <summary>
        /// 下一个请求ID
        /// </summary>
        int NextRequestId
        {
            get
            {
                lock (this)
                {
                    return ++_requestId;
                }
            }
        }

        string[] _hosts = null;
        int _port = 0;

        public TDXDataAPI()
        {
            blockInfoList.Add(new BlockInfo("所有A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" || symbol.BlockType == "5" || symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }), 1));
            blockInfoList.Add(new BlockInfo("中小版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
               =>
               {
                   if (symbol.BlockType == "5")
                   {
                       return true;
                   }
                   return false;
               }), 4));

            blockInfoList.Add(new BlockInfo("创业版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }),4));
            blockInfoList.Add(new BlockInfo("沪市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange == ConstsExchange.EXCH_SSE)
                    {
                        return true;
                    }
                    return false;
                }), 4));
            blockInfoList.Add(new BlockInfo("深市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange == ConstsExchange.EXCH_SZE)
                    {
                        return true;
                    }
                    return false;
                }), 4));
            blockInfoList.Add(new BlockInfo("基金", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "4")
                    {
                        return true;
                    }
                    return false;
                }), 4));
            blockInfoList.Add(new BlockInfo("指数", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "7")
                    {
                        return true;
                    }
                    return false;
                }), 4));
            blockInfoList.Add(new BlockInfo("债券", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "3")
                    {
                        return true;
                    }
                    return false;
                }), 4));
            blockInfoList.Add(new BlockInfo("三板", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }), 4));
        }


        public bool Connected { get { return _connected; } }
        bool _connected = false;
        bool _requestheartbeat = false;//请求心跳回复
        bool _recvheartbeat = false;//收到心跳回复

        //心跳相应是否正常 连接正常 并且 请求心跳与接收心跳一致(确定发送心跳回复请求后是否收到心跳回报)
        /// <summary>
        /// 心跳是否正常,需满足一下条件
        /// 1.处于连接状态
        /// 2.请求状态与接收状态一致
        /// </summary>
        public bool IsHeartbeatOk { get { return _connected && (_requestheartbeat == _recvheartbeat); } }




        public void Connect(string[] hosts, int port)
        {
            logger.Info(string.Format("Try to connect to server:{0} port:{1}", hosts[0], port));
            _hosts = hosts;
            _port = port;

            if (_connected)
            {
                logger.Warn("Server is already connected");
                return;
            }

            try
            {
                m_hSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_hSocket.Connect(_hosts[0], _port);

                if (m_hSocket.Connected)
                {
                    logger.Info("Connect to server success");
                    OnConnected();
                    MDService.EventHub.FireConnectedEvent();

                    _connected = true;
                }
                else
                {
                    logger.Info("Connect to server success");
                }
            }
            catch(Exception ex)
            {
                logger.Error("Connect error:"+ex.ToString());
                m_hSocket.Close();
                m_hSocket = null;
            }
        }


        public void Disconnect()
        {
            if (m_hSocket != null && m_hSocket.Connected)
            {
                m_hSocket.Shutdown(SocketShutdown.Both);
                m_hSocket.Disconnect(true);
                _connected = false;
                m_hSocket = null;
            }
            //停止接收线程
            StopRecv();
            OnDisconnectd();
            MDService.EventHub.FireDisconnectedEvent();
        }

        uint stkDate = 0;
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        public void Login(string user,string pass)
        {
            byte[] RecvBuffer = null;
            byte[] a = { 0xC, 0x2, 0x18, 0x93, 0x0, 0x1, 0x3, 0x0, 0x3, 0x0, 0xD, 0x0, 0x1 };
            if (Command(a, a.Length, ref RecvBuffer))
            {
                int i = 42;
                stkDate = (uint)TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);// RecvBr.ReadUInt32();
                logger.Info(string.Format("login success, date:{0}",stkDate));
                byte[] bb = { 0x0C, 0x03, 0x18, 0x99, 0x00, 0x01, 0x20, 0x00, 0x20, 0x00, 0xDB, 0x0F, 0x74, 0x64, 0x78, 0x6C, 0x65, 0x76, 0x65, 0x6C, 0x32, 0x00, 0x00, 0x29, 0x5C, 0xE7, 0x40, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };
                if (Command(bb, bb.Length, ref RecvBuffer))
                {

                }
                MDLoginResponse respone = new MDLoginResponse();
                respone.LoginSuccess = true;
                respone.TradingDay = (int)stkDate;

                MDService.EventHub.FireLoginEvent(respone);

                //登入成功 基础数据未初始化则初始化基础数据
                if (!MDService.Initialized && respone.LoginSuccess)
                {
                    //初始化数据查询
                    InitBasicData();
                    //启动后台维护线程
                    StartWatchDog();
                    //调用初始化完毕 该操作修改相关状态并对外出发初始化完毕事件
                    MDService.Initialize();
                }

                _reconnectreq = false;//注通过Mod重新建立连接的过程中,连接线程会停止在 TLFound过程中，会一直等待服务器返回服务名
                _recvheartbeat = true;
                _requestheartbeat = true;
                //_connect = true;//连接建立标识
                //_connected = true;
                //启动后台数据接收线程
                StartRecv();
            }
            else
            {
                MDLoginResponse respone = new MDLoginResponse();
                respone.LoginSuccess = false;
                respone.ErrorMessage = "登入失败";
                MDService.EventHub.FireLoginEvent(respone);
                m_hSocket.Close();
                m_hSocket = null;
            }
        }

        




        static ManualResetEvent _processWaiting = new ManualResetEvent(false);

        private void NewRequest( SendBuf request)
        {
            SendList.Enqueue(request);
            if ((mainthread != null) && (mainthread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
            {
                //logger.Info("reset signal");
                _processWaiting.Set();
            }
        }




    }
}
