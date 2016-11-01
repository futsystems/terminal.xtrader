using System;
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
        /// <summary>
        /// 当前合约
        /// </summary>
        MDSymbol _currentSymbol = null;

        string _currentFreq = ConstFreq.Freq_Day;

        /// <summary>
        /// 当前kChart合约
        /// </summary>
        MDSymbol CurrentKChartSymbol { get { return _currentSymbol; } }

        /// <summary>
        /// 当前KChart周期
        /// </summary>
        string CurrentKChartFreq { get { return _currentFreq; } }


        /// <summary>
        /// 选中当前合约
        /// </summary>
        /// <param name="symbol"></param>
        void SetKChartSymbol(MDSymbol symbol,bool focus=true)
        {
            bool changeSymbol = false;
            if (_currentSymbol != symbol)
            {
                changeSymbol = true;
            }
            _currentSymbol = symbol;

            if (focus)
            {
                ctrlKChart.Focus();
            }
            bool symChange = (symbol != ctrlKChart.Symbol);
            bool cycleChange = _currentFreq != ctrlKChart.Cycle;
            

            if (symChange || cycleChange)
            {
                logger.Info("symbol change clear data");//需要重新获得数据
                ctrlKChart.ClearData();
                ctrlKChart.SetSymbol(symbol);
                ctrlKChart.SetCycle(_currentFreq);
            }

            //盘口面板信息与频率和合约无关 直接查询获得更新
            if (ctrlKChart.ShowDetailPanel)
            {
                if (ctrlKChart.TabValue == 0)
                {
                    int reqId = MDService.DataAPI.QryTradeSplitData(symbol.Exchange, symbol.Symbol, 0, ctrlKChart.TabHigh);
                    kChartLoadTradeRequest.TryAdd(reqId, this);
                }
                if (ctrlKChart.TabValue == 1)
                {
                    MDService.DataAPI.QryPriceVol(symbol.Exchange, symbol.Symbol);
                }
            }

            //合约或频率任何一个数据改变 需要重新查询所有数据
            if (symChange || cycleChange)
            {
                //分时数据查询
                if (ctrlKChart.DaysForIntradayView > 1)
                {
                    minuteData.Clear();
                    int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, ConstFreq.Freq_Day, 1, 10);//获得最近10日K线 当天日新不请求 该日分时通过日内分时查询
                    kChartIntraViewDayBarRequest.TryAdd(reqid, this);
                }
                else
                {
                    MDService.DataAPI.QryMinuteDate(symbol.Exchange, symbol.Symbol, 0);
                }

                //Bar数据查询
                MDService.DataAPI.QrySecurityBars(symbol.Exchange, symbol.Symbol, _currentFreq, 0, 800);
            }
        }


    }


}
