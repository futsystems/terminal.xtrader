﻿using System;
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

namespace XTraderLite
{
    public partial class MainForm
    {
        ConcurrentDictionary<int, object> tickListLoadRequest = new ConcurrentDictionary<int, object>();
        ConcurrentDictionary<int, object> tickListUpdateRequest = new ConcurrentDictionary<int, object>();

        ConcurrentDictionary<int, object> kChartLoadTradeRequest = new ConcurrentDictionary<int, object>();
        ConcurrentDictionary<int, object> kChartUpdateRequest = new ConcurrentDictionary<int, object>();

        ConcurrentDictionary<int, object> priceVolListRequest = new ConcurrentDictionary<int, object>();

        //K线图加载更多历史数据请求
        ConcurrentDictionary<int, object> kChartLoadMoreDataRequest = new ConcurrentDictionary<int, object>();
        //K线图实时Bar数据请起
        ConcurrentDictionary<int, object> kChartRealTimeBarRequest = new ConcurrentDictionary<int, object>();
        //显示多日分时 需要对应交易日期,通过查询最近10日日线数据来获得
        ConcurrentDictionary<int, object> kChartIntraViewDayBarRequest = new ConcurrentDictionary<int, object>();

        //}

        /// <summary>
        /// 绑定行情接口回调
        /// </summary>
        void BindDataAPICallBack()
        {
            //当日分时数据返回
            MDService.DataAPI.OnRspQryMinuteData += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQryMinuteData);
            MDService.DataAPI.OnRspQryHistMinuteData += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQryHistMinuteData);
            MDService.DataAPI.OnRspQrySecurityBar += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQrySecurityBar);
            MDService.DataAPI.OnRspQryTickSnapshot += new Action<List<MDSymbol>, RspInfo, int, int>(DataAPI_OnRspQryTickSnapshot);

            //量价信息
            MDService.DataAPI.OnRspQryPriceVolPair += new Action<List<PriceVolPair>, RspInfo, int, int>(DataAPI_OnRspQryPriceVolPair);
            //分笔数据
            MDService.DataAPI.OnRspQryTradeSplit += new Action<List<TradeSplit>, RspInfo, int, int>(DataAPI_OnRspQryTradeSplit);

            //合约信息类别
            MDService.DataAPI.OnRspQrySymbolInfoType += new Action<List<SymbolInfoType>, RspInfo, int, int>(DataAPI_OnRspQrySymbolInfoType);
            //合约信息回报
            MDService.DataAPI.OnRspQrySymbolInfo += new Action<string, RspInfo, int, int>(DataAPI_OnRspQrySymbolInfo);
        }





        /// <summary>
        /// 响应量价数据
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void DataAPI_OnRspQryPriceVolPair(List<PriceVolPair> arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (this.InvokeRequired)
            {
                //logger.Info("invoked required");
                this.Invoke(new Action<List<PriceVolPair>, RspInfo, int, int>(DataAPI_OnRspQryPriceVolPair), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                object target = null;
                if (priceVolListRequest.TryGetValue(arg4, out target))
                {
                    ctrlPriceVolList.BeginUpdate();
                    ctrlPriceVolList.Clear();
                    ctrlPriceVolList.Add(arg1);
                    ctrlPriceVolList.EndUpdate();
                    return;
                }

                ctrlKChart.ClearPriceVol();
                ctrlKChart.AddPriceVol(arg1, true);
            }
        }

        /// <summary>
        /// 响应分笔数据
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void DataAPI_OnRspQryTradeSplit(List<TradeSplit> arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (this.InvokeRequired)
            {
                //logger.Info("invoked required");
                this.Invoke(new Action<List<TradeSplit>, RspInfo, int, int>(DataAPI_OnRspQryTradeSplit), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                object target = null;
                //绘图控件盘口明细加载分笔
                if (kChartLoadTradeRequest.TryRemove(arg4, out target))
                {
                    ctrlKChart.ClearTxnData();
                    ctrlKChart.AddTrade(arg1, true);
                }
                //绘图控件 更新分笔
                if (kChartUpdateRequest.TryRemove(arg4, out target))
                {
                    
                    ctrlKChart.ClearTxnData();
                    ctrlKChart.AddTrade(arg1, true);
                }

                //分笔明细视图 查询
                if (tickListLoadRequest.TryRemove(arg4, out target))
                {
                    logger.Info("Got TradeSplit for TickList");
                    ctrlTickList.BeginUpdate();
                    ctrlTickList.AddFirst(arg1);
                    ctrlTickList.EndUpdate();
                    //如果返回的数量与我们请求数量一致 表面可能还有更多数据需要加载 因此再次请求 起始位置为我们当前已经获得数据量
                    if (arg3 == 2000)
                    {
                        logger.Info("there are more data");
                        int reqId = MDService.DataAPI.QryTradeSplitData(ctrlTickList.Symbol.Exchange, ctrlTickList.Symbol.Symbol, ctrlTickList.Count, 2000);
                        tickListLoadRequest.TryAdd(reqId, ctrlTickList);
                    }
                    return;
                }

                //分笔明细视图 实时更新
                if (tickListUpdateRequest.TryRemove(arg4, out target))
                {
                    logger.Info("Got TradeSplit update for ticklist");
                    ctrlTickList.BeginUpdate();
                    ctrlTickList.Update(arg1);
                    ctrlTickList.EndUpdate();
                }




                
            }
        }



        void DataAPI_OnRspQryHistMinuteData(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {
            double[] d1 = arg1["date"];//date
            double[] d2 = arg1["time"];//time
            double[] d3 = arg1["close"];//close
            double[] d4 = arg1["vol"];//vol
            for (int j = 0; j < arg3; j++)
            {
                MinuteData dt = new MinuteData();
                dt.Date = (int)d1[j];
                dt.Time = (int)d2[j];
                dt.Close = d3[j];
                dt.Vol = d4[j];
                minuteData.Add(dt);
            }
        }

        void DataAPI_OnRspQryMinuteData(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {

            if (this.InvokeRequired)
            {
                Invoke(new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQryMinuteData), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                //清空当前分时数据
                ctrlKChart.ClearIntraViewData();

                double[] d1 = arg1["date"];//date
                double[] d2 = arg1["time"];//time
                double[] d3 = arg1["close"];//close
                double[] d4 = arg1["vol"];//vol
                //多日分时查询将历史分时数据与今日数据拼接后传递给绘图控件
                if ((ctrlKChart.DaysForIntradayView > 1) && (minuteData.Count > 0))
                {
                    double[] date1 = new double[minuteData.Count + arg3 + 1];
                    double[] time11 = new double[minuteData.Count + arg3 + 1];
                    double[] close1 = new double[minuteData.Count + arg3 + 1];
                    double[] vol1 = new double[minuteData.Count + arg3 + 1];
                    for (int j = 0; j < minuteData.Count; j++)
                    {
                        MinuteData dt = minuteData[j];
                        date1[j] = dt.Date;
                        time11[j] = dt.Time;
                        close1[j] = dt.Close;
                        vol1[j] = dt.Vol;
                    }
                    for (int j = 0; j < arg3; j++)
                    {
                        date1[minuteData.Count + j] = d1[j];
                        time11[minuteData.Count + j] = d2[j];
                        close1[minuteData.Count + j] = d3[j];
                        vol1[minuteData.Count + j] = d4[j];
                    }
                    int total = minuteData.Count + arg3;
                    ctrlKChart.FS_AddAll("date", date1, total, false);
                    ctrlKChart.FS_AddAll("time", time11, total, false);
                    ctrlKChart.FS_AddAll("vol", vol1, total, false);
                    ctrlKChart.FS_AddAll("close", close1, total, true);
                }
                else//当日历史分时数据直接传递给绘图控件
                {
                    ctrlKChart.FS_AddAll("date", d1, arg3, false);
                    ctrlKChart.FS_AddAll("time", d2, arg3, false);
                    ctrlKChart.FS_AddAll("close", d3, arg3, false);
                    ctrlKChart.FS_AddAll("vol", d4, arg3, true);
                }
            }
        }

        /// <summary>
        /// 响应Bar数据
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void DataAPI_OnRspQrySecurityBar(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {

            if (this.InvokeRequired)
            {
                Invoke(new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQrySecurityBar), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                object target = null;
                //查询最近的Bar数据并实时插入更新
                if (kChartRealTimeBarRequest.TryRemove(arg4, out target))
                {
                    if (arg3 == 0)
                    {
                        logger.Info("There is no realtime bar request,return");
                        return;
                    }
                    double[] date1, time1, high1, low1, open1, close1, amount1, upcount, downcount, vol1;
                    arg1.TryGetValue("date", out date1);
                    arg1.TryGetValue("time", out time1);
                    arg1.TryGetValue("high", out high1);
                    arg1.TryGetValue("low", out low1);
                    arg1.TryGetValue("open", out open1);
                    arg1.TryGetValue("close", out close1);
                    arg1.TryGetValue("amount", out amount1);
                    arg1.TryGetValue("vol", out vol1);
                    bool have = arg1.TryGetValue("upcount", out upcount);
                    arg1.TryGetValue("downcount", out downcount);


                    string first = string.Format("{0}-{1}", (int)date1[0], (int)time1[0] * 100);
                    string last = string.Format("{0}-{1}", (int)date1[arg3 - 1], (int)time1[arg3 - 1] * 100);
                    //logger.Info("Got RelaTime Bar, Count:{0} FirstTime:{1} LastTime:{2}".Put(arg3, first, last));
                    //获得返回数据的第一个时间 并查找该时间index然后SetValue
                    long dt = Utils.ToTLDateTime((int)date1[0], (int)time1[0] * 100);
                    int index = ctrlKChart.GetIndex(dt);

                    //将数据集重置到该index
                    ctrlKChart.ResetIndex(index);

                    ctrlKChart.AddKViewData("date", date1, arg3);//在数据集之后添加新的数据
                    ctrlKChart.AddKViewData("time", time1, arg3);
                    ctrlKChart.AddKViewData("high", high1, arg3);
                    ctrlKChart.AddKViewData("low", low1, arg3);
                    ctrlKChart.AddKViewData("open", open1, arg3);
                    ctrlKChart.AddKViewData("close", close1, arg3);
                    ctrlKChart.AddKViewData("amount", amount1, arg3);

                    if (have)
                    {
                        ctrlKChart.AddKViewData("upcount", upcount, arg3);
                        ctrlKChart.AddKViewData("downcount", downcount, arg3);
                    }
                    ctrlKChart.AddKViewData("vol", vol1, arg3);
                    ctrlKChart.ReCalculate("RealTimeBar");
                    ctrlKChart.ReDraw();
                    return;
                }

                //分时图标执行的日线查询 则获得有效日期然后提交历史分时与当日分时查询
                if (kChartIntraViewDayBarRequest.TryRemove(arg4, out target))
                {
                    double[] date;
                    if (arg1.TryGetValue("date", out date))
                    {
                        //如果显示多日分时数据
                        if (ctrlKChart.DaysForIntradayView > 1)
                        {
                            int t1 = Math.Max(0, arg3 - ctrlKChart.DaysForIntradayView + 1);
                            for (int j = t1; j < arg3; j++)
                            {
                                MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, (int)date[j]);//查询历史分时
                            }
                            MDService.DataAPI.QryMinuteDate(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, 0);//查询今日分时
                        }
                    }
                    return;
                }


                //K线图加载更多历史数据返回处理
                if (kChartLoadMoreDataRequest.TryRemove(arg4, out target))
                {
                    if (arg3 == 0)
                    {
                        logger.Info("there is no more data from server");
                        ctrlKChart.NoMoreBarDate = true;
                        return;
                    }
                    //从行情源返回的数据中加载所有数据 并插入到当前数据集前面
                    double[] date1, time1, high1, low1, open1, close1, amount1, upcount, downcount, vol1;
                    arg1.TryGetValue("date", out date1);
                    arg1.TryGetValue("time", out time1);
                    arg1.TryGetValue("high", out high1);
                    arg1.TryGetValue("low", out low1);
                    arg1.TryGetValue("open", out open1);
                    arg1.TryGetValue("close", out close1);
                    arg1.TryGetValue("amount", out amount1);
                    arg1.TryGetValue("vol", out vol1);


                    ctrlKChart.AddKViewData("date", date1, arg3, true);
                    ctrlKChart.AddKViewData("time", time1, arg3, true);
                    ctrlKChart.AddKViewData("high", high1, arg3, true);
                    ctrlKChart.AddKViewData("low", low1, arg3, true);
                    ctrlKChart.AddKViewData("open", open1, arg3, true);
                    ctrlKChart.AddKViewData("close", close1, arg3, true);
                    ctrlKChart.AddKViewData("amount", amount1, arg3, true);

                    bool have = arg1.TryGetValue("upcount", out upcount);
                    arg1.TryGetValue("downcount", out downcount); ;
                    if (have)
                    {
                        ctrlKChart.AddKViewData("upcount", upcount, arg3, true);
                        ctrlKChart.AddKViewData("downcount", downcount, arg3, true);
                    }
                    ctrlKChart.AddKViewData("vol", vol1, arg3, true);
                    //CStock.Constants.Profiler.EnterSection("Calculate");
                    ctrlKChart.ReCalculate("MoreData");
                    //CStock.Constants.Profiler.LeaveSection();

                    //CStock.Constants.Profiler.EnterSection("Draw");
                    ctrlKChart.ZoomOut();
                    //CStock.Constants.Profiler.LeaveSection();

                    //logger.Info(CStock.Constants.Profiler.GetStatsString());
                    return;
                }

                //K线图初始化数据查询回报处理 通过AddAll将数组一次性添加到数据集
                if (ctrlKChart.Visible)
                {
                    logger.Info("Fill  bar into KView");
                    //int ll = GP.StartIndex;

                    ctrlKChart.ClearKViewData();

                    int loadnum = arg3;
                    double[] date1, time1, high1, low1, open1, close1, amount1, upcount, downcount, vol1;
                    arg1.TryGetValue("date", out date1);
                    arg1.TryGetValue("time", out time1);
                    arg1.TryGetValue("high", out high1);
                    arg1.TryGetValue("low", out low1);
                    arg1.TryGetValue("open", out open1);
                    arg1.TryGetValue("close", out close1);
                    arg1.TryGetValue("amount", out amount1);
                    arg1.TryGetValue("vol", out vol1);


                    ctrlKChart.AddKViewData("date", date1, loadnum);
                    ctrlKChart.AddKViewData("time", time1, loadnum);
                    ctrlKChart.AddKViewData("high", high1, loadnum);
                    ctrlKChart.AddKViewData("low", low1, loadnum);
                    ctrlKChart.AddKViewData("open", open1, loadnum);
                    ctrlKChart.AddKViewData("close", close1, loadnum);
                    ctrlKChart.AddKViewData("amount", amount1, loadnum);

                    bool have = arg1.TryGetValue("upcount", out upcount);
                    arg1.TryGetValue("downcount", out downcount); ;
                    if (have)
                    {
                        ctrlKChart.AddKViewData("upcount", upcount, loadnum);
                        ctrlKChart.AddKViewData("downcount", downcount, loadnum);
                    }
                    ctrlKChart.AddKViewData("vol", vol1, loadnum);
                    ctrlKChart.ReCalculate("Init");
                    ctrlKChart.ReDraw();

                }
                logger.Info("KChart Updated");
            }
        }


        void DataAPI_OnRspQryTickSnapshot(List<MDSymbol> arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action<List<MDSymbol>, RspInfo, int, int>(DataAPI_OnRspQryTickSnapshot), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                foreach (var symbol in arg1)
                {

                    if (ctrlQuoteList.Visible)
                    {
                        ctrlQuoteList.Update(symbol);
                    }
                    ctrlSymbolHighLight.Update(symbol);

                    //保存LastTick
                    if (symbol.TickSnapshot.Time != symbol.LastTickSnapshot.Time)
                    {
                        symbol.LastTickSnapshot = symbol.TickSnapshot;
                    }
                }

                //驱动完毕所有行情快照监听者之后 更新last
                
            }


        }


        /// <summary>
        /// 响应基本信息查询中的类别回报
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void DataAPI_OnRspQrySymbolInfoType(List<SymbolInfoType> arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (this.InvokeRequired)
            {

                Invoke(new Action<List<SymbolInfoType>, RspInfo, int, int>(DataAPI_OnRspQrySymbolInfoType), new object[] { arg1, arg2, arg3, arg4 });
            }
            else
            {
                if (ctrlSymbolInfo.Visible)
                {
                    ctrlSymbolInfo.BeginUpdate();
                    ctrlSymbolInfo.Clear();
                    ctrlSymbolInfo.AddType(arg1);
                    ctrlSymbolInfo.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 响应基本信息回报
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void DataAPI_OnRspQrySymbolInfo(string arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action<string, RspInfo, int, int>(DataAPI_OnRspQrySymbolInfo), new object[] { arg1, arg2, arg3, arg4 });

            }
            else
            {
                if (ctrlSymbolInfo.Visible)
                {
                    ctrlSymbolInfo.SetInfo(arg1);
                }
            }
        }


    }
}
