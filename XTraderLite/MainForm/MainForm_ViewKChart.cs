using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;
using TradingLib.XTrader.Control;

namespace XTraderLite
{
    public partial class MainForm
    {

        List<MDMinuteData> minuteData = new List<MDMinuteData>();

        

        void InitKChart()
        {

            ctrlKChart.KChartModeChange += new Action<object, CStock.KChartModeChangeEventArgs>(kChartView_KChartModeChange);

            //盘口面板Tab页切换
            ctrlKChart.TabSwitch += new Action<object, CStock.TabSwitchEventArgs>(ctrlKChart_TabSwitch);
            //盘口慢板Tab页面双击
            ctrlKChart.TabDoubleClick += new Action<object, CStock.TabDoubleClickEventArgs>(ctrlKChart_TabDoubleClick);

            //加载更多K线
            ctrlKChart.KViewLoadMoreData += new Action<object, CStock.KViewLoadMoreDataEventArgs>(ctrlKChart_KViewLoadMoreData);

            //查看分时天数发生变化 需要加载更多分时数据
            ctrlKChart.TimeViewDaysChanged += new Action<object, int>(ctrlKChart_TimeViewDaysChanged);

            //菜单点击频率切换
            ctrlKChart.KFrequencyMenuClick += new Action<object, CStock.KFrequencyMenuClickEventAargs>(ctrlKChart_KFrequencyMenuClick);
        }


        string GetFreq(CStock.KFrequencyType type)
        {
            switch (type)
            {
                case CStock.KFrequencyType.F_1Min: return ConstFreq.Freq_M1;
                case CStock.KFrequencyType.F_5Min: return ConstFreq.Freq_M5;
                case CStock.KFrequencyType.F_15Min: return ConstFreq.Freq_M15;
                case CStock.KFrequencyType.F_30Min: return ConstFreq.Freq_M30;
                case CStock.KFrequencyType.F_60Min: return ConstFreq.Freq_M60;
                case CStock.KFrequencyType.F_Day: return ConstFreq.Freq_Day;
                case CStock.KFrequencyType.F_Week: return ConstFreq.Freq_Week;
                case CStock.KFrequencyType.F_Month: return ConstFreq.Freq_Month;
                case CStock.KFrequencyType.F_Quarter: return ConstFreq.Freq_Quarter;
                case CStock.KFrequencyType.F_Year: return ConstFreq.Freq_Year;
                default:
                    return ConstFreq.Freq_Day;
            }
            
        }

        void ctrlKChart_KFrequencyMenuClick(object arg1, CStock.KFrequencyMenuClickEventAargs arg2)
        {
            //设定当前频率
            _currentFreq = GetFreq(arg2.KFrequencyType);
            ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            //设定当前显示视图
            ViewKChart();
        }

        void ctrlKChart_TimeViewDaysChanged(object arg1, int arg2)
        {
            if(CurrentKChartSymbol == null) return;
            ctrlKChart.ClearIntraViewData();//分时显示日期数量变化时 立即执行分时数据清理并重绘 如果在等待历史数据到达时候再清理数据会造成老数据任然在屏幕且有明显的切换感

            if (ctrlKChart.DaysForIntradayView>1)
            {
                //DayTicks.Clear();
                //for (int i = 0; i < 10; i++)
                //    dateList[i] = -1;
                //多日分时 由于不知道交易日信息 因此先查询日线 获得有效日期，然后再按此日期进行历史分时查询
                int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, ConstFreq.Freq_Day, 1, 10);//获得最近10日K线 当天日新不请求 该日分时通过日内分时查询
                kChartIntraViewDayBarRequest.TryAdd(reqid, this);

            }
            else//查询今日分时
            {

                MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol,0);
            }
        }

        void ctrlKChart_KViewLoadMoreData(object arg1, CStock.KViewLoadMoreDataEventArgs arg2)
        {
            string key = string.Format("{0}-{1}-{2}-{3}", CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, CurrentKChartFreq, arg2.Count);
            if (!kChartLoadMoreDataRequest.Values.Select(o=>o as string).Contains(key))
            {
                logger.Info(string.Format("load more data from server current data count:{0}", arg2.Count));
                //一致按住下箭头不放 K线控件会一直请求数据,造成同样的数据多次请求 多次返回 使得K线数据回补出现重复波段 在请求数据前 检查是否有相同的请求在队列 进行过滤
                int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, CurrentKChartFreq, arg2.Count, 800);
                kChartLoadMoreDataRequest.TryAdd(reqid, key);
            }
           
        }



        void ctrlKChart_TabDoubleClick(object arg1, CStock.TabDoubleClickEventArgs arg2)
        {
            switch (arg2.TabType)
            {
                case CStock.DetailBoardTabType.TradeDetails:
                    {
                        ViewTickList();
                    }
                    break;
                case CStock.DetailBoardTabType.PriceDistribution:
                    {
                        ViewPriceVolList();
                    }
                    break;
                default:
                    break;
            
            }

        }

        /// <summary>
        /// 盘口面板下方Tab切换
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void ctrlKChart_TabSwitch(object arg1, CStock.TabSwitchEventArgs arg2)
        {
            logger.Info(string.Format("TabSwitch Event,tab:{0}", arg2.TabType));
            switch (arg2.TabType)
            {
                case CStock.DetailBoardTabType.TradeDetails:
                    {

                        int reqId = MDService.DataAPI.QryTradeSplitData(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0, ctrlKChart.TabHigh);
                        kChartLoadTradeRequest.TryAdd(reqId, ctrlTickList);
                        return;
                    }
                case CStock.DetailBoardTabType.PriceDistribution:
                    {
                        MDService.DataAPI.QryPriceVol(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol);
                        return;
                    }
            }
        }

        /// <summary>
        /// ViewType发生变化 分时图与K线图切换
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void kChartView_KChartModeChange(object arg1, CStock.KChartModeChangeEventArgs arg2)
        {
            switch (arg2.ViewType)
            {
                case CStock.KChartViewType.TimeView:
                    {
                        SetCurrentViewType(EnumViewType.KChart);
                        //SetCurBoard(BoardStyle.Stock);
                        //GP.ShowFs ^= true;
                        //if (FCurStock == null)
                        //    FCurStock = (CStock.Stock)Stklist.Items[Stklist.SelectedIndex];
                        if (_currentSymbol != null)
                            SetKChartSymbol(_currentSymbol);
                        return;
                    }
                case CStock.KChartViewType.KView:
                    {
                        SetCurrentViewType(EnumViewType.KChart);
                        //GP.ShowFs ^= true;
                        //if (FCurStock == null)
                        //    FCurStock = (CStock.Stock)Stklist.Items[Stklist.SelectedIndex];
                        if (_currentSymbol != null)
                            SetKChartSymbol(_currentSymbol);
                        return;
                    }
                default:
                    return;
            }
        }



    }
}
