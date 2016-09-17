using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TradingLib.MarketData
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FinanceData
    {
        public double LTG;//流通股数量
        public ushort t1, t2;
        public uint day1, day2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public float[] zl;
    };
}
