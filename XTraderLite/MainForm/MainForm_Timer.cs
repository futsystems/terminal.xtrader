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
        System.Timers.Timer timer;

        /// <summary>
        /// 初始化定时任务
        /// </summary>
        void InitTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            logger.Info("do timer work");
            if (ctrlTickList.Visible)
            {
                int reqId = MDService.DataAPI.QryTradeSplitData(_currentSymbol.Exchange, _currentSymbol.Symbol, 0, ctrlTickList.RowCount);//*ctrlTickList.ColumnCount);
                tickListUpdateRequest.TryAdd(reqId, this);

            }
        }


       
    }
}
