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

        public static Color ColorSize = Color.FromArgb(255, 255, 0);
        public static Font QuoteFont = new Font("Arial", 10f, FontStyle.Bold);

        public static Color ColorLabel = Color.White;
        //public static Profiler Profiler = new Profiler();
    }
}
