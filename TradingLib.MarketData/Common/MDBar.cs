using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class MDBar
    {
        public MDBar(DateTime time, double open, double high, double low, double close, int vol, int oi, int tvol)
        {
            this.DateTimeStamp = time;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.PeriodVolume = vol;
            this.OpenInterest = oi;
            this.TotalVolume = tvol;
        }
        public MDBar(DateTime time, double open, double high, double low, double close, int vol, int oi)
            : this(time, open, high, low, close, vol, oi, 0)
        { 
        
        }

        public MDBar(DateTime time,double open,double high,double low,double close,int vol)
            :this(time,open,high,low,close,vol,0,0)
        {
            
        }
        
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTimeStamp { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// 区间成交量
        /// </summary>
        public int PeriodVolume { get; set; }

        /// <summary>
        /// 持仓
        /// </summary>
        public int OpenInterest { get; set; }

        /// <summary>
        /// 日内数据的所有累加成交量
        /// </summary>
        public int TotalVolume { get; set; }

        public override string ToString()
        {
            return string.Format("{0} O:{1} H:{2} L:{3} C:{4} V:{5} OI:{6} T:{7}", this.DateTimeStamp, this.Open, this.High, this.Low, this.Close, this.PeriodVolume, this.OpenInterest, this.TotalVolume);
        }
    }
}
