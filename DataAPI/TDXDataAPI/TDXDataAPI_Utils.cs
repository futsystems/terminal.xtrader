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
        int GetMarketCode(string exchange)
        {
            switch (exchange)
            {
                case ConstsExchange.EXCH_SSE:
                    return 1;
                case ConstsExchange.EXCH_SZE:
                    return 0;
                default:
                    return -1;
            }
        }

        string GetMarketString(int market)
        {
            if (market == 0)
            {
                return ConstsExchange.EXCH_SZE;
            }
            if (market == 1)
            {
                return ConstsExchange.EXCH_SSE;
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
    }
}
