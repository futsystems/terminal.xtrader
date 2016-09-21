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

namespace XTraderLite
{
    public partial class MainForm
    {
        ConcurrentDictionary<int, object> tickListLoadRequest = new ConcurrentDictionary<int, object>();
        ConcurrentDictionary<int, object> tickListUpdateRequest = new ConcurrentDictionary<int, object>();

        ConcurrentDictionary<int, object> kChartLoadTradeRequest = new ConcurrentDictionary<int, object>();
        ConcurrentDictionary<int, object> kChartUpdateRequest = new ConcurrentDictionary<int, object>();

        ConcurrentDictionary<int, object> priceVolListRequest = new ConcurrentDictionary<int, object>();
        //ConcurrentDictionary<int, object> priceVolUpdateRequest = new ConcurrentDictionary<int, object>();

        //void InitDataAPI()
        //{
        //    //_dataAPI = new DataAPI.TDX.TDXDataAPI(new string[] {"218.85.137.40"},7709);
        //    //_dataAPI.OnConnected += new Action(_dataAPI_OnConnected);
        //    //_dataAPI.OnDisconnectd += new Action(_dataAPI_OnDisconnectd);
        //    //_dataAPI.OnLoginSuccess += new Action(_dataAPI_OnLoginSuccess);
        //    //_dataAPI.OnLoginFail += new Action(_dataAPI_OnLoginFail);
            
        //}

        /// <summary>
        /// 绑定行情接口回调
        /// </summary>
        void BindDataAPICallBack()
        {

            MDService.DataAPI.OnRspQryMinuteData += new Action<Dictionary<string, double[]>, RspInfo, int, int>(DataAPI_OnRspQryMinuteData);
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
                logger.Info("invoked required");
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
                logger.Info("invoked required");
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





        void DataAPI_OnRspQryMinuteData(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {

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

        void DataAPI_OnRspQrySecurityBar(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {

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


        void DataAPI_OnRspQryTickSnapshot(List<MDSymbol> arg1, RspInfo arg2, int arg3, int arg4)
        {
            //更新报价面板
            if (ctrlQuoteList.Visible)
            {
                logger.Info("更新报价面板");
                foreach(var symbol in arg1)
                {
                    ctrlQuoteList.Update(symbol);
                }
            }

            foreach (var symbol in arg1)
            {
                ctrlSymbolHighLight.Update(symbol);
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
            if (ctrlSymbolInfo.Visible)
            {
                ctrlSymbolInfo.BeginUpdate();
                ctrlSymbolInfo.Clear();
                ctrlSymbolInfo.AddType(arg1);
                ctrlSymbolInfo.EndUpdate();
            }
        }

        void DataAPI_OnRspQrySymbolInfo(string arg1, RspInfo arg2, int arg3, int arg4)
        {
            if (ctrlSymbolInfo.Visible)
            {
                ctrlSymbolInfo.SetInfo(arg1);
            }
        }


    }
}
