using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.XTrader.Control
{
    //绘制的一个单元格
    public class QuoteCell
    {
        //public event DebugDelegate SendDebutEvent;
        void debug(string msg)
        {
            //if (SendDebutEvent != null)
            //    SendDebutEvent(msg);
        }



        string _dispformat;
        private string DisplayFormat { get { return _dispformat; } set { _dispformat = value; } }

        QuoteColumn _column;
        internal QuoteColumn Column { get { return _column; } }

        QuoteRow _row;

        string _symbol;
        public string Symbol { get { return _symbol; } set { _symbol = value; } }

        string _time="00:00:00";
        public string Time { get { return _time; } set { _time = value; } }

        double _value;
        public double Value { get { return _value; } set { _value = value; } }

        CellStyle _cellStyle;
        public CellStyle CellStyle { get { return _cellStyle; } set { _cellStyle = value; } }

        #region 构造函数
        public QuoteCell(QuoteRow row,QuoteColumn column, CellStyle cellstyle, string disfromat)
        {
            _row = row;
            _column = column;
            _cellStyle = new CellStyle(cellstyle);
            _dispformat = disfromat;

            if (_column.FieldType == EnumFileldType.SYMBOL || _column.FieldType == EnumFileldType.SYMBOLNAME)
            {
                _cellStyle.DrawFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                _cellStyle.DrawFormat.Alignment = StringAlignment.Far;
            }
            //我们需要通过colname来指定默认的cellstyle中文字颜色以及数字的显示方式
            switch (_column.FieldType)
            {
                case EnumFileldType.INDEX:
                    _cellStyle.FontColor = Color.Silver;
                    break;
                case EnumFileldType.SYMBOL:
                    _cellStyle.FontColor = Color.Yellow;
                    break;
                case EnumFileldType.TIME:
                    _cellStyle.FontColor = Color.Yellow;
                    break;
                case EnumFileldType.SYMBOLNAME:
                    _cellStyle.FontColor = Color.Yellow;
                    break;
                case EnumFileldType.LASTSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.ASKSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.BIDSIZE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.VOL:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.OI:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.OICHANGE:
                    _cellStyle.FontColor = Color.Yellow;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.CHANGE:
                    _dispformat = "{0:F2}";
                    break;
                case EnumFileldType.CHANGEPECT:
                    _dispformat = "{0:F2}";
                    break;
                case EnumFileldType.PRESETTLEMENT:
                case EnumFileldType.PRECLOSE:
                case EnumFileldType.PREOI:
                    _cellStyle.FontColor = Color.Silver;
                    break;
                case EnumFileldType.SSIDE:
                    _cellStyle.FontColor = row._quotelist.DefaultQuoteStyle.UPColor;
                    _dispformat = "{0:F0}";
                    break;
                case EnumFileldType.BSIDE:
                    _cellStyle.FontColor = row._quotelist.DefaultQuoteStyle.DNColor;
                    _dispformat = "{0:F0}";
                    break;
                default:
                    break;

            }
        }


        public QuoteCell(QuoteRow row,QuoteColumn column, CellStyle cellstyle, double value, string disformat)
            : this(row,column, cellstyle, disformat)
        {
            _value = value;
        }

        #endregion


        System.Drawing.RectangleF _cellRect;
        bool _setRect = false;
        public bool NeedCalcRect
        {
            get { return !_setRect; }
        }

        public RectangleF CellRect
        {
            get 
            {
                if (!_setRect) return RectangleF.Empty;
                return _cellRect; 
            }
            set 
            { 
                _cellRect = value;
                _setRect = true;
            }
        }

        /// <summary>
        /// 重置Cell绘图区域 尺寸或相关参数改变单元格对应的Rect发生变化 需要重新计算区域
        /// </summary>
        public void ResetRect()
        {
            _setRect = false;
        }


        public void Paint(PaintEventArgs e, QuoteStyle quoteStyle)
        {
            Graphics g = e.Graphics;
            //用颜色填充单元格
            g.FillRectangle(CellStyle.BackBrush, _cellRect);

            //绘制单元格 提供了单元格的rectangle就得到了 方格位置与长 宽
            //debug("fill the cellrect:"+"x:"+cellRect.X.ToString()+" y:"+cellRect.Y.ToString()+" width:"+cellRect.Width.ToString()+" height"+cellRect.Height.ToString());
            //绘制单元格 需要考虑线宽 实际绘制宽度需要扣除2倍线宽
            //g.DrawRectangle(CellStyle.LinePen, _cellRect.X, _cellRect.Y, _cellRect.Width, _cellRect.Height);
            //绘制出文字
            //矩形区域的定义是由左上角的坐标进行定义的,当要输出文字的时候从左上角坐标 + 本行高度度 - 实际输出文字的高度 + 文字距离下界具体
            if (_column.FieldType == EnumFileldType.SYMBOL)
            {
                g.DrawString(_symbol, CellStyle.QuoteFont, CellStyle.FontBrush, _cellRect.X, _cellRect.Y + (quoteStyle.RowHeight - CellStyle.SymbolFont.Height) / 2);
            }
            else if (_column.FieldType == EnumFileldType.SYMBOLNAME)
            {
                g.DrawString(_symbol, CellStyle.SymbolFont, CellStyle.FontBrush, _cellRect.X, _cellRect.Y + (quoteStyle.RowHeight - CellStyle.SymbolFont.Height) / 2);
            }
            else if (_column.FieldType == EnumFileldType.INDEX)
            {
                g.DrawString((_row.RowID + 1).ToString(), CellStyle.QuoteFont, CellStyle.FontBrush, (_cellRect.X + (_cellStyle.DrawFormat.Alignment == StringAlignment.Far ? Column.Width - 5 : 0)), _cellRect.Y + (quoteStyle.RowHeight - CellStyle.SymbolFont.Height) / 2, _cellStyle.DrawFormat);
            }
            else if (_column.FieldType == EnumFileldType.TIME)
            {
                g.DrawString(this.Time, CellStyle.QuoteFont, CellStyle.FontBrush, (_cellRect.X + (_cellStyle.DrawFormat.Alignment == StringAlignment.Far ? Column.Width - 5 : 0)), _cellRect.Y + (quoteStyle.RowHeight - CellStyle.SymbolFont.Height) / 2, _cellStyle.DrawFormat);
            }
            else
            {
                string val = double.IsNaN(_value) ? "—" : string.Format(DisplayFormat, _value);
                g.DrawString(val, CellStyle.QuoteFont, double.IsNaN(_value) ? _row._quotelist.DefaultQuoteStyle.NaNBrush : CellStyle.FontBrush, (_cellRect.X + (_cellStyle.DrawFormat.Alignment == StringAlignment.Far ? Column.Width - 5 : 0)), _cellRect.Y + (quoteStyle.RowHeight - CellStyle.QuoteFont.Height) / 2, _cellStyle.DrawFormat);
            }
        }
    }
}
