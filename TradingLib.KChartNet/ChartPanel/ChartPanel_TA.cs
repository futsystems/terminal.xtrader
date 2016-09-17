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
using TradingLib.MarketData;


namespace CStock
{
    public partial class TGongSi
    {

        #region Run 执行运算
        public bool run(string paramstr)
        {
            if (paramstr != null)
                if (paramstr.Length > 0)
                {
                    string[] ps = paramstr.Split(',');
                    if (ps.Length > 0)
                    {
                        CurTech = TechList[0];
                        for (int j = 0; j < ps.Length; j++)
                            CurTech.param[j] = Convert.ToInt32(ps[j]);
                    }
                }
            return run();
        }
        public bool run()
        {
            string[] fh = { "+", "-", "*", "/", ">=", "<=", "<", ">", "=", "|", "&" };
            string[] clist = { "COLORWHITE", "COLORYELLOW", "COLORMAGENTA", "COLORGREEN", "COLORGRAY",
                                 "COLORBLUE" };
            string var1, var2, var3, wei;
            string s1, s2;
            int i, p, p1, dlen, t1, k;
            string[] param = new string[50];
            Boolean ok, f1;
            FUNC sf;
            double f2;
            TBian b1, b2;
            Boolean mh; //使用: 表示该变量同时用于输出 mh;
            int cc; //颜色计数 cc;


            dlen = RecordCount;
            if (dlen == 0)
            {
                return false;
            }
            CurTech = TechList[0];
            if ((QuanStyle != QuanType.qsNone) && (showfs == false))
            {
                b1 = check("q_close");
                if (b1 == null)
                    SetQuanStyle(QuanStyle);
            }
            string curtechname = "";
            int curline = 0;
            int bz;
            cc = 0;
            ok = true;
            for (int t = 0; t < TechList.Count; t++)
            {
                CurTech = TechList[t];
                CurTech.outline.Clear();
                CurTech.ls.Clear();
                CurTech.Input.Clear();
                for (i = 0; i < CurTech.pg1.Count; i++)
                {
                    curline = i;
                    s1 = CurTech.pg1[i].Trim();
                    s2 = s1;
                    if (s1.Length == 0)
                        continue;
                    if (s1[0] == '*')
                        continue;
                    if (s1[0] == '#')
                        continue;
                    var1 = "";
                    mh = false;
                    p = s1.IndexOf(":=");// pos("=", s1);
                    if (p == -1)
                    {
                        p = s1.IndexOf(':');
                        p1 = s1.IndexOf('\'');
                        if ((p1 > 0) && (p > p1))  //存在单引号 并且在:之前
                            p = 0;
                        if (p > 0)
                        {
                            var1 = s1.Substring(0, p);// .Left(p);
                            mh = true;
                        }
                    }
                    else
                    {
                        var1 = s1.Substring(0, p);// s1.Left(p);
                        p++;
                    }
                    if (p > -1)
                        s1 = s1.Substring(p + 1);//.Right(s1.GetLength() - p - 1);
                    ////////////////取输出修饰
                    p = -1;
                    for (t1 = s1.Length - 1; t1 > -1; t1--)
                    {
                        if (s1[t1] == ')')
                            break;
                        if (s1[t1] == ',')
                            p = t1;
                    }
                    wei = "";
                    if (p > -1)
                    {
                        wei = s1.Substring(p + 1);//, s1.Length - p - 1);// copy(s1, p + 1, 1000);
                        s1 = s1.Substring(0, p);// copy(s1, 1, p - 1);
                    }
                    ////////////
                    if (var1 == "TECHNAME")
                    {
                        if (t == 0)
                            ftechname = s1;
                        CurTech.techname = s1;
                        curtechname = s1;
                        continue;
                    }
                    if (var1 == "DIGIT")
                    {
                        if (t == 0)
                            digit = int.Parse(s1);
                        continue;
                    }
                    if (var1 == "YSTR")
                    {
                        if (t == 0)
                            yStr = s1;
                        continue;
                    }
                    if (var1 == "YVALUE")
                    {
                        if (t == 0)
                            yValue = s1;
                        continue;
                    }
                    if (var1 == "TECHTITLE")
                    {
                        if (t == 0)
                            ftechtitle = s1;
                        CurTech.techtitle = s1;
                        continue;
                    }
                    if (var1 == "PASSWORD")
                    {
                        if (t == 0)
                            fpassword = s1;
                        CurTech.password = s1;
                        continue;
                    }
                    if (var1 == "DRAWK")
                    {
                        if (t == 0)//叠加公式失效
                        {
                            if (s1 == "TRUE")
                                showk = 1;
                            else
                                showk = 0;
                        }
                        continue;
                    }

                    //---------------
                    if (mh == true)
                    {
                        p = wei.IndexOf("COLOR");
                        if (wei.IndexOf("COLOR") == -1)
                        {
                            wei = clist[cc] + "," + wei;
                            cc++;
                        }
                        CurTech.outline.Add("SYLINE=" + var1 + "::" + wei);
                        if (cc == clist.Length)
                            cc = 0;
                    }
                    ok = true;
                    p = s1.IndexOf('(');
                    p1 = s1.IndexOf(')');
                    if ((p > -1) && (p1 > p))
                    {
                        var2 = s1.Substring(0, p);
                        var2 = var2.ToLower();
                        s2 = s1.Substring(p + 1, p1 - p - 1);
                        if (wei.Length > 0)
                            s2 = s2 + ",::" + wei;
                        param = s2.Split(',');

                        ok = false;
                        s1 = s2;
                        sf = func.GetFunc(var2);
                        if (sf != null) // assigned(sf) )
                            ok = sf(var1, var2, param);
                        if (ok == false)
                            break;
                        else
                            continue;
                    }

                    if (var1 == "")
                    {
                        f2 = 0.0F;
                        if (teststr(s1, ref f2))
                        {
                            if (wei.IndexOf("COLOR") == -1)
                            {
                                wei = clist[cc] + "," + wei;
                                cc++;
                            }
                            CurTech.outline.Add("SYLINE=" + s1 + "::" + wei);
                            if (cc == clist.Length)
                                cc = 0;
                        }
                        continue;
                    }

                    bz = -1;
                    t1 = 0;
                    if ((p == -1) && (p1 == -1))
                    {
                        for (t1 = 0; t1 < fh.Length; t1++)
                        {
                            k = s1.IndexOf(fh[t1]);
                            if (k > -1)
                            {
                                var2 = s1.Substring(0, k);
                                var3 = s1.Substring(k + fh[t1].Length, s1.Length - k - fh[t1].Length);
                                ok = js(var1, var2, fh[t1], var3);
                                bz = 1;
                                break;
                            }
                        }

                    }

                    if (bz == -1)
                    {
                        if (var1 == "")
                        {
                            f2 = 0.0F;
                            if (teststr(s1, ref f2))
                            {
                                if (wei.IndexOf("COLOR") == -1)
                                {
                                    wei = clist[cc] + "," + wei;
                                    cc++;
                                }
                                CurTech.outline.Add("SYLINE=" + s1 + "::" + wei);
                                if (cc == clist.Length)
                                    cc = 0;
                            }
                            continue;
                        }
                        else
                        {
                            f2 = 0.0f;
                            f1 = teststr(s1, ref f2);
                            if (f1 == true)
                                CurTech.ls.Add(var1 + "=" + s1);
                            else
                            {
                                b2 = check(s1);
                                if (b2 != null)
                                {
                                    b1 = AddBian(var1, b2.len);
                                    b1.SetBian(b2);
                                }
                                else
                                    ok = false;
                            }
                        }
                    }
                    if (!ok)
                        break;
                }
                if (!ok)
                    break;
            }
            if ((ok == false))
                showerror(curtechname + ": \n第" + string.Format("{0:d}", curline + 1) + "行发现错误:" + CurTech.pg1[curline]);
            CurTech = TechList[0];
            GC.Collect();
            return ok;
        }
        #endregion


        public Boolean x_n(string name, string[] xx, string type1)
        {
            TBian var11, x1, nn;
            double f1, n1;
            int i, j, k, n, dlen;

            x1 = check(xx[0]);
            if (x1 == null)
            {
                showerror(xx[0] + "未定义!");
                return false;
            }
            nn = check(xx[1]);
            n1 = 0;
            n = 0;
            if (nn == null)
            {
                if (teststr(xx[1], ref  n1) == false)
                {
                    showerror(xx[1] + "未定义!");
                    return false; ;
                }
                n = Convert.ToInt32(n1);
            }

            var11 = AddBian(name, x1.len);
            f1 = 0.0;
            dlen = RecordCount;
            if (type1 == "sumbars")
            {
                for (i = ft; i < dlen; i++)
                {
                    f1 = 0.0;
                    var11.value[i] = i;
                    for (k = i; k > -1; k--)
                    {
                        f1 = f1 + x1.value[i];
                        if ((f1 > n1))
                        {
                            var11.value[i] = i - k;
                            break;
                        }
                    }
                }
                return true;
            }
            if (type1 == "sum")
            {
                f1 = 0;
                if (nn == null)
                {
                    for (i = 0; i < dlen; i++)
                    {
                        f1 = f1 + x1.value[i];
                        if ((i >= n) && (n > 0))
                            f1 = f1 - x1.value[i - n];
                        var11.value[i] = f1;
                    }
                }
                else
                {
                    for (i = 0; i < dlen; i++)
                    {
                        if (x1.value[i] == NA)
                        {
                            var11.value[i] = NA;
                            continue;
                        }
                        n = Convert.ToInt32(nn.value[i]);
                        if (n == NA)
                            n = 0;
                        if (n > 0)
                            n = Math.Max(0, i - n + 1);
                        f1 = 0;
                        for (j = n; j <= i; j++)
                            f1 = f1 + x1.value[j];
                        var11.value[i] = f1;
                    }
                }
                return true;
            }
            if (type1 == "backset")
            {
                for (i = ft; i < dlen; i++)
                {
                    if (x1.value[i] == 1)
                    {
                        for (k = i; k >= Math.Max(0, i - n); k--)
                            var11.value[k] = 1;
                    }
                }
                return true;
            }
            if (type1 == "filter")
            {
                for (i = ft; i < dlen; i++)
                {
                    var11.value[i] = 0;
                    if (x1.value[i] > 0.0)
                    {
                        var11.value[i] = 1;
                        for (k = i + 1; k >= i + n; k++)
                            var11.value[k] = 0;
                    }
                }
                return true;
            }
            if (type1 == "count")
            {
                k = 0;
                for (i = ft; i < dlen; i++)
                {
                    if (x1.value[i] > 0.0)
                        k++;
                    var11.value[i] = k;
                    if ((n > 0) && (i > n))
                    {
                        if ((i >= n) && (x1.value[i - n] > 0.0))
                            k--; ;
                        var11.value[i] = k;
                    }
                }
                return true;
            }
            if (type1 == "ref")
            {
                for (i = 0; i < dlen; i++)
                {
                    if ((i < n))
                        var11.value[i] = x1.value[0];
                    else
                        var11.value[i] = x1.value[i - n];
                }
                return true;
            }
            if (type1 == "ma")
            {
                //y=[2*x+(n-1)*y"]/(n+1)
                f1 = 0;
                for (i = 0; i < dlen; i++)
                {
                    if (nn != null)
                        n = Convert.ToInt32(nn.value[i]);
                    if ((n == NA) || (x1.value[i] == NA))
                    {
                        var11.value[i] = NA;
                        continue;
                    }
                    f1 = f1 + x1.value[i];
                    if ((n > 0) && (i >= (n - 1)))
                    {
                        if (x1.value[i - n + 1] != NA)
                        {
                            var11.value[i] = f1 / n;
                            f1 = f1 - x1.value[i - n + 1];
                        }
                        else
                            var11.value[i] = NA;
                    }
                    else
                        var11.value[i] = NA;
                }
                return true;
            }
            if (type1 == "dma")
            {
                //y=a*x+(1-a)*y",
                if (n1 > 1)
                {
                    for (i = 0; i < dlen; i++)
                        var11.value[i] = x1.value[i];
                }
                else
                {
                    for (i = 0; i < dlen; i++)
                    {
                        if (nn != null)
                            n1 = nn.value[i];
                        if ((x1.value[i] == NA) || (n == NA))
                        {
                            var11.value[i] = NA;
                            continue;
                        }
                        if (n1 > 1)
                        {
                            var11.value[i] = x1.value[i];
                            continue;
                        }
                        if (i == 0)
                            var11.value[i] = n1 * x1.value[i];
                        else
                        {
                            if (var11.value[i - 1] == NA)
                                var11.value[i] = x1.value[i];
                            else
                                var11.value[i] = n1 * x1.value[i] + (1 - n1) * var11.value[i - 1];
                        }
                    }
                }
                return true;
            }
            if (type1 == "expmema")
            {
                if (n > dlen)
                    n = dlen - 1;

                f1 = 0;
                i = 0;
                j = 0;
                while (j < dlen)
                {
                    var11.value[j] = NA;
                    if (x1.value[j] != NA)
                    {
                        i++;
                        f1 = f1 + x1.value[j];
                    }
                    if (i == n)
                        break;
                    j = j + 1;
                }
                if ((j < dlen + 1) && (i == n))
                {
                    var11.value[j] = f1 / i;
                    for (i = j + 1; i < dlen; i++)
                        var11.value[i] = (2 * x1.value[i] + (n - 1) * var11.value[i - 1]) / (n + 1);
                }
                return true;
            }
            if (type1 == "ema")
            {
                // Y=[2*X+(n-1)*Y"]/(n+1)
                if (n > dlen)
                    n = dlen - 1;
                var11.value[0] = x1.value[0];
                for (i = 1; i < dlen; i++)
                {
                    if (nn != null)
                        n = (int)(nn.value[i]);
                    if ((x1.value[i] == NA) || (n == NA))
                    {
                        var11.value[i] = NA;
                        continue;
                    }
                    if (var11.value[i - 1] == NA)
                        var11.value[i] = x1.value[i];
                    else
                        var11.value[i] = (2 * x1.value[i] + (n - 1) * var11.value[i - 1]) / (n + 1);
                }
                return true;
            }
            if (type1 == "llv")
            {
                for (i = 0; i < dlen; i++)
                {
                    if ((i == 0) || (x1.value[i] == NA))
                    {
                        var11.value[i] = x1.value[i];
                        continue;
                    }
                    if (nn != null)
                    {
                        if (nn.value[i] == NA)
                        {
                            var11.value[i] = x1.value[i];
                            continue;
                        }
                        else
                            n = Convert.ToInt32(nn.value[i]);

                    }
                    f1 = -NA;
                    for (k = Math.Max(0, i - n + 1); k <= i; k++)
                    {
                        if (x1.value[k] < f1)
                            f1 = x1.value[k];
                    }
                    var11.value[i] = f1;
                }
                return true;
            }

            if (type1 == "hhv")
            {
                for (i = 0; i < dlen; i++)
                {
                    if ((i == 0) || (x1.value[i] == NA))
                    {
                        var11.value[i] = x1.value[i];
                        continue;
                    }
                    if (nn != null)
                    {
                        if (nn.value[i] == NA)
                        {
                            var11.value[i] = x1.value[i];
                            continue;
                        }
                        else
                            n = Convert.ToInt32(nn.value[i]);

                    }
                    f1 = NA;
                    for (k = Math.Max(0, i - n + 1); k <= i; k++)
                    {
                        if (x1.value[k] > f1)
                            f1 = x1.value[k];
                    }
                    var11.value[i] = f1;
                }
                return true;
            }
            return false;
        }

        public Boolean js(string name, string aa, string c, string bb)
        {
            int dlen, i;
            double f1, f2, f11, f21;
            TBian a1, b1, n1;

            a1 = check(aa);
            f1 = 0;
            if (a1 == null)
            {
                if (teststr(aa, ref f1) == false)
                {
                    ErrorString = "表达式:[" + name + "=" + aa + c + bb + "] 参数:" + aa + " 错误!";
                    return false;

                }
            }
            b1 = check(bb);
            f2 = 0;
            if (b1 == null)
            {
                if (teststr(bb, ref f2) == false)
                {
                    ErrorString = "表达式:[" + name + "=" + aa + c + bb + "] 参数:" + bb + " 错误!";
                    return false;

                }
            }

            if ((a1 == null) && (b1 == null)) //常数
            {
                int i1 = Convert.ToInt32(f1);
                int i2 = Convert.ToInt32(f2);
                double i3 = 0;
                if (c == "|")
                    i3 = i1 | i2;
                if ((c == "&") || (c == "&&"))
                    i3 = i1 & i2;
                if ((c == ">"))
                    i3 = f1 > f2 ? 1 : 0;
                if ((c == "==") || (c == "="))
                    i3 = f1 == f2 ? 1 : 0;
                if (c == ">=")
                    i3 = f1 >= f2 ? 1 : 0;
                if (c == "<")
                    i3 = f1 < f2 ? 1 : 0;
                if (c == "<=")
                    i3 = f1 <= f2 ? 1 : 0;
                if (c == "+")
                    i3 = f1 + f2;
                if (c == "-")
                    i3 = f1 - f2;
                if (c == "*")
                    i3 = f1 * f2;
                if (c == "/")
                {
                    if (i2 == 0)
                        i3 = 0;
                    else
                        i3 = f1 / f2;
                }
                CurTech.ls.Add(name + "=" + i3.ToString());
                return true;
            }

            dlen = RecordCount;
            n1 = AddBian(name, dlen);
            f11 = f1;
            f21 = f2;
            for (i = 0; i < dlen; i++)
            {
                if (a1 != null)
                    f11 = a1.value[i];
                if (b1 != null)
                    f21 = b1.value[i];
                if ((f11 == NA) || (f21 == NA))
                {
                    n1.value[i] = NA;
                    continue;
                }
                if (c == "|")
                {
                    n1.value[i] = Convert.ToInt32(f11) | Convert.ToInt32(f21);
                    continue;
                }
                if ((c == "&") || (c == "&&"))
                {
                    n1.value[i] = Convert.ToInt32(f11) & Convert.ToInt32(f21);
                    continue;
                }
                if ((c == ">"))
                {
                    if ((f11 > f21))
                        n1.value[i] = 1.0;
                    else
                        n1.value[i] = 0.0;
                    continue;
                }
                if ((c == "=") || (c == "="))
                {
                    if ((f11 == f21))
                        n1.value[i] = 1.0;
                    else
                        n1.value[i] = 0.0;
                    continue;
                }
                if (c == ">=")
                {
                    if ((f11 >= f21))
                        n1.value[i] = 1.0;
                    else
                        n1.value[i] = 0.0;
                    continue;
                }
                if (c == "<")
                {
                    if ((f11 < f21))
                        n1.value[i] = 1.0;
                    else
                        n1.value[i] = 0.0;
                    continue;
                }
                if (c == "<=")
                {
                    if ((f11 <= f21))
                        n1.value[i] = 1.0;
                    else
                        n1.value[i] = 0.0;
                    continue;
                }
                if (c == "+")
                {
                    n1.value[i] = f11 + f21;
                    continue;
                }
                if (c == "-")
                {
                    n1.value[i] = f11 - f21;
                    continue;
                }
                if (c == "*")
                {
                    n1.value[i] = f11 * f21;
                    continue;
                }
                if (c == "/")
                {
                    if (f21 == 0.0)
                        n1.value[i] = 0;
                    else
                        n1.value[i] = f11 / f21;
                }
            }
            return true;
        }

        public Boolean tj_x_n(string name, string[] xx, string type1)
        {
            TBian var11, x1;
            double f1, ma, avedev, devsq, n1, varp;
            int n, i, k, dlen;
            x1 = check(xx[0]);
            if (x1 == null)
            {
                showerror(xx[0] + "未定义!");
                return false;
            }
            n1 = 0.0f;
            if (teststr(xx[1], ref n1) == false)
            {
                showerror(xx[1] + "未定义!");
                return false;
            }
            n = Convert.ToInt32(n1) - 1;

            var11 = AddBian(name, x1.len);
            f1 = 0.0;
            ma = 0.0;
            dlen = x1.len;
            for (i = 0; i < dlen; i++)
            {
                f1 = f1 + x1.value[i];
                if (i >= n)
                {
                    f1 = f1 - x1.value[i - n];
                    ma = f1 / n;
                }
                else
                {
                    var11.value[i] = NA;
                    continue;
                }
                avedev = 0.0;
                devsq = 0.0;
                for (k = i; k > i - n + 1; k--)
                {
                    avedev = avedev + Math.Abs(x1.value[k] - ma);
                    devsq = devsq + (x1.value[k] - ma) * (x1.value[k] - ma);
                }
                varp = devsq / n;
                if (type1 == "std")
                    var11.value[i] = Math.Sqrt(devsq / (n - 1));
                else if ((type1 == "stdp"))
                    var11.value[i] = Math.Sqrt(devsq / n);
                else if ((type1 == "devsq"))
                    var11.value[i] = devsq;
                else if ((type1 == "avedev"))
                    var11.value[i] = avedev;
                else if (type1 == "var")
                    var11.value[i] = devsq / (n - 1);
                else if (type1 == "varp")
                    var11.value[i] = varp; //devsq/n
                else
                    break;
                if (i == n)
                {
                    for (k = n - 1; k > -1; k--)
                    {
                        var11.value[k] = var11.value[i];
                    }
                }
            }
            return true;
        }

        public Boolean a_b(string name, string[] xx, string type1)
        {
            int i, dlen;
            double fa, fb, fa1, fb1;
            TBian a, b, var11;

            a = check(xx[0]);
            fa = 0;
            if (a == null)
            {
                if (teststr(xx[0], ref fa) == false)
                {
                    ErrorString = "函数" + type1 + "参数:" + xx[0] + "未定义!";
                    return false;
                }
            }

            b = check(xx[1]);
            fb = 0;
            if (b == null)
            {
                if (teststr(xx[1], ref fb) == false)
                {
                    ErrorString = "函数" + type1 + "参数:" + xx[1] + "未定义!";
                    return false;
                }
            }
            dlen = RecordCount;
            var11 = AddBian(name, dlen);
            for (i = ft; i < dlen; i++)
            {
                if (a != null)
                    fa = a.value[i];
                if (b != null)
                    fb = b.value[i];
                if ((fa == NA) || (fb == NA))
                {
                    var11.value[i] = NA;
                    continue;
                }
                if (type1 == "pow")
                    var11.value[i] = Math.Pow(fa, fb);
                else if (type1 == "max")
                {
                    if (fa > fb)
                        var11.value[i] = fa;
                    else
                        var11.value[i] = fb;
                }
                else if (type1 == "min")
                {
                    if (fa > fb)
                        var11.value[i] = fb;
                    else
                        var11.value[i] = fa;
                }
                else if (type1 == "%")
                    var11.value[i] = Convert.ToInt32(fa) % Convert.ToInt32(fb);
                else if (type1 == "cross")
                {
                    if (i > 0)
                    {
                        fa1 = fa;
                        fb1 = fb;
                        if (a != null)
                            fa1 = a.value[i - 1];
                        if (b != null)
                            fb1 = b.value[i - 1];
                        if ((fa1 < fb1) && (fa > fb))
                            var11.value[i] = 1.0;
                        else
                            var11.value[i] = 0.0;
                    }
                    else
                        var11.value[0] = 0.0;
                }
            }
            return true;
        }

        public Boolean x(string name, string[] xx, string type1)
        {
            int i, k, dlen;
            double fa;
            TBian a, var11;
            type1 = type1.ToLower();
            fa = 0.0;
            a = check(xx[0]);
            if (a == null)
            {
                if (teststr(xx[0], ref fa) == false)
                {
                    ErrorString = "函数" + type1 + "参数:" + xx[0] + "未定义!";
                    return false;
                }
            }
            dlen = RecordCount;
            var11 = AddBian(name, dlen);

            for (i = 0; i < dlen; i++)
            {
                if (a != null)
                    fa = a.value[i];
                if (fa == NA)
                {
                    var11.value[i] = NA;
                    continue;
                }
                if (type1 == "sin")
                    var11.value[i] = Math.Sin(fa * Math.PI / 180);
                else if ((type1 == "abs"))
                    var11.value[i] = Math.Abs(fa);
                else if ((type1 == "cos"))
                    var11.value[i] = Math.Cos(fa * Math.PI / 180);
                else if ((type1 == "sqrt"))
                    var11.value[i] = Math.Sqrt(fa);
                else if ((type1 == "log"))
                    var11.value[i] = Math.Log10(fa);
                else if (type1 == "tan")
                    var11.value[i] = Math.Tan(fa * Math.PI / 180);
                else if (type1 == "exp")
                    var11.value[i] = Math.Exp(fa);
                else if ((type1 == "revrrse"))
                    var11.value[i] = -fa;
                else if ((type1 == "acos"))
                    var11.value[i] = Math.Acos(fa * Math.PI / 180);
                else if ((type1 == "asin"))
                    var11.value[i] = Math.Asin(fa * Math.PI / 180);
                else if ((type1 == "atan"))
                    var11.value[i] = Math.Atan(fa * Math.PI / 180);
                else if ((type1 == "ceiling"))
                    var11.value[i] = Math.Ceiling(fa);
                else if ((type1 == "floor"))
                    var11.value[i] = Math.Floor(fa);
                else if ((type1 == "intpart"))
                    var11.value[i] = Convert.ToInt32(fa);
                else if ((type1 == "not"))
                {
                    if (fa == 0.0)
                        var11.value[i] = 1.0;
                    else
                        var11.value[i] = 0.0;
                }
                else if ((type1 == "ln"))
                    var11.value[i] = Math.Log10(fa);
                else if ((type1 == "sgn"))
                {
                    if (fa > 0)
                        var11.value[i] = 1.0;
                    else
                        var11.value[i] = -1.0;
                    if ((fa == 0.0))
                        var11.value[i] = 0.0;
                }
                else if ((type1 == "barscount"))
                    var11.value[i] = i + 1;
                else if ((type1 == "barslast"))
                {
                    var11.value[i] = 0.0;
                    for (k = i - 1; k > -1; k--)
                    {
                        if ((a.value[k] > 0.0))
                        {
                            var11.value[i] = i - k;
                            break;
                        }
                    }
                }
                else if ((type1 == "barssince"))
                {
                    var11.value[i] = 0.0;
                    for (k = i - 1; k > -1; k--)
                    {
                        if ((a.value[k] > 0))
                        {
                            var11.value[i] = k;
                            break;
                        }
                    }
                }
            }
            return true;
        }



        private Boolean backset(string name, string func, string[] xx)
        {
            return x_n(name, xx, "backset");
        }
        private Boolean barscount(string name, string func, string[] xx)
        {
            return x(name, xx, "barscount");
        }
        private Boolean barssince(string name, string func, string[] xx)
        {
            return x(name, xx, "barssince");
        }
        private Boolean barslast(string name, string func, string[] xx)
        {
            return x(name, xx, "barslast");
        }
        private Boolean ceiling(string name, string func, string[] xx)
        {
            return x_n(name, xx, "barslast");
        }
        private Boolean count1(string name, string func, string[] xx)
        {
            return x_n(name, xx, "count");
        }
        private Boolean dma(string name, string func, string[] xx)
        {
            return x_n(name, xx, "dma");
        }
        private Boolean filter(string name, string func, string[] xx)
        {
            return x_n(name, xx, "filter");
        }
        private Boolean floor1(string name, string func, string[] xx)
        {
            return x(name, xx, "floor");
        }
        private Boolean intpart(string name, string func, string[] xx)
        {
            return x(name, xx, "intpart");
        }
        private Boolean hhvbars(string name, string func, string[] xx)
        {
            return true;
        }
        private Boolean llvbars(string name, string func, string[] xx)
        {
            return true;
        }
        private Boolean range(string name, string func, string[] xx)
        {
            return true;
        }
        private Boolean ema(string name, string func, string[] xx)
        {
            return x_n(name, xx, "ema");
        }
        private Boolean expmema(string name, string func, string[] xx)
        {
            return x_n(name, xx, "expmema");
        }
        private Boolean abs1(string name, string func, string[] xx)
        {
            return x(name, xx, "abs");
        }
        private Boolean acos1(string name, string func, string[] xx)
        {
            return x(name, xx, "acos");
        }
        private Boolean asin1(string name, string func, string[] xx)
        {
            return x(name, xx, "asin");
        }
        private Boolean atan1(string name, string func, string[] xx)
        {
            return x(name, xx, "atan");
        }
        private Boolean avedev(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "avedev");
        }
        private Boolean cos1(string name, string func, string[] xx)
        {
            return x(name, xx, "cos");
        }
        private Boolean cross(string name, string func, string[] xx)
        {
            return a_b(name, xx, "cross");
        }
        private Boolean devsq(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "devsq");
        }
        private Boolean exp1(string name, string func, string[] xx)
        {
            return x(name, xx, "exp");
        }
        private Boolean hhv(string name, string func, string[] xx)
        {
            return x_n(name, xx, "hhv");
        }
        private Boolean llv(string name, string func, string[] xx)
        {
            return x_n(name, xx, "llv");
        }
        private Boolean ln1(string name, string func, string[] xx)
        {
            return x(name, xx, "ln");
        }
        private Boolean ma(string name, string func, string[] xx)
        {
            return x_n(name, xx, "ma");
        }
        private Boolean max1(string name, string func, string[] xx)
        {
            return a_b(name, xx, "max");
        }
        private Boolean min1(string name, string func, string[] xx)
        {
            return a_b(name, xx, "min");
        }
        private Boolean mod1(string name, string func, string[] xx)
        {
            return a_b(name, xx, "mod");
        }
        private Boolean not1(string name, string func, string[] xx)
        {
            return x(name, xx, "not");
        }
        private Boolean pow1(string name, string func, string[] xx)
        {
            return a_b(name, xx, "pow");
        }
        private Boolean ref1(string name, string func, string[] xx)
        {
            return x_n(name, xx, "ref");
        }
        private Boolean revrrse(string name, string func, string[] xx)
        {
            return x(name, xx, "revrrse");
        }
        private Boolean sgn1(string name, string func, string[] xx)
        {
            return x(name, xx, "sgn");
        }
        private Boolean sin1(string name, string func, string[] xx)
        {
            return x(name, xx, "sin");
        }
        private Boolean std(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "std");
        }
        private Boolean stdp(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "stdp");
        }
        private Boolean sum(string name, string func, string[] xx)
        {
            return x_n(name, xx, "sum");
        }
        private Boolean sumbars(string name, string func, string[] xx)
        {
            return x_n(name, xx, "sumbars");
        }

        private Boolean tan1(string name, string func, string[] xx)
        {
            return x(name, xx, "tan");
        }

        private Boolean var11(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "var");
        }

        private Boolean varp(string name, string func, string[] xx)
        {
            return tj_x_n(name, xx, "varp");
        }

        private Boolean log1(string name, string func, string[] xx)
        {
            return x(name, xx, "log");
        }

        private Boolean sqrt1(string name, string func, string[] xx)
        {
            return x(name, xx, "sqrt");
        }

        private Boolean between(string name, string func, string[] xx)
        {
            int i, dlen;
            TBian b11, b21, b31;
            double ff1, ff2, ff3;
            Boolean b1, b2, b3;
            TBian var1;
            ff1 = ff2 = ff3 = 0.0f;
            b1 = teststr(xx[0], ref ff1);
            b2 = teststr(xx[1], ref ff2);
            b3 = teststr(xx[2], ref ff3);
            b11 = GetBian(xx[0]);
            b21 = GetBian(xx[0]);
            b31 = GetBian(xx[0]);
            if (((b1 == false) && (b11 == null)) || ((b2 == false) && (b21 == null)) || ((b3 = false) && (b31 == null)))
            {
                return false;
            }
            dlen = RecordCount;
            var1 = AddBian(name, dlen);
            for (i = ft; i < dlen; i++)
            {
                var1.value[i] = 0;
                if (!b1)
                    ff1 = b11.value[i];
                if (!b2)
                    ff2 = b21.value[i];
                if (!b3)
                    ff3 = b31.value[i];
                if ((ff2 > ff1) && (ff1 > ff3))
                    var1.value[i] = 1;
                if ((ff2 < ff1) && (ff1 < ff3))
                    var1.value[i] = 1;
            }
            return true;
        }

        private Boolean if1(string name, string func, string[] xx)
        {
            int i, f1, f2, dlen;
            double f11, f21, fa, fb;
            TBian x1, a, b, var11;

            x1 = check(xx[0]);
            if (x1 == null)
            {
                showerror("IF参数错误！");
                return false;
            }
            f1 = 0;
            f2 = 0;
            f11 = 0.0;
            f21 = 0.0;
            if (teststr(xx[1], ref f11))
                f1 = 1;
            if (teststr(xx[2], ref f21))
                f2 = 1;
            a = check(xx[1]);
            b = check(xx[2]);
            if ((a != null))
                f1 = 2;
            if ((b != null))
                f2 = 2;
            if ((f1 == 0) || (f2 == 0))
            {
                showerror("IF 参数不存在!");
                return false;
            }
            dlen = RecordCount;
            var11 = AddBian(name, dlen);
            for (i = ft; i < dlen; i++)
            {
                fa = f11;
                fb = f21;
                if (f1 == 2)
                    fa = a.value[i];
                if (f2 == 2)
                    fb = b.value[i];
                if ((x1.value[i] != 0))
                    var11.value[i] = fa;
                else
                    var11.value[i] = fb;
            }
            return true;
        }

        private Boolean input(string name, string func, string[] xx)
        {
            Tinput it = new Tinput();
            it.name = name;
            it.min1 = Convert.ToInt32(xx[0]);
            it.max1 = Convert.ToInt32(xx[1]);
            it.def1 = Convert.ToInt32(xx[2]);
            it.val1 = Convert.ToInt32(xx[3]);
            if (xx.Length > 4)
            {
                if (xx[4].Length > 0)
                    it.notes = xx[4];
            }
            if (CurTech.param[CurTech.Input.Count] > 0)
                it.val1 = CurTech.param[CurTech.Input.Count];
            CurTech.ls.Add(name + "=" + it.val1.ToString());
            CurTech.Input.Add(it);
            return true;
        }

        private Boolean sma(string name, string func, string[] xx)
        {
            TBian sma1, x1;
            double n1;
            int i, dlen, n = 0, m;
            // y=[m*x+(n-m)*y")/n
            x1 = check(xx[0]);
            if (x1 == null)
            {
                showerror("SMA 参数错误!");
                return false;
            }
            n1 = 0;
            TBian nn = check(xx[1]);
            if (nn == null)
            {
                if ((teststr(xx[1], ref n1) == false))
                {
                    showerror(xx[1] + "没有定义!");
                    return false;
                }
                n = (int)(n1);
            }
            if ((teststr(xx[2], ref n1) == false))
            {
                showerror(xx[1] + "没有定义!");
                return false;
            }
            m = (int)(n1);
            dlen = RecordCount;
            sma1 = AddBian(name, x1.len);
            for (i = ft; i < dlen; i++)
            {
                if (x1.value[i] == NA)
                {
                    sma1.value[i] = NA;
                    continue;
                }
                if (nn != null)
                    n = Convert.ToInt32(nn.value[i]);
                if (n == NA)
                {
                    sma1.value[i] = NA;
                    continue;
                }
                if (i == 0)
                    sma1.value[i] = x1.value[i];
                else
                {
                    if (sma1.value[i - 1] == NA)
                        sma1.value[i] = x1.value[i];
                    else
                        sma1.value[i] = (m * x1.value[i] + (n - m) * sma1.value[i - 1]) / n;
                }
            }
            return true;
        }

        private Boolean dynainfo(string name, string func, string[] xx)
        {
            double d1 = NA;
            if (teststr(xx[0], ref d1) == false)
            {
                showerror("DYNAINFO(N)参数N:" + xx[0] + "未定义!");
                return false;
            }
            if (Symbol == null)
            {
                showerror("DYNAINFO(N)需要设置 StockInfo!");
                return false;
            }
            TDX Value = Symbol.TickSnapshot;
            double[] dynainfo = new double[24];


            dynainfo[3] = Value.last;
            dynainfo[4] = Value.open;
            dynainfo[5] = Value.high;
            dynainfo[6] = Value.low;
            dynainfo[7] = Value.prize;
            dynainfo[8] = Value.volume;
            dynainfo[9] = Value.tradeQTY;
            dynainfo[12] = Value.prize - Value.last;
            dynainfo[13] = 0;
            dynainfo[14] = 0;
            if (Value.low != 0)
                dynainfo[13] = (Value.high - Value.low) / Value.low;
            if (Value.last != 0)
                dynainfo[14] = (Value.prize - Value.last) * 100 / Value.last;
            dynainfo[20] = Value.buy1;
            dynainfo[21] = Value.sell1;
            dynainfo[22] = Value.b;
            dynainfo[23] = Value.s;

            int id = Convert.ToInt32(d1);
            if ((id < 0) || (id > 24))
            {
                showerror("'DYNAINFO(N)参数N:" + xx[0] + "不在0~23之间!");
                return false;
            }
            d1 = dynainfo[id];
            String s1 = name + "=" + d1.ToString("F4");
            CurTech.ls.Add(s1);
            return true;
        }

        private Boolean finance(string name, string func, string[] xx)
        {
            double d1 = NA;
            if (teststr(xx[0], ref d1) == false)
            {
                showerror("finance(N)参数N:" + xx[0] + "未定义!");
                return false;
            }
            if (Symbol == null)
            {
                showerror("finance(N)需要设置 StockInfo!");
                return false;
            }
            double[] finance = new double[64];
            FinanceData Value = Symbol.FinanceData;

            finance[0] = Value.zl[0] * 10000;
            finance[0] = Value.zl[0] * 10000;
            finance[5] = Value.zl[4] * 10000;
            finance[7] = Value.LTG * 10000;
            finance[10] = Value.zl[7] * 1000;
            finance[11] = Value.zl[8] * 1000;
            finance[12] = Value.zl[9] * 1000;
            finance[13] = Value.zl[10] * 1000;
            finance[14] = Value.zl[11] * 1000;
            finance[15] = Value.zl[12] * 1000;
            finance[16] = Value.zl[13] * 1000;
            finance[17] = Value.zl[14] * 10000;
            finance[18] = Value.zl[14] * 10000 / Value.zl[0];
            finance[19] = Value.zl[15] * 1000;
            finance[20] = Value.zl[16] * 1000;
            finance[21] = Value.zl[17] * 1000;
            finance[22] = Value.zl[18] * 1000;
            finance[23] = Value.zl[19] * 1000;
            finance[24] = Value.zl[20] * 1000;
            finance[25] = Value.zl[21] * 1000;
            finance[26] = Value.zl[22] * 1000;
            finance[27] = Value.zl[23] * 1000;
            finance[28] = Value.zl[24] * 1000;
            finance[29] = Value.zl[25] * 1000;
            finance[30] = Value.zl[26] * 1000;
            finance[31] = Value.zl[27] * 1000;
            finance[32] = Value.zl[27] * 1000 / Value.zl[0];
            finance[33] = Value.zl[15] / Value.zl[0] / 10 / Value.zl[29] * 12;
            finance[34] = Value.zl[19] * 1000 / Value.zl[0];
            finance[40] = Symbol.TickSnapshot.prize * finance[7];
            finance[41] = Symbol.TickSnapshot.prize * finance[0];
            finance[42] = Value.day1;

            int id = Convert.ToInt32(d1);
            if ((id < 0) || (id > 64))
            {
                showerror("'finance(N)参数N:" + xx[0] + "不在0~23之间!");
                return false;
            }
            d1 = finance[id];
            String s1 = name + "=" + d1.ToString("F4");
            CurTech.ls.Add(s1);
            return true;
        }

        private Boolean drawline1(string name, string func, string[] xx)
        {
            bool result = false;
            string s1;
            int i, j, k, t, dlen;
            double v1, v2, v3, v4, ex;
            TBian var1, cd1, pr1, cd2, pr2;
            v1 = 0;
            v2 = 0;
            v3 = 0;
            v4 = 0;
            cd1 = check(xx[0]);
            if (cd1 == null)
            {
                if (teststr(xx[0], ref v1) == false)
                {
                    showerror("DRAWLINE(X1,V1,X2,V2,EX)参数X1:" + xx[0] + "没定义!");
                    return result;
                }
            }
            pr1 = check(xx[1]);
            if (pr1 == null)
            {
                if (teststr(xx[1], ref v2) == false)
                {
                    showerror("DRAWLINE(X1,V1,X2,V2,EX)参数v1:" + xx[1] + "没定义!");
                    return result;
                }
            }
            cd2 = check(xx[2]);
            if (cd2 == null)
            {
                if (teststr(xx[2], ref v3) == false)
                {
                    showerror("DRAWLINE(X1,V1,X2,V2,EX)参数X2:" + xx[2] + "没定义!");
                    return result;
                }
            }
            pr2 = check(xx[3]);
            if (pr2 == null)
            {
                if (teststr(xx[3], ref v4) == false)
                {
                    showerror("DRAWLINE(X1,V1,X2,V2,EX)参数V2:" + xx[3] + "没定义!");
                    return result;
                }
            }
            ex = 0;
            if (teststr(xx[4], ref ex) == false)
            {
                showerror("DRAWLINE(X1,V1,X2,V2,EX)参数EX:" + xx[4] + "没定义!");
                return result;
            }
            if (name.IndexOf('@') == 0)
            {
                s1 = "syline=" + name;
                for (i = 0; i < 19; i++)
                {
                    if (xx[i].IndexOf("::") > -1)
                    {
                        s1 += "::";
                        break;
                    }
                }
                CurTech.outline.Add(s1);
            }
            dlen = RecordCount;
            var1 = AddBian(name, dlen);
            t = dlen - 1;
            i = dlen - 1;
            while (i > 0)
            {
                if (cd2 != null)
                    v3 = cd2.value[i];
                if (v3 < 1)
                {
                    i--;
                    continue;
                }
                else
                {
                    for (j = i - 1; j > -1; j--)
                    {
                        if (cd1 != null)
                            v1 = cd1.value[j];
                        if (v1 > 0)
                        {
                            v2 = pr1.value[j];
                            if (pr2 != null)
                                v4 = pr2.value[i];
                            for (k = j; k <= i; k++)
                                var1.value[k] = v2 + (v4 - v2) * (k - j) / (i - j);
                            if ((t > i) && (ex == 1))
                            {
                                for (k = i; k < t; k++)
                                    var1.value[k] = v2 + (v4 - v2) * (k - j) / (i - j);
                            }
                            t = j - 1;
                            break;
                        }
                    }
                    i = j;
                }
            }
            return true;
        }

        private Boolean draw(string name, string func, string[] xx)
        {
            string s1;
            int i;
            s1 = func.ToUpper() + "=" + xx[0];
            for (i = 1; i < xx.Length; i++)
            {
                if (xx[i] != "")
                    s1 = s1 + "," + xx[i];
                else
                    break;
            }
            CurTech.outline.Add(s1);
            return true;
        }
    }
}
