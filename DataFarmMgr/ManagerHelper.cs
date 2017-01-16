using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public class ManagerHelper
    {
        ///// <summary>
        ///// 解析返回的Json数据到对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static T ParseJsonResponse<T>(string json)
        //{
        //    JsonReply<T> reply = JsonReply.ParseReply<T>(json);
        //    if (reply.Code == 0)
        //    {
        //        return reply.Payload;
        //    }
        //    else
        //    {
        //        return default(T);
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static TradingLib.Mixins.Json.JsonData ToJsonObject(string json)
        //{
        //    return TradingLib.Mixins.Json.JsonMapper.ToObject(json);
        //}


        /// <summary>
        /// 将控件适配到IDataSource用于数据的统一绑定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDataSource AdapterToIDataSource(object obj)
        {
            if (obj is ListBox)
                return new ListBox2IDataSource(obj as ListBox);
            else if (obj is ComboBox)
                return new ComboBox2IDataSource(obj as ComboBox);
            else if (obj is CheckedListBox)
                return new CheckedListBox2IDataSource(obj as CheckedListBox);
            return new Invalid2IDataSource(); ;
        }

        const string ANYSTRING = "所有";

        public static ArrayList GetExchangeCombList(bool isany = true)
        {
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<int> vo1 = new ValueObject<int>();
                vo1.Name = ANYSTRING;
                vo1.Value = 0;
                list.Add(vo1);
            }
            foreach (var ex in DataCoreService.DataClient.Exchanges)
            {
                ValueObject<int> vo = new ValueObject<int>();
                vo.Name = ex.Name;
                vo.Value = ex.ID;
                list.Add(vo);
            }
            return list;
        }

        public static ArrayList GetSecurityCombListViaExchange(int id,bool any = false)
        {
            ArrayList list = new ArrayList();
            if (any)
            {
                ValueObject<int> vo = new ValueObject<int>();
                vo.Name = ANYSTRING;
                vo.Value = 0;
                list.Add(vo);
            }
            //未指定品种类型 则返回所有品种

            if (id == 0)
            {
                foreach (SecurityFamilyImpl sec in DataCoreService.DataClient.Securities)
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = sec.Code + "-" + sec.Name;
                    vo.Value = sec.ID;
                    list.Add(vo);
                }

            }
            else
            {
                foreach (SecurityFamilyImpl sec in DataCoreService.DataClient.Securities.Where(ex => (ex != null && ((ex.Exchange as ExchangeImpl).ID == id))).ToArray())
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = sec.Code + "-" + sec.Name;
                    vo.Value = sec.ID;
                    list.Add(vo);
                }
            }
            
            return list;
        }

        public static ArrayList GetSymbolCombListViaSecurity(int id, bool any = false)
        {
            ArrayList list = new ArrayList();
            if (any)
            {
                ValueObject<int> vo = new ValueObject<int>();
                vo.Name = ANYSTRING;
                vo.Value = 0;
                list.Add(vo);
            }
            //未指定品种类型 则返回所有品种

            if (id == 0)
            {
                foreach (var symbol in DataCoreService.DataClient.Symbols)
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = symbol.Symbol;
                    vo.Value =symbol.ID;
                    list.Add(vo);
                }

            }
            else
            {
                foreach (var  symbol in DataCoreService.DataClient.Symbols.Where(sym=>sym.security_fk == id))
                {
                    ValueObject<Symbol> vo = new ValueObject<Symbol>();
                    vo.Name = symbol.Symbol;
                    vo.Value = symbol;
                    list.Add(vo);
                }
            }

            return list;
        }

        /// <summary>
        /// 获得时区列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetTimeZoneList()
        {

            ArrayList list = new ArrayList();
            ValueObject<string> vo0 = new ValueObject<string>();
            vo0.Name = "系统默认时区";
            vo0.Value = "";
            list.Add(vo0);

            ValueObject<string> vo1 = new ValueObject<string>();
            vo1.Name = "中国标准时间";
            vo1.Value = "Asia/Shanghai";
            list.Add(vo1);


            ValueObject<string> vo10 = new ValueObject<string>();
            vo10.Name = "香港标准时间";
            vo10.Value = "Asia/Hong_Kong";
            list.Add(vo10);


            ValueObject<string> vo2 = new ValueObject<string>();
            vo2.Name = "新加坡标准时间";
            vo2.Value = "Asia/Singapore";
            list.Add(vo2);

            ValueObject<string> vo3 = new ValueObject<string>();
            vo3.Name = "美国中部时间(CT)";
            vo3.Value = "US/Central";
            list.Add(vo3);

            ValueObject<string> vo4 = new ValueObject<string>();
            vo4.Name = "美国东部时间(ET)";
            vo4.Value = "US/Eastern";
            list.Add(vo4);

            return list;
        }

        public static ArrayList GetEnumValueObjects<T>(bool isany = false)
        {
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<T> vo = new ValueObject<T>();
                vo.Name = ANYSTRING;
                vo.Value = (T)(Enum.GetValues(typeof(T)).GetValue(0));
                list.Add(vo);
            }
            foreach (T c in Enum.GetValues(typeof(T)))
            {

                ValueObject<T> vo = new ValueObject<T>();
                vo.Name = Util.GetEnumDescription(c);
                vo.Value = c;
                list.Add(vo);
            }
            return list;
        }

        public static ArrayList GetMarketTimeCombList(bool isany = false)
        {
            ArrayList list = new ArrayList();
            if (isany)
            {
                ValueObject<int> vo1 = new ValueObject<int>();
                vo1.Name = ANYSTRING;
                vo1.Value = 0;
                list.Add(vo1);
            }
            foreach (MarketTimeImpl mt in DataCoreService.DataClient.MarketTimes)
            {
                ValueObject<int> vo = new ValueObject<int>();
                vo.Name = mt.Name;
                vo.Value = mt.ID;
                list.Add(vo);
            }
            return list;
        }


        public static ArrayList GetExpireMonth()
        {
            ArrayList list = new ArrayList();
            DateTime lastday = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
            for (int i = 0; i < 20; i++)
            {
                ValueObject<int> vo = new ValueObject<int>();
                vo.Name = lastday.AddMonths(i).ToString("yyyyMM");//201501
                vo.Value = Convert.ToInt32(vo.Name);
                list.Add(vo);
            }
            return list;
        }

        public static ArrayList GenExpireMonthWithOutYear()
        {
            ArrayList list = new ArrayList();
            for (int i = 1; i <= 12; i++)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = string.Format("{0:00}", i);
                vo.Value = string.Format("{0:00}", i);
                list.Add(vo);
            }
            return list;
        }

        public static ArrayList GetBarFrequency()
        {
            ArrayList list = new ArrayList();
            ValueObject<BarFrequency> vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("1分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 60);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("3分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 180);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("5分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 300);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("15分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 900);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("30分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 1800);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("60分钟");
            vo.Value = new BarFrequency(BarInterval.CustomTime, 3600);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("日");
            vo.Value = new BarFrequency(BarInterval.Day, 1);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("周");
            vo.Value = new BarFrequency(BarInterval.Day, 7);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("季");
            vo.Value = new BarFrequency(BarInterval.Day,90);
            list.Add(vo);
            vo = new ValueObject<BarFrequency>();
            vo.Name = string.Format("年");
            vo.Value = new BarFrequency(BarInterval.Day, 360);
            list.Add(vo);
            return list;
        }

        /// <summary>
        /// 生成月连续合约
        /// [SecCode][Month]
        /// </summary>
        /// <param name="seccode"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GenSymbolMonthContinuous(string seccode, string month)
        {
            return string.Format("{0}{1}", seccode, month);
        }

        public static string GenFutSymbol(SecurityFamilyImpl sec, int month)
        {
            if (sec.Exchange.Country == Country.CN && sec.Exchange.EXCode != "HKEX")//中国交易所 非香港交易所 合约按中国格式生成
            {

                if (sec.Exchange.EXCode.Equals("CZCE"))
                {
                    return sec.Code + month.ToString().Substring(3);
                }
                else
                {
                    return sec.Code + month.ToString().Substring(2);
                }
            }

            //按合约XXV5 的格式进行合约
            string monthstr = month.ToString().Substring(4);
            string yearstr = month.ToString().Substring(3, 1);
            return string.Format("{0}{1}{2}", sec.Code, GetMonthCode(monthstr), yearstr);
        }
        static string GetMonthCode(string month)
        {
            if (month == "01")
            {
                return "F";
            }
            else if (month == "02")
            {
                return "G";
            }
            else if (month == "03")
            {
                return "H";
            }
            else if (month == "04")
            {
                return "J";
            }
            else if (month == "05")
            {
                return "K";
            }
            else if (month == "06")
            {
                return "M";
            }
            else if (month == "07")
            {
                return "N";
            }
            else if (month == "08")
            {
                return "Q";
            }
            else if (month == "09")
            {
                return "U";
            }
            else if (month == "10")
            {
                return "V";
            }
            else if (month == "11")
            {
                return "X";
            }
            else
            {
                return "Z";
            }
        }

        /// <summary>
        /// 201501 获得01作为Month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetMonth(int month)
        {
            return month.ToString().Substring(4);
        }

    }
}
