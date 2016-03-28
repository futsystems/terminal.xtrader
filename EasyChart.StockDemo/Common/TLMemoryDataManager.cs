using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace Easychart.Finance.DataProvider
{
    /// <summary>
    /// 内存Bar数据管理器
    /// </summary>
    public class TLMemoryDataManager : DataManagerBase
    {
        ILog logger = LogManager.GetLogger("TLMemoryDataManager");

        
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

        TradingLib.MDClient.IHistData _histdata = null;
        

        Dictionary<int, List<BarImpl>> responseMap = new Dictionary<int, List<BarImpl>>();
        ThreadSafeList<int> reqIdList = new ThreadSafeList<int>();

        bool _intraday = true;
        DataCycle _mainCycle = DataCycle.Minute;

        FrequencyManager _freqmgr = null;

        public TLMemoryDataManager(bool intraday,TradingLib.MDClient.IHistData histdata)
        {
            _intraday = intraday;
            //日内主频率为分钟线 否则为日线
            _mainCycle = intraday ? DataCycle.Minute : DataCycle.Day;
            
            this.InnerDataManager = new TLLocalDataManager(intraday);
            this._histdata = histdata;
            this._histdata.OnRspBarEvent += new Action<RspQryBarResponseBin>(_histdata_OnRspBarEvent);
            this._histdata.OnRtnTickEvent += new Action<Tick>(_histdata_OnRtnTickEvent);

            _freqmgr = new FrequencyManager("Default", QSEnumDataFeedTypes.DEFAULT);
            _freqmgr.NewFreqKeyBarEvent += new Action<FrequencyManager.FreqKey, SingleBarEventArgs>(_freqmgr_NewFreqKeyBarEvent);
            Symbol sym = this._histdata.GetSymbol("IF1604");
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
                //DataPacket dp = new DataPacket(arg2.Bar.Symbol,)
                TLCommonDataProvider stream_cdp = TLMemoryDataManager.htStreaming["IF1604"] as TLCommonDataProvider;
                if (stream_cdp == null)
                {
                    stream_cdp = TLCommonDataProvider.CreateEmptyDataProvider("IF1604", _intraday);

                    TLMemoryDataManager.htStreaming["IF1604"] = stream_cdp;
                }

                TLCommonDataProvider main_cdp = TLMemoryDataManager.htHistorical["IF1604"] as TLCommonDataProvider;
                if (main_cdp == null)
                {
                    return;
                }
                 
                //获得当前DataProvider最新Bar
                double[] date = main_cdp["DATE"];
                DateTime lasttime = DateTime.FromOADate(date[date.Length - 1]);
                logger.Info("Last DateTime:" + lasttime);
                
                //1.将新产生的Bar数据合并到DataProvider中,DataProvider中的最后一个数据由Tick更新,FrequencyManager生成的Bar需要与当前比较
                Bar b = arg2.Bar;
                DataPacket dp = new DataPacket(b.Symbol, b.BarStartTime.ToOADate(), b.Open, b.High, b.Low, b.Close, b.Volume, b.Close);
                main_cdp.Merge(dp);

                //2.创建一个新的Bar进入下一个Bar周期 FrequencyManager默认用上个Bar的收盘价作为开盘价,有Tick过来时会进行更新 在绘图空间中没有这个逻辑 会始终以上个收盘价作为开盘价 因此控件上的Bar由Tick进行触发添加
                //dp = new DataPacket(b.Symbol,TimeFrequency.RoundTime(arg2.BarEndTime,TimeSpan.FromSeconds(_mainCycle.ToSeconds())), b.Close, b.Close, b.Close, b.Close,0, b.Close);
                //main_cdp.Merge(dp);

                //触发Bar更新事件 用于重绘
                if (BarUpdatedEvent != null)
                {
                    BarUpdatedEvent();
                }

            }

        }

        void _histdata_OnRtnTickEvent(Tick obj)
        {
            logger.Info("got tick:" + obj.ToString());
        }


        void _histdata_OnRspBarEvent(RspQryBarResponseBin response)
        {
            logger.Info("got bar response reqid:{0} islast:".Put(response.RequestID, response.IsLast));
            if (!reqIdList.Contains(response.RequestID)) return;

            List<BarImpl> barlist = null;
            if (!responseMap.TryGetValue(response.RequestID, out barlist))
            {
                barlist = new List<BarImpl>();
                responseMap.Add(response.RequestID, barlist);
            }
            barlist.AddRange(response.Bars);

            //最后一条数据 执行数据合并
            if (response.IsLast)
            {
                TLCommonDataProvider main_cdp = (TLCommonDataProvider)TLMemoryDataManager.htHistorical["IF1604"];
                
                //将barlist生成临时DataProvider并执行合并
                TLCommonDataProvider tmp = TLCommonDataProvider.CreateEmptyDataProvider("IF1604",_intraday);
                tmp.DataCycle = _mainCycle;

                tmp.LoadBinary(ConvertBars(barlist.ToArray()));
                main_cdp.Merge(tmp);

                TLCommonDataProvider real_cdp = (TLCommonDataProvider)TLMemoryDataManager.htStreaming["IF1604"];
                if (real_cdp != null)
                {
                    main_cdp.Merge(real_cdp);
                    real_cdp.ClearData();
                }
                //commonDataProvider.SetStringData("Code", Code);

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
            //1.Bar数据生成器响应Tick 如果有新的Bar生成 会合并到当前数据集合
            _freqmgr.ProcessTick(k);

            //2.调用对应的DataProvider处理Tick用于更新最新Bar的高开低收
            bool update = false;
            TLCommonDataProvider commonDataProvider = (TLCommonDataProvider)TLMemoryDataManager.htHistorical["IF1604"];
            if (commonDataProvider != null)
            {
                commonDataProvider.Merge(k, out update);
            }
            if (update)
            {
                if (BarUpdatedEvent != null)
                {
                    BarUpdatedEvent();
                }
            }

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
                data[5][j] = bars[j].BarStartTime.ToOADate();
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
            int reqId = this._histdata.QryBar(symbol, interval, start, end);
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
            QryBar("IF1604",secends,DateTime.MinValue, DateTime.Now);

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

                RegisterSymbol("IF1604");

                
            }
            //logger.Info("current datacycle:" + commonDataProvider.DataCycle.ToString());
            //获得实时Bar数据并进行合并同时清空实时数据
            TLCommonDataProvider commonDataProvider2 = (TLCommonDataProvider)TLMemoryDataManager.htStreaming[Code];
            if (commonDataProvider2 != null)
            {
                commonDataProvider.Merge(commonDataProvider2);
                commonDataProvider2.ClearData();
            }
            commonDataProvider.SetStringData("Code", Code);
            return commonDataProvider;
        }
    }
}
