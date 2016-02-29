using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TradingLib.KryptonControl
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

        public static string DefaultDecimalFormat = "{0:F2}";

        /// <summary>
        /// 查询间隔
        /// </summary>
        public static int QRYINTERVAL = 3;
    }
}
