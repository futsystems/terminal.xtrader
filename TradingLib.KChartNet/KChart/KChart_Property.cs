using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace CStock
{
    public partial class TStock
    {
        /// <summary>
        /// 分时图标显示日期
        /// IntradayView
        /// BarView
        /// </summary>
        public int DaysForIntradayView
        {
            get { return FSGS[0].days; }
            set
            {
                int old = this.DaysForIntradayView;

                if (value > 0 && value < 11)
                {
                    for (int i = 0; i < 10; i++)
                        FSGS[i].days = value;
                    this.Invalidate();

                    //if (StockClick != null)
                    //{
                    //    StockEventArgs ee = new StockEventArgs(ClickStyle.csDays);  
                    //    StockClick(this, ee);
                    //}

                    if (TimeViewDaysChanged != null && old != value)
                    {
                        TimeViewDaysChanged(this, value);
                    }
                }
            }
        }

        /// <summary>
        /// 数据个数
        /// </summary>
        public int RecordCount
        {
            get
            {
                if (this.IsIntraView)
                    return FSGS[0].RecordCount;
                if(this.IsBarView)
                    return GS[0].RecordCount;
                return 0;
            }
        }


        /// <summary>
        /// Bar宽度
        /// </summary>
        public double BarWidth
        {
            get { return GS[0].FScale; }
            private set
            {
                for (int i = 0; i < 10; i++)
                    GS[i].FScale = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否处于分时绘图模式
        /// </summary>
        public bool IsIntraView
        {
            get 
            { 
                return _viewType == KChartViewType.TimeView; 
            }
        }

        /// <summary>
        /// 是否处于K线图模式
        /// </summary>
        public bool IsBarView
        {
            get
            {
                return _viewType == KChartViewType.KView;
            }
        }

        KChartViewType _viewType = KChartViewType.KView;

        /// <summary>
        /// 显示类别
        /// </summary>
        public KChartViewType KChartViewType
        {
            get
            {
                return _viewType;
            }
            set
            {
                _viewType = value;

                //模式切换中进行的初始化
                if (this.IsIntraView)
                {
                    curgs = FSGS[0];
                    Tab.Visible = false;
                }

                if (this.IsBarView)
                {
                    Tab.Visible = Ftab;
                    curgs = GS[0];
                }

                for (int i = 1; i < 10; i++)
                {
                    FSGS[i].CurWindow = false;
                    GS[i].CurWindow = false;
                }
                if (DataHint.Visible)
                    DataHint.Visible = false;

                Invalidate();


                //if (KChartModeChange != null)// && oldShowfs != this.FShowFS)
                //{
                //    KChartModeChange(this, new KChartModeChangeEventArgs(_viewType));

                //}
            }
        }

        /// <summary>
        /// 是否显示分时
        /// </summary>
        //public Boolean ShowFs0
        //{
        //    get
        //    {
        //        return _IsIntraView;
        //    }
        //    set
        //    {
        //        bool oldShowfs = this.IsIntraView;
        //        _IsIntraView = value;
        //        if (value)
        //        {
        //            curgs = FSGS[0];
        //            Tab.Visible = false;
        //        }
        //        else
        //        {
        //            Tab.Visible = Ftab;
        //            curgs = GS[0];
        //        }
        //        for (int i = 1; i < 10; i++)
        //        {
        //            FSGS[i].CurWindow = false;
        //            GS[i].CurWindow = false;
        //        }
        //        if (DataHint.Visible)
        //            DataHint.Visible = false;

        //        Invalidate();

        //        if (KChartModeChange != null)// && oldShowfs != this.FShowFS)
        //        {
        //            KChartViewType type = KChartViewType.TimeView;
        //            if (this.IsIntraView) type = KChartViewType.TimeView;
        //            if (!this.IsIntraView) type = KChartViewType.KView;

        //            KChartModeChange(this, new KChartModeChangeEventArgs(type));

        //        }
        //    }
        //}

        #region Show Config

        public Boolean FsAll
        {
            get { return FSGS[0].FsAll; }
            set
            {
                FSGS[0].FsAll = value;
                this.Invalidate();
            }
        }
        public Boolean FsFull
        {
            get { return FSGS[0].FsFull; }
            set
            {
                FSGS[0].FsFull = value;
                Invalidate();
            }
        }
        public Boolean HighPicture
        {
            get { return FSGS[0].High; }
            set
            {
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].High = value;
                    GS[i].High = value;
                }
                Invalidate();
            }
        }


        /// <summary>
        /// 是否显示绘图面板
        /// </summary>
        public Boolean ShowDrawToolBox
        {
            get { return DrawBoard.Visible; }
            set
            {
                if (this.IsBarView)
                {
                    DrawBoard.Visible = value;
                    ReSizeBarChart(false);
                }
            }
        }

        /// <summary>
        /// 是否显示盘口面板
        /// </summary>
        public Boolean ShowDetailPanel
        {
            get { return Board.Visible; }
            set
            {
                SP1.Visible = value;
                Board.Visible = value;
                ReSizeBarChart(false);
                //this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示十字光标
        /// </summary>
        public Boolean ShowCrossCursor
        {
            get { return GS[0].ShowCursor; }
            set
            {
                if (value)
                {
                    if (!DataHint.Visible)
                        DataHint.Visible = true;
                }
                else
                {
                    if (DataHint.Visible)
                        DataHint.Visible = false;
                }
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].ShowCursor = value;
                    GS[i].ShowCursor = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示顶部数值输出区域
        /// </summary>
        public Boolean ShowTopHeader
        {
            get
            {
                return FShowTop;
            }
            set
            {
                FShowTop = value;
                FSGS[0].ShowTop = value;
                GS[0].ShowTop = value;
                this.Invalidate();
            }

        }

        /// <summary>
        /// 是否显示指标选择面板
        /// </summary>
        public Boolean ShowBottomTabMenu
        {
            get { return Ftab; }
            set
            {
                Ftab = value;
                if (this.IsBarView)
                    Tab.Visible = Ftab;
                ReSizeBarChart(false);
            }
        }

        /// <summary>
        /// 是否显示底部区域
        /// </summary>
        public Boolean ShowBottomCalendar
        {
            get
            {
                return FShowBottom;
            }
            set
            {
                FShowBottom = value;
                FSGS[techwindows - 1].ShowBottom = value;
                GS[techwindows - 1].ShowBottom = value;
                this.Invalidate();
            }

        }

        /// <summary>
        /// 是否显示左侧坐标轴
        /// </summary>
        public bool ShowLeftAxis
        {
            get
            {
                return FShowLeft;
            }
            set
            {
                FShowLeft = value;
                for (int i = 0; i < 10; i++)
                {
                    GS[i].ShowLeft = value;
                }
                this.Invalidate();
            }

        }

        /// <summary>
        /// 是否显示右侧坐标轴
        /// </summary>
        public bool ShowRightAxis
        {
            get
            {
                return FShowRight;
            }
            set
            {
                FShowRight = value;
                for (int i = 0; i < 10; i++)
                {
                    GS[i].ShowRight = value;
                }
                this.Invalidate();
            }

        }
        #endregion




        /// <summary>
        /// K线图 技术窗口
        /// </summary>
        public int BarViewWindowCount
        {
            get { return techwindows; }
            set
            {
                if ((value < 1) || (value > 10))
                {
                    MessageBox.Show("窗口数量在1到10之间");
                    return;
                }
                if (value != techwindows)
                {
                    GS[techwindows - 1].ShowBottom = false;
                    techwindows = value;
                    GS[techwindows - 1].ShowBottom = FShowBottom;
                    ReSizeBarChart(false);
                }

            }
        }


        /// <summary>
        /// 分时技术窗口数量
        /// </summary>
        public int IntradayViewWindowCount
        {
            get { return fswindows; }
            set
            {
                if ((value < 1) || (value > 10))
                {
                    MessageBox.Show("窗口数量在1到10之间");
                    return;
                }
                if (value != fswindows)
                {
                    FSGS[fswindows - 1].ShowBottom = false;
                    int h1 = Height;
                    if (Tab.Visible)
                        h1 -= Tab.Height;
                    int hh = (h1 / (value + 1));
                    FSGSH[0] = hh * 2;
                    for (int i = 1; i < value; i++)
                        FSGSH[i] = hh;
                    for (int i = 0; i < value; i++)
                        FSGS[i].run();
                    fswindows = value;
                    FSGS[fswindows - 1].ShowBottom = true;
                    Invalidate();
                }
            }
        }



        Color mBackColor = Color.Black;
        public Color KChartBackColor
        {
            get { return mBackColor; }

            set
            {
                mBackColor = value;
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].BackColor = value;
                    GS[i].BackColor = value;
                }
                this.Invalidate();
                //TabBox.Invalidate(); 
            }
        }
        Color mBoardColor = Color.Maroon;
        public Color KChartLineColor
        {
            get { return mBoardColor; }

            set
            {
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].LineColor = value;
                    GS[i].LineColor = value;
                }
                mBoardColor = value;
                this.Invalidate();
                //TabBox.Invalidate();
            }
        }


        /// <summary>
        /// 数据开始位置
        /// 左侧最小，右侧最大
        /// </summary>
        public int StartIndex
        {
            get { return GS[0].StartIndex; }
            set
            {
                //设置LeftBar位置时 设置所有公式对应的LeftBar并重绘图形
                for (int i = 0; i < 10; i++)
                    GS[i].StartIndex = value;
                this.Invalidate();
            }
        }

        public int EndIndex
        {
            get { return GS[0].EndIndex; }
            //set
            //{
            //    //设置LeftBar位置时 设置所有公式对应的LeftBar并重绘图形
            //    for (int i = 0; i < 10; i++)
            //        GS[i].EndIndex = value;
            //    this.Invalidate();
            //}
        }

        bool _startFix = false;
        /// <summary>
        /// K线区域Start固定
        /// </summary>
        private bool StartFix
        {
            get { return _startFix; }

            set { _startFix = value; }
        }

        int _extendedRightSpace = 100;
        private int ExtendedRightSpace
        {
            get { return _extendedRightSpace; }
            set { _extendedRightSpace = value; }
        }

        //bool _keySelect = false;
        //internal bool KeySelect
        //{
        //    get { return _keySelect; }
        //    set { _keySelect = value; }
        //}
    }
}
