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








        List<MDSymbol> symbolList = new List<MDSymbol>();
        Dictionary<string, MDSymbol> symbolMap = new Dictionary<string, MDSymbol>();
        /// <summary>
        /// 所有合约
        /// </summary>
        public IEnumerable<MDSymbol> Symbols { get { return symbolMap.Values; } }

        List<SymbolHighLight> hightLight = new List<SymbolHighLight>();
        /// <summary>
        /// 底部亮显合约
        /// </summary>
        public IEnumerable<SymbolHighLight> HightLightSymbols { get { return hightLight; } }

        List<BlockInfo> blockInfoList = new List<BlockInfo>();
        /// <summary>
        /// 所有板块信息
        /// </summary>
        public IEnumerable<BlockInfo> BlockInfos { get { return blockInfoList; } }


        Dictionary<string, RawData> rawDataMap = new Dictionary<string, RawData>();
        RawData GetRawData(string exchange, string symbol)
        {
            RawData target = null;
            string key = string.Format("{0}-{1}", exchange, symbol);
            if (!rawDataMap.TryGetValue(key, out target))
            {
                rawDataMap.Add(key, new RawData());
            }
            return rawDataMap[key];

        }

        MDSymbol GetSymbol(string exchange, string symbol)
        {
            string key = string.Format("{0}-{1}", exchange, symbol);
            MDSymbol target = null;
            if (symbolMap.TryGetValue(key, out target))
            {
                return target;
            }
            return null;
        }

        string GetBaseFileName()
        {
            return Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "base.dat" });
        }

        string GetSession(MDSymbol symbol)
        {
            return "93000-113000,130000-150000";
        }
        /// <summary>
        /// 初始化合约信息
        /// </summary>
        void InitSymbol()
        {

            int i, count, n, j;
            MDService.EventHub.FireInitializeStatusEvent("深圳代码初始化");
            //ConvertHzToPz_Gb2312 htp = new ConvertHzToPz_Gb2312();
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
                i = 0;
                count = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);// RecvBr.ReadUInt16();
                i = 0;
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
                            gname = (TGPNAME)TDX.TDXDecoder.BytesToStuct(RecvBuffer, pp, type);

                            symbol = new MDSymbol();

                            symbol.Name = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.name);
                            symbol.Symbol = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.code);
                            symbol.Key = ConvertHzToPz_Gb2312.Convert(symbol.Name);
                            symbol.NCode = TDX.TDXDecoder.EnCodeMark(symbol.Symbol, 0);
                            symbol.Exchange = ConstsExchange.EXCH_SZE;
                            symbol.BlockType = TDXDecoder.GetStockType(0, symbol.Symbol).ToString();
                            symbol.PreClose = gname.YClose;
                            symbol.Session = GetSession(symbol);

                            symbolMap[symbol.UniqueKey] = symbol;
                            symbolList.Add(symbol);

                            RawData rd = GetRawData(symbol.Exchange, symbol.Symbol);
                            rd.TGPNAME = gname;
                            rd.TGPNAME.w3 = (ushort)0;
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
                            symbol.Key = ConvertHzToPz_Gb2312.Convert(symbol.Name);
                            symbol.NCode = TDX.TDXDecoder.EnCodeMark(symbol.Symbol, 0);
                            symbol.Exchange = ConstsExchange.EXCH_SSE;
                            symbol.BlockType = TDXDecoder.GetStockType(1, symbol.Symbol).ToString();
                            symbol.PreClose = gname.YClose;
                            symbol.Session = GetSession(symbol);


                            symbolMap[symbol.UniqueKey] = symbol;
                            symbolList.Add(symbol);


                            RawData rd = GetRawData(symbol.Exchange, symbol.Symbol);
                            rd.TGPNAME = gname;
                            rd.TGPNAME.w3 = (ushort)1;
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
                            RawData rd = GetRawData(target.Exchange, target.Symbol);
                            rd.FinanceData = target.FinanceData;
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
                            RawData rd = GetRawData(target.Exchange, target.Symbol);
                            rd.PowerData = target.PowerData;

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
            SaveRawData();
        }

        /// <summary>
        /// 保存基础数据到文件
        /// </summary>
        void SaveRawData()
        {
            string fn = GetBaseFileName();
            using (Stream fs1 = File.Create(fn))
            {
                byte[] dbyte = BitConverter.GetBytes(stkDate);
                fs1.Write(dbyte, 0, dbyte.Length);
                byte[] lenbyte = BitConverter.GetBytes(rawDataMap.Count);
                fs1.Write(lenbyte, 0, lenbyte.Length);

                int pp = 0;
                MDService.EventHub.FireInitializeStatusEvent("保存基础数据");
                for (int k = 0; k < rawDataMap.Count; k++)
                {

                    byte[] b1 = TDX.TDXDecoder.StructToBytes(rawDataMap.ElementAt(k).Value.TGPNAME);
                    fs1.Write(b1, 0, b1.Length);
                    byte[] b2 = TDX.TDXDecoder.StructToBytes(rawDataMap.ElementAt(k).Value.FinanceData);
                    fs1.Write(b2, 0, b2.Length);
                    byte[] b3 = TDX.TDXDecoder.StructToBytes(rawDataMap.ElementAt(k).Value.PowerData);
                    fs1.Write(b3, 0, b3.Length);
                    pp++;
                    MDService.EventHub.FireInitializeStatusEvent("保存基础数据:" + ((double)k / (double)rawDataMap.Count).ToString("0%"));
                }
                fs1.Close();
            }
        }


        /// <summary>
        /// 初始化基础数据
        /// 合约 财务 权息等
        /// </summary>
        void InitBasicData()
        {
            string fn = GetBaseFileName();
            if (File.Exists(fn))
            {
                using (Stream fs2 = File.Open(fn, FileMode.Open))
                {
                    BinaryReader br1 = new BinaryReader(fs2);
                    int datetime1 = br1.ReadInt32();
                    if (datetime1 == stkDate)
                    {
                        MDService.EventHub.FireInitializeStatusEvent("初始化数据");
                        int cnt = br1.ReadInt32();
                        for (int i = 0; i < cnt; i++)
                        {
                            MDSymbol symbol = new MDSymbol();

                            byte[] bb1 = br1.ReadBytes(Marshal.SizeOf(typeof(TGPNAME)));
                            TGPNAME gname = (TGPNAME)TDX.TDXDecoder.BytesToStuct(bb1, 0, typeof(TGPNAME));

                            symbol.Name = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.name);
                            symbol.Symbol = System.Text.Encoding.GetEncoding("GB2312").GetString(gname.code);
                            symbol.Key = ConvertHzToPz_Gb2312.Convert(symbol.Name);
                            symbol.NCode = TDX.TDXDecoder.EnCodeMark(symbol.Symbol, gname.w3);
                            symbol.Exchange = gname.w3 == 0 ? ConstsExchange.EXCH_SZE : ConstsExchange.EXCH_SSE;
                            symbol.BlockType = TDXDecoder.GetStockType(gname.w3, symbol.Symbol).ToString();
                            symbol.PreClose = gname.YClose;
                            symbol.Session = GetSession(symbol);


                            byte[] bb2 = br1.ReadBytes(Marshal.SizeOf(typeof(FinanceData)));
                            symbol.FinanceData = (FinanceData)TDX.TDXDecoder.BytesToStuct(bb2, 0, typeof(FinanceData));

                            byte[] bb3 = br1.ReadBytes(Marshal.SizeOf(typeof(PowerData)));
                            symbol.PowerData = (PowerData)TDX.TDXDecoder.BytesToStuct(bb3, 0, typeof(PowerData));

                            symbolMap[symbol.UniqueKey] = symbol;
                            symbolList.Add(symbol);
                            MDService.EventHub.FireInitializeStatusEvent("初始化数据:" + ((double)i / (double)cnt).ToString("0%"));
                        }
                        //初始化完毕后返回
                        this.InitHightLight();
                        return;
                    }
                }
            }
            //没有文件或者日期不相等 则从接口初始化数据 并保存最新数据
            this.InitSymbol();
            this.InitFinance();
            this.InitPower();
            this.SaveRawData();
            this.InitHightLight();


            
        }

        void InitHightLight()
        {
            MDSymbol sh = this.GetSymbol(ConstsExchange.EXCH_SSE, "999999");
            MDSymbol sz = this.GetSymbol(ConstsExchange.EXCH_SZE, "399001");

            if (sh != null)
            {
                hightLight.Add(new SymbolHighLight("沪", sh));
            }
            if (sz != null)
            {
                hightLight.Add(new SymbolHighLight("深", sz));
            }
        }


    }
}
