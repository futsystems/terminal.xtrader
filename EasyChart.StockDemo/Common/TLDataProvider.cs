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
    public class TLDataProvider : IDataProvider
    {
        ILog logger = LogManager.GetLogger("TLDataProvider");

        /// <summary>
        /// 日期合并方式
        /// </summary>
        public MergeCycleType dateMergeType = MergeCycleType.OPEN;

        int weekAdjust = 1;

        public Hashtable htData = new Hashtable();//主周期数据

        public Hashtable htRealtime = new Hashtable();//日内数据

        public Hashtable htConstData = new Hashtable();

        public Hashtable htStringData = new Hashtable();

        public Hashtable htAllCycle = new Hashtable();//所有频率数据

        public DataCycle dataCycle = DataCycle.Minute;

        public IDataProvider baseDataProvider;

        public int maxCount = -1;

        public bool adjusted = true;


        public DataCycle DataCycle { get; set; }


        public bool Adjusted
        {
            get
            {
                return this.adjusted;
            }
            set
            {
                this.adjusted = value;
            }
        }


        public double[] this[string name]
        {
            get
	        {
                logger.Info("this[name] called");
		        return this.GetData(name);
	        }
        }

        public int Count
        {
            get
            {
                logger.Info("get count");
                return this["DATE"].Length;
            }
        }

        IDataManager _dataManager = null;
        public IDataManager DataManager {get{return _dataManager;} set{_dataManager = value;}}

        IDataProvider  _baseDataProvider = null;
        public IDataProvider BaseDataProvider{get{return _baseDataProvider;} set{_baseDataProvider = value;}}

        int _maxCount = 10000;
        public int MaxCount {get{return _maxCount;} set{_maxCount = value;}}


        public TLDataProvider(IDataManager dm)
		{
            this._dataManager = dm;
            this.dataCycle = new DataCycle(DataCycleBase.MINUTE, 60);
		}


        public double GetConstData(string DataType)
        {
            logger.Info("Get ConstData:" + DataType);

            return (double)this.htConstData[DataType];
        }


        public string GetStringData(string DataType)
        {
            logger.Info("Get StringData:" + DataType);
            return (string)this.htStringData[DataType.ToUpper()];
        }

        public string GetUnique()
        {
            logger.Info("GetUnique");
            return this.DataCycle.ToString();
        }




        public virtual double[] GetData(string DataType)
        {
            
            //cycleData
            Hashtable cycleData = this.GetCycleData(this.DataCycle);
            if (cycleData == null)
            {
                throw new Exception("Bar Data for cycle:{0} do not exist".Put(this.dataCycle.ToString()));
            }

            double[] array = (double[])cycleData[DataType.ToUpper()];
            if (array == null)
            {
                throw new Exception("The name " + DataType + " does not exist.");
            }

            //如果有BaseDataProvider并且不为自己 则通过BaseDataProvider处理数据 
            if (this.BaseDataProvider != null && this.BaseDataProvider != this)
            {
                array = this.AdjustByBase((double[])cycleData["DATE"], array);
            }

            double[] result;
            //截取数据
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

        public virtual double[] AdjustByBase(double[] Date, double[] dd)
        {
            double[] array = this.BaseDataProvider["DATE"];//获得BaseDataProvider中的Date序列 并且只有当Data中的日期与Base一致时候才复制数据
            double[] array2 = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = double.NaN;
            }
            int num = dd.Length - 1;
            int num2 = array.Length - 1;//至于数据高位
            while (num2 >= 0 && num >= 0)
            {
                if (array[num2] == Date[num])//如果日期相等 则将dd中的数据复制到array2
                {
                    array2[num2--] = dd[num--];
                }
                else if (array[num2] > Date[num])//如果日期不相等则调整对应的序号
                {
                    num2--;
                }
                else
                {
                    num--;
                }
            }
            return array2;
        }



        public virtual Hashtable GetCycleData(DataCycle dc)
        {
            Hashtable result;

            if (dc.CycleBase == DataCycleBase.DAY && dc.Repeat == 1 && !this.Adjusted)
            {
                result = this.htData;
            }
            else
            {
                dc.WeekAdjust = this.weekAdjust;
                //从所有DataCycle中获得数据集
                Hashtable hashtable = (Hashtable)this.htAllCycle[dc.ToString()];
                //没有找到对应的频率数据
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

                    //IEnumerator enumerator = hashtable2.get_Keys().GetEnumerator();
                    //try
                    //{
                    //    while (enumerator.MoveNext())
                    //    {
                    //        string text = (string)enumerator.get_Current();
                    //        hashtable.set_Item(text, new double[num2 + 1]);
                    //    }
                    //}
                    //finally
                    //{
                    //    IDisposable disposable = enumerator as IDisposable;
                    //    if (disposable != null)
                    //    {
                    //        disposable.Dispose();
                    //    }
                    //}
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


        #region 数据加载
        /// <summary>
        /// 将某个double数组加载到某个SeriesName下
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="ds"></param>
        public virtual void LoadBinary(string DataType, double[] ds)
        {
            this.htData[DataType.ToUpper()] = ds;
            this.htAllCycle.Clear();
        }

        /// <summary>
        /// 将Bar数据加载到数据集
        /// </summary>
        /// <param name="bars"></param>
        public virtual void LoadBars(BarImpl[] bars)
        {
            int len = bars.Count();
            double[][] data = new double[6][];
            for (int i = 0; i < 6;i++ )
            {
                data[i]= new double[len];
            }
            for (int j=0;j<len;j++)
            {
                data[0][j] = bars[j].Open;
                data[1][j] = bars[j].High;
                data[2][j] = bars[j].Low;
                data[3][j] = bars[j].Close;
                data[4][j] = bars[j].Volume ;
                data[5][j] = bars[j].BarStartTime.ToOADate();
            }
            this.LoadBinary(data);
        }

        /// <summary>
        /// 加载一个二维数组到当前数据集
        /// </summary>
        /// <param name="ds"></param>
        public virtual void LoadBinary(double[][] ds)
        {
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
            this.htAllCycle.Clear();
        }

        #endregion

        #region 数据合并如 日线数据合并成 周线数据 月线数据等
        protected double Min(double d1, double d2)
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

        protected double Max(double d1, double d2)
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

        protected double Sum(double d1, double d2)
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

        protected double First(double d1, double d2)
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
        public virtual void MergeCycle(double[] ODATE, int[] NEWDATE, double[] CLOSE, double[] ADJCLOSE, double[] ht, double[] htCycle, MergeCycleType mct, bool DoAdjust)
        {
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
