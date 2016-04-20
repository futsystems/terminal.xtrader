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
    struct BarData
		{
			public double jDate; // Julian date
			public double OpenPrice;
			public double HighPrice;
			public double LowPrice;
			public double ClosePrice;
			public double Volume;
		}
	
		

    public partial class PageStockChartX : UserControl,IPage
    {
        string _pageName = "KCHART";
        public string PageName { get { return _pageName; } }


        ILog logger = LogManager.GetLogger("PageStockChartX");

        public PageStockChartX()
        {
            InitializeComponent();

            InitStockChartX();

            InitMenu();

            WireEvent();
            //获取StockChartX1内置指标
            StockChartX1.EnumIndicators();

            this.Load += new EventHandler(ctlChart_Load);
        }

        void ctlChart_Load(object sender, EventArgs e)
        {
            //HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
        }


        bool _showMenu = false;
        private ArrayList Data = new ArrayList();

        Dictionary<int, string> _indicatorMap = new Dictionary<int, string>();


        void InitStockChartX()
        {
            StockChartX1.MouseMoveEvent += new AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEventHandler(StockChartX1_MouseMoveEvent);
            StockChartX1.ItemDoubleClick += new AxSTOCKCHARTXLib._DStockChartXEvents_ItemDoubleClickEventHandler(StockChartX1_ItemDoubleClick);
            StockChartX1.ItemLeftClick += new AxSTOCKCHARTXLib._DStockChartXEvents_ItemLeftClickEventHandler(StockChartX1_ItemLeftClick);
            StockChartX1.ItemMouseMove += new AxSTOCKCHARTXLib._DStockChartXEvents_ItemMouseMoveEventHandler(StockChartX1_ItemMouseMove);
            StockChartX1.ItemRightClick += new AxSTOCKCHARTXLib._DStockChartXEvents_ItemRightClickEventHandler(StockChartX1_ItemRightClick);
            StockChartX1.Zoom += new EventHandler(StockChartX1_Zoom);
            StockChartX1.OnKeyDown += new AxSTOCKCHARTXLib._DStockChartXEvents_OnKeyDownEventHandler(StockChartX1_OnKeyDown);

            StockChartX1.SelectSeries += new AxSTOCKCHARTXLib._DStockChartXEvents_SelectSeriesEventHandler(StockChartX1_SelectSeries);

            StockChartX1.OnRButtonDown += new EventHandler(StockChartX1_OnRButtonDown);
            StockChartX1.OnRButtonUp += new EventHandler(StockChartX1_OnRButtonUp);
            StockChartX1.OnLButtonDown += new EventHandler(StockChartX1_OnLButtonDown);
            StockChartX1.OnLButtonUp += new EventHandler(StockChartX1_OnLButtonUp);
            StockChartX1.DoubleClickEvent += new EventHandler(StockChartX1_DoubleClickEvent);
            StockChartX1.EnumIndicator += new AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEventHandler(StockChartX1_EnumIndicator);
            StockChartX1.OnKeyUp += new AxSTOCKCHARTXLib._DStockChartXEvents_OnKeyUpEventHandler(StockChartX1_OnKeyUp);
            StockChartX1.OnChar += new AxSTOCKCHARTXLib._DStockChartXEvents_OnCharEventHandler(StockChartX1_OnChar);

            
            StockChartX1.DebugEvent += new AxSTOCKCHARTXLib._DStockChartXEvents_DebugEventEventHandler(StockChartX1_DebugEvent);
        }



        void StockChartX1_DebugEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_DebugEventEvent e)
        {
            logger.Info("StockChartX:" + e.msg);
        }

        void StockChartX1_SelectSeries(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_SelectSeriesEvent e)
        {
            logger.Info("seriees selected:"+e.name);
        }

        void StockChartX1_DoubleClickEvent(object sender, EventArgs e)
        {
            logger.Info("Double click");
            //StockChartX1.CrossHairs = true;
        }


        void StockChartX1_OnKeyDown(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_OnKeyDownEvent e)
        {
            
            Keys keycode = (Keys)e.nChar;
            logger.Info("KeyDown Code:{0}".Put(keycode));
            switch (keycode)
            {
                case Keys.Down:
                    {
                        this.ZoomOut(0.2);
                        
                        logger.Info("RecordCount:{0} FirstVisibleRecord:{1} LastVisbleRecord:{2}".Put(StockChartX1.RecordCount, StockChartX1.FirstVisibleRecord,StockChartX1.LastVisibleRecord));
                        break;
                    }
                case Keys.Up:
                    {
                        this.ZommIn(0.2);
                        break;
                    }
                case Keys.Left:
                    {
                        this.ScrollLeft(5);
                        break;
                    }
                case Keys.Right:
                    {
                        this.ScrollRight(5);
                        break;
                    }
                case Keys.U:
                    {
                        logger.Info("add visiable count");
                        StockChartX1.VisibleRecordCount += 1;
                        StockChartX1.Update();
                        break;
                    }
                case Keys.D:
                    {
                        logger.Info("add visiable count");
                        //StockChartX1.FirstVisibleRecord -= 1;
                        
                        break;
                    }
            }
        }

        void StockChartX1_OnChar(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_OnCharEvent e)
        {
            logger.Info("OnChar Event");
            
            
        }

        void WireEvent()
        {

        }



        /// <summary>
        /// 放大缩小事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StockChartX1_Zoom(object sender, EventArgs e)
        {
            logger.Info("Zoome Show Record Cnt:{0}".Put(StockChartX1.VisibleRecordCount));
            //记录当前图表显示Bar数量 频率切换或合约显示数量一致
            _currentVisibleRecordCount = StockChartX1.VisibleRecordCount;

        }

        void StockChartX1_ItemRightClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemRightClickEvent e)
        {
            logger.Info(string.Format("item right click name:{0} type:{1} ", e.name, e.objectType));
        }

        void StockChartX1_ItemMouseMove(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemMouseMoveEvent e)
        {
            //logger.Info("item mouse move");
        }

        void StockChartX1_ItemLeftClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemLeftClickEvent e)
        {
            logger.Info("current selected:" + e.name + " objtype:" + e.objectType);
            
        }

        void StockChartX1_ItemDoubleClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemDoubleClickEvent e)
        {
            logger.Info("item double click");
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StockChartX1_MouseMoveEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEvent e)
        {
            //logger.Info(string.Format("mouse move event record:{0} x:{1} y:{2} ",e.record,e.x,e.y));
        }

        void StockChartX1_EnumIndicator(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEvent e)
        {
            //MessageBox.Show(string.Format("{0}-{1}", e.indicatorID, e.indicatorName));
            _indicatorMap.Add(e.indicatorID, e.indicatorName);
        }

        void InitMenu()
        {
            
        }

       

        //void mnuBuy_Click(object sender, Nevron.UI.WinForm.Controls.CommandEventArgs e)
        //{
        //    if (StockChartX1.PriceStyle != PriceStyle.psStandard)
        //    {
        //        MessageBox.Show("Chart trading can be used only with standard HLC or candle charts!", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    MessageBox.Show("it is ok current panel:" + StockChartX1.CurrentPanel);
        //    //if (txtQuantity.Text == "") txtQuantity.Text = "1";

        //    if (StockChartX1.CurrentPanel != 0) return;
        //    //LoadPortfolios();
        //    //grpOrder.Left = StockChartX1.GetXPixel(m_Record - StockChartX1.FirstVisibleRecord) - 50;
        //    //grpOrder.Top = StockChartX1.GetYPixel(0, m_Value) - 20;

        //    // Don't go offscreen
        //    //if (grpOrder.Left + grpOrder.Width > StockChartX1.Left + StockChartX1.Width)
        //    //    grpOrder.Left = StockChartX1.Left + StockChartX1.Width - grpOrder.Width;
        //    //if (grpOrder.Top + grpOrder.Height > StockChartX1.Top + StockChartX1.Height)
        //    //    grpOrder.Top = StockChartX1.Top + StockChartX1.Height - grpOrder.Height;

        //    //if (cmbPortfolio.Items.Count > -1) cmbPortfolio.SelectedIndex = 0;
        //    //if (grpOrder.Text == "") grpOrder.Text = "1";
        //    //m_Side = ctlPortfolio.Orders.Side.LongSide;
        //    //grpOrder.Visible = true;
        //    //txtQuantity.SelectAll();
        //    //txtQuantity.Focus();
        //}

        /// <summary>
        /// 按键Up事件nChar标示按键码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StockChartX1_OnKeyUp(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_OnKeyUpEvent e)
        {
            //throw new NotImplementedException();
            logger.Info(string.Format("key up nChar:{0} nFlags:{1} nRepCnt:{2}",e.nChar,e.nFlags,e.nRepCnt));
            
        }

        void StockChartX1_OnLButtonUp(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void StockChartX1_OnLButtonDown(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void StockChartX1_OnRButtonUp(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            _showMenu = false;
        }

        void StockChartX1_OnRButtonDown(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (_showMenu) return;
            //if (StockChartX1.SelectedKey != "") return;
            Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };
            //ctmRight.Show(this, p);
        }
        
        /* A word about Julian dates:
		Julian dates are used in many financial applications.
		The Julian date represents the Gregorian equivalent (like 7/04/2004
		12:10:05 AM) as a double value (like 2342123.92) The value also stores
		the time so it is useful for real time data as well as EOD data.
		StockChartX makes it easy to use these dates by providing you with a
		ToJulianDate and a FromJulianDate function.

		This function converts a string into a Julian date format so the
		StockChartX control can interpret the date properly:
		*/
        public double GetJDate(string szDate)
        {

            int hr = 0, mn = 0, sc = 0;
            DateTime date;

            try
            {
                date = DateTime.Parse(szDate);
            }
            catch
            {
                return -1;
            }

            // Default to midnight if no time
            hr = date.Hour;
            mn = date.Minute;
            sc = date.Second;
            if (hr == 0) hr = 12;

            // Get a Julian date from the StockChartX control
            szDate = StockChartX1.ToJulianDate(date.Year, date.Month,
                date.Day, hr, mn, sc).ToString();
            
            return Double.Parse(szDate);

        }

        #region 图表事件操作

        /// <summary>
        /// 左移
        /// </summary>
        /// <param name="record"></param>
        public void ScrollLeft(int record=1)
        {
            StockChartX1.ScrollLeft(record);
        }
        /// <summary>
        /// 右移
        /// </summary>
        /// <param name="record"></param>
        public void ScrollRight(int record = 1)
        {
            StockChartX1.ScrollRight(record);
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="ratio"></param>
        public void ZommIn(double ratio)
        {
            if (ratio >= 1)
            {
                StockChartX1.ZoomIn((int)ratio);
            }
            else
            {
                StockChartX1.ZoomIn((int)(ratio * StockChartX1.VisibleRecordCount));
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="ratio"></param>
        public void ZoomOut(double ratio)
        {
            if (ratio >= 1)
            {
                StockChartX1.ZoomOut((int)ratio);
            }
            else
            {
                StockChartX1.ZoomOut((int)(ratio * StockChartX1.VisibleRecordCount));
            }
        }



        #endregion


        ChartStyle _style = new ChartStyle();



        Dictionary<int, ChartPanel> chartmap = new Dictionary<int, ChartPanel>();

        public void ResetStockChartX()
        {
            logger.Info("Reset StockChratX");

            int panel = 0;

            StockChartX1.RemoveAllSeries();
            StockChartX1.Symbol = this.Symbol.Symbol.Replace(".", "");
            StockChartX1.Visible = true;
            StockChartX1.PriceStyle = PriceStyle.psStandard;

            //First add a panel (chart area) for the OHLC data:
            panel = StockChartX1.AddChartPanel();

            //Now add the open, high, low and close series to that panel:
            StockChartX1.AddSeries(this.NAME_OPEN, SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(this.NAME_HIGH, SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(this.NAME_LOW, SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(this.NAME_CLOSE, SeriesType.stCandleChart, panel);

            // Change the color:
            //StockChartX1.set_SeriesColor(this.NAME_CLOSE, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray));


            panel = StockChartX1.AddChartPanel();
            StockChartX1.AddSeries(this.NAME_VOLUME, STOCKCHARTXLib.SeriesType.stVolumeChart, panel);

            // Change volume color and weight of the volume panel:
            StockChartX1.set_SeriesColor(this.NAME_VOLUME, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue));
            StockChartX1.set_SeriesWeight(this.NAME_VOLUME, 3);
            StockChartX1.SetSeriesUpDownColors(this.NAME_VOLUME, (uint)System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green), (uint)System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
            //StockChartX1.UseVolumeUpDownColors = true;

            //panel = StockChartX1.AddChartPanel();
            //StockChartX1.AddSeries(this.NAME_VOLUME, STOCKCHARTXLib.SeriesType.stLineChart, panel);

            //panel = StockChartX1.AddChartPanel();
            //StockChartX1.AddSeries(this.NAME_CLOSE, SeriesType.stLineChart, panel);

            // Resize the volume panel to make it smaller
            StockChartX1.set_PanelY1(1, (int)(StockChartX1.Height * 0.8));

            //StockChartX1.DisplayTitleBorder = true;

            this.ApplyChartStyle(new ChartStyle());

            //默认显示K线数量
            this.DefaultVisibleRecordCount = 50;
            

        }

       



        public void UpdateBarAsync(Bar bar,bool isNewBar = false)
        {
            Action a = () =>
            {
                this.UpdateBar(bar, isNewBar);
            };
            BeginInvoke(a);
        }

        private void UpdateYScale()
        {
            double max = StockChartX1.GetMaxValue(StockChartX1.Symbol + ".high");
            double min = StockChartX1.GetMinValue(StockChartX1.Symbol + ".low");
            StockChartX1.YScaleMinTick = (max - min) < 1.0 ? 0.05 : 0.25;
        }


        /// <summary>
        /// 加载Bar数据
        /// </summary>
        /// <param name="barlist"></param>
        public void LoadBars(IEnumerable<Bar> barlist)
        {
            double prevJDate = 0;
            foreach (var bar in barlist)
            {
                double jdate = StockChartX1.ToJulianDate(bar.StartTime.Year, bar.StartTime.Month,
                                                            bar.StartTime.Day, bar.StartTime.Hour,
                                                            bar.StartTime.Minute, bar.StartTime.Second);

                if (jdate != prevJDate)
                {
                    this.UpdateBar(bar, true,false);
                }
            
            }

            //UpdateYScale();

            if (StockChartX1.RecordCount > this.DefaultVisibleRecordCount)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - this.DefaultVisibleRecordCount;
            }

            StockChartX1.Update();


           
        }

        /// <summary>
        /// 更新一条Bar数据
        /// PartialBar更新则为Update
        /// </summary>
        /// <param name="bar"></param>
        public void UpdateBar(Bar bar,bool isNewBar=false,bool needUpdate=true)
        {
            if (isNewBar)//插入一个Bar数据
            {
                double jdate = StockChartX1.ToJulianDate(bar.StartTime.Year, bar.StartTime.Month, bar.StartTime.Day,
                        bar.StartTime.Hour, bar.StartTime.Minute, bar.StartTime.Second);
                StockChartX1.AppendValue(GetSerieseName(OPEN),jdate, bar.Open);
                StockChartX1.AppendValue(GetSerieseName(HIGH), jdate, bar.High);
                StockChartX1.AppendValue(GetSerieseName(LOW), jdate, bar.Low);
                StockChartX1.AppendValue(GetSerieseName(ClOSE), jdate, bar.Close);
                StockChartX1.AppendValue(GetSerieseName(VOLUME), jdate, bar.Volume);

            }
            else//更新一个Bar数据
            {
                double jdate = StockChartX1.GetJDate(GetSerieseName(ClOSE), StockChartX1.RecordCount);
                StockChartX1.EditValue(GetSerieseName(OPEN), jdate, bar.Open);
                StockChartX1.EditValue(GetSerieseName(HIGH), jdate, bar.High);
                StockChartX1.EditValue(GetSerieseName(LOW), jdate, bar.Low);
                StockChartX1.EditValue(GetSerieseName(ClOSE), jdate, bar.Close);
                StockChartX1.EditValue(GetSerieseName(VOLUME), jdate, bar.Volume);
                StockChartX1.EditJDate(StockChartX1.RecordCount, jdate);//更新时间
            }

            if (needUpdate)
            {
                if (StockChartX1.RecordCount > 100)
                {
                    StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - 100;
                }
                //实时触发更新
                StockChartX1.Update();
            }
        }

        public void UpdateView()
        {
            //int cnt = StockChartX1.Width / 20;
            if (StockChartX1.RecordCount > 100)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - 100;
            }
            //实时触发更新
            StockChartX1.Update();
        }

        public void Clear()
        {
            StockChartX1.RemoveAllSeries();
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        public void AddIndicator(EnumIndicator local_indicator)
        {
            if (StockChartX1.RecordCount < 3) return;
            Indicator indicator = (Indicator)local_indicator;
            string cnt = "";
            int n = StockChartX1.GetIndicatorCountByType(indicator);
            if (n > 0)
            {
                cnt = " " + (n + 1);
            }
            int panel = 0;// IsOverlay(_indicatorMap[(int)indicator]) ? 0 : StockChartX1.AddChartPanel();
            //int indicator = m_frmMain.cboIndicators.HostedControl.SelectedIndex;
            StockChartX1.AddIndicatorSeries((Indicator)indicator,_indicatorMap[(int)indicator]+cnt, panel, true);
            StockChartX1.Update();

            


        }

        /// <summary>
        /// 判断是否主图叠加
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsOverlay(string name)
        {
            string[] overlays = new[]
                             {
                               "PARABOLIC", "PSAR", "FORECAST", "INTERCEPT",
                               "WEIGHTED CLOSE", "TYPICAL PRICE", "WEIGHTED PRICE",
                               "MEDIAN PRICE", "SMOOTHING", "BOLLINGER",
                               "MOVING AVERAGE", "BANDS"
                             };
            return overlays.Any(overlay => name.IndexOf(overlay, StringComparison.CurrentCultureIgnoreCase) != -1);
            //return false;
        }


        







    }
}
