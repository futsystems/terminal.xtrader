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

        #region PHint相关事件
        private void phint_MouseDown(object sender, MouseEventArgs e)
        {
            //记住原来的位置，这里是鼠标相对于button1的位置
            oriPoint = e.Location;
            this.Cursor = Cursors.SizeAll;
        }

        private void phint_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //这个地方记录的是button1对于窗口的位置
                Point newPos = DataHint.Location;
                //适当调整button1的位置
                newPos.Offset(e.Location.X - oriPoint.X, e.Location.Y - oriPoint.Y);
                //保证拖动后控件还在当前窗体的可见范围内
                if (new Rectangle(new Point(0, 0), this.Size).Contains(newPos))
                    DataHint.Location = newPos;
            }
            this.Cursor = Cursors.Default;
        }

        private void phint_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //这个地方记录的是button1对于窗口的位置
                Point newPos = DataHint.Location;
                //适当调整button1的位置
                newPos.Offset(e.Location.X - oriPoint.X, e.Location.Y - oriPoint.Y);
                //保证拖动后控件还在当前窗体的可见范围内
                //if (new Rectangle(0, 0, this.Width - DataHint.Width, this.Height - DataHint.Height * 5 / 2).Contains(newPos))
                DataHint.Location = newPos;

            }
        }

        private void HintClose_Click(object sender, EventArgs e)
        {
            DataHint.Visible = false;
        }
        #endregion

    }
}
