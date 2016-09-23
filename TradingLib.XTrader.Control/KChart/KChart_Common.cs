using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CStock
{
    public partial class TStock
    {


        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref  RECT rect);
        struct RECT
        {
            int left, top, right, bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
        }
        int trunc(double v)
        {
            return Convert.ToInt32(v);
        }
        int trunc(float v)
        {
            return Convert.ToInt32(v);
        }
    }
}
