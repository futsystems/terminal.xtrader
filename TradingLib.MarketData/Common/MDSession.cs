using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{
    public class MDSession
    {
        public MDSession()
        {
            this.Start = 0;
            this.End = 0;
            this.EndInNextDay = false;
        }
        /// <summary>
        /// 开始
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// 结束时间在第二天
        /// </summary>
        public bool EndInNextDay { get; set; }


        
        /// <summary>
        /// 返回区间分钟数
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                if (!this.EndInNextDay)
                {
                    return (FT2FTS(this.End) - FT2FTS(this.Start)) / 60;
                }
                else //结束时间在第二天
                {
                    return 24 * 60 - (FT2FTS(this.Start) - FT2FTS(this.End)) / 60;
                }
            }
        }


        public static void ParseHMS(int time, out int h, out int m, out int s)
        {
            s = time % 100;
            int m1 = (time - s) / 100;
            m = m1 % 100;
            h = m1 / 100;
        }
        static int FT2FTS(int fasttime)
        {
            int s1 = fasttime % 100;
            int m1 = ((fasttime - s1) / 100) % 100;
            int h1 = (int)((fasttime - (m1 * 100) - s1) / 10000);
            return h1 * 3600 + m1 * 60 + s1;
        }

        /// <summary>
        /// 在某个时刻加上多少秒
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="fasttimespaninseconds"></param>
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
            if (h1 >= 24)
            {
                h1 = h1 % 24;
            }
            int sum = h1 * 10000 + m1 * 100 + s1;
            return sum;


        }


        public static MDSession Deserialize(string str)
        {
            MDSession s = new MDSession();
            string[] rec = str.Split('-');
            s.Start = int.Parse(rec[0]);
            if (rec[1].StartsWith("N"))
            {
                s.EndInNextDay = true;
                string nstr = rec[1].Substring(1);
                s.End = int.Parse(nstr);
            }
            else
            {
                s.End = int.Parse(rec[1]);
            }
            return s;
        }

        public static string Serialize(MDSession session)
        {
            return string.Format("{0}-{1}{2}", session.Start, session.EndInNextDay ? "N" : "", session.End);
        }
    }
}
