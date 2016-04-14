using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;

using System.Reflection;
using System.Runtime.InteropServices;

using STOCKCHARTXLib;
using TradingLib.API;
using TradingLib.Common;
using Easychart.Finance;
using Easychart.Finance.DataProvider;
using TradingLib.DataProvider;

namespace TradingLib.KryptonControl
{
    public partial class PageStockChartX
    {

        TLMemoryDataManager _mdmIntraday = null;
        TLMemoryDataManager _mdmHist = null;

        DataCycle _currentDataCycle = null;

        public void BindDataManager(TLMemoryDataManager mdmIntraday)
        {
            this._mdmIntraday = mdmIntraday;
            //绑定数据源事件
            this._mdmIntraday.DataProcessedEvent += new Action(_mdmIntraday_DataProcessedEvent);


        }

        Profiler pf = new Profiler();
        void _mdmIntraday_DataProcessedEvent()
        {

            this.BindData();
            logger.Info("RecordCount:{0} PanelCount:{1} First:{2} Last:{3}".Put(StockChartX1.RecordCount, StockChartX1.PanelCount,FirstVisibleRecord,LastVisibleRecord));
            if (StockChartX1.RecordCount > this.DefaultVisibleRecordCount)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - this.DefaultVisibleRecordCount;
            }

            double[] date = new double[]{2,2,2};
            double[] value = new double[]{3,3,3};
            
            logger.Info("\n" + pf.GetStatsString());

            StockChartX1.Update();

            double jdate = StockChartX1.GetJDate(this.NAME_CLOSE, this.RecordCount);

            
            logger.Info("Jdate:" + jdate.ToString());
            logger.Info("LastBarTime:" + this.LastBarStartTime.ToString());
        }

        public void ShowChart(Symbol symbol)
        {
            this.ShowChart(symbol, new BarFrequency(BarInterval.CustomTime, 60));
        }

        public void ShowChart(Symbol symbol, BarFrequency freq)
        {
            try
            {
                logger.Info("Show Chart Symbol:{0} Freq:{1}".Put(symbol.Symbol, freq.ToUniqueId()));
                _symbol = symbol;
                _freq = freq;
                _currentDataCycle = new DataCycle(DataCycleBase.MINUTE, freq.Interval / 60);

                //
                this.ResetStockChartX();

                //
                this.BindData();

                //double[] date = new double[] { 1.1, 1.2, 1.3, 1.4, 1.5 };
                //int size = Marshal.SizeOf(date[0]) * date.Length;
                //IntPtr pnt = Marshal.AllocHGlobal(size);
                //Marshal.Copy(date, 0, pnt, date.Length);//建立非托管数组

                //StockChartX1.DemoMethod

            }
            catch (Exception ex)
            {
                logger.Error("ShowChart Error:" + ex.ToString());
            }
        }

        /// <summary>
        /// 判断Chart是否有数据
        /// </summary>
        /// <returns></returns>
        bool HasData()
        {
            return false;
        }

        private DataManagerBase _currentDataManager = null;
        private CommonDataProvider _currentDataProvider = null;

        /// <summary>
        /// 绑定数据
        /// 绑定数据时需要判断合约与频率是否发生变化如果发生变化则需要重新加载所有数据
        /// </summary>
        void BindData()
        {
            pf.EnterSection("BindData");
            logger.Info("Bind Data");
            bool result = false;
            if (_freq.TimeSpan >= TimeSpan.FromDays(1))
            {
                _currentDataManager = _mdmHist;
            }
            else
            {
                _currentDataManager = _mdmIntraday;
            }
            try
            {

                _currentDataProvider = (CommonDataProvider)_currentDataManager[_symbol.Symbol];
                //if (this.AfterBindData != null)
                //{
                //    this.AfterBindData(this, new BindDataEventArgs(commonDataProvider, this.CurrentDataManager));
                //}

                this.AppendBars(_currentDataProvider);

                result = (_currentDataProvider != null);
            }
            catch
            {

            }
            pf.LeaveSection();
        }

        /// <summary>
        /// 插入一个Bar数据
        /// </summary>
        /// <param name="bar"></param>
        void AppendBar(Bar bar)
        {
            this.AppendBar(ConvertToJulianDate(bar.StartTime), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume);
        }


        /// <summary>
        /// 插入一个Bar数据
        /// </summary>
        /// <param name="jdate"></param>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="vol"></param>
        void AppendBar(double jdate, double open, double high, double low, double close, double vol)
        {
            StockChartX1.AppendValue(NAME_OPEN, jdate,open);
            StockChartX1.AppendValue(NAME_HIGH, jdate, high);
            StockChartX1.AppendValue(NAME_LOW, jdate,low);
            StockChartX1.AppendValue(NAME_CLOSE, jdate,close);
            StockChartX1.AppendValue(NAME_VOLUME, jdate,vol);
        }

        void LoadSeries(string name, double[] jdate, double[] value)
        {
            try
            {
                logger.Info("Load Series:{0} Length:{1}".Put(name, jdate.Length));
                StockChartX1.AppendValues(name, jdate, value, jdate.Length);
            }
            catch (Exception ex)
            {
                logger.Error("LoadSeries Error:" + ex.ToString());
            }
        }

        /// <summary>
        /// 将IDataProvider提供的数据 填充到Chart中
        /// 大批量数组加载数据 通过StockChartX1.AppendValues(string,double[],double[],length)进行加载
        /// 
        /// </summary>
        /// <param name="provider"></param>
        void AppendBars(IDataProvider provider)
        {

            pf.EnterSection("AppendBars");
            double[] date = provider["DATE"];
            if (date.Length == 0)
            {
                logger.Info("Provider empty data");
                return;
            }
            date = date.Select(d => d.OADateToJulianDate()).ToArray();
            int count = this.RecordCount;
            //数据集长度大于等于当前Chart的数据个数
            if (date.Length >= count)
            {
                double[] open = provider["OPEN"];
                double[] high = provider["HIGH"];
                double[] low = provider["LOW"];
                double[] close =  provider["CLOSE"];
                double[] vol = provider["VOLUME"];

                LoadSeries(NAME_OPEN, date, open);
                LoadSeries(NAME_HIGH, date, high);
                LoadSeries(NAME_LOW, date, low);
                LoadSeries(NAME_CLOSE, date, close);
                LoadSeries(NAME_VOLUME, date,vol);


                ////更新count对应的数据
                //if (count != 0)
                //{
                //    this.UpdateBar(count, open[count - 1], high[count - 1], low[count - 1], close[count - 1], vol[count - 1]);
                //}
                ////新增
                //for (int i = count ; i < date.Length; i++)
                //{ 
                //    double jdate = ConvertToJulianDate(DateTime.FromOADate(date[i]));
                //    pf.EnterSection("AppendBar");
                //    this.AppendBar(jdate, open[i], high[i], low[i], close[i], vol[i]);
                //    pf.LeaveSection();
                //}
            }
            pf.LeaveSection();
        }

        /// <summary>
        /// 更新某个时间对应的Bar值
        /// </summary>
        /// <param name="jdate"></param>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="vol"></param>
        void UpdateBar(double jdate, double open, double high, double low, double close, double vol)
        {
            int record = GetRecordByDate(jdate);
            this.UpdateBar(record, open, high, low, close, vol);
        }

        /// <summary>
        /// StockChartX底层EditValueByRecord快于EditValue
        /// </summary>
        /// <param name="record"></param>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="vol"></param>
        void UpdateBar(int record,double open, double high, double low, double close, double vol)
        {
            StockChartX1.EditValueByRecord(NAME_OPEN, record, open);
            StockChartX1.EditValueByRecord(NAME_HIGH, record, high);
            StockChartX1.EditValueByRecord(NAME_LOW, record, low);
            StockChartX1.EditValueByRecord(NAME_CLOSE, record, close);
            StockChartX1.EditValueByRecord(NAME_VOLUME, record, vol);
        }

        /// <summary>
        /// 通过事件来获得对应的RecordID
        /// 通过遍历所有Panel以及对应的Series来比较Jdate
        /// </summary>
        /// <param name="df"></param>
        int GetRecordByDate(DateTime df)
        {
            return StockChartX1.GetRecordByJDate(ConvertToJulianDate(df));
        }

        int GetRecordByDate(double jdate)
        {
            return StockChartX1.GetRecordByJDate(jdate);
        }

        /// <summary>
        /// 转换时间到JulianDate
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        double ConvertToJulianDate(DateTime dt)
        {
            return StockChartX1.ToJulianDate(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        double GetValue(string name, int record)
        {
            return StockChartX1.GetValue(name, record);
        }

        double GetValueByJDate(string name, double jdate)
        {
            return StockChartX1.GetValueByJDate(name, jdate);
        }

        double GetValueByDate(string name, DateTime dt)
        {
            return StockChartX1.GetValueByJDate(name, ConvertToJulianDate(dt));

            
        }


        /// <summary>
        /// 最后一个Bar时间
        /// </summary>
        DateTime LastBarStartTime
        {
            get
            {
                return DateTime.FromOADate(StockChartX1.GetJDate(this.NAME_CLOSE, this.RecordCount).JulianDateToOADate());
            }
        }

        /// <summary>
        /// 获得Chart数据个数
        /// </summary>
        int RecordCount
        {
            get { return StockChartX1.RecordCount; }
        }



        /// <summary>
        /// 第一个可视记录
        /// </summary>
        int FirstVisibleRecord
        {
            get { return StockChartX1.FirstVisibleRecord; }
        }

        /// <summary>
        /// 最后一个视记录
        /// </summary>
        int LastVisibleRecord
        {
            get { return StockChartX1.LastVisibleRecord; }
        }
    }
}
