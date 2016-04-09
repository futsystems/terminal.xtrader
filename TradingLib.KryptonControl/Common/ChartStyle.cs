using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.KryptonControl
{
    /// <summary>
    /// 绘图样式
    /// </summary>
    public class ChartStyle
    {
        public static  System.Drawing.Color COLOR_UP = System.Drawing.Color.FromArgb(255, 60, 57);
        public static  System.Drawing.Color COLOR_DOWN = System.Drawing.Color.FromArgb(0, 255, 255);
        public static System.Drawing.Color COLOR_SEPERATOR = System.Drawing.Color.FromArgb(255, 60, 57);
        public ChartStyle()
        {
            this.ShowPanelSeperator = true;
            this.RightDrawingSpace = 70;
            this.ScaleAlignment = EnumScaleAlignment.Left;
            this.ScaleDecimalPlace = 2;
            this.ShowXGrid = false;
            this.ShowYGrid = true;
            this.ScaleType = EnumScaleType.Linear;
            this.ShowTitle = true;
            this.ColorUpBody = System.Drawing.Color.Black;
            this.ColorUpBox = ChartStyle.COLOR_UP;
            this.ColorDownBody = ChartStyle.COLOR_DOWN;
            this.ColorDownBox = ChartStyle.COLOR_DOWN;
            this.ColorPanelSeperator = ChartStyle.COLOR_SEPERATOR;
            this.ThreeD = false;

        }


        public System.Drawing.Color ColorUpBody { get; set; }
        public System.Drawing.Color ColorDownBody { get; set; }

        public System.Drawing.Color ColorUpBox { get; set; }

        public System.Drawing.Color ColorDownBox { get; set; }

        public System.Drawing.Color ColorPanelSeperator { get; set; }


        public bool ThreeD { get; set; }
        /// <summary>
        /// 是否显示绘图Panel之间的分割线
        /// </summary>
        public bool ShowPanelSeperator { get; set; }

        /// <summary>
        /// 右侧K线绘图间距
        /// </summary>
        public int RightDrawingSpace { get; set; }

        /// <summary>
        /// Y坐标小数点位置
        /// </summary>
        public int ScaleDecimalPlace { get; set; }

        /// <summary>
        /// 显示X轴向格子
        /// </summary>
        public bool ShowXGrid { get; set; }

        /// <summary>
        /// 显示Y轴向格子
        /// </summary>
        public bool ShowYGrid { get; set; }

        /// <summary>
        /// Y周坐标类型
        /// </summary>
        public EnumScaleType ScaleType { get; set; }

        /// <summary>
        /// Y轴坐标位置
        /// </summary>
        public EnumScaleAlignment ScaleAlignment { get; set; }


        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool ShowTitle { get; set; }
    }
}
