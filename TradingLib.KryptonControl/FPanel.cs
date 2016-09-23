using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TradingLib.KryptonControl
{
    public class FPanel:System.Windows.Forms.Panel
    {

        public FPanel()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Rectangle rect1 = this.ClientRectangle;
            Rectangle rect2 = this.ClientRectangle;
            rect1.Height = 20;
            rect2.Y = 18;

            LinearGradientBrush brush1 = new LinearGradientBrush(rect1, Color.WhiteSmoke, Color.LightGray, LinearGradientMode.Vertical);
            LinearGradientBrush brush2 = new LinearGradientBrush(rect2, Color.LightGray, Color.WhiteSmoke, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush1, rect1);
            e.Graphics.FillRectangle(brush2, rect2);
        }
    }
}
