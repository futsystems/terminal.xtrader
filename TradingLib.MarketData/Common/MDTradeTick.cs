using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class MDTradeTick
    {

        /// <summary>
        /// 日期
        /// </summary>
        public long DateTimeStamp { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 最新价
        /// </summary>
        public double Last { get; set; }

        /// <summary>
        /// 最新成交数量
        /// </summary>
        public int LastSize { get; set; }

        /// <summary>
        /// 累计数量
        /// </summary>
        public int TotalVol { get; set; }


        public bool IsValid()
        {
            if (this.Last * this.LastSize == 0) return false;
            return true;
        }
    }


    public class MDTradeSet
    {
        List<MDTradeTick> tradeList = new List<MDTradeTick>();

        public void NewTick(MDTradeTick tick)
        {
            tradeList.Add(tick);
        }

        /// <summary>
        /// 成交笔数
        /// </summary>
        public int TradeCount { get { return tradeList.Count; } }

        public MDTradeTick CalcSnapshot()
        {
            //区间内成交量
            int vol = tradeList.Sum(t => t.LastSize);
            //区间内成交均价
            double avgprice = tradeList.Sum(t => t.Last * t.LastSize) / vol;

            MDTradeTick tick = new MDTradeTick();
            tick.Last = avgprice;
            tick.LastSize = vol;
            tick.TotalVol = tradeList.Last().TotalVol;

            return tick;
        }

    }

}
