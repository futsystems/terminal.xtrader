using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace TradingLib.MarketData
{
    public enum DateMatchType
    {
        None = 0,
        Day = 1,
        Month = 2,
        Year = 4,
    }

    public class Utils
    {
        #region TLDate and TLTime
        /// <summary>
        /// Converts date to DateTime (eg 20070926 to "DateTime.Mon = 9, DateTime.Day = 26, DateTime.ShortDate = Sept 29, 2007"
        /// </summary>
        /// <param name="TradeLinkDate"></param>
        /// <returns></returns>
        public static DateTime TLD2DT(int TradeLinkDate)
        {
            if (TradeLinkDate < 10000) throw new Exception("Not a date, or invalid date provided");
            return ToDateTime(TradeLinkDate, 0);
        }
        /// <summary>
        /// Converts  Time to DateTime.  If not using seconds, put a zero.
        /// </summary>
        /// <param name="TradeLinkTime"></param>
        /// <param name="TradeLinkSec"></param>
        /// <returns></returns>
        public static DateTime TLT2DT(int TradeLinkTime)
        {
            return ToDateTime(0, TradeLinkTime);
        }

        public static DateTime ToDateTime(long tldatetime)
        {
            int date = (int)(tldatetime / 1000000);
            int time = (int)(tldatetime - date * 1000000);
            return ToDateTime(date, time);
        }
        /// <summary>
        /// Converts TradeLink Date and Time format to a DateTime. 
        /// eg DateTime ticktime = ToDateTime(tick.date,tick.time);
        /// </summary>
        /// <param name="TradeLinkDate"></param>
        /// <param name="TradeLinkTime"></param>
        /// <param name="TradeLinkSec"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(int TradeLinkDate, int TradeLinkTime)
        {
            int sec = TradeLinkTime % 100;
            int hm = TradeLinkTime % 10000;
            int hour = (int)((TradeLinkTime - hm) / 10000);
            int min = (TradeLinkTime - (hour * 10000)) / 100;
            if (sec > 59) { sec -= 60; min++; }
            if (min > 59) { hour++; min -= 60; }
            int year = 1, day = 1, month = 1;
            if (TradeLinkDate != 0)
            {
                int ym = (TradeLinkDate % 10000);
                year = (int)((TradeLinkDate - ym) / 10000);
                int mm = ym % 100;
                month = (int)((ym - mm) / 100);
                day = mm;
            }
            return new DateTime(year, month, day, hour, min, sec);
        }
        /// <summary>
        /// gets fasttime/tradelink time for now
        /// </summary>
        /// <returns></returns>
        public static int DT2FT() { return DT2FT(DateTime.Now); }
        /// <summary>
        /// converts datetime to fasttime format
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int DT2FT(DateTime d) { return TL2FT(d.Hour, d.Minute, d.Second); }
        /// <summary>
        /// converts tradelink time to fasttime
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int TL2FT(int hour, int min, int sec) { return hour * 10000 + min * 100 + sec; }

        /// <summary>
        /// gets elapsed seconds between two fasttimes
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="latertime"></param>
        /// <returns></returns>
        public static int FTDIFF(int firsttime, int latertime)
        {
            int span1 = FT2FTS(firsttime);
            int span2 = FT2FTS(latertime);
            return span2 - span1;
        }
        /// <summary>
        /// converts fasttime to fasttime span, or elapsed seconds
        /// 获得fasttime对应的秒数
        /// </summary>
        /// <param name="fasttime"></param>
        /// <returns></returns>
        public static int FT2FTS(int fasttime)
        {
            int s1 = fasttime % 100;
            int m1 = ((fasttime - s1) / 100) % 100;
            int h1 = (int)((fasttime - (m1 * 100) - s1) / 10000);
            return h1 * 3600 + m1 * 60 + s1;
        }
        /// <summary>
        /// adds fasttime and fasttimespan (in seconds).  does not rollover 24hr periods.
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="secondtime"></param>
        /// <returns></returns>
        public static int FTADD(int firsttime, int fasttimespaninseconds)
        {
            int s1 = firsttime % 100;
            int m1 = ((firsttime - s1) / 100) % 100;
            int h1 = (int)((firsttime - m1 * 100 - s1) / 10000);
            s1 += fasttimespaninseconds;
            if (s1 >= 60)
            {
                m1 += (int)(s1 / 60);
                s1 = s1 % 60;
            }
            if (m1 >= 60)
            {
                h1 += (int)(m1 / 60);
                m1 = m1 % 60;
            }
            int sum = h1 * 10000 + m1 * 100 + s1;
            return sum;


        }
        /// <summary>
        /// converts fasttime to a datetime
        /// </summary>
        /// <param name="ftime"></param>
        /// <returns></returns>
        public static DateTime FT2DT(int ftime)
        {
            int s = ftime % 100;
            int m = ((ftime - s) / 100) % 100;
            int h = (int)((ftime - m * 100 - s) / 10000);
            return new DateTime(1, 1, 1, h, m, s);
        }

        public static long ToTLDateTime(int tldate, int tltime) { return (long)tldate * 1000000 + (long)tltime; }
        /// <summary>
        /// get long for current date + time
        /// </summary>
        /// <returns></returns>
        public static long ToTLDateTime() { return ((long)ToTLDate() * 1000000) + (long)ToTLTime(); }
        /// <summary>
        /// get long for date + time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToTLDateTime(DateTime dt)
        {
            if (dt == DateTime.MinValue) return long.MinValue;
            if (dt == DateTime.MaxValue) return long.MaxValue;

            return ((long)ToTLDate(dt) * 1000000) + (long)ToTLTime(dt);
        }
        /// <summary>
        /// gets TradeLink date for today
        /// </summary>
        /// <returns></returns>
        public static int ToTLDate() { return ToTLDate(DateTime.Now); }
        /// <summary>
        /// Converts a DateTime to TradeLink Date (eg July 11, 2006 = 20060711)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int ToTLDate(DateTime dt)
        {

            return (dt.Year * 10000) + (dt.Month * 100) + dt.Day;
        }
        /// <summary>
        /// Converts a DateTime.Ticks values to TLDate (eg 8million milliseconds since 1970 ~= 19960101 (new years 1996)
        /// </summary>
        /// <param name="DateTimeTicks"></param>
        /// <returns></returns>
        public static int ToTLDate(long DateTimeTicks)
        {
            return ToTLDate(new DateTime(DateTimeTicks));
        }
        /// <summary>
        /// gets tradelink time for now
        /// </summary>
        /// <returns></returns>
        public static int ToTLTime() { return DT2FT(DateTime.Now); }
        /// <summary>
        /// gets tradelink time from date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToTLTime(DateTime date)
        {
            return DT2FT(date);
        }


        /// <summary>
        /// Converts a TLDate integer format into an array of ints
        /// </summary>
        /// <param name="fulltime">The fulltime.</param>
        /// <returns>int[3] { year, month, day}</returns>
        static int[] TLDateSplit(int fulltime)
        {
            int[] splittime = new int[3]; // year, month, day
            splittime[2] = (int)((double)fulltime / 10000);
            splittime[1] = (int)((double)(fulltime - (splittime[2] * 10000)) / 100);
            double tmp = (int)((double)fulltime / 100);
            double tmp2 = (double)fulltime / 100;
            splittime[0] = (int)(Math.Round(tmp2 - tmp, 2, MidpointRounding.AwayFromZero) * 100);
            return splittime;
        }



        /// <summary>
        /// Tests if two dates are the same, given a mask as DateMatchType.
        /// 
        /// ie, 20070605 will match 20070705 if DateMatchType is Day or Year.
        /// </summary>
        /// <param name="fulldate">The fulldate in TLDate format (int).</param>
        /// <param name="matchdate">The matchdate to test against (int).</param>
        /// <param name="dmt">The "mask" that says what to pay attention to when matching.</param>
        /// <returns></returns>
        public static bool TLDateMatch(int fulldate, int matchdate, DateMatchType dmt)
        {
            const int d = 0, m = 1, y = 2;
            if (dmt == DateMatchType.None)
                return false;
            bool matched = true;
            // if we're requesting a day match,
            if ((dmt & DateMatchType.Day) == DateMatchType.Day)
                matched &= TLDateSplit(fulldate)[d] == TLDateSplit(matchdate)[d];
            if ((dmt & DateMatchType.Month) == DateMatchType.Month)
                matched &= TLDateSplit(fulldate)[m] == TLDateSplit(matchdate)[m];
            if ((dmt & DateMatchType.Year) == DateMatchType.Year)
                matched &= TLDateSplit(fulldate)[y] == TLDateSplit(matchdate)[y];
            return matched;
        }


        #endregion



        /// <summary>
        /// 估算某个时间点到目前有多少个Bar数据 用于请求实时数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="freq">0->5分钟K线    1->15分钟K线    2->30分钟K线  3->1小时K线    4->日K线  5->周K线  6->月K线  7->1分钟    10->季K线  11->年K线</param>
        /// <returns></returns>
        public static int RequestCount(DateTime lasttime, string freq)
        {
            if (freq == ConstFreq.Freq_M1)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute + 1;
            }

            if (freq == ConstFreq.Freq_M5)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 5 + 1;
            }
            if (freq == ConstFreq.Freq_M15)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 15 + 1;
            }
            if (freq == ConstFreq.Freq_M30)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 30 + 1;
            }
            if (freq == ConstFreq.Freq_M60)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 60 + 1;
            }

            if (freq == ConstFreq.Freq_Day)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day + 1;
            }
            if (freq == ConstFreq.Freq_Week)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 7 + 1;
            }
            if (freq == ConstFreq.Freq_Month)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 30 + 1;
            }

            if (freq == ConstFreq.Freq_Quarter)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 90 + 1;
            }
            if (freq == ConstFreq.Freq_Year)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 360 + 1;
            }
            return 800;

        }


        public static string GetCycleName(string freq)
        {
            switch (freq)
            {
                case ConstFreq.Freq_Day: return "日线";
                case ConstFreq.Freq_Week: return "周线";
                case ConstFreq.Freq_Month: return "月线";
                case ConstFreq.Freq_Quarter: return "季线";
                case ConstFreq.Freq_Year: return "年线";
                case ConstFreq.Freq_M1: return "1分";
                case ConstFreq.Freq_M5: return "5分";
                case ConstFreq.Freq_M15: return "15分";
                case ConstFreq.Freq_M30: return "30分";
                case ConstFreq.Freq_M60: return "60分";
                default:
                    return "未知";

            }

        }



        /// <summary>
        /// 加载交易API
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ITraderAPI LoadTraderAPI(string name)
        {
            return LoadAPI<ITraderAPI>("TraderAPI");
        }

        /// <summary>
        /// 加载行情API
        /// </summary>
        public static IMarketDataAPI LoadDataAPI(string name)
        {
            return LoadAPI<IMarketDataAPI>("DataAPI");
        }

        static List<Type> GetImplementors(string path, Type needtype)
        {
            //遍历搜索路径 获得所有dll文件
            List<string> dllfilelist = new List<string>();
            dllfilelist.AddRange(Directory.GetFiles(path, "*.dll"));

            List<Type> types = new List<Type>();
            foreach (string dllfile in dllfilelist)
            {
                try
                {
                    var assembly = Assembly.ReflectionOnlyLoadFrom(dllfile);
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllfile);
                    AssemblyName[] referenced = assembly.GetReferencedAssemblies();
                    foreach (var an in referenced)
                    {
                        try
                        {
                            Assembly.ReflectionOnlyLoad(an.FullName);
                        }
                        catch
                        {
                            Assembly.ReflectionOnlyLoadFrom(Path.Combine(Path.GetDirectoryName(dllfile), an.Name + ".dll"));
                        }
                    }
                    Type[] exportedTypes = assembly.GetExportedTypes();
                    foreach (Type type in exportedTypes)
                    {
                        //程序集中的type不是抽象函数并且其实现了needType接口,则标记为有效
                        if (!type.IsAbstract && type.GetInterface(needtype.FullName) != null)
                        {
                            //Assembly a = Assembly.Load(assemblyName);
                            Assembly a = Assembly.LoadFrom(dllfile);
                            Type[] ts = a.GetTypes();
                            types.Add(a.GetType(type.FullName));
                        }

                    }
                }
                catch (Exception ex)
                {
                   
                }
            }
            return types;
        }


        static T LoadAPI<T>(string dir)
        {
            Type t = typeof(T);
            List<Type> typelist = GetImplementors(dir, t);
            if (typelist.Count > 0)
            {
                //
                T obj = (T)Activator.CreateInstance(typelist[0], new object[] { });
                return obj;
            }
            else
            {
                return default(T);
            }
        }
    }
}
