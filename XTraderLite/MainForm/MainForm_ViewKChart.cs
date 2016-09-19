﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class MainForm
    {
        List<MinuteData> minuteData = new List<MinuteData>();
        void InitKChart()
        {

            ctrlKChart.KChartModeChange += new Action<object, CStock.KChartModeChangeEventArgs>(kChartView_KChartModeChange);

            ctrlKChart.TabSwitch += new Action<object, CStock.TabSwitchEventArgs>(ctrlKChart_TabSwitch);
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
                        MDService.DataAPI.QryTradeSplitData(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0, ctrlKChart.TabHigh);
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
                        SetViewType(EnumTraderViewType.KChart);
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
                        SetViewType(EnumTraderViewType.KChart);
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
