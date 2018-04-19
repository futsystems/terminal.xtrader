using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using TradingLib.MarketData;


namespace XTraderLite
{
    public class WatchList
    {
        Dictionary<string, MDSymbol> watchMap = new Dictionary<string, MDSymbol>();


        List<string> symList = new List<string>();

        public event Action WatchListChanged = delegate { };
        public WatchList()
        {
            
        }

        public void Load()
        {
            //加载
            var fn = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "config", "watch" });
            bool exist = File.Exists(fn);
            if (exist == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(fn), Encoding.UTF8);
                writer.Close();
            }
            string line;
            using (FileStream fs = new FileStream(fn, FileMode.Open))
            {
                using (StreamReader sw = new StreamReader(fs))
                {
                    while ((line = sw.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            MDSymbol tmp = MDService.DataAPI.Symbols.Where(s => s.Symbol == line).FirstOrDefault();
                            if (tmp == null) continue;
                            symList.Add(line);
                        }
                    }
                }
            }

            this.Save();
        }

        public void WatchSymbol(string sym)
        {
            if (!symList.Contains(sym))
            {
                symList.Add(sym);
                this.Save();
                WatchListChanged();
            }
        }

        public void UnWatchSymbol(string sym)
        {
            if (symList.Contains(sym))
            {
                symList.Remove(sym);
                this.Save();
                WatchListChanged();
            }
        }

        public int UpSymbol(string symbol)
        {
            var idx = symList.IndexOf(symbol);
            if(idx>0)
            {
                symList.RemoveAt(idx);
                symList.Insert(idx - 1, symbol);
                this.Save();
                WatchListChanged();
                return idx - 1;
            }
            return idx;
        }

        public int DownSymbol(string symbol)
        {
            var idx = symList.IndexOf(symbol);
            if (idx < symList.Count-1)
            {
                symList.RemoveAt(idx);
                symList.Insert(idx + 1, symbol);
                this.Save();
                WatchListChanged();
                return idx + 1;
            }
            return idx;
        
        }

        public bool IsWatched(string sym)
        {
            return symList.Any(s => s == sym);
        }


        public IEnumerable<string> GetWatchSymbolList()
        {
            return symList;
        }

        public IEnumerable<MDSymbol> GetWatchSymbols()
        {
            List<MDSymbol> list = new List<MDSymbol>();
            foreach (var sym in this.symList)
            {
                MDSymbol tmp = MDService.DataAPI.Symbols.Where(s => s.Symbol == sym).FirstOrDefault();
                if (tmp == null) continue;
                list.Add(tmp);
            }

            return list;
        }

        public void Save()
        {
            var fn = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "config", "watch" });

            using (FileStream fs = new FileStream(fn, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var sym in symList)
                    {
                        sw.WriteLine(sym);
                    }
                }
            }


        }
    }
}
