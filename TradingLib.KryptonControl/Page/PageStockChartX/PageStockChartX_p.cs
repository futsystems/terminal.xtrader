using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;


using STOCKCHARTXLib;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.KryptonControl
{
    public partial class PageStockChartX
    {

        public void ApplyChartStyle(ChartStyle style)
        {
            this.ThreeD = style.ThreeD;
            this.RightDrawingSpace = style.RightDrawingSpace;
            this.ScaleAlignment = style.ScaleAlignment;
            this.ScaleDecimalPlace = style.ScaleDecimalPlace;
            this.ScaleType = style.ScaleType;
            this.ShowPanelSeperator = style.ShowPanelSeperator;
            this.ShowTitle = style.ShowTitle;
            this.ShowXGrid = style.ShowXGrid;
            this.ShowYGrid = style.ShowYGrid;

            this.ColorDownBody = style.ColorDownBody;
            this.ColorUpBody = style.ColorUpBody;

            //3D绘图不需要绘制边框
            if (!this.ThreeD)
            {
                this.ColorDownBox = style.ColorDownBox;
                this.ColorUpBox = style.ColorUpBox;
            }
            this.ColorSeperator = style.ColorPanelSeperator;
        }


        #region 属性


        int _visibleRecoredCount = 150;
        /// <summary>
        /// 默认图表显示Bar个数
        /// </summary>
        public int DefaultVisibleRecordCount
        {
            get { return _visibleRecoredCount; }

            set
            {
                _visibleRecoredCount = value;
                StockChartX1.VisibleRecordCount = _visibleRecoredCount;
            }
        }

        bool _threeD = true;
        public bool ThreeD
        {
            get { return _threeD; }
            set
            {
                _threeD = value;
                StockChartX1.ThreeDStyle = _threeD;
            }
        }

        System.Drawing.Color _upBodyColor = System.Drawing.Color.Black;
        public System.Drawing.Color ColorUpBody
        {
            get { return _upBodyColor; }
            set
            {
                _upBodyColor = value;
                StockChartX1.UpColor = _upBodyColor;
            }
        }
        System.Drawing.Color _upBoxColor = ChartStyle.COLOR_UP;
        public System.Drawing.Color ColorUpBox
        {
            get { return _upBoxColor; }
            set
            {
                _upBoxColor = value;
                if (!this.ThreeD)//3D模式不设置边框颜色
                {
                    StockChartX1.CandleUpOutlineColor = _upBoxColor;
                }
            }
        }
        System.Drawing.Color _downBodyColor = ChartStyle.COLOR_DOWN;
        public System.Drawing.Color ColorDownBody
        {
            get { return _downBodyColor; }
            set
            {
                _downBodyColor = value;
                StockChartX1.DownColor = _downBodyColor;

            }
        }
        System.Drawing.Color _downBoxColor = ChartStyle.COLOR_DOWN;
        public System.Drawing.Color ColorDownBox
        {
            get { return _downBoxColor; }
            set
            {
                _downBoxColor = value;
                if (!this.ThreeD)//3D模式不设置边框颜色
                {
                    StockChartX1.CandleDownOutlineColor = _downBoxColor;
                }
            }
        }

        System.Drawing.Color _seperatorColor = ChartStyle.COLOR_SEPERATOR;
        public System.Drawing.Color ColorSeperator
        {
            get { return _seperatorColor; }
            set
            {
                _seperatorColor = value;
                StockChartX1.HorizontalSeparatorColor = _seperatorColor;
            }
        }




        bool _showTitle = true;
        public bool ShowTitle
        {
            get { return _showTitle; }
            set
            {
                _showTitle = value;
                StockChartX1.DisplayTitles = _showTitle;
            }
        }

        EnumScaleAlignment _xScaleAlignment = EnumScaleAlignment.Left;
        /// <summary>
        /// Y轴坐标位置
        /// </summary>
        public EnumScaleAlignment ScaleAlignment
        {
            get
            {
                return _xScaleAlignment;
            }
            set
            {
                _xScaleAlignment = value;
                if (_xScaleAlignment == EnumScaleAlignment.Left)
                {
                    StockChartX1.Alignment = (int)STOCKCHARTXLib.ScaleType.stAlignLeft;
                }
                if (_xScaleAlignment == EnumScaleAlignment.Right)
                {
                    StockChartX1.Alignment = (int)STOCKCHARTXLib.ScaleType.stAlignRight;
                }

            }
        }

        EnumScaleType _xScaleType = EnumScaleType.Linear;
        /// <summary>
        /// Y轴坐标类型
        /// </summary>
        public EnumScaleType ScaleType
        {
            get
            {
                return _xScaleType;
            }
            set
            {
                _xScaleType = value;
                if (_xScaleType == EnumScaleType.Linear)
                {
                    StockChartX1.ScaleType = STOCKCHARTXLib.ScaleType.stLinearScale;
                }
                if (_xScaleType == EnumScaleType.Log)
                {
                    StockChartX1.ScaleType = STOCKCHARTXLib.ScaleType.stSemiLogScale;
                }
            }
        }

        bool _showXGrid = false;
        /// <summary>
        /// 是否显示X轴向格子线
        /// </summary>
        public bool ShowXGrid
        {
            get { return _showXGrid; }
            set
            {
                _showXGrid = value;
                StockChartX1.XGrid = _showXGrid;
            }
        }

        bool _showYGrid = false;
        /// <summary>
        /// 是否显示Y轴向格子线
        /// </summary>
        public bool ShowYGrid
        {
            get { return _showYGrid; }
            set
            {
                _showYGrid = value;
                StockChartX1.YGrid = _showYGrid;
            }
        }

        int _scaleDecimalPlace = 2;
        /// <summary>
        /// X轴价格小数点位置
        /// </summary>
        public int ScaleDecimalPlace
        {
            get { return _scaleDecimalPlace; }
            set
            {
                int val = value;
                if (val < 0 || val > 4) val = 2;
                _scaleDecimalPlace = val;
                StockChartX1.ScalePrecision = val;
            }
        }

        int _rightDrawingSpace = 10;
        /// <summary>
        /// K线图右侧绘图空距
        /// </summary>
        public int RightDrawingSpace
        {
            get { return _rightDrawingSpace; }
            set
            {
                int val = value;
                if (val < 1 || val > 300) val = 75;
                _rightDrawingSpace = val;
                StockChartX1.RightDrawingSpacePixels = val;
            }

        }

        bool _showPanelSeperator = true;
        /// <summary>
        /// 是否显示绘图Panel分割线
        /// </summary>
        public bool ShowPanelSeperator
        {
            get { return _showPanelSeperator; }
            set
            {
                _showPanelSeperator = value;
                //设置颜色
                StockChartX1.HorizontalSeparators = _showPanelSeperator;
            }
        }

        bool _showCrossHairs = false;
        /// <summary>
        /// 是否显示定位十字线
        /// </summary>
        public bool ShowCrossHairs
        {
            get { return _showCrossHairs; }
            set
            {
                _showCrossHairs = value;
                StockChartX1.CrossHairs = _showCrossHairs;
            }
        }
        #endregion
    }
}
