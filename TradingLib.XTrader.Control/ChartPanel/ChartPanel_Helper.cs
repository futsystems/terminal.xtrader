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
using TradingLib.MarketData;

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
        void GetFormat(double max, int spaceLen, bool mainview, ref int sc, ref string fmt)
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
            //默认取2位小数
            if ((vf > 2))
                vf = 2;
            //TODO  根据主图绘制内容 进行调整小数点 默认取合约对应的小数位数 这里逻辑部分需要完善
            //主图则以合约的Precision来定显示格式
            if (mainview)
            {
                vf = pCtrl.Symbol.Precision;
            }
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
        List<MDSession> ParseSession(string sessionStr)
        {
            List<MDSession> list = new List<MDSession>();
            string[] rec = sessionStr.Split(',');
            foreach (var str in rec)
            {
                list.Add(MDSession.Deserialize(str));
            }
            return list;
        }


    }

    
}
