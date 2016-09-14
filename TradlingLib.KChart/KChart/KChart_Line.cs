using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class TStock
    {

        #region 自画线

        //清除所有自画线
        public void ClearLine()
        {
            for (int i = 0; i < 10; i++)
            {
                GS[i].ClearLine();
                FSGS[i].ClearLine();
            }
        }
        //隐藏所有自画线
        public void HideLine(bool value)
        {
            for (int i = 0; i < 10; i++)
            {
                GS[i].showline = value;
                FSGS[i].showline = value;
            }
        }

        /// <summary>
        /// 点击绘图面板按钮后 设定当前鼠标状态为绘图状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw(object sender, EventArgs e)
        {
            ToolStripButton sb = (ToolStripButton)sender;
            int t1 = Convert.ToInt32(sb.Tag);

            if (t1 == 0)
            {
                FCursorType = TCursorType.ctNone;
                this.Cursor = Cursors.Default;
                return;
            }
            if (t1 == 25)
            {
                ClearLine();
                this.Invalidate();
                return;
            }
            if (t1 == 26)
            {
                sb.Checked ^= true;
                HideLine(!sb.Checked);
                this.Invalidate();
                return;
            }
            if (t1 == 27)
            {
                LineShow ls = new LineShow();
                TGongSi gs = curgs;
                if (gs != null)
                    for (int i = 0; i < gs.Lines.Count; i++)
                    {
                        XLine xl = gs.Lines[i];
                        ListViewItem lv = new ListViewItem();
                        lv.Text = i.ToString();
                        lv.SubItems.Add(xl.type.ToString());
                        lv.SubItems.Add(xl.name);
                        lv.SubItems.Add(xl.fxx[0].ToString("F2"));
                        lv.SubItems.Add(xl.fyy[0].ToString("F2"));
                        lv.SubItems.Add(xl.fxx[1].ToString("F2"));
                        lv.SubItems.Add(xl.fyy[1].ToString("F2"));
                        lv.SubItems.Add(xl.fxx[2].ToString("F2"));
                        lv.SubItems.Add(xl.fyy[2].ToString("F2"));
                        ls.LView.Items.Add(lv);
                    }

                ls.ShowDialog();
                ls.Dispose();
                return;
            }
            FLine.SetStyle(t1);
            FCursorType = TCursorType.ctDrawLine;
            FLineCount = 0;
            Cursor = Cursors.Hand;

            Debug();
        }

        #endregion



    }
}
