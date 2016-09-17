using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;

namespace CStock
{
    public partial class TGongSi
    {
        #region 数据添加

        /// <summary>
        /// 添加某个数据集
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public TBian Add(string name, double[] f1, int len)
        {
            return this.BInsert(name, f1, len);
        }

        /// <summary>
        /// 数据集前面插入
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public TBian FInsert(string name, double[] f1, int len)
        {
            TBian B1 = null;
            name = name.ToLower();
            B1 = GetBian(name);
            if (B1 == null)
            {
                //B1 = new TBian(name, len);
                //B1.SetDouble(f1, len, false);
                B1 = new TBian(name, 0);
                B1.FInsert(f1, len);
                mainbian.Add(B1);
            }
            else
            {
                B1.FInsert(f1, len);
            }
            return B1;
        }

        /// <summary>
        /// 重置数据集到某个位置
        /// </summary>
        /// <param name="index"></param>
        public void ResetIndex(int index)
        {
            if (index < 0) return;
            foreach (var arg in mainbian)
            {
                arg.ResetToIndex(index);
            }
        }
        /// <summary>
        /// 数据集后面插入
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public TBian BInsert(string name, double[] f1, int len)
        {
            TBian B1 = null;
            name = name.ToLower();
            B1 = GetBian(name);
            if (B1 == null)
            {
                //B1 = new TBian(name, len);
                //B1.SetDouble(f1, len, false);
                B1 = new TBian(name, 0);
                B1.BInsert(f1, len);
                mainbian.Add(B1);
            }
            else
            {
                B1.BInsert(f1, len);
            }
            return B1;
        }

        #endregion

        /// <summary>
        /// 在数据集后面添加一个值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <returns></returns>
        public TBian AppendValue(string name, double f1)
        {
            TBian b1;

            name = name.ToLower();
            b1 = GetBian(name);
            if (b1 == null)
            {
                b1 = new TBian(name, 0);
                mainbian.Add(b1);
            }
            b1.AppendValue(f1);
            return b1;
        }

        /// <summary>
        /// 设置某个下标对应的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public TBian EditValue(string name, int index, double val)
        {
            TBian data = GetBian(name.ToLower());
            if (data != null)
            {
                data.EditValue(index, val);
            }
            return data;
        }


        /// <summary>
        /// 添加一个变量到
        /// </summary>
        /// <param name="name"></param>
        /// <param name="len1"></param>
        /// <returns></returns>
        public TBian AddBian(string name, int len1)
        {
            TBian s1;
            name = name.ToLower();
            s1 = check(name);
            if (s1 == null)
            {
                s1 = new TBian(name, len1);
                CurTech.namebian.Add(s1);
            }
            else
                s1.len = len1;
            return s1;
        }


        /// <summary>
        /// 获得某个名称的数据集
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TBian GetBian(string name)
        {
            TBian b1 = null;
            name = name.ToLower();
            if (mainbian != null)
            {
                for (int i = 0; i < mainbian.Count; i++)
                {
                    if (mainbian[i].name == name)
                    {
                        b1 = mainbian[i];
                        return b1;
                    }
                }
            }
            if (CurTech != null)
            {
                for (int i = 0; i < CurTech.namebian.Count; i++)
                {
                    if (CurTech.namebian[i].name == name)
                    {
                        b1 = CurTech.namebian[i];
                        return b1;
                    }
                }
            }
            return b1;
        }

        /// <summary>
        /// 通过数据序列名称获得数据集
        /// name进行相关转换h->high
        /// </summary>
        /// <param name="name1"></param>
        /// <returns></returns>
        public TBian check(string name1)
        {
            TBian b1;
            name1 = name1.ToLower();
            //名字直接存在则返回
            b1 = GetBian(name1);
            if (b1 != null)
                return b1;
            //简写转换成全拼 获得数据集
            if (name1 == "h")
                name1 = "high";
            if (name1 == "l")
                name1 = "low";
            if (name1 == "o")
                name1 = "open";
            if (name1 == "c")
                name1 = "close";
            if (name1 == "v")
                name1 = "vol";
            b1 = GetBian(name1);
            //默认返回close
            if (b1 == null)
            {
                if ((name1 == "high") || (name1 == "low") || (name1 == "open"))
                    b1 = GetBian("close");
            }
            return b1;
        }



        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        static bool TryStrToFloat(string strValue, ref double outValue)
        {
            outValue = 0.0;
            if ((strValue == null) || (strValue.Length > 10))
                return false;
            bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            if (IsFloat)
                double.TryParse(strValue, out outValue);
            return IsFloat;
        }


        public Boolean teststr(string s1, ref double f1)
        {
            string s2;
            s2 = CurTech.ls.values(s1.ToLower());
            if (s2.Length == 0)
                s2 = s1;

            if (Symbol != null)
            {
                if (s1 == "capital")
                {
                    f1 = Symbol.FinanceData.LTG / 100;
                    return true;
                }

                if (s1 == "totalcapital")
                {
                    f1 = Symbol.FinanceData.zl[0] / 100;
                    return true;
                }
            }
            return TryStrToFloat(s2, ref f1);
        }


        public double getvalue(string name, int index, ref Boolean value)
        {
            double result;
            TBian bian;
            double fdatetime;
            int hour, minute, year, month, day;
            DateTime dt;
            result = 0.0F;

            //如果是常数则直接返回
            value = teststr(name, ref result);
            if (value)
                return result;
            //某个数据集则返回对应序号的值
            bian = check(name);
            if (bian != null)
            {
                result = bian.value[index];
                value = true;
                return result;
            }
            name = name.ToLower();
            if ((name == "year") || (name == "month") || (name == "week") || (name == "day"))
            {
                bian = check("date");
                if (bian == null)
                {
                    value = false;
                    return result;
                }
                fdatetime = bian.value[index];
                year = Convert.ToInt32(fdatetime / 10000);
                month = Convert.ToInt32((fdatetime % 10000) / 100);
                day = Convert.ToInt32(fdatetime % 100);
                //decodedate(fdatetime, year, month, day);
                if (name == "year")
                    result = year;
                if (name == "month")
                    result = month;
                if (name == "day")
                    result = day;
                if (name == "week")
                {
                    dt = new DateTime(year, month, day);
                    result = Convert.ToInt32(dt.DayOfWeek);
                }
                value = true;
                return result;
            }
            if ((name == "hour") || (name == "minute") || (name == "fromopen"))
            {
                bian = check("time");
                if (bian == null)
                {
                    value = false;
                    result = 0.0;
                    return result;
                }
                fdatetime = bian.value[index];
                hour = Convert.ToInt32(fdatetime / 100);
                minute = Convert.ToInt32(fdatetime - hour * 100);
                if (name == "hour")
                    result = hour;
                if (name == "minute")
                    result = minute;
                if (name == "fromopen")
                    result = hour * 60 + minute - (9 * 60 + 30);
                value = true;
                return result;
            }
            value = false;
            return result;
        }

        /// <summary>
        /// 获得某个数据集最新数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        double GetLastValue(string name)
        {
            TBian b1;
            double result = NA;
            b1 = check(name);
            if (b1 != null)
            {
                result = b1.value[b1.len - 1];
            }
            return result;
        }



        /// <summary>
        /// 获得数据集长度
        /// </summary>
        /// <returns></returns>
        public int RecordCount
        {
            get
            {
                if (mainbian.Count == 0) return 0;
                TBian b1 = mainbian[0];
                return b1.len;
            }
            
        }
    }
}
