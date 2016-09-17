using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    [Serializable()]
    public struct TDX
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
