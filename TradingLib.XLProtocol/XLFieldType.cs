﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.XLProtocol
{
    /// <summary>
    /// 数据域ID
    /// </summary>
    public enum XLFieldType : ushort
    {
        /// <summary>
        /// ErrorField
        /// </summary>
        F_ERROR = 0,

        /// <summary>
        /// 请求登入
        /// </summary>
        F_REQ_LOGIN = 100,

        /// <summary>
        /// 登入响应
        /// </summary>
        F_RSP_LOGIN = 101,

        /// <summary>
        /// 请求更新密码
        /// </summary>
        F_REQ_UPDATEPASS = 102,

        /// <summary>
        /// 更新密码回报
        /// </summary>
        F_RSP_UPDATEPASS = 103,

        /// <summary>
        /// 请求查询合约
        /// </summary>
        F_QRY_SYMBOL = 104,

        /// <summary>
        /// 查询合约响应
        /// </summary>
        F_RSP_SYMBOL = 105,

        /// <summary>
        /// 请求查询委托
        /// </summary>
        F_QRY_ORDER = 106,

        /// <summary>
        /// 查询委托响应
        /// </summary>
        F_RSP_ORDER = 107,

        /// <summary>
        /// 请求查询成交
        /// </summary>
        F_QRY_TRADE = 108,

        /// <summary>
        /// 查询成交响应
        /// </summary>
        F_RSP_TRADE = 109,

        /// <summary>
        /// 请求查询持仓
        /// </summary>
        F_QRY_POSITION = 110,

        /// <summary>
        /// 查询持仓响应
        /// </summary>
        F_RSP_POSITION = 111,

        /// <summary>
        /// 请求查询交易账户
        /// </summary>
        F_QRY_ACCOUNT = 112,

        /// <summary>
        /// 查询交易账户响应
        /// </summary>
        F_RSP_ACCOUNT = 113,

        /// <summary>
        /// 请求查询最大报单数量
        /// </summary>
        F_QRY_MAXORDVOL = 114,

        /// <summary>
        /// 查询最大报单数量响应
        /// </summary>
        F_RSP_MAXORDVOL = 115,

        /// <summary>
        /// 请求提交委托
        /// </summary>
        F_REQ_INSERTORDER = 116,

        /// <summary>
        /// 请求提交委托操作
        /// </summary>
        F_REQ_ORDERACTION = 117,

        /// <summary>
        /// 委托回报
        /// </summary>
        F_RTN_ORDER = 118,

        /// <summary>
        /// 成交回报
        /// </summary>
        F_RTN_TRADE = 119,

        /// <summary>
        /// 持仓更新回报
        /// </summary>
        F_RTN_POSITIONUPDATE = 220,
    }
}
