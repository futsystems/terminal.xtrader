using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAPI.TDX
{
    public class RspInfo
    {

        public string Code { get; set; }

        public string Message { get; set; }


    }
    /// <summary>
    /// 价格分布
    /// 用于记录在某个价位上成交量形成价格分布
    /// </summary>
    public class PriceVolPair
    {
        /// <summary>
        /// 成交价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public int Vol { get; set; }

        public PriceVolPair(double price, int vol)
        {
            this.Price = price;
            this.Vol = vol;
        }
    }

    /// <summary>
    /// 分笔成交快照
    /// 用于统计某个时间内发生的成交价格，成交
    /// </summary>
    public class TradeSplit
    {
        /// <summary>
        /// 成交时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public int Vol { get; set; }

        /// <summary>
        /// 买卖标识
        /// </summary>
        public int Flag { get; set; }

        /// <summary>
        /// 成交次数
        /// </summary>
        public int TradeCount { get; set; }

        public TradeSplit(int time, double price, int vol, int flag, int count)
        {
            this.Time = time;
            this.Price = price;
            this.Vol = vol;
            this.Flag = flag;
            this.TradeCount = count;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", this.Time, this.Price, this.Vol, this.Flag, this.TradeCount);
        }
    }

    
}
