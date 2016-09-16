using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace DataAPI.TDX
{

    
    // 接收数据头
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RecvDataHeader
    {
        public UInt32 CheckSum;
        public byte EncodeMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] tmp;         //char tmp[5];
        public ushort msgid;
        public ushort Size;
        public ushort DePackSize;
    }

    public class SendBuf
    {
        public int RequestId = -1;
        public byte[] Send = null;
        //public CStock.Stock sk = null;
        public int type = 0;
        public RecvDataHeader hd = null;
        public Byte[] Buffer = null;

        /// <summary>
        /// 市场
        /// </summary>
        public byte Market;

        /// <summary>
        /// 代码
        /// </summary>
        public string Code;


        int _stktype = -1;
        /// <summary>
        /// 股票类别
        /// //1:=A股 2:=B股 5:=中小 6:=创业 3:=债券  7:=指数 10=权证 4:=基金 8:三板
        /// </summary>
        public int StkType
        { 
            get
            {
                if (_stktype == -1 && this.Code.Length == 6)
                {
                    _stktype = TDX.TDXDecoder.GetStockType((int)this.Market, this.Code);
                }
                return _stktype;
            }
        
        }
    }




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

        public TDXT now = new TDXT();// 最新盘口资料
        public TDXT last = new TDXT();// 盘口资料
        public Int64 NCode;
        public double[] List = new double[30];
        public int bk1, bk2;//行业板块
    };

    [Serializable()]
    public struct TDXT
    {
        //public byte MarketMode; // 市场
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        //public byte[] code;
        public int Time;
        public double high;
        public double last;
        public double low;
        public double open;
        public double prize; // 现价
        public double volume;
        public double amount;
        public double tradeQTY;
        public double b;
        public double s;
        public double sell1;
        public double sell2;
        public double sell3;
        public double sell4;
        public double sell5;
        public double sell6;
        public double sell7;
        public double sell8;
        public double sell9;
        public double sell10;
        public double sellQTY1;
        public double sellQTY2;
        public double sellQTY3;
        public double sellQTY4;
        public double sellQTY5;
        public double sellQTY6;
        public double sellQTY7;
        public double sellQTY8;
        public double sellQTY9;
        public double sellQTY10;

        public double buy1;
        public double buy2;
        public double buy3;
        public double buy4;
        public double buy5;
        public double buy6;
        public double buy7;
        public double buy8;
        public double buy9;
        public double buy10;
        public double buyQTY1;
        public double buyQTY2;
        public double buyQTY3;
        public double buyQTY4;
        public double buyQTY5;
        public double buyQTY6;
        public double buyQTY7;
        public double buyQTY8;
        public double buyQTY9;
        public double buyQTY10;

        public int BiCount;
        public double buyall, sellall, buyQTYall, sellQTYall;
        //d1, d2, d3, d4, d5: single;
    };

}
