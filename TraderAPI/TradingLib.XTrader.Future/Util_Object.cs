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

        static Utils_TraderCore()
        {
            np_exchangelist.Add("SHFE");
            np_exchangelist.Add("DCE");
            np_exchangelist.Add("CZCE");
            np_exchangelist.Add("CFFEX");
        }
        static List<string> np_exchangelist = new List<string>();
        /// <summary>
        /// 在客户端如果Symbol存在则对应的SecurityFamily也存在 客户端加载合约数据时 如果SecurityFamily为空则屏蔽
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FormatPrice(this Position pos, decimal val)
        {

            if (pos.oSymbol != null)
            {
                //内盘保留3位
                if (np_exchangelist.Contains(pos.oSymbol.Exchange))
                {
                    return val.ToFormatStr("{0:F3}");
                }
                return val.ToFormatStr(pos.oSymbol);
            }
            return val.ToFormatStr();
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
