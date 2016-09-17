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
        List<MinuteData> minuteData = new List<MinuteData>();
        void InitKChart()
        {
            
        }

        void DataAPI_OnRspQryMinuteData(Dictionary<string, double[]> arg1, RspInfo arg2, int arg3, int arg4)
        {

            double[] d1 = arg1["date"];//date
            double[] d2 = arg1["time"];//time
            double[] d3 = arg1["close"];//close
            double[] d4 = arg1["vol"];//vol
            //多日分时查询将历史分时数据与今日数据拼接后传递给绘图控件
            if ((kChartView.DaysForIntradayView > 1) && (minuteData.Count > 0))
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
                kChartView.FS_AddAll("date", date1, total, false);
                kChartView.FS_AddAll("time", time11, total, false);
                kChartView.FS_AddAll("vol", vol1, total, false);
                kChartView.FS_AddAll("close", close1, total, true);
            }
            else//当日历史分时数据直接传递给绘图控件
            {
                kChartView.FS_AddAll("date", d1, arg3, false);
                kChartView.FS_AddAll("time", d2, arg3, false);
                kChartView.FS_AddAll("close", d3, arg3, false);
                kChartView.FS_AddAll("vol", d4, arg3, true);
            }
        }
    }
}
