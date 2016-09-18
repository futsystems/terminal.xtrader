using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.KryptonControl
{
    public enum EnumFileldType
    {
        [Description("序号")]
        INDEX,
        [Description("合约")]
        SYMBOL,
        [Description("名称")]
        SYMBOLNAME,
        [Description("最新")]
        LAST,
        [Description("现手")]
        LASTSIZE,
        [Description("买量")]
        BIDSIZE,
        [Description("买价")]
        BID,
        [Description("卖价")]
        ASK,
        [Description("卖量")]
        ASKSIZE,
        [Description("成交量")]
        VOL,
        [Description("涨跌")]
        CHANGE,
        [Description("涨幅")]
        CHANGEPECT,
        [Description("持仓")]
        OI,
        [Description("仓差")]
        OICHANGE,
        [Description("结算价")]
        SETTLEMENT,
        [Description("开盘")]
        OPEN,
        [Description("最高")]
        HIGH,
        [Description("最低")]
        LOW,
        [Description("昨结算")]
        PRESETTLEMENT,
        [Description("昨收")]
        PRECLOSE,
        [Description("昨持仓")]
        PREOI,
        [Description("交易所")]
        EXCHANGE,
        [Description("均价")]
        AVGPRICE,
        [Description("内盘")]
        BSIDE,
        [Description("外盘")]
        SSIDE,
    }
}
