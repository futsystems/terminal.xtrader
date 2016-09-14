using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CStock
{
    public partial class TStock
    {

        #region K线控件绘制主逻辑
        /// <summary>
        /// K线控件绘制
        /// DrawBoard为左侧 自绘图按钮部分
        /// Board为右侧盘口部分
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = null;//绘制区域位图
            Graphics cv = e.Graphics;

            int chartWidth, chartHeight, bottomTabHeight, drawBoxWidth;
            chartWidth = this.Width;//绘图区域总宽
            chartHeight = this.Height;//绘图区域总高
            bottomTabHeight = 0;//底部Tab菜单高度
            drawBoxWidth = 0;//自绘按钮区域宽度

            //自绘按钮区域可见
            if (DrawBoard.Visible)
            {
                drawBoxWidth = DrawBoard.Width;
                chartWidth -= drawBoxWidth;
            }

            //底部Tab菜单区域可见
            if (Tab.Visible)
                bottomTabHeight = Tab.Height;

            //盘口区域可见
            if (Board.Visible)
            {
                chartWidth = chartWidth - (Board.Width + SP1.Width);
            }

            //绘图区域小于一定尺寸 直接返回不执行绘制操作
            if ((chartWidth < 10) || (chartHeight < 10))
                return;

            //计算每个指标区域的默认高度
            if (FSGSH[0] == -1)
            {
                //计算窗口高度,主窗口高度为指标窗口高度2倍
                int h1 = Height;
                int hh0 = (h1 / (fswindows + 1));
                FSGSH[0] = hh0 * 2;
                for (int i = 1; i < fswindows; i++)
                    FSGSH[i] = hh0;
                FSGSH[fswindows - 1] = h1 - hh0 * fswindows;

                if (Ftab)
                    h1 -= Tab.Height;
                chartHeight = (h1 / techwindows);
                for (int i = 0; i < techwindows; i++)
                    GSH[i] = chartHeight;

                //设定当前公式s
                curgs = null;
                if (this.IsIntraView)
                    curgs = FSGS[0];
                if (this.IsBarView)
                    curgs = GS[0];
            }


            //绘制分时
            if (this.IsIntraView)
            {
                FSGS[0].Focused = focus;
                chartHeight = 0;
                for (int i = 0; i < fswindows; i++)
                {
                    bm = new Bitmap(chartWidth, FSGSH[i]);
                    if (cury == -2)
                    {
                        FSGS[i].SetCurxy(curx - drawBoxWidth, -2);
                    }
                    else
                    {
                        if ((cury > chartHeight) && (cury < (chartHeight + FSGSH[i])))
                        {
                            FSGS[i].SetCurxy(curx - drawBoxWidth, cury - chartHeight);
                        }
                        else
                            FSGS[i].SetCurxy(curx - drawBoxWidth, -1);
                    }
                    FSGS[i].OnPaint(bm);
                    e.Graphics.DrawImage(bm, drawBoxWidth, chartHeight);
                    bm.Dispose();
                    chartHeight += FSGSH[i];
                }
            }

            //绘制Bar图
            if (this.IsBarView)
            {
                GS[0].Focused = focus;
                chartHeight = 0;
                //遍历所有窗口
                for (int i = 0; i < techwindows; i++)
                {
                    bm = new Bitmap(chartWidth, GSH[i]);//根据宽度和公式区域高度生成Bitmap
                    if (cury == -2)//cury=-2时,curx=是周期数 左右移动时候 需要curx cury
                    {
                        GS[i].SetCurxy(curx, -2);
                    }
                    else //这个控件的curx cury映射到 对应的指标区域，计算出 指标区域内的当前 x y
                    {
                        if ((cury > chartHeight) && (cury < (chartHeight + GSH[i])))
                        {
                            GS[i].SetCurxy(curx - drawBoxWidth, cury - chartHeight);
                        }
                        else
                            GS[i].SetCurxy(curx - drawBoxWidth, -1);
                    }
                    GS[i].OnPaint(bm);//在对应的Bitmap上绘制图像
                    cv.DrawImage(bm, drawBoxWidth, chartHeight);//在主绘图区域绘制每个公式区域的Bitmap
                    bm.Dispose();
                    chartHeight += GSH[i];
                }

                //绘制矩形缩放
                if ((GS[0].CurWindow) && (PressXY.X > -1) && (FCursorType == TCursorType.ctZoom))
                {

                    SolidBrush sb = new SolidBrush(Color.FromArgb(80, 223, 239, 223));
                    Pen pen = new Pen(Color.LawnGreen, 1);
                    int x = Math.Min(PressXY.X, NowXY.X);
                    int y = Math.Min(PressXY.Y, NowXY.Y);
                    chartWidth = Math.Abs(NowXY.X - PressXY.X);
                    chartHeight = Math.Abs(NowXY.Y - PressXY.Y);
                    cv.DrawRectangle(pen, x, y, chartWidth, chartHeight);
                    cv.FillRectangle(sb, x + 1, y + 1, chartWidth - 1, chartHeight - 1);
                }

            }

            //绘制整个绘图区域外侧矩形框
            Pen pen1 = new Pen(this.KChartLineColor, 1);
            //cv.DrawRectangle(pen1, drawBoxWidth, 0, this.Width - drawBoxWidth - 1, this.Height - 1 - bottomTabHeight);

            if (DataHint.Visible)
            {
                //填充信息输出窗口
                this.PaintDataHint();
            }
        }

                   
        void PaintDataHint()
        {
            try
            {
                TGongSi g1 = null;
                if (this.IsIntraView)
                    g1 = FSGS[0];
                else
                    g1 = GS[0];
                int curbar = g1.FCurBar;
                int len = g1.RecordCount;
                if ((curbar > -1) && (len > 0) && (curbar < len))
                {
                    int k = 0;
                    if (this.IsIntraView)
                    {
                        TBian b1 = g1.check("date");
                        if (b1 != null)
                        {
                            HL[k++].Text = "日期";
                            int day = (int)(b1.value[curbar]);
                            HL[k++].Text = String.Format("{0:D}", day);
                        }
                        b1 = g1.check("time");
                        if (b1 != null)
                        {
                            HL[k++].Text = "时间";
                            int day = (int)(b1.value[curbar]);
                            HL[k++].Text = String.Format("{0:D}:{1:D}", (day % 10000) / 100, (day % 10000) % 100);
                        }
                        b1 = g1.check("close");
                        if (b1 != null)
                        {
                            HL[k++].Text = "最新";
                            HL[k].ForeColor = Color.White;
                            double aa = b1.value[curbar];
                            HL[k].Text = String.Format("{0:F2}", aa);
                            if (PreClose != NA)
                            {
                                if (aa > PreClose)
                                    HL[k].ForeColor = Color.Red;
                                if (aa < PreClose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                            if (PreClose != NA)
                            {
                                HL[k++].Text = "涨跌";
                                HL[k].ForeColor = Color.White;
                                double f1 = Math.Abs(aa - PreClose);// (aa - PreClose) * 100 / PreClose);
                                string ss = String.Format("{0:F2}", f1);
                                if (aa > PreClose)
                                {
                                    HL[k].ForeColor = Color.Red;
                                    ss = "▲" + ss;
                                }
                                if (aa < PreClose)
                                {
                                    HL[k].ForeColor = Color.Lime;
                                    ss = "▽" + ss;
                                }
                                HL[k].Text = ss;
                                k++;

                                HL[k++].Text = "幅度";
                                HL[k].ForeColor = Color.SkyBlue;
                                f1 = Math.Abs((aa - PreClose) * 100 / PreClose);
                                if (aa > PreClose)
                                    HL[k].ForeColor = Color.Red;
                                if (aa < PreClose)
                                    HL[k].ForeColor = Color.Lime;
                                HL[k].Text = String.Format("{0:F2}%", f1); ;
                                k++;
                            }
                        }
                        b1 = g1.check("均价线");
                        if (b1 != null)
                        {
                            HL[k++].Text = "均价";
                            HL[k].ForeColor = Color.SkyBlue;
                            HL[k++].Text = String.Format("{0:F2}", b1.value[curbar]);
                        }

                        b1 = g1.check("vol");
                        if (b1 != null)
                        {
                            HL[k++].Text = "手数";
                            HL[k].ForeColor = Color.Yellow;
                            HL[k++].Text = String.Format("{0:D}", (int)(b1.value[curbar] / 100));
                        }

                    }
                    if (this.IsBarView)
                    {
                        TBian b1 = g1.check("date");
                        if (b1 != null)
                        {
                            int day = (int)(b1.value[curbar]);
                            if (day > 19900101)
                            {
                                int yy = day / 10000;
                                int mm = (day % 10000) / 100;
                                int dd = day % 100;
                                if ((mm > 0) && (mm < 13) && (dd > 0) && (dd < 32))
                                {
                                    HL[k++].Text = "日期";
                                    DateTime dt = new DateTime(day / 10000, (day % 10000) / 100, day % 100);
                                    string wk = WeekStr[(int)dt.DayOfWeek];
                                    HL[k++].Text = String.Format("{0:D}", day) + "/" + wk; ;
                                }
                            }
                        }
                        b1 = g1.check("close");
                        double pclose = NA;
                        if (curbar > 0)
                            pclose = b1.value[curbar - 1];//取上一天的收盘价，用来计算今天涨幅
                        b1 = g1.check("open");
                        if (b1 != null)
                        {
                            HL[k++].Text = "开盘";
                            HL[k].ForeColor = Color.White;
                            HL[k].Text = String.Format("{0:F2}", b1.value[curbar]);
                            if (pclose != NA)
                            {
                                if (b1.value[curbar] > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (b1.value[curbar] < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        b1 = g1.check("high");
                        if (b1 != null)
                        {
                            HL[k++].Text = "最高";
                            HL[k].ForeColor = Color.White;
                            HL[k].Text = String.Format("{0:F2}", b1.value[curbar]);
                            if (pclose != NA)
                            {
                                if (b1.value[curbar] > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (b1.value[curbar] < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        b1 = g1.check("low");
                        if (b1 != null)
                        {
                            HL[k++].Text = "最低";
                            HL[k].ForeColor = Color.White;
                            HL[k].Text = String.Format("{0:F2}", b1.value[curbar]);
                            if (pclose != NA)
                            {
                                if (b1.value[curbar] > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (b1.value[curbar] < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        b1 = g1.check("close");
                        if (b1 != null)
                        {
                            HL[k++].Text = "收盘";
                            HL[k].ForeColor = Color.White;
                            HL[k].Text = String.Format("{0:F2}", b1.value[curbar]);
                            if (pclose != NA)
                            {
                                if (b1.value[curbar] > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (b1.value[curbar] < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        b1 = g1.check("vol");
                        if (b1 != null)
                        {
                            HL[k++].Text = "成交";
                            HL[k].ForeColor = Color.White;
                            int aa = (int)(b1.value[curbar]);
                            if (aa > 100000)
                            {
                                aa = aa / 10000;
                                HL[k].Text = String.Format("{0:D}万", aa);
                            }
                            else if (aa > 100000000)
                                HL[k].Text = String.Format("{0:D}亿", aa / 100000000);
                            else
                                HL[k].Text = String.Format("{0:D}", aa);
                            if (pclose != NA)
                            {
                                if (b1.value[curbar] > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (b1.value[curbar] < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        b1 = g1.check("amount");
                        if (b1 != null)
                        {
                            if (b1.value[curbar] != NA)
                            {
                                HL[k++].Text = "金额";
                                HL[k].ForeColor = Color.White;
                                Int64 aa = Convert.ToInt64(b1.value[curbar]);
                                if (aa > 100000)
                                {
                                    aa = aa / 10000;
                                    HL[k].Text = String.Format("{0:D}万", aa);
                                }
                                else if (aa > 100000000)
                                    HL[k].Text = String.Format("{0:D}亿", aa / 100000000);
                                else
                                    HL[k].Text = String.Format("{0:D}", aa);
                                if (pclose != NA)
                                {
                                    if (b1.value[curbar] > pclose)
                                        HL[k].ForeColor = Color.Red;
                                    if (b1.value[curbar] < pclose)
                                        HL[k].ForeColor = Color.Lime;
                                }
                                k++;
                            }
                        }

                        b1 = g1.check("close");
                        if ((b1 != null) && (pclose != NA))
                        {
                            HL[k++].Text = "涨跌幅";
                            HL[k].ForeColor = Color.White;
                            double aa = b1.value[curbar];
                            double f1 = Math.Abs((aa - pclose) * 100 / pclose);
                            HL[k].Text = String.Format("{0:F2}%", f1);
                            if (b1.value[curbar] > pclose)
                                HL[k].ForeColor = Color.Red;
                            if (b1.value[curbar] < pclose)
                                HL[k].ForeColor = Color.Lime;
                            k++;
                        }
                        TBian b2 = g1.check("high");
                        TBian b3 = g1.check("low");
                        if ((b2 != null) && (b3 != null))
                        {

                            HL[k++].Text = "振幅";
                            double hh1 = b2.value[curbar];
                            double ll1 = b3.value[curbar];
                            HL[k].ForeColor = Color.White;
                            double f1 = Math.Abs((hh1 - ll1) * 100 / ll1);
                            HL[k].Text = String.Format("{0:F2}%", f1);
                            if (pclose != NA)
                            {
                                if (hh1 > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (hh1 < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k++;
                        }
                        if (g1.FCurValue != NA)
                        {
                            HL[k++].Text = "数值";
                            double f1 = g1.FCurValue;
                            HL[k].Text = String.Format("{0:F2}", f1);
                            if (pclose != NA)
                            {
                                if (f1 > pclose)
                                    HL[k].ForeColor = Color.Red;
                                if (f1 < pclose)
                                    HL[k].ForeColor = Color.Lime;
                            }
                            k += 1;
                        }
                    }

                    if (this.IsIntraView)
                        DataHint.Height = 14 * LineHeight + 2;
                    if (this.IsBarView)
                        DataHint.Height = 20 * LineHeight + 2;

                }
                else
                {
                    for (int i = 0; i < 22; i++)
                        HL[i].Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion


    }
}
