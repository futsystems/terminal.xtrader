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
        void GetFormat(double max, int spaceLen, ref int sc, ref string fmt)
        {
            string s1;
            int integralLength, vi, vf;

            vf = 0;
            vi = spaceLen;
            sc = 1;
            //整数部分长度
            s1 = String.Format("{0:d}", Convert.ToInt64(max));//  format('%.0f', [v]);

            integralLength = s1.Length;// length(s1);
            //整数部分大于12位
            if ((integralLength > 12))
            {
                fmt = "{0:f}";
                return;
            }

            //整数部分大于 可显示长度 根据可显示长度 计算显示单位  X1 X10 X万等
            if ((integralLength > spaceLen))
            {
                sc = Convert.ToInt32(Math.Pow(10, integralLength - spaceLen));// trunc(intpower(10, ll - len));
                vf = 0;
                vi = spaceLen;
            }

            if ((integralLength == spaceLen) || (integralLength == spaceLen - 1))
            {
                vf = 0;
                vi = spaceLen;
            }

            if ((integralLength < spaceLen - 1))
            {
                vi = integralLength;
                vf = spaceLen - integralLength;
            }
            if ((vf > 3))
                vf = 3;
//TODO  根据主图绘制内容 进行调整小数点 默认取合约对应的小数位数 这里逻辑部分需要完善
            vf = vf >= pCtrl.Symbol.Precision ? pCtrl.Symbol.Precision : vf;
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
                long datetime = (long)idate * 1000000 + itime;//130101
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


        /*
         *  分时绘制 原先按照交易所 来定linecount 
         *  在多品种交易后,需要按照实际的交易小节来动态计算
         * 
         * */

        /// <summary>
        /// 从交易小节设置信息 解析出Session对象
        /// 93000-113000,130000-150000
        /// </summary>
        /// <param name="sessionStr"></param>
        /// <returns></returns>
        List<Session> ParseSession(string sessionStr)
        {
            List<Session> list = new List<Session>();
            string[] rec = sessionStr.Split(',');
            foreach (var str in rec)
            {
                list.Add(Session.Deserialize(str));
            }
            return list;
        }


    }

    internal class Session
    {
        public Session()
        {
            this.Start = 0;
            this.End = 0;
            this.EndInNextDay = false;
        }
        /// <summary>
        /// 开始
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// 结束时间在第二天
        /// </summary>
        public bool EndInNextDay { get; set; } 

        /// <summary>
        /// 返回区间分钟数
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                if (!this.EndInNextDay)
                {
                    return (FT2FTS(this.End) - FT2FTS(this.Start)) / 60;
                }
                else //结束时间在第二天
                {
                    return 24*60 - (FT2FTS(this.Start) - FT2FTS(this.End)) / 60;
                }
            }
        }


        public static void ParseHMS(int time,out int h, out int m, out int s)
        {
            s = time % 100;
            int m1 = (time - s) / 100;
            m = m1 % 100;
            h = m1 / 100;
        }
        static int FT2FTS(int fasttime)
        {
            int s1 = fasttime % 100;
            int m1 = ((fasttime - s1) / 100) % 100;
            int h1 = (int)((fasttime - (m1 * 100) - s1) / 10000);
            return h1 * 3600 + m1 * 60 + s1;
        }

        /// <summary>
        /// 在某个时刻加上多少秒
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="fasttimespaninseconds"></param>
        /// <returns></returns>
        public static int FTADD(int firsttime, int fasttimespaninseconds)
        {
            int s1 = firsttime % 100;
            int m1 = ((firsttime - s1) / 100) % 100;
            int h1 = (int)((firsttime - m1 * 100 - s1) / 10000);
            s1 += fasttimespaninseconds;
            if (s1 >= 60)
            {
                m1 += (int)(s1 / 60);
                s1 = s1 % 60;
            }
            if (m1 >= 60)
            {
                h1 += (int)(m1 / 60);
                m1 = m1 % 60;
            }
            if (h1 >= 24)
            {
                h1 = h1 % 24;
            }
            int sum = h1 * 10000 + m1 * 100 + s1;
            return sum;


        }


        public static Session Deserialize(string str)
        {
            Session s = new Session();
            string[] rec = str.Split('-');
            s.Start = int.Parse(rec[0]);
            if (rec[1].StartsWith("N"))
            {
                s.EndInNextDay = true;
                string nstr = rec[1].Substring(1);
                s.End = int.Parse(nstr);
            }
            else
            {
                s.End = int.Parse(rec[1]);
            }
            return s;
        }
    }
}
