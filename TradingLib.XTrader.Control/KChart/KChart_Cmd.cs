using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CStock
{
    /// <summary>
    /// 执行命令接口
    /// 用于统一执行内部菜单或外部调用的命令
    /// </summary>
    public partial class TStock
    {
        #region 执行相关命令操作
        public void StockCmd(String Cmd)
        {
            ToolStripMenuItem m1 = new ToolStripMenuItem();
            m1.Text = Cmd;
            StockMenu_Click(m1, null);
        }

        private void StockMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m1 = (ToolStripMenuItem)sender;
            TGongSi gs = curgs;
            if (m1.Tag != null)
            {

                if (((int)m1.Tag >= 0x9000) & ((int)m1.Tag < 0x9999))//叠加
                {
                    FuncStr fs = new FuncStr();
                    string str = fs.Get(m1.Text);
                    if (gs.AddTech(str))
                        Invalidate();
                    return;
                }
                if (((int)m1.Tag >= 0x8000) & ((int)m1.Tag < 0x8999))
                {

                    gs.LoadWfc(m1.Text);
                    Invalidate();
                    return;
                }
                if (((int)m1.Tag >= 0x2000) & ((int)m1.Tag < 0x2999))
                {
                    if (this.IsIntraView)
                        this.IntradayViewWindowCount = (int)m1.Tag - 0x2000;
                    if(this.IsBarView)
                        this.BarViewWindowCount = (int)m1.Tag - 0x2000;
                    return;
                }
                if (((int)m1.Tag >= 0x1000) & ((int)m1.Tag < 0x1999))
                {
                    _cycle = m1.Text;
                    return;
                }

                //菜单切换周期
                if (((int)m1.Tag >= 0x4000) & ((int)m1.Tag < 0x4999))
                {

                    if (KFrequencyMenuClick != null)
                    {
                        KFrequencyType type = (KFrequencyType)((int)m1.Tag - 0x4000);
                        KFrequencyMenuClick(this, new KFrequencyMenuClickEventAargs(type));
                    }
                    return;
                }

                //响应日期菜单
                if (((int)m1.Tag >= 0x5000) & ((int)m1.Tag < 0x5999))
                {
                    int old = this.DaysForIntradayView;
                    this.DaysForIntradayView = (int)m1.Tag - 0x5000 + 1;
                    if (TimeViewDaysChanged != null && old != this.DaysForIntradayView)
                    {
                        TimeViewDaysChanged(this, this.DaysForIntradayView);
                    }
                    return;
                }
            }
            Command(m1.Text);
        }

        /// <summary>
        /// 执行菜单命令
        /// </summary>
        /// <param name="str"></param>
        public void Command(String str)
        {
            TGongSi gs = curgs;
            string s, s1;
            if (gs == null)
                return;

            //导出指标数据
            if (str == ChartMenuItems.MENU_SAVE_DATA)
            {
                sfd.Filter = "csv|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gs.SaveToTdx(sfd.FileName);
                }
                return;
            }
            //保存图像
            if (str == ChartMenuItems.MENU_SAVE_IMG)
            {
                int ww = Width;
                if (this.ShowDetailPanel)
                    ww -= Board.Width - 1;
                Bitmap bit = new Bitmap(ww, Height);//实例化一个和窗体一样大的bitmap
                Graphics g = Graphics.FromImage(bit);
                g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                g.CopyFromScreen(PointToScreen(Point.Empty), Point.Empty, Size);//只保存某个控件（这里是panel游戏区）
                //bit.Save("weiboTemp.png");//默认保存格式为PNG，保存成jpg格式质量不是很好
                sfd.DefaultExt = "*.png";
                sfd.Filter = "png|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bit.Save(sfd.FileName);
                }
                bit.Dispose();
                return;
            }
            if (str == "保存指标图像")
            {
                sfd.DefaultExt = "*.png";
                sfd.Filter = "png|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    int ww, hh = 20;
                    int lb = 0;
                    if (this.IsIntraView)
                    {
                        for (int i = 0; i < 10; i++)
                            if (curgs == FSGS[i])
                                hh = FSGSH[i];
                        lb = 0;
                    }
                    if(this.IsBarView)
                    {
                        for (int i = 0; i < 10; i++)
                            if (curgs == GS[i])
                                hh = GSH[i];
                        lb = this.StartIndex;
                    }
                    ww = Width;
                    if (this.ShowDetailPanel)
                        ww -= Board.Width - 1;
                    Bitmap bm = new Bitmap(ww, hh);
                    gs.OnPaint(bm);//, lb, gs.FScale, -1, -1);
                    bm.Save(sfd.FileName);
                    bm.Dispose();
                }
                return;
            }
            if (str == "显示指标栏")
            {
                ShowBottomTabMenu ^= true;
                return;
            }



            if (str == "叠加库公式")
            {
                SelectFunc sf = new SelectFunc();
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    string str1 = sf.GetStr;
                    if (gs.AddTech(str1))
                        Invalidate();
                }
                sf.Dispose();
                return;
            }
            if (str == "删除叠加")
            {
                gs.ClearTech();
                Invalidate();
                return;
            }

            //不复权
            if (str == ChartMenuItems.MENU_POWER_NO)
            {
                if (this.IsIntraView)
                    return;
                SetQuanStyle(QuanType.qsNone);
                return;
            }

            //前复权
            if (str == ChartMenuItems.MENU_POWER_BEFORE)
            {
                if (this.IsIntraView)
                    return;
                SetQuanStyle(QuanType.qsBefore);
                return;
            }

            //后复权
            if (str == ChartMenuItems.MENU_POWER_AFTER)
            {
                if (this.IsIntraView)
                    return;
                SetQuanStyle(QuanType.qsBack);
                return;
            }

            #region 指标菜单操作
            //指标用法注释
            if (str == ChartMenuItems.MENU_TECH_TXT)
            {
                Notes nt = new Notes();
                TStringList sl = new TStringList();
                sl.Text = gs.getprogtext();
                s1 = "";
                for (int i = 0; i < sl.Count; i++)
                {
                    s = sl[i].Trim();
                    if (s.IndexOf('*') > -1)
                    {
                        s1 += s.Substring(1) + "\r\n";
                    }
                }
                if (s1.Length > 0)
                    nt.textBox1.Text = s1;
                nt.ShowDialog();
                sl.Dispose();
                nt.Dispose();
                return;
            }
            //调整指标参数
            if (str == ChartMenuItems.MENU_TECH_ARGS)
            {
                tiao ta = new tiao();
                ta.gs = gs;
                ta.sk = this;
                if (ta.ShowDialog() == DialogResult.OK)
                    Invalidate();
                ta.Dispose();
                return;
            }
            //修改指标公式
            if (str == ChartMenuItems.MENU_TECH_EDIT)
            {
                TStringList src = new TStringList();
                TStringList dst = new TStringList();
                src.Text = gs.getprogtext();
                Compile cm = new Compile();
                cm.S_Compile(src, dst);
                string pass = dst.values("password:");
                bool pw = true;
                if (pass.Length > 0)
                {
                    pw = false;
                    CheckPass cp = new CheckPass();
                    if (cp.ShowDialog() == DialogResult.OK)
                    {
                        pw = cp.password.Text == pass;
                        if (!pw)
                            MessageBox.Show("密码错误,不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                if (pw)
                {
                    Editor ef = new Editor();
                    ef.SetText(gs.getprogtext().Trim());
                    if (ef.ShowDialog() == DialogResult.OK)
                    {
                        string str1 = ef.gongsi.Text;
                        gs.setprogtext(str1);
                        Invalidate();
                    }
                    ef.Dispose();
                }
                return;
            }
            //恢复默认指标
            if (str == ChartMenuItems.MENU_TECH_RESTORE)
            {
                FSGS[0].LoadWfc("fs");
                FSGS[1].LoadWfc("vol");
                for (int i = 2; i < 10; i++)
                    FSGS[2].LoadWfc(wfc[i].ToLower());

                GS[0].LoadWfc("ma");
                for (int i = 1; i < 10; i++)
                    GS[i].LoadWfc(wfc[i].ToLower());
                Invalidate();
                return;
            }
            //加载指标公式
            if (str == ChartMenuItems.MENU_TECH_LOAD)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string fn = ofd.FileName;
                    if (gs.loadprogram(fn))
                        Invalidate();
                }
            }

            //if (str == "加载库公式")
            //{
            //    SelectFunc sf = new SelectFunc();
            //    if (sf.ShowDialog() == DialogResult.OK)
            //    {
            //        string str1 = sf.GetStr;
            //        if (gs.setprogtext(str1))
            //            Invalidate();
            //    }
            //    sf.Dispose();
            //    return;
            //}


            #endregion


            if (str == "联系方式")
            {
                MessageBox.Show("*****");
                return;
            }
            if (str == "删除窗口")
            {
                if ((!this.IsIntraView) && (techwindows > 1))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (curgs == GS[i])
                        {
                            for (int j = i; j < 9; j++)
                            {
                                GS[j] = GS[j + 1];
                            }
                            GS[9] = curgs;
                            techwindows--;
                            break;
                        }
                    }
                }
            }
            if (str == "分时全高")
            {
                FsAll = FsAll ^ true;
                return;
            }
            if (str == "分时全宽")
            {
                FsFull = FsFull ^ true;
                return;
            }

            //盘口信息窗口
            if (str == ChartMenuItems.MENU_DETAILBOARD)
            {
                this.ShowDetailPanel ^= true;
                //Board.Visible ^= true;
                return;
            }

            //画线工具
            if (str == ChartMenuItems.MENU_DRAW)
            {
                this.ShowDrawToolBox ^= true;
                //DrawBoard.Visible ^= true;
                return;
            }

            if (str == ChartMenuItems.MENU_VIEW_TIME)
            {
                this.KChartViewType = KChartViewType.TimeView;
            }

            if (str == ChartMenuItems.MENU_VIEW_K)
            {
                this.KChartViewType = KChartViewType.KView;
            }

            //if (str == "分时切换")
            //{
            //    ShowFs = ShowFs ^ true;
            //    return;
            //}
            if (str == "显示光标")
            {
                ShowCrossCursor = ShowCrossCursor ^ true;
                return;
            }
            if (str == "显示顶边")
            {
                ShowTopHeader = ShowTopHeader ^ true;
                return;
            }
            if (str == "显示底边")
            {
                ShowBottomCalendar = ShowBottomCalendar ^ true;
                return;
            }
            if (str == "显示左边")
            {
                ShowLeftAxis = ShowLeftAxis ^ true;
                return;
            }
            if (str == "显示右边")
            {
                ShowRightAxis = ShowRightAxis ^ true;
                return;
            }

            if (str == "高清图像")
            {
                HighPicture ^= true;
                return;
            }

            //MessageBox.Show(str);
        }
        #endregion


    }
}
