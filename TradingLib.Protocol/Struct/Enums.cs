﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XLProtocol
{
    public enum XLEnumSeqType:byte
    {
        /// <summary>
        /// 请求
        /// </summary>
        SeqReq=0,
        /// <summary>
        /// 回报
        /// </summary>
        SeqRtn=2,
        /// <summary>
        /// 查询
        /// </summary>
        SeqQry=4,
    }
}
