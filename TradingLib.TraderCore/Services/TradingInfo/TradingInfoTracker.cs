using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    public partial class TradingInfoTracker
    {
        /// <summary>
        /// 委托维护器
        /// </summary>
        public OrderTracker OrderTracker { get; set; }

        /// <summary>
        /// 当日持仓维护器
        /// </summary>
        public LSPositionTracker PositionTracker { get; set; }

        /// <summary>
        /// 昨日持仓维护器
        /// </summary>
        public LSPositionTracker HoldPositionTracker { get; set; }

        /// <summary>
        /// 成交维护器
        /// </summary>
        public ThreadSafeList<Trade> TradeTracker { get; set; }

        public AccountLite Account { get; set; }

        public TradingInfoTracker()
        {
            OrderTracker = new OrderTracker();
            PositionTracker = new LSPositionTracker("");
            HoldPositionTracker = new LSPositionTracker("");
            TradeTracker = new ThreadSafeList<Trade>();
            Account = new AccountLite();

        }

        

    }
}
