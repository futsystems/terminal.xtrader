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
    public class TLCommonDataProvider:CommonDataProvider
    {
        ILog logger = LogManager.GetLogger("TLCommonDataProvider");

        bool _intraday = false;
        DataCycle _mainCycle = DataCycle.Minute;

        string _symbol = string.Empty;
        public string Symbol { get { return _symbol; } }

        public TLCommonDataProvider(IDataManager dm,string symbol,bool intraday)
            : base(dm)
        {
            _symbol = symbol;
            _intraday = intraday;
            _mainCycle = intraday ? DataCycle.Minute : DataCycle.Day;
            //this.FutureBars = 50;
        }

        public static TLCommonDataProvider CreateEmptyDataProvider(string symbol, bool intraday)
        {
            TLCommonDataProvider cdp = new TLCommonDataProvider(null,symbol,intraday);
            cdp.DataCycle = intraday ? DataCycle.Minute : DataCycle.Day;
            cdp.LoadByteBinary(new byte[0]);
            return cdp;
        }

        /// <summary>
        /// 处理行情数据
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool GotTick(Tick k)
        {
            if (k.Symbol != k.Symbol) return false;
            
            return true;
        }
        //public new DataCycle DataCycle
        //{
        //    get
        //    {
        //        return this.dataCycle;
        //    }
        //    set
        //    {

        //        logger.Info("set dataprovider's datacycle");

        //        this.dataCycle = value;
        //    }
        //}


        /// <summary>
        /// 通过this[string Name] 主入口获得数据
        /// 如果该函数没有重写 则会调用父类的GetData(string DataType)
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public override double[] this[string Name]
        {
            get
            {
                //logger.Info("this[string name] is called,datacycle:{0}".Put(this.dataCycle));
                return this.GetData(Name);
            }
        }
        public override double[] GetData(string DataType)
        {
            //lock (this)
            {
                //logger.Info("try to get data series:" + DataType);
                //1.获得对应的频率数据
                Hashtable cycleData = this.GetCycleData(this.DataCycle);
                if (cycleData == null)
                {
                    throw new Exception(string.Concat(new object[]
		        {
			        "Quote data????? ",
			        DataType,
			        " ",
			        this.DataCycle,
			        " not found"
		        }));
                }

                //2.从频率数据中获得数据集
                double[] array = (double[])cycleData[DataType.ToUpper()];
                if (array == null)
                {
                    throw new Exception("The name " + DataType + " does not exist.");
                }

                //2.通过BaseDataProvider进行数据修正
                if (this.BaseDataProvider != null && this.BaseDataProvider != this)
                {
                    array = this.AdjustByBase((double[])cycleData["DATE"], array);
                }

                //数据截取
                double[] result;
                if (this.MaxCount == -1 || array.Length <= this.MaxCount)
                {
                    result = array;
                }
                else
                {
                    double[] array2 = new double[this.MaxCount];
                    Array.Copy(array, array.Length - this.MaxCount, array2, 0, this.MaxCount);
                    result = array2;
                }
                return result;
            }
        }


        /// <summary>
        /// 获得某个频率数据
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public override Hashtable GetCycleData(DataCycle dc)
        {
            //logger.Info("Get Cycle Data:" + dc.ToString());
            Hashtable result;

            //如果与主频率数据相等则返回htData
            if (_mainCycle.ToSeconds() == dc.ToSeconds())
            {
                result = this.htData;
            }
            //其余数据进行合并结算后返回
            //如果是日线 则直接返回
            //if (dc.CycleBase == DataCycleBase.DAY && dc.Repeat == 1)// && !this.Adjusted)
            //{
            //    result = this.htData;
            //}
            else
            {
                dc.WeekAdjust = this.weekAdjust;
                //从所有DataCycle中获得数据集
                Hashtable hashtable = (Hashtable)this.htAllCycle[dc.ToString()];
                //没有找到对应的频率数据 则生成该频率数据 否则直接返回获得的数据
                if (hashtable == null)
                {
                    //日线数据为空 则返回空
                    if (this.htData == null) return null;

                    //处理日线数据
                    Hashtable hashtable2 = this.htData;
                    //if (this.intradayInfo != null)
                    //{
                    //    hashtable2 = this.DoExpandMinute(hashtable2);
                    //}
                    //if (this.futureBars != 0)
                    //{
                    //    hashtable2 = this.ExpandFutureBars(hashtable2);
                    //}


                    //收盘价数据不为空
                    if (hashtable2["CLOSE"] != null)
                    {
                        //判定Open High Low 等数据 如果为空则用Close填充
                        if (hashtable2["OPEN"] == null)
                        {
                            hashtable2["OPEN"] = hashtable2["CLOSE"];
                        }
                        if (hashtable2["HIGH"] == null)
                        {
                            hashtable2["HIGH"] = hashtable2["CLOSE"];
                        }
                        if (hashtable2["LOW"] == null)
                        {
                            hashtable2["LOW"] = hashtable2["CLOSE"];
                        }
                    }

                    //获得日期数据
                    double[] array = (double[])hashtable2["DATE"];
                    //如果日期数据为空 则返回空
                    if (array == null) return null;

                    //按当前DataCycle获得对应的序号
                    int[] array2 = new int[array.Length];
                    int num = int.MinValue;
                    int num2 = -1;
                    for (int i = 0; i < array.Length; i++)
                    {
                        int num3;
                        if (this.DataCycle.CycleBase == DataCycleBase.TICK)
                        {
                            num3 = i;
                        }
                        else
                        {
                            num3 = this.DataCycle.GetSequence(array[i]);//获得对应序号
                        }
                        if (num3 > num)
                        {
                            num2++;
                        }
                        array2[i] = num2;
                        num = num3;
                    }

                    hashtable = new Hashtable();
                    //生成对应大小的double[]
                    foreach (var key in hashtable2.Keys)
                    {
                        hashtable[key] = new double[num2 + 1];
                    }

                    //合并数据集
                    bool flag = this.Adjusted && hashtable2["ADJCLOSE"] != null && hashtable2["CLOSE"] != null;
                    double[] cLOSE = (double[])hashtable2["CLOSE"];
                    double[] aDJCLOSE = (double[])hashtable2["ADJCLOSE"];

                    foreach (string key in hashtable2.Keys)
                    {
                        bool doAdjust = false;
                        MergeCycleType mct;
                        if (key == "DATE")
                        {
                            mct = this.dateMergeType;
                        }
                        else if (key == "VOLUME" || key == "AMOUNT")
                        {
                            mct = MergeCycleType.SUM;
                        }
                        else
                        {
                            try
                            {
                                mct = (MergeCycleType)Enum.Parse(typeof(MergeCycleType), key);
                                doAdjust = true;
                            }
                            catch
                            {
                                mct = MergeCycleType.CLOSE;
                            }
                        }
                        this.MergeCycle(array, array2, cLOSE, aDJCLOSE, (double[])hashtable2[key], (double[])hashtable[key], mct, doAdjust);
                    }
                    this.htAllCycle[dc.ToString()] = hashtable;//将合并后生成的数据存放在allCycle中
                }
                result = hashtable;
            }
            return result;
        }



        /// <summary>
        /// 加载一个二维数组
        /// 可以实现分别加载分钟数据和日线数据 然后在GetCycleData中针对不同的频率来获得对应的数据 需要合并生成的进行合并
        /// </summary>
        /// <param name="ds"></param>
        public override void LoadBinary(double[][] ds)
        {
            //lock (this)
            {
                logger.Info("************load binary data");
                if (ds.Length > 4)
                {
                    this.htData.Clear();
                    this.htData.Add("OPEN", ds[0]);
                    this.htData.Add("HIGH", ds[1]);
                    this.htData.Add("LOW", ds[2]);
                    this.htData.Add("CLOSE", ds[3]);
                    this.htData.Add("VOLUME", ds[4]);
                    this.htData.Add("DATE", ds[5]);
                    if (ds.Length > 6)
                    {
                        this.htData.Add("ADJCLOSE", ds[6]);
                    }
                    else
                    {
                        double[] array = new double[ds[0].Length];
                        Buffer.BlockCopy(ds[3], 0, array, 0, ds[0].Length * 8);
                        this.htData.Add("ADJCLOSE", array);
                    }
                }

                var date = (double[])this.htData["DATE"];
                if (date.Length > 0)
                {
                    _latestBarTime = DateTime.FromOADate(date[date.Length - 1]);//获得最新的Bar时间
                }
                this.htAllCycle.Clear();
            }
        }


        //public DateTime StartTime
        //{
        //    get 
        //    {
        //        double[] date = this["date"];
        //        DateTime dt =  DateTime.FromOADate(date[0]);
        //        return dt;
        //    }
        //}

        //public DateTime EndTime
        //{
        //    get 
        //    {
        //        double[] date = this["date"];
                
        //        DateTime dt=  DateTime.FromOADate(date[this.Count-1]);
        //        return dt;

        //    }
        //}


        #region 数据合并如 日线数据合并成 周线数据 月线数据等
        protected new double Min(double d1, double d2)
        {
            double result;
            if (double.IsNaN(d1))
            {
                result = d2;
            }
            else
            {
                result = Math.Min(d1, d2);
            }
            return result;
        }

        protected new double Max(double d1, double d2)
        {
            double result;
            if (double.IsNaN(d1))
            {
                result = d2;
            }
            else
            {
                result = Math.Max(d1, d2);
            }
            return result;
        }

        protected new double Sum(double d1, double d2)
        {
            double result;
            if (double.IsNaN(d1))
            {
                result = d2;
            }
            else
            {
                result = d1 + d2;
            }
            return result;
        }

        protected new double First(double d1, double d2)
        {
            double result;
            if (double.IsNaN(d1))
            {
                result = d2;
            }
            else
            {
                result = d1;
            }
            return result;
        }

        TimeSpan spanMin1 = TimeSpan.FromSeconds(60);
        TimeSpan spanDay = TimeSpan.FromDays(1);

        DateTime _latestBarTime = DateTime.MinValue;
        /// <summary>
        /// 
        /// </summary>
        public DateTime LatestBarTime
        {
            get
            {
                return _latestBarTime;
            }
        }
        public void Merge(Tick k,out bool update,bool neecheck = true)
        {
            //lock (this)
            {
                update = false;
                //主数据无记录直接返回
                if (this.htData.Keys.Count == 0) return;
                double[] date = (double[])this.htData["DATE"];
                //if (date.Length == 0) return;

                if (neecheck)
                {
                    //判定最后一个Bar的数据时间和当前行情时间是否在同一个周期内 不在同一个周期则添加一个新Bar
                    bool same = DataCycle.Minute.SameSequence(k.DateTime(), _latestBarTime);
                    if (!same)
                    {
                        DateTime round = TimeFrequency.RoundTime(k.DateTime(), _intraday ? spanMin1 : spanDay);
                        double val = (double)k.Trade;
                        DataPacket dp = new DataPacket(k.Symbol, round.ToOADate(), val, val, val, val, 0, val);//先以当前成交价格生成新的Bar然后再MergeTick 处理价格和成交量，如果这里有成交量会造成第一次Tick 成交量累加2次
                        this.Merge(dp);
                        this.Merge(k, out update, false);
                        return;
                    }
                }

                var high = (double[])this.htData["HIGH"];
                bool hithigh = (double)k.Trade > high[high.Length - 1];
                if (hithigh)
                {
                    update = true;
                    high[high.Length - 1] = (double)k.Trade;
                }

                var low = (double[])this.htData["LOW"];
                bool hitlow = (double)k.Trade < low[low.Length - 1];
                if (hitlow)
                {
                    update = true;
                    low[low.Length - 1] = (double)k.Trade;
                }
                var close = (double[])this.htData["CLOSE"];
                bool hitclose = (double)k.Trade != close[low.Length - 1];
                if (hitclose)
                {
                    update = true;
                    close[close.Length - 1] = (double)k.Trade;
                }
                var volume = (double[])this.htData["VOLUME"];
                volume[volume.Length - 1] += k.Size;
            }

        }


        /// <summary>
        /// 数据合并
        /// 将两个DataProvider进行合;
        /// </summary>
        /// <param name="cdp"></param>
        public override void Merge(CommonDataProvider cdp)
        {
            //lock (this)
            {
                //如果主频率不相同 则无法合并
                if (cdp.DataCycle.ToSeconds() != _mainCycle.ToSeconds())
                {
                    throw new Exception("not same datacycle,can not merge");
                }
                ArrayList[] source = new ArrayList[CommonDataProvider.Keys.Length];
                ArrayList[] pending = new ArrayList[CommonDataProvider.Keys.Length];

                //将本地数据和待合并数据复制到数组内
                for (int i = 0; i < source.Length; i++)
                {
                    source[i] = new ArrayList();
                    source[i].AddRange((double[])this.htData[CommonDataProvider.Keys[i]]);
                    pending[i] = new ArrayList();
                    pending[i].AddRange((double[])cdp.htData[CommonDataProvider.Keys[i]]);
                }
                int num = 0;
                int j = 0;
                //遍历所有待合并的数据
                while (j < pending[0].Count)
                {
                    //如果遍历到原数据最后一项 则原数据插入待合并数据
                    if (num >= source[0].Count)
                    {
                        for (int k = j; k < pending[0].Count; k++)
                        {
                            for (int l = 0; l < CommonDataProvider.Keys.Length; l++)
                            {
                                source[l].Add(pending[l][k]);
                            }
                        }
                        break;
                    }
                    //如果原始数组日期小于 待合并日期则 计数器递增
                    if ((double)source[0][num] < (double)pending[0][j])
                    {
                        num++;
                    }
                    //原来的位置的日期>=当前待合并数据日期
                    else if ((double)source[0][num] >= (double)pending[0][j])
                    {
                        if ((double)source[0][num] > (double)pending[0][j])//日期大于 则将待合并数据插入
                        {
                            for (int m = 0; m < CommonDataProvider.Keys.Length; m++)
                            {
                                source[m].Insert(num, pending[m][j]);
                            }
                        }
                        else //日期等于 则设置对应的数据
                        {
                            for (int n = 1; n < CommonDataProvider.Keys.Length; n++)
                            {
                                source[n][num] = pending[n][j];
                            }
                        }
                        num++;
                        j++;
                    }
                }
                //清空主数据集 并重新添加对应的数据
                this.htData.Clear();
                for (int num2 = 0; num2 < CommonDataProvider.Keys.Length; num2++)
                {
                    this.htData.Add(CommonDataProvider.Keys[num2], (double[])source[num2].ToArray(typeof(double)));
                }
                var date = (double[])this.htData["DATE"];
                _latestBarTime = DateTime.FromOADate(date[date.Length - 1]);//获得最新的Bar时间
                this.htAllCycle.Clear();
            }
        }

        public override void Merge(DataPacket dp)
        {
            //lock (this)
            {
                if (dp != null && !dp.IsZeroValue)
                {
                    ArrayList[] array = new ArrayList[CommonDataProvider.Keys.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ArrayList();
                        array[i].AddRange((double[])this.htData[CommonDataProvider.Keys[i]]);
                    }
                    for (int j = 0; j <= array[0].Count; j++)
                    {
                        if (j >= array[0].Count)
                        {
                            for (int k = 0; k < CommonDataProvider.Keys.Length; k++)
                            {
                                array[k].Add(dp[CommonDataProvider.Keys[k]]);
                            }
                            break;
                        }
                        if ((double)array[0][j] >= dp.DoubleDate)
                        {
                            if ((double)array[0][j] > dp.DoubleDate)
                            {
                                for (int l = 0; l < CommonDataProvider.Keys.Length; l++)
                                {
                                    array[l].Insert(j, dp[CommonDataProvider.Keys[l]]);
                                }
                            }
                            else
                            {
                                for (int m = 1; m < CommonDataProvider.Keys.Length; m++)
                                {
                                    array[m][j] = dp[CommonDataProvider.Keys[m]];
                                }
                            }
                            break;
                        }
                    }
                    this.htData.Clear();
                    for (int n = 0; n < CommonDataProvider.Keys.Length; n++)
                    {
                        this.htData.Add(CommonDataProvider.Keys[n], (double[])array[n].ToArray(typeof(double)));
                    }
                    var date = (double[])this.htData["DATE"];
                    _latestBarTime = DateTime.FromOADate(date[date.Length - 1]);//获得最新的Bar时间
                    this.htAllCycle.Clear();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ODATE">原始时间</param>
        /// <param name="NEWDATE"></param>
        /// <param name="CLOSE">收盘价</param>
        /// <param name="ADJCLOSE">调整收盘价</param>
        /// <param name="ht">原始频率数据</param>
        /// <param name="htCycle">目标频率数据</param>
        /// <param name="mct">合并类别</param>
        /// <param name="DoAdjust"></param>
        public override void MergeCycle(double[] ODATE, int[] NEWDATE, double[] CLOSE, double[] ADJCLOSE, double[] ht, double[] htCycle, MergeCycleType mct, bool DoAdjust)
        {
            //logger.Info("MergeCycle is called");
            int num = -1;
            int num2 = -1;
            for (int i = 0; i < ODATE.Length; i++)
            {
                double ratio = 1.0;
                //判定是否需要调整 如果调整则按调整价/收盘价获得调整系数
                if (DoAdjust && ADJCLOSE != null)
                {
                    ratio = ADJCLOSE[i] / CLOSE[i];
                }
                double value = ht[i] * ratio;
                if (ratio != 1.0)
                {
                    value = Math.Round(value, 2);
                }

                if (num != NEWDATE[i])//目标频率对应的计数位置,在该位置需要生成数据
                {
                    num2++;
                    htCycle[num2] = value;
                }
                else if (!double.IsNaN(value))
                {
                    if (mct == MergeCycleType.HIGH)
                    {
                        htCycle[num2] = this.Max(htCycle[num2], value);
                    }
                    else if (mct == MergeCycleType.LOW)
                    {
                        htCycle[num2] = this.Min(htCycle[num2], value);
                    }
                    else if (mct == MergeCycleType.CLOSE)
                    {
                        htCycle[num2] = value;
                    }
                    else if (mct == MergeCycleType.ADJCLOSE)
                    {
                        htCycle[num2] = ht[i];
                    }
                    else if (mct == MergeCycleType.OPEN)
                    {
                        htCycle[num2] = this.First(htCycle[num2], value);
                    }
                    else
                    {
                        htCycle[num2] = this.Sum(htCycle[num2], value);
                    }
                }
                num = NEWDATE[i];
            }
        }
        #endregion


    }
}
