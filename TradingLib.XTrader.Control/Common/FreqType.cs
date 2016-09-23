using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CStock
{
    //WeekString ={ "5分钟", "15分钟", "30分钟", "60分钟", "日线", "周线", "月线", "1分钟", "10分钟", "45天", "季线", "年线", "分笔线" };
    public enum KFrequencyType
    {
        [Description("5分钟")]
        F_5Min=0,
        [Description("15分钟")]
        F_15Min=1,
        [Description("30分钟")]
        F_30Min=2,
        [Description("60分钟")]
        F_60Min=3,
        [Description("日线")]
        F_Day=4,
        [Description("周线")]
        F_Week=5,
        [Description("月线")]
        F_Month=6,
        [Description("1分钟")]
        F_1Min=7,
        [Description("10分钟")]
        F_10Min=8,
        [Description("45天")]
        F_45Day=9,
        [Description("季线")]
        F_Quarter=10,
        [Description("年线")]
        F_Year=11,
        [Description("分笔线")]
        F_Trade=12,


    }
}
