using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TradingLib.XTrader.Control
{
    public class UIConstant
    {
        public static int HeaderHeight = 26;
        public static int RowHeight = 24;

        public static System.Drawing.Color LongLabelColor = System.Drawing.Color.Red;
        public static System.Drawing.Color ShortLabelColor = System.Drawing.Color.Blue;

        public static System.Drawing.Color LongSideColor = System.Drawing.Color.Crimson;
        public static System.Drawing.Color ShortSideColor = System.Drawing.Color.LimeGreen;
        public static System.Drawing.Color DefaultColor = System.Drawing.Color.Black;

        public static System.Drawing.Font BoldFont = new Font("微软雅黑", 9, FontStyle.Bold);
        public static System.Drawing.Font DefaultFont = new Font("微软雅黑", 9, FontStyle.Regular);


        public static Font QuoteFont = new Font("Arial", 10f, FontStyle.Bold);

        public static Font LableFont = new Font("宋体", 10.5f);
        public static Font HelpFont = new Font("宋体", 10f);

        public static Color DebugColor = System.Drawing.Color.White;

        public static Color ColorUp = Color.FromArgb(255, 60, 57);
        public static Color ColorDown = Color.FromArgb(0, 231, 0);

        public static Color ColorSize = Color.FromArgb(255, 255, 0);
        public static Color ColorLine = Color.Red;

        public static string DefaultDecimalFormat = "{0:F2}";

        /// <summary>
        /// 查询间隔
        /// </summary>
        public static int QRYINTERVAL = 3;


        //K线图默认
        public static Font AxisFont = new Font("Arial", 9f);


        public static Color GetChangeColor(double change)
        {
            if (change > 0) return LongSideColor;
            if (change < 0) return ShortSideColor;
            return Color.Gray;
        }
    }
}
