using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TradingLib.TraderAPI
{
    public class FButton:System.Windows.Forms.Button
    {
        private bool mouseover = false;//鼠标经过
        private bool mousedown = false;
        public FButton()
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {

            //在这里用自己的方法来绘制Button的外观(其实也就是几个框框)
            Graphics g = e.Graphics; 
            g.Clear(this.BackColor);
            Rectangle rect = e.ClipRectangle;
            rect = new Rectangle(rect.X,rect.Y,rect.Width-1,rect.Height-2);
            //g.ReleaseHdc();
            if (mouseover)
            {
                if (Focused && !mousedown)
                {
                    Utils_GDI.DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonFocuseAndMouseOver);
                    return;
                }
                if (mousedown)
                {
                    Utils_GDI.DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonDown);
                    return;
                }


                Utils_GDI.DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonMouseOver);
                return;
            }
            if (Focused)
            {
                Utils_GDI.DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonFocuse);
                return;
            }
            Utils_GDI.DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonNormal);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            mousedown = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            mousedown = false;
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseover = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            mouseover = false;
            base.OnMouseLeave(e);   
        }
    
    }
}
