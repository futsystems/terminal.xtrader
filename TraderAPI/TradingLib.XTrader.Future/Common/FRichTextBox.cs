using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public class  FRichTextBox:System.Windows.Forms.RichTextBox
    {
        static Color lineColor = Color.FromArgb(127, 157, 185);
        ILog logger = LogManager.GetLogger("FGrid");
        Pen _borderPen = new Pen(lineColor);
        public FRichTextBox()
        {

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = e.ClipRectangle;
            e.Graphics.DrawRectangle(_borderPen, rect);
        }

    }
}
