using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader
{
    public static class Utils_TraderCore
    {
        /// <summary>
        /// 在客户端如果Symbol存在则对应的SecurityFamily也存在 客户端加载合约数据时 如果SecurityFamily为空则屏蔽
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FormatPrice(this Position pos, decimal val)
        {
            if (pos.oSymbol != null) return val.ToFormatStr(pos.oSymbol);
            //通过Security获得品种信息 然后通过品种信息来获得格式化输出样式
            SecurityFamily sec = null;
            if (sec == null) val.ToFormatStr();
            return val.ToFormatStr(sec);
        }

        public static string FormatPrice(this Trade fill, decimal val)
        {
            if (fill.oSymbol != null) return val.ToFormatStr(fill.oSymbol);
            SecurityFamily sec = CoreService.BasicInfoTracker.GetSecurity(fill.SecurityCode);
            if (sec == null) return val.ToFormatStr();
            return val.ToFormatStr(sec);
        }
    }
}
