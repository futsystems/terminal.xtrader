using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class MDLoginResponse
    {
        public MDLoginResponse()
        {
            this.LoginSuccess = false;
            this.ErrorCode = string.Empty;
            this.ErrorMessage = string.Empty;
            this.TradingDay = 0;
        }
        /// <summary>
        /// 是否登入成功
        /// </summary>
        public bool LoginSuccess { get; set; }

        /// <summary>
        /// 登入错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 登入错误消息
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// 日期
        /// </summary>
        public int TradingDay { get; set; }
    }
}
