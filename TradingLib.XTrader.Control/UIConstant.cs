using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TradingLib.XTrader.Control
{
    public class UIConstant
    {
        /// <summary>
        /// 报价列表第一列标准合约隐藏
        /// </summary>
        public static bool QuoteViewStdSumbolHidden = false;
        /// <summary>
        /// 盘口头部是否合约是否以博易样式显示
        /// 0为长
        /// 1为短
        /// </summary>
        public static int BoardSymbolTitleStyle = 0;

        public static int BoardSymbolNameStyle = 0;

       

        /// <summary>
        /// 报价合约名称
        /// 0为长恒生1701
        /// 1为短恒生01
        /// </summary>
        public static int QuoteSymbolNameStyle = 1;


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


        /// <summary>
        /// 获得持仓线 盈亏颜色
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        public static Color GetPositionLableProfitColor(double change)
        {
            if (change > 0) return System.Drawing.Color.Crimson;
            if (change < 0) return Color.FromArgb(0,255,255);
            return Color.Gray;
        }
    }
}
