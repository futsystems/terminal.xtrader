using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace TradingLib.MarketData
{
    /// <summary>
    /// write tradelink tick files
    /// </summary>
    public class MDBarWriter : BinaryWriter
    {
        const string FILE_DOT_EXT = ".data";


        string _symbol = string.Empty;
        string _file = string.Empty;
        string _path = string.Empty;

        public string FolderPath { get { return _path; } set { _path = value; } }
        int _date = 0;
        /// <summary>
        /// real symbol represented by tick file
        /// </summary>
        public string Symbol { get { return _symbol; } }
        /// <summary>
        /// path of this file
        /// </summary>
        public string FilePath { get { return _file; } }
        /// <summary>
        /// date represented by data
        /// </summary>
        public int Date { get { return _date; } }
        /// <summary>
        /// ticks written
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// create tikwriter with specific location, symbol and date.
        /// auto-creates header
        /// </summary>
        /// <param name="path"></param>
        /// <param name="realsymbol"></param>
        /// <param name="date"></param>
        public MDBarWriter(MDSymbol symbol, int date)
        {
            _symbol = symbol.Symbol;
            _path = GetBarPath(symbol.Exchange, symbol.SecCode);
            _date = date;

            // get filename from path and symbol
            _file = SafeFilename(_path, _symbol, date);

            if (File.Exists(_file))
            {
                OutStream = new FileStream(_file, FileMode.Open, FileAccess.Write, FileShare.Read);
                //已经存在的文件 设置当前position为末尾 用于向文件追加数据
                OutStream.Position = OutStream.Length;
            }
            else
            {
                OutStream = new FileStream(_file, FileMode.Create, FileAccess.Write, FileShare.Read);
            }

        }

        /// <summary>
        /// close a tickfile
        /// </summary>
        public override void Close()
        {
            base.Close();
        }

        

        /// <summary>
        /// gets symbol safe to use as filename
        /// </summary>
        /// <param name="realsymbol"></param>
        /// <param name="path"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string SafeFilename(string path, string realsymbol, int date)
        {
            return Path.Combine(new string[] { path, string.Format("{0}-{1}{2}", realsymbol, date, FILE_DOT_EXT) });
        }

        ///// <summary>
        ///// 获得某个合约的所有tick文件名
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="realysymbol"></param>
        ///// <returns></returns>
        //public static IEnumerable<string> GetBarFiles(string path, string symbol)
        //{
        //    string[] files = Directory.GetFiles(path);
        //    return files.Where(fn => { string name = Path.GetFileName(fn); return name.StartsWith(symbol) && name.EndsWith(FILE_DOT_EXT); });
        //}


        ///// <summary>
        ///// 判断某个文件夹下面是否有Tick文件
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="symbol"></param>
        ///// <returns></returns>
        //public static bool HaveAnyTickFiles(string path, string symbol)
        //{
        //    return GetTickFiles(path, symbol).Any();
        //}

        ///// <summary>
        ///// 获得某个合约最新的Tick文件日期
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="symbol"></param>
        ///// <returns></returns>
        //public static int GetEndTickDate(string path, string symbol)
        //{
        //    IEnumerable<string> tickfiles = GetTickFiles(path, symbol);
        //    IEnumerable<int> datelist = tickfiles.Select(fn => GetTickFileDate(fn));

        //    return datelist.Max();

        //}

        //public static int GetStartTickDate(string path, string symbol)
        //{
        //    IEnumerable<string> tickfiles = GetTickFiles(path, symbol);
        //    IEnumerable<int> datelist = tickfiles.Select(fn => GetTickFileDate(fn));

        //    return datelist.Min();
        //}

        //static int GetTickFileDate(string fn)
        //{
        //    string name = Path.GetFileNameWithoutExtension(fn);
        //    string[] rec = name.Split('-');
        //    return int.Parse(rec[1]);
        //}

        /// <summary>
        /// 获得某个合约的Tick文件储存目录
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string GetBarPath(string exchange,string secCode)
        {
            string path = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory,"Data","Bar", exchange, secCode });
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static bool IsFileWritetable(string path)
        {
            FileStream stream = null;

            try
            {
                if (!System.IO.File.Exists(path))
                    return true;
                System.IO.FileInfo file = new FileInfo(path);
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return true;
        }

        /// <summary>
        /// 写入结束标识
        /// </summary>
        public void End()
        {
            Write(Encoding.UTF8.GetBytes("END"));
            Flush();
        }
        /// <summary>
        /// write a tick to file
        /// </summary>
        /// <param name="k"></param>
        //public void NewTick(MDTradeTick k)
        //{
        //    if (!k.IsValid()) return;

        //    this.NewTick(k.DateTimeStamp, k.Last, k.LastSize, k.TotalVol);
        //}
        //public void NewTick(DateTime t, double price, int size, int vol)
        //{
        //    this.NewTick(Utils.ToTLDateTime(t), price, size, vol);
        //}

        public void NewBar(long datetime, double open, double high,double low, double close,int oi,int vol,int tradecount)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(datetime);
            sb.Append(d);
            sb.Append(open);
            sb.Append(d);
            sb.Append(high);
            sb.Append(d);
            sb.Append(low);
            sb.Append(d);
            sb.Append(close);
            sb.Append(d);
            sb.Append(oi);
            sb.Append(d);
            sb.Append(vol);
            sb.Append(d);
            sb.Append(tradecount);
            sb.Append("\n");
            Write(Encoding.UTF8.GetBytes(sb.ToString()));
            // write to disk
            Flush();
            // count it
            Count++;
        }
    }
}
