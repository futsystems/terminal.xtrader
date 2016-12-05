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

            //开盘前2分钟 重置分时数据
            if ((!_openReset) && CurrentKChartSymbol != null && CurrentKChartSymbol.OpenTime != null)
            {
                int now = Utils.ToTLTime();
                int diff = Utils.FTDIFF(now, (int)CurrentKChartSymbol.OpenTime);
                //开盘前5分钟 执行数据重置
                if (now <= CurrentKChartSymbol.OpenTime && diff < 2 * 60 && diff > 0)//开盘前2分钟执行数据重置 避免客户端时间与服务端时间有偏差导致重置后任然获得上个交易日的数据 从而数据无法重置
                {
                    _openReset = true;
                    //清空当前分时数据
                    ctrlKChart.ResetIntraView();
                    //ctrlKChart.ClearIntraViewData();
                    //RefreshKChart();
                }
            }
            

            //KChart视图
            if (ctrlKChart.Visible)
            {
                //分时
                if (ctrlKChart.IsIntraView)
                {
                    if (MDService.DataAPI.APISetting.QryMinuteDataTimeSupport)
                    {
                        if (ctrlKChart.LastMinuteDataDay > 0 && ctrlKChart.LastMinuteDataTime >= 0)
                        {
                            long start = Utils.ToTLDateTime(ctrlKChart.LastMinuteDataDay, ctrlKChart.LastMinuteDataTime);
                            int reqid = MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, start);
                            kChartMinuteDataUpdateRequest.TryAdd(reqid, this);
                        }
                        else
                        {
                            //查询当天所有分时 进行数据更新
                            MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0);
                        }
                    }
                    else
                    {
                        //查询当天所有分时 进行数据更新
                        MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0);
                    }
                }

                //处于K线图模式 实时更新最新的Bar数据
                if (ctrlKChart.IsBarView)
                {
                    if (MDService.DataAPI.APISetting.QryBarTimeSupport)//通过最近的Bar时间来恢复该事件以来的所有Bar数据
                    {
                        if (ctrlKChart.LastDate > 0 && ctrlKChart.LastTime >= 0)//EOD Bar数据时间为0点 因此这里时间判断加上 等于0
                        {
                            long start = Utils.ToTLDateTime(ctrlKChart.LastDate, ctrlKChart.LastTime);
                            int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, CurrentKChartFreq,start,Utils.ToTLDateTime(DateTime.MaxValue));
                            kChartRealTimeBarRequest.TryAdd(reqid, this);
                        }
                    }
                    else //不支持按时间查询
                    {
                        //K线数据已经加载了Bar数据 获得最近一个Bar的日期与时间
                        if (ctrlKChart.LastDate > 0 && ctrlKChart.LastTime > 0)
                        {
                            DateTime lastTime = Utils.ToDateTime(ctrlKChart.LastDate, ctrlKChart.LastTime);
                            int reqCount = Utils.RequestCount(lastTime, CurrentKChartFreq);
                            logger.Info(string.Format("last date:{0} time:{1} now:{2} reqCount:{3}", ctrlKChart.LastDate, ctrlKChart.LastTime, DateTime.Now.ToShortTimeString(), reqCount));

                            int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, CurrentKChartFreq, 0, reqCount);
                            kChartRealTimeBarRequest.TryAdd(reqid, this);
                        }
                        //获得当前Bar时间 然后通过时间进行查询
                    }
                }

                //盘口明细显示 根据Tab页实时请求更新
                if (ctrlKChart.ShowDetailPanel)
                {
                    if (ctrlKChart.TabValue == 0)
                    {
                        //窗口最小化时候获得的TabHigh为0 会导致查询所有分时数据
                        if (ctrlKChart.TabHigh > 0)
                        {
                            int reqId = MDService.DataAPI.QryTradeSplitData(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0, ctrlKChart.TabHigh);
                            kChartUpdateRequest.TryAdd(reqId, this);
                        }
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


            #region 根据当前控件所显示合约执行合约查询
            IEnumerable<MDSymbol> symlist = GetSymbolsNeeded();

            //查询模式直接查询合约列表
            if (MDService.DataAPI.APISetting.TickMode == EnumMDTickMode.FreqQry)
            {
                if (symlist.Count() > 0)
                {
                    MDService.DataAPI.QryTickSnapshot(symlist.ToArray());
                }
            }
            else
            { 
                List<MDSymbol> needReg = new List<MDSymbol>();
                foreach(var sym in symlist)
                {
                    if (!symbolRegister.Contains(sym))
                    {
                        needReg.Add(sym);
                    }
                }
                if (needReg.Count > 0)
                {
                    MDService.DataAPI.RegisterSymbol(needReg.ToArray());

                    symbolRegister.AddRange(needReg);
                }
            }
            #endregion



            UpdateTime();
        }


       
    }
}
