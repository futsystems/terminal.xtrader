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
    //单元格样式 单元格背景色,字体颜色,字体等信息
    public class CellStyle
    {
        //重复使用静态变量Brush以及Pen避免重复重复创建
        public static SolidBrush _brush = new SolidBrush(Color.Black);
        public static Pen _pen = new Pen(Color.Black, 1);

        public CellStyle(Color backcolor, Color fontcolor, Font quotefont,Font symbolfont,Color gridColor)
        {
            BackColor = backcolor;
            FontColor = fontcolor;
            QuoteFont = quotefont;
            SymbolFont = symbolfont;
            LineColor = gridColor;
        }

        

        public CellStyle(CellStyle copythis)
        {
            BackColor = copythis.BackColor;
            FontColor = copythis.FontColor;
            QuoteFont = copythis.QuoteFont;
            SymbolFont = copythis.SymbolFont;
            LineColor = copythis.LineColor;


        }
        Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                //设定线条颜色的同时 设定了LinePen
                _lineColor = value;
                
            }
        }
        //Pen _linepen;
        public Pen LinePen {
            get 
            {
                _pen.Color = _lineColor;
                return _pen;
            } 
        }


        Color _backColor;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
            }
        }

        
        public Brush BackBrush 
        { 
            get 
            {
                _brush.Color = this.BackColor;
                return _brush;
            } 
        
        }
        Color _FontColor;
        public Color FontColor
        {
            get { return _FontColor; }
            set
            {
                _FontColor = value;
            }
        }


        public Brush FontBrush
        {
            get
            {
                _brush.Color = _FontColor;
                return _brush; 
            
            }
        }
        Font _quoteFont;
        public Font QuoteFont { get { return _quoteFont; } set { _quoteFont = value; } }

        Font _symbolFont;
        public Font SymbolFont { get { return _symbolFont; } set { _symbolFont = value; } }
       

    }
    //报表样式
    public class QuoteStyle
    {
        public QuoteStyle(Color quoteback1,Color quoteback2,Font quotefont,Font symbolfront,Color linecolor, Color upcolor,Color dncolor,int headheight, int rowheight)
        {
            _quoteBackColor1 = quoteback1;
            _quoteBackColor2 = quoteback2;
            LineColor = linecolor;
            _headerheight = headheight;
            _rowheight = rowheight;
            _quoteFont = quotefont;
            _symbolFont = symbolfront;
            _upcolor = upcolor;
            _dncolor = dncolor;

        }

        Color _upcolor;
        public Color UPColor { get { return _upcolor; } set { _upcolor = value; } }
        Color _dncolor;
        public Color DNColor { get { return _dncolor; } set { _dncolor = value; } }

        Font _quoteFont;
        /// <summary>
        /// 报价字体
        /// </summary>
        public Font QuoteFont { get { return _quoteFont; } set { _quoteFont = value; } }
        Color _FontColor;

        Font _symbolFont;
        /// <summary>
        /// 合约字体
        /// </summary>
        public Font SymbolFont { get { return _symbolFont; } set { _symbolFont = value; } }

        public Color FontColor{get{return _FontColor;} set{_FontColor = value;}}


        Color _quoteBackColor1;
        /// <summary>
        /// 报价背景色 奇
        /// </summary>
        public Color QuoteBackColor1 { get { return _quoteBackColor1; } set { _quoteBackColor1 = value; } }
        Color _quoteBackColor2;
        /// <summary>
        /// 报价背景色 偶
        /// </summary>
        public Color QuoteBackColor2 { get { return _quoteBackColor2; } set { _quoteBackColor2 = value; } }

        Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                //设定线条颜色的同时 设定了LinePen
                _lineColor = value;
                _linepen = new Pen(_lineColor);
            }
        }
        Pen _linepen;
        public Pen LinePen { get { return _linepen; } }

        int _rowheight;
        /// <summary>
        /// 行高
        /// </summary>
        public int RowHeight { get { return _rowheight; } set { _rowheight = value; } }

        int _headerheight;
        /// <summary>
        /// 标题高度
        /// </summary>
        public int HeaderHeight { get { return _headerheight; } set { _headerheight = value; } }



    }
}
