using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

using Easychart.Finance;
using Easychart.Finance.DataProvider;

namespace TradingLib.DataProvider
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
        /// 历史数据 用于从本地储存加载获得的数据
        /// 该数据集为主数据 实时生成的数据需要合并到该数据集
        /// </summary>
        private static Hashtable htHistorical = new Hashtable();

        /// <summary>
        /// 实时数据
        /// 服务端查询获得的历史数据以及本地实时生成的数据
        /// </summary>
        private static Hashtable htStreaming = new Hashtable();

        /// <summary>
        /// 内部DataManager用于从本地文件加载历史数据
        /// </summary>
        private DataManagerBase InnerDataManager = null;

        /// <summary>
        /// 行情客户端接口
        /// 用于订阅实时行情 查询历史数据
        /// </summary>
        IDataClient dataClient = null;


        /// <summary>
        /// 内部查询RequestID 用于记录本地发起的请求
        /// 在回报处理函数中需要判断该回报是否需要处理 有可能是其他组件发起的数据查询操作
        /// </summary>
        ThreadSafeList<int> reqIdList = new ThreadSafeList<int>();
        
        /// <summary>
        /// 用于存放某个Bar数据查询所返回的Bar数据结果，服务端可能分几个消息进行返回 但是数据加载时需要整体进行加载
        /// </summary>
        Dictionary<int, List<BarImpl>> barRspMap = new Dictionary<int, List<BarImpl>>();
        

        /// <summary>
        /// 是否为日内数据集
        /// </summary>
        bool intraday = true;

        public bool IntraDay { get { return intraday; } }

        /// <summary>
        /// 主频率
        /// IDataProvider提供给绘图控件使用 绘图控件在绑定数据时,只提供Symbol作为参数
        /// 在绑定过程中动态设定IDataProvider的DataCycle参数来实现数据的返回
        /// 因此绘图控件的数据集 分为日内数据和Eod数据 方便数据维护与缩影
        /// 日线级别以上的数据由日线数据维护 分钟线级别以上的数据由分钟数据维护
        /// </summary>
        DataCycle mainCycle = DataCycle.Minute;

        /// <summary>
        /// Bar数据发生器
        /// </summary>
        FrequencyManager freqmgr = null;


        public TLMemoryDataManager(IDataClient histdata,bool isintraday)
        {
            intraday = isintraday;
            //日内主频率为分钟线 否则为日线
            mainCycle = intraday ? DataCycle.Minute : DataCycle.Day;
            
            this.InnerDataManager = new TLLocalDataManager(intraday);

            this.dataClient = histdata;
            this.dataClient.OnRspBarEvent += new Action<RspQryBarResponseBin>(OnRspBarEvent);
            this.dataClient.OnRtnTickEvent += new Action<Tick>(OnRtnTickEvent);

            this.freqmgr = new FrequencyManager("Default", QSEnumDataFeedTypes.DEFAULT);
            if (intraday)
            {
                this.freqmgr.RegisterFrequencies(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 60)));
            }
            this.freqmgr.NewFreqKeyBarEvent += new Action<FrequencyManager.FreqKey, SingleBarEventArgs>(OnNewFreqKeyBarEvent);
            

            Symbol sym = this.dataClient.GetSymbol(SYMBOL);
            this.freqmgr.RegisterSymbol(sym);
        }

        /// <summary>
        /// 响应新的本地Bar数据
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void OnNewFreqKeyBarEvent(FrequencyManager.FreqKey arg1, SingleBarEventArgs arg2)
        {
            
            //分钟数据产生新的Bar数据
            if (arg1.Settings.BarFrequency.Interval == 60)
            {
                logger.Info("New Bar Generated:" + arg2.Bar.ToString());
                //1.将新产生的Bar数据合并到stream_cdp中,在下次获取数据时候 并入main_cdp.DataProvider中的最后一个数据由Tick更新,FrequencyManager生成的Bar需要与当前比较
                Bar b = arg2.Bar;
                DataPacket dp = new DataPacket(b.Symbol, b.StartTime.ToOADate(), b.Open, b.High, b.Low, b.Close, b.Volume, b.Close);
                TLCommonDataProvider stream_cdp = GetStreamCDP(SYMBOL);
                //将新生成的Bar放入stream_cdp 在下次获取数据时候 并入main_cdp
                stream_cdp.Merge(dp);

                //触发Bar更新事件 用于重绘
                if (BarUpdatedEvent != null)
                {
                    BarUpdatedEvent();
                }
            }

        }
        void OnRspBarEvent(RspQryBarResponseBin response)
        {
            logger.Info("got bar response reqid:{0} islast:".Put(response.RequestID, response.IsLast));
            //不属于本组件请求的回报直接过滤
            if (!reqIdList.Contains(response.RequestID)) return;
            //将数据存在对应的缓存中
            List<BarImpl> barlist = null;
            if (!barRspMap.TryGetValue(response.RequestID, out barlist))
            {
                barlist = new List<BarImpl>();
                barRspMap.Add(response.RequestID, barlist);
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
                barRspMap.Remove(response.RequestID);
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
            freqmgr.ProcessTick(k);

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

        /// <summary>
        /// 获得StreamCDP
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        TLCommonDataProvider GetStreamCDP(string symbol)
        {
            TLCommonDataProvider stream_cdp = TLMemoryDataManager.htStreaming[symbol] as TLCommonDataProvider;
            if (stream_cdp == null)
            {
                stream_cdp = TLCommonDataProvider.CreateEmptyDataProvider(symbol, intraday);
                stream_cdp.DataCycle = mainCycle;
                TLMemoryDataManager.htStreaming[symbol] = stream_cdp;
            }
            return stream_cdp;
        }


      

        /// <summary>
        /// 将Bar列表转换成我们需要的二维数组
        /// 用于加载到DataProvider中
        /// </summary>
        /// <param name="bars"></param>
        /// <returns></returns>
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
            int reqId = this.dataClient.QryBar(symbol, interval, start, end,10000);
            reqIdList.Add(reqId);
        }

        /// <summary>
        /// 注册合约实时行情
        /// </summary>
        /// <param name="symbol"></param>
        void RegisterSymbol(string symbol)
        {
            this.dataClient.RegisterSymbol(new string[] { symbol });
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
                    commonDataProvider = TLCommonDataProvider.CreateEmptyDataProvider(Code,intraday);
                    commonDataProvider.DataManager = this;
                }
                TLMemoryDataManager.htHistorical[Code] = commonDataProvider;

                //本地加载完毕后 回补数据
                CoverBars(commonDataProvider, mainCycle);

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


        public void RemoveSymbol(string Symbol)
        {
            TLMemoryDataManager.htHistorical.Remove(Symbol);
            TLMemoryDataManager.htStreaming.Remove(Symbol);
        }

    }
}
