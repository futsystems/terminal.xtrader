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

        void InitDataAPI()
        {
            //_dataAPI = new DataAPI.TDX.TDXDataAPI(new string[] {"218.85.137.40"},7709);
            //_dataAPI.OnConnected += new Action(_dataAPI_OnConnected);
            //_dataAPI.OnDisconnectd += new Action(_dataAPI_OnDisconnectd);
            //_dataAPI.OnLoginSuccess += new Action(_dataAPI_OnLoginSuccess);
            //_dataAPI.OnLoginFail += new Action(_dataAPI_OnLoginFail);
            
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
            foreach (var v in arg1)
            {
                ctrlKChart.AddPriceVol(v.Price, v.Vol);
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
            foreach (var v in arg1)
            {
                ctrlKChart.AddTxnData(v.Time, v.Price, v.Vol, v.Flag, v.TradeCount);
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


        }

    }
}
