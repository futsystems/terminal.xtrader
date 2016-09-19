using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TradingLib.KryptonControl
{
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class ctrlTickList : System.Windows.Forms.Control
    {
        public ctrlTickList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }




        int _defaultColumnWidth = 220;
        Pen _pen = new Pen(UIConstant.ColorLine, 1);
        int topHeight = 20;
        int lineHeight = 22;
        int fontHeight = UIConstant.QuoteFont.Height;
        SolidBrush _brushTime = new SolidBrush(Color.Silver);


        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, this.ClientRectangle);

            int columnCnt = this.Width / _defaultColumnWidth;
            if (columnCnt == 0)
            {
                g.DrawString("控件太窄", UIConstant.QuoteFont, Brushes.Red, 10, 10);
                return;
            }
            int columnWidth = this.Width / columnCnt;

            //string t = string.Format("{0} {1}",columnCnt,columnWidth);
            //g.DrawString(t, UIConstant.QuoteFont, Brushes.Red, 10, 10);

            Rectangle rect = new Rectangle();
            float tWidth = 0;
            System.Drawing.Font font = UIConstant.QuoteFont;

            string text = string.Empty;
            int lw = (columnWidth - 90) / 2;
            for (int i = 0; i < columnCnt; i++)
            {
                rect.X = i * columnWidth;

                if (i == columnCnt - 1)
                    columnWidth = this.Width - (columnCnt - 1) * columnWidth;

                int rowCnt = (this.Height - topHeight) / lineHeight;

                g.DrawLine(_pen, rect.X, 0, rect.X + columnWidth, 0);
                g.DrawLine(_pen, rect.X, topHeight, rect.X + columnWidth, topHeight);

                if (i > 0)
                    g.DrawLine(_pen, rect.X, 0, rect.X, this.Height);

                //string d = string.Format("{0}-{1}", columnCnt, columnWidth);
                //g.DrawString(d, UIConstant.QuoteFont, Brushes.Red, 10, 10);


                g.DrawString("时间", UIConstant.LableFont, Brushes.White, rect.X + 60 - 40, 0 + (lineHeight - fontHeight) / 2);
                g.DrawString("价格", UIConstant.LableFont, Brushes.White, rect.X + 60 + lw - 40, 0 + (lineHeight - fontHeight) / 2);
                g.DrawString("数量", UIConstant.LableFont, Brushes.White, rect.X + columnWidth - 50, 0 + (lineHeight - fontHeight) / 2);


                for (int j = 0; j < rowCnt; j++)
                {
                    rect.Y = (j) * lineHeight + topHeight;

                    int timeval = 1132;
                    double value = 12.22;
                    int vol = 2322;
                    int t = 1;

                    text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeval / 100, timeval % 100, 1);
                    tWidth = g.MeasureString(text, font).Width;
                    g.DrawString(text, UIConstant.QuoteFont, _brushTime, rect.X + 60 - tWidth, rect.Y + (lineHeight - fontHeight) / 2);


                    text = string.Format("{0:F2}", value);
                    tWidth = g.MeasureString(text, font).Width;
                    g.DrawString(text, font, Brushes.Gray, rect.X + 60 + lw - tWidth, rect.Y + (lineHeight - fontHeight) / 2);


                    text = string.Format("{0:D}", vol);
                    tWidth = g.MeasureString(text, font).Width;
                    g.DrawString(text, font, Brushes.Gray, rect.X + 60 + 2 * lw - tWidth, rect.Y + (lineHeight - fontHeight) / 2);

                    if (t == 1)
                        text = "B";
                    else
                        text = "S";
                    tWidth = g.MeasureString(text, font).Width;
                    if (t == 1)
                        g.DrawString(text, font, Brushes.Red, rect.X + columnWidth - 25, rect.Y + (lineHeight - fontHeight) / 2);
                    else
                        g.DrawString(text, font, Brushes.Lime, rect.X + columnWidth - 25, rect.Y + (lineHeight - fontHeight) / 2);

                }


            }
        }
    }
}
