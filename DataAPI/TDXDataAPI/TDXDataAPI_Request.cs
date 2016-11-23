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
    public partial class TDXDataAPI
    {

        public event Action<MDSymbol> OnRtnTick;
        /// <summary>
        /// 注册合约行情
        /// </summary>
        /// <param name="symbols"></param>
        public void RegisterSymbol(MDSymbol[] symbols)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// 注销合约行情
        /// </summary>
        /// <param name="symbols"></param>
        public void UnregisterSymbol(MDSymbol[] symbols)
        {
            throw new NotImplementedException();
        }


        
        /// <summary>
        /// 查询一组合约的行情快照数据
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public int QryTickSnapshot(MDSymbol[] symbols)
        {
            //查询列表中的一组合约数据
            //int hh = Stklist.Height / Stklist.ItemHeight + 1;
            //if (hh > (Stklist.Items.Count - Stklist.TopIndex))
            //    hh = Stklist.Items.Count - Stklist.TopIndex;
            int count = symbols.Length;

            byte[] header = { 0x0C, 0x01, 0x08, 0x00, 0x02, 0x01, 0x0F, 0x00, 0x0F, 0x00, 0x26, 0x05, 0x01, 0x00 };
            int j = (short)header.Length;
            byte[] request = new byte[count * 11 + j];
            header.CopyTo(request, 0);
            foreach (var sym in symbols)// (int i = 0; i < hh; i++)
            {
                if (sym == null) continue;
                //CStock.Stock sk1 = (CStock.Stock)Stklist.Items[Stklist.TopIndex + i];
                request[j] = (byte)GetMarketCode(sym.Exchange);//sk1.mark;
                Encoding.GetEncoding("GB2312").GetBytes(sym.Symbol).CopyTo(request, j + 1);
                //sk1.GP.code.CopyTo(request, j + 1);
                byte[] btime = BitConverter.GetBytes(sym.TickSnapshot.Time);
                btime.CopyTo(request, j + 7);
                j += 11;
            }
            request[12] = (byte)count;//数量
            short len = (short)(count * 11 + 4);
            byte[] b1 = BitConverter.GetBytes(len);
            b1.CopyTo(request, 6);
            b1.CopyTo(request, 8);
            SendBuf sb = new SendBuf();
            sb.type = 100;
            sb.Send = request;
            sb.RequestId = this.NextRequestId;
            NewRequest(sb);
            return sb.RequestId;
        }

        /// <summary>
        /// 查询当日成交分布数据
        /// </summary>
        /// <param name="market"></param>bb
        /// <param name="symbol"></param>
        public int QryPriceVol(string exchange, string symbol)
        {
            int market = GetMarketCode(exchange);

            byte[] bb = { 0x0C, 0x25, 0x08, 0x00, 0x03, 0x01, 0x0A, 0x00, 0x0A, 0x00, 0x1A, 0x05, 0x00, 0x00, 0x30, 0x30, 0x30, 0x30, 0x30, 0x32 };
            bb[13] = (byte)(ushort)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(bb, 14);

            SendBuf sb11 = new SendBuf();
            //sb11.sk = null;
            sb11.Send = bb;
            sb11.RequestId = this.NextRequestId;

            NewRequest(sb11);

            return sb11.RequestId;

        }

        /// <summary>
        /// 查询当日分笔数据
        /// </summary>
        /// <param name="market">0 深证 1 上海</param>
        /// <param name="symbol">股票代码</param>
        /// <param name="start">起始位置 最后一条为0 前一条为1，以此类推</param>
        /// <param name="Count">请求记录数量</param>
        public int QryTradeSplitData(string exchange, string symbol, int start, int Count)
        {
            logger.Info(string.Format("QryTradeSplitData exchange:{0} symbol:{1} start:{2} count:{3}", exchange, symbol, start, Count));
            int market = GetMarketCode(exchange);

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
            NewRequest(sb11);
            //logger.Info("put tradesplit request into Enqueue");
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
            NewRequest(sb11);
            return sb11.RequestId;
        }


        /// <summary>
        /// 查询当日分时数据
        /// </summary>
        /// <param name="market"></param>
        /// <param name="symbol"></param>
        public int QryMinuteDate(string exchange, string symbol, int date)
        {
            logger.Info(string.Format("QryMinuteDate exchange:{0} symbol:{1} date:{2}", exchange, symbol, date));
            int market = GetMarketCode(exchange);
            if (date <= 0)
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
                NewRequest(sb);
                //logger.Info("put minute request into Enqueue");
                return sb.RequestId;
            }
            else
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
                sb11.type = date;//将日期放入type TDX服务端返回历史分时 没有日期数据
                sb11.RequestId = this.NextRequestId;
                NewRequest(sb11);
                return sb11.RequestId;
            }
        }
        public int QryMinuteDate(string exchange, string symbol, long start)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询合约信息类别
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QrySymbolInfoType(string exchange, string symbol)
        {
            int market = GetMarketCode(exchange);
            byte[] request = { 0xC, 0xF, 0x10, 0x9B, 0x0, 0x1, 0xE, 0x0, 0xE, 0x0, 0xCF, 0x2, 0x0, 0xFF, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x0, 0x0, 0x0, 0x0 };
            request[13] = (byte)market;
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);
            SendBuf sb = new SendBuf();
            sb.Send = request;
            sb.Code = symbol;
            sb.Market = (byte)(ushort)market;
            sb.RequestId = this.NextRequestId;
            NewRequest(sb);
            return sb.RequestId;
        }

        /// <summary>
        /// 查询合约信息
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int QrySymbolInfo(string exchange, string symbol, SymbolInfoType type)
        {
            int market = GetMarketCode(exchange);
            byte[] request = {0xC, 0x7, 0x10, 0x9C, 0x0, 0x1, 0x68, 0x0, 0x68, 0x0, 0xD0, 0x2, 0x0, 0xFF, 0x31, 0x32,
               0x33, 0x34, 0x35, 0x36, 0xAA, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x2E, 0x74, 0x78, 0x74,
               0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 
               0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
               0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
               0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
               0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD7, 0xD8, 0x0, 0x0, 0x0, 0x0, 0x0};
            request[13] = (byte)market;
            request[20] = (byte)type.TypeCode; //资料分类 -序号0~15
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 14);
            Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 22);
            byte[] tmp = BitConverter.GetBytes(type.Start);
            request[102] = tmp[0];
            request[103] = tmp[1];
            request[104] = tmp[2];
            request[105] = tmp[3];
            byte[] tmp1 = BitConverter.GetBytes(type.Length);
            request[106] = tmp1[0];
            request[107] = tmp1[1];
            request[108] = tmp1[2];
            request[109] = tmp1[3];
            SendBuf sb = new SendBuf();
            sb.Send = request;
            sb.Code = symbol;
            sb.Market = (byte)(ushort)market;
            sb.RequestId = this.NextRequestId;
            NewRequest(sb);
            return sb.RequestId;
        }


        /// <summary>
        /// 查询历史分时
        /// </summary>
        /// <param name="market"></param>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        //public int QryHistMinuteDate(int market, string symbol, int date)
        //{
        //    byte[] request = { 0xC, 0x1, 0x30, 0x0, 0x1, 0x1, 0xD, 0x0, 0xD, 0x0, 0xB4, 0xF, 0xD1, 0xD2, 0xD3, 0xD4, 0xFF, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36 };
        //    byte[] sb = BitConverter.GetBytes(date);
        //    request[12] = sb[0];
        //    request[13] = sb[1];
        //    request[14] = sb[2];
        //    request[15] = sb[3];
        //    request[16] = (byte)(ushort)market;
        //    Encoding.GetEncoding("GB2312").GetBytes(symbol).CopyTo(request, 17);
        //    SendBuf sb11 = new SendBuf();
        //    //sb11.sk = null;
        //    sb11.Send = request;
        //    sb11.Code = symbol;
        //    sb11.Market = (byte)(ushort)market;
        //    //sb11.type = Convert.ToInt32(e.Date);
        //    sb11.RequestId = this.NextRequestId;
        //    SendList.Enqueue(sb11);
        //    return sb11.RequestId;
        //}

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
        /// 查询某个时间之后的Bar数据
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        /// <param name="freqStr"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int QrySecurityBars(string exchange, string symbol, string freqStr, long start, long end)
        {
            logger.Info("not support qry bar via time");
            return 0;
        }

        public int QrySecurityBars(string exchange, string symbol, string freqStr, int start, int count)
        {
            return QrySeurityBars(exchange, symbol, freqStr, start, count, 0);
        }

        /// <summary>
        /// 查询Bar数据
        /// </summary>
        /// <param name="market">0 深证 1 上海</param>
        /// <param name="symbol">股票代码</param>
        /// <param name="freq">K线种类, 0->5分钟K线    1->15分钟K线    2->30分钟K线  3->1小时K线    4->日K线  5->周K线  6->月K线  7->1分钟    10->季K线  11->年K线</param>
        /// <param name="start">K线开始位置,最后一条K线位置是0, 前一条是1, 依此类推</param>
        /// <param name="count">API执行前,表示用户要请求的K线数目, API执行后,保存了实际返回的K线数目, 最大值800</param>
        public int QrySeurityBars(string exchange, string symbol, string freqStr, int start, int count,int type)
        {
            if (type != 1000)
            {
                logger.Info(string.Format("QrySecurityBars exchange:{0} symbol:{1} start:{2} count:{3}", exchange, symbol, start, count));
            }
            //else
            //{
            //    logger.Info("HeartBeat Request");
            //}
                int market = GetMarketCode(exchange);
            int freq = GetFreqCode(freqStr);

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
            sb11.type = type;
            NewRequest(sb11);
            return sb11.RequestId;
        }

    }
}
