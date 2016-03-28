using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace Easychart.Finance.DataProvider
{
    /// <summary>
    /// 本地文件数据管理
    /// </summary>
    public class TLLocalDataManager:DataManagerBase
    {
        ILog logger = LogManager.GetLogger("TLLocalDataManager");
        private string FilePath;
        const string BARPATH = "Bar";
        const int FIELDS = 9;

        Dictionary<string, DataMaster> masterMap = new Dictionary<string, DataMaster>();
        /// <summary>
        /// 交易所 到合约映射 合约与对应合约数据映射
        /// </summary>
        Dictionary<string, Dictionary<string, SortedDictionary<string, DataMaster>>> exchSymMap = new Dictionary<string, Dictionary<string, SortedDictionary<string, DataMaster>>>();

        public IEnumerable<string> GetExchanges()
        {
            return exchSymMap.Keys;
        }

        public IEnumerable<string> GetSymbols(string exchange)
        {
            Dictionary<string, SortedDictionary<string, DataMaster>> extarget = null;
            if (!this.exchSymMap.TryGetValue(exchange, out extarget))
            {
                return new List<string>();
            }
            return extarget.Keys;
        }

        /// <summary>
        /// 获得所有MasterMap数据
        /// </summary>
        public Dictionary<string, Dictionary<string, SortedDictionary<string, DataMaster>>> MasterMap
        {
            get { return exchSymMap; }
        }


        bool _intraday = false;
        DataCycle _mainCycle = DataCycle.Minute;
        public TLLocalDataManager(bool intraday)
		{
            _intraday = intraday;
            _mainCycle = intraday ? DataCycle.Minute : DataCycle.Day;

            FilePath = Path.Combine(new string[] {AppDomain.CurrentDomain.BaseDirectory,"Data"});

            string filename = GetMasterFile();

            if (File.Exists(filename))
            {
                this.LoadMaster();
            }
            else
            {
                DataMaster m = new DataMaster();
                m.Exchange = "CFFEX";
                m.Symbol = "IF1604";
                m.Name = "股指123";
                m.SymbolType = QSEnumSymbolType.MonthContinuous;
                m.StartTime = DateTime.Now.ToTLDateTime();
                m.EndTime = DateTime.Now.ToTLDateTime();
                m.ModifiedTime = DateTime.Now.ToTLDateTime();
                m.IntervalType = BarInterval.CustomTime;
                m.Interval = 60;
                m.FN = 101;
                this.masterMap.Add(m.Key, m);
                this.SaveMaster();
            }
            string barpath = Path.Combine(new string[] { this.FilePath, BARPATH });
            if (!Directory.Exists(barpath))
            {
                Directory.CreateDirectory(barpath);
            }

            string fn = this.LookupDataFile("CFFEX", "IF1604", 60, BarInterval.CustomTime, null);
		}

        

        private int MaxNum
        {
            get
            {
                int num = -1;
                foreach (var m in this.masterMap.Values)
                {
                    num = num >= m.FN ? num : m.FN;
                }
                return num;
            }
        }


        public DataMaster FindMaster(string exchange, string symbol, int interval, BarInterval type)
        { 
            string key = "{0}-{1}-{2}-{3}".Put(exchange,symbol,interval,type);
            DataMaster target = null;
            if (masterMap.TryGetValue(key, out target))
            {
                return target;
            }
            return null;
        }
        /// <summary>
        /// 获得某个交易所 某个合约 某个频率的数据文件
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="cdp"></param>
        /// <param name="Fields"></param>
        /// <param name="TimeFrame"></param>
        /// <returns></returns>
        public string LookupDataFile(string exchange,string symbol,int interval,BarInterval type, CommonDataProvider cdp)
        {
            string key = "{0}-{1}-{2}-{3}".Put(exchange, symbol, interval, type);
            if (cdp != null)
            {
                cdp.SetStringData("Code", symbol);
            }
            DataMaster target = null;
            if (masterMap.TryGetValue(key, out target))
            {
                if (cdp != null)
                {
                    cdp.SetStringData("Name", target.Name);
                }
                return Path.Combine(new string[] { this.FilePath, BARPATH, "F{0}.Dat".Put(target.FN) });
            }
            return string.Empty;
        }



        /// <summary>
        /// 获得某个合约的数据
        /// 日期 时间 高 开 低 收 成交量 持仓
        /// 从文件加载Bar数据
        /// DataProvider提供接口用于获得Bar数据,同时指定DataCycle用于获得不同频率的数据
        /// 加载时 同时加载分钟数据和日线数据
        /// 分钟数据 可以合并成3分钟5分钟等频率的数据
        /// 日线数据 可以合并成周线,月线等数据
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public override IDataProvider GetData(string Code, int Count)
        {
            logger.Info("Qry symbol:{0} bar data, count:{1}".Put(Code, Count));

            TLCommonDataProvider commonDataProvider = new TLCommonDataProvider(this,Code, _intraday);

            string filename = this.LookupDataFile("CFFEX",Code,_mainCycle.ToSeconds(),BarInterval.CustomTime, commonDataProvider);

            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                using (FileStream fileStream = this.ReadData(filename))
                {
                    byte[] array = new byte[FIELDS * 4];
                    byte[] array2 = new byte[fileStream.Length - (long)array.Length];

                    //将数据array
                    fileStream.Read(array, 0, array.Length);
                    fileStream.Read(array2, 0, array2.Length);

                    //数据以float储存 数据长度/4 则为所有数据个数
                    float[] data = new float[array2.Length / 4];
                    Buffer.BlockCopy(array2, 0, data, 0, array2.Length);
                    //将arrys字节数据转换成
                    this.fmsbin2ieee(data);

                    //数据个数/字段个数 为所有bar个数
                    int num = data.Length / FIELDS;

                    double[] date = new double[num];//date 4
                    double[] open = new double[num];//open 5
                    double[] high = new double[num];//high 6
                    double[] low = new double[num];//low 7
                    double[] close = new double[num];//close 8
                    double[] vol = new double[num];//vol 9
                    double[] adjust = new double[num];//adjust 10
                    double[] oi = new double[num]; //11


                    for (int i = 0; i < num; i++)
                    {
                        int idate = (int)data[i * FIELDS];
                        DateTime dateTime = new DateTime(idate / 10000 + 1900, idate / 100 % 100, idate % 100);

                        int itime = (int)data[i * FIELDS + 1];
                        dateTime += new TimeSpan(itime / 10000, itime / 100 % 100, itime % 100);

                        //设置时间
                        date[i] = dateTime.ToOADate();

                        open[i] = (double)data[i * FIELDS + 2];
                        high[i] = (double)data[i * FIELDS + 3];
                        low[i] = (double)data[i * FIELDS + 4];
                        close[i] = (double)data[i * FIELDS + 5];
                        vol[i] = (double)data[i * FIELDS + 6];
                        adjust[i] = (double)data[i * FIELDS + 7];
                        oi[i] = (double)data[i * FIELDS + 8];
                    }

                    //将本地数据加载到dataprovider
                    commonDataProvider.LoadBinary(new double[][]
					{
						open,//open
						high,//high
						low,//low
						close,//close
						vol,//vol
						date,//date
						close//adjust
					});

                    //double[] tmp = commonDataProvider.GetData("Date");
                    return commonDataProvider;
                }
            }

            var cdp = TLCommonDataProvider.CreateEmptyDataProvider(Code,_intraday);
            cdp.DataManager = this;
            return cdp;

        }

        /// <summary>
        /// 获得某个合约 某个频率类别的数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IDataProvider GetData(Symbol symbol, BarInterval type, int interval,DateTime start,DateTime end,int maxcount)
        {

            return null;
        }

        /// <summary>
        /// 储存Bar数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="idp"></param>
        /// <param name="OutStream"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="Intraday"></param>
        public override void SaveData(string symbol, IDataProvider idp, Stream OutStream, DateTime Start, DateTime End, bool Intraday)
        {
            TLCommonDataProvider commonDataProvider = (TLCommonDataProvider)idp;
            if (!string.IsNullOrEmpty(symbol))
            {
                int num = this.MaxNum + 1;
                int count = commonDataProvider.Count;
                DataMaster master = this.FindMaster("CFFEX",symbol,60,BarInterval.CustomTime);

                //如果master不存在则创建
                if (master == null)
                {
                    master = new DataMaster();
                    master.Symbol = symbol;
                    master.Exchange = "CFFEX";
                    master.Name = "Name";
                    master.IntervalType = BarInterval.CustomTime;
                    master.Interval = 60;

                    this.masterMap.Add(master.Key, master);
                }

                double[] date = commonDataProvider["DATE"];
                double[] open = commonDataProvider["OPEN"];
                double[] high = commonDataProvider["HIGH"];
                double[] low = commonDataProvider["LOW"];
                double[] close = commonDataProvider["CLOSE"];
                double[] volume = commonDataProvider["VOLUME"];
                double[] adjust = commonDataProvider["ADJCLOSE"];
                double[] oi = close;

                float[] data = new float[count * FIELDS];
                for (int i = 0; i < count; i++)
                {
                    DateTime dateTime = DateTime.FromOADate(date[i]);
                    data[i * FIELDS + 0] = (float)((dateTime.Year - 1900) * 10000 + dateTime.Month * 100 + dateTime.Day);
                    data[i * FIELDS + 1] = (float)(dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second);

                    data[i * FIELDS + 2] = (float)open[i];
                    data[i * FIELDS + 3] = (float)high[i];
                    data[i * FIELDS + 4] = (float)low[i];
                    data[i * FIELDS + 5] = (float)close[i];
                    data[i * FIELDS + 6] = (float)volume[i];
                    data[i * FIELDS + 7] = (float)adjust[i];
                    data[i * FIELDS + 8] = (float)oi[i];
                }

                this.fieee2msbin(data);

                byte[] target = new byte[data.Length * 4];
                Buffer.BlockCopy(data, 0, target, 0, target.Length);

                string filename = this.LookupDataFile("CFFEX", symbol, 60, BarInterval.CustomTime, commonDataProvider);
                using (FileStream fileStream = File.Create(filename))
                {
                    fileStream.Write(target, 0, target.Length);
                }

                master.ModifiedTime = DateTime.Now.ToTLDateTime();
                //记录DataProvider的起止时间 这里需要判断第一次创建记录开始时间 后面就记录结束时间
                master.EndTime = DateTime.FromOADate(date[count - 1]).ToTLDateTime();
                master.StartTime = DateTime.FromOADate(date[0]).ToTLDateTime();
                //储存Master
                this.SaveMaster();
                
               
            }
        }



        #region 
        string GetMasterFile()
        {
            return Path.Combine(new string[] { this.FilePath, "MASTER" });
        }
        /// <summary>
        /// 加载数据索引文件
        /// </summary>
        private void LoadMaster()
        {
            string fn = GetMasterFile();
            using (FileStream fileStream = this.ReadData(fn))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    do
                    {
                        DataMaster m = DataMaster.Read(binaryReader);
                        this.AddMaster(m);
                        
                    }
                    while (binaryReader.BaseStream.Position<fileStream.Length);
                }
            }
        }

        /// <summary>
        /// 添加Master到数据结构
        /// </summary>
        /// <param name="m"></param>
        void AddMaster(DataMaster m)
        {
            this.masterMap.Add(m.Key, m);

            Dictionary<string, SortedDictionary<string, DataMaster>> extarget = null;
            if (!this.exchSymMap.TryGetValue(m.Exchange, out extarget))
            {
                extarget = new Dictionary<string, SortedDictionary<string, DataMaster>>();
                this.exchSymMap.Add(m.Exchange, extarget);
            }

            SortedDictionary<string, DataMaster> symtarget = null;
            if (!extarget.TryGetValue(m.Symbol, out symtarget))
            {
                symtarget = new SortedDictionary<string, DataMaster>();
                extarget.Add(m.Symbol, symtarget);
            }

            if (!symtarget.Keys.Contains(m.Key))
            {
                symtarget.Add(m.Key, m);
            }
        }

        /// <summary>
        /// 保存数据索引文件
        /// </summary>
        private void SaveMaster()
        {
            string fn = GetMasterFile();
            using (FileStream fileStream = File.Create(fn))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    foreach(var m in this.masterMap.Values)
                    {
                        DataMaster.Write(binaryWriter,m);
                    }
                }
            }
        }


        /// <summary>
        /// 读取某个文件获得FileStream
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        private FileStream ReadData(string Filename)
        {
            for (int i = 5; i >= 0; i--)
            {
                try
                {
                    return new FileStream(Filename,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                }
                catch
                {
                    if (i == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(100);
                }
            }
            return null;
        }

        private void fmsbin2ieee(float[] ff)
        {
            uint[] array = new uint[ff.Length];
            Buffer.BlockCopy(ff, 0, array, 0, ff.Length * 4);
            for (int i = 0; i < ff.Length; i++)
            {
                if (array[i] != 0u)
                {
                    uint num = array[i] >> 16;
                    uint num2 = (num & 65280u) - 512u;
                    num = ((num & 127u) | (num << 8 & 32768u));
                    num |= num2 >> 1;
                    array[i] = ((array[i] & 65535u) | num << 16);
                }
            }
            Buffer.BlockCopy(array, 0, ff, 0, ff.Length * 4);
        }

        private void fieee2msbin(float[] ff)
        {
            uint[] array = new uint[ff.Length];
            Buffer.BlockCopy(ff, 0, array, 0, ff.Length * 4);
            for (int i = 0; i < ff.Length; i++)
            {
                if (array[i] != 0u)
                {
                    uint num = array[i] >> 16;
                    uint num2 = (num << 1 & 65280u) + 512u;
                    if ((num2 & 32768u) == (num << 1 & 32768u))
                    {
                        num = ((num & 127u) | (num >> 8 & 128u));
                        num |= num2;
                        array[i] = ((array[i] & 65535u) | num << 16);
                    }
                }
            }
            Buffer.BlockCopy(array, 0, ff, 0, ff.Length * 4);
        }
        #endregion
    }
}
