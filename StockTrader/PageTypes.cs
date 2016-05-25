using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockTrader
{
    public class PageTypes
    {

        /// <summary>
        /// 提交委托页面
        /// </summary>
        public static string PAGE_ORDER_ENTRY = "OrderEntry";

        /// <summary>
        /// 取消委托页面
        /// </summary>
        public static string PAGE_ORDER_CANCEL = "OrderCancel";

        /// <summary>
        /// 当日委托
        /// </summary>
        public static string PAGE_ORDER_TODAY = "OrderToday";

        /// <summary>
        /// 历史委托
        /// </summary>
        public static string PAGE_ORDER_HIST = "OrderHist";

        /// <summary>
        /// 当日成交
        /// </summary>
        public static string PAGE_TRADE_TODAY = "TradeToday";

        /// <summary>
        /// 历史成交
        /// </summary>
        public static string PAGE_TRADE_HIST = "TradeHist";

        /// <summary>
        /// 账户持仓
        /// </summary>
        public static string PAGE_ACCOUNT_POSITION = "AccountPosition";

        /// <summary>
        /// 交割单
        /// </summary>
        public static string PAGE_DELIVERY = "Delivery";


        /// <summary>
        /// 修改密码
        /// </summary>
        public static string PAGE_CHANGE_PASS = "ChangePass";


    }
}
