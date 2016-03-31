﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;
using System.Windows.Forms;

namespace Easychart.Finance.DataProvider
{
    /// <summary>
    /// 内存Bar数据管理器
    /// </summary>
    public class TLMemoryDataManager : DataManagerBase
    {
        ILog logger = LogManager.GetLogger("TLMemoryDataManager");

        //const string SYMBOL = "rb1610";
        const string SYMBOL = "rb1610";
        /// <summary>
        /// 数据查询处理完毕事件
        /// </summary>
        public event Action DataProcessedEvent;

        public event Action BarUpdatedEvent;
        /// <summary>
        /// 实时数据
        /// 响应实时行情 生成的数据
        /// </summary>
        private static Hashtable htStreaming = new Hashtable();

        /// <summary>
        /// 历史数据 用于从本地储存加载获得的数据
        /// </summary>
        private static Hashtable htHistorical = new Hashtable();


        private static Hashtable htQrying = new Hashtable();

        private DataManagerBase InnerDataManager;

        //public void AddNewPacket(DataPacket dp)
        //{
        //    string text = dp.Symbol;
        //    if (text.EndsWith("=X"))
        //    {
        //        text = text.Substring(0, text.Length - 2);
        //    }
        //    CommonDataProvider commonDataProvider = TLMemoryDataManager.htStreaming[text] as CommonDataProvider;
        //    if (commonDataProvider == null)
        //    {
        //        commonDataProvider = CommonDataProvider.Empty;
        //        TLMemoryDataManager.htStreaming[text]  =  commonDataProvider;
        //    }
        //    commonDataProvider.Merge(dp);
        //}

        public void RemoveSymbol(string Symbol)
        {
            TLMemoryDataManager.htHistorical.Remove(Symbol);
            TLMemoryDataManager.htStreaming.Remove(Symbol);
        }

        IDataClient _histdata = null;
        

        Dictionary<int, List<BarImpl>> responseMap = new Dictionary<int, List<BarImpl>>();
        ThreadSafeList<int> reqIdList = new ThreadSafeList<int>();

        bool _intraday = true;
        DataCycle _mainCycle = DataCycle.Minute;

        FrequencyManager _freqmgr = null;

        public TLMemoryDataManager(bool intraday,IDataClient histdata)
        {
            _intraday = intraday;
            //日内主频率为分钟线 否则为日线
            _mainCycle = intraday ? DataCycle.Minute : DataCycle.Day;
            
            this.InnerDataManager = new TLLocalDataManager(intraday);
            this._histdata = histdata;
            this._histdata.OnRspBarEvent += new Action<RspQryBarResponseBin>(_histdata_OnRspBarEvent);
            this._histdata.OnRtnTickEvent += new Action<Tick>(OnRtnTickEvent);

            _freqmgr = new FrequencyManager("Default", QSEnumDataFeedTypes.DEFAULT);
            _freqmgr.NewFreqKeyBarEvent += new Action<FrequencyManager.FreqKey, SingleBarEventArgs>(_freqmgr_NewFreqKeyBarEvent);
            Symbol sym = this._histdata.GetSymbol(SYMBOL);
            _freqmgr.RegisterSymbol(sym);
        }

        /// <summary>
        /// 响应新的本地Bar数据
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void _freqmgr_NewFreqKeyBarEvent(FrequencyManager.FreqKey arg1, SingleBarEventArgs arg2)
        {
            //分钟数据产生新的Bar数据
            if (arg1.Settings.BarFrequency.Interval == 60)
            {
                logger.Info("New Bar Generated:" + arg2.Bar.ToString());
                //lock (main_cdp)
                {
                    //1.将新产生的Bar数据合并到stream_cdp中,在下次获取数据时候 并入main_cdp.DataProvider中的最后一个数据由Tick更新,FrequencyManager生成的Bar需要与当前比较
                    Bar b = arg2.Bar;
                    DataPacket dp = new DataPacket(b.Symbol, b.StartTime.ToOADate(), b.Open, b.High, b.Low, b.Close, b.Volume, b.Close);
                    TLCommonDataProvider stream_cdp = GetStreamCDP(SYMBOL);
                    //将新生成的Bar放入stream_cdp 在下次获取数据时候 并入main_cdp
                    stream_cdp.Merge(dp);
                }

                //触发Bar更新事件 用于重绘
                if (BarUpdatedEvent != null)
                {
                    BarUpdatedEvent();
                }

            }

        }

        void OnRtnTickEvent(Tick k)
        {
            //过滤无效合约
            if (k == null) return;
            if (!k.IsTrade()) return;//成交数据
            if (string.IsNullOrEmpty(k.Symbol)) return;//合约字段有效
            if (k.Date * k.Time == 0) return;//时间字段有效

            if (k.DataFeed != QSEnumDataFeedTypes.CTP) return;
            //1.Bar数据生成器响应Tick 如果有新的Bar生成 会合并到当前数据集合
            _freqmgr.ProcessTick(k);

            //2.调用对应的DataProvider处理Tick用于更新最新Bar的高开低收
            bool update = false;
            TLCommonDataProvider commonDataProvider = (TLCommonDataProvider)TLMemoryDataManager.htHistorical[SYMBOL];

            if (commonDataProvider != null)
            {
                commonDataProvider.Merge(k, out update);
            }
            //TLCommonDataProvider stream_cdp = GetStreamCDP(SYMBOL);
            //stream_cdp.Merge(k, out update);
            if (update)
            {
                if (BarUpdatedEvent != null)
                {
                    BarUpdatedEvent();
                }
            }
        }


        TLCommonDataProvider GetStreamCDP(string symbol)
        {
            TLCommonDataProvider stream_cdp = TLMemoryDataManager.htStreaming[symbol] as TLCommonDataProvider;
            if (stream_cdp == null)
            {
                stream_cdp = TLCommonDataProvider.CreateEmptyDataProvider(symbol, _intraday);
                stream_cdp.DataCycle = _mainCycle;
                TLMemoryDataManager.htStreaming[symbol] = stream_cdp;
            }
            return stream_cdp;
        }


        void _histdata_OnRspBarEvent(RspQryBarResponseBin response)
        {

            
            logger.Info("got bar response reqid:{0} islast:".Put(response.RequestID, response.IsLast));
            //不属于本组件请求的回报直接过滤
            if (!reqIdList.Contains(response.RequestID)) return;
            //将数据存在对应的缓存中
            List<BarImpl> barlist = null;
            if (!responseMap.TryGetValue(response.RequestID, out barlist))
            {
                barlist = new List<BarImpl>();
                responseMap.Add(response.RequestID, barlist);
            }
            barlist.AddRange(response.Bars);

            //接收完所有数据后 将服务端返回的Bar数据加载到stream 等待下次取数据时合并
            if (response.IsLast)
            {
                TLCommonDataProvider stream_cdp = GetStreamCDP(SYMBOL);
                stream_cdp.LoadBinary(ConvertBars(barlist.ToArray()));

                if (DataProcessedEvent != null)
                {
                    logger.Info("HistBar update");
                    DataProcessedEvent();
                }

                //this.InnerDataManager.SaveData("IF1604", main_cdp,true);
                reqIdList.Remove(response.RequestID);
            }
        }


        public void GotTick(Tick k)
        {

        }


        public double[][] ConvertBars(BarImpl[] bars)
        {
            int len = bars.Count();
            double[][] data = new double[6][];
            for (int i = 0; i < 6; i++)
            {
                data[i] = new double[len];
            }
            for (int j = 0; j < len; j++)
            {
                data[0][j] = bars[j].Open;
                data[1][j] = bars[j].High;
                data[2][j] = bars[j].Low;
                data[3][j] = bars[j].Close;
                data[4][j] = bars[j].Volume;
                data[5][j] = bars[j].StartTime.ToOADate();
            }
            return data;
        }


        /// <summary>
        /// 响应实时数据
        /// </summary>
        /// <param name="dp"></param>
        public void AddNewPacket(Tick k)
        {
            string key = k.Symbol;

            TLCommonDataProvider stream_cdp = TLMemoryDataManager.htStreaming[key] as TLCommonDataProvider;
            if (stream_cdp == null)
            {
                //stream_cdp = TLCommonDataProvider.CreateEmptyDataProvider(_intraday);
                //TLMemoryDataManager.htStreaming[key] = stream_cdp;
            }
            //stream_cdp.Merge(dp);
        }

        Random rm = new Random();
        public void DemoTick()
        {
            TLCommonDataProvider dp = TLMemoryDataManager.htHistorical["IF1604"] as TLCommonDataProvider;
            if (dp != null)
            {
                //double[] array = dp["Close"];
                //logger.Info("old vlaue:{0}".Put(array[array.Length - 1]));
                //array[array.Length - 1] += rm.Next(-5, 5);
                //logger.Info("new value:{0}".Put(array[array.Length - 1]));

                Tick k = new TickImpl();
                k.Symbol = "IF1604";
                k.Date = DateTime.Now.ToTLDate();
                k.Time = DateTime.Now.ToTLTime();
                k.Trade = 3129 + rm.Next(-5, 5);
                k.Size = 10;


                this.GotTick(k);

                
                //DataPacket tmp = new DataPacket(k.Symbol, DateTime.Now, (double)k.Trade, (double)k.Size);

                //TLCommonDataProvider stream_cdp = TLMemoryDataManager.htStreaming["IF1604"] as TLCommonDataProvider;
                //if (stream_cdp == null)
                //{
                //    stream_cdp = TLCommonDataProvider.CreateEmptyDataProvider("IF1604",_intraday);
                    
                //    TLMemoryDataManager.htStreaming["IF1604"] = stream_cdp;
                //}
                //stream_cdp.Merge(tmp);

                //if (BarUpdatedEvent != null)
                //{
                //    BarUpdatedEvent();
                //}
            }
            
        }

        
        /// <summary>
        /// 查询某个合约某个时间段内的Bar数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void QryBar(string symbol, int interval, DateTime start, DateTime end)
        {
            logger.Info("Qry Bar from server,symbol:{0} interval:{1} start:{2} end:{3}".Put(symbol, interval, start, end));
            int reqId = this._histdata.QryBar(symbol, interval, start, end,10000);
            reqIdList.Add(reqId);
        }

        void RegisterSymbol(string symbol)
        {
            this._histdata.RegisterSymbol(new string[] { symbol });
        }

        /// <summary>
        /// 回补某个DataProvider历史数据
        /// </summary>
        /// <param name="cdp"></param>
        void CoverBars(TLCommonDataProvider cdp,DataCycle cycle)
        {
            DateTime start = DateTime.MinValue;
            var data = cdp.GetCycleData(cycle);
            if (data.Keys.Count == 0)
            {
                start = DateTime.MinValue;
            }
            else
            {
                double[] array = (double[])data["DATE"];
                if (array.Length == 0)
                {
                    start = DateTime.MinValue;
                }
                else
                {
                    start = DateTime.FromOADate(array[array.Length - 1]);
                }
            }
            int secends = cycle.ToSeconds();
            QryBar(SYMBOL,secends,DateTime.MinValue, DateTime.Now);

        }


        /// <summary>
        /// ChartControl分为HistDay数据,IntraDay为日内
        /// 绘图控件通过GetData返回IDataProvider,在BindData中设定频率然后在通过IDataProvider获取数据时 获得对应频率的数据
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public override IDataProvider GetData(string Code, int Count)
        {
            //logger.Info("GetData code:{0} count:{1}".Put(Code, Count));

            TLCommonDataProvider tmp = (TLCommonDataProvider)TLMemoryDataManager.htHistorical[Code];
            if (tmp != null)
            {
                //logger.Info("Last datacycle:{0}".Put(tmp.DataCycle));
            }
            
            //TLLocalDataManager lm = new TLLocalDataManager();
            //通过编码获得历史数据 如果内存中历史数据不存在则通过本地储存进行加载
            TLCommonDataProvider commonDataProvider = (TLCommonDataProvider)TLMemoryDataManager.htHistorical[Code];
            if (commonDataProvider == null)
            {
                if (this.InnerDataManager != null)
                {
                    this.InnerDataManager.StartTime = base.StartTime;
                    this.InnerDataManager.EndTime = base.EndTime;
                    commonDataProvider = (this.InnerDataManager[Code, Count] as TLCommonDataProvider);
                    commonDataProvider.DataManager = this;
                }
                else
                {
                    commonDataProvider = TLCommonDataProvider.CreateEmptyDataProvider(Code,_intraday);
                    commonDataProvider.DataManager = this;
                }
                TLMemoryDataManager.htHistorical[Code] = commonDataProvider;

                //本地加载完毕后 回补数据
                CoverBars(commonDataProvider, _mainCycle);

                RegisterSymbol(SYMBOL);
            }
            //logger.Info("current datacycle:" + commonDataProvider.DataCycle.ToString());
            //获得实时Bar数据并进行合并同时清空实时数据
            TLCommonDataProvider stream_cdp = (TLCommonDataProvider)TLMemoryDataManager.htStreaming[Code];
            if (stream_cdp != null && stream_cdp.Count>0)
            {
                commonDataProvider.Merge(stream_cdp);
                stream_cdp.ClearData();
            }
            commonDataProvider.SetStringData("Code", Code);
            return commonDataProvider;
        }
    }
}
