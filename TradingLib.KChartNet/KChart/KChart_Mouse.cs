using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace CStock
{
    public partial class TStock
    {

        #region 响应鼠标事件
        /// <summary>
        /// 响应鼠标下击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_MouseDown(object sender, MouseEventArgs e)
        {
            TGongSi gs = curgs;
            PressXY.X = e.X;
            PressXY.Y = e.Y;

            curx = e.X;
            cury = e.Y;
            if (this.IsIntraView)
            {
                int hh = 0;
                for (int i = 0; i < fswindows; i++)
                {
                    if ((cury > hh) && (cury < (hh + FSGSH[i])))
                    {
                        tophigh = hh;
                        curgs = FSGS[i];
                        break;
                    }
                    hh += FSGSH[i];
                }
            }
            else
            {
                int hh = 0;
                for (int i = 0; i < techwindows; i++)
                {
                    if ((cury > hh) && (cury < (hh + GSH[i])))
                    {
                        tophigh = hh;
                        curgs = GS[i];
                        break;
                    }
                    hh += GSH[i];
                }
            }
            if (gs != null)
                gs.CurWindow = false;
            curgs.CurWindow = true;

            if (FSelectLine != null)
            {
                if (FCursorType == TCursorType.ctInLine)// 点击自画线
                {
                    FSelectLine.select = true;
                    this.Invalidate();
                    return;
                }
                if (FCursorType == TCursorType.ctNone)
                {
                    FSelectLine.select = false;
                    FSelectLine = null;
                    FSelectWhere = -1;
                    //this.Invalidate();
                    //return;
                }
            }

            //鼠标状态为空置状态，主图点击鼠标再移动实现放大缩小功能，其他窗口点击鼠标再移动实现移动
            if (FCursorType == TCursorType.ctNone)
            {
                if (this.IsBarView)
                {

                    if (tophigh == 0)
                    {
                        FCursorType = TCursorType.ctZoom;
                        NowXY = PressXY;
                    }
                    else
                        FCursorType = TCursorType.ctMove;
                }
            }

            // 画线状态
            if (FCursorType == TCursorType.ctDrawLine)
            {
                int lw = 0;
                if (DrawBoard.Visible)
                    lw = DrawBoard.Width;
                gs = curgs;
                if (gs != null)
                {
                    if (gs.RecordCount == 0)//没有数据不能画线
                    {
                        FCursorType = TCursorType.ctNone;
                        return;
                    }
                    if (FLineCount == 0)
                    {
                        FLine.fxx[0] = gs.StartIndex + trunc((e.X - lw - gs.Bounds.Left) / gs.FScale);
                        FLine.fyy[0] = gs.MaxValue - (e.Y - tophigh - gs.Bounds.Top) * (gs.MaxValue - gs.MinValue) / (gs.Bounds.Bottom - gs.Bounds.Top);
                        FLineCount++;
                        FLine.fxx[1] = FLine.fxx[0];
                        FLine.fyy[1] = FLine.fyy[0];
                        FLine.select = true;
                        gs.CurLine = FLine;
                        //RECT R1 = new RECT(gs.Bounds.X, gs.Bounds.Y, gs.Bounds.Right, gs.Bounds.Bottom);
                        //ClipCursor(ref R1);
                    }
                }
                return;
            }

            //根据不同状态动态生成菜单
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                PrepareStockMenu();
                StockMenu.Show(this.PointToScreen(e.Location));
                //StockMenu.Items.Clear();
            }
            this.Invalidate();

            Debug();

        }

        /// <summary>
        /// 响应鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_MouseMove(object sender, MouseEventArgs e)
        {
            int ww = this.Width;
            Rectangle r1 = new Rectangle();
            if (this.ShowDetailPanel)
                ww -= Board.Width;
            int lw = 0;
            if (DrawBoard.Visible)
                lw = DrawBoard.Width;
            int xx = e.X - lw;
            int yy = e.Y;

            //画线
            if (FCursorType == TCursorType.ctDrawLine)
            {
                if (FLineCount > 0)
                {
                    TGongSi g1 = curgs;
                    if (g1 != null)
                    {
                        FLine.fxx[FLineCount] = g1.StartIndex + trunc((xx - g1.Bounds.Left) / g1.FScale);
                        FLine.fyy[FLineCount] = g1.MaxValue - (yy - tophigh - g1.Bounds.Top) * (g1.MaxValue - g1.MinValue) / g1.Bounds.Height;
                        if (FLine.pointcount == 1)
                        {
                            FLine.fxx[0] = g1.StartIndex + trunc((xx - g1.Bounds.Left) / g1.FScale);
                            FLine.fyy[0] = g1.MaxValue - (yy - tophigh - g1.Bounds.Top) * (g1.MaxValue - g1.MinValue) / g1.Bounds.Height;
                        }
                    }
                }
                curx = e.X;
                cury = e.Y;
                this.Invalidate();
                return;
            }

            #region 移动或修改自画线
            if ((FCursorType == TCursorType.ctInLine) && (PressXY.X > -1) && (FSelectLine != null))
            {
                TGongSi g1 = curgs;
                if (g1 != null)
                {
                    int cx = g1.StartIndex + trunc((e.X - lw - g1.Bounds.Left) / g1.FScale);
                    int ccx = trunc((e.X - PressXY.X) / g1.FScale);
                    double cy = (e.Y - PressXY.Y);

                    cy = cy * (g1.MaxValue - g1.MinValue) / g1.Bounds.Height;
                    if (FSelectWhere == 1)
                    {
                        FSelectLine.fxx[0] = cx;
                        FSelectLine.fyy[0] = FSelectLine.fyy[0] - cy;
                    }
                    if (FSelectWhere == 2)
                    {
                        FSelectLine.fxx[1] = cx;
                        FSelectLine.fyy[1] = FSelectLine.fyy[1] - cy;
                    }
                    if (FSelectWhere == 3)
                    {
                        FSelectLine.fxx[2] = cx;
                        FSelectLine.fyy[2] = FSelectLine.fyy[2] - cy;
                    }
                    if (FSelectWhere == 4)
                    {
                        for (int i = 0; i < FSelectLine.pointcount; i++)
                        {
                            FSelectLine.fxx[i] = FSelectLine.fxx[i] + ccx;
                            FSelectLine.fyy[i] = FSelectLine.fyy[i] - cy;
                        }
                    }
                    this.Invalidate();
                    if (ccx != 0)
                        PressXY.X = e.X;
                    PressXY.Y = e.Y;
                }
                curx = e.X;
                cury = e.Y;
                return;
            }
            #endregion


            //缩放K线
            if (FCursorType == TCursorType.ctZoom)
            {
                //记录当前鼠标位置
                NowXY.X = e.X;
                NowXY.Y = e.Y;
                curx = e.X;
                cury = e.Y;
                //控件重绘，在绘制过程中会根据当前鼠标位置来绘制缩放矩形
                this.Invalidate();
                return;
            }

            #region 移动K线
            if (FCursorType == TCursorType.ctMove)
            {
                //拖动K线
                int lx, start;
                lx = e.X - PressXY.X;//鼠标移动值
                int move = (int)(lx / GS[0].FScale);//计算对应Bar数量
                if (move!=0)//如果移动的Bar数量大于0
                {
                    //计算左侧Bar起始位置在绘图过程中会对StartIndex的有效性进行判断
                    start = GS[0].StartIndex - move;
                    if (move>0)//右侧移动 如果StartIndex不固定 则切换成固定模式
                    {
                        if (!this.StartFix)
                        {
                            logger.Info("move right,startindex fix");
                            this.StartFix = true;
                        }
                    }
                    else//左侧移动 如果StartIndex固定 根据当前可显示Bar数量 与 最小显示数量 进行比较 切换成不固定模式
                    {
                        int minCnt = GetMinShowBarCount();
                        if (this.StartFix)
                        {
                            int dataAvabile = this.RecordCount - start;
                            //logger.Info("left move, data can show:" + dataAvabile.ToString() + " start:" + start.ToString());
                            if (dataAvabile <= minCnt)//预移动位置对应的有效数据小于最小显示数量 则达到边界,设置StartIndex为False
                            {
                                logger.Info("move left,minCnt hit,startindex not fix");
                                start = this.RecordCount - minCnt;
                                this.StartFix = false;
                            }
                        }
                        else
                        {
                            start = this.RecordCount - minCnt;
                        }
                    }

                    //设置起始位置
                    for (int i = 0; i < 10; i++)
                        GS[i].StartIndex = start;
                    PressXY.X = e.X;
                }

                curx = e.X;
                cury = e.Y;
                this.Invalidate();
                return;
            }
            #endregion


            #region 缩放绘图窗口
            if (PressXY.X > 0)
            {

                if (FCursorType == TCursorType.FsSize)
                {
                    int hh = FSGSH[0];
                    int j = 0;
                    for (int i = 1; i < fswindows; i++)
                    {
                        r1.X = 0;
                        r1.Y = hh - 2;
                        r1.Width = ww;
                        r1.Height = 4;
                        if (r1.Contains(PressXY))
                        {
                            if (((FSGSH[j] + (e.Y - PressXY.Y)) > 40) && ((FSGSH[j + 1] - (e.Y - PressXY.Y)) > 40))
                            {
                                FSGSH[j] = FSGSH[j] + (e.Y - PressXY.Y);
                                FSGSH[j + 1] = FSGSH[j + 1] - (e.Y - PressXY.Y);
                                PressXY.Y = e.Y;
                            }
                            break;
                        }
                        hh += FSGSH[i];
                        j++;
                    }
                    curx = e.X;
                    cury = e.Y;
                    this.Invalidate();
                    return;
                }

                if (FCursorType == TCursorType.KSize) //改变K线窗口大小
                {
                    int hh = GSH[0];
                    int j = 0;
                    for (int i = 1; i < techwindows; i++)
                    {
                        r1.X = 0;
                        r1.Y = hh - 2;
                        r1.Width = ww;
                        r1.Height = 4;
                        if (r1.Contains(PressXY))
                        {
                            if (((GSH[j] + (e.Y - PressXY.Y)) > 40) && ((GSH[j + 1] - (e.Y - PressXY.Y)) > 40))
                            {
                                GSH[j] = GSH[j] + (e.Y - PressXY.Y);
                                GSH[j + 1] = GSH[j + 1] - (e.Y - PressXY.Y);
                                PressXY.Y = e.Y;
                            }
                            break;
                        }
                        hh += GSH[i];
                        j++;
                    }
                    curx = e.X;
                    cury = e.Y;
                    this.Invalidate();
                    return;
                }
            }
            #endregion


            //重置鼠标状态
            this.Cursor = Cursors.Default;
            FCursorType = TCursorType.ctNone;
            //FSelectLine = null;

            #region 判定鼠标缩放窗口状态
            if (e.X < ww)//限定缩放可响应范围为绘图窗口的分界线上
            {
                if (this.IsIntraView)
                {
                    int hh = FSGSH[0];
                    for (int i = 1; i < fswindows; i++)
                    {

                        if ((e.Y > (hh - 2)) && (e.Y < (hh + 2)))
                        {
                            this.Cursor = Cursors.SizeNS;
                            FCursorType = TCursorType.FsSize;
                            curx = -1;
                            cury = -1;
                            break;
                        }
                        hh += FSGSH[i];
                    }
                }
                if(this.IsBarView)
                {
                    int hh = GSH[0];
                    for (int i = 1; i < techwindows; i++)
                    {

                        if ((e.Y > (hh - 2)) && (e.Y < (hh + 2)))
                        {
                            this.Cursor = Cursors.SizeNS;
                            FCursorType = TCursorType.KSize;
                            curx = -1;
                            cury = -1;
                            break;
                        }
                        hh += GSH[i];
                    }

                }
                if (FCursorType != TCursorType.ctNone)
                {
                    this.Invalidate();
                    Debug();
                    return;
                }
            }
            #endregion


            if (this.IsBarView && (FCursorType == TCursorType.ctNone))
            {
                if ((DL != null) && (curx < ww) && (cury > GS[0].toph) && (cury < (GS[0].toph + 8))) //在主窗口中
                {
                    int n = GS[0].FCurBar;
                    TBian b1 = GS[0].GetBian("date");
                    if (b1 != null)
                    {
                        int cdate = Convert.ToInt32(b1.value[n]);//获取当前日期
                        for (int i = 0; i < DL.Count; i++)
                        {
                            if (cdate == DL[i].date)//判定地雷信息与当前Bar日期
                            {
                                if (DiLei != i)
                                {
                                    Hint.Text = cdate.ToString() + " " + DL[i].name;
                                    Hint.Location = new Point(curx, GS[0].toph + 10 + 16);
                                    if (!Hint.Visible)
                                        Hint.Visible = true;
                                    DiLei = i;
                                }
                                Cursor = Cursors.Hand;
                                FCursorType = TCursorType.InDiLei;
                                break;
                            }
                        }
                    }
                }
            }


            if ((FCursorType != TCursorType.InDiLei) && (Hint.Visible))
            {
                if (Hint.Visible)
                {
                    Hint.Visible = false;
                    DiLei = -1;
                }
            }


            #region 选择自画线 需要在没有按住鼠标左键时进行判定
            if ((PressXY.X == -1) && (PressXY.Y == -1))
            {
                lw = 0;
                if (DrawBoard.Visible)
                    lw = DrawBoard.Width;

                //绘图完毕后 鼠标移动自动进入ctNone状态
                if (FCursorType == TCursorType.ctInLine)
                    FCursorType = TCursorType.ctNone;

                //找到当前指标窗口
                TGongSi gs = curgs;
                curx = e.X;
                cury = e.Y;
                if (this.IsIntraView)
                {
                    int hh = 0;
                    for (int i = 0; i < techwindows; i++)
                    {
                        if ((cury > hh) && (cury < (hh + FSGSH[i])))
                        {
                            tophigh = hh;
                            gs = FSGS[i];
                            break;
                        }
                        hh += FSGSH[i];
                    }
                }
                else
                {
                    int hh = 0;
                    for (int i = 0; i < techwindows; i++)
                    {
                        if ((cury > hh) && (cury < (hh + GSH[i])))
                        {
                            tophigh = hh;
                            gs = GS[i];
                            break;
                        }
                        hh += GSH[i];
                    }
                }

                if (gs.showline)//指标窗口是否有自画线
                {
                    for (int i = 0; i < gs.Lines.Count; i++)//遍历所有自画线
                    {
                        XLine pl = gs.Lines[i];
                        int j = pl.AtLine(gs, e.X - lw, e.Y - tophigh);//判定光标当前所在自画线位置 
                        if (j > -1)
                        {
                            //执行自画线选择
                            if ((FSelectLine != null) && (pl != FSelectLine))
                                FSelectLine.select = false;
                            FSelectLine = pl;

                            FSelectWhere = j;
                            if (j == 4)
                                Cursor = Cursors.SizeAll;//自画线本身
                            else
                                Cursor = Cursors.Hand;
                            FCursorType = TCursorType.ctInLine;//自画线端点
                            break;
                        }
                        else
                        {
                            if (pl != FSelectLine)
                                pl.select = false;
                        }
                    }
                    if (FSelectLine != null)
                    {
                        PressXY.X = -1;
                        PressXY.Y = -1;
                    }
                }
            }
            #endregion

            this.Invalidate();

            Debug();
        }

        /// <summary>
        /// 响应松开鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_MouseUp(object sender, MouseEventArgs e)
        {
            //画线
            if ((FCursorType == TCursorType.ctDrawLine) && (FLineCount > 0))
            {
                curx = e.X;
                cury = e.Y;
                TGongSi g1 = curgs;
                int lw = 0;
                if (DrawBoard.Visible)
                    lw = DrawBoard.Width;
                if (g1 != null)
                {
                    FLine.fxx[FLineCount] = g1.StartIndex + trunc((e.X - lw - g1.Bounds.Left) / g1.FScale);
                    FLine.fyy[FLineCount] = g1.MaxValue - (e.Y - tophigh - g1.Bounds.Top) * (g1.MaxValue - g1.MinValue) / (g1.Bounds.Bottom - g1.Bounds.Top);
                    FLineCount++;
                    FLine.count = FLineCount;
                    FLine.fxx[FLineCount] = FLine.fxx[FLineCount - 1];
                    FLine.fyy[FLineCount] = FLine.fyy[FLineCount - 1];
                    if (FLine.pointcount <= FLineCount)
                    {
                        //RECT r1 = new RECT(0, 0, 0, 0);
                        //ClipCursor(ref r1);
                        if (FLine.type == 24)  // 输入字符串
                        {
                            AddString ad = new AddString();
                            if (ad.ShowDialog() == DialogResult.OK)
                            {
                                FLine.color = ad.Color1.BackColor;
                                FLine.str = ad.InputText.Text;
                                FLine.fontsize = Convert.ToInt32(ad.Size1.Value);
                            }
                            ad.Dispose();
                        }
                        XLine pl = new XLine(0);
                        FLine.count = FLine.pointcount;
                        pl.clone(FLine);
                        if (pl.pointcount == 1)
                        {
                            pl.fxx[0] = FLine.fxx[1];
                            pl.fyy[0] = FLine.fyy[1];
                        }
                        pl.select = false;
                        g1.Lines.Add(pl);
                        g1.CurLine = null;
                        PressXY.X = -1;
                        PressXY.Y = -1;
                        NowXY = PressXY;
                        FCursorType = TCursorType.ctNone;
                    }
                    this.Invalidate();
                }
                return;
            }

            //缩放
            if ((PressXY.X > 0) && (FCursorType == TCursorType.ctZoom))
            {
                NowXY.X = e.X;
                NowXY.Y = e.Y;
                if (GS[0].CurWindow)
                {
                    int ww = this.Width;
                    if (Board.Visible)
                        ww = ww - Board.Width - SP1.Width;
                    int xx1, xx2;
                    xx1 = Math.Min(PressXY.X, NowXY.X);
                    xx2 = Math.Max(PressXY.X, NowXY.X);
                    TGongSi gs = GS[0];
                    xx1 = xx1 - gs.leftYAxisWidth;
                    if (xx1 < 0)
                        xx1 = 0;
                    if (xx2 >= (ww - gs.rightYAxisWidth))
                        xx2 = ww - gs.rightYAxisWidth;
                    xx2 = xx2 - gs.leftYAxisWidth;
                    int b1 = gs.StartIndex + (int)(xx1 / gs.FScale);
                    if (b1 > gs.RecordCount)
                        b1 = gs.RecordCount;
                    int b2 = gs.StartIndex + (int)(xx2 / gs.FScale);
                    if (b2 > gs.RecordCount)
                        b2 = gs.RecordCount;
                    double sc = (double)(ww - gs.leftYAxisWidth - gs.rightYAxisWidth) / (double)(b2 - b1 + 1);
                    if (sc < (ww / 4) && (sc > 0.002))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            GS[i].StartIndex = b1;
                            GS[i].FScale = sc;
                        }
                    }
                }
            }
            //鼠标松开后 重置第一次按下鼠标的位置
            PressXY.X = -1;
            PressXY.Y = -1;
            NowXY = PressXY;
            FCursorType = TCursorType.ctNone;
            this.Invalidate();

            Debug();
        }

        /// <summary>
        /// 响应鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_MouseLeave(object sender, EventArgs e)
        {
            if (this.IsIntraView)
            {
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].SetCurxy(-1, -1);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    GS[i].SetCurxy(-1, -1);
                }
            }
            curx = -1;
            cury = -1;
            PressXY.X = -1;
            PressXY.Y = -1;
            NowXY = PressXY;
            this.Invalidate();
            Debug();
        }

        /// <summary>
        /// 复写鼠标滚轮
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)  //on  OnTextChanged(EventArgs e)
        {
            base.OnMouseWheel(e);
            if (MyMouseWheel != null)
            {
                MyMouseWheel(this, e);
            }

        }

        public event StockMouseWheel MyMouseWheel = null;
        #endregion
    }
}
