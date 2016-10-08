using System;
using System.Collections.Generic;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using System.IO;

namespace TradingLib.DataFarmManager
{

    /// <summary>
    /// read tradelink tick files
    /// </summary>
    public class BarReader : StreamReader
    {
        string _realsymbol = string.Empty;
        string _sym = string.Empty;

        string _path = string.Empty;
        /// <summary>
        /// estimate of ticks contained in file
        /// </summary>
        //public int ApproxTicks = 0;
        /// <summary>
        /// real symbol for data represented in file
        /// </summary>
        public string RealSymbol { get { return _realsymbol; } }
        /// <summary>
        /// security-parsed symbol
        /// </summary>
        public string Symbol { get { return _sym; } }

        /// <summary>
        /// file is readable, has version and real symbol
        /// </summary>
        //public bool isValid { get { return (_filever != 0) && (_realsymbol != string.Empty) && BaseStream.CanRead; } }
        /// <summary>
        /// count of ticks presently read
        /// </summary>
        public int Count = 0;
        public BarReader(string filepath)
            : base(new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            _path = filepath;
            FileInfo fi = new FileInfo(filepath);

        }


        public event Action<BarImpl> GotBar;

        /// <summary>
        /// returns true if more data to process, false otherwise
        /// </summary>
        /// <returns></returns>
        public bool NextTick()
        {
            try
            {
               
                BarImpl bar = new BarImpl();

                
                // get the tick
                string tmp = this.ReadLine();
                if (string.IsNullOrEmpty(tmp)) return false;
                string[] rec = tmp.Split(',');
                bar.EndTime = Util.ToDateTime(long.Parse(rec[0]));
                bar.Open = double.Parse(rec[1]);
                bar.High = double.Parse(rec[2]);
                bar.Low = double.Parse(rec[3]);
                bar.Close = double.Parse(rec[4]);
                bar.OpenInterest = int.Parse(rec[5]);
                bar.Volume = int.Parse(rec[6]);
                bar.TradeCount = int.Parse(rec[7]);


                // send any tick we have
                if (GotBar != null)
                    GotBar(bar);
                // count it
                Count++;
                // assume there is more
                return true;
            }
            catch (EndOfStreamException)
            {
                Close();
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }

    public class BadTikFile : Exception
    {
        public BadTikFile() : base() { }
        public BadTikFile(string message) : base(message) { }
    }

}
