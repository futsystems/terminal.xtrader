﻿using System;
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

using Easychart.Finance;
using Easychart.Finance.DataProvider;

namespace TradingLib.Chart
{
    public partial class ctlEasyChart : UserControl
    {

        TLMemoryDataManager datamanager = null;
        public ctlEasyChart()
        {
            InitializeComponent();
            this.Load += new EventHandler(ctlEasyChart_Load);
            
        }

        void ctlEasyChart_Load(object sender, EventArgs e)
        {
            InitChartControl();
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

            //靠右缩放
            WinChartControl.ZoomPosition = Easychart.Finance.Win.ZoomCenterPosition.Right;
            //WinChartControl.ViewChanged += new ViewChangedHandler(WinChartControl_ViewChanged);
            //WinChartControl.NativePaint += new NativePaintHandler(WinChartControl_NativePaint);
            //WinChartControl.DragEnter += new DragEventHandler(WinChartControl_DragEnter);
            //WinChartControl.DragDrop += new DragEventHandler(WinChartControl_DragDrop);
            //WinChartControl.AfterApplySkin += new EventHandler(WinChartControl_AfterApplySkin);
            //WinChartControl.MouseUp += new MouseEventHandler(WinChartControl_MouseUp);

            WinChartControl.Skin = "RedWhite";
            WinChartControl.BeforeApplySkin += new Easychart.Finance.Win.ApplySkinHandler(WinChartControl_BeforeApplySkin);

            
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

            //绘图区域背景颜色
            skin.Back.BackGround.Color = setting.ChartBackgroundColor;
            //绘制Bar颜色
            skin.BarPens = new PenMapper[] { new PenMapper(setting.CandleUpBorder), new PenMapper(setting.CandleDownBorder), new PenMapper(setting.CandleDownBorder) };
            skin.BarBrushes = new BrushMapper[] {BrushMapper.Empty,BrushMapper.Empty,new BrushMapper(setting.CandleDownColor)};// new BrushMapper(setting.CandleUpColor), new BrushMapper(setting.CandleUpColor), new BrushMapper(setting.CandleDownColor) };


            ////十字光标颜色
            //skin.CursorPen.Color = setting.CrossHairColor;

            //坐标尺上的标尺线颜色
            skin.AxisX.MajorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisX.MinorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisY.MajorTick.TickPen.Color = setting.TickLineColor;
            skin.AxisY.MinorTick.TickPen.Color = setting.TickLineColor;

            ////背景格子线颜色
            //skin.AxisX.MajorTick.LinePen.Color = setting.GridLineColor;
            //skin.AxisX.MinorTick.LinePen.Color = setting.GridLineColor;
            //skin.AxisY.MajorTick.LinePen.Color = setting.GridLineColor;
            //skin.AxisY.MinorTick.LinePen.Color = setting.GridLineColor;

            //skin.AxisX.LabelBrush.Color = setting.LabelColor;
            //skin.AxisY.LabelBrush.Color = setting.LabelColor;
            ////skin.AxisY.AxisPos = AxisPos.Left;
            ////skin.AxisY.ShowAsPercent = true;
            ////坐标轴小数点输出
            ////skin.AxisY.Format = "N1";

            ////坐标面板颜色
            //skin.AxisX.Back.BackGround.Color = setting.SolidFrameColor;
            //skin.AxisY.Back.BackGround.Color = setting.SolidFrameColor;

            

            ////绘图pane的边框颜色
            ////skin.Back.FrameColor = setting.ChartBorderColor;
            //skin.Back.TopPen = new PenMapper(Color.Red);
            ////skin.Back.LeftPen = new PenMapper(Color.Red);
            ////skin.Back.BottomPen =  new PenMapper(Color.Red);

            ////坐标面板边框 可分为上 下 左 右
            //skin.AxisX.Back.TopPen = new PenMapper(Color.Red);
            ////skin.AxisX.Back.FrameColor = setting.ChartBorderColor;
            ////skin.AxisY.Back.FrameColor = setting.ChartBorderColor;
            //skin.AxisY.Back.TopPen = new PenMapper(Color.Red);
            //skin.AxisY.Back.LeftPen = new PenMapper(Color.Red);
            ////skin.AxisY.Back.RightPen = new PenMapper(Color.Red);


            //skin.AxisY.MultiplyBack.BackGround.Color = setting.MultiplierBackgroundColor;
            //skin.AxisY.MultiplyBack.FrameColor = setting.MultiplierBackgroundColor;

            //switch (setting.ChartType)
            //{
            //    case ChartType.CandleStick:
            //        WinChartControl.StockRenderType = StockRenderType.Candle;
            //        break;
            //    case ChartType.UpDownStick:
            //        WinChartControl.StockRenderType = StockRenderType.OHLCBars;
            //        break;
            //    case ChartType.Line:
            //        WinChartControl.StockRenderType = StockRenderType.Line;
            //        break;
            //}

            //skin.ShowXAxisInLastArea = true;
        }

        /// <summary>
        /// 初始化chartcontrol配置
        /// </summary>
        void SetChartControl()
        {
            //WinChartControl.MouseZoomBackColor = Color.Red;

            WinChartControl.ShowCrossCursor = true;
            WinChartControl.ShowCursorLabel = true;
            WinChartControl.StockBars = 150;//chart显示的Bar数
            WinChartControl.ShowVerticalGrid = Easychart.Finance.Win.ShowLineMode.HideAll;//水平
            WinChartControl.ShowHorizontalGrid = Easychart.Finance.Win.ShowLineMode.Show;//垂直

            WinChartControl.ShowTopLine = true;
            WinChartControl.StickRenderType = StickRenderType.Column;
        }

        IDataClient _client = null;
        /// <summary>
        /// 初始化datamanager
        /// </summary>
        public void InitDataManager(IDataClient client)
        {
            _client = client;
            this.datamanager = new TLMemoryDataManager(_client, true);
            this.datamanager.DataProcessedEvent += new Action(datamanager_DataProcessedEvent);
            this.datamanager.BarUpdatedEvent += new Action(datamanager_BarUpdatedEvent);

            WinChartControl.DataManager = this.datamanager;
            WinChartControl.EndTime = DateTime.MinValue;
            WinChartControl.Symbol ="rb1610";
            WinChartControl.CurrentDataCycle = Easychart.Finance.DataCycle.Minute;
        }

        void datamanager_BarUpdatedEvent()
        {
            WinChartControl.EndTime = DateTime.MaxValue;
            WinChartControl.NeedRefresh();
        }

        void datamanager_DataProcessedEvent()
        {
            WinChartControl.NeedRefresh();
        }


        Symbol _symbol = null;
        /// <summary>
        /// 显示某个合约
        /// </summary>
        /// <param name="symbol"></param>
        public void Display(IDataClient client)
        {
            _symbol = null ;

            //设置控件
            SetChartControl();


            //初始化datamanager
            InitDataManager(client);
        }
    }

}
