using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TradingLib.MarketData;


namespace CStock
{
    public class Compile
    {
        public string ErrorString = "";
        private string yh(string bd, TStringList pgm)//优化
        {
            string s1, s2;
            s1 = "";
            for (int j3 = 0; j3 < pgm.Count; j3++)
            {
                s2 = pgm[j3];
                int j = s2.IndexOf(":=" + bd);
                if (j > -1)
                {
                    if (j > 1)
                        if (s2[j - 1] == ':')
                            j--;
                    s1 = s2.Substring(0, j);
                    break;
                }
            }
            return s1;
        }

        public Boolean S_Compile(TStringList src, TStringList dst)
        {
            int i, j;
            Boolean h, R;
            string s1, s2, s3;
            string[] ss;
            char cc;
            dst.Clear();
            R = true;
            ErrorString = "";
            for (i = 0; i < src.Count; i++)
            {
                s2 = src[i].Trim();
                if (s2.Length == 0)
                    continue;
                if (s2[0] == '*')
                    continue;
                if (s2[0] == '{')
                    continue;
                if (s2[0] == '#')
                    continue;
                s2 = s2.Replace(")and(", ")&(");
                s2 = s2.Replace(")and ", ")&");
                s2 = s2.Replace(" and ", "&");
                s2 = s2.Replace(")or(", ")|(");
                s2 = s2.Replace(")or ", ")|");
                s2 = s2.Replace(" or ", "|");
                s2 = s2.Replace(")AND(", ")&(");
                s2 = s2.Replace(")AND ", ")&");
                s2 = s2.Replace(" AND ", "&");
                s2 = s2.Replace(")OR(", ")|(");
                s2 = s2.Replace(")OR ", ")|");
                s2 = s2.Replace(" OR ", "|");
                h = false;
                s3 = "";
                for (j = 0; j < s2.Length; j++)
                {
                    cc = s2[j];
                    if (cc == '\'')
                    {
                        h = h ^ true;
                    }
                    if ((cc == ' ') && (h == false))
                    {
                        continue;
                    }
                    if ((cc == ';') && (j == s2.Length))
                        continue;
                    if ((h == false) && (cc >= 'a') && (cc <= 'z'))
                        s3 += cc.ToString().ToUpper();
                    else
                        s3 += cc;
                }
                s2 = s3;
                ss = s3.Split(';');
                for (j = 0; j < ss.Length; j++)
                {
                    s1 = ss[j].Trim();
                    if (s1.Length > 0)
                    {
                        if (fen(s1, dst, i) == false)
                            return false;
                    }
                }
            }
            return R;
        }

        public Boolean fen(string s2, TStringList pgm, int lineno)
        {

            string s1, s3, s4, s5;
            int i, j, j1, j2, j3;
            string f1, f2, f3;
            string[] ss;
            string var1, wei, str;
            Boolean h, R;

            str = "";
            j1 = s2.IndexOf('\'');
            if (j1 > -1)
            {
                str = s2.Substring(j1 + 1, s2.Length - j1 - 1);// copy(s2, j1 + 1, 1000)
                j2 = str.IndexOf('\'');
                if (j2 > -1)
                {
                    str = str.Substring(0, j2);
                    s2 = s2.Replace(str, "@@@@_s");
                }
            }

            j1 = s2.IndexOf(":=");
            if (j1 == -1)
                j1 = s2.IndexOf(":");

            if (j1 > -1)
            {
                i = s2.IndexOf("(");//, s2);
                if ((i > -1) && (i < j1))
                    j1 = -1;
            }

            var1 = "";
            if (j1 > -1)
            {
                j2 = 0;
                if (s2[j1 + 1] == '=')
                    j2++;
                var1 = s2.Substring(0, j1 + j2 + 1);// 最终变量
                s2 = s2.Substring(j1 + j2 + 1);//, s2.Length - j1 - j2-1);
                /*
                var1 = s2.Substring(0, j1);
                j2 = 1;
                if (s2[j1 + 1] == '=')
                    j2 = 2;
                if ((j2 == 1) && s2[j1] == ':')
                    var1 = var1 + ":";
                s2 = s2.Substring(j1 + j2, s2.Length - j1 - j2);
                 * */
            }
            j2 = 0;
            h = false;

            for (j = 0; j < s2.Length; j++)// TO length(s2) 
            {
                if (s2[j] == '\'')// $27 
                    h = h ^ true;
                if ((s2[j] == '(') && (h == false))
                    j2 += 1;
                if ((s2[j] == ')') && (h == false))
                    j2 -= 1;

            }
            if (j2 != 0)
            {
                ErrorString = "括号不配匹!\n第" + lineno.ToString() + "行";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }

            j = 0;
            for (j1 = s2.Length - 1; j1 > -1; j1--)// j1 = length(s2) WNTO 1 
            {
                if (s2[j1] == ')')
                    break;
                if (s2[j1] == ',')
                    j = j1;
            }
            wei = "";
            if (j > 0)
            {
                wei = s2.Substring(j, s2.Length - j);// copy(s2, j, 1000);
                s2 = s2.Substring(0, j);// copy(s2, 1, j - 1);
            }

            j2 = s2.IndexOf(')');// Pos(')', s2);
            while (j2 > -1)
            {
                for (j = j2 - 1; j > -1; j--)// WNTO 1 
                {
                    if (s2[j] == '(')
                        break;
                }

                for (j1 = j - 1; j1 > -1; j1--)// WNTO 1 
                {
                    if ((s2[j1] < 'A') || (s2[j1] > 'Z'))
                        break;
                }

                s1 = s2.Substring(j + 1, j2 - j - 1);// copy(s2, j + 1, j2 - j - 1); //参数表
                if ((checkfh(s1) == 0) && (j1 == -1) && (j2 == s2.Length - 1))
                    break;
                if ((j > j1) && (j2 > j))
                {
                    s3 = s2.Substring(0, j1 + 1);// copy(s2, 1, J1);
                    s4 = s2.Substring(j1 + 1, j - j1 - 1);// copy(s2, j1 + 1, J - j1 - 1); //函数名
                    s5 = s2.Substring(j1 + 1, j2 - j1);// copy(s2, J1 + 1, j2 - J1); //函数与参数
                    s1 = s2.Substring(j + 1, j2 - j - 1);// copy(s2, j + 1, j2 - j - 1); //参数表
                    if (s4.Length == 0)
                        s5 = s1; //没有函数名仅用参数表

                    if ((checkfh(s1) > 0) && (s4.Length > 0))  //
                    {
                        f1 = "";
                        ss = s1.Split(',');
                        for (i = 0; i < ss.Length; i++)// I = 0 TO 100 
                        {
                            f2 = ss[i].Trim();
                            if (f2.Length == 0)
                                break;
                            if (i > 0)
                                f1 = f1 + ",";
                            j3 = checkfh(f2);
                            if (j3 > 0)
                            {
                                f3 = yh(f2, pgm);
                                if (f3.Length == 0)
                                {
                                    if ((j1 > -1) || (var1.Length > 0))
                                    {
                                        f3 = "@T_" + lineno.ToString() + "_" + pgm.Count.ToString();//  format('@_%d_%d', [lineno, pgm.count])
                                    }
                                    else
                                    {
                                        f3 = "@L_" + lineno.ToString() + "_" + pgm.Count.ToString();
                                    }
                                    R = fen2(f3 + ":=" + f2, "", pgm, lineno);
                                    if (R == false)
                                    {
                                        return false;
                                    }
                                }
                                f1 = f1 + f3;
                            }
                            else
                                f1 = f1 + f2;
                        }
                        s5 = s4 + '(' + f1 + ')';
                    }
                    if ((j1 == -1) && (j2 == s2.Length - 1))
                    {
                        s2 = s5;
                        break;
                    }
                    s4 = yh(s5, pgm);
                    if (s4.Length == 0)
                    {
                        if (((j1 == -1) || (s2[j2] == ',')) && (var1.Length == 0))
                        {
                            s4 = "@L_" + lineno.ToString() + "_" + pgm.Count.ToString();// format('L_%d_%d', [lineno, pgm.count])
                        }
                        else
                        {
                            s4 = "@T_" + lineno.ToString() + "_" + pgm.Count.ToString();// format('@_%d_%d', [lineno, pgm.count]);
                        }
                        R = fen2(s4 + ":=" + s5, "", pgm, lineno);
                        if (R == false)
                            return false;
                    }
                    s2 = s3 + s4 + s2.Substring(j2 + 1, s2.Length - j2 - 1);// copy(s2, J2 + 1, Length(s2) - J2);
                }
                j2 = s2.IndexOf(')');//, s2);
            }
            if (str.Length > 0)
                s2 = s2.Replace("'@@@@_s'", str);// StringReplace(s2, '@@@@_s', str, [rfReplaceAll, rfIgnoreCase]);
            if ((var1.Length == 0) && (s2.IndexOf('(') > -1))
            {
              string fn=s2.Substring(0,s2.IndexOf('('));
              string str11=" VOLSTICK SYLINE BTX TWR PARTLINE COLORSTICK DRAWICON DRAWTEXT BOX DRAWKLINE STICKLINE ";
              if (str11.IndexOf(" "+fn+" ")==-1)
                  var1 = "LINEOUT"+ lineno.ToString() + "_" + pgm.Count.ToString()+":";
            }
            R = fen2(var1 + s2, wei, pgm, lineno);
            return R;
        }

        public static bool TryStrToFloat(string strValue, ref double outValue)
        {
            outValue = 0.0;
            if ((strValue == null) || (strValue.Length > 10))
                return false;
            bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            if (IsFloat)
                double.TryParse(strValue, out outValue);
            return IsFloat;
        }

        public int checkfh(string s2)//表达式含有符号
        {
            string[] fh = { "+", "-", "*", "/", "<", "<=", ">=", ">", "=", "|", "&", "^" };
            int i, k, j1;
            Boolean h, b;
            double d1 = 0.0;
            if (TryStrToFloat(s2, ref d1))
                return 0;

            j1 = 0;
            i = 0;
            h = false;

            while (i < s2.Length)
            {
                if (s2[i] == 0x27)
                    h = h ^ true;

                b = false;
                for (k = 0; k < fh.Length; k++)
                {
                    if (s2[i] == fh[k][0])
                    {
                        b = true;
                        break;
                    }
                }
                if ((h == false) && (b == true))// && (s2[i] IN ['+', '-', '*', '/', '<', '>', '=', '|', '&', '^'])) 
                {
                    if (((s2[i] == '<') || (s2[i] == '>')) && (s2[i + 1] == '='))
                        i++;
                    j1 += 1;//INC (J1);
                }
                i++;
            }
            return j1;

        }
        public Boolean fen2(string s2, string wei, TStringList pgm, int lineno)
        {
            string[] fh = { "<=", ">=", "+", "-", "*", "/", "<", ">", "=", "|", "&", "^" };
            TStringList cs;
            int p1, i, k, j1, j2;
            string f1, f2, f4;
            Boolean b;


            j1 = s2.IndexOf(":=");
            if (j1 == -1)
                j1 = s2.IndexOf(':');

            f2 = "";
            f4 = s2;
            if (j1 > -1)
            {
                j2 = 0;
                if (s2[j1 + 1] == '=')
                    j2++;
                f2 = s2.Substring(0, j1 + j2 + 1);
                f4 = s2.Substring(j1 + j2 + 1);//, s2.Length - j1 - j2);
                //f2 = s2.Substring(0, j1 + 2);// copy(s2, 1, j1 + j2);
                //f4 = s2.Substring(j1 + 2, s2.Length - j1 - 2);// copy(s2, j1 + 1 + j2, length(s2) - j1 - j2);
            }

            i = checkfh(f4);
            if (i < 1)
            {
                
                if ((f2.Length==0) && (s2.IndexOf('(')>-1))
                {
                    string fn = s2.Substring(0, s2.IndexOf('('));
                    string str11 = " VOLSTICK SYLINE BTX TWR PARTLINE COLORSTICK DRAWICON DRAWTEXT BOX DRAWKLINE STICKLINE ";
                    int kk = str11.IndexOf(" " + fn + " ");
                    if (kk == -1)
                        s2 = "LINEOUT" + lineno.ToString() + "_" + pgm.Count.ToString() + ":" + s2;
                }
                
                pgm.Add(s2 + wei);
                return true;
            }
            s2 = f4;
            i = 0;
            string ds = "0123456789.";
            f4 = "";
            if (s2[0] == '-')  //-A*N  or -2*N
            {
                if (ds.IndexOf(s2[1]) > -1)
                {
                    f4 = "-";
                    i++;
                }
                else
                    s2 = '0' + s2;
            }

            cs = new TStringList();
            //   cs.Text = s2;

            i = 0;
            f4 = "";
            while (i < s2.Length)
            {
                b = false;
                b = false;
                for (k = 0; k < fh.Length; k++)
                {
                    if (s2[i] == fh[k][0])
                    {
                        b = true;
                        break;
                    }
                }
                if (b)//s2[i] IN ['+', '-', '*', '/', '<', '>', '=', '|', '&', '^']) 
                {
                    f4 = f4 + "\n" + s2[i];
                    if (((s2[i] == '<') || (s2[i] == '>')) && (s2[i + 1] == '='))
                    {
                        f4 = f4 + '=';
                        i += 1;
                    }
                    f4 = f4 + "\n";
                    if (s2[i + 1] == '-')
                    {
                        f4 = f4 + '-';
                        i += 1;
                    }
                }
                else
                {
                    f4 = f4 + s2[i];
                }
                i += 1;
            }
            cs.Text = f4;


            if ((cs.Count % 2) == 0)
            {
                ErrorString = "运算符不配匹!\n第" + lineno.ToString() + "行";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }
            i = 0;
            while (i < cs.Count && (cs.Count > 3))
            {
                f1 = cs[i];
                if ((f1 =="*") || (f1 =="/"))
                {
                    f1 = "@T_" + lineno.ToString() + "_" + (pgm.Count + 1);// format('@_%d_%d', [lineno, pgm.Count]);
                    pgm.Add(f1 + ":=" + cs[i - 1] + cs[i] + cs[i + 1]);
                    cs[i - 1] = f1;
                    cs.Delete(i);
                    cs.Delete(i);
                    i --;
                }
                i += 1;

            }
            if ((cs.Count % 2) == 0)
            {
                ErrorString = "运算符不配匹!\n第" + lineno.ToString() + "行";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }
            i = 0;
            while (i < cs.Count && (cs.Count > 3))
            {
                f1 = cs[i];
                if ((f1 == "+") || (f1 == "-"))
                {
                    f1 = "@T_" + lineno.ToString() + "_" + (pgm.Count + 1);// format('@_%d_%d', [lineno, pgm.Count]);
                    pgm.Add(f1 + ":=" + cs[i - 1] + cs[i] + cs[i + 1]);
                    cs[i - 1] = f1;
                    cs.Delete(i);
                    cs.Delete(i);
                    i--;
                }
                i += 1;

            }
            if ((cs.Count % 2) == 0)
            {
                ErrorString = "运算符不配匹!\n第" + lineno.ToString() + "行";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }

            for (i = 0; i < fh.Length; i++)// I = 0 TO length(fh) - 1 
            {
                p1 = cs.IndexOf(fh[i]);
                while ((p1 > 0) && (cs.Count > 3))
                {
                    f1 = "@T_" + lineno.ToString() + "_" + (pgm.Count + 1);// format('@_%d_%d', [lineno, pgm.Count]);
                    pgm.Add(f1 + ":=" + cs[p1 - 1] + cs[p1] + cs[p1 + 1]);
                    cs[p1 - 1] = f1;
                    cs.Delete(p1);
                    cs.Delete(p1);
                    p1 = cs.IndexOf(fh[i]);
                }
                if (cs.Count < 4)
                    break;
            }
            if ((cs.Count % 2) == 0)
            {
                ErrorString = "运算符不配匹!\n第" + lineno.ToString() + "行";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }
           if (f2.Length == 0)
                f2 = "LINEOUT" + lineno.ToString() + "_" + pgm.Count.ToString() + ":";
            f4 = "";
            for (i = 0; i < cs.Count; i++)// I = 0 TO cs.Count - 1 
                f4 = f4 + cs[i];
            pgm.Add(f2 + f4 + wei);
            return true;
        }

        /// <summary>
        /// 公式测试
        /// </summary>
        /// <param name="str">公式字符串</param>
        /// <param name="Error">返回错误信息</param>
        /// <returns>是否成功</returns>
        public static bool CheckGongSi1(string str, ref string Error)
        {
            double[] fopen ={ 22.59, 22.17, 21, 20.02, 20.20, 20.38 };
            double[] fhigh = { 22.59, 22.67, 21, 20.37, 20.6, 20.9 };
            double[] flow = { 22.08, 21.92, 19.94, 19.64, 20.10, 20.19 };
            double[] fclose = { 22.14, 22.15, 20.02, 20.12, 20.39, 20.44 };
            double[] fvol = { 55977300, 38599693, 117504326, 56984842, 38554183, 46305400 };
            double[] famount = { 1248606080, 859815744, 2385878272, 1139477760, 786200256, 952838144 };

            double[] fdate = { 20130326, 20130327, 20130328, 20130329, 20130401, 20130402 };
            double[] ftime = { 930, 930, 930, 930, 930, 930 };

            Compile cm = new Compile();
            TStringList src = new TStringList();
            src.Text = str;
            TStringList dst = new TStringList();
            if (!cm.S_Compile(src, dst))
            {
                Error = cm.ErrorString;
                return false;
            }

            TGongSi gs = new TGongSi();
            gs.Add("open", fopen, fopen.Length);
            gs.Add("high", fhigh, fhigh.Length);
            gs.Add("low", flow, flow.Length);
            gs.Add("close", fclose, fclose.Length);
            gs.Add("vol", fvol, fvol.Length);
            gs.Add("amount", famount, famount.Length);
            gs.Add("data", fdate, fdate.Length);
            gs.Add("time", ftime, ftime.Length);
            bool rt = gs.setprogtext(str);
            Error = gs.ErrorString;
            return rt;
        }

       public static  bool CheckGongSi(string str, ref string Error)
        {
            double[] fopen = { 22.59, 22.17, 21, 20.02, 20.20, 20.38 };
            double[] fhigh = { 22.59, 22.67, 21, 20.37, 20.6, 20.9 };
            double[] flow = { 22.08, 21.92, 19.94, 19.64, 20.10, 20.19 };
            double[] fclose = { 22.14, 22.15, 20.02, 20.12, 20.39, 20.44 };
            double[] fvol = { 55977300, 38599693, 117504326, 56984842, 38554183, 46305400 };
            double[] famount = { 1248606080, 859815744, 2385878272, 1139477760, 786200256, 952838144 };

            double[] fdate = { 20130326, 20130327, 20130328, 20130329, 20130401, 20130402 };
            double[] ftime = { 930, 930, 930, 930, 930, 930 };

            TradingLib.MarketData.MDSymbol stockinfo = new TradingLib.MarketData.MDSymbol();
            //stockinfo.GP = new TGPNAME();
            //stockinfo.GP.code = new byte[6];
            byte[] cd = { 30, 30, 30, 30, 30, 31 };
            //cd.CopyTo(stockinfo.GP.code, 0);
            stockinfo.Symbol = "000001";
            stockinfo.Name = "平安银行";
            stockinfo.Precision = 2;
            stockinfo.PreClose = 11.5f;

            stockinfo.TickSnapshot = new TDX();
            stockinfo.TickSnapshot.Sell5 = 11.54;
            stockinfo.TickSnapshot.Sell4 = 11.53;
            stockinfo.TickSnapshot.Sell3 = 11.52;
            stockinfo.TickSnapshot.Sell2 = 11.51;
            stockinfo.TickSnapshot.Sell1 = 11.50;
            stockinfo.TickSnapshot.Buy1 = 11.49;
            stockinfo.TickSnapshot.Buy2 = 11.48;
            stockinfo.TickSnapshot.Buy3 = 11.47;
            stockinfo.TickSnapshot.Buy4 = 11.46;
            stockinfo.TickSnapshot.Buy5 = 11.45;
            stockinfo.TickSnapshot.SellQTY5 = 1048;
            stockinfo.TickSnapshot.SellQTY4 = 2080;
            stockinfo.TickSnapshot.SellQTY3 = 568;
            stockinfo.TickSnapshot.SellQTY2 = 529;
            stockinfo.TickSnapshot.SellQTY1 = 782;
            stockinfo.TickSnapshot.BuyQTY1 = 2398;
            stockinfo.TickSnapshot.BuyQTY2 = 2354;
            stockinfo.TickSnapshot.BuyQTY3 = 2714;
            stockinfo.TickSnapshot.BuyQTY4 = 1598;
            stockinfo.TickSnapshot.BuyQTY5 = 1363;
            stockinfo.TickSnapshot.last = 11.5;
            stockinfo.TickSnapshot.Open = 11.49;
            stockinfo.TickSnapshot.High = 11.62;
            stockinfo.TickSnapshot.Low = 11.46;
            stockinfo.TickSnapshot.Price = 11.50;
            stockinfo.TickSnapshot.Volume = 318773;
            stockinfo.TickSnapshot.Amount = 367350848;

            //stockinfo.FinanceData = new TradingLib.MarketData.FinanceData();
            stockinfo.FinanceData.LTG = 557590.1875;
            stockinfo.FinanceData.day1 = 20140506;
            stockinfo.FinanceData.day2 = 19910403;
            stockinfo.FinanceData.t1 = 18;
            stockinfo.FinanceData.t2 = 1;
            stockinfo.FinanceData.zl = new float[30];
            stockinfo.FinanceData.zl[0] = 952074.563f;
            stockinfo.FinanceData.zl[1] = 180199.0f;
            stockinfo.FinanceData.zl[3] = 3589000.0f;
            stockinfo.FinanceData.zl[4] = 10802000.0f;
            stockinfo.FinanceData.zl[7] = 2097102080.0f;
            stockinfo.FinanceData.zl[9] = 3580000.0f;
            stockinfo.FinanceData.zl[10] = 5332000.0f;
            stockinfo.FinanceData.zl[11] = 319109.0f;
            stockinfo.FinanceData.zl[14] = 51899000.0f;
            stockinfo.FinanceData.zl[15] = 117300000.0f;
            stockinfo.FinanceData.zl[16] = 16100000.0f;
            stockinfo.FinanceData.zl[19] = 6712000.0f;
            stockinfo.FinanceData.zl[20] = 2185000.0f;
            stockinfo.FinanceData.zl[21] = 60097000.0f;
            stockinfo.FinanceData.zl[22] = 77155000.0f;
            stockinfo.FinanceData.zl[24] = 6697000.0f;
            stockinfo.FinanceData.zl[25] = 5054000.0f;
            stockinfo.FinanceData.zl[26] = 5054000.0f;
            stockinfo.FinanceData.zl[27] = 35017000.0f;
            stockinfo.FinanceData.zl[29] = 3.0f;



            Compile cm = new Compile();
            TStringList src = new TStringList();
            src.Text = str;
            TStringList dst = new TStringList();
            if (!cm.S_Compile(src, dst))
            {
                Error = cm.ErrorString;
                return false;
            }

            TGongSi gs = new TGongSi();
            gs.Symbol = stockinfo;
            gs.Add("open", fopen, fopen.Length);
            gs.Add("high", fhigh, fhigh.Length);
            gs.Add("low", flow, flow.Length);
            gs.Add("close", fclose, fclose.Length);
            gs.Add("vol", fvol, fvol.Length);
            gs.Add("amount", famount, famount.Length);
            gs.Add("data", fdate, fdate.Length);
            gs.Add("time", ftime, ftime.Length);
            bool rt = gs.setprogtext(str);
            Error = gs.ErrorString;
            return rt;
        }

    }


}