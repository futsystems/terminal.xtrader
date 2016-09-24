using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TradingLib.XTrader.Stock
{
    public class FPanel:System.Windows.Forms.Panel
    {

        public FPanel()
        {
            this.DoubleBuffered = true;
        }

        //protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    //Rectangle rect1 = this.ClientRectangle;
        //    Rectangle rect2 = this.ClientRectangle;
        //    //rect1.Height = 20;
        //    //rect2.Y = 18;

        //    //LinearGradientBrush brush1 = new LinearGradientBrush(rect1, Color.WhiteSmoke, Color.LightGray, LinearGradientMode.Vertical);
        //    LinearGradientBrush brush2 = new LinearGradientBrush(rect2, Color.LightGray, Color.WhiteSmoke, LinearGradientMode.Vertical);
        //    //e.Graphics.FillRectangle(brush1, rect1);
        //    e.Graphics.FillRectangle(brush2, rect2);
        //    base.OnPaint(e);

        //}

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            
            //base.OnPaintBackground(e);
            //Rectangle rect1 = this.ClientRectangle;
            Rectangle rect2 = this.ClientRectangle;
            if (rect2.Height == 0 || rect2.Width == 0) return;
            //rect1.Height = 20;
            //rect2.Y = 18;

            //LinearGradientBrush brush1 = new LinearGradientBrush(rect1, Color.WhiteSmoke, Color.LightGray, LinearGradientMode.Vertical);
            LinearGradientBrush brush2 = new LinearGradientBrush(rect2, Color.LightGray, Color.WhiteSmoke, LinearGradientMode.Vertical);
            //e.Graphics.FillRectangle(brush1, rect1);
            e.Graphics.FillRectangle(brush2, rect2);
            //base.OnPaint(e);
        }

        //protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        //{
        //    //base.OnPaintBackground(e);

        //    Rectangle rect1 = this.ClientRectangle;
        //    Rectangle rect2 = this.ClientRectangle;
        //    rect1.Height = 20;
        //    rect2.Y = 20;

        //    Bitmap b1 = new Bitmap(rect1.Width, rect1.Height);
        //    Graphics g1 = Graphics.FromImage(b1);

        //    LinearGradientBrush brush1 = new LinearGradientBrush(rect1, Color.WhiteSmoke, Color.LightGray, LinearGradientMode.Vertical);
            
        //    g1.FillRectangle(brush1,new Rectangle(0,0,rect1.Width,rect1.Height));


        //    Bitmap b2 = new Bitmap(rect2.Width, rect2.Height);
        //    Graphics g2 = Graphics.FromImage(b2);

        //    LinearGradientBrush brush2 = new LinearGradientBrush(rect2, Color.LightGray, Color.WhiteSmoke, LinearGradientMode.Vertical);
        //    g2.FillRectangle(brush2, new Rectangle(0, 0, rect2.Width, rect2.Height));

        //    Bitmap bm = null;//绘制区域位图
        //    Graphics cv = e.Graphics;

        //    //cv.DrawImage(b1, b1.Width, 0);
        //    //cv.DrawImage(b2, b1.Width, rect1.Height);

        //}
    }
}
