using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.KryptonControl
{
    public enum EnumFileldType
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        INDEX,
        /// <summary>
        /// 合约
        /// </summary>
        [Description("合约")]
        SYMBOL,
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        SYMBOLNAME,
        /// <summary>
        /// 最新
        /// </summary>
        [Description("最新")]
        LAST,
        /// <summary>
        /// 现量
        /// </summary>
        [Description("现量")]
        LASTSIZE,
        /// <summary>
        /// 买量
        /// </summary>
        [Description("买量")]
        BIDSIZE,
        /// <summary>
        /// 买价
        /// </summary>
        [Description("买价")]
        BID,
        /// <summary>
        /// 卖价
        /// </summary>
        [Description("卖价")]
        ASK,
        /// <summary>
        /// 卖量
        /// </summary>
        [Description("卖量")]
        ASKSIZE,
        /// <summary>
        /// 成交量
        /// </summary>
        [Description("成交量")]
        VOL,
        /// <summary>
        /// 涨跌
        /// </summary>
        [Description("涨跌")]
        CHANGE,
        /// <summary>
        /// 涨幅
        /// </summary>
        [Description("涨幅")]
        CHANGEPECT,
        /// <summary>
        /// 持仓
        /// </summary>
        [Description("持仓")]
        OI,
        /// <summary>
        /// 仓差
        /// </summary>
        [Description("仓差")]
        OICHANGE,
        /// <summary>
        /// 结算价
        /// </summary>
        [Description("结算价")]
        SETTLEMENT,
        /// <summary>
        /// 开盘价
        /// </summary>
        [Description("开盘")]
        OPEN,
        /// <summary>
        /// 最高价
        /// </summary>
        [Description("最高")]
        HIGH,
        /// <summary>
        /// 最低价
        /// </summary>
        [Description("最低")]
        LOW,
        /// <summary>
        /// 昨结算
        /// </summary>
        [Description("昨结算")]
        PRESETTLEMENT,
        /// <summary>
        /// 昨收
        /// </summary>
        [Description("昨收")]
        PRECLOSE,
        /// <summary>
        /// 昨持仓
        /// </summary>
        [Description("昨持仓")]
        PREOI,
        /// <summary>
        /// 交易所
        /// </summary>
        [Description("交易所")]
        EXCHANGE,
        /// <summary>
        /// 均价
        /// </summary>
        [Description("均价")]
        AVGPRICE,
        /// <summary>
        /// 内盘
        /// </summary>
        [Description("内盘")]
        BSIDE,
        /// <summary>
        /// 外盘
        /// </summary>
        [Description("外盘")]
        SSIDE,
    }
}
