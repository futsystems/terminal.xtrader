﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    public partial class TradingInfoTracker
    {
        public Order SentOrder(long oid)
        {
            return this.OrderTracker.SentOrder(oid);
        }
    }
}
