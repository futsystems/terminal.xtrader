using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.DataFarmManager
{
    public class ManagerHelper
    {
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
            foreach (var ex in CoreService.MDClient.Exchanges)
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
                foreach (SecurityFamilyImpl sec in CoreService.MDClient.Securities)
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = sec.Code + "-" + sec.Name;
                    vo.Value = sec.ID;
                    list.Add(vo);
                }

            }
            else
            {
                foreach (SecurityFamilyImpl sec in CoreService.MDClient.Securities.Where(ex => (ex != null && ((ex.Exchange as Exchange).ID == id))).ToArray())
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = sec.Code + "-" + sec.Name;
                    vo.Value = sec.ID;
                    list.Add(vo);
                }
            }
            
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

        public static ArrayList GetExpireMonth()
        {
            ArrayList list = new ArrayList();
            DateTime lastday = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
            for (int i = 0; i < 12; i++)
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

    }
}
