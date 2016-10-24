using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CStock
{
    public class Constants
    {
        public static Color DebugColor = System.Drawing.Color.White;

        public static Color ColorUp = Color.FromArgb(255, 60, 57);
        public static Color ColorDown = Color.FromArgb(0, 231, 0);
        public static Color ColorEq = Color.Silver;

        public static Color ColorSize = Color.FromArgb(255, 255, 0);
        public static Font QuoteFont = new Font("Arial", 10f, FontStyle.Bold);
        public static Font Font_TradeListSecendLabel = new Font("Arial", 10f);

        public static Font Font_QuoteInfo_SymbolTitle = new Font("Arial", 14f);
        public static Font Font_QuoteInfo_FieldTitle = new Font("Arial", 10f);
        public static Font Font_QuoteInfo_FieldPrice = new Font("Arial", 10f, FontStyle.Bold);

        public static Font Font_QuoteInfo_BigQuote = new Font("Arial", 18f,FontStyle.Bold);
        /// <summary>
        /// 表格
        /// </summary>
        public static Color Color_TableLine = Color.Maroon;
        public static Color Color_ChartFrame = Color.Maroon;

        public static Color ColorLabel = Color.White;
        //public static Profiler Profiler = new Profiler();


        
    }
}
