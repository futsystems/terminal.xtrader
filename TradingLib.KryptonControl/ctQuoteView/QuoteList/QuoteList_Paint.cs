using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {
        //关于绘图:如果不指定更新区域 Invalidate 会更新所有区域,这样会造成更新某个单元各 却需要更新所有的单元格 使得运行起来很不经济
        //默认模式中我们循环所有的列然后对呗该列区域与更新区域是否相交 若不相交则直接返回不进行更新,若相交则据需遍历所有列进行比较若有相交则更新该单元格。
        //这种模式是正常工作模式下动态更新价格信息所采用的方式。
        //我们需要找到效率相对最高的方式来进行工作。
        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            _brush.Color = this.BackColor;
            e.Graphics.FillRectangle(_brush, e.ClipRectangle);
            //绘制表头
            PaintHeader(e);
            //绘制我们需要显示的数据行
            //debug("begin:" + _beginIdx.ToString() + " end:" + _endIdx.ToString());
            try
            {
                for (int i = _beginIdx; i <= _endIdx; i++)
                {
                    //可以实现行的排列,当排列后我们将_idxQuoteRowMap重新映射到新的QuoteRow队列即可
                    _idxQuoteRowMap[i].Paint(e);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 绘制标题行
        /// </summary>
        /// <param name="e"></param>
        void PaintHeader(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (e.ClipRectangle.IntersectsWith(new Rectangle(0, 0, ClientSize.Width, DefaultQuoteStyle.HeaderHeight)))
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    PointF cellLocation = new PointF(GetColumnStarX(i), 0);
                    RectangleF cellRect = new RectangleF(cellLocation.X, cellLocation.Y, GetColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                    g.FillRectangle(new SolidBrush(HeaderBackColor), cellRect);
                    //绘制方形区域边界
                    //绘制单元格
                    g.DrawRectangle(DefaultQuoteStyle.LinePen, GetColumnStarX(i), 0, GetColumnWidth(i), DefaultQuoteStyle.HeaderHeight);
                    //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
                    g.DrawString(columns[i], HeaderFont, new SolidBrush(HeaderFontColor), cellRect.X, cellRect.Y + DefaultQuoteStyle.HeaderHeight - HeaderFont.Height);//-DefaultQuoteStyle.HeaderHeightHeaderHeight);
                }
            }
        }
    }
}
