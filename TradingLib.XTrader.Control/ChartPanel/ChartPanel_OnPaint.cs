using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
namespace CStock
{
    /// <summary>
    /// 主绘图函数
    /// 根据显示参数以及公式类型 输出图形
    /// </summary>
    public partial class TGongSi
    {
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);


        List<Session> _sessionList = new List<Session>();
        //string _sessionStr = "210000-N23000,90000-101500,103000-113000,133000-150000";
        string _sessionStr = "180000-N170000";
        int _totalMinutes = 0;
        /// <summary>
        /// 绘制K线
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool OnPaint(Bitmap map)// Graphics canvas)
        {
            
            if (pCtrl == null)
                return false;
            //this.showright = !pCtrl.StartFix;
            //map为主绘图函数传递进来的某个窗口对应的区域 该区域不包含左侧DrawBox和右侧DetailBox区域
            int recordCount = this.RecordCount;
            Color asixBackColor = this.BackColor;

            string[] fj;
            int i, p, fh1, fw, c2;
            int dx, dy, cx, cy, cy1, y1, cury1;
            int h, m, b;
            //int rt, cc;

            string fmt;
            string s1, s2, s3;//, wei;

            //List<TBian> f = new List<TBian>();
            double[] ff1, f11, f12, f13, f14;

            TBian b11, b12, b13, b14;
            TBian vopen, vclose, vhigh, vlow;

            double fbsc1, de, de1, f1, scale, sch, max1, min1, d1;

            double fclose, b1, b2, b3, b4, b5, cmax1, cmin1;
            Boolean bb1, bb2, bb3, bb4, bb5;
            Color cc1, cc2, bgcolor, gbcolor;
            int rectWidth, rectHeight;
            float fcx, fcy, fcy1; //fcx1, 
            Bitmap bm;
            //通过传入的Bitmap获得绘图句柄
            Graphics canvas = Graphics.FromImage(map);

            



            //bool volstick;
            bgcolor = BackColor;// this.BackColor;
            dx = (bgcolor.R + 60) & 0x0ff;
            dy = (bgcolor.G + 60) & 0x0ff;
            cx = (bgcolor.B + 60) & 0x0ff;
            gbcolor = BackColor;// Color.FromArgb(dx, dy, cx);// tcolor(rgb(rg[0], rg[1], rg[2]));
            fh1 = font.Height + 4;//获取文字输出所需高度 作为顶部值输出与底部坐标高度基准值

            toph = fh1;
            both = toph / 2;
            //左右坐标轴默认宽度
            leftYAxisWidth = SpaceWidth;
            rightYAxisWidth = SpaceWidth;
            //int extendexRightSpace = 100;
            //if (pCtrl.StartFix)
            //{
            //    extendexRightSpace = 0;
            //}

            FCurBar = 0;
            FCurValue = NA;

            //获得绘图区域总宽度与总高度
            rectWidth = map.Width;
            rectHeight = map.Height;

            if (showtop == false)
                toph = fh1 / 2;
            if (showbottom)
                both = fh1 + 2;

            //不显示左侧坐标轴 则左侧宽度为0
            if (showleft == false)
                leftYAxisWidth = 0;
            //不显示右侧坐标轴 则右侧宽度为0
            if (showright == false)
                rightYAxisWidth = 0;

            //分时模式 底部宽度为0 底部宽度为1
            if (showfs)
            {
                if (showtop == false)
                    toph = 0;
                if (showbottom == false)
                    both = 1;
            }

            //获得绘制图像区域
            Bounds.X = leftYAxisWidth;
            Bounds.Y = toph;
            Bounds.Width = rectWidth - (leftYAxisWidth - rightYAxisWidth) - 1;
            Bounds.Height = rectHeight - (toph + both);

            weilen = 2;
            if (Symbol != null)
                weilen = Symbol.Precision;

            if (_sessionList.Count == 0)
            {
                foreach (var session in ParseSession(_sessionStr))
                {
                    _sessionList.Add(session);
                }

                _totalMinutes = _sessionList.Sum(s => s.TotalMinutes);
            }
            
            //根据不同的类型 设定分时固定宽度 显示固定个数的分时数据
            int linecount = _totalMinutes;
            //if (marktype == MarkType.MarkGuZhi)
            //    linecount = 270;
            //if (marktype == MarkType.MarkZhengZhoung)
            //    linecount = 225;
            //if (marktype == MarkType.MarkShangHai)
            //    linecount = 556;


            //EndIndex = 0;
            //计算分时每个数据占用的宽度
            if (showfs == true)
            {
                if (fsfull == false)
                    FScale = (double)(rectWidth - leftYAxisWidth - rightYAxisWidth) / (double)(linecount * days);
                if ((recordCount > 0) && fsfull)
                    FScale = (rectWidth - leftYAxisWidth - rightYAxisWidth) / recordCount;
            }


            rectWidth = rectWidth - rightYAxisWidth;



            ////显示股票名称和公式名称
            fw = 0;


            #region 填充背景色 如果当前窗口选中则左侧Y轴区域填充阴影背景
            //填充绘图底色
            FBrush.Color = BackColor;
            canvas.FillRectangle(FBrush, 0, 0, map.Width, map.Height);
            if (curgs && showleft)
            {
                //i = 0;
                //if (showfs && (main == false) && (showbottom))
                //    i = both;
                asixBackColor = System.Drawing.Color.FromArgb(24, 24, 24);//暗灰阴影背景颜色
                FBrush.Color = asixBackColor;
                canvas.FillRectangle(FBrush, 1, 1, leftYAxisWidth - 1, rectHeight - 1);
                FBrush.Color = BackColor;
            }
            #endregion

            #region 显示框架线 注:整个绘图区域的外部一圈边框由多个指标窗口图拼接完成整图后统一最后绘制

            //显示K线框架
            #region BarView框架
            if (showfs == false)
            {
                if (yStr.Length == 0)
                {
                    pen.Color = LineColor;
                    canvas.DrawLine(pen, leftYAxisWidth, 0, leftYAxisWidth, rectHeight);//LeftYAsixLine
                    if (ShowBottom)
                    {
                        canvas.DrawLine(pen, leftYAxisWidth, rectHeight - both, rectWidth, rectHeight - both);//BottomCalendarTopLine
                    }

                    //绘制X轴 虚线
                    pen.DashStyle = DashStyle.Dot;
                    pen.Color = LineColor;
                    dy = Math.Min(Convert.ToInt32((rectHeight - toph - both) / (fh1 + 6)) + 1, 4);//默认分为4根虚线
                    sch = (rectHeight - toph - both) / dy;//净宽/根数 = 步长
                    for (i = 0; i <= dy; i++)
                    {
                        y1 = toph + Convert.ToInt32(i * sch);
                        canvas.DrawLine(pen, leftYAxisWidth - 3, y1, rectWidth, y1);//YAsixGridLine
                    }
                }
                else //绘制顶部与底部2根实线
                {
                    pen.Color = LineColor;
                    canvas.DrawLine(pen, leftYAxisWidth, 0, leftYAxisWidth, rectHeight);
                    if (toph > 0)
                    {
                        canvas.DrawLine(pen, leftYAxisWidth, toph, rectWidth, toph);
                    }
                    if (both > 0)
                    {
                        canvas.DrawLine(pen, leftYAxisWidth, rectHeight - both, rectWidth, rectHeight - both);
                    }

                }
            }
            #endregion


            //显示分时框架
            #region IntraView 框架
            if (showfs == true)
            {
                FBrush.Color = LineColor;
                pen.Color = LineColor;
                if (showtop)
                {
                    canvas.DrawLine(pen, leftYAxisWidth, toph, rectWidth, toph);//IntraViewTopLine
                }
                if (showbottom)
                {
                    canvas.DrawLine(pen, 0, rectHeight - both, rectWidth + leftYAxisWidth, rectHeight - both);//IntraViewCalendarTopLine
                }

                int hh, mm, ss;
                if (days == 1) //只有一天
                {
                    sch = Convert.ToDouble(rectWidth - leftYAxisWidth) / linecount;
                    
                    int spaceMinute = 0;
                    //自适应间距计算
                    while (spaceMinute * sch < 50)
                    {
                        if (spaceMinute < 60)
                        {
                            spaceMinute += 30;
                        }
                        else
                        {
                            spaceMinute += 60;
                        }
                    }
                    
                    int cursorMin = 0;//当前累计分钟数
                    int finishMin = 0;//绘制完毕的Session分钟数累加
                    int sessionMin = 0;//在某个Session内移动的分钟数
                    for (int j = 0; j < _sessionList.Count;j++ )
                    {
                        Session session = _sessionList[j];
                        //开盘
                        if (j == 0)
                        {
                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32(cursorMin * sch), toph, leftYAxisWidth + Convert.ToInt32(cursorMin * sch), rectHeight - both);//IntraviewXAsixGridLine
                            Session.ParseHMS(session.Start, out hh, out mm, out ss);
                            s1 = String.Format("{0:d2}", hh) + ":" + String.Format("{0:d2}", mm);
                            canvas.DrawString(s1, font, FBrush, leftYAxisWidth + Convert.ToInt32(cursorMin * sch - canvas.MeasureString(s1, font).Width / 2), rectHeight - both + 2);//IntraViewCalendarTxt
                        }

                        //获得Session开始时间 判定分钟数是否处于整数 否则执行偏移
                        Session.ParseHMS(session.Start, out hh, out mm, out ss);
                        if (mm != 0 && mm != 30)
                        {
                            sessionMin += 15;//15分/45分开盘 偏移15
                            pen.DashStyle = DashStyle.Dash;
                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32((finishMin + sessionMin) * sch), toph, leftYAxisWidth + Convert.ToInt32((finishMin + sessionMin) * sch), rectHeight - both);//IntraviewXAsixGridLine
                            pen.DashStyle = DashStyle.Solid;
                        }

                        while (sessionMin < session.TotalMinutes)
                        {
                            //进入session后 直接进行偏移绘制区间中的竖线 区间第一条线由上个区间的最后一条来替代 区间首位相连
                            sessionMin += spaceMinute;
                            cursorMin = finishMin + sessionMin;//完成Session的分钟数累加 + 当前Session的分钟数
                            //比如11：45收盘 30分区间 则最后一条线会直接绘制成12点,这个不是我们希望的 因此跳过
                            if (sessionMin > session.TotalMinutes)
                                continue;

                            Session.ParseHMS(Session.FTADD(session.Start, sessionMin * 60), out hh, out mm, out ss);
                            if (mm == 30)
                            {
                                pen.DashStyle = DashStyle.Dash;
                            }
                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32(cursorMin * sch), toph, leftYAxisWidth + Convert.ToInt32(cursorMin * sch), rectHeight - both);//IntraviewXAsixGridLine
                            pen.DashStyle = DashStyle.Solid;
                            
                            s1 = String.Format("{0:d2}", hh) + ":" + String.Format("{0:d2}", mm);
                            
                            canvas.DrawString(s1, font, FBrush, leftYAxisWidth + Convert.ToInt32(cursorMin * sch - canvas.MeasureString(s1, font).Width / 2), rectHeight - both + 2);//IntraViewCalendarTxt
                            
                        }

                        finishMin += session.TotalMinutes;
                        //区间末尾直接越过 没有绘制 比如23:45分收盘 但是spaceMinute为30分钟 因此区间结束线 被越过
                        if (sessionMin > session.TotalMinutes)
                        {
                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32(finishMin * sch), toph, leftYAxisWidth + Convert.ToInt32(finishMin * sch), rectHeight - both);//IntraviewXAsixGridLine
                            Session.ParseHMS(Session.FTADD(session.Start, session.TotalMinutes * 60), out hh, out mm, out ss);
                            s1 = String.Format("{0:d2}", hh) + ":" + String.Format("{0:d2}", mm);
                            canvas.DrawString(s1, font, FBrush, leftYAxisWidth + Convert.ToInt32(finishMin * sch - canvas.MeasureString(s1, font).Width / 2), rectHeight - both + 2);//IntraViewCalendarTxt

                        }
                        //if (j == _sessionList.Count - 1)
                        //{ 
                        //    //绘制Session结束
                        //}

                        sessionMin = 0;
                    }
                }
                else
                {
                    sch = Convert.ToDouble(rectWidth - leftYAxisWidth) / days;
                    for (i = 0; i < days + 1; i++)
                    {

                        canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32(i * sch), toph, leftYAxisWidth + Convert.ToInt32(i * sch), rectHeight - both);
                        if (i > 0)
                        {
                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32(i * sch - sch / 2), toph, leftYAxisWidth + Convert.ToInt32(i * sch - sch / 2), rectHeight - both);
                        }
                        if (i < days)
                        {
                            dy = days - i - 1;
                            if (dy > 0)
                                s1 = "前" + dy.ToString() + "天";
                            else
                                s1 = "当天";

                            int sw = (int)canvas.MeasureString(s1, font).Width;
                            int lx = leftYAxisWidth + Convert.ToInt32(i * sch - sw / 2);
                            canvas.DrawString(s1, font, FBrush, lx, rectHeight - both + 2);
                        }

                    }
                }
            }
            #endregion

            //每个指标窗口底部一条贯穿左右的实线，最后一个窗口改线会被外框或者底部时间栏覆盖，因此修改颜色后 分时图标显示不出该线
            pen.Color = LineColor;
            pen.DashStyle = DashStyle.Solid;
            pen.Width = 1;
            //画笔占1个像素的位置,如果绘制在map.Height则无法显示该线
            canvas.DrawLine(pen, 0, map.Height-1, map.Width, map.Height-1);//WindowBottomLine
            #endregion

            #region 显示指标窗口头部信息 用于输出合约代码,名称,指标值
            if (showtop)
            {
                string topTitle = !string.IsNullOrEmpty(this.pCtrl.Symbol.Name) ? this.pCtrl.Symbol.Name : this.pCtrl.Symbol.Symbol;
                if (showfs == false)
                {
                    if (!string.IsNullOrEmpty(_cycleTitle))
                        topTitle = topTitle + "<" + _cycleTitle + ">";
                    if (ftechname.Length > 0)
                        topTitle = topTitle + " " + ftechname.ToUpper() + " ";
                }

                fw = (int)canvas.MeasureString(topTitle, font).Width;
                pen.DashStyle = DashStyle.Solid;

                if (Focused)
                    canvas.DrawString(topTitle, font, Brushes.Lime, leftYAxisWidth + 2, 2);
                else
                    canvas.DrawString(topTitle, font, Brushes.Gray, leftYAxisWidth + 2, 2);
                if (showfs)
                {
                    f1 = GetLastValue("close");
                    if ((f1 != NA) && (prevclose != NA))
                    {
                        topTitle = string.Format("{0:F} {1:F}%", f1, Math.Abs(f1 - prevclose) * 100 / prevclose);
                        if (prevclose > f1)
                            FBrush.Color = Color.Green;
                        else
                            FBrush.Color = Color.Red;
                        canvas.DrawString(topTitle, font, FBrush, leftYAxisWidth + 5 + fw, 2);
                        fw = fw + (int)canvas.MeasureString(topTitle, font).Width + 5;
                    }
                }
            }
            #endregion

            #region 计算可绘制Bar数量,计算StartIndex EndIndex 以及curx cury以及当前所在CurBar
            //此处FScale由外部计算后赋值给对象属性,可以修改成通过StartIndex Endindex来控制，通过绘制Bar个数来自动计算FScale,放大缩小都是通过StartIndex EndIndex的调整进行
            //如果没有数据 则直接返回
            if ((recordCount ==0) || (FScale < 0)) //没有数据返回　
                return false;

            //this.FScale = (double)(rectWidth - leftYAxisWidth) / (this.EndIndex - this.StartIndex);

            //计算当前绘Bar区域可显示记录数量
            FCurWidth = Convert.ToInt32((rectWidth - leftYAxisWidth) / FScale);
            if (showfs == true)
            {
                FCurWidth = linecount * days;
                StartIndex = 0;
            }

            //整理StartIndex 左侧显示数据的位置
            if (StartIndex >= recordCount)
                StartIndex = recordCount - 1;
            if (StartIndex < 0)
                StartIndex = 0;
            //计算EndIndex 右侧显示数据的位置
            EndIndex = Math.Min(StartIndex + FCurWidth, recordCount);

            //if (training) //训练模式 
            //{
            //    if (TrainEnd > RecordCount)
            //        TrainEnd = RecordCount;
            //    if (TrainEnd < 0)
            //        TrainEnd = 0;
            //    if (StartIndex >= TrainEnd)
            //    {
            //        StartIndex = Math.Max(0, TrainEnd - FCurWidth + 1);
            //        rt = TrainEnd;
            //    }
            //    if (rt > TrainEnd)
            //        rt = TrainEnd;
            //}

            //EndIndex = rt;
            FCurBar = 0;
            //cury为-2 则curx为当前Bar的位置
            if (cury == -2)
            {
                FCurBar = curx;
                if (!showfs)
                {
                    if (curx < StartIndex)
                        curx = StartIndex;
                    if (curx > EndIndex)
                        curx = EndIndex - 1;
                    FCurBar = curx;
                    curx = leftYAxisWidth + Convert.ToInt32((curx - StartIndex) * FScale + FScale / 2);
                    if (FScale > 8)
                        curx -= 1;
                }
                else
                    curx = leftYAxisWidth + Convert.ToInt32((curx - StartIndex) * FScale);
            }
            else
            {
                if (showfs)
                    FCurBar = (int)((curx - leftYAxisWidth) / FScale);// Convert.ToInt32((curx - leftYAxisWidth) / FScale);
                else
                    FCurBar = StartIndex + (int)((curx - leftYAxisWidth) / FScale);
                if (FCurBar < 0)
                    FCurBar = 0;
                if (FCurBar > recordCount)
                    FCurBar = recordCount - 1;
            }


            //计算K线宽度,K线间隔最小2，或者是Bar宽的0.2
            fbsc1 = FScale - Math.Max(2, (int)(FScale * 0.2));
            #endregion


            //处理公式列表(第1个为主要公式,后面为叠加公式
            List<TBian> args = new List<TBian>();
            string drawline = string.Empty;
            string[] drawSplit;
            string drawStr = string.Empty;
            string drawStyle = string.Empty;
            
            string drawArg;
            string drawType;
            string[] drawArgSplit;

            for (int t = 0; t < TechList.Count; t++)
            {
                #region 准备指标所需要用的数据集,计算该指标的最大最小值,计算数值输出样式以及绘图比例 主要用于数据准备
                args.Clear();
                //获得高开低收数据序列
                if ((t == 0)&&(showfs == false))
                {
                    if (((showk == -1) && (main == true)) || (showk == 1)) //是主图加入k线
                    {
                        b11 = GetBian("open");
                        if ((b11 != null))
                            args.Add(b11);
                        b11 = GetBian("close");
                        if ((b11 != null))
                            args.Add(b11);
                        b11 = GetBian("high");
                        if (b11 != null)
                            args.Add(b11);
                        b11 = GetBian("low");
                        if ((b11 != null))
                            args.Add(b11);
                    }
                }
                max1 = NA;
                min1 = -NA;
                if ((fsall) && (prevclose != NA) && (showfs == true) && (main == true))
                {
                    min1 = prevclose;
                    max1 = prevclose;
                }

                f1 = 0.0;
                CurTech = TechList[t];
                //分解指标输出字符串
                for (i = 0; i < CurTech.outline.Count; i++)
                {
                    //SYLINE=均价线::COLORYELLOW
                    drawline = TechList[t].outline[i];
                    if (drawline.IndexOf("STICKLINE") > -1)
                        drawStyle = "";
                    drawSplit = Regex.Split(drawline, "::");
                    if (drawSplit.Length > 1)
                    {
                        drawStr = drawSplit[0];
                        drawStyle = drawSplit[1];
                        
                    }
                    if (drawStyle.IndexOf("nodraw") > -1)
                        continue;
                    drawSplit = drawStr.Split('=');
                    drawType = string.Empty;
                    drawArg = string.Empty;
                    if (drawSplit.Length > 1)
                    {
                        drawType = drawSplit[0];// fenjie(s1, "=", 1);//s1为绘图类型 s2为参数列表
                        drawArg = drawSplit[1];// fenjie(s1, "=", 2);
                    }
                    //if (wei.IndexOf("VOLSTICK") > -1)
                    //    volstick = true;
                    drawArgSplit = drawArg.Split(',');//分解参数列表
                    //根据不同的Line类型,获得对应参数起始位置,并检查这些位置对应参数的数据 更新最大最小值或放入到参数列表
                    dx = -1;
                    dy = -1;
                    if ((drawType == "SYLINE") || (drawType == "BTX") || (drawType == "TWR") || (drawType == "PARTLINE") || (drawType == "COLORSTICK"))
                    {
                        dx = 1;
                        dy = 1;
                    }
                    if ((drawType == "DRAWICON") || (drawType == "DRAWTEXT") || (drawType == "BOX"))
                    {
                        dx = 2;
                        dy = 2;
                        continue;
                    }
                    if (drawType == "DRAWKLINE")
                    {
                        dx = 1;
                        dy = 4;
                    }
                    if (drawType == "STICKLINE")
                    {
                        dx = 2;
                        dy = 3;
                    }
                    if (dx == -1)
                        continue;
                    for (p = dx; p <= dy; p++)
                    {
                        string arg = drawArgSplit[p - 1];
                        //检查如果是常数则比较最大最小值,如果是数值序列则加入到参数列表
                        if (teststr(arg, ref f1))
                        {
                            if (f1 > max1)
                                max1 = f1;
                            if (f1 < min1)
                                min1 = f1;
                        }
                        else
                        {
                            b11 = check(arg);
                            if (b11 != null)
                                args.Add(b11);
                        }
                    }
                }

                //没有数据 则不输出图形
                if ((args.Count == 0) && (max1 == NA) && (min1 == -NA))
                    break;

                //遍历数据集f(包含了OHLC) 同时根据显示K线的左侧与右侧坐标 获得最大和最小值
                for (p = 0; p < args.Count; p++)
                {
                    for (i = StartIndex; i < EndIndex; i++)
                    {
                        if (args[p].value[i] == NA)
                            continue;
                        if (args[p].value[i] > max1)
                            max1 = args[p].value[i];
                        if (args[p].value[i] < min1)
                            min1 = args[p].value[i];
                    }
                }

                //if ((volstick == true) && (min1 > 0.0))
                //    min1 = 0.0;
                if ((showfs == true) && (main == true) && (prevclose != NA))
                {
                    if (!fsall)
                    {
                        sch = Math.Abs(max1 - prevclose);
                        if (sch < Math.Abs(min1 - prevclose))
                            sch = Math.Abs(min1 - prevclose);
                        if (prevclose - sch < min1)
                            min1 = prevclose - sch;
                        if (prevclose + sch > max1)
                            max1 = prevclose + sch;
                    }
                }
                if (max1 == NA)
                    max1 = 0;
                if (min1 == NA)
                    min1 = 0;
                if (min1 == -NA)
                    min1 = 0;

                //避免最大值和最小值相等
                if (Convert.ToInt64(max1 * 1000) == Convert.ToInt64(min1 * 1000))
                {
                    max1 += 0.001;
                    min1 -= 0.001;
                }

                if (main && showfs && percent10 && (prevclose != NA)) //按10%显示
                {
                    double pv = 0.1;
                    if (pCtrl.Symbol.Name.IndexOf("st") > -1)
                        pv = 0.05;
                    if ((prevclose * (1 + pv) > max1) && (prevclose * (1 - pv) < min1))
                    {
                        max1 = prevclose * 1.1;
                        min1 = prevclose * 0.9;
                    }
                }
                


                //取得输出的格式
                c2 = 0;
                fmt = "";
                //根据可以输出的字符长度，判断是否需要增加单位设置 成交量 X万 c2为1 10 100等 用于单位显示数值
                GetFormat(max1, SpaceWidth / TextWidth(canvas, font, "8"), ref  c2, ref fmt);
                scale = (rectHeight - toph - both) / (max1 - min1);//绘图尺寸与公式值比例

                #endregion

                #region 第一个指标 需要完善框架线上的的坐标值等信息 如果是主图则需要绘制K线
                if (t == 0)
                {
                    MaxValue = max1;
                    MinValue = min1;

                    #region 绘制K线水平坐标值
                    if (showfs == false) //显示K线水平框架
                    {
                        //绘制Y轴坐标值
                        if (yStr.Length == 0)
                        {
                            dy = Math.Min((rectHeight - toph - both) / (fh1 + 6) + 1, 4);
                            sch = Convert.ToDouble(rectHeight - toph - both) / Convert.ToDouble(dy);//计算步长
                            for (i = 0; i <= dy; i++)
                            {
                                f1 = (max1 - i * sch / scale) / c2;//屏幕长度/比例 = 数值
                                if (i == dy)
                                    f1 = min1 / c2;
                                if (i == 0)
                                    f1 = max1 / c2;
                                //根据对应的坐标值正负设定绘制颜色
                                if (f1 > 0)
                                    FBrush.Color = Color.Red;
                                else if (f1 == 0)
                                    FBrush.Color = Color.White;
                                else
                                    FBrush.Color = Color.Green;

                                f1 = Math.Abs(f1);
                                s1 = String.Format(fmt, f1);
                                y1 = toph + Convert.ToInt32(i * sch)-6;//坐标文字位置 用于对中 YAsixGridLine
                                if (i == dy)
                                    y1 -= 6; 
                                if ((y1 < 0))
                                    y1 = 0; 
                                //显示坐标值
                                if (showleft)
                                    canvas.DrawString(s1, font, FBrush, leftYAxisWidth - TextWidth(canvas, font, s1) - 2, y1);
                                if (showright)
                                    canvas.DrawString(s1, font, FBrush, rectWidth + 2, y1);
                            }
                            //如果数值过大有对应单位 则在坐标轴上输出单位信息
                            if (c2 > 1)
                            {
                                pen.DashStyle = DashStyle.Solid;
                                if ((c2 < 10000))
                                    s1 = string.Format("×{0:d}", c2);
                                else
                                    s1 = string.Format("×{0:d}万", c2 / 10000);
                                pen.Color = LineColor;
                                FBrush.Color = asixBackColor;
                                canvas.FillRectangle(FBrush, 1, rectHeight - fh1 - 2, leftYAxisWidth - 2, fh1);
                                canvas.DrawRectangle(pen, 1, rectHeight - fh1 - 2, leftYAxisWidth - 2, fh1);
                                FBrush.Color = LineColor;// Color.Red;
                                canvas.DrawString(s1, font, FBrush, (leftYAxisWidth - (int)canvas.MeasureString(s1, font).Width) / 2, rectHeight - fh1);
                            }
                        }
                        else
                        {
                            string[] vs = yStr.Split('_');
                            pen.DashStyle = DashStyle.Dot;
                            pen.Color = LineColor;
                            FBrush.Color = Color.Red;
                            for (i = 0; i < vs.Length; i++)
                            {
                                d1 = Convert.ToDouble(vs[i]);
                                if ((d1 > min1) && (d1 < max1))
                                {
                                    y1 = Convert.ToInt32(rectHeight - (both + (float)((d1 - min1) * scale)));
                                    canvas.DrawLine(pen, leftYAxisWidth, y1, rectWidth - rightYAxisWidth, y1);
                                    s1 = String.Format(fmt, d1);
                                    if (showleft)
                                        canvas.DrawString(s1, font, FBrush, leftYAxisWidth - TextWidth(canvas, font, s1) - 2, y1 - 6);
                                    if (showright)
                                        canvas.DrawString(s1, font, FBrush, rectWidth + 2, y1);
                                }
                            }
                        }
                        if (yValue.Length > 0)
                        {
                            string[] vs = yValue.Split('_');
                            pen.DashStyle = DashStyle.Dot;
                            pen.Color = Color.White;
                            for (i = 0; i < vs.Length; i++)
                            {
                                if (vs[i].Length > 0)
                                {
                                    d1 = Convert.ToDouble(vs[i]);
                                    if ((d1 > min1) && (d1 < max1))
                                    {
                                        y1 = Convert.ToInt32(rectHeight - (both + (float)((d1 - min1) * scale)));
                                        canvas.DrawLine(pen, leftYAxisWidth, y1, rectWidth - rightYAxisWidth, y1);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 绘制分时主图 坐标值
                    if ((showfs == true) && (main == true)) //显示分时主图水平框架
                    {
                        y1 = Math.Min((rectHeight - toph - both) / (fh1 + 6) + 1, 14);
                        if ((y1 % 2) == 1)
                            y1 -= 1;
                        sch = Convert.ToDouble(rectHeight - toph - both) / Convert.ToDouble(y1);
                        de1 = prevclose;
                       // FBrush.Color = Color.White;
                        for (i = 0; i < y1; i++)
                        {
                            pen.Color = LineColor;
                            canvas.DrawLine(pen, leftYAxisWidth, toph + Convert.ToInt32(i * sch), rectWidth, toph + Convert.ToInt32(i * sch));
                            f1 = (max1 - (i * sch) / scale) / c2;
                            if ((de1 != NA) && (de1 != 0.0))
                            {
                                Int64 dx1 = Convert.ToInt32((f1 - de1) * 10000 / de1);
                                if (dx1 > 0)
                                    FBrush.Color = Color.Red;
                                if (dx1 == 0)
                                    FBrush.Color = Color.White;
                                if (dx1 < 0)
                                    FBrush.Color = Color.Green;
                            }
                            else
                                FBrush.Color = Color.White;

                            s1 = string.Format(fmt, f1);
                            if (showleft)
                                canvas.DrawString(s1, font, FBrush, leftYAxisWidth - (int)canvas.MeasureString(s1, font).Width - 1, toph + Convert.ToInt32(i * sch) - 6);
                            if (showright)
                            {
                                if ((de1 != NA) && (de1 != 0.0))
                                {
                                    de = Math.Abs(f1 - de1) * 100 / de1;
                                    s1 = string.Format("{0:f2}%", de);
                                    canvas.DrawString(s1, font, FBrush, rectWidth + 4, toph + Convert.ToInt32(i * sch) - 6);
                                }
                                else
                                    canvas.DrawString(s1, font, FBrush, rectWidth + 1, toph + Convert.ToInt32(i * sch) - 6);
                            }
                        }
                    }

                    if ((showfs == true) && (main == false))//显示分时副图水平框架
                    {
                        pen.DashStyle = DashStyle.Dot;
                        FBrush.Color = LineColor;// Color.Red;// Color.Yellow;
                        pen.Color = LineColor;
                        y1 = Math.Min((rectHeight - toph - both) / (fh1 + 6) + 1, 7);
                        sch = (rectHeight - toph - both) / y1;
                        for (i = 0; i < y1; i++)
                        {
                            canvas.DrawLine(pen, leftYAxisWidth, toph + Convert.ToInt32(i * sch), rectWidth, toph + Convert.ToInt32(i * sch));
                            f1 = Math.Abs(max1 - i * sch / scale) / c2;
                            s1 = string.Format(fmt, f1);
                            cx = Math.Max(toph + Convert.ToInt32(i * sch) - 6, 0);
                            canvas.DrawString(s1, font, FBrush, leftYAxisWidth - TextWidth(canvas, font, s1) - 2, cx);
                            canvas.DrawString(s1, font, FBrush, rectWidth + 2, cx);
                        }
                    }
                    #endregion

                    if (high == true) //是否抗锯齿
                    {
                        canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    }

                    #region 绘制主图k线
                    pen.DashStyle = DashStyle.Solid;
                    if (showfs == false)
                    {
                        if ((showk == 1) || ((showk == -1) && (main == true))) //主图显示k线
                        {

                            #region 分笔K线绘制
                            vhigh = GetBian("high"); //high
                            vlow = GetBian("low"); //low
                            vopen = GetBian("open"); //open
                            vclose = GetBian("close"); //close
                            b11 = GetBian("date"); //high
                            b12 = GetBian("time"); //low

                            //只包含close数据
                            if ((vhigh == null) && (vlow == null) && (vopen == null) && (vclose != null))//显示分笔K线
                            {
                                pen.DashStyle = DashStyle.Dash;
                                pen.Color = Color.Red;
                                for (i = StartIndex; i < EndIndex; i++)
                                {
                                    if (i < 1)
                                        continue;
                                    if (b11 != null)
                                    {
                                        cx = Convert.ToInt32(b11.value[i]);
                                        cy = Convert.ToInt32(b11.value[i - 1]);
                                        if ((cx != cy) || (i == recordCount - 1))
                                        {
                                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale), toph, leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale), rectHeight - both);
                                            if (showbottom)
                                            {
                                                h = Convert.ToInt32(b11.value[i]);
                                                m = h % 10000;
                                                s1 = string.Format("{0:d4}", m);
                                                fw = TextWidth(canvas, font, s1);// canvas.textwidth(s1);
                                                h = leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale) - fw / 2;
                                                if (h < leftYAxisWidth)
                                                    h = leftYAxisWidth;
                                                canvas.DrawString(s1, font, FBrush, h, rectHeight - both + 2);
                                            }
                                        }
                                    }
                                    if (b12 != null)
                                    {
                                        cy = Convert.ToInt32(b12.value[i - 1]) / 100;// / 100);// % 100; // / 100);
                                        cx = Convert.ToInt32(b12.value[i]) / 100;// / 100);
                                        if ((cx - cy) > 0)// (cx == 0) && (cy == 59)) || ((cx == 30) && (cy == 29)) //&&((cx=0)||(cx=31))) )
                                        {
                                            canvas.DrawLine(pen, leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale), toph, leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale), rectHeight - both);
                                            if (showbottom)
                                            {
                                                h = Convert.ToInt32(b12.value[i]);
                                                m = h % 100;
                                                h = h / 100;
                                                s1 = string.Format("{0:d2}:{1:d2}", h, m);
                                                fw = TextWidth(canvas, font, s1);
                                                h = leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale) - fw / 2;
                                                if (h < leftYAxisWidth)
                                                    h = leftYAxisWidth;
                                                canvas.DrawString(s1, font, FBrush, h, rectHeight - both + 2);
                                            }
                                        }
                                    }
                                }

                            }
                            #endregion


                            #region 蜡烛图绘制
                            pen.DashStyle = DashStyle.Solid;
                            b11 = GetBian("high"); //high
                            b12 = GetBian("low"); //low
                            b13 = GetBian("open"); //open
                            b14 = GetBian("close"); //close
                            if ((b11 != null) && (b12 != null) && (b13 != null) && (b14 != null))//显示K线蜡烛图 同时具备OHLC数据
                            {
                                f11 = @b11.value;
                                f12 = @b12.value;
                                f13 = @b13.value;
                                f14 = @b14.value;
                                double high1 = NA, low1 = -NA;
                                int hx = -1, lx = -1;
                                //从左绘制到右
                                for (i = StartIndex; i < EndIndex; i++)
                                {
                                    //获得区间最大最小值
                                    if (f11[i] > high1)
                                    {
                                        hx = i;
                                        high1 = f11[i];
                                    }
                                    if (f12[i] < low1)
                                    {
                                        lx = i;
                                        low1 = f12[i];
                                    }

                                    //获得X轴向位置
                                    cx = leftYAxisWidth + Convert.ToInt32(FScale * (i - StartIndex));
                                    fcx = leftYAxisWidth + (float)(FScale * (i - StartIndex));//获得当前Bar的X坐标

                                    //比较收盘价与开盘价 获得颜色
                                    bool up_bar = f14[i] > f13[i];//收盘价是否大于开盘价
                                    bool dn_bar = f14[i] < f13[i];
                                    bool eq_bar = f14[i] == f13[i];
                                    if (up_bar)
                                    {
                                        pen.Color = Color.Red;
                                        FBrush.Color = Color.Red;
                                    }
                                    if(dn_bar)
                                    {
                                        pen.Color = Color.Aqua;
                                        FBrush.Color = Color.Aqua;
                                    }
                                    if (eq_bar)
                                    {
                                        pen.Color = Color.Silver;
                                        FBrush.Color = Color.Silver;
                                    }
                                    //绘制空心K线
                                    float half_tick = (float)(fbsc1 / 2);
                                    float open_y, close_y, high_y, low_y;

                                    high_y = (rectHeight - both) - (float)((f11[i] - min1) * scale);
                                    low_y = (rectHeight - both) - (float)((f12[i] - min1) * scale);
                                    open_y = (rectHeight - both) - (float)((f13[i] - min1) * scale);
                                    close_y = (rectHeight - both) - (float)((f14[i] - min1) * scale);


                                    if (up_bar)//上升
                                    {
                                        //绘制上下Tick
                                        canvas.DrawLine(pen, fcx + half_tick, high_y, fcx + half_tick, close_y);
                                        canvas.DrawLine(pen, fcx + half_tick, open_y, fcx + half_tick, low_y);

                                        canvas.DrawRectangle(pen, fcx, close_y, (float)fbsc1, open_y - close_y);
                                        //canvas.FillRectangle(FBrush, fcx, close_y, (float)fbsc1, open_y - close_y);
                                    }
                                    else if (dn_bar)//下降
                                    {
                                        //绘制上下Tick
                                        canvas.DrawLine(pen, fcx + half_tick, high_y, fcx + half_tick, open_y);
                                        canvas.DrawLine(pen, fcx + half_tick, close_y, fcx + half_tick, low_y);

                                        canvas.FillRectangle(FBrush, fcx, open_y, (float)fbsc1, close_y - open_y);
                                        //canvas.FillRectangle(FBrush, fcx, close_y, (float)fbsc1, open_y - close_y);
                                    }
                                    else
                                    {
                                        canvas.DrawLine(pen, fcx + half_tick, high_y, fcx + half_tick, low_y);
                                        canvas.DrawLine(pen, fcx, open_y, fcx + (float)fbsc1, open_y);
                                    }

                                    /* 绘制实心K线
                                    cy = hh - (both + Convert.ToInt32((f11[i] - min1) * scale));//high
                                    cy1 = hh - (both + Convert.ToInt32((f12[i] - min1) * scale));//low
                                    fcy = (hh - both) - (float)((f11[i] - min1) * scale);//high
                                    fcy1 = (hh - both) - (float)((f12[i] - min1) * scale);//low

                                    //绘制最高 最低之间的线
                                    canvas.DrawLine(pen, fcx + (float)(fbsc1 / 2), fcy, fcx + (float)(fbsc1 / 2), fcy1);

                                    cy = hh - (both + Convert.ToInt32((f13[i] - min1) * scale));//open
                                    cy1 = hh - (both + Convert.ToInt32((f14[i] - min1) * scale));//close
                                    fcy = (hh - both) - (float)((f13[i] - min1) * scale);//open
                                    fcy1 = (hh - both) - (float)((f14[i] - min1) * scale);//close
                                    if (cy == cy1)//开盘价与收盘价一致
                                    {
                                        canvas.DrawLine(pen, fcx, fcy, fcx + (float)fbsc1, fcy);
                                    }
                                    else//否则执行填充
                                    {
                                        canvas.FillRectangle(FBrush, fcx, fcy, (float)fbsc1, fcy1 - fcy);
                                        canvas.FillRectangle(FBrush, fcx, fcy1, (float)fbsc1, fcy - fcy1);
                                    }
                                    **/




                                }
                                //显示最高值
                                if (hx != -1)
                                {
                                    cy = rectHeight - (both + Convert.ToInt32((high1 - min1) * scale));
                                    cx = leftYAxisWidth + Convert.ToInt32(FScale * (hx - StartIndex) + fbsc1 / 2);
                                    s1 = string.Format("{0:f2}", high1);
                                    int xx = 15;
                                    int fx = 0;
                                    if ((rectWidth + leftYAxisWidth - cx) < (cx - leftYAxisWidth))
                                    {
                                        xx = -15;
                                        fx = TextWidth(canvas, font, s1);
                                    }
                                    canvas.DrawString(s1, font, Brushes.White, cx + xx - fx, cy + 5);
                                    canvas.DrawLine(Pens.White, cx, cy, cx + xx, cy + 5);
                                }
                                //显示最小值
                                if (lx != -1)
                                {
                                    cy = rectHeight - (both + Convert.ToInt32((low1 - min1) * scale));
                                    cx = leftYAxisWidth + Convert.ToInt32(FScale * (lx - StartIndex) + fbsc1 / 2);
                                    s1 = string.Format("{0:f2}", low1);
                                    int xx = 15;
                                    int fx = 0;
                                    if ((rectWidth + leftYAxisWidth - cx) < (cx - leftYAxisWidth))
                                    {
                                        xx = -15;
                                        fx = TextWidth(canvas, font, s1);
                                    }
                                    canvas.DrawString(s1, font, Brushes.White, cx + xx - fx, Math.Min(rectHeight - both - 15, cy - 15));
                                    canvas.DrawLine(Pens.White, cx, cy, cx + xx, Math.Min(rectHeight - both - 15, cy - 15));
                                }


                                b11 = GetBian("date"); //high
                                if ((DL != null) && (b11 != null)) //显示信息地雷标记
                                {
                                    pen.Color = Color.Red;
                                    for (int j = 0; j < DL.Count; j++)
                                    {
                                        for (i = StartIndex; i < EndIndex; i++)
                                        {
                                            if (b11.value[i] > DL[j].date)
                                                break;
                                            if (b11.value[i] == DL[j].date)
                                            {
                                                cx = leftYAxisWidth + Convert.ToInt32((i - StartIndex) * FScale + FScale / 2) - 2;
                                                canvas.DrawLine(pen, cx, toph + 2, cx + 4, toph + 6);
                                                canvas.DrawLine(pen, cx, toph + 6, cx + 4, toph + 2);
                                                break;
                                            }
                                        }
                                    }
                                }


                                if (QuanInfo.Count > 0)//显示除权标志
                                {
                                    b11 = check("date");
                                    if (b11 != null)
                                    {
                                        pen.Color = Color.Green;
                                        bm = (Bitmap)(resources.GetObject("a100"));
                                        if (bm != null)
                                        {
                                            bm.MakeTransparent();
                                            cy = rectHeight - both;
                                            double date0 = b11.value[StartIndex];
                                            if (StartIndex > 0)
                                                date0 = b11.value[StartIndex - 1];

                                            for (i = StartIndex; i < EndIndex; i++)
                                            {
                                                double date = b11.value[i];
                                                for (int j = 0; j < QuanInfo.Count; j++)
                                                {

                                                    if ((QuanInfo[j].Date > date0) && (date >= QuanInfo[j].Date))
                                                    {
                                                        cx = leftYAxisWidth + (int)(FScale * (i - StartIndex) + fbsc1 / 2);
                                                        cy1 = rectHeight - (both + Convert.ToInt32((f12[i] - min1) * scale));
                                                        canvas.DrawImage(bm, cx - bm.Width / 2, cy - bm.Height - 2);
                                                        break;
                                                    }
                                                }
                                                date0 = date;
                                            }
                                        }
                                    }

                                }
                            }
                            else if ((b14 != null) && (fbsc1 > 1)) //显示点
                            {
                                f14 = b14.value;
                                pen.Color = Color.Aqua;
                                for (i = StartIndex; i < EndIndex; i++)
                                {
                                    fcx = leftYAxisWidth + (float)FScale * (i - StartIndex);
                                    fcy1 = rectHeight - (both + (float)((f14[i] - min1) * scale));
                                    if ((i > 0))
                                    {
                                        if (f14[i] > f14[i - 1])
                                            pen.Color = Color.Red;
                                        else
                                            pen.Color = Color.Aqua;
                                    }
                                    FBrush.Color = pen.Color;
                                    p = Convert.ToInt32(fbsc1 / 2);
                                    canvas.DrawEllipse(pen, fcx, fcy1 - (float)(fbsc1 / 2), (float)fbsc1, (float)fbsc1);
                                }
                            }
                            #endregion 蜡烛图绘制
                        }
                    }
                    #endregion

                }
                #endregion

                //显示多日分时 当前日期显示宽度小于1日 如果查询多日分时单只返回1日数据 将图绘制到右侧左侧无数据空开
                //if (showfs && (days > 1))
                //{
                //    cx = recordCount / linecount;
                //    cy = recordCount % linecount;
                //    if (cy > 0)
                //        cx++;
                //    f1 = Convert.ToDouble(rectWidth - leftYAxisWidth) / days;
                //    leftYAxisWidth = leftYAxisWidth + Convert.ToInt32((days - cx) * f1);
                //}
                

                #region 绘制指标
                for (p = 0; p < CurTech.outline.Count; p++)
                {
                    //continue;
                    pen.Width = 1;
                    cc1 = Color.White;
                    cc2 = cc1;
                    s1 = CurTech.outline[p];
                    fj = Regex.Split(s1, "::");
                    drawStyle = "";
                    if (fj.Length > 1)
                    {
                        drawStyle = fj[1];
                        s1 = fj[0];
                    }
                    fj = s1.Split('=');
                    s2 = "";
                    if (fj.Length > 1)
                    {
                        s2 = fj[1];
                        s1 = fj[0];
                    }
                    s1 = s1.ToLower();
                    fj = s2.Split(',');
                    //canvas.pen.style = pssolid;
                    //canvas.brush.style = bsclear;
                    //wei 指标尾部输出设置 wei可以设置线型,线宽,线色
                    pen.DashStyle = DashStyle.Solid;
                    if (drawStyle.Length > 0)
                        userwei(drawStyle);

                    #region btx
                    if (s1 == "btx")
                    {
                        b11 = check(fj[0]);// fenjie(s2, ",", 1));
                        if (b11 == null)
                            continue;
                        ff1 = b11.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (ff1[i] == NA)
                                continue;
                            if ((i < 4))
                                continue;
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            fcx = (float)(leftYAxisWidth + FScale * (i - StartIndex));
                            fclose = ff1[i];
                            b1 = ff1[i - 1];
                            b2 = ff1[i - 2];
                            b3 = ff1[i - 3];
                            b4 = ff1[i - 4];
                            cmax1 = fclose;
                            if ((b1 > cmax1))
                                cmax1 = b1;
                            if ((b2 > cmax1))
                                cmax1 = b2;
                            if ((b3 > cmax1))
                                cmax1 = b3;
                            cmin1 = fclose;
                            if ((b1 < cmin1))
                                cmin1 = b1;
                            if ((b2 < cmin1))
                                cmin1 = b2;
                            if ((b3 < cmin1))
                                cmin1 = b3;
                            bb1 = (fclose == cmax1) && ((b1 >= b2) || (b1 >= b3)) || (b1 == cmax1)
                                && ((b2 == cmin1) || (b3 == cmin1)) && (fclose >= b2) || (b2 == cmax1)
                              && (b3 == cmin1) && (fclose >= b1) || (b3 == cmax1) && (fclose >= b1) && (fclose >= b2);
                            if ((bb1))
                            {
                                fcy = rectHeight - (both + (float)((b1 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((fclose - min1) * scale));
                                FBrush.Color = GetColor("color0221f7");// Color.FromArgb(0x0221f7);
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                            if ((b1 == cmax1) && (fclose == cmin1))
                            {
                                fcy = rectHeight - (both + (float)((b2 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((fclose - min1) * scale));
                                FBrush.Color = GetColor("color009900");// Color.FromArgb(0x0221f7);
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                            //bb2:=fclose=cmax&&b1=cmin;
                            //变盘:stickline(bb2,b2,fclose,8,0),color930093;
                            bb2 = (fclose == cmax1) && (b1 == cmin1);
                            if ((bb2))
                            {
                                fcy = rectHeight - (both + (float)((b2 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((fclose - min1) * scale));
                                FBrush.Color = GetColor("ee00ee");
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                            //下跌:stickline(bb3,b1,fclose,8,0),color026f2e;
                            //bb3:=fclose=cmin&&(b1<b2||b1<b3)||b1=cmin&&(b2=cmax||b3=cmax)&&fclose<b2||b2=cmin&&b3=cmax&&fclose<b1||b3=cmin&&fclose<b1&&fclose<b2;
                            bb3 = (fclose == cmin1) && ((b1 < b2) || (b1 < b3)) || (b1 == cmin1) && ((b2 == cmin1) || (b3 == cmin1)) && (fclose < b2) || (b2 == cmin1) && (b3 == cmin1) && (fclose < b1) || (b3 == cmin1) && (fclose < b1) && (fclose < b2);
                            if ((bb3))
                            {
                                fcy = rectHeight - (both + (float)((b1 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((fclose - min1) * scale));
                                FBrush.Color = GetColor("color03f163");
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                            bb4 = (b1 == cmin1) && (fclose >= b2) || ((b2 == cmin1) && (b1 <= b3) || (b3 == cmin1) && (b1 <= b2)) && (b2 < b4) && (fclose >= b1);
                            if ((bb4))
                            {
                                fcy = rectHeight - (both + (float)((b1 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((b2 - min1) * scale));
                                FBrush.Color = GetColor("colorfcfcfc");
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                            bb5 = (b1 == cmax1) && (fclose < b2) || ((b2 == cmax1) && (b1 > b3) || (b3 == cmax1) && (b1 > b2)) && (b2 >= b4) && (fclose < b1);
                            if ((bb5))
                            {
                                fcy = rectHeight - (both + (float)((b1 - min1) * scale));
                                fcy1 = rectHeight - (both + (float)((b2 - min1) * scale));
                                FBrush.Color = Color.Yellow;
                                pen.Color = FBrush.Color;
                                if (fcy == fcy1)
                                    fcy1 += 2;
                                canvas.FillRectangle(FBrush, fcx, Math.Min(fcy, fcy1), (float)FScale, Math.Abs(fcy - fcy1));
                            }
                        }
                    }
                    #endregion

                    #region twr
                    if (s1 == "twr")
                    {
                        b11 = check(fj[0]);
                        if ((b11 == null))
                            continue;
                        pen.Color = Color.Green;
                        ff1 = b11.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (ff1[i] == NA)
                                continue;
                            if (i > 0)
                            {
                                if (ff1[i - 1] == NA)
                                    continue;
                            }
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            if ((i > StartIndex))
                            {
                                cy = rectHeight - (both + (int)((ff1[i] - min1) * scale));
                                cy1 = rectHeight - (both + (int)((ff1[i - 1] - min1) * scale));
                                if (ff1[i] >= ff1[i - 1])
                                    FBrush.Color = Color.Red;
                                else
                                    FBrush.Color = Color.Lime;
                                if (Math.Abs(cy1 - cy) < 1)
                                    cy = cy - 1;
                                canvas.FillRectangle(FBrush, cx, Math.Min(cy1, cy), (int)(FScale), Math.Abs(cy1 - cy));
                            }
                            if ((i - StartIndex > 2))
                            {
                                f1 = ff1[i];
                                if ((ff1[i] < ff1[i - 2]))
                                    f1 = ff1[i - 2];
                                cy = rectHeight - (both + (int)((ff1[i - 1] - min1) * scale));
                                cy1 = rectHeight - (both + (int)((f1 - min1) * scale));
                                if ((ff1[i - 1] >= ff1[i - 2]) && (ff1[i] < ff1[i - 1]))
                                    FBrush.Color = Color.Red;
                                if ((ff1[i - 1] < ff1[i - 2]) && (ff1[i] >= ff1[i - 1]))
                                    FBrush.Color = Color.Lime;
                                if (cy == cy1)
                                    cy--;
                                canvas.FillRectangle(FBrush, cx, Math.Min(cy1, cy), (int)(FScale), Math.Abs(cy1 - cy));
                            }
                        }
                    }
                    #endregion

                    #region box
                    //BOX(条件1, 价格, 高度, 条件2, BOOL)
                    if (s1 == "box")
                    {
                        b11 = check(fj[0]);
                        b12 = check(fj[3]);
                        if ((b11 == null) || (b12 == null))
                            continue;
                        b13 = check(fj[1]);
                        double vv1 = 0;
                        if (b13 == null)
                            teststr(fj[1], ref vv1);
                        double hh1 = 0;
                        teststr(fj[2], ref hh1);
                        double kk1 = 0;
                        teststr(fj[4], ref kk1);

                        dx = -1;
                        dy = -1;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (b11.value[i] == 1)
                            {
                                dx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                if (b13 != null)
                                    vv1 = b13.value[i];
                                if (vv1 == NA)
                                    continue;
                                dy = rectHeight - (both + (int)((vv1 - min1) * scale));
                                continue;
                            }
                            if (b12.value[i] == 1)
                            {
                                if (dx == -1)
                                {
                                    for (int k = i - 1; k > -1; k--)
                                    {
                                        if (b11.value[k] == 1)
                                        {
                                            dx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                            if (b13 != null)
                                                vv1 = b13.value[i];
                                            if (vv1 == NA)
                                                continue;
                                            dy = rectHeight - (both + (int)((vv1 - min1) * scale));
                                            break;
                                        }
                                    }
                                }
                                if (dx > -1)
                                {
                                    cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                    if (kk1 == 0) //空心
                                        canvas.DrawRectangle(pen, new Rectangle(dx, dy, cx - dx, (int)hh1));
                                    else
                                        canvas.FillRectangle(FBrush, new Rectangle(dx, dy, cx - dx, (int)hh1));
                                }
                                dx = -1;
                            }
                        }
                        if (dx > -1)
                        {
                            for (int k = EndIndex; k < RecordCount; k++)
                            {
                                if (b12.value[i] == 1)
                                {
                                    cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                    if (kk1 == 0) //空心
                                        canvas.DrawRectangle(pen, new Rectangle(dx, dy, cx - dx, (int)hh1));
                                    else
                                        canvas.FillRectangle(FBrush, new Rectangle(dx, dy, cx - dx, (int)hh1));
                                }
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region syline 绘制曲线
                    if (s1 == "syline")
                    {
                        userwei(drawStyle);
                        s3 = fj[0];
                        b11 = check(s3);
                        if (b11 == null)
                        {
                            f1 = 0.0F;
                            if (teststr(s3, ref f1))
                            {
                                cy = rectHeight - (both + (int)((f1 - min1) * scale));
                                canvas.DrawLine(pen, leftYAxisWidth, cy, rectWidth, cy);
                            }
                            continue;
                        }
                        if (drawStyle.IndexOf("COLORSTICK") > -1)
                        {
                            cc1 = Color.Aqua;
                            cc2 = Color.Red;
                            f11 = b11.value;
                            cy1 = rectHeight - (both + (int)((0 - min1) * scale));
                            for (i = StartIndex; i < EndIndex; i++)
                            {
                                if (f11[i] == NA)
                                    continue;
                                cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                                if (f11[i] > 0)
                                    pen.Color = cc1;
                                else
                                    pen.Color = cc2;
                                canvas.DrawLine(pen, cx + (int)(fbsc1 / 2), cy, cx + (int)(fbsc1 / 2), cy1);
                            }
                        }
                        else if (drawStyle.IndexOf("VOLSTICK") > -1)
                        {

                            b12 = check("open");
                            b13 = check("close");
                            if ((b12 == null) || (b13 == null))
                            {
                                ErrorString = b11.name + ":volstick输出需要开盘,收盘原始数据!";
                                continue;
                            }
                            f11 = b11.value;
                            f12 = b12.value;//open
                            f13 = b13.value;//close
                            if (min1 < 0)
                                cy1 = rectHeight - (both + (int)((0 - min1) * scale));
                            else
                                cy1 = rectHeight - both - 2;
                            //从左侧Bar绘制当前右侧Bar
                            for (i = StartIndex; i < EndIndex; i++)
                            {
                                if (f11[i] == NA)
                                    continue;
                                cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                fcx = leftYAxisWidth + (float)(FScale * (i - StartIndex));//获得当前Bar的X坐标

                                cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                                fcy = (rectHeight - both) - (float)((f11[i] - min1) * scale);


                                if (cy == cy1)
                                    cy -= 1;

                                //根据收盘价与开盘价设定颜色
                                bool upbar = f13[i] > f12[i];
                                if (upbar)
                                    pen.Color = Color.Red;
                                //else if (f13[i] == f12[i]) 价格相等为黄色
                                //    pen.Color = Color.Yellow;
                                else
                                    pen.Color = Color.Aqua;

                                //分时副图 柱子颜色为黄色
                                if (showfs && !main)
                                {
                                    pen.Color = Color.Yellow;
                                }

                                //柱体宽度小于1则绘制直线 否则绘制矩形
                                if (fbsc1 >= 1.0)
                                {
                                    FBrush.Color = pen.Color;
                                    if (showfs && !main)//分时副图
                                    {
                                        canvas.DrawLine(pen, cx, cy, cx, cy1);
                                    }
                                    else
                                    {
                                        if (upbar)
                                        {
                                            canvas.DrawRectangle(pen, cx, cy, (int)fbsc1, cy1 - cy);//视觉原因 实心红色矩形感觉与绿色矩形不齐
                                        }
                                        else
                                        {
                                            canvas.FillRectangle(FBrush, cx, cy, (int)fbsc1, cy1 - cy);//+ (f13[i] > f12[i] ?1:0));// cy1 - cy);底部不齐原因 
                                        }
                                    }

                                }
                                else
                                {
                                    //pen.Color = (f13[i] > f12[i]) ? Color.Red : Color.Aqua;
                                    canvas.DrawLine(pen, cx, cy, cx, cy1);
                                }
                            }
                        }
                        else
                        {
                            ff1 = b11.value;
                            dx = dy = -1;
                            //绘制曲线
                            for (i = StartIndex; i < EndIndex; i++)
                            {
                                if (ff1[i] == NA)
                                    continue;
                                cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                cy = rectHeight - (both + (int)((ff1[i] - min1) * scale));
                                if (cy > rectHeight - both)
                                    cy = rectHeight - both;
                                if (dx == -1)
                                {
                                    dx = cx + (int)(FScale / 2);
                                    dy = cy;
                                }
                                else
                                {
                                    canvas.DrawLine(pen, dx, dy, cx + (int)(FScale / 2), cy);
                                    dx = cx + (int)(FScale / 2);
                                    dy = cy;
                                }
                            }
                        }

                    }
                    #endregion

                    #region drawicon 绘制图标

                    if (s1 == "drawicon")
                    {
                        b11 = check(fj[0]);
                        if (b11 == null)
                            continue;
                        b12 = check(fj[1]);
                        if (b12 == null)
                            continue;
                        s2 = "a" + fj[2];
                        bm = (Bitmap)(resources.GetObject(s2));
                        bm.MakeTransparent(BackColor);
                        f11 = b11.value;
                        f12 = b12.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (f12[i] == NA)
                                continue;
                            if (f11[i] == 1)
                            {
                                cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                cy = rectHeight - (both + (int)((f12[i] - min1) * scale));
                                canvas.DrawImage(bm, cx + (int)(FScale / 2) - bm.Width / 2, cy - 4);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region drawtext 绘制文字
                    if (s1 == "drawtext")
                    {
                        pen.Color = Color.White;
                        FBrush.Color = Color.White;
                        b11 = check(fj[0]);
                        if (b11 == null)
                            continue;
                        b12 = check(fj[1]);
                        if (b12 == null)
                            continue;
                        s3 = fj[2];
                        f11 = b11.value;
                        f12 = b12.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (f12[i] == NA)
                                continue;
                            if (f11[i] == 1)
                            {
                                cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                                cy = rectHeight - (both + (int)((f12[i] - min1) * scale));
                                canvas.DrawString(s3, font, FBrush, cx, cy);
                            }
                        }
                        continue;
                    }
                    #endregion 

                    #region partline
                    if (s1 == "partline")
                    {
                        b11 = check(fj[0]);
                        if ((b11 == null))
                            continue;
                        cc1 = GetColor(fj[1]);
                        cc2 = GetColor(fj[2]);
                        ff1 = b11.value;
                        dx = dy = 0;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (ff1[i] == NA)
                                continue;
                            if (i > 0)
                            {
                                if (ff1[i - 1] == NA)
                                    continue;
                            }

                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            cy = rectHeight - (both + (int)((ff1[i] - min1) * scale));
                            if ((i == StartIndex))
                            {
                                dx = cx + (int)(FScale / 2);
                                dy = cy;
                            }
                            else
                            {
                                if ((ff1[i] > ff1[i - 1]))
                                    pen.Color = cc1;
                                else
                                    pen.Color = cc2;
                                canvas.DrawLine(pen, dx, dy, cx + (int)(fbsc1 / 2), cy);
                                dx = cx + (int)(FScale / 2);
                                dy = cy;
                            }
                        }
                    }
                    #endregion

                    #region volstick
                    if (s1 == "volstick")
                    {
                        b11 = check(fj[0]);
                        b12 = check(fj[1]);
                        b13 = check(fj[2]);
                        if ((b11 == null) || (b12 == null) || (b13 == null))
                            continue;
                        cc1 = GetColor(fj[3]);
                        cc2 = GetColor(fj[4]);
                        f11 = b11.value;
                        f12 = b12.value;
                        f13 = b13.value;
                        cy1 = rectHeight - both - 1;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if ((f12[i] == NA) || (f13[i] == NA))
                                continue;
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                            if ((FScale >= 1.0))
                            {
                                if ((f13[i] > f12[i]))
                                    FBrush.Color = cc1;
                                else
                                    FBrush.Color = cc2;
                                if (cy == cy1)
                                    cy--;
                                canvas.FillRectangle(FBrush, cx, cy, (int)(fbsc1), cy1 - cy);
                            }
                            else
                            {
                                if (f13[i] > f12[i])
                                    pen.Color = cc1;
                                else
                                    pen.Color = cc2;
                                canvas.DrawLine(pen, cx, cy, cx, cy1);
                            }
                        }
                    }
                    #endregion

                    #region stickline
                    if (s1 == "stickline")
                    {
                        b11 = check(fj[0]);
                        b12 = check(fj[1]);
                        if ((b11 == null) || (b12 == null))
                            continue;
                        cc1 = GetColor(fj[2]);
                        cc2 = GetColor(fj[3]);
                        f11 = b11.value;
                        f12 = b12.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if ((f11[i] == NA) || (f12[i] == NA))
                                continue;
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                            cy1 = rectHeight - (both + (int)((f12[i] - min1) * scale));
                            if (f11[i] > f12[i])
                                pen.Color = cc1;
                            else
                                pen.Color = cc2;
                            canvas.DrawLine(pen, cx + (int)(fbsc1 / 2), cy, cx + (int)(fbsc1 / 2), cy1);
                        }
                    }
                    #endregion

                    #region sticklinebar
                    if (s1 == "sticklineBar")
                    {
                        userwei(drawStyle);
                        b1 = b2 = b3 = b4 = b5 = 0.0;
                        b11 = check(fj[0]);
                        if (b11 == null)
                        {
                            if (!teststr(fj[0], ref b1))
                                continue;
                        }
                        b12 = check(fj[1]);
                        if (b12 == null)
                        {
                            if (!teststr(fj[1], ref b2))
                                continue;
                        }
                        b13 = check(fj[2]);
                        if (b13 == null)
                        {
                            if (!teststr(fj[2], ref b3))
                                continue;
                        }
                        teststr(fj[3], ref b4);
                        dx = (int)(fbsc1 * b4 / 6);
                        teststr(fj[4], ref b5);
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            if (b11 != null)
                                b1 = b11.value[i];
                            if (b1 == NA)
                                continue;
                            if (b12 != null)
                                b2 = b12.value[i];
                            if (b13 != null)
                                b3 = b13.value[i];
                            if (b1 > 0)
                            {
                                cy = rectHeight - (both + (int)((b2 - min1) * scale));
                                cy1 = rectHeight - (both + (int)((b3 - min1) * scale));
                                cx = cx + (int)(fbsc1 / 2);
                                if (b5 == 0)
                                {
                                    //FBrush.Color = pen.Color;// cc1;
                                }
                                else if (b5 == -1)
                                {
                                    pen.DashStyle = DashStyle.DashDot;
                                    pen.Width = 1;
                                    FBrush.Color = bgcolor;
                                }
                                if (dx > 0)
                                {
                                    if (cy == cy1)
                                        cy -= 1;
                                    canvas.FillRectangle(FBrush, cx - dx, Math.Min(cy1, cy), 2 * dx, Math.Abs(cy1 - cy));
                                }
                                else
                                    canvas.DrawLine(pen, cx, cy, cx, cy1);
                            }
                        }
                        continue;
                    }
                    #endregion

                    #region drawkline
                    if (s1 == "drawkline")
                    {
                        b11 = check(fj[0]);// fenjie(s2, ",", 1)); //high
                        b12 = check(fj[1]);//fenjie(s2, ",", 2)); //low
                        b13 = check(fj[2]);//fenjie(s2, ",", 3)); //open
                        b14 = check(fj[3]);//fenjie(s2, ",", 4)); //close
                        if ((b11 == null) || (b12 == null) || (b13 == null) || (b14 == null))
                            continue;
                        f11 = b11.value;
                        f12 = b12.value;
                        f13 = b13.value;
                        f14 = b14.value;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            if ((f13[i] == NA) || (f14[i] == NA))
                                continue;
                            if (f14[i] > f13[i])
                                pen.Color = Color.Red;
                            else
                                pen.Color = Color.Aqua;
                            cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                            cy1 = rectHeight - (both + (int)((f12[i] - min1) * scale));
                            canvas.DrawLine(pen, cx + (int)(fbsc1 / 2), cy, cx + (int)(fbsc1 / 2), cy1);
                            cy = rectHeight - (both + (int)((f13[i] - min1) * scale));
                            cy1 = rectHeight - (both + (int)((f14[i] - min1) * scale));
                            FBrush.Color = pen.Color;
                            canvas.FillRectangle(FBrush, cx, cy, (int)fbsc1, cy1 - cy);
                        }
                    }
                    #endregion

                    #region colorstick 分时彩色小柱
                    if (s1 == "colorstick")
                    {
                        b11 = check(fj[0]);
                        if (b11 == null)
                            continue;
                        cc1 = GetColor(fj[1]);
                        cc2 = GetColor(fj[2]);
                        f11 = b11.value;
                        cy1 = rectHeight - (both + (int)((0 - min1) * scale));
                        pen.Color = Color.White;
                        canvas.DrawLine(pen, leftYAxisWidth, cy1, rectWidth, cy1);
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            if (f11[i] == NA)
                                continue;
                            cx = leftYAxisWidth + (int)(FScale * (i - StartIndex));
                            cy = rectHeight - (both + (int)((f11[i] - min1) * scale));
                            if (f11[i] > 0)
                                pen.Color = cc1;
                            else
                                pen.Color = cc2;
                            canvas.DrawLine(pen, cx + (int)(fbsc1 / 2), cy, cx + (int)(fbsc1 / 2), cy1);
                        }
                    }
                    #endregion

                    #region 如果显示头部信息区域 则将指标输出值进行输出
                    if (showtop)
                    {
                        b11 = check(fj[0]);
                        if (b11 == null)
                            continue;
                        h = StartIndex + (int)((curx - leftYAxisWidth) / FScale);
                        if ((cursor == false) || ((h < 0) || (h >= EndIndex)) || (curx == -1))
                            h = EndIndex - 1;
                        if (h < 0)
                            h = 0;
                        if (b11.value[h] != NA)
                        {
                            FBrush.Color = bgcolor;
                            FBrush.Color = pen.Color;
                            s1 = string.Format(fmt, b11.value[h]);
                            s1 = fj[0].ToUpper() + ":" + s1;
                            int len = TextWidth(canvas, font, s1);
                            if ((leftYAxisWidth + fw + 2 + len) < (rectWidth - rightYAxisWidth))
                            {
                                DrawText(canvas, leftYAxisWidth + 2 + fw, 2, s1);
                                fw = fw + len + 3;
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                if (t > 0)
                    continue;
                //if (days > 0)
                //{
                //    leftYAxisWidth = showleft ? SpaceWidth : 0;
                //}


                #region 显示自画线
                SmoothingMode sm = canvas.SmoothingMode;
                canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// .HighSpeed;//.AntiAlias;

                if (showline)
                {
                    for (i = 0; i < Lines.Count; i++)
                    {
                        XLine xl = Lines[i];
                        xl.Draw(this, canvas);
                    }
                }

                if (CurLine != null)
                {
                    Pen pen1 = new Pen(CurLine.color, CurLine.linewidth);
                    pen1.DashStyle = DashStyle.Solid;
                    canvas.DrawRectangle(pen1, Bounds);
                    CurLine.Draw(this, canvas);
                }
                canvas.SmoothingMode = sm;// System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                #endregion


                #region K线图显示底部日期栏
                ////显示光标
                pen.DashStyle = DashStyle.Solid;
                pen.Width = 1;
                if ((!showfs) && (showbottom))
                {
                    b11 = check("date");
                    b12 = check("time");
                    if (b11 != null)
                    {
                        pen.Color = LineColor;// Color.DarkRed;
                        FBrush.Color = Color.Gray;
                        dy = -1;

                        y1 = -1;
                        int m1 = -1;
                        int dd1 = -1;
                        for (i = StartIndex; i < EndIndex; i++)
                        {
                            dx = leftYAxisWidth + (int)(FScale * (i - StartIndex) + FScale / 2);
                            cx = (int)b11.value[i];
                            int yy = cx / 10000;
                            int mm = (cx % 10000) / 100;
                            int dd = cx % 100;

                            if ((y1 != yy) && (dx > dy + 10))
                            {
                                s1 = yy.ToString() + "年";
                                if (y1 != -1)
                                    canvas.DrawLine(pen, dx, rectHeight - both, dx, rectHeight);
                                canvas.DrawString(s1, font, FBrush, new PointF(dx, rectHeight - both + 3));
                                dy = dx + TextWidth(canvas, font, s1);
                                y1 = yy;
                            }

                            if ((mm != m1) && (dx > dy + 10))
                            {
                                s1 = mm.ToString() + "月";
                                canvas.DrawLine(pen, dx, rectHeight - both, dx, rectHeight);
                                canvas.DrawString(s1, font, FBrush, new Point(dx + 2, rectHeight - both + 4));
                                dy = dx + TextWidth(canvas, font, s1);
                                m1 = mm;
                            }

                            if ((dd != dd1) && (dx > dy + 10))
                            {
                                s1 = dd.ToString() + "日";
                                canvas.DrawLine(pen, dx, rectHeight - both, dx, rectHeight);
                                canvas.DrawString(s1, font, FBrush, new Point(dx + 2, rectHeight - both + 4));
                                dy = dx + TextWidth(canvas, font, s1);
                                dd1 = dd;
                            }
                        }
                    }
                }
                #endregion


                #region 十字线状态 显示时间栏中的滑块标签
                if ((cursor) || (cury == -2))
                {
                    //logger.Info(string.Format("cursor:{0} curx:{1} cury:{2}", cursor, curx, cury));
                    if ((cury == -2) && (this.main))
                    {
                        i = Convert.ToInt32(StartIndex + (curx - leftYAxisWidth) / FScale);
                        b14 = check("close"); // close
                        if ((b14 != null) && (i < b14.len) && (i > -1))
                        {
                            cury = rectHeight - both - Convert.ToInt32((b14.value[i] - min1) * scale);
                            Point sp = pCtrl.PointToScreen(new Point(curx, cury));
                            SetCursorPos(sp.X, sp.Y);
                        }
                    }
                    //logger.Info(string.Format("cursor:{0} curx:{1} cury:{2}", cursor, curx, cury));
                    FCurXX = curx;
                    FCurYY = cury;
                    //绘制Y轴向十字线
                    if ((curx > leftYAxisWidth) && (curx < rectWidth))
                    {
                        pen.Color = Color.White;
                        canvas.DrawLine(pen, curx,main?toph:0, curx, rectHeight);

                        if ((showfs))
                        {
                            int fw11 = Convert.ToInt32(TextWidth(canvas, font, "88:88") / 2) + 3;
                            i = (int)((curx - leftYAxisWidth) / FScale);
                            if ((both >= fh1) && (i < linecount))
                            {
                                FBrush.Color = Color.Blue;
                                pen.Color = Color.Red;
                                dx = Math.Max(curx, leftYAxisWidth + fw11);
                                canvas.FillRectangle(FBrush, dx - fw11, rectHeight - both, fw11 + fw11, both - 2);
                                canvas.DrawRectangle(pen, dx - fw11, rectHeight - both, fw11 + fw11, both - 2);
                                b = i > 120 ? 1 : 0;
                                h = 9 + b * 4 + Convert.ToInt32((i + 30 - 150 * b) / 60);
                                m = (i + 30 - 150 * b) % 60;
                                s1 = string.Format("{0:d}:{1:d}", h, m);
                                dx = Math.Max(curx - fw11 + 3, leftYAxisWidth + 1);
                                FBrush.Color = Color.White;
                                canvas.DrawString(s1, font, FBrush, dx, rectHeight - both + 2);
                            }
                        }
                        else
                        {
                            h = StartIndex + (int)((curx - leftYAxisWidth) / FScale);
                            s1 = "";
                            s2 = "";
                            if ((h < recordCount))
                            {
                                b11 = check("date");
                                if ((b11 != null))
                                    s1 = string.Format("{0:d}", (int)(b11.value[h]));
                                b12 = check("time");
                                if ((b12 != null))
                                {
                                    h = (int)b12.value[h];
                                    h = h / 100;
                                    m = h % 100;
                                    h = h / 100;
                                    s2 = string.Format("{0:d2}:{1:d}", h, m);
                                }
                                if ((s2 != ""))
                                    s1 = s1 + " " + s2;
                            }
                            if (s1.Length > 0)
                            {
                                int fw11 = TextWidth(canvas, font, s1) / 2 + 2;
                                cx = curx;
                                if ((curx + fw11 + 3 > rectWidth))
                                    cx = rectWidth - fw11 - 2;
                                if ((both >= fh1) && (h < EndIndex))
                                {
                                    FBrush.Color = Color.FromArgb(0, 0, 127);// .Blue;
                                    pen.Color = Color.Red;
                                    canvas.FillRectangle(FBrush, cx, rectHeight - both, fw11 + fw11, both - 2);
                                    FBrush.Color = Color.FromArgb(191, 191, 191);// .White;
                                    canvas.DrawString(s1, font, FBrush, cx + 3, rectHeight - both + 2);
                                }
                            }
                        }

                    }
                    //绘制X轴向十字线
                    if ((cury > toph) && (cury < rectHeight - both))
                    {
                        pen.Color = Color.White;
                        canvas.DrawLine(pen, leftYAxisWidth, cury, rectWidth, cury);
                        f1 = (cury - toph) / scale;
                        f1 = max1 - f1;
                        FCurValue = f1 / c2;
                        cury1 = cury;
                        if ((cury1 - 6 + fh1 + 2 > rectHeight))
                            cury1 = rectHeight + 3 - fh1;
                        if ((cury1 < 7))
                            cury1 = 7;
                        s1 = string.Format(fmt, Math.Abs(f1 / c2));
                        if (showleft)
                        {
                            FBrush.Color = Color.Blue;
                            pen.Color = Color.Red;
                            canvas.FillRectangle(FBrush, 1, cury1 - 7, leftYAxisWidth - 2, -6 + fh1 + 2);
                            canvas.DrawRectangle(pen, 1, cury1 - 7, leftYAxisWidth - 2, -6 + fh1 + 2);
                            FBrush.Color = f1 > 0 ? Color.Red : Color.Green;
                            canvas.DrawString(s1, font, FBrush, leftYAxisWidth - TextWidth(canvas, font, s1) - 2, cury1 - 6);
                        }
                        if ((showright) && (showfs) && (main) && (prevclose != NA))
                        {
                            FBrush.Color = Color.Blue;
                            pen.Color = Color.Red;
                            canvas.FillRectangle(FBrush, rectWidth + 1, cury1 - 7, rightYAxisWidth - 2, fh1 - 6 + 2);
                            canvas.DrawRectangle(pen, rectWidth + 1, cury1 - 7, rightYAxisWidth - 2, fh1 - 6 + 2);
                            d1 = prevclose;
                            de1 = Math.Abs((f1 - d1) * 100 / d1);
                            if (((d1 > min1) && (d1 < max1)) || (de1 < 30))
                            {
                                FBrush.Color = f1 >= d1 ? Color.Red : Color.Green;
                                de = Math.Abs(f1 - d1) * 100 / d1;
                                if ((de > 100))
                                    s1 = "{0:F1}%";
                                else if (de > 10)
                                    s1 = "{0:F2}%";
                                else if ((de < 10))
                                    s1 = "{0:F2}%";
                                s1 = string.Format(s1, de);
                                canvas.DrawString(s1, font, FBrush, rectWidth + 2, cury1 - 6);
                            }
                        }
                        else if (showright)
                        {
                            FBrush.Color = Color.Blue;
                            pen.Color = Color.Red;
                            canvas.FillRectangle(FBrush, rectWidth + 2, cury1 - 7, rightYAxisWidth - 3, -6 + fh1 + 2);
                            canvas.DrawRectangle(pen, rectWidth + 2, cury1 - 7, rightYAxisWidth - 3, -6 + fh1 + 2);
                            FBrush.Color = Color.Red;
                            s1 = string.Format(fmt, Math.Abs(f1 / c2));
                            canvas.DrawString(s1, font, FBrush, rectWidth + 2, cury1 - 6);
                        }
                    }
                }
                #endregion

            }


            return true;
        }

    }
}
