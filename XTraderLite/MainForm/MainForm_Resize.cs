using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    //定义一个枚举，表示拖动方向
     public enum MouseDirection
     {
         Herizontal,//水平方向拖动，只改变窗体的宽度        
         Vertical,//垂直方向拖动，只改变窗体的高度 
         Declining,//倾斜方向，同时改变窗体的宽度和高度        
        None//不做标志，即不拖动窗体改变大小 
     }

    public partial class MainForm
    {

        private const int cGrip = 24;      // Grip size
        private const int cCaption = 32;   // Caption bar height;


        protected override void OnPaint(PaintEventArgs e)
        {
            //绘制移动框
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // 右下角同步变大
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip) //16左角
                {
                    m.Result = (IntPtr)11; // 右移
                    return;
                }
                if (pos.Y >= this.ClientSize.Height - cGrip)//下侧
                {
                    m.Result = (IntPtr)15; // 下移
                    return;
                }

            }
            base.WndProc(ref m);
        }
    }
}
