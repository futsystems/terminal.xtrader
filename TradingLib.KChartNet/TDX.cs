using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using TradingLib.MarketData;

namespace CStock
{

    // 公司资料原始数据
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TGPNAME // 初始化数据 29字节
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] code;         //char tmp[5];
        public ushort rate;// 实时盘口中的成交量除去的除数？1手=n股？
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] name;//名称
        public ushort w1, w2;
        public byte PriceMag;//小数点位数
        public float YClose;//昨收
        public ushort w3, w4;
    };
    //财务
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CaiWu
    {
        public double LTG;//流通股数量
        public ushort t1, t2;
        public uint day1, day2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public float[] zl;
    };
    //权息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QuanInfo
    {
        public byte style;
        public uint Date;
        public float Money;
        public float PeiMoney;
        public float Number;
        public float PeiNumber;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Quan
    {
        public int QuanLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public QuanInfo[] quan;
    };

    public class Stock
    {
        public String names, codes, keys;
        public TGPNAME GP = new TGPNAME(); // 原始资料
        public Quan qu = new Quan();// QuanInfo[] qu = new QuanInfo[80];
        public CaiWu cw = new CaiWu();// 财务资料
        public byte mark, type;//市场和类型

        public TDX now = new TDX();// 最新盘口资料
        public TDX last = new TDX();// 盘口资料
        public Int64 NCode;
        public double[] List = new double[30];
        public int bk1, bk2;//行业板块
    };

    
}
