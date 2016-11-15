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

        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;
        /// <summary>
        /// 最高价
        /// </summary>
        public double High;

        /// <summary>
        /// 昨日收盘
        /// </summary>
        public double PreClose;
        /// <summary>
        /// 最低价
        /// </summary>
        public double Low;
        /// <summary>
        /// 开盘价
        /// </summary>
        public double Open;
        /// <summary>
        /// 现价
        /// </summary>
        public double Price; // 现价
        /// <summary>
        /// 成交量
        /// </summary>
        public double Volume;
        /// <summary>
        /// 成交额
        /// </summary>
        public double Amount;
        /// <summary>
        /// 现量
        /// </summary>
        public double Size;

        /// <summary>
        /// 内盘
        /// </summary>
        public double B;
        /// <summary>
        /// 外盘
        /// </summary>
        public double S;

        public double Sell1;
        public double Sell2;
        public double Sell3;
        public double Sell4;
        public double Sell5;
        public double Sell6;
        public double Sell7;
        public double Sell8;
        public double Sell9;
        public double Sell10;
        public double SellQTY1;
        public double SellQTY2;
        public double SellQTY3;
        public double SellQTY4;
        public double SellQTY5;
        public double SellQTY6;
        public double SellQTY7;
        public double SellQTY8;
        public double SellQTY9;
        public double SellQTY10;

        public double Buy1;
        public double Buy2;
        public double Buy3;
        public double Buy4;
        public double Buy5;
        public double Buy6;
        public double Buy7;
        public double Buy8;
        public double Buy9;
        public double Buy10;
        public double BuyQTY1;
        public double BuyQTY2;
        public double BuyQTY3;
        public double BuyQTY4;
        public double BuyQTY5;
        public double BuyQTY6;
        public double BuyQTY7;
        public double BuyQTY8;
        public double BuyQTY9;
        public double BuyQTY10;

        /// <summary>
        /// 逐笔数量
        /// </summary>
        public int BiCount;


        public double buyall, sellall, buyQTYall, sellQTYall;
        //d1, d2, d3, d4, d5: single;

        /// <summary>
        /// 持仓
        /// </summary>
        public int OI;

        /// <summary>
        /// 昨日持仓
        /// </summary>
        public int PreOI;

        /// <summary>
        /// 结算价
        /// </summary>
        public double Settlement;

        /// <summary>
        /// 昨日结算价
        /// </summary>
        public double PreSettlement;
    };
}
