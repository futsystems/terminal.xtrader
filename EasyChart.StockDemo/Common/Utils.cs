using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace Easychart.Finance.DataProvider
{
    public static class Utils
    {

        public static int ToSeconds(this DataCycle cycle)
        { 
            
            int baseinterval = 60;
            switch(cycle.CycleBase)
            {
                case DataCycleBase.MINUTE:
                    baseinterval = 60;
                    break;
                case DataCycleBase.DAY:
                    baseinterval = 86400;
                    break;
                default:
                    break;
            }
            return baseinterval * cycle.Repeat;
        }
    }
}
