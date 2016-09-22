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

        bool timeGo = true;
        System.Timers.Timer timer;

        /// <summary>
        /// 初始化定时任务
        /// </summary>
        void InitTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!timeGo) return;

            //KChart视图
            if (ctrlKChart.Visible)
            {
                if (ctrlKChart.IsIntraView)
                {
                    MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0);
                }

                //处于K线图模式 实时更新最新的Bar数据
                if (ctrlKChart.IsBarView)
                {
                    //K线数据已经加载了Bar数据 获得最近一个Bar的日期与时间
                    if (ctrlKChart.LastDate > 0 && ctrlKChart.LastTime > 0)
                    {
                        DateTime lastTime = Utils.ToDateTime(ctrlKChart.LastDate, ctrlKChart.LastTime * 100);
                        int reqCount = Utils.RequestCount(lastTime, CurrentKChartFreq);
                        logger.Info(string.Format("last date:{0} time:{1} now:{2} reqCount:{3}", ctrlKChart.LastDate, ctrlKChart.LastTime, DateTime.Now.ToShortTimeString(), reqCount));

                        int reqid =MDService.DataAPI.QrySeurityBars(CurrentKChartSymbol.Exchange,CurrentKChartSymbol.Symbol,CurrentKChartFreq, 0, reqCount);
                        kChartRealTimeBarRequest.TryAdd(reqid, this);
                    }
                }

                //盘口明细显示 根据Tab页实时请求更新
                if (ctrlKChart.ShowDetailPanel)
                {
                    if (ctrlKChart.TabValue == 0)
                    {
                        int reqId = MDService.DataAPI.QryTradeSplitData(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0, ctrlKChart.TabHigh);
                        kChartUpdateRequest.TryAdd(reqId, this);
                    }
                    if (ctrlKChart.TabValue == 1)
                    {
                        MDService.DataAPI.QryPriceVol(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol);
                    }
                }
            }


            //分笔视图
            if (ctrlTickList.Visible)
            {
                int reqId = MDService.DataAPI.QryTradeSplitData(ctrlTickList.Symbol.Exchange, ctrlTickList.Symbol.Symbol, 0, ctrlTickList.RowCount);//*ctrlTickList.ColumnCount);
                tickListUpdateRequest.TryAdd(reqId, this);
            }

            //分价视图
            if (ctrlPriceVolList.Visible)
            {
                int reqId = MDService.DataAPI.QryPriceVol(ctrlPriceVolList.Symbol.Exchange, ctrlPriceVolList.Symbol.Symbol);
                priceVolListRequest.TryAdd(reqId, this);
            }


            IEnumerable<MDSymbol> symlist = new List<MDSymbol>();
            //底部高亮合约
            symlist = symlist.Union(ctrlSymbolHighLight.Symbols);

            //当前K线图合约
            symlist = symlist.Union(new MDSymbol[] { ctrlKChart.Symbol });

            //如果合约报价列表可见 合并对应可见合约
            if(ctrlQuoteList.Visible)
            {
                symlist = symlist.Union(ctrlQuoteList.SymbolVisible);
            }
            if (symlist.Count() > 0)
            {
                MDService.DataAPI.QryTickSnapshot(symlist.ToArray());
            }


            UpdateTime();
        }


       
    }
}
