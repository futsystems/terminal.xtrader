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
        //属性获得和设置
        [DefaultValue("Aqua")]
        Color _UPColor = Color.Red;
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
        [DefaultValue("Aqua")]
        Color _DNColor = Color.Green;
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
        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _headFont = new Font("Arial,style=Bold", 10);
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
        [DefaultValue("Aqua")]
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

        [DefaultValue("Aqua")]
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

        [DefaultValue("Arial, 10.5pt, style=Bold")]
        Font _quoteFont = new Font("Aria", 10, FontStyle.Bold);
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

        [DefaultValue("Arial, 10.5pt")]
        Font _symbolFont = new Font("Aria", 10);
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
        [DefaultValue("Aqua")]
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

        [DefaultValue("Blue")]
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



        [DefaultValue("SlateGray")]
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

        [DefaultValue("LightSlateGray")]
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

        [DefaultValue("Silver")]
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

        [DefaultValue("False")]
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
        EnumQuoteType _quoteType = EnumQuoteType.CNQUOTE;
        public EnumQuoteType QuoteType
        {
            get { return _quoteType; }
            set { _quoteType = value; }
        }

        #endregion

    }
}
