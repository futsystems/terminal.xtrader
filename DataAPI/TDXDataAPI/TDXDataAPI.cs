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

        private Queue SendList = new Queue(); //接收到的数据表


        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static void ConnectCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket s = (Socket)ar.AsyncState;
            try
            {
                s.EndConnect(ar);
            }
            catch
            {

            }

        }

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
        }


        public bool Connected { get { return _connected; } }
        bool _connected = false;
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
        public void Login(string user,string pass)
        {
            byte[] RecvBuffer = null;
            byte[] a = { 0xC, 0x2, 0x18, 0x93, 0x0, 0x1, 0x3, 0x0, 0x3, 0x0, 0xD, 0x0, 0x1 };
            if (Command(a, a.Length, ref RecvBuffer))
            {
                
                //FConnect.Text = "已连接";
                //FConnect.ForeColor = Color.Green;
                int i = 42;
                stkDate = (uint)TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);// RecvBr.ReadUInt32();
                //LinkServer.Enabled = false;
                //CloseLink.Enabled = true;
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
                    this.InitSymbol();
                    //this.InitFinance();
                    //this.InitPower();
                    //调用初始化完毕 该操作修改相关状态并对外出发初始化完毕事件
                    MDService.Initialize();
                }
                
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
                //FConnect.Text = "登录失败";
                //FConnect.ForeColor = Color.Red;
                //return false;
            }
        }

        List<MDSymbol> symbolList = new List<MDSymbol>();
        Dictionary<string, MDSymbol> symbolMap = new Dictionary<string, MDSymbol>();

        /// <summary>
        /// 所有合约
        /// </summary>
        public IEnumerable<MDSymbol> Symbols { get { return symbolMap.Values; } }

        #region 初始化操作
        /// <summary>
        /// 初始化合约信息
        /// </summary>
        void InitSymbol()
        {

            int i,count,n,j;
            MDService.EventHub.FireInitializeStatusEvent("深圳代码初始化");
            ConvertHzToPz_Gb2312 htp = new ConvertHzToPz_Gb2312();
            byte[] a1 = { 0xC, 0xC, 0x18, 0x6C, 0x0, 0x1, 0x8, 0x0, 0x8, 0x0, 0x4E, 0x4, 0x0, 0x0, 0x1, 0x2, 0x3, 0x4 };
            byte[] b1;
            b1 = BitConverter.GetBytes(stkDate);
            a1[14] = b1[0];
            a1[15] = b1[1];
            a1[16] = b1[2];
            a1[17] = b1[3];
            byte[] RecvBuffer = null;
            if (Command(a1, a1.Length, ref RecvBuffer))
            {
                //count = RecvBr.ReadUInt16();
                i = 0;
                count = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);// RecvBr.ReadUInt16();
                i = 0;
                //FlashWindow.JinDu.Minimum = 0;
                //FlashWindow.JinDu.Maximum = count;
                logger.Info(string.Format("ShengZheng Market have {0} stocks", count));
                while (i < count)
                {
                    //合约数据单个单个查询
                    byte[] a2 = { 0xC, 0x1, 0x18, 0x64, 0x1, 0x1, 0x6, 0x0, 0x6, 0x0, 0x50, 0x4, 0x0, 0x0, 0xF2, 0xF3 };
                    byte[] b2;
                    b2 = BitConverter.GetBytes(i);
                    a2[14] = b2[0];
                    a2[15] = b2[1];
                    if (Command(a2, a2.Length, ref RecvBuffer))
                    {
                        int ii = 0;
                        n = TDXDecoder.TDXGetInt16(RecvBuffer, ii, ref ii);// RecvBr.ReadUInt16();
                        int pp = 2;
                        TGPNAME gname = new TGPNAME();
                        Type type = gname.GetType();
                        int size = Marshal.SizeOf(gname.GetType());
                      
                        for (j = 0; j < n; j++)
                        {
                            MDSymbol symbol = new MDSymbol();
                            gname = (TGPNAME)TDX.TDXDecoder.BytesToStuct(RecvBuffer, pp,type);

                            symbol = new MDSymbol();

                            symbol.Name = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.name);
                            symbol.Symbol = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.code);
                            symbol.Key = htp.Convert(symbol.Name);
                            symbol.NCode = TDX.TDXDecoder.EnCodeMark(symbol.Symbol, 0);
                            symbol.Exchange = Exchange.EXCH_SZE;
                            symbol.BlockType = TDXDecoder.GetStockType(0, symbol.Symbol).ToString();
                            symbol.PreClose = gname.YClose;

                            symbolMap[symbol.UniqueKey] = symbol;
                            symbolList.Add(symbol);

                            pp = pp + Marshal.SizeOf(type);
                            //logger.Info(string.Format("ID:{0} Symbol:{1} Name:{2}", ncode, codes, names));
                        }
                        i = i + n;
                    }
                    else
                        break;
                }
            }

            MDService.EventHub.FireInitializeStatusEvent("上海代码初始化");
            byte[] a3 = { 0xC, 0xC, 0x18, 0x6C, 0x0, 0x1, 0x8, 0x0, 0x8, 0x0, 0x4E, 0x4, 0x1, 0x0, 0x1, 0x2, 0x3, 0x4 };
            byte[] b3 = BitConverter.GetBytes(stkDate);
            a3[14] = b3[0];
            a3[15] = b3[1];
            a3[16] = b3[2];
            a3[17] = b3[3];
            if (Command(a3, a3.Length, ref RecvBuffer))
            {
                i = 0;
                count = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);// RecvBr.ReadUInt16();
                i = 0;
                while (i < count)
                {
                    byte[] a2 = { 0xC, 0x1, 0x18, 0x64, 0x1, 0x1, 0x6, 0x0, 0x6, 0x0, 0x50, 0x4, 0x1, 0x0, 0xF2, 0xF3 };
                    byte[] b2;
                    b2 = BitConverter.GetBytes(i);
                    a2[14] = b2[0];
                    a2[15] = b2[1];
                    if (Command(a2, a2.Length, ref RecvBuffer))
                    {
                        int ii = 0;
                        n = TDXDecoder.TDXGetInt16(RecvBuffer, ii, ref ii);// RecvBr.ReadUInt16();
                        int pp = 2;
                        TGPNAME gname = new TGPNAME();
                        Type type = gname.GetType();
                        int size = Marshal.SizeOf(gname.GetType());

                        for (j = 0; j < n; j++)
                        {
                            MDSymbol symbol = new MDSymbol();
                            gname = (TGPNAME)TDX.TDXDecoder.BytesToStuct(RecvBuffer, pp, type);

                            symbol = new MDSymbol();

                            symbol.Name = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.name);
                            symbol.Symbol = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.code);
                            symbol.Key = htp.Convert(symbol.Name);
                            symbol.NCode = TDX.TDXDecoder.EnCodeMark(symbol.Symbol, 0);
                            symbol.Exchange = Exchange.EXCH_SSE;
                            symbol.BlockType = TDXDecoder.GetStockType(1, symbol.Symbol).ToString();
                            symbol.PreClose = gname.YClose;
                            symbolMap[symbol.UniqueKey] = symbol;
                            symbolList.Add(symbol);
                            pp = pp + Marshal.SizeOf(type);
                            //logger.Info(string.Format("ID:{0} Symbol:{1} Name:{2} PriceMag:{3} Rate:{4} YClose:{5}", ncode, codes, names, gname.PriceMag, gname.rate, gname.YClose));
                        }
                        i = i + n;
                    }
                    else
                        break;
                }
            }

           
        }

        /// <summary>
        /// 初始化财务信息
        /// </summary>
        void InitFinance()
        {
            string codes;
            byte[] code = new byte[6];
            byte[] request = new byte[2000];
            int Len, i, t, n, k, kk;
            string uniqueKey;
            int j;
            Len = 100;
            byte[] bb = { 0xC, 0x1F, 0x18, 0x75, 0x0, 0x1, 0xB, 0x0, 0xB, 0x0, 0x10, 0x0, 0x0, 0x0 };
            bb.CopyTo(request, 0);
            i = 100;
            MDSymbol target = null;
            MDService.EventHub.FireInitializeStatusEvent("初始化财务信息");
            int TDXBKLen = 0;
            while (i < symbolMap.Count - TDXBKLen)
            {
                j = bb.Length;
                Len = Math.Min(100, symbolMap.Count - i - TDXBKLen);
                for (t = i; t < i + Len; t++)
                {
                    request[j] = (byte)GetMarketCode(symbolList[t].Exchange);//FSK[t].mark;
                    Encoding.GetEncoding("GB2312").GetBytes(symbolList[t].Symbol).CopyTo(request, j + 1);
                    j = j + 7;
                }
                byte[] b1;
                ushort jj = (ushort)(j - 10);
                b1 = BitConverter.GetBytes(jj);
                b1.CopyTo(request, 6);
                b1.CopyTo(request, 8);
                request[12] = (byte)Len;
                byte[] RecvBuffer = null;
                MDService.EventHub.FireInitializeStatusEvent("初始化财务信息:" + ((double)i / (double)symbolMap.Count).ToString("0%"));
                if (Command(request, j, ref RecvBuffer))
                {
                    MemoryStream ms = new MemoryStream(RecvBuffer);
                    BinaryReader RecvBr = new BinaryReader(ms);
                    n = RecvBr.ReadInt16();
                    if (n > 0)
                    {
                        for (j = 0; j < n; j++)
                        {
                            byte m = RecvBr.ReadByte();
                            code = RecvBr.ReadBytes(6);
                            codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                            uniqueKey = string.Format("{0}-{1}", GetMarketString((int)m), codes);
                            //查找到对应的Symbol并赋值财务数据
                            if (symbolMap.TryGetValue(uniqueKey, out target))
                            {
                                target.FinanceData.LTG = RecvBr.ReadSingle();
                                target.FinanceData.t1 = RecvBr.ReadUInt16();
                                target.FinanceData.t2 = RecvBr.ReadUInt16();
                                target.FinanceData.day1 = RecvBr.ReadUInt32();
                                target.FinanceData.day2 = RecvBr.ReadUInt32();
                                target.FinanceData.zl = new float[30];
                                for (k = 0; k < 30; k++)
                                {
                                    target.FinanceData.zl[k] = RecvBr.ReadSingle();
                                }
                            }
                        }
                    }
                    RecvBr.Close();
                    ms.Close();
                }
                else
                {
                    break;
                }
                i = i + Len;

            }
            MDService.EventHub.FireInitializeStatusEvent("财务初始化完成");
        }

        /// <summary>
        /// 初始化话除权数据
        /// </summary>
        void InitPower()
        {
            string codes;
            byte[] code = new byte[6];
            byte[] request = new byte[2000];
            int Len, i, t, n, k, ii, tt;
            string uniqueKey;
            int j;
            Len = 100;
            byte[] bb = { 0xC, 0x1F, 0x18, 0x75, 0x0, 0x1, 0xB, 0x0, 0xB, 0x0, 0xF, 0x0, 0x0, 0x0 };
            bb.CopyTo(request, 0);
            i = 100;
            int TDXBKLen = 0;
            MDSymbol target = null;
            MDService.EventHub.FireInitializeStatusEvent("初始化权息信息");
            while (i < symbolMap.Count - TDXBKLen)
            {
                j = bb.Length;
                Len = Math.Min(100, symbolMap.Count - i - TDXBKLen);
                for (t = i; t < i + Len; t++)
                {
                    request[j] = (byte)GetMarketCode(symbolList[t].Exchange);
                    Encoding.GetEncoding("GB2312").GetBytes(symbolList[t].Symbol).CopyTo(request, j + 1);
                    j = j + 7;
                }
                byte[] b1;
                ushort jj = (ushort)(j - 10);
                b1 = BitConverter.GetBytes(jj);
                b1.CopyTo(request, 6);
                b1.CopyTo(request, 8);
                request[12] = (byte)Len;
                byte[] RecvBuffer = null;
                MDService.EventHub.FireInitializeStatusEvent("初始化权息信息:" + ((double)i / (double)symbolMap.Count).ToString("0%"));
                if (Command(request, j, ref RecvBuffer))
                {
                    ii = 0;
                    n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, ii, ref ii);// RecvBr.ReadUInt16();
                    if (n > 0)
                    {
                        ii = 2;
                        int num5;
                        for (j = 0; j < n; j++)
                        {
                            byte m = RecvBuffer[ii];
                            Array.Copy(RecvBuffer, ii + 1, code, 0, 6);
                            codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);

                            uniqueKey = string.Format("{0}-{1}", GetMarketString((int)m), codes);
                            //查找到对应的Symbol并赋值财务数据
                            if (symbolMap.TryGetValue(uniqueKey, out target))
                            {

                                //CStock.Stock sk = (CStock.Stock)FSH[TDX.TDXDecoder.EnCodeMark(codes, m)];
                                //if (sk == null)
                                //    break;
                                if (target.PowerData.quan == null)
                                {
                                    target.PowerData.quan = new PowerItem[80];
                                }
                                //if (sk.qu.quan == null)
                                //    sk.qu.quan = new CStock.QuanInfo[80];
                                ii = ii + 7;
                                k = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, ii, ref ii);
                                if (k == 0)
                                    continue;
                                tt = 0;
                                for (t = 0; t < k; t++)
                                {
                                    m = RecvBuffer[ii];
                                    num5 = ii + 8;
                                    int date1 = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, num5, ref  num5);
                                    byte t1 = RecvBuffer[ii + 12];

                                    if (t1 == 1)
                                    {
                                        target.PowerData.quan[tt].Date = (uint)date1;
                                        target.PowerData.quan[tt].style = t1;
                                        num5 = ii + 13;
                                        target.PowerData.quan[tt].Money = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, num5, ref num5);
                                        target.PowerData.quan[tt].PeiMoney = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, num5, ref  num5);
                                        target.PowerData.quan[tt].Number = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, num5, ref  num5);
                                        target.PowerData.quan[tt].PeiNumber = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, num5, ref  num5);
                                        tt = tt + 1;
                                    }
                                    target.PowerData.QuanLen = tt;
                                    ii = ii + 29;
                                }
                            }

                        }
                    }
                }
                else
                {
                    break;
                }
                i = i + Len;
            }
            MDService.EventHub.FireInitializeStatusEvent("权息初始化完成");
        }
        #endregion



        #region 辅助函数

        int GetMarketCode(string exchange)
        {
            switch (exchange)
            { 
                case Exchange.EXCH_SSE:
                    return 1;
                case Exchange.EXCH_SZE:
                    return 0;
                default:
                    return -1;
            }
        }

        string GetMarketString(int market)
        {
            if (market == 0)
            {
                return Exchange.EXCH_SZE;
            }
            if (market == 1)
            {
                return Exchange.EXCH_SSE;
            }
            return string.Empty;
        }
        /// <summary>
        /// K线种类, 0->5分钟K线    1->15分钟K线    2->30分钟K线  3->1小时K线    4->日K线  5->周K线  6->月K线  7->1分钟    10->季K线  11->年K线</param>
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        int GetFreqCode(string freq)
        {
            switch (freq)
            {
                case ConstFreq.Freq_Day: return 4;
                case ConstFreq.Freq_Week: return 5;
                case ConstFreq.Freq_Month: return 6;
                case ConstFreq.Freq_Quarter: return 10;
                case ConstFreq.Freq_Year: return 11;
                case ConstFreq.Freq_M1: return 7;
                case ConstFreq.Freq_M5: return 0;
                case ConstFreq.Freq_M15: return 1;
                case ConstFreq.Freq_M30: return 2;
                case ConstFreq.Freq_M60: return 3;
                default:
                    return -1;

            }
        }

        #endregion



        int gptime(byte mark, int v)
        {
            int i, h, m;
            i = v;
            if ((mark == 0) || (mark == 1))
            {
                i = v + 9 * 60 + 30;
                if (v > 120)
                {
                    i = i + 90;
                }
            }
            if ((mark == 0x41) || (mark == 0x42) || (mark == 0x43))
            {
                i = v + 9 * 60;
                if (v > 75)
                {
                    i = i + 15;
                };
                if (v > 135)
                {
                    i = i + 120;
                }
            }
            if (mark == 0x47)
            {
                i = v + 9 * 60 + 15;
                if (v > 135)
                {
                    i = i + 90;
                }
            }
            h = i / 60;
            m = i % 60;
            return h * 100 + m;
        }


        /// <summary>
        /// 向服务端提交一个请求
        /// 服务端对应给出一个返回
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Len"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool Command(byte[] request, int Len, ref byte[] response)
        {
            busying = true;
            bool rt = false;
            if (Senddata(request, Len))
                rt = RecvData(ref response);
            busying = false;
            return rt;
        }

        /// <summary>
        /// 向服务端发送数据
        /// </summary>
        /// <param name="v"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public bool Senddata(byte[] v, int Len)
        {
            if (m_hSocket == null)
            {
                logger.Error("Server is not connected");
                return false;
            }

            try
            {
                m_hSocket.Send(v, Len, SocketFlags.None);
                return true;
            }
            catch(Exception ex)
            {
                logger.Error("SendData error:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="recvbuf"></param>
        /// <returns></returns>
        public bool RecvData(ref byte[] recvbuf)
        {
            if (m_hSocket == null)
            {
                logger.Error("Server is not connected");
                return false;
            }

            byte[] DHeader = new byte[16];
            recvbuf = null;
            string s1;
            int Len;
            try
            {
                Len = m_hSocket.Receive(DHeader, 16, SocketFlags.None);
                if (Len != 16)
                {
                    return false;
                }
                RecvDataHeader rhd = new RecvDataHeader();
                rhd = (RecvDataHeader)TDX.TDXDecoder.BytesToStuct(DHeader, 0, rhd.GetType());
                if (rhd.CheckSum != 7654321)
                {
                    return false;
                }

                byte[] dbuf = new byte[rhd.Size];
                int elen = rhd.Size;
                int fcur = 0;
                int Len1, min1 = 1024;
                while (fcur < elen)
                {
                    min1 = Math.Min(1024, elen - fcur);
                    Len1 = m_hSocket.Receive(dbuf, fcur, min1, SocketFlags.None);
                    if (Len1 > 0)
                        fcur += Len1;
                }
                if (fcur != elen)
                {
                    return false;
                }
                recvbuf = new byte[rhd.DePackSize + 1];
                if ((rhd.EncodeMode & 0x10) == 0x10)
                {
                    int LL = TDX.TDXDecoder.Decompress(dbuf, rhd.DePackSize, ref recvbuf);
                    if (LL != rhd.DePackSize)
                    {
                        //SaveRecvData(recvbuf);
                        s1 = "解压出错:长度不同=depacksize=" + rhd.DePackSize.ToString() + " 解压长度:=" + LL.ToString();
                        logger.Error(s1);
                    }
                }
                else
                {
                    dbuf.CopyTo(recvbuf, 0);
                }
                int t = 0;
                switch (rhd.msgid)
                {
                    case 0x526:
                    case 0x527: t = 0x39; break;
                    case 0x551: t = 0x49; break;
                    case 0x556: t = 0x69; break;
                    case 0x56e:
                    case 0x573: t = 0x77; break;
                }
                if (t > 0)
                {
                    for (int i = 0; i < rhd.DePackSize; i++)
                        recvbuf[i] = (byte)(recvbuf[i] ^ t);
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.Error("Recv Data Error:" + ex.ToString());
            }
            busying = false;
            return false;
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
