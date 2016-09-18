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

        #region 属性
        Color _UPColor = Color.FromArgb(255, 60, 57);
        public Color UPColor
        {
            get
            {
                return _UPColor;
            }
            set
            {
                _UPColor = value;
                Invalidate();
            }
        }
        Color _DNColor = Color.FromArgb(0, 231, 0);
        public Color DNColor
        {
            get
            {
                return _DNColor;
            }
            set
            {
                _DNColor = value;
                Invalidate();
            }
        }

       

        //调用Invalidate()可以保证设置属性之后重绘控件
        //[DefaultValue("Arial, 13.5pt, style=Bold")]
        Font _headFont = new Font("宋体", 10.5f, FontStyle.Bold);
        public Font HeaderFont
        {
            get
            {
                return _headFont;
            }
            set
            {
                _headFont = value;
                Invalidate();
            }
        }
        //[DefaultValue("Arial, 10.5pt, style=Bold")]
        //Arial,Gulim
        Font _quoteFont = new Font("Arial", 10f, FontStyle.Bold);
        public Font QuoteFont
        {
            get
            {
                return _quoteFont;
            }
            set
            {
                _quoteFont = value;
                _cellstyle.QuoteFont = value;
                Invalidate();
            }
        }

        //[DefaultValue("Arial, 10.5pt")]
        Font _symbolFont = new Font("System", 11f);
        public Font SymbolFont
        {
            get
            {
                return _symbolFont;
            }
            set
            {
                _symbolFont = value;
                Invalidate();
            }
        }


        Color _headFontColor = Color.FromArgb(0, 255, 255);
        public Color HeaderFontColor
        {
            get
            {
                return _headFontColor;
            }
            set
            {
                _headFontColor = value;
                Invalidate();
            }
        }

        Color _headBackColor = Color.FromArgb(0, 0, 0);
        public Color HeaderBackColor
        {
            get
            {
                return _headBackColor;
            }
            set
            {
                _headBackColor = value;
                Invalidate();
            }
        }

        
        Color _symbolFontColor = Color.Green;
        public Color SymbolFontColor
        {
            get
            {
                return _symbolFontColor;
            }
            set
            {
                _symbolFontColor = value;
                Invalidate();
            }
        }

        Color _selectedColor = Color.FromArgb(75, 75, 75);
        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
            }
        }



        Color _quoteBackColor1 = Color.FromArgb(0, 0, 0);
        public Color QuoteBackColor1
        {
            get
            {
                return _quoteBackColor1;
            }
            set
            {
                _quoteBackColor1 = value;
                //_cellstyle.BackColor = value;
                DefaultQuoteStyle.QuoteBackColor1 = value;
                Invalidate();
            }
        }


        Color _quoteBackColor2 = Color.FromArgb(0, 0, 0);
        public Color QuoteBackColor2
        {
            get
            {
                return _quoteBackColor2;
            }
            set
            {
                _quoteBackColor2 = value;
                //_cellstyle.BackColor = value;
                DefaultQuoteStyle.QuoteBackColor2 = value;
                Invalidate();
            }
        }


        Color _tableLineColor = Color.FromArgb(0, 0, 0);
        public Color TableLineColor
        {
            get
            {

                return _tableLineColor;
            }
            set
            {
                _tableLineColor = value;

                DefaultQuoteStyle.LineColor = value;
                Invalidate();
            }
        }

        bool _showmenu = false;
        public bool MenuEnable
        {
            get
            {

                return _showmenu;
            }
            set
            {
                _showmenu = value;
                if (_showmenu)
                {
                    initMenu();
                }

            }
        }

        [DefaultValue("CNQUOTE")]
        EnumQuoteListType _quoteType = EnumQuoteListType.STOCK_CN;
        /// <summary>
        /// 报价列表类别 用于显示不同市场的字段与排列顺序
        /// </summary>
        public EnumQuoteListType QuoteType
        {
            get { return _quoteType; }
            set { _quoteType = value; }
        }

        #endregion


        /// <summary>
        /// 标题高度
        /// </summary>
        private int HeaderHeight { get { return _headFont.Height + (int)(_headFont.Height*0.4); } }

        /// <summary>
        /// 报价行高度
        /// </summary>
        private int RowHeight { get { return _symbolFont.Height + (int)(_symbolFont.Height*0.3); } }

    }
}
