using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.KryptonControl
{
    //绘制的一个单元格
    public class QuoteCell
    {
        public event DebugDelegate SendDebutEvent;
        void debug(string msg)
        {
            if (SendDebutEvent != null)
                SendDebutEvent(msg);
        }



        public string _dispformat;
        public string DisplayFormat { get { return _dispformat; } set { _dispformat = value; } }
        string _colname;
        public string ColumnName { get { return _colname; } set { _colname = value; } }
        string _symbol;
        public string Symbol { get { return _symbol; } set { _symbol = value; } }
        decimal _value;
        public decimal Value { get { return _value; } set { _value = value; } }
        CellStyle _cellStyle;
        public CellStyle CellStyle { get { return _cellStyle; } set { _cellStyle = value; } }

        #region 构造函数
        public QuoteCell(string colname, CellStyle cellstyle, string disfromat)
        {
            _colname = colname;
            _cellStyle = new CellStyle(cellstyle);
            _dispformat = disfromat;
            //我们需要通过colname来指定默认的cellstyle中文字颜色以及数字的显示方式
            switch (colname)
            {
                case QuoteListConst.SYMBOL:
                    _cellStyle.FontColor = Color.Yellow;
                    break;
                case QuoteListConst.SYMBOLNAME:
                    _cellStyle.FontColor = Color.Yellow;
                    break;
                case QuoteListConst.LASTSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.ASKSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.BIDSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.VOL:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.OI:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.OICHANGE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case QuoteListConst.LASTSETTLEMENT:
                    _cellStyle.FontColor = Color.Silver;
                    break;
                default:
                    break;

            }
        }

        public QuoteCell(string colname, CellStyle cellstyle, string value, string disformat)
            : this(colname, cellstyle, disformat)
        {
            _symbol = value;
        }

        public QuoteCell(string colname, CellStyle cellstyle, decimal value, string disformat)
            : this(colname, cellstyle, disformat)
        {
            _value = value;
        }

        #endregion



        public void Paint(PaintEventArgs e, System.Drawing.RectangleF cellRect, QuoteStyle quoteStyle)
        {
            //debug("更新段元格:"+this.ColumnName );
            Graphics g = e.Graphics;
            //_quoteStyle = quoteStyle;
            //用颜色填充单元格
            g.FillRectangle(CellStyle.BackBrush, cellRect);
            //debug("填充颜色:" + CellStyle.BackColor.ToString());
            //绘制方形区域边界
            //g.DrawRectangle(LinePen, cellRect);
            //绘制单元格 提供了单元格的rectangle就得到了 方格位置与长 宽
            //debug("fill the cellrect:"+"x:"+cellRect.X.ToString()+" y:"+cellRect.Y.ToString()+" width:"+cellRect.Width.ToString()+" height"+cellRect.Height.ToString());
            g.DrawRectangle(CellStyle.LinePen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
            //绘制出文字
            //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
            //debug(_value.ToString());
            if (_colname == QuoteListConst.SYMBOL || _colname == QuoteListConst.SYMBOLNAME)
            {
                g.DrawString(_symbol, CellStyle.SymbolFont, CellStyle.FontBrush, cellRect.X, cellRect.Y + quoteStyle.RowHeight - CellStyle.QuoteFont.Height);
            }
            
            else
                g.DrawString(string.Format(DisplayFormat, _value), CellStyle.QuoteFont, CellStyle.FontBrush, cellRect.X, cellRect.Y + quoteStyle.RowHeight - CellStyle.QuoteFont.Height);
            //cellRect.Y + HeaderHeight - _headFont.Height + 2
        }

        /*
        //调用方指定value输出到表格
        public void Paint(PaintEventArgs e, System.Drawing.Rectangle cellRect, object value, QuoteStyle quoteStyle)
        {
            Graphics g = e.Graphics;
            //用颜色填充单元格
            g.FillRectangle(CellStyle.BackBrush, cellRect);
            
            //绘制方形区域边界
            //g.DrawRectangle(LinePen, cellRect);
            //绘制单元格 提供了单元格的rectangle就得到了 方格位置与长 宽
            //debug("fill the cellrect:"+"x:"+cellRect.X.ToString()+" y:"+cellRect.Y.ToString()+" width:"+cellRect.Width.ToString()+" height"+cellRect.Height.ToString());
            g.DrawRectangle(quoteStyle.LinePen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
            //绘制出文字
            //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
            debug(value.ToString());
            g.DrawString(value.ToString(), CellStyle.Font, CellStyle.FontBrush, cellRect.X, cellRect.Y + quoteStyle.RowHeight - CellStyle.Font.Height);
            //cellRect.Y + HeaderHeight - _headFont.Height + 2
        }**/



    }
}
