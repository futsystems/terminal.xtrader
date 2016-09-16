using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CStock
{
    /// <summary>
    /// 底部Tab菜单绘制与交互
    /// </summary>
    public partial class TStock
    {

        #region 底部指标选择Tab
        private void Tab_Paint(object sender, PaintEventArgs e)
        {
            int x = 0;
            try
            {
                Brush br1 = new SolidBrush(Color.FromArgb(80, 80, 80));
                Brush bh1 = new SolidBrush(Color.FromArgb(28, 28, 28));
                Brush bh2 = new SolidBrush(Color.Yellow);
                Pen pn = new Pen(Color.FromArgb(128, 0, 0));

                Graphics cv = e.Graphics;
                Rectangle TabArea = Tab.ClientRectangle;

                Brush br = new SolidBrush(mBackColor);
                cv.FillRectangle(br, TabArea);
                cv.DrawLine(pn, 0, 0, 0, Tab.Height);
                cv.DrawLine(pn, 0, Tab.Height - 1, Tab.Width - 1, Tab.Height - 1);
                br.Dispose();

                x = 1;

                string s1 = "";
                if (curgs != null)
                    s1 = curgs.CurTech.techname.ToUpper();
                float t = 0;
                x = 2;
                for (int i = 0; i < TabList.Count; i++)
                {
                    x = i + 2000;
                    if (x == 2024)
                    {
                        int y = 2;
                    }
                    string value = TabList[i].ToUpper().Trim();
                    SizeF si = cv.MeasureString(value, Font);
                    TabWidth[i] = si.Width + 9;
                    if (t + TabWidth[i] > Tab.Width)
                        break;
                    if (i == TabType)
                        cv.FillRectangle(br1, t + 1, 0, TabWidth[i] - 1, Tab.Height - 1);
                    if (s1 == value)
                        cv.DrawString(value, Font, Brushes.Aqua, t + 4, (Tab.Height - si.Height) / 2);
                    else
                        cv.DrawString(value, Font, Brushes.White, t + 4, (Tab.Height - si.Height) / 2);
                    t += TabWidth[i];
                    cv.DrawLine(pn, t, 0, t, Tab.Height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(x.ToString() + ex.ToString());
            }
        }

        private void Tab_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            float t = 0;
            for (int i = 0; i < TabList.Count; i++)
            {
                t += TabWidth[i];
                if (t > Tab.Width)
                    break;
                if (t < x)
                    continue;
                string s = TabList[i];
                if (i > 2)
                {
                    if (curgs.LoadWfc(s))
                    {
                        Invalidate();
                        Tab.Invalidate();
                    }
                }
                else
                {
                    TabType = i;
                    if (i == 0)
                    {
                        FuncStr FS = new FuncStr();
                        TabList.Clear();
                        TabList.Add("指标");
                        TabList.Add("全部");
                        TabList.Add("高级");
                        for (i = 0; i < FS.Count(); i++)
                        {
                            if (FS.functype[i] == 1)
                                TabList.Add(FS.funcname[i]);
                        }
                        TabWidth = new float[TabList.Count];
                    }
                    if (i == 1)
                    {
                        FuncStr FS = new FuncStr();
                        TabList.Clear();
                        TabList.Add("指标");
                        TabList.Add("全部");
                        TabList.Add("高级");
                        for (i = 0; i < FS.Count(); i++)
                        {
                            TabList.Add(FS.funcname[i]);
                        }
                        TabWidth = new float[TabList.Count];

                    }
                    if (i == 2)
                    {

                        FuncStr FS = new FuncStr();
                        TabList.Clear();
                        TabList.Add("指标");
                        TabList.Add("全部");
                        TabList.Add("高级");
                        for (i = 0; i < FS.Count(); i++)
                        {
                            if (FS.functype[i] == 2)
                                TabList.Add(FS.funcname[i]);
                        }
                        TabWidth = new float[TabList.Count];
                    }
                    Tab.Invalidate();
                }
                break;
            }

        }

        #endregion

    }
}
