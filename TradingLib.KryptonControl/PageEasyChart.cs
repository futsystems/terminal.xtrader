using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataProvider;
using Common.Logging;

using Easychart.Finance;
using Easychart.Finance.DataProvider;
using TradingLib.KryptonControl;

namespace TradingLib.KryptonControl
{
    public partial class PageEasyChart : UserControl,IPage
    {

        string _pageName = "KCHART";
        public string PageName { get { return _pageName; } }

        public event Action<Symbol> OpenQuoteEvent;

        ILog logger = LogManager.GetLogger("ctlEasyChart");
        Symbol _symbol = null;
        BarFrequency _freq = null;



        public PageEasyChart()
        {
            
            InitializeComponent();

            //初始化Chart
            InitChartControl();

            SetChartControl();
            this.Load += new EventHandler(ctlEasyChart_Load);
            
        }

        void ctlEasyChart_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 合约
        /// </summary>
        public Symbol Symbol
        {
            get { return _symbol; }
            set 
            {
                _symbol = value;
            }
        }

        /// <summary>
        /// 频率
        /// </summary>
        public BarFrequency BarFrequency
        {
            get { return _freq; }
            set
            {
                _freq = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string ChartName
        {
            get
            {
                if (_symbol == null || _freq == null)
                {
                    throw new Exception("symbol or freq not valid");
                }
                return "{0}({1})-{2}".Put(_symbol.Symbol, _symbol.SecurityFamily.Exchange.Title, _freq.ToString());
            }
        }


        
        /// <summary>
        /// 设置ChartControl
        /// </summary>
        void InitChartControl()
        {
            PluginManager.Load(Environment.CurrentDirectory + "\\Plugins\\");
            PluginManager.OnPluginChanged += new System.IO.FileSystemEventHandler(PluginManager_OnPluginChanged);

            //WinChartControl.ContextMenu = null;
            WinChartControl.DefaultFormulas = "MAIN;VOLMA";//默认公式区域

            WinChartControl.StockBars = 150;
            WinChartControl.AfterBars = 1;

            //样式应用
            //WinChartControl.IntradayInfo = new ExchangeIntraday();
            //WinChartControl.MaxPrice = 0.0;
            //WinChartControl.NativeContextMenu = false;
            //WinChartControl.ShowStatistic = false;
            //WinChartControl.Margin = new System.Windows.Forms.Padding(0);
            WinChartControl.ShowHorizontalGrid = Easychart.Finance.Win.ShowLineMode.HideAll;
            WinChartControl.ShowVerticalGrid = Easychart.Finance.Win.ShowLineMode.HideAll;

            //靠右缩放
            WinChartControl.ZoomPosition = Easychart.Finance.Win.ZoomCenterPosition.Right;
            //WinChartControl.ViewChanged += new ViewChangedHandler(WinChartControl_ViewChanged);
            //WinChartControl.NativePaint += new NativePaintHandler(WinChartControl_NativePaint);
            //WinChartControl.DragEnter += new DragEventHandler(WinChartControl_DragEnter);
            //WinChartControl.DragDrop += new DragEventHandler(WinChartControl_DragDrop);
            //WinChartControl.AfterApplySkin += new EventHandler(WinChartControl_AfterApplySkin);
            //WinChartControl.MouseUp += new MouseEventHandler(WinChartControl_MouseUp);
            WinChartControl.KeyDown += new KeyEventHandler(WinChartControl_KeyDown);
            WinChartControl.KeyPress += new KeyPressEventHandler(WinChartControl_KeyPress);
            WinChartControl.KeyUp += new KeyEventHandler(WinChartControl_KeyUp);
            //WinChartControl.Skin = "GreenRed";
            
            WinChartControl.BeforeApplySkin += new Easychart.Finance.Win.ApplySkinHandler(WinChartControl_BeforeApplySkin);
            //WinChartControl.Chart.ViewChanged += new ViewChangedHandler(Chart_ViewChanged);
            
        }

        void WinChartControl_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Info("KeyUp:{0}".Put(e.KeyCode));
        }

        void WinChartControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            logger.Info("KeyPress:{0}".Put(e.KeyChar));
        }

        void WinChartControl_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Info("KeyDown:{0}".Put(e.KeyCode));

            if (e.KeyCode == Keys.Return)
            {
                if (OpenQuoteEvent != null)
                {
                    
                    logger.Info("OpenQuote Page view symbol:{0}".Put(_symbol.Symbol));
                    OpenQuoteEvent(_symbol);
                }
            }
        }

        void Chart_ViewChanged(object sender, ViewChangedArgs e)
        {
            logger.Info("View Changed");
        }

        FormulaSkin DefaultSkion
        {
            get
            {
                ChartSetting setting = new ChartSetting();

                return new FormulaSkin("RedWhite")
                {
                    Back =
                    {
                        BackGround = new BrushMapper(Color.Black),
                        
                        FrameWidth = 2,
                        FrameColor = Color.Red,
                    },
                    Colors = new Color[]
					{
						Color.Black,
						Color.Blue,
						Color.Red,
						Color.Fuchsia,
						Color.DarkGray,
						Color.Maroon,
						Color.DarkGreen,
						Color.Plum,
						Color.Olive
					},
                    BarPens = new PenMapper[]
					{
						new PenMapper(Color.Black),
						new PenMapper(Color.Black),
						new PenMapper(Color.Red)
					},
                    BarBrushes = new BrushMapper[]
					{
						BrushMapper.Empty,
						BrushMapper.Empty,
						new BrushMapper(Color.Maroon)
					},
                    NameBrush = new BrushMapper(Color.Black),
                    AxisX =
                    {
                        Back =
                        {
                            BackGround = new BrushMapper(Color.Azure),
                            LeftPen =
                            {
                                Width = 2
                            }
                        }
                    },
                    AllXFormats = XFormatCollection.Default,
                    AxisY =
                    {
                        Back =
                        {
                            BackGround = new BrushMapper(Color.AliceBlue),
                            RightPen =
                            {
                                Width = 2
                            },
                            TopPen =
                            {
                                Width = 2
                            }
                        },
                        MultiplyBack =
                        {
                            BackGround = new BrushMapper(Color.BlanchedAlmond)
                        },
                    },
                    CursorPen = new PenMapper(Color.Black)
                };
            }
        }

        void PluginManager_OnPluginChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            WinChartControl.NeedRefresh();
        }

        //在样式应用前根据我们的设定进行调整样式
        void WinChartControl_BeforeApplySkin(object sender, FormulaSkin skin)
        {
            ChartSetting setting = new ChartSetting();
            //公式绘制线颜色
            skin.Colors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Black, Color.Orange, Color.DarkGray, Color.DarkTurquoise };

            ////公式文字标注颜色
            //skin.NameBrush.Color = setting.ChartTextColor;

            //绘图区域 背景颜色
            skin.Back.BackGround.Color = setting.ChartBackgroundColor;
            //绘图区域 边框颜色
            //skin.Back.FrameColor = setting.ChartBorderColor;
            //skin.Back.TopPen = new PenMapper(Color.Green);
            //skin.Back.LeftPen = new PenMapper(Color.Green);//有效
            //skin.Back.BottomPen = new PenMapper(Color.Red);//无效 可能是被X轴覆盖
            //skin.Back.RightPen = new PenMapper(Color.Green);

            //绘制Bar颜色
            skin.BarPens = new PenMapper[] { new PenMapper(setting.CandleUpBorder), new PenMapper(setting.CandleDownBorder), new PenMapper(setting.CandleDownBorder) };
            skin.BarBrushes = new BrushMapper[] {BrushMapper.Empty,BrushMapper.Empty,new BrushMapper(setting.CandleDownColor)};// new BrushMapper(setting.CandleUpColor), new BrushMapper(setting.CandleUpColor), new BrushMapper(setting.CandleDownColor) };


            //十字光标颜色
            skin.CursorPen.Color = setting.CrossHairColor;

            //坐标 标尺线刻度颜色
            //skin.AxisX.MajorTick.TickPen.Color = setting.TickLineColor;
            //skin.AxisX.MinorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisX.MajorTick.ShowTick = false;
            skin.AxisX.MajorTick.ShowLine = false;
            skin.AxisX.MajorTick.ShowText = true;
            skin.AxisX.MinorTick.ShowTick = false;
            skin.AxisX.MinorTick.ShowLine = false;
            skin.AxisX.MinorTick.ShowText = false;

            skin.ShowXAxisInLastArea = true;//只在最后一个Area显示X轴

            skin.AxisY.MajorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisY.MinorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisY.MajorTick.FullTick = false;
            skin.AxisY.MajorTick.ShowTick = true;
            skin.AxisY.MajorTick.ShowLine = false;
            skin.AxisY.MajorTick.ShowText = true;
            skin.AxisY.AutoMultiply = false;
            skin.AxisY.MultiplyFactor = 10;
            skin.AxisY.RefValue = 10;
            

            skin.AxisY.MinorTick.ShowTick = false;
            skin.AxisY.MinorTick.ShowLine = false;
            skin.AxisY.MinorTick.ShowText = false;
            skin.AxisY.MajorTick.MinimumPixel = 100;

            //坐标 面板颜色
            skin.AxisX.Back.BackGround.Color = setting.SolidFrameColor;
            skin.AxisY.Back.BackGround.Color = setting.SolidFrameColor;

            //坐标 面板边框 可分为上 下 左 右
            //skin.AxisX.Back.TopPen = new PenMapper(Color.Red);
            //skin.AxisX.Back.BottomPen = new PenMapper(Color.Red);
            //skin.AxisX.Back.FrameColor = setting.ChartBorderColor;//x轴

            //skin.AxisY.Back.FrameColor = setting.ChartBorderColor;
            //skin.AxisY.Back.TopPen = new PenMapper(Color.Red);
            skin.AxisY.Back.LeftPen = new PenMapper(Color.Red);
            //skin.AxisY.Back.RightPen = new PenMapper(Color.Red);
            //skin.AxisY.Back.FrameWidth = 1;

            //skin.AxisY.AxisPos = AxisPos.Left;//Y轴 左侧 右侧显示
            //skin.AxisY.ShowAsPercent = true;
            
            //坐标轴小数点输出
            skin.AxisY.Format = "F1";

            skin.AxisY.MultiplyFactor = 10;
            skin.AxisY.MultiplyBack.BackGround.Color = setting.MultiplierBackgroundColor;
            skin.AxisY.MultiplyBack.FrameColor = setting.MultiplierBackgroundColor;

            //坐标 标签颜色
            skin.AxisX.LabelBrush.Color = setting.LabelColor;
            skin.AxisY.LabelBrush.Color = setting.LabelColor;



            //FormulaArea fa = new FormulaArea(WinChartControl.Chart);
            //this.WinChartControl.Chart.MainArea.AxisX.Visible = false;
            
            ////背景格子线颜色
            skin.AxisX.MajorTick.LinePen.Color = setting.GridLineColor;
            //skin.AxisX.MinorTick.LinePen.Color = setting.GridLineColor;
            skin.AxisY.MajorTick.LinePen.Color = setting.GridLineColor;
            //skin.AxisY.MinorTick.LinePen.Color = setting.GridLineColor;






            switch (setting.ChartType)
            {
                case ChartType.CandleStick:
                    WinChartControl.StockRenderType = StockRenderType.Candle;
                    break;
                case ChartType.UpDownStick:
                    WinChartControl.StockRenderType = StockRenderType.OHLCBars;
                    break;
                case ChartType.Line:
                    WinChartControl.StockRenderType = StockRenderType.Line;
                    break;
            }

            //skin.ShowXAxisInLastArea = true;
        }

        /// <summary>
        /// 初始化chartcontrol配置
        /// </summary>
        void SetChartControl()
        {
            //WinChartControl.MouseZoomBackColor = Color.Red;
            //WinChartControl.ShowCrossCursor = true;
            //WinChartControl.ShowCursorLabel = true;
            WinChartControl.StockBars = 150;//chart显示的Bar数
            //WinChartControl.ShowVerticalGrid = Easychart.Finance.Win.ShowLineMode.Default;//水平
            //WinChartControl.ShowHorizontalGrid = Easychart.Finance.Win.ShowLineMode.HideAll;//垂直

            //WinChartControl.ShowTopLine = true;
            WinChartControl.StickRenderType = StickRenderType.Column;
        }

        //IDataClient _client = null;
        ///// <summary>
        ///// 初始化datamanager
        ///// </summary>
        //public void InitDataManager(IDataClient client)
        //{
        //    _client = client;
        //    this.datamanager = new TLMemoryDataManager(_client, true);
        //    this.datamanager.DataProcessedEvent += new Action(OnBarUpdatedEvent);
        //    this.datamanager.BarUpdatedEvent += new Action(datamanager_BarUpdatedEvent);

        //    WinChartControl.DataManager = this.datamanager;
        //    WinChartControl.EndTime = DateTime.MinValue;
        //    WinChartControl.Symbol ="rb1610";
        //    WinChartControl.CurrentDataCycle = Easychart.Finance.DataCycle.Minute;
        //}

        TLMemoryDataManager _mdmIntraday = null;
        TLMemoryDataManager _mdmHist = null;

        public void BindDataManager(TLMemoryDataManager mdmIntraday)
        {

            this._mdmIntraday = mdmIntraday;
            this._mdmIntraday.DataProcessedEvent += new Action(OnBarUpdatedEvent);
            this._mdmIntraday.BarUpdatedEvent += new Action(OnDataProcessedEvent);

            WinChartControl.DataManager = _mdmIntraday;
            WinChartControl.EndTime = DateTime.MinValue;
            WinChartControl.Symbol = "rb1610";
            WinChartControl.CurrentDataCycle = Easychart.Finance.DataCycle.Minute;
        }

        void OnBarUpdatedEvent()
        {
            WinChartControl.EndTime = DateTime.MaxValue;
            WinChartControl.NeedRefresh();
        }

        void OnDataProcessedEvent()
        {
            WinChartControl.NeedRefresh();
        }


        public void ShowChart(Symbol symbol)
        {
            this.ShowChart(symbol, new BarFrequency(BarInterval.CustomTime, 60));
            

        }

        public void ShowChart(Symbol symbol, BarFrequency freq)
        {
            _symbol = symbol;
            _freq = freq;
            WinChartControl.Symbol = _symbol.Symbol;
            SetChartControl();
            WinChartControl.NeedRefresh();
        }

        ///// <summary>
        ///// 显示某个合约
        ///// </summary>
        ///// <param name="symbol"></param>
        //public void Display(IDataClient client)
        //{
        //    _symbol = null ;

        //    //设置控件
        //    SetChartControl();

        //    //初始化datamanager
        //    //InitDataManager(client);
        //}
    }

}
