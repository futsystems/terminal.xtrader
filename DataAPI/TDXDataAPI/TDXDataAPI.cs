using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;

using Common.Logging;

using TradingLib.MarketData;

namespace DataAPI.TDX
{
    public class TDXDataAPI:IMarketDataAPI
    {
        ILog logger = LogManager.GetLogger("TDXDataAPI");


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
        public event Action<PriceVolPair, RspInfo, int, bool> OnRspQryPriceVolPair;

        
        /// <summary>
        /// 查询当日分笔数据回报事件
        /// </summary>
        public event Action<TradeSplit, RspInfo, int, bool> OnRspQryTradeSplit;

        /// <summary>
        /// 查询历史分笔数据回报事件
        /// </summary>
        public event Action<TradeSplit, RspInfo, int, bool> OnRspQryHistTradeSplit;


        /// <summary>
        /// 查询Bar数据回报事件
        /// </summary>
        //public event Action<double[][], RspInfo, int> OnRspQrySecurityBar;


        /// <summary>
        /// 分时数据回报事件
        /// </summary>
        public event Action<double[][], RspInfo, int,int> OnRspQryMinuteData;

        /// <summary>
        /// 历史分时数据回报
        /// </summary>
        public event Action<double[][], RspInfo, int, int> OnRspQryHistMinuteData;




        /// <summary>
        /// 查询Bar数据响应
        /// </summary>
        public event Action<Dictionary<string, double[]>, RspInfo, int, int> OnRspQrySecurityBar;


        Thread mainthread = null;
        Socket m_hSocket = null;
        bool keepalive = false;
        bool busying = false;

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

        public void Connect(string[] hosts, int port)
        {
            _hosts = hosts;
            _port = port;

            if (m_hSocket != null)
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
                    this.InitData();
                }

                if (mainthread == null)
                {
                    keepalive = true;
                    mainthread = new Thread(ReceiveChat);
                    mainthread.IsBackground = true;
                    mainthread.Start();
                }
                //return true;
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

        Dictionary<string, MDSymbol> symbolMap = new Dictionary<string, MDSymbol>();

        /// <summary>
        /// 所有合约
        /// </summary>
        public IEnumerable<MDSymbol> Symbols { get { return symbolMap.Values; } }


        void InitData()
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
                            symbol.Exchange = "SZ";
                            symbol.BlockType = TDXDecoder.GetStockType(0, symbol.Symbol).ToString();
                            symbolMap[symbol.UniqueKey] = symbol;

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
                            symbol.Exchange = "SH";
                            symbol.BlockType = TDXDecoder.GetStockType(1, symbol.Symbol).ToString();
                            symbolMap[symbol.UniqueKey] = symbol;

                            pp = pp + Marshal.SizeOf(type);
                            //logger.Info(string.Format("ID:{0} Symbol:{1} Name:{2} PriceMag:{3} Rate:{4} YClose:{5}", ncode, codes, names, gname.PriceMag, gname.rate, gname.YClose));
                        }
                        i = i + n;
                    }
                    else
                        break;
                }
            }

            //调用初始化完毕 该操作修改相关状态并对外出发初始化完毕事件
            MDService.Initialize();
        }

        /// <summary>
        /// 查询当日成交分布数据
        /// </summary>
        /// <param name="market"></param>bb
        /// <param name="symbol"></param>
        public int QryPriceVol(int market, string symbol)
        {
            byte[] bb = { 0x0C, 0x25, 0x08, 0x00, 0x03, 0x01, 0x0A, 0x00, 0x0A, 0x00, 0x1A, 0x05, 0x00, 0x00, 0x30, 0x30, 0x30, 0x30, 0x30, 0x32 };
            bb[13] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(bb, 14);

            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = bb;
            sb11.RequestId = this.NextRequestId;
            SendList.Enqueue(sb11);
            return sb11.RequestId;

        }

        /// <summary>
        /// 查询当日分笔数据
        /// </summary>
        /// <param name="market">0 深证 1 上海</param>
        /// <param name="symbol">股票代码</param>
        /// <param name="start">起始位置 最后一条为0 前一条为1，以此类推</param>
        /// <param name="Count">请求记录数量</param>
        public int QryTransactionData(int market,string symbol, int start, int Count)
        {
            byte[] request = { 0x0C, 0x24, 0x08, 0x00, 0x03, 0x01, 0x0E, 0x00, 0x0E, 0x00, 0xC5, 0x0F, 0x00, 0x00, 0x30, 0x30, 0x30, 0x30, 0x30, 0x32, 0x00, 0x00, 0x14, 0x00 };
            request[13] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);
            
            byte[] sb = BitConverter.GetBytes(start);
            request[20] = sb[0];
            request[21] = sb[1];
            byte[] sb1 = BitConverter.GetBytes(Count);
            request[22] = sb1[0];
            request[23] = sb1[1];

            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = request;
            sb11.type = 1;

            sb11.Code = symbol;
            sb11.Market = (byte)(ushort)market;
            sb11.RequestId = this.NextRequestId;
            SendList.Enqueue(sb11);

            return sb11.RequestId;
        }

        /// <summary>
        /// 查询历史分笔数据
        /// </summary>
        /// <param name="market">市场代码</param>
        /// <param name="symbol">股票代码</param>
        /// <param name="date">请求日期</param>
        /// <param name="start">起始位置</param>
        /// <param name="Count">请求记录数量</param>
        public int QryHistTransactionData(int market, string symbol, int date, int start, int Count)
        {
            byte[] request = { 0xC, 0x1, 0x30, 0x0, 0x1, 0x1, 0x12, 0x0, 0x12, 0x0, 0xB5, 0xF, 0xD1, 0xD2, 0xD3, 0xD4, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0xF1, 0xF2, 0xE1, 0xE2 };
            byte[] bdate = BitConverter.GetBytes(date);
            //设置日期
            request[12] = bdate[0];
            request[13] = bdate[1];
            request[14] = bdate[2];
            request[15] = bdate[3];
            //设置市场类别
            request[16] = (byte)(ushort)market;
            //设置股票代码
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 18);
            //设置起始位置
            byte[] bstart = BitConverter.GetBytes(start);
            request[24] = bstart[0];
            request[25] = bstart[1];
            byte[] bcount = BitConverter.GetBytes(Count);
            request[26] = bcount[0];
            request[27] = bcount[1];

            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = request;
            sb11.type = 1;

            sb11.Code = symbol;
            sb11.Market = (byte)(ushort)market;
            sb11.RequestId = this.NextRequestId;
            SendList.Enqueue(sb11);
            return sb11.RequestId;
        }


        /// <summary>
        /// 查询当日分时数据
        /// </summary>
        /// <param name="market"></param>
        /// <param name="symbol"></param>
        public int QryMinuteDate(int market, string symbol)
        {
            byte[] request = { 0xC, 0x1B, 0x8, 0x0, 0x1, 0x1, 0xE, 0x0, 0xE, 0x0, 0x1D, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x0, 0x0, 0x0, 0x0 };
            request[12] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);

            SendBuf sb = new SendBuf();
            //sb.sk = null;
            sb.Send = request;
            sb.Code = symbol;
            sb.Market = (byte)(ushort)market;
            sb.RequestId = this.NextRequestId;
            SendList.Enqueue(sb);
            return sb.RequestId;
        }

        /// <summary>
        /// 查询历史分时
        /// </summary>
        /// <param name="market"></param>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        public int QryHistMinuteDate(int market, string symbol, int date)
        {
            byte[] request = { 0xC, 0x1, 0x30, 0x0, 0x1, 0x1, 0xD, 0x0, 0xD, 0x0, 0xB4, 0xF, 0xD1, 0xD2, 0xD3, 0xD4, 0xFF, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36 };
            byte[] sb = BitConverter.GetBytes(date);
            request[12] = sb[0];
            request[13] = sb[1];
            request[14] = sb[2];
            request[15] = sb[3];
            request[16] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 17);
            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = request;
            sb11.Code = symbol;
            sb11.Market = (byte)(ushort)market;
            //sb11.type = Convert.ToInt32(e.Date);
            sb11.RequestId = this.NextRequestId;
            SendList.Enqueue(sb11);
            return sb11.RequestId;
        }

        //public int QryHistSecurityBars(int market, string symbol, int freq)
        //{
        //    byte[] request = { 0xC, 0x1, 0x8, 0x64, 0x1, 0x1, 0x12, 0x0, 0x12, 0x0, 0x29, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0xFE, 0x0, 0x1, 0x0, 0x00, 0x00, 0x0a, 0x0 };
        //    request[12] = (byte)(ushort)market;
        //    request[20] = (byte)freq;
        //    Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);

        //    SendBuf sb11 = new SendBuf();
        //    //sb11.sk = null;
        //    sb11.Send = request;
        //    sb11.Code = symbol;
        //    sb11.Market = (byte)(ushort)market;
        //    sb11.RequestId = this.NextRequestId;
        //    SendList.Enqueue(sb11);
        //    return sb11.RequestId;
          
        //}
        /// <summary>
        /// 查询Bar数据
        /// </summary>
        /// <param name="market">0 深证 1 上海</param>
        /// <param name="symbol">股票代码</param>
        /// <param name="freq">K线种类, 0->5分钟K线    1->15分钟K线    2->30分钟K线  3->1小时K线    4->日K线  5->周K线  6->月K线  7->1分钟    10->季K线  11->年K线</param>
        /// <param name="start">K线开始位置,最后一条K线位置是0, 前一条是1, 依此类推</param>
        /// <param name="count">API执行前,表示用户要请求的K线数目, API执行后,保存了实际返回的K线数目, 最大值800</param>
        public int QrySeurityBars(int market, string symbol, int freq, int start, int count)
        {
                           //{ 0xC, 0x1, 0x8, 0x64, 0x1, 0x1, 0x12, 0x0, 0x12, 0x0, 0x29, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0xFE, 0x0, 0x1, 0x0, 0x00, 0x00, 0x0a, 0x0 };
            byte[] request = { 0xC, 0x1, 0x8, 0x64, 0x1, 0x1, 0x12, 0x0, 0x12, 0x0, 0x29, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0xFE, 0x0, 0x1, 0x0, 0xF1, 0xF2, 0xE1, 0xE2 };
            
            request[12] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);
            request[20] = (byte)freq;

            byte[] sb = BitConverter.GetBytes(start);
            request[24] = sb[0];
            request[25] = sb[1];
            byte[] sb1 = BitConverter.GetBytes(count);
            request[26] = sb1[0];
            request[27] = sb1[1];
            

            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = request;
            sb11.Code = symbol;
            sb11.Market = (byte)(ushort)market;
            sb11.RequestId = this.NextRequestId;
            SendList.Enqueue(sb11);
            return sb11.RequestId;
        }

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


        private void ReceiveChat()
        {
            logger.Info("Start Main Process Loop");
            byte[] DataHeader = new byte[16];
            SendBuf sb = null;
            RecvDataHeader head = new RecvDataHeader();
            IAsyncResult sr = null;
            while (keepalive)
            {
                try
                {

                    if (m_hSocket == null)
                        break; ;
                    if (busying)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    sb = null;
                    if (m_hSocket.Available == 0)
                    {
                        //发送请求
                        if (SendList.Count > 0)
                        {
                            sb = (SendBuf)SendList.Dequeue();
                            m_hSocket.Send(sb.Send, sb.Send.Length, SocketFlags.None);
                            Thread.Sleep(10);
                        }
                        else
                        {

                            Thread.Sleep(10);
                            continue;
                        }
                    }
                    //获取消息头
                    int Len = m_hSocket.Receive(DataHeader, 16, SocketFlags.None);
                    if (Len != 16)
                    {
                        m_hSocket.Close();
                        m_hSocket = null;
                        break;
                    }
                    head = (RecvDataHeader)TDX.TDXDecoder.BytesToStuct(DataHeader, 0, head.GetType());
                    if (head.CheckSum != 7654321)
                        continue;

                    //根据消息头中的size获得对应长度的服务端传输过来的消息
                    byte[] buf = new byte[head.Size];
                    int elen = head.Size;
                    int fcur = 0;
                    int Len1, min1 = 1024;
                    while (fcur < elen)
                    {
                        min1 = Math.Min(1024, elen - fcur);
                        Len1 = m_hSocket.Receive(buf, fcur, min1, SocketFlags.None);
                        if (Len1 > 0)
                            fcur += Len1;
                    }
                    if (fcur != elen)
                    {
                        m_hSocket.Close();
                        m_hSocket = null;
                        break;
                    }
                    byte[] recvbuf = new byte[head.DePackSize + 1];
                    if ((head.EncodeMode & 0x10) == 0x10)
                    {
                        int LL = TDX.TDXDecoder.Decompress(buf, head.DePackSize, ref recvbuf);
                        if (LL != head.DePackSize)
                        {
                            //SaveRecvData(recvbuf);
                            //s1 = "解压出错:长度不同=depacksize=" + hd.DePackSize.ToString() + " 解压长度:=" + LL.ToString();
                            //MessageBox.Show(s1);
                            //return false;
                        }
                    }
                    else
                        Array.Copy(buf, recvbuf, head.DePackSize);
                    int t = 0;
                    switch (head.msgid)
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
                        for (int i = 0; i < head.DePackSize; i++)
                            recvbuf[i] = (byte)(recvbuf[i] ^ t);
                    }
                    sb.hd = head;
                    sb.Buffer = recvbuf;
                    ProcessData(sb);
                    //sr = this.BeginInvoke(DataCuLi, sb);//处理数据
                    //this.EndInvoke(sr);
                    //String data = myDelegate.EndInvoke(result, null, null);
                    //while (doing)
                    //{
                    //    Thread.Sleep(30); 
                    //}
                    //this.BeginInvoke(DataCuLi, sb);
                }
                catch (Exception ex)
                {
                    keepalive = false;
                    //FConnect.Text = "线程内中断";
                    logger.Info("Process Loop Error:" + ex.ToString());
                    break;

                }
            }
            //this.BeginInvoke(ExitRecv);
        }

        public void ProcessData(DataAPI.TDX.SendBuf sb)
        {
            if (sb == null)
                return;
            try
            {
                byte[] RecvBuffer = sb.Buffer;
                string codes;
                byte[] code = new byte[6];
                byte[] name = new byte[8];
                string names;
                int i, t, n;
                int num4, num5, num6, num7, num8, num9 = 0, num10;
                //if (List1.Visible)
                //{
                //    if (List1.Items.Count > 30)
                //        List1.Items.Clear();
                //    List1.Items.Add(sb.hd.msgid.ToString("X04"));
                //}
                int time1;
                //CStock.Stock sk = null;
                switch (sb.hd.msgid)
                {

                    case 0x51a:
                        #region 当日价格分布数据处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info(string.Format("QryPriceVol Response Count:{0}", n));

                        if (n == 0)
                            return;
                        //GP.ClearJia();
                        i = 11;
                        for (int j = 0; j < 5; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 4;
                        for (int j = 0; j < 3; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 4;
                        for (int j = 0; j < 16; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 2;
                        num4 = 0;
                        for (int j = 0; j < n; j++)
                        {
                            num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            double v1 = (double)num4 / 100.0;
                            //logger.Info("PV {0} {1}".Put(v1, num5));
                            //GP.AddJia(v1, num5);
                            if (OnRspQryPriceVolPair != null)
                            {
                                OnRspQryPriceVolPair(new PriceVolPair(v1, num5), null, sb.RequestId, j == n - 1);
                            }
                        }
                        #endregion


                        break;
                    case 0xfb5://历史分笔
                        #region 历史分笔处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info(string.Format("QryHistTransactionData Response Count:{0}", n));
                        if (n > 0)
                        {
                            i = 6;
                            num4 = 0;
                            double close;
                            int vol, dealcount, sellorbuy;

                            //List<CStock.Tick> lt1 = new List<CStock.Tick>();
                            for (int j = 0; j < n; j++)
                            {
                                //CStock.Tick tk = new CStock.Tick();
                                time1 = TDX.TDXDecoder.TDXGetTime(RecvBuffer, i, ref i);
                                num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close = (double)((num4 + num5) / 100.0);
                                vol = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                dealcount = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //lt1.Add(tk);
                                num4 = num5 + num4;
                                i = i + 1;

                                TradeSplit split = new TradeSplit(time1, close, vol, 0, dealcount);
                                //logger.Info(split.ToString());
                            }
                            //HisTicks.InsertRange(0, lt1);
                        }
                        //if (n == 2000)
                        //    GetHisFenBiLine(sb.sk, HisDate, HisTicks.Count, 2000);
                        //if (n < 2000)
                        //{
                        //    HisList.Items.Clear();
                        //    HisList.BeginUpdate();
                        //    for (i = 0; i < HisTicks.Count; i++)
                        //    {
                        //        CStock.Tick tk = HisTicks[i];
                        //        string ss = string.Format("{0:D2}:{1:D2}  ", tk.time / 100, tk.time % 100);
                        //        ss += string.Format("{0:F2}  ", tk.value);
                        //        ss += tk.vol.ToString("D5") + "  ";// string.Format("{0:D}  ", tk.vol);
                        //        if (tk.tick == 1)
                        //            ss += "B ";
                        //        else if (tk.tick == 0)
                        //            ss += "S ";
                        //        else
                        //            ss += "  ";
                        //        HisList.Items.Add(ss);
                        //    }
                        //    HisList.EndUpdate();
                        //    HisList.SelectedIndex = HisTicks.Count - 1;
                        //    HisTicks.Clear();
                        //}
                        #endregion

                        break;
                    case 0xfc5://分笔
                        #region 当日分笔数据处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info(string.Format("QryTransactionData Response Count:{0}", n));
                        //不存在分笔数据
                        if (n == 0)
                        {
                            //Ticks.Clear();
                            if (OnRspQryTradeSplit != null)
                            {
                                OnRspQryTradeSplit(null, null, 0, true);
                            }
                            return;
                        }
                        //if (StockBoard.Visible && (GP.TabValue == 0) && (sb.type == 100))
                        {
                            //GP.FenBiList.Clear();
                            //GP.ClearFenbi();
                            double close;
                            int vol, dealcount, sellorbuy;
                            num4 = 0;
                            i = 2;
                            for (int j = 0; j < n; j++)
                            {
                                time1 = TDX.TDXDecoder.TDXGetTime(RecvBuffer, i, ref i);
                                num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close = (num4 + num5) / 100.0;
                                vol = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                dealcount = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                sellorbuy = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                num4 = num5 + num4;
                                i = i + 1;
                                //GP.AddTick(time1, close, vol, sellorbuy, dealcount);
                                TradeSplit split = new TradeSplit(time1, close, vol, sellorbuy, dealcount);
                                //logger.Info(split.ToString());
                                if (OnRspQryTradeSplit != null)
                                {
                                    OnRspQryTradeSplit(split, null, sb.RequestId, j == n - 1);
                                }
                            }
                        }
                        //sb.type为请求时候设定，用于标记返回数据更新的回路100更新分笔Tab 1则更新分笔图
                        //if (sb.type == 1)
                        //{
                        //    List<CStock.Tick> lt = new List<CStock.Tick>();
                        //    num4 = 0;
                        //    i = 2;
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        CStock.Tick tk = new CStock.Tick();

                        //        tk.time = TDX.TDXDecoder.TDXGetTime(RecvBuffer, i, ref i);
                        //        num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        tk.value = (num4 + num5) / 100.0;
                        //        tk.vol = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        tk.tickcount = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        tk.tick = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        num4 = num5 + num4;
                        //        i = i + 1;
                        //        lt.Add(tk);
                        //    }
                            
                        //    Ticks.InsertRange(0, lt);

                        //    if ((zq == 12) && (StockBoard.Visible))
                        //    {
                        //        int m = Ticks.Count;
                        //        time = new double[m + 1];
                        //        close = new double[m + 1];
                        //        vol = new double[m + 1];
                        //        for (i = 0; i < m; i++)
                        //        {
                        //            CStock.Tick tk = Ticks[i];
                        //            time[i] = tk.time;
                        //            close[i] = tk.value;
                        //            vol[i] = tk.vol;
                        //        }

                        //        GP.K_cleardata();
                        //        sk = sb.sk;
                        //        if (sk != null)
                        //        {
                        //            GP.PreClose = sk.GP.YClose;
                        //            if (sk.now.last != 0)
                        //                GP.PreClose = sk.now.last;
                        //            GP.StkCode = sk.codes;
                        //            GP.StkName = sk.names;
                        //            GP.StkWeek = WeekString[zq];
                        //        }
                        //        GP.AddAll("time", time, m, false);
                        //        GP.AddAll("close", close, m, false);
                        //        GP.AddAll("vol", vol, m, true);
                        //    }
                        //    if (TickBoard.Visible)
                        //    {
                        //        TickList.Items.Clear();
                        //        TickList.BeginUpdate();
                        //        for (i = 0; i < Ticks.Count; i++)
                        //        {
                        //            CStock.Tick tk = Ticks[i];
                        //            string ss = string.Format("{0:D2}:{1:D2}  ", tk.time / 100, tk.time % 100);
                        //            ss += string.Format("{0:F2}  ", tk.value);
                        //            ss += tk.vol.ToString("D5") + "  ";// string.Format("{0:D}  ", tk.vol);
                        //            if (tk.tick == 1)
                        //                ss += "B ";
                        //            else if (tk.tick == 0)
                        //                ss += "S ";
                        //            else
                        //                ss += "  ";
                        //            ss += tk.tickcount.ToString("D3");
                        //            TickList.Items.Add(ss);
                        //        }
                        //        TickList.SelectedIndex = Ticks.Count - 1;
                        //        TickList.EndUpdate();

                        //        TickName.Text = sb.sk.codes + "  " + sb.sk.names;

                        //    }
                        //    if (JiaBoard.Visible)
                        //    {
                        //        SortedList<double, int> sl = new SortedList<double, int>();
                        //        for (i = 0; i < Ticks.Count; i++)
                        //        {
                        //            CStock.Tick tk = Ticks[i];

                        //            if (sl.IndexOfKey(tk.value) > -1)
                        //                sl[tk.value] += tk.vol;
                        //            else
                        //                sl[tk.value] = tk.vol;
                        //        }
                        //        Bitmap bm = (Bitmap)JiaBox.Image;
                        //        if (bm != null)
                        //            bm.Dispose();
                        //        int hc = JiaBox.Height / 20;
                        //        int cc = sl.Count / hc + 1;
                        //        double preclose = sb.sk.GP.YClose;//昨收价
                        //        bm = new Bitmap(Math.Max(cc * 180, JiaBoard1.Width), JiaBox.Height);
                        //        JiaBox.Width = bm.Width;
                        //        Graphics cv = Graphics.FromImage(bm);
                        //        SizeF si;
                        //        cv.FillRectangle(Brushes.Black, 0, 0, bm.Width - 1, bm.Height - 1);
                        //        int maxvol = 0;
                        //        for (i = 0; i < sl.Count; i++)
                        //        {
                        //            if (sl.Values[i] > maxvol)
                        //                maxvol = sl.Values[i];
                        //        }

                        //        for (i = 0; i < sl.Count; i++)
                        //        {
                        //            int x = i / hc;
                        //            int y = i % hc;
                        //            double value = sl.Keys[i];
                        //            string s1 = value.ToString("F3");
                        //            float ww = (float)sl.Values[i] / (float)maxvol * 50;
                        //            //cv.FillRectangle(Brushes.White, x * 180 + 127, y * 20 + 3, ww, 14);
                        //            if (value > preclose)
                        //            {
                        //                cv.FillRectangle(Brushes.Red, x * 180 + 127, y * 20 + 3, ww, 14);
                        //                cv.DrawString(s1, Font, Brushes.Red, x * 180 + 4, y * 20 + 3);
                        //            }
                        //            if (value == preclose)
                        //            {
                        //                cv.FillRectangle(Brushes.White, x * 180 + 127, y * 20 + 3, ww, 14);
                        //                cv.DrawString(s1, Font, Brushes.White, x * 180 + 4, y * 20 + 3);
                        //            }
                        //            if (value < preclose)
                        //            {
                        //                cv.DrawString(s1, Font, Brushes.Lime, x * 180 + 4, y * 20 + 3);
                        //                cv.FillRectangle(Brushes.Aqua, x * 180 + 127, y * 20 + 3, ww, 14);
                        //            }
                        //            s1 = sl.Values[i].ToString();
                        //            si = cv.MeasureString(s1, Font);
                        //            cv.DrawString(s1, Font, Brushes.YellowGreen, x * 180 + 125 - si.Width, y * 20);
                        //        }
                        //        JiaBox.Image = bm;
                        //        JiaName.Text = sb.sk.codes + "  " + sb.sk.names;
                        //    }
                        //    if (n == 2000)
                        //        GetFenBiLine(sb.sk, Ticks.Count, 2000);
                        //    else
                        //        Ticks.Clear();

                        //}
                        #endregion

                        break;
                    case 0x526:
                        //SaveRecvData(RecvBuffer);
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        if (n == 0)
                            return;
                        //sk = null;
                        i = 2;
                        //for (int j = 0; j < n; j++)
                        //{
                        //    byte m = RecvBuffer[i];
                        //    Array.Copy(RecvBuffer, i + 1, code, 0, 6);
                        //    codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                        //    sk = (CStock.Stock)FSH[EnCodeMark(codes, m)];
                        //    if (sk == null)
                        //        break;
                        //    i = i + 9;
                        //    double prize = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i) / 100.0;
                        //    sk.now.prize = prize;
                        //    sk.now.last = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100.0);
                        //    if (sk.GP.YClose == 0.0)
                        //        sk.GP.YClose = (float)sk.now.last;
                        //    sk.now.open = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.high = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.low = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.Time = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.volume = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.tradeQTY = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);// '现量;
                        //    sk.now.amount = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                        //    sk.now.b = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.s = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                        //    sk.now.buy1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //    sk.now.BiCount = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);//逐笔 笔数
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);


                        //    sk.now.buy6 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell6 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy7 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell7 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy8 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell8 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy9 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell9 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY9 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY9 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy10 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell10 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buyall = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);//买均
                        //    sk.now.sellall = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100); ;//卖均
                        //    sk.now.buyQTYall = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);//总买
                        //    sk.now.sellQTYall = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);//总卖

                        //    if (sk == FCurStock)
                        //        GP.SetStock(sk);

                        //    StockJS(sk);
                        //}
                        //if (ListBoard.Visible)
                        //{
                        //    if (Block1.Visible)
                        //    {
                        //        Stklist.Refresh();

                        //        if ((FCurPage == -1) && (fsorttype > 0))
                        //        {
                        //            if (fsorttype == 1)
                        //            {
                        //                Stklist.BeginUpdate();
                        //                QuickSortHightoLow(Stklist.Items, 0, Stklist.Items.Count - 1, fsort - 3);
                        //                Stklist.EndUpdate();
                        //                Stklist.Invalidate();
                        //            }
                        //            if (fsorttype == 2)
                        //            {
                        //                Stklist.BeginUpdate();
                        //                QuickSortLowToHigh(Stklist.Items, 0, Stklist.Items.Count - 1, fsort - 3);
                        //                Stklist.EndUpdate();
                        //                Stklist.Invalidate();
                        //            }
                        //        }
                        //    }

                        //}
                        break;
                    case 0x53e:
                    case 0x53d:
                        i = 2;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        if (n == 0)
                            return;
                        i = 4;
                        int dd;
                        t = 0;
                        //for (int j = 0; j < n; j++)
                        //{
                        //    byte m = RecvBuffer[i];
                        //    Array.Copy(RecvBuffer, i + 1, code, 0, 6);
                        //    codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                        //    sk = (CStock.Stock)FSH[EnCodeMark(codes, m)];
                        //    if (sk == null)
                        //        break;
                        //    i = i + 7;
                        //    dd = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d1:" + dd.ToString());
                        //    double prize = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i) / 100.0;
                        //    sk.now.prize = prize;
                        //    sk.now.last = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100.0);
                        //    sk.now.open = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.high = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.low = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.Time = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d0:" + dd.ToString()); ;
                        //    sk.now.volume = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.tradeQTY = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);// '现量;
                        //    sk.now.amount = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                        //    sk.now.b = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.s = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d1:" + dd.ToString()); ;
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d2:" + dd.ToString()); ;

                        //    sk.now.buy1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref t);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, t, ref t);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, t, ref t);
                        //    i = i + 3;
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    double speed = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i) / 100;
                        //    TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);

                        //    if ((sb.hd.msgid == 0x53d) && (ListBoard.Visible) && (fsorttype > 0))
                        //    {
                        //        if (sb.type == 100)
                        //        {

                        //            Stklist.Items[Stklist.TopIndex + j] = sk;
                        //        }
                        //    }

                        //    if (sk == SHZS)
                        //    {
                        //        if (sk.now.Time > 14700000) //收盘
                        //        {
                        //            working = false;

                        //        }


                        //        shzs.Text = string.Format("{0:F2}  {1:F2}  {2:F2}亿", sk.now.prize, sk.now.prize - sk.now.last, sk.now.amount / 100000000);
                        //        if (sk.now.prize > sk.now.last)
                        //            shzs.ForeColor = Color.Red;
                        //        else
                        //            shzs.ForeColor = Color.Green;
                        //    }
                        //    if (sk == SZZS)
                        //    {
                        //        szzs.Text = string.Format("{0:F2}  {1:F2}  {2:F2}亿", sk.now.prize, sk.now.prize - sk.now.last, sk.now.amount / 100000000);
                        //        if (sk.now.prize > sk.now.last)
                        //            szzs.ForeColor = Color.Red;
                        //        else
                        //            szzs.ForeColor = Color.Green;
                        //    }

                        //    StockJS(sk);
                        //}
                        //if ((sb.hd.msgid == 0x53d) && (ListBoard.Visible))
                        //{
                        //    Stklist.Refresh();
                        //}
                        break;
                    case 0x51d://当日分时
                        #region 当日分时数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        logger.Info(string.Format("QryMinuteDate Response Count:{0}", n));
                        if (n == 0)
                        {
                            if (OnRspQryMinuteData != null)
                            {
                                double[][] tmp = new double[4][];
                                OnRspQryMinuteData(tmp, null, n, sb.RequestId);
                            }
                        }
                        if (n > 0)
                        {
                            i = 4;
                            num4 = 0;
                            double res;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] close = new double[n + 1];
                            double[] amount = new double[n + 1];
                            double[] vol = new double[n + 1];
                            for (int j = 0; j < n; j++)
                            {
                                //date[j] = StkDate;
                                time[j] = gptime(sb.Market, j);
                                num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (float)(num4 / 100.0);
                                res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //logger.Info("{0} {1} {2}".Put(time[j], close[j], vol[j]));
                            }

                            if (OnRspQryMinuteData != null)
                            { 
                                double[][] tmp = new double[4][];
                                tmp[0] = date;
                                tmp[1] = time;
                                tmp[2] = close;
                                tmp[3] = vol;
                                OnRspQryMinuteData(tmp, null, n, sb.RequestId);
                            }
                        }
                        //if (n > 0)
                        //{
                        //    i = 4;
                        //    num4 = 0;
                        //    double res;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        date[j] = StkDate;
                        //        time[j] = gptime(sb.sk.mark, j);
                        //        num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (float)(num4 / 100.0);
                        //        res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    }
                            //if (MutiBoard.Visible)
                            //{
                            //    sk = sb.sk;
                            //    for (i = 0; i < mgp.Length; i++)//遍历所有数据结构 如果stock对象相同则更新对应控件
                            //    {
                            //        if (msk[i] == sk)
                            //        {
                            //            mgp[i].BeginUpdate();
                            //            mgp[i].cleardata();
                            //            mgp[i].PreClose = sk.GP.YClose;
                            //            if (sk.now.last != 0)
                            //                mgp[i].PreClose = sk.now.last;
                            //            mgp[i].StkCode = sk.codes;
                            //            mgp[i].StkName = sk.names;
                            //            mgp[i].StkWeek = WeekString[zq];
                            //            mgp[i].EndUpdate();
                            //            mgp[i].FS_AddAll("date", date, n, false);
                            //            mgp[i].FS_AddAll("time", time, n, false);
                            //            mgp[i].FS_AddAll("vol", vol, n, false);
                            //            mgp[i].FS_AddAll("close", close, n, true);
                            //            break;
                            //        }
                            //    }
                            //}
                            //返回分时数据处理
                            //if ((sb.sk == FCurStock) && (StockBoard.Visible))
                            //{
                            //    GP.FS_cleardata();
                            //    sk = sb.sk;
                            //    if (sk != null)
                            //    {
                            //        GP.PreClose = sk.GP.YClose;
                            //        if (sk.now.last != 0)
                            //            GP.PreClose = sk.now.last;
                            //        GP.StkCode = sk.codes;
                            //        GP.StkName = sk.names;
                            //        GP.StkWeek = WeekString[zq];
                            //    }

                            //    if ((GP.Days > 1) && (DayTicks.Count > 0))
                            //    {

                            //        double[] date1 = new double[DayTicks.Count + n + 1];
                            //        double[] time11 = new double[DayTicks.Count + n + 1];
                            //        double[] close1 = new double[DayTicks.Count + n + 1];
                            //        double[] vol1 = new double[DayTicks.Count + n + 1];
                            //        for (int j = 0; j < DayTicks.Count; j++)
                            //        {
                            //            DayTick dt = DayTicks[j];
                            //            date1[j] = dt.date;
                            //            time11[j] = dt.time;
                            //            close1[j] = dt.close;
                            //            vol1[j] = dt.vol;
                            //        }
                            //        for (int j = 0; j < n; j++)
                            //        {
                            //            date1[DayTicks.Count + j] = date[j];
                            //            time11[DayTicks.Count + j] = time[j];
                            //            close1[DayTicks.Count + j] = close[j];
                            //            vol1[DayTicks.Count + j] = vol[j];
                            //        }
                            //        n = DayTicks.Count + n;
                            //        GP.FS_AddAll("date", date1, n, false);
                            //        GP.FS_AddAll("time", time11, n, false);
                            //        GP.FS_AddAll("vol", vol1, n, false);
                            //        GP.FS_AddAll("close", close1, n, true);
                            //    }
                            //    else
                            //    {
                            //        GP.FS_AddAll("date", date, n, false);
                            //        GP.FS_AddAll("time", time, n, false);
                            //        GP.FS_AddAll("vol", vol, n, false);
                            //        GP.FS_AddAll("close", close, n, true);
                            //    }
                            //}
                            //if ((StockBoard.Visible) && (GP.TabValue == 2))
                            //{
                            //    if ((sb.sk == SZZS) || (sb.sk == SHZS))
                            //    {
                            //        GP.zs_Clear();
                            //        GP.zs_SetName(sb.sk.names);
                            //        GP.zs_AddAll("date", date, n, false);
                            //        GP.zs_AddAll("time", time, n, false);
                            //        GP.zs_AddAll("vol", vol, n, false);
                            //        GP.zs_AddAll("close", close, n, true);
                            //    }
                            //}
                        //}
                        #endregion

                        break;
                    case 0xfb4://历史分时
                        #region 历史分时数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        logger.Info(string.Format("QryHistMinuteDate Response Count:{0}", n));

                        if (n > 0)
                        {
                            i = 6;
                            num4 = 0;
                            double res;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] close= new double[n + 1];
                            double[] amount= new double[n + 1];
                            double[] vol= new double[n + 1];
                            for (int j = 0; j < n; j++)
                            {
                                //Application.DoEvents();
                                //date[j] = sb.type;
                                time[j] = gptime(sb.Market, j);
                                num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (double)(num4 / 100.0);
                                res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //logger.Info("{0} {1} {2}".Put(time[j], close[j], vol[j]));
                            }

                            if (OnRspQryHistMinuteData != null)
                            {
                                double[][] tmp = new double[4][];
                                tmp[0] = date;
                                tmp[1] = time;
                                tmp[2] = close;
                                tmp[3] = vol;
                                OnRspQryHistMinuteData(tmp, null, n, sb.RequestId);
                            }

                        }

                        //if (n > 0)
                        //{
                        //    i = 6;
                        //    num4 = 0;
                        //    double res;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        date[j] = sb.type;
                        //        time[j] = gptime(sb.sk.mark, j);
                        //        num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (double)(num4 / 100.0);
                        //        res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    }
                        //    if (HisBoard.Visible)
                        //    {

                        //        tStock1.FS_cleardata();
                        //        sk = sb.sk;
                        //        if (sk != null)
                        //        {
                        //            tStock1.PreClose = hispreclose;
                        //            tStock1.StkCode = sk.codes;
                        //            tStock1.StkName = sk.names;
                        //            tStock1.StkWeek = WeekString[zq];
                        //        }
                        //        HisName.Text = sk.names + " " + HisDate.ToString() + " 历史分时图";
                        //        tStock1.FS_AddAll("date", date, n, false);
                        //        tStock1.FS_AddAll("time", time, n, false);
                        //        tStock1.FS_AddAll("vol", vol, n, false);
                        //        tStock1.FS_AddAll("close", close, n, true);
                        //    }
                        //    if (StockBoard.Visible) //多日分时
                        //    {
                        //        for (int j = 0; j < n; j++)
                        //        {
                        //            DayTick dt = new DayTick();
                        //            dt.date = (int)date[j];
                        //            dt.time = (int)time[j];
                        //            dt.vol = vol[j];
                        //            dt.close = close[j];
                        //            DayTicks.Add(dt);
                        //        }
                        //    }

                        //}
                        #endregion

                        break;
                    case 0x529://日线
                        #region K线数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        //logger.Info("QrySecurityBars Response Count:{0}".Put(n));
                        if (n == 0)
                        {
                            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                            tmp["date"] = new double[1];
                            tmp["time"] = new double[1];
                            tmp["open"] = new double[1];
                            tmp["high"] = new double[1];
                            tmp["low"] = new double[1];
                            tmp["close"] = new double[1];
                            tmp["vol"] = new double[1];
                            tmp["amount"] = new double[1];

                            if (OnRspQrySecurityBar != null)
                            {
                                OnRspQrySecurityBar(tmp, null, n, sb.RequestId);
                            }
                        }
                        if (n > 0)
                        {
                            int yy1 = 0, mm1 = 0, dd1 = 0, hhh = 0, mmm = 0;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] high = new double[n + 1];
                            double[] low = new double[n + 1];
                            double[] open = new double[n + 1];
                            double[] close = new double[n + 1];
                            double[] amount = new double[n + 1];
                            double[] vol = new double[n + 1];
                            double[] upcount = new double[n];
                            double[] downcount = new double[n];
                            bool isIndex = false;
                            //判断是否是指数Bars
                            if ((sb.StkType == 100) || (sb.StkType == 7))//指数
                                isIndex = true;
                            i = 2;
                            num9 = 0;
                            for (int j = 0; j < n; j++)
                            {
                                //Application.DoEvents();
                                TDX.TDXDecoder.TDXGetDate(TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i), ref yy1, ref mm1, ref dd1, ref hhh, ref mmm);
                                date[j] = yy1 * 10000 + mm1 * 100 + dd1;
                                time[j] = hhh * 100 + mmm;
                                num10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                open[j] = (float)((num9 + num10) / 1000.0);
                                num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (float)((num9 + num10 + num6) / 1000.0);
                                num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                high[j] = (float)((num9 + num10 + num7) / 1000.0);
                                num8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                low[j] = (float)((num9 + num10 + num8) / 1000.0);

                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                amount[j] = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                                num9 = num9 + num10 + num6;
                                //如果是指数 则还记录了上涨与下跌
                                if (isIndex)
                                {
                                    upcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                                    downcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                                }
                                //logger.Info("O:{0} H:{1} L:{2} C:{3}".Put(open[j], high[j], low[j], close[j]));
                            }

                            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                            tmp["date"] = date;
                            tmp["time"] = time;
                            tmp["open"] = open;
                            tmp["high"] = high;
                            tmp["low"] = low;
                            tmp["close"] = close;
                            tmp["vol"] = vol;
                            tmp["amount"] = amount;

                            if (OnRspQrySecurityBar != null)
                            {
                                OnRspQrySecurityBar(tmp, null, n, sb.RequestId);
                            }
                        }
                        //if (n > 0)
                        //{
                        //    int yy1 = 0, mm1 = 0, dd1 = 0, hhh = 0, mmm = 0;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    high = new double[n + 1];
                        //    low = new double[n + 1];
                        //    open = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    double[] upcount = new double[n];
                        //    double[] downcount = new double[n];
                        //    bool zs = false;
                        //    if ((sb.sk.type == 100) || (sb.sk.type == 7))//指数
                        //        zs = true;
                        //    i = 2;
                        //    num9 = 0;
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        TDX.TDXDecoder.TDXGetDate(TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i), ref yy1, ref mm1, ref dd1, ref hhh, ref mmm);
                        //        date[j] = yy1 * 10000 + mm1 * 100 + dd1;
                        //        time[j] = hhh * 100 + mmm;
                        //        num10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        open[j] = (float)((num9 + num10) / 1000.0);
                        //        num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (float)((num9 + num10 + num6) / 1000.0);
                        //        num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        high[j] = (float)((num9 + num10 + num7) / 1000.0);
                        //        num8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        low[j] = (float)((num9 + num10 + num8) / 1000.0);

                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        amount[j] = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                        //        num9 = num9 + num10 + num6;
                        //        if (zs)
                        //        {
                        //            upcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //            downcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //        }
                        //    }
                        //    if (sb.type == 1000)
                        //    {
                        //        if (GP.Days > 1)
                        //        {
                        //            List1.Items.Clear();
                        //            int t1 = Math.Max(0, n - GP.Days + 1);
                        //            for (int j = t1; j < n; j++)
                        //            {
                        //                List1.Items.Add(date[j].ToString());
                        //                byte[] aaa = { 0xC, 0x1, 0x30, 0x0, 0x1, 0x1, 0xD, 0x0, 0xD, 0x0, 0xB4, 0xF, 0xD1, 0xD2, 0xD3, 0xD4, 0xFF, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36 };
                        //                byte[] sb12 = BitConverter.GetBytes(Convert.ToInt32(date[j]));
                        //                sb12.CopyTo(aaa, 12);
                        //                aaa[16] = FCurStock.mark;
                        //                FCurStock.GP.code.CopyTo(aaa, 17);
                        //                SendBuf sb11 = new SendBuf();
                        //                sb11.sk = FCurStock;
                        //                sb11.Send = aaa;
                        //                sb11.type = 1;
                        //                SendList.Enqueue(sb11);
                        //            }

                        //            byte[] a = { 0xC, 0x1B, 0x8, 0x0, 0x1, 0x1, 0xE, 0x0, 0xE, 0x0, 0x1D, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x0, 0x0, 0x0, 0x0 };
                        //            a[12] = FCurStock.mark;
                        //            FCurStock.GP.code.CopyTo(a, 14);
                        //            SendBuf sb21 = new SendBuf();
                        //            sb21.Send = a;
                        //            sb21.sk = FCurStock;
                        //            SendList.Enqueue(sb21);
                        //        }
                        //        return;
                        //    }
                        //    if (StockBoard.Visible)
                        //    {
                        //        int ll = GP.LeftBar;

                        //        GP.K_cleardata();
                        //        sk = sb.sk;
                        //        if (sk != null)
                        //        {
                        //            GP.PreClose = sk.GP.YClose;
                        //            if (sk.now.last != 0)
                        //                GP.PreClose = sk.now.last;
                        //            GP.StkCode = sk.codes;
                        //            GP.StkName = sk.names;
                        //            GP.StkWeek = WeekString[zq];
                        //        }

                        //        GP.AddAll("date", date, n, false);
                        //        GP.AddAll("time", time, n, false);
                        //        GP.AddAll("high", high, n, false);
                        //        GP.AddAll("low", low, n, false);
                        //        GP.AddAll("open", open, n, false);
                        //        GP.AddAll("close", close, n, false);
                        //        GP.AddAll("amount", amount, n, false);
                        //        if (zs)
                        //        {
                        //            GP.AddAll("upcount", upcount, n, false);
                        //            GP.AddAll("downcount", downcount, n, false);
                        //        }
                        //        GP.AddAll("vol", vol, n, true);
                        //        if (ll > 0)
                        //            GP.LeftBar = ll;
                        //    }
                        //    if (MutiBoard.Visible)
                        //    {
                        //        sk = sb.sk;
                        //        for (int j = 0; j < mgp.Length; j++)
                        //        {
                        //            if (msk[j] == sk)
                        //            {
                        //                mgp[j].BeginUpdate();
                        //                mgp[j].K_cleardata();
                        //                mgp[j].PreClose = sk.GP.YClose;
                        //                if (sk.now.last != 0)
                        //                    mgp[j].PreClose = sk.now.last;
                        //                mgp[j].StkCode = sk.codes;
                        //                mgp[j].StkName = sk.names;
                        //                mgp[j].StkWeek = WeekString[zq];
                        //                mgp[j].AddAll("date", date, n, false);
                        //                mgp[j].AddAll("time", time, n, false);
                        //                mgp[j].AddAll("high", high, n, false);
                        //                mgp[j].AddAll("low", low, n, false);
                        //                mgp[j].AddAll("open", open, n, false);
                        //                mgp[j].AddAll("close", close, n, false);
                        //                mgp[j].AddAll("amount", amount, n, false);
                        //                if (zs)
                        //                {
                        //                    mgp[j].AddAll("upcount", upcount, n, false);
                        //                    mgp[j].AddAll("downcount", downcount, n, false);
                        //                }
                        //                mgp[j].AddAll("vol", vol, n, true);
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        break;
                        #endregion

                    case 0x2cf:
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //if (n > 0)
                        //{
                        //    i = 2;
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Array.Copy(RecvBuffer, i, name, 0, 8);
                        //        i = i + 64;
                        //        Array.Copy(RecvBuffer, i, code, 0, 6);
                        //        i = i + 80;
                        //        int start = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                        //        int Len = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                        //        names = System.Text.Encoding.GetEncoding("GB2312").GetString(name);
                        //        codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                        //        info[j].name = names;
                        //        info[j].code = codes;
                        //        info[j].start = start;
                        //        info[j].len = Len;
                        //        Binfo[j].Text = names;
                        //    }
                        //    bt1_Click(Binfo[0], null);
                        //}
                        break;
                    case 0x2d0:
                        int k = RecvBuffer.Length - 24;
                        //byte[] txt = new byte[k];
                        //Array.Copy(RecvBuffer, 24, txt, 0, k);
                        //Info1.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(txt);
                        break;
                }
            }
            catch
            {
            }
        }
    }
}
