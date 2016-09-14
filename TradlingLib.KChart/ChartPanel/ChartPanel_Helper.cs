using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;

namespace CStock
{
    public partial class TGongSi
    {
        /// <summary>
        /// 获得输出格式
        /// </summary>
        /// <param name="v">数值 </param>
        /// <param name="len">最大字符宽度</param>
        /// <param name="sc">格式化输出单位1,10,100</param>
        /// <param name="fmt">格式化输出字符串 用于格式化输出数值</param>
        static void GetFormat(double v, int len, ref int sc, ref string fmt)
        {
            string s1;
            int ll, vi, vf;

            vf = 0;
            vi = len;
            sc = 1;
            s1 = String.Format("{0:d}", Convert.ToInt64(v));//  format('%.0f', [v]);

            ll = s1.Length;// length(s1);
            if ((ll > 12))
            {
                fmt = "{0:f}";
                return;
            }
            if ((ll > len))
            {
                sc = Convert.ToInt32(Math.Pow(10, ll - len));// trunc(intpower(10, ll - len));
                vf = 0;
                vi = len;
            }
            if ((ll == len) || (ll == len - 1))
            {
                vf = 0;
                vi = len;
            }
            if ((ll < len - 1))
            {
                vi = ll;
                vf = len - ll;
            }
            if ((vf > 3))
                vf = 3;
            fmt = "{0:f" + vf.ToString() + "}";
            return;
        }






        /// <summary>
        /// 测量文字宽度
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="font"></param>
        /// <param name="s1"></param>
        /// <returns></returns>
        static int TextWidth(Graphics canvas, System.Drawing.Font font, string s1)
        {
            return (int)canvas.MeasureString(s1, font).Width;
        }

        /// <summary>
        /// 获得颜色
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        static Color GetColor(string cs)
        {
            string[] pp = {
      "colorblack", //黑色",//
      "colorblue", //蓝色",//
      "colorgreen", //绿色",//
      "colorcyan", //青色",//
      "colorred", //红色",//
      "colormagenta", //洋红色",//
      "colorbrown", //棕色",//
      "colorlightgray", //淡灰色",//
      "colorgray", //深灰色",//
      "colorlightblue", //淡蓝色",//
      "colorlightgreen", //淡绿色",//
      "colorlightcyan", //淡青色",//
      "colorlightred", //淡红色",//
      "colorlightmagenta", //淡洋红色",//
      "coloryellow", //黄色",//
      "colorwhite" //白色
                };
            Color[] cc = {
                Color.Black,
                Color.Blue,
                Color.Lime,
                Color.Cyan,
                Color.Red,
                Color.Magenta,
                Color.Brown,
                Color.LightGray,
                Color.Gray,
                Color.LightBlue,
                Color.LightGreen,
                Color.LightCyan, 
                Color.Red,
                Color.Magenta,
                Color.Yellow, 
                Color.White
            };
            Color c1;
            int i;
            string s1;
            s1 = cs.ToLower();
            for (i = 0; i < 15; i++)
            {
                if ((s1 == pp[i]))
                {
                    return cc[i];
                }
            }
            if ((s1.Length == 11) && (s1.IndexOf("color") == 0))
            {
                s1 = s1.Substring(5, 6);
                c1 = Color.FromArgb(Convert.ToInt32(s1, 16));
                c1 = Color.FromArgb(c1.B, c1.G, c1.R);
                return c1;
            }
            return Color.White;
        }


        /// <summary>
        /// 在某个位置输出文字
        /// </summary>
        /// <param name="e"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="str"></param>
        private void DrawText(Graphics e, int x, int y, string str)
        {
            e.DrawString(str, font, FBrush, new Point(x, y));
        }

        /// <summary>
        /// 按时间大小 返回时间dt在事件序列中所处的位置
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetIndex(long dt)
        {
            TBian d = GetBian("date");
            TBian t = GetBian("time");
            int datalength = this.RecordCount;
            for (int i = 0; i < this.RecordCount; i++)
            {
                int index = datalength - i - 1;//从数据集后面往前面遍历
                int idate = (int)d.value[index];
                int itime = (int)t.value[index];
                long datetime = (long)idate * 1000000 + itime * 100;//130101
                //如果该时间大于数据集最后
                if (dt > datetime)
                    return index + 1;
                if (dt == datetime)
                    return index;
                if (dt < datetime)
                    continue;
            }
            return 0;
        }

    }
}
