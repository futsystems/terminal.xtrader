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
    /// <summary>
    /// 数据源与StockChartX更新
    /// 1.数据源结束一个Bar数据 Chart更新该Bar到图表
    /// 2.数据源产生一个新的Bar Chart插入一个Bar数据
    /// 3.
    /// </summary>
    public partial class PageStockChartX
    {

        TLMemoryDataManager _mdmIntraday = null;
        TLMemoryDataManager _mdmHist = null;

        DataCycle _currentDataCycle = null;

        bool _needRebind = false;

        public void BindDataManager(TLMemoryDataManager mdmIntraday)
        {
            this._mdmIntraday = mdmIntraday;
            //绑定数据源事件
            this._mdmIntraday.DataProcessedEvent += new Action(_mdmIntraday_DataProcessedEvent);


        }

        Profiler pf = new Profiler();
        System.Threading.Thread thread = null;
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
            thread = new System.Threading.Thread(Demotick);
            thread.IsBackground = true;
            thread.Start();
        }

        int vol = 10;
        Bar b = null;
        Random rd = new Random();
        void Demotick()
        {
            while (true)
            {
                if (b == null)
                {
                    b = new BarImpl();
                    b.StartTime = Util.ToDateTime(20160407150000);
                    b.Open = 2191.000000;
                    b.High = 2191.000000;
                    b.Low = 2191.000000;
                    b.Close = 2191.000000;
                }
                double trade = 2191 + rd.Next(-8, 8);
                b.High = b.High > trade ? b.High : trade;
                b.Low = b.Low < trade ? b.Low : trade;
                b.Close = trade;

                b.Volume = b.Volume + 5;
                this.UpdateBar(b);
                this.UpdateChart();
                Util.sleep(500);
            }

            
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

                bool symbolChagned = _symbol == null || _symbol.Symbol != symbol.Symbol;
                _symbol = symbol;
                bool freqChanged = _freq == null || _freq.TimeSpan != freq.TimeSpan;
                _freq = freq;
                if (symbolChagned || freqChanged)
                {
                    _needRebind = true;
                }
                else
                {
                    logger.Info("Symbol and freq not changed");
                    //return;
                }
                _currentDataCycle = new DataCycle(DataCycleBase.MINUTE, freq.Interval / 60);


                //重置Chart
                this.ResetStockChartX();

                //绑定数据
                this.BindData();

                //更新图表
                this.UpdateChart();

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

            CommonDataProvider commonDataProvider = null;

            //根据频率切换日内数据源或日线数据源头
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
                commonDataProvider = (CommonDataProvider)_currentDataManager[_symbol.Symbol];
                commonDataProvider.dataCycle = _currentDataCycle;


                _currentDataProvider = commonDataProvider;

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

        #region 响应实时数据

        /// <summary>
        /// 获得实时行情数据
        /// 由于控件只显示一个合约 因此需要判断行情合约与当前显示合约是否一致
        /// 该Tick只用最新价格去更新最新Bar数据 不负责检查该Tick是否在该Bar内
        /// Bar的具体生成逻辑由DataManager去负责
        /// 对应的Bar为该Tick更新后的最新Bar数据
        /// </summary>
        /// <param name="k"></param>
        void GotTick(Tick k,Bar b)
        {
            if (k == null) return;
            if (!k.IsTrade()) return;//非成交Tick直接返回
            if (_symbol == null) return;
            if (_symbol.Symbol != k.Symbol) return;

            this.UpdateBar(b);
        }
        #endregion

        #region 插入/更新一个Bar数据

        void UpdateBar(Bar bar)
        {
            this.UpdateBar(bar.StartTime.ToJulianDate(), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume,bar.OpenInterest);
        }

        void UpdateBar(double jdate, double open, double high, double low, double close, double vol, double oi)
        {
            int record = GetRecordByDate(jdate);
            //判断record是否有效
            StockChartX1.EditValueByRecord(NAME_OPEN, record, open);
            StockChartX1.EditValueByRecord(NAME_HIGH, record, high);
            StockChartX1.EditValueByRecord(NAME_LOW, record, low);
            StockChartX1.EditValueByRecord(NAME_CLOSE, record, close);
            StockChartX1.EditValueByRecord(NAME_VOLUME, record, vol);
        }


        /// <summary>
        /// 插入一个Bar数据
        /// </summary>
        /// <param name="bar"></param>
        void AppendBar(Bar bar)
        {
            this.AppendBar(bar.StartTime.ToJulianDate(), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume);
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


        #endregion



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
        /// 通过时间来获得对应的RecordID
        /// 通过遍历所有Panel以及对应的Series来比较Jdate
        /// </summary>
        /// <param name="df"></param>
        int GetRecordByDate(DateTime df)
        {
            return StockChartX1.GetRecordByJDate(df.ToJulianDate());
        }

        int GetRecordByDate(double jdate)
        {
            return StockChartX1.GetRecordByJDate(jdate);
        }


        #region 获得某个数据序列 某个记录的值

        double GetValue(string name, int record)
        {
            return StockChartX1.GetValue(name, record);
        }

        double GetValueByDate(string name, double jdate)
        {
            return StockChartX1.GetValueByJDate(name, jdate);
        }

        double GetValueByDate(string name, DateTime dt)
        {
            return StockChartX1.GetValueByJDate(name, dt.ToJulianDate());
        }

        #endregion



        //当前显示数量
        int _currentVisibleRecordCount = -1;

        /// <summary>
        /// 更新图表
        /// </summary>
        void UpdateChart()
        {

            if (_currentVisibleRecordCount < 0)
            {
                _currentVisibleRecordCount = this.DefaultVisibleRecordCount;
            }

            //设定显示数量
            if (StockChartX1.RecordCount > _currentVisibleRecordCount)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - _currentVisibleRecordCount;
            }
            StockChartX1.Update();
        }

        /// <summary>
        /// 最后一个Bar时间
        /// </summary>
        DateTime LastBarStartTime
        {
            get
            {
                if (this.RecordCount == 0) return DateTime.MinValue;
                return DateTime.FromOADate(StockChartX1.GetJDate(this.NAME_CLOSE, this.RecordCount).JulianDateToOADate());
            }
        }

        /// <summary>
        /// 获得总Bar数据个数
        /// </summary>
        int RecordCount
        {
            get { return StockChartX1.RecordCount; }
        }

        /// <summary>
        /// 当前图表显示的Bar数据
        /// </summary>
        int VisibleRecordCount
        {
            get { return StockChartX1.VisibleRecordCount; }
        }


        /// <summary>
        /// 第一个可视记录
        /// 通过设置 图表会更新
        /// 不用单独调用Update
        /// </summary>
        int FirstVisibleRecord
        {
            get { return StockChartX1.FirstVisibleRecord; }
            set { StockChartX1.FirstVisibleRecord = value; }
        }

        /// <summary>
        /// 最后一个视记录
        /// 通过设置后 图表会更新
        /// 不用单独调用Update
        /// </summary>
        int LastVisibleRecord
        {
            get { return StockChartX1.LastVisibleRecord; }
            set { StockChartX1.LastVisibleRecord = value; }
        }
    }
}
