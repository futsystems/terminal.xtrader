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
        void SetKChartSymbol(MDSymbol symbol)
        {
            bool changeSymbol = false;
            if (_currentSymbol != symbol)
            {
                changeSymbol = true;
            }
            _currentSymbol = symbol;

            //设定当前视图类型
            SetViewType(EnumTraderViewType.KChart);


            ctrlKChart.Focus();
            ctrlKChart.ClearData();

            //GP.SetQuan(sk.qu);
            //GP.PreClose = sk.GP.YClose;
            //ctrlKChart.SetQuan(symbol.PowerData);//设定除权数据
            ctrlKChart.StkCode = symbol.Symbol;
            ctrlKChart.StkName = symbol.Name;
            ctrlKChart.SetStock(symbol);


            if (ctrlKChart.ShowDetailPanel)
            {
                if (ctrlKChart.TabValue == 0)
                {
                    MDService.DataAPI.QryTradeSplitData(symbol.Exchange, symbol.Symbol, 0, ctrlKChart.TabHigh);
                }
                if (ctrlKChart.TabValue == 1)
                {
                    MDService.DataAPI.QryPriceVol(symbol.Exchange, symbol.Symbol);
                }

            }


            //如果是分时模式 则请求分时数据
            if (ctrlKChart.IsIntraView)
            {
                //多日分时
                if ((ctrlKChart.DaysForIntradayView > 1) && changeSymbol)
                {
                    minuteData.Clear();
                    //for (int i = 0; i < 10; i++)
                    //    dateList[i] = -1;

                    //多日分时 由于不知道交易日信息 因此先查询日线 获得有效日期，然后再按此日期进行历史分时查询
                    //int reqid = dataApi.QrySeurityBars(FCurStock.mark, FCurStock.codes, 4, 0, 10);
                    //reqSender_TimeView.TryAdd(reqid, GP);
                }
                else //MDService.DataAPI
                {
                    MDService.DataAPI.QryMinuteDate(symbol.Exchange, symbol.Symbol, 0);
                }
            }

            //如果是K线模式则请求K线数据
            if (ctrlKChart.IsBarView)
            {

                //if (zq == 12)
                //{
                //    //Ticks.Clear();
                //    //GetFenBiLine(sk, 0, 2000);
                //}
                //else
                {
                    MDService.DataAPI.QrySeurityBars(symbol.Exchange, symbol.Symbol, _currentFreq, 0, 800);
                }
            }



        }


    }


}
