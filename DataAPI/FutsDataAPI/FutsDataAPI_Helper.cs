using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;

namespace DataAPI.Futs
{
    public partial class FutsDataAPI : IMarketDataAPI
    {
        /// <summary>
        /// 20161018:20161017170000-20161017234500 20161018091500-20161018120000 20161018130000-20161018161500
        /// 将某个交易日的交易时间小节 转换成 分时图需要的小节数据
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        string TradingSessionToMDSession(string session)
        {

            List<string> list = new List<string>();
            string[] rec = session.Split(':');
            if (rec.Length == 2)
            {
                rec = rec[1].Split(' ');
                foreach (var str in rec)
                {
                    string[] date = str.Split('-');
                    DateTime start = Util.ToDateTime(long.Parse(date[0]));
                    DateTime end = Util.ToDateTime(long.Parse(date[1]));

                    list.Add(string.Format("{0}-{1}{2}", start.ToTLTime(), end.ToTLDate() > start.ToTLDate() ? "N" : "", end.ToTLTime()));
                }
            }
            return string.Join(",", list.ToArray());
            
        }
    }
}
