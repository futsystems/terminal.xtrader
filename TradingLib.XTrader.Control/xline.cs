using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CStock
{
    public class XLine
    {
        public string name;
        public int type;// 类型
        public Boolean select;
        public int count;
        public int pointcount; // 所需坐标点数
        public Color color; // 颜色
        public DashStyle linetype;// 线型
        public int linewidth; // 线宽
        public int fontsize; // 字体大小
        public string str; // 字符串
        public int[] fxx = new int[4];// 坐标xx
        public double[] fyy = new double[4]; // 坐标yy

        public XLine(int linestyle)
        {
            SetStyle(linestyle);
        }
        public void SetStyle(int linestyle)
        {
            string[] fs ={
       "无定义", "线段", "直线", "箭头", "射线", "价格通道线",
    "平行直线", "圆弧", "黄金价位线", "黄金目标线", "黄金分割", "百分比线", "波段线", "线形回归带", "延长线形回归带",
    "线形回归线", "周期线", "费波拉契线", "江恩时间序列", "阻速线", "江恩角度线", "矩形", "涨标记", "跌标记",
    "文字注释", "删除画线", "隐藏自画线", "画线信息浏览","外接圆"};

            type = linestyle;
            name = "";
            if ((linestyle > -1) && (linestyle < 28))
                name = fs[linestyle];
            select = false;
            count = 0;
            color = Color.Yellow;
            linetype = DashStyle.Solid;
            linewidth = 1;
            fontsize = 9;
            str = "";
            pointcount = 2;
            for (int i = 0; i < fxx.Length; i++)
            {
                fxx[i] = 0;
                fyy[i] = 0f;
            }
            switch (linestyle)
            {
                case 5:
                case 6:
                case 9:
                case 28:
                    pointcount = 3;
                    break;
                case 17:
                case 18:
                case 22:
                case 23:
                case 24:
                    pointcount = 1;
                    break;
            }
        }
        public void clone(XLine old)
        {
            name = old.name;
            type = old.type;
            select = old.select;
            count = old.count;
            pointcount = old.pointcount;
            color = old.color;
            linetype = old.linetype;
            linewidth = old.linewidth;
            fontsize = old.fontsize;
            str = old.str;
            old.fxx.CopyTo(fxx, 0);
            old.fyy.CopyTo(fyy, 0);
        }

        public struct FPoint
        {
            public int x;
            public int y;
        }

        public struct FRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref FRect Rects, FPoint lpPoint);

        // 求两点(x1,y1) (x2,y2)的延长线在矩形FRECT 边上的交点坐标

        Boolean GetPointAtRect(int x1, int y1, int x2, int y2, Rectangle rect, ref int x3, ref int y3)
        {
            float kk, bb;
            if ((x2 == x1) || (y2 == y1))
                return false;
            kk = (float)(y2 - y1) / (float)(x2 - x1);
            bb = y1 - kk * x1;
            if (y2 < y1)
                y3 = rect.Top;
            else
                y3 = rect.Bottom;
            x3 = Convert.ToInt32((y3 - bb) / kk);
            if (x3 < rect.Left)
            {
                x3 = rect.Left;
                y3 = Convert.ToInt32(kk * x3 + bb);
            }
            if (x3 > rect.Right)
            {
                x3 = rect.Right;
                y3 = Convert.ToInt32(kk * x3 + bb);
            }
            return true;
        }

        // 求点(X3,y3) 到两点(x1,y1) (x2,y2) 所在直线的距离
        float GetPointAtLine(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            float kk, bb;
            float result = 100000;
            if ((x3 < Math.Min(x1, x2)) || (x3 > Math.Max(x1, x2)))
                return result;
            if ((y3 < Math.Min(y1, y2)) || (y3 > Math.Max(y1, y2)))
                return result;
            if (x1 == x2)
                result = Math.Abs(x3 - x1);
            else
            {
                kk = (float)(y2 - y1) / (float)(x2 - x1);
                bb = y1 - kk * x1;
                result = Convert.ToSingle(Math.Abs(kk * x3 - y3 + bb) / (Math.Sqrt(kk * kk + 1)));
            }
            return result;
        }

        //X,Y --  X,Y两轴的坐标
        //M   --  结果变量组数
        //N   --  采样数目
        //A   --  结果参数}
        Boolean CalculateCurveParameter(double[] x, double[] y, int m, int n, double[] a)
        {

            int i, j, k;
            double z, d1, d2, c, p, g, q;
            double[] b;
            double[] t;
            double[] s;

            if (n < 1)
                return false;
            b = new double[n];
            t = new double[n];
            s = new double[n];
            if (m > n)
                m = n;
            for (i = 0; i < m; i++)
                a[i] = 0;
            z = 0;
            b[0] = 1;
            d1 = n;
            p = 0;
            c = 0;
            for (i = 0; i < n; i++)
            {
                p = p + x[i] - z;
                c = c + y[i];
            }
            c = c / d1;
            p = p / d1;
            a[0] = c * b[0];
            if (m > 1)
            {
                t[1] = 1;
                t[0] = -p;
                d2 = 0;
                c = 0;
                g = 0;
                for (i = 0; i < n; i++)
                {
                    q = x[i] - z - p;
                    d2 = d2 + q * q;
                    c = y[i] * q + c;
                    g = (x[i] - z) * q * q + g;
                }
                c = c / d2;
                p = g / d2;
                q = d2 / d1;
                d1 = d2;
                a[1] = c * t[1];
                a[0] = c * t[0] + a[0];
                for (j = 2; j < m; j++)
                {
                    s[j] = t[j - 1];
                    s[j - 1] = -p * t[j - 1] + t[j - 2];
                    if (j >= 3)
                    {
                        for (k = j - 2; k > 0; k--)// downto 1 do
                            s[k] = -p * t[k] + t[k - 1] - q * b[k];
                    }
                    s[0] = -p * t[0] - q * b[0];
                    d2 = 0;
                    c = 0;
                    g = 0;
                    for (i = 0; i < n; i++)
                    {
                        q = s[j];
                        for (k = j - 1; k > -1; k--)// downto 0 do
                            q = q * (x[i] - z) + s[k];
                        d2 = d2 + q * q;
                        c = y[i] * q + c;
                        g = (x[i] - z) * q * q + g;
                    }
                    c = c / d2;
                    p = g / d2;
                    q = d2 / d1;
                    d1 = d2;
                    a[j] = c * s[j];
                    t[j] = s[j];
                    for (k = j - 1; k > -1; k--)// downto 0 do
                    {
                        a[k] = c * s[k] + a[k];
                        b[k] = t[k];
                        t[k] = s[k];
                    }
                }
            }
            return true;
        }
        int trunc(double v)
        {
            return Convert.ToInt32(v);
        }
        int trunc(float v)
        {
            return Convert.ToInt32(v);
        }


        //显示线条
        public Boolean Draw(TGongSi gs, Graphics cv)
        {
            float[] bfb = { 0.250f, 0.333f, 0.50f }; // 百分比线
            float[] hj = { 0.191f, 0.382f, 0.50f, 0.618f, 0.809f, 1 };
            float[] hj1 = { 0.236f, 0.382f, 0.500f, 0.618f, 0.809f, 1.382f, 1.618f, 2, 2.382f, 2.618f };
            int[] jy = { 1, 5, 9, 11, 14, 17, 21, 23, 25, 32, 37, 41, 45, 50, 57, 59, 61, 65, 68, 71, 73, 77, 81 }; // 江恩时间序列


            int i, j, x1, y1, x2, y2, x3, y3;
            int dx, dy, cx, cy;
            Rectangle r1;
            int leftbar;
            float fbsc1, widthscale, hightscale;
            double kk, bb, dd; // jl是当前点到自画直线的距离
            string s1;
            int fs;

            double[] x;
            double[] y;
            double[] a;
            int m1, m2;
            TBian bb1;
            if (gs == null)
                return false;

            r1 = gs.Bounds;
            leftbar = gs.StartIndex;
            widthscale = (float)gs.FScale;
            hightscale = Convert.ToSingle((r1.Bottom - r1.Top) / (gs.MaxValue - gs.MinValue));

            fbsc1 = widthscale;
            if (widthscale > 2)
                fbsc1 = widthscale - 1;
            if (widthscale > 8)
                fbsc1 = widthscale - 2;

            x1 = r1.Left + trunc(widthscale * (fxx[0] - leftbar) + fbsc1 / 2);
            y1 = r1.Bottom - (trunc((fyy[0] - gs.MinValue) * hightscale));

            x2 = r1.Left + trunc(widthscale * (fxx[1] - leftbar) + fbsc1 / 2);
            y2 = r1.Bottom - (trunc((fyy[1] - gs.MinValue) * hightscale));

            x3 = r1.Left + trunc(widthscale * (fxx[2] - leftbar) + fbsc1 / 2);
            y3 = r1.Bottom - (trunc((fyy[2] - gs.MinValue) * hightscale));


            if (select)
            {
                cv.FillRectangle(Brushes.White, x1 - 3, y1 - 3, 6, 6);
                if (pointcount > 1)
                    cv.FillRectangle(Brushes.White, x2 - 3, y2 - 3, 6, 6);
                if (pointcount == 3)
                    cv.FillRectangle(Brushes.White, x3 - 3, y3 - 3, 6, 6);
            }

            Font font = new Font("宋体", fontsize);
            SolidBrush brush = new SolidBrush(color);
            Pen pen = new Pen(color, linewidth);

            fs = font.Height;// fontsize;
            switch (type)
            {
                case 1:// 线段
                    cv.DrawLine(pen, x1, y1, x2, y2);
                    break;
                case 2:// 射线
                case 5:
                case 6:
                    if (x2 == x1)
                    {
                        y1 = r1.Top;
                        x2 = x1;
                        y2 = r1.Bottom;
                        cv.DrawLine(pen, x1, y1, x2, y2);
                        if (count > 1)
                        {
                            if (type == 6)
                                cv.DrawLine(pen, x3, r1.Top, x3, r1.Bottom);

                            if (type == 5)
                            {
                                cv.DrawLine(pen, x3, r1.Top, x3, r1.Bottom);
                                if (x3 > x1)
                                    x3 = x1 - (x3 - x1);
                                else
                                    x3 = x1 + (x1 - x3);
                                cv.DrawLine(pen, x3, r1.Top, x3, r1.Bottom);
                            }
                        }
                    }
                    else
                    {
                        if (y2 == y1)
                        {
                            x1 = r1.Left;
                            x2 = r1.Right;
                            cv.DrawLine(pen, x1, y1, x2, y2);
                            if (count > 1)
                            {
                                if (type == 6)
                                    cv.DrawLine(pen, r1.Left, y3, r1.Right, y3);
                                if (type == 5)
                                {
                                    cv.DrawLine(pen, r1.Left, y3, r1.Right, y3);
                                    if (y3 > y1)
                                        y3 = y1 - (y3 - y1);
                                    else
                                        y3 = y1 + (y1 - y3);
                                    cv.DrawLine(pen, r1.Left, y3, r1.Right, y3);
                                }
                            }
                        }
                        else
                        {
                            kk = (double)(y2 - y1) / (double)(x2 - x1);
                            bb = y1 - kk * x1;
                            y1 = r1.Top;
                            x1 = trunc((y1 - bb) / kk);
                            if (x1 < r1.Left)
                            {
                                x1 = r1.Left;
                                y1 = trunc(kk * x1 + bb);
                            }
                            if (x1 > r1.Right)
                            {
                                x1 = r1.Right;
                                y1 = trunc(kk * x1 + bb);
                            }
                            y2 = r1.Bottom;
                            x2 = trunc((y2 - bb) / kk);
                            if (x2 < r1.Left)
                            {
                                x2 = r1.Left;
                                y2 = trunc(kk * x2 + bb);
                            }
                            if (x2 > r1.Right)
                            {
                                x2 = r1.Right;
                                y2 = trunc(kk * x2 + bb);
                            }
                            cv.DrawLine(pen, x1, y1, x2, y2);
                            if (count > 1)
                            {
                                if ((type == 5) || (type == 6))
                                {
                                    dd = bb;
                                    bb = (y3 - kk * x3);
                                    y1 = r1.Top;
                                    x1 = trunc((y1 - bb) / kk);
                                    if (x1 < r1.Left)
                                    {
                                        x1 = r1.Left;
                                        y1 = trunc(kk * x1 + bb);
                                    }
                                    if (x1 > r1.Right)
                                    {
                                        x1 = r1.Right;
                                        y1 = trunc(kk * x1 + bb);
                                    }
                                    y2 = r1.Bottom;
                                    x2 = trunc((y2 - bb) / kk);
                                    if (x2 < r1.Left)
                                    {
                                        x2 = r1.Left;
                                        y2 = trunc(kk * x2 + bb);
                                    }
                                    if (x2 > r1.Right)
                                    {
                                        x2 = r1.Right;
                                        y2 = trunc(kk * x2 + bb);
                                    }
                                    cv.DrawLine(pen, x1, y1, x2, y2);
                                    if (type == 5)
                                    {
                                        if (bb > dd)
                                            bb = dd - (bb - dd);
                                        else
                                            bb = dd + (dd - bb);
                                        y1 = r1.Top;
                                        x1 = trunc((y1 - bb) / kk);
                                        if (x1 < r1.Left)
                                        {
                                            x1 = r1.Left;
                                            y1 = trunc(kk * x1 + bb);
                                        }
                                        if (x1 > r1.Right)
                                        {
                                            x1 = r1.Right;
                                            y1 = trunc(kk * x1 + bb);
                                        }
                                        y2 = r1.Bottom;
                                        x2 = trunc((y2 - bb) / kk);
                                        if (x2 < r1.Left)
                                        {
                                            x2 = r1.Left;
                                            y2 = trunc(kk * x2 + bb);
                                        }
                                        if (x2 > r1.Right)
                                        {
                                            x2 = r1.Right;
                                            y2 = trunc(kk * x2 + bb);
                                        }
                                        cv.DrawLine(pen, x1, y1, x2, y2);
                                    }
                                }
                            }
                        }

                    }
                    break;
                case 3:// 箭头
                    cv.DrawLine(pen, x1, y1, x2, y2);
                    kk = Convert.ToSingle(Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)));
                    if (kk > 0)
                    {
                        cx = trunc(x2 + 8 * ((x1 - x2) + (y1 - y2) / 2) / kk);
                        cy = trunc(y2 + 8 * ((y1 - y2) - (x1 - x2) / 2) / kk);
                        dx = trunc(x2 + 8 * ((x1 - x2) - (y1 - y2) / 2) / kk);
                        dy = trunc(y2 + 8 * ((y1 - y2) + (x1 - x2) / 2) / kk);
                        cv.DrawLine(pen, x2, y2, cx, cy);
                        cv.DrawLine(pen, x2, y2, dx, dy);
                    }
                    break;
                case 4:// 射线
                    if (x2 == x1)
                    {
                        if (y2 < y1)
                            y2 = r1.Top;
                        else
                            y2 = r1.Bottom;
                        cv.DrawLine(pen, x1, y1, x2, y2);
                    }
                    else
                    {
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                            cv.DrawLine(pen, x1, y1, x3, y3);
                    }
                    break;
                case 7: // 圆弧
                    /*
                    dx = Math.Abs(x2 - x1);
                    cx = Math.Abs(y2 - y1);
                    i = 0;
                    j = 0;
                    if (x2 >= x1)
                        i = x1;
                    else
                        i = (x1 - ((x1 - x2) * 2));
                    if (y2 >= y1)
                        j = (y2 - ((y2 - y1) * 2));
                    else
                        j = y2;
                    if ((cx == 0) || (dx == 0))
                        cv.DrawLine(pen, i, j, i + 2 * dx, j + 2 * cx);
                    else
                        cv.DrawEllipse(pen, i, j, i + 2 * dx, j + 2 * cx);
                    */
                    if (select)
                    cv.DrawLine(pen, x1, y1, x2, y2);
                    dx = Math.Abs(x2 - x1);
                    cx = Math.Abs(y2 - y1);
                    if ((dx > 0) && (cx > 0))
                    {
                        if (y2 > y1)
                        {
                            if (x1 > x2)
                                cv.DrawArc(pen, x1 - dx, y1, 2 * dx, 2 * cx, 90, 180);
                            if (x1 < x2)
                                cv.DrawArc(pen, x1, y1 - cx, 2 * dx, 2 * cx, 0, 180);
                        }
                        else
                        {
                            if (x2 > x1)
                                cv.DrawArc(pen, x1 - dx, y1 - 2 * cx, 2 * dx, 2 * cx, 270, 180);
                            if (x2 < x1)
                                cv.DrawArc(pen, x1 - 2 * dx, y1 - cx, 2 * dx, 2 * cx, 180, 180);
                        }
                    }
                    break;
                case 8: // 黄金价位线
                    cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                    pen.DashStyle = DashStyle.Dot;
                    kk = (gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                    bb = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                    s1 = String.Format("  Base {0:000.00}", bb);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                    for (j = 0; j < 6; j++)
                    {
                        dd = bb * (1 + hj[j]);
                        y2 = r1.Top + trunc((gs.MaxValue - dd) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                            s1 = String.Format("{0:000.0}% {1:0.000}", (1 + hj[j]) * 100, dd);
                            cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                        }
                        dd = bb * (1 - hj[j]);
                        y2 = r1.Top + trunc((gs.MaxValue - dd) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                            s1 = String.Format("{0:000.0}% {1:0.000}", (1 - hj[j]) * 100, dd);
                            cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                        }
                    }
                    break;
                case 9: // 黄金目标线
                    cv.DrawLine(pen, x1, y1, x2, y2);
                    if (count > 1)
                    {
                        cv.DrawLine(pen, x2, y2, x3, y3);
                        cv.DrawLine(pen, r1.Left, y3, r1.Right, y3);
                        kk = Convert.ToSingle(gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                        dd = Convert.ToSingle(gs.MaxValue - (y1 - r1.Top) * kk);
                        bb = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                        dd = bb - dd;
                        bb = Convert.ToSingle(gs.MaxValue - (y3 - r1.Top) * kk);
                        s1 = String.Format("  Base {0:0.00}", bb);
                        cv.DrawString(s1, font, brush, r1.Left + 2, y3 - fs - 1);
                        pen.DashStyle = DashStyle.Dot;
                        for (j = 0; j < 6; j++)
                        {
                            y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * hj[j])) / kk);
                            if ((y2 > r1.Top) && (y2 < r1.Bottom))
                            {
                                cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                                //s1 = String.Format("%5.1f%% %6.3f", hj[j] * 100, (bb + dd * hj[j]));
                                s1 = String.Format("{0:000.0}% {1:0.000}", hj[j] * 100, (bb + dd * hj[j]));
                                cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                            }
                        }
                    }
                    break;
                case 10: // 黄金分割线
                    cv.DrawLine(pen, r1.Left, y1, r1.Right, y1);
                    cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                    kk = Convert.ToSingle((gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top));
                    bb = Convert.ToSingle(gs.MaxValue - (y1 - r1.Top) * kk);
                    s1 = String.Format("  Base {0:##.000}", bb);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y1 - fs - 1);
                    dd = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                    s1 = String.Format("100.0% {0:##.000}", dd);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                    dd = dd - bb;
                    pen.DashStyle = DashStyle.Dot;
                    for (j = 0; j < 10; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * hj1[j])) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                            s1 = String.Format("{0:###.0}% {1:##.000}", hj1[j] * 100, (bb + dd * hj1[j]));
                            cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs);
                        }
                    }
                    break;

                case 11: // 百分比线
                    kk = Convert.ToSingle((gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top));
                    bb = Convert.ToSingle(gs.MaxValue - (y1 - r1.Top) * kk);
                    s1 = String.Format("  Base {0:##.000}", bb);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y1 - fs - 1);
                    cv.DrawLine(pen, r1.Left, y1, r1.Right, y1);
                    dd = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                    s1 = String.Format("100.0% {0:##.000}", dd);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                    cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                    dd = dd - bb;
                    pen.DashStyle = DashStyle.Dot;
                    for (j = 0; j < 3; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * bfb[j])) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                            s1 = String.Format(" {0:####.0}% {1:##.000}", bfb[j] * 100, (bb + dd * bfb[j]));
                            cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs + 1);
                        }
                    }
                    break;
                case 12: // 波段线
                    kk = Convert.ToSingle((gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top));
                    bb = Convert.ToSingle(gs.MaxValue - (y1 - r1.Top) * kk);
                    s1 = String.Format("  Base {0:##.000}", bb);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y1 - fs - 1);
                    cv.DrawLine(pen, r1.Left, y1, r1.Right, y1);
                    dd = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                    s1 = String.Format("100.0% {0:##.000}", dd);
                    cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs - 1);
                    cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                    dd = dd - bb;
                    pen.DashStyle = DashStyle.Dot;
                    for (j = 1; j < 8; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * (0.125 * j))) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            cv.DrawLine(pen, r1.Left, y2, r1.Right, y2);
                            s1 = String.Format(" {0:##.0}% {1:##.000}", 12.5 * j, (bb + dd * 0.125 * j));
                            cv.DrawString(s1, font, brush, r1.Left + 2, y2 - fs + 1);
                        }
                    }
                    break;
                case 13:
                case 14:
                case 15:
                    if (select)
                    {
                        cv.DrawLine(pen, x1, r1.Top, x1, r1.Bottom);
                        cv.DrawLine(pen, x2, r1.Top, x2, r1.Bottom);
                    }
                    if ((gs.showk == 1) || ((gs.showk == -1) && (gs.Main == true) && (gs.ShowFs == false)))
                    {
                        bb1 = gs.check("close");
                        if (bb1 == null)
                            return true;
                        m1 = Math.Min(fxx[0], fxx[1]);
                        m2 = Math.Max(fxx[0], fxx[1]);
                        if (bb1.len < m2)
                            return true;
                        i = m2 - m1 + 1; // 点数
                        x = new double[i];
                        y = new double[i];
                        a = new double[2];
                        for (j = m1; j < m2 + 1; j++)// To M2 Do
                        {
                            x[j - m1] = j;
                            y[j - m1] = bb1.value[j];
                        }
                        if (!CalculateCurveParameter(x, y, 2, i, a))
                            return true;

                        kk = a[0] + a[1] * fxx[0];
                        y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));

                        kk = a[0] + a[1] * fxx[1];
                        y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                        cv.DrawLine(pen, x1, y1, x2, y2);

                        if (type == 14)
                        {
                            if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                            {
                                DashStyle ps1 = pen.DashStyle;// cv.Pen.Style;
                                pen.DashStyle = DashStyle.Dash;// .Dot;
                                cv.DrawLine(pen, x1, y1, x3, y3);
                                pen.DashStyle = ps1;
                            }
                        }

                        if ((type == 13) || (type == 14))
                        {
                            bb1 = gs.check("high");
                            if (bb1 != null)
                            {
                                dd = -1;
                                for (j = m1; j < m2 + 1; j++)// To M2 Do
                                {
                                    kk = a[0] + a[1] * j;
                                    bb = bb1.value[j] - kk;
                                    if (bb > dd)
                                        dd = bb;
                                }
                                if (dd > 0)
                                {
                                    kk = a[0] + dd + a[1] * fxx[0];
                                    y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    kk = a[0] + dd + a[1] * fxx[1];
                                    y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    cv.DrawLine(pen, x1, y1, x2, y2);
                                    if (type == 14)
                                    {
                                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                                        {
                                            DashStyle ps1 = pen.DashStyle;// cv.Pen.Style;
                                            pen.DashStyle = DashStyle.Dash;// .Dot;
                                            cv.DrawLine(pen, x1, y1, x3, y3);
                                            pen.DashStyle = ps1;
                                        }
                                    }
                                }
                            }
                            bb1 = gs.check("low");
                            if (bb1 != null)
                            {
                                dd = -1;
                                for (j = m1; j < m2 + 1; j++)// To M2 Do
                                {
                                    kk = a[0] + a[1] * j;
                                    bb = kk - bb1.value[j];
                                    if (bb > dd)
                                        dd = bb;
                                }
                                if (dd > 0)
                                {
                                    kk = a[0] - dd + a[1] * fxx[0];
                                    y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    kk = a[0] - dd + a[1] * fxx[1];
                                    y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    cv.DrawLine(pen, x1, y1, x2, y2);
                                    if (type == 14)
                                    {
                                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                                        {
                                            DashStyle ps1 = pen.DashStyle;// cv.Pen.Style;
                                            pen.DashStyle = DashStyle.Dash;// .Dot;
                                            cv.DrawLine(pen, x1, y1, x3, y3);
                                            pen.DashStyle = ps1;
                                        }
                                    }

                                }

                            }

                        }
                    }
                    break;
                case 16: // 周期线
                    j = Math.Abs(x2 - x1);
                    if (j > 0)
                    {
                        x1 = Math.Min(x1, x2);
                        while (x1 < r1.Right)
                        {
                            cv.DrawLine(pen, x1, r1.Top, x1, r1.Bottom);
                            x1 = x1 + j;
                        }
                    }
                    cv.DrawLine(pen, x1, r1.Top, x1, r1.Bottom);
                    break;

                case 17: // 费波拉契线
                    j = trunc(widthscale);
                    x3 = 0;
                    y3 = 1;
                    x2 = x1;
                    while (x2 < r1.Right)
                    {
                        cv.DrawLine(pen, x2, r1.Top, x2, r1.Bottom);
                        s1 = String.Format("{0:d}", y3);
                        cv.DrawString(s1, font, brush, x2 + 2, r1.Top);
                        y3 = x3 + y3;
                        x3 = y3 - x3;
                        x2 = x1 + j * (y3 - 1);
                    }
                    break;
                case 18: // 江恩时间序列
                    cx = trunc(widthscale);
                    x2 = x1;
                    for (j = 0; j < 23; j++)
                    {
                        if (x2 > r1.Right)
                            break;
                        cv.DrawLine(pen, x2, r1.Top, x2, r1.Bottom);
                        s1 = String.Format("{0:d}", jy[j]);
                        cv.DrawString(s1, font, brush, x2 + 2, r1.Top);
                        x2 = x1 + cx * (jy[j]);
                    }
                    break;

                case 19: // 阻速线
                    if (x2 > x1)
                    {
                        pen.DashStyle = DashStyle.Dot;
                        y3 = y1 + ((y2 - y1) / 3);
                        s1 = "3:1";
                        cv.DrawString(s1, font, brush, x2, y3);
                        if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                            cv.DrawLine(pen, x1, y1, x3, y3);
                        y3 = y1 + ((y2 - y1) / 3) * 2;
                        s1 = "3:2";
                        cv.DrawString(s1, font, brush, x2, y3);
                        if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                            cv.DrawLine(pen, x1, y1, x3, y3);
                        pen.DashStyle = DashStyle.Solid;
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                            cv.DrawLine(pen, x1, y1, x3, y3);
                    }
                    break;
                case 20:
                    int[] ww ={ 2, 3, 4, 8 };
                    if (x2 > x1)  // 江恩角度线
                    {
                        y3 = r1.Bottom;
                        if (y2 < y1)
                            y3 = r1.Top;
                        x3 = r1.Right;
                        cv.DrawLine(pen, x1, y1, x1, y3);
                        cv.DrawLine(pen, x1, y1, x3, y1);
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                            cv.DrawLine(pen, x1, y1, x3, y3);
                        pen.DashStyle = DashStyle.Dot;
                        for (i = 0; i < 4; i++)
                        {
                            y3 = y1 + ((y2 - y1) / ww[i]);
                            s1 = String.Format("{0:d}:1", ww[i]);// "2:1";
                            cv.DrawString(s1, font, brush, x2, y3);
                            if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                                cv.DrawLine(pen, x1, y1, x3, y3);
                            x3 = x1 + ((x2 - x1) / ww[i]);
                            cv.DrawString(s1, font, brush, x3, y2);
                            if (GetPointAtRect(x1, y1, x3, y2, r1, ref x3, ref y3))
                                cv.DrawLine(pen, x1, y1, x3, y3);
                        }
                    }
                    break;
                case 21: // 矩形
                    cv.DrawRectangle(pen, x1, y1, x2 - x1, y2 - y1);
                    break;
                case 22: // 输出涨图标
                    s1 = "a1";
                    Bitmap icon = (Bitmap)(gs.resources.GetObject(s1));
                    if (icon != null)
                    {
                        icon.MakeTransparent(Color.Black);
                        cv.DrawImage(icon, (x1-icon.Width/2), y1);
                    }
                    break;
                case 23: // 输出跌图标
                    s1 = "a2";
                    Bitmap icon1 = (Bitmap)(gs.resources.GetObject(s1));
                    if (icon1 != null)
                    {
                        icon1.MakeTransparent(Color.Black);
                        cv.DrawImage(icon1, (x1 - icon1.Width / 2), y1);
                    }
                    break;
                case 24: // 字符串
                    cv.DrawString(str, font, brush, x1, y1);
                    break;
                case 28:
                    cv.DrawLine(pen, x1, y1, x2, y2);
                    if (count > 1)
                    {
                        cv.DrawLine(pen, x2, y2, x3, y3);
                        cv.DrawLine(pen, x3, y3, x1, y1);

                        /*
                        kk = (double)(y2 - y1) / (double)(x2 - x1);
                        double x22, y22;
                        x22 = (x2 + x1) / 2;
                        y22 = (y2 + y1) / 2;
                        kk = 1 / kk;
                        bb = y22 - 1 / kk * x22;

                        double kk3, bb3;

                        kk3 = (double)(y3 - y1) / (double)(x3 - x1);
                        double x33, y33;
                        x33 = (x3 + x1) / 2;
                        y33 = (y3 + y1) / 2;
                        kk3 = 1 / kk3;
                        bb3 = y3 - 1 / kk3 * x33;
                        */
                       Point cp=new Point();
                       double r=4;
                       bool kb=cocircle(new Point(x1,y1),new Point(x2,y2),new Point(x3,y3),ref cp,ref r);
                       float rr = (float)r;
                       if (kb)
                       {
                        cv.DrawArc(pen,cp.X-rr,cp.Y-rr,2*rr,2*rr,0,360);
                       }
                        



                    }
                    break;
            }
            //end switch
            return true;
        }
        
        bool cocircle(Point p1, Point p2, Point p3,ref Point q, ref double r)
        {
            double x12 = p2.X - p1.X;
            double y12 = p2.Y - p1.Y;
            double x13 = p3.X - p1.X;
            double y13 = p3.Y - p1.Y;
            double z2 = x12 * (p1.X + p2.X) + y12 * (p1.Y + p2.Y);
            double z3 = x13 * (p1.X + p3.X) + y13 * (p1.Y + p3.Y);
            double d = 2.0 * (x12 * (p3.Y - p2.Y) - y12 * (p3.X - p2.X));
            if (Math.Abs(d) < 3) //共线，圆不存在
                return false;
            q.X =trunc( (y13 * z2 - y12 * z3) / d);
            q.Y =trunc( (x12 * z3 - x13 * z2) / d);
            r = Math.Sqrt((q.X - p1.X) * (q.X - p1.X) + (q.Y - p1.Y) * (q.Y - p1.Y));
            //r = dist(p1, q);
            return true;
        }
        
    
        bool PtInRect(int Left, int Top, int Right, int Bottom, int xx, int yy)
        {
            Rectangle r1 = new Rectangle(Left, Top, Right - Left, Bottom - Top);
            return r1.Contains(xx, yy);
        }

        //取得光标所有自画线位置 
        public int AtLine(TGongSi gs, int xx, int yy)
        {
            float[] bfb = { 0.250f, 0.333f, 0.50f }; // 百分比线
            float[] hj = { 0.191f, 0.382f, 0.50f, 0.618f, 0.809f, 1 };
            float[] hj1 = { 0.236f, 0.382f, 0.500f, 0.618f, 0.809f, 1.382f, 1.618f, 2, 2.382f, 2.618f };
            int[] jy = { 1, 5, 9, 11, 14, 17, 21, 23, 25, 32, 37, 41, 45, 50, 57, 59, 61, 65, 68, 71, 73, 77, 81 }; // 江恩时间序列


            int i, j, x1, y1, x2, y2, x3, y3;
            int dx, dy, cx, cy;
            Rectangle r1;
            int leftbar;
            float fbsc1, widthscale, hightscale;
            double kk, bb, dd; // jl是当前点到自画直线的距离
            string s1;
            int fs;

            double[] x;
            double[] y;
            double[] a;
            int m1, m2;
            TBian bb1;
            float jl = 6;
            int result = -1;

            if (gs == null)
                return result;


            r1 = gs.Bounds;
            leftbar = gs.StartIndex;
            widthscale = (float)gs.FScale;
            hightscale = Convert.ToSingle((r1.Bottom - r1.Top) / (gs.MaxValue - gs.MinValue));

            fbsc1 = widthscale;
            if (widthscale > 2)
                fbsc1 = widthscale - 1;
            if (widthscale > 8)
                fbsc1 = widthscale - 2;

            x1 = r1.Left + trunc(widthscale * (fxx[0] - leftbar) + fbsc1 / 2);
            y1 = r1.Bottom - (trunc((fyy[0] - gs.MinValue) * hightscale));

            x2 = r1.Left + trunc(widthscale * (fxx[1] - leftbar) + fbsc1 / 2);
            y2 = r1.Bottom - (trunc((fyy[1] - gs.MinValue) * hightscale));

            x3 = r1.Left + trunc(widthscale * (fxx[2] - leftbar) + fbsc1 / 2);
            y3 = r1.Bottom - (trunc((fyy[2] - gs.MinValue) * hightscale));

            FPoint p1;
            p1.x = xx;
            p1.y = yy;


            if (PtInRect(x1 - 3, y1 - 3, x1 + 3, y1 + 3, xx, yy))
                result = 1;
            if ((pointcount > 1) && PtInRect(x2 - 3, y2 - 3, x2 + 3, y2 + 3, xx, yy))
                result = 2;
            if ((pointcount > 2) && PtInRect(x3 - 3, y3 - 3, x3 + 3, y3 + 3, xx, yy))
                result = 3;
            if (result > -1)
                return result;

            result = 4;

            Font font = new Font("宋体", fontsize);
            SolidBrush brush = new SolidBrush(color);
            Pen pen = new Pen(color, linewidth);

            fs = font.Height;// fontsize;
            switch (type)
            {
                case 1:// 线段
                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                        return result;
                    break;
                case 2:// 射线
                case 5:
                case 6:
                    if (x2 == x1)
                    {
                        y1 = r1.Top;
                        x2 = x1;
                        y2 = r1.Bottom;
                        if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                            return result;
                        if (count > 1)
                        {
                            if (type == 6)
                                if (GetPointAtLine(x3, r1.Top, x3, r1.Bottom, xx, yy) < jl)
                                    return result;
                            if (type == 5)
                            {
                                if (GetPointAtLine(x3, r1.Top, x3, r1.Bottom, xx, yy) < jl)
                                    return result;
                                if (x3 > x1)
                                    x3 = x1 - (x3 - x1);
                                else
                                    x3 = x1 + (x1 - x3);
                                if (GetPointAtLine(x3, r1.Top, x3, r1.Bottom, xx, yy) < jl)
                                    return result;
                            }
                        }
                    }
                    else
                    {
                        if (y2 == y1)
                        {
                            x1 = r1.Left;
                            x2 = r1.Right;
                            if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                return result;

                            if (count > 1)
                            {
                                if (type == 6)
                                    if (GetPointAtLine(r1.Left, y3, r1.Right, y3, xx, yy) < jl)
                                        return result;
                                if (type == 5)
                                {
                                    if (GetPointAtLine(r1.Left, y3, r1.Right, y3, xx, yy) < jl)
                                        return result;
                                    if (y3 > y1)
                                        y3 = y1 - (y3 - y1);
                                    else
                                        y3 = y1 + (y1 - y3);
                                    if (GetPointAtLine(r1.Left, y3, r1.Right, y3, xx, yy) < jl)
                                        return result;
                                }
                            }
                        }
                        else
                        {
                            kk = (double)(y2 - y1) /(double)(x2 - x1);
                            bb = y1 - kk * x1;
                            y1 = r1.Top;
                            x1 = trunc((y1 - bb) / kk);
                            if (x1 < r1.Left)
                            {
                                x1 = r1.Left;
                                y1 = trunc(kk * x1 + bb);
                            }
                            if (x1 > r1.Right)
                            {
                                x1 = r1.Right;
                                y1 = trunc(kk * x1 + bb);
                            }
                            y2 = r1.Bottom;
                            x2 = trunc((y2 - bb) / kk);
                            if (x2 < r1.Left)
                            {
                                x2 = r1.Left;
                                y2 = trunc(kk * x2 + bb);
                            }
                            if (x2 > r1.Right)
                            {
                                x2 = r1.Right;
                                y2 = trunc(kk * x2 + bb);
                            }
                            if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                return result;
                            if (count > 1)
                            {
                                if ((type == 5) || (type == 6))
                                {
                                    dd = bb;
                                    bb = (y3 - kk * x3);
                                    y1 = r1.Top;
                                    x1 = trunc((y1 - bb) / kk);
                                    if (x1 < r1.Left)
                                    {
                                        x1 = r1.Left;
                                        y1 = trunc(kk * x1 + bb);
                                    }
                                    if (x1 > r1.Right)
                                    {
                                        x1 = r1.Right;
                                        y1 = trunc(kk * x1 + bb);
                                    }
                                    y2 = r1.Bottom;
                                    x2 = trunc((y2 - bb) / kk);
                                    if (x2 < r1.Left)
                                    {
                                        x2 = r1.Left;
                                        y2 = trunc(kk * x2 + bb);
                                    }
                                    if (x2 > r1.Right)
                                    {
                                        x2 = r1.Right;
                                        y2 = trunc(kk * x2 + bb);
                                    }
                                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                        return result;
                                    if (type == 5)
                                    {
                                        if (bb > dd)
                                            bb = dd - (bb - dd);
                                        else
                                            bb = dd + (dd - bb);
                                        y1 = r1.Top;
                                        x1 = trunc((y1 - bb) / kk);
                                        if (x1 < r1.Left)
                                        {
                                            x1 = r1.Left;
                                            y1 = trunc(kk * x1 + bb);
                                        }
                                        if (x1 > r1.Right)
                                        {
                                            x1 = r1.Right;
                                            y1 = trunc(kk * x1 + bb);
                                        }
                                        y2 = r1.Bottom;
                                        x2 = trunc((y2 - bb) / kk);
                                        if (x2 < r1.Left)
                                        {
                                            x2 = r1.Left;
                                            y2 = trunc(kk * x2 + bb);
                                        }
                                        if (x2 > r1.Right)
                                        {
                                            x2 = r1.Right;
                                            y2 = trunc(kk * x2 + bb);
                                        }
                                        if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                            return result;
                                    }
                                }
                            }
                        }

                    }
                    break;
                case 3:// 箭头
                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                        return result;
                    kk = Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));
                    if (kk > 0)
                    {
                        cx = trunc(x2 + 8 * ((x1 - x2) + (y1 - y2) / 2) / kk);
                        cy = trunc(y2 + 8 * ((y1 - y2) - (x1 - x2) / 2) / kk);
                        dx = trunc(x2 + 8 * ((x1 - x2) - (y1 - y2) / 2) / kk);
                        dy = trunc(y2 + 8 * ((y1 - y2) + (x1 - x2) / 2) / kk);
                        if (GetPointAtLine(x2, y2, cx, cy, xx, yy) < jl)
                            return result;
                        if (GetPointAtLine(x2, y2, dx, dy, xx, yy) < jl)
                            return result;
                    }
                    break;
                case 4:// 射线
                    if (x2 == x1)
                    {
                        if (y2 < y1)
                            y2 = r1.Top;
                        else
                            y2 = r1.Bottom;
                        if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                            return result;
                    }
                    else
                    {
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                        {
                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;
                case 7: // 圆弧
                    dx = Math.Abs(x2 - x1);
                    cx = Math.Abs(y2 - y1);
                    i = 0;
                    j = 0;
                    if (x2 >= x1)
                        i = x1;
                    else
                        i = (x1 - ((x1 - x2) * 2));
                    if (y2 >= y1)
                        j = (y2 - ((y2 - y1) * 2));
                    else
                        j = y2;
                    if ((cx == 0) || (dx == 0))
                    {
                        if (GetPointAtLine(i, j, i + 2 * dx, j + 2 * cx, xx, yy) < jl)
                            return result;
                    }
                    else
                    {
                        int ox = i + dx;
                        int oy = j + cx;
                        int clickX = xx - ox;
                        int clickY = yy - oy;
                        kk = (clickX * clickX) / (dx * dx) + (clickY * clickY) / (cx * cx);
                        if ((kk >= 0.8) && (kk <= 1.2))
                            return result;
                    }
                    break;
                case 8: // 黄金价位线
                    if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                        return result;
                    pen.DashStyle = DashStyle.Dot;
                    kk = 1 / hightscale;// (gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                    bb = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                    for (j = 0; j < 6; j++)
                    {
                        dd = bb * (1 + hj[j]);
                        y2 = r1.Top + trunc((gs.MaxValue - dd) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                return result;

                        }
                        dd = bb * (1 - hj[j]);
                        y2 = r1.Top + trunc((gs.MaxValue - dd) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;
                case 9: // 黄金目标线
                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                        return result;
                    if (count > 1)
                    {
                        if (GetPointAtLine(r1.Left, y3, r1.Right, y3, xx, yy) < jl)
                            return result;
                        kk = Convert.ToSingle(gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                        dd = Convert.ToSingle(gs.MaxValue - (y1 - r1.Top) * kk);
                        bb = Convert.ToSingle(gs.MaxValue - (y2 - r1.Top) * kk);
                        dd = bb - dd;
                        bb = Convert.ToSingle(gs.MaxValue - (y3 - r1.Top) * kk);
                        for (j = 0; j < 6; j++)
                        {
                            y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * hj[j])) / kk);
                            if ((y2 > r1.Top) && (y2 < r1.Bottom))
                            {
                                if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                    return result;
                            }
                        }
                    }
                    break;
                case 10: // 黄金分割线
                    if (GetPointAtLine(r1.Left, y1, r1.Right, y1, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                        return result;
                    kk = (gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                    bb = gs.MaxValue - (y1 - r1.Top) * kk;
                    dd = gs.MaxValue - (y2 - r1.Top) * kk;
                    dd = dd - bb;
                    for (j = 0; j < 10; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * hj1[j])) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;

                case 11: // 百分比线
                    if (GetPointAtLine(r1.Left, y1, r1.Right, y1, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                        return result;
                    kk = (gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                    bb = gs.MaxValue - (y1 - r1.Top) * kk;
                    dd = gs.MaxValue - (y2 - r1.Top) * kk;
                    dd = dd - bb;
                    for (j = 0; j < 3; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * bfb[j])) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;
                case 12: // 波段线
                    if (GetPointAtLine(r1.Left, y1, r1.Right, y1, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                        return result;
                    kk = (gs.MaxValue - gs.MinValue) / (r1.Bottom - r1.Top);
                    bb = gs.MaxValue - (y1 - r1.Top) * kk;
                    dd = gs.MaxValue - (y2 - r1.Top) * kk;
                    dd = dd - bb;
                    pen.DashStyle = DashStyle.Dot;
                    for (j = 1; j < 8; j++)
                    {
                        y2 = r1.Top + trunc((gs.MaxValue - (bb + dd * (0.125 * j))) / kk);
                        if ((y2 > r1.Top) && (y2 < r1.Bottom))
                        {
                            if (GetPointAtLine(r1.Left, y2, r1.Right, y2, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;
                case 13:
                case 14:
                case 15:
                    if ((gs.showk == 1) || ((gs.showk == -1) && (gs.Main == true) && (gs.ShowFs == false)))
                    {
                        bb1 = gs.check("close");
                        if (bb1 == null)
                            return -1;
                        m1 = Math.Min(fxx[0], fxx[1]);
                        m2 = Math.Max(fxx[0], fxx[1]);
                        if (bb1.len < m2)
                            return -1;
                        i = m2 - m1 + 1; // 点数
                        x = new double[i];
                        y = new double[i];
                        a = new double[2];
                        for (j = m1; j < m2 + 1; j++)// To M2 Do
                        {
                            x[j - m1] = j;
                            y[j - m1] = bb1.value[j];
                        }
                        if (!CalculateCurveParameter(x, y, 2, i, a))
                            return -1;

                        kk = Convert.ToSingle(a[0] + a[1] * fxx[0]);
                        y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                        kk = Convert.ToSingle(a[0] + a[1] * fxx[1]);
                        y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                        if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                            return result;
                        if (type == 14)
                        {
                            if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                            {
                                if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                    return result;
                            }
                        }

                        if ((type == 13) || (type == 14))
                        {
                            bb1 = gs.check("high");
                            if (bb1 == null)
                            {
                                dd = -1;
                                for (j = m1; j < m2 + 1; j++)// To M2 Do
                                {
                                    kk = Convert.ToSingle(a[0] + a[1] * j);
                                    bb = Convert.ToSingle(bb1.value[j] - kk);
                                    if (bb > dd)
                                        dd = bb;
                                }
                                if (dd > 0)
                                {
                                    kk = a[0] + dd + a[1] * fxx[0];
                                    y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    kk = a[0] + dd + a[1] * fxx[1];
                                    y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                        return result;
                                    if (type == 14)
                                    {
                                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                                        {
                                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                                return result;
                                        }
                                    }
                                }
                            }
                            bb1 = gs.check("low");
                            if (bb1 != null)
                            {
                                dd = -1;
                                for (j = m1; j < m2 + 1; j++)// To M2 Do
                                {
                                    kk = a[0] + a[1] * j;
                                    bb = kk - bb1.value[j];
                                    if (bb > dd)
                                        dd = bb;
                                }
                                if (dd > 0)
                                {
                                    kk = a[0] - dd + a[1] * fxx[0];
                                    y1 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    kk = a[0] - dd + a[1] * fxx[1];
                                    y2 = r1.Bottom - (trunc((kk - gs.MinValue) * hightscale));
                                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                                        return result;
                                    if (type == 14)
                                    {
                                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                                        {
                                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                                return result;
                                        }
                                    }

                                }

                            }
                        }
                    }
                    break;
                case 16: // 周期线
                    j = Math.Abs(x2 - x1);
                    if (j > 0)
                    {
                        x1 = Math.Min(x1, x2);
                        while (x1 < r1.Right)
                        {
                            if (GetPointAtLine(x1, r1.Top, x1, r1.Bottom, xx, yy) < jl)
                                return result;
                            x1 = x1 + j;
                        }
                    }
                    if (GetPointAtLine(x1, r1.Top, x1, r1.Bottom, xx, yy) < jl)
                        return result;
                    break;

                case 17: // 费波拉契线
                    j = trunc(widthscale);
                    x3 = 0;
                    y3 = 1;
                    x2 = x1;
                    while (x2 < r1.Right)
                    {
                        if (GetPointAtLine(x2, r1.Top, x2, r1.Bottom, xx, yy) < jl)
                            return result;
                        y3 = x3 + y3;
                        x3 = y3 - x3;
                        x2 = x1 + j * (y3 - 1);
                    }
                    break;
                case 18: // 江恩时间序列
                    cx = trunc(widthscale);
                    x2 = x1;
                    for (j = 0; j < 23; j++)
                    {
                        if (x2 > r1.Right)
                            break;
                        if (GetPointAtLine(x2, r1.Top, x2, r1.Bottom, xx, yy) < jl)
                            return result;
                        x2 = x1 + cx * (jy[j]);
                    }
                    break;

                case 19: // 阻速线
                    if (x2 > x1)
                    {
                        pen.DashStyle = DashStyle.Dot;
                        y3 = y1 + ((y2 - y1) / 3);
                        if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                        {
                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                return result;
                        }
                        y3 = y1 + ((y2 - y1) / 3) * 2;
                        if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                        {
                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                return result;
                        }
                        pen.DashStyle = DashStyle.Solid;
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                        {
                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                return result;
                        }
                    }
                    break;
                case 20:
                    int[] ww ={ 2, 3, 4, 8 };
                    if (x2 > x1)  // 江恩角度线
                    {
                        y3 = r1.Bottom;
                        if (y2 < y1)
                            y3 = r1.Top;
                        x3 = r1.Right;
                        if (GetPointAtLine(x1, y1, x1, y3, xx, yy) < jl)
                            return result;
                        if (GetPointAtLine(x1, y1, x3, y1, xx, yy) < jl)
                            return result;
                        if (GetPointAtRect(x1, y1, x2, y2, r1, ref x3, ref y3))
                        {
                            if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                return result;
                        }
                        for (i = 0; i < 4; i++)
                        {
                            y3 = y1 + ((y2 - y1) / ww[i]);
                            if (GetPointAtRect(x1, y1, x2, y3, r1, ref x3, ref y3))
                            {
                                if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                    return result;
                            }
                            x3 = x1 + ((x2 - x1) / ww[i]);
                            if (GetPointAtRect(x1, y1, x3, y2, r1, ref x3, ref y3))
                            {
                                if (GetPointAtLine(x1, y1, x3, y3, xx, yy) < jl)
                                    return result;
                            }
                        }
                    }
                    break;
                case 21: // 矩形
                    if (GetPointAtLine(x1, y1, x2, y1, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(x1, y1, x1, y2, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(x2, y2, x2, y1, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(x2, y2, x1, y2, xx, yy) < jl)
                        return result;
                    break;
                case 22: // 输出涨图标
                case 23:
                    s1 = "a1";
                    Bitmap icon = (Bitmap)(gs.resources.GetObject(s1));
                    if (icon != null)
                    {
                        i = icon.Width;
                        x1 = x1 - 8;
                        if ((xx > x1) && (yy > y1) && (xx < (x1 + i)) && (yy < (y1 + i)))
                        {
                            return 1;
                        }
                    }
                    break;
                case 24: // 字符串
                    i = fontsize * str.Length;
                    if ((xx > x1) && (yy > y1) && (xx < (x1 + i)) && (yy < (y1 + i)))
                        return result;
                    break;
                case 28:
                    if (GetPointAtLine(x1, y1, x2, y2, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(x2, y2, x3, y3, xx, yy) < jl)
                        return result;
                    if (GetPointAtLine(x3, y3, x1, y1, xx, yy) < jl)
                        return result;
                    break;
            }
            //end switch
            return -1;
        }



    }

}
