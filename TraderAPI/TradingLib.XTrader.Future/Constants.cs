using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace TradingLib.XTrader.Future
{
    public class Constants
    {
        public const string PRODUCT_INFO = "XTrader.Future";
        public static bool HedgeFieldVisible = false;

        public static Color BorderColor = Color.FromArgb(127, 157, 185);
        public static Color ListMenuSelectedBGColor = Color.FromArgb(51, 153, 255);
        public static System.Drawing.Color LongSideColor = System.Drawing.Color.Crimson;
        public static System.Drawing.Color ShortSideColor = System.Drawing.Color.LimeGreen;

        public static System.Drawing.Color BuyColor = Color.FromArgb(254, 36, 36);
        public static System.Drawing.Color SellColor = Color.FromArgb(0, 127, 0);

        public static int SymbolTitleStyle = 0;

        public static int SymbolNameStyle = 0;


        public static int PageBankStyle = 0;

        public static string BranName = "";

        public static string CashURL1 = "";

        public static string CashURL2 = "";

        public static bool  EnableConfigBank = true;

        public static string QRDescription = "";

        public static string AppServer = "127.0.0.1";

        /// <summary>
        /// 公司网站标题
        /// </summary>
        public static string CompanyTitle = "";

        /// <summary>
        /// 公司网站地址
        /// </summary>
        public static string CompanyUrl = "";


    }
}
