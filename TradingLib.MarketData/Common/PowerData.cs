using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TradingLib.MarketData
{
    //权息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PowerItem
    {
        public byte style;
        public uint Date;
        public float Money;
        public float PeiMoney;
        public float Number;
        public float PeiNumber;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PowerData
    {
        public int QuanLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public PowerItem[] quan;
    };
}
