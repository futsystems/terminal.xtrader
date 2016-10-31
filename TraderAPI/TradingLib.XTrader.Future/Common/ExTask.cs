using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader.Future
{
    public enum EnumExTaskType
    { 
        /// <summary>
        /// 反手任务
        /// 用于强平某一持仓并建立反向持仓
        /// </summary>
        TaskReserve,
    }
    public class ExTask
    {

        public ExTask()
        {
            this.Position = null;
            this.FlatSentCount = 0;
            this.ReverseSentcount = 0;
            this.Order = null;
            this.StartTime = DateTime.Now;
        }
        /// <summary>
        /// 持仓
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 原始数量
        /// </summary>
        public int OrigSize { get; set; }


        public int FlatSentCount { get; set; }

        /// <summary>
        /// 反手委托发送次数
        /// </summary>
        public int ReverseSentcount { get; set; }
        /// <summary>
        /// 待发送委托
        /// </summary>
        public Order Order { get; set; }


        /// <summary>
        /// 任务类别
        /// </summary>
        public EnumExTaskType TaskType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }


        public static ExTask CreateReserveTask(Position pos)
        {
            ExTask task = new ExTask();
            task.TaskType = EnumExTaskType.TaskReserve;

            task.Position = pos;
            task.OrigSize = pos.Size;

            OrderImpl o = new OrderImpl();
            o.Symbol = pos.oSymbol.Symbol;
            o.Exchange = pos.oSymbol.Exchange;
            o.LimitPrice = 0;
            o.Side = pos.isLong ? false : true;
            o.Size = pos.UnsignedSize;

            task.Order = o;
            return task;
        }
    }
}
