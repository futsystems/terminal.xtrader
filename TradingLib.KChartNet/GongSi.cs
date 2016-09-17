
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
using Common.Logging;
using TradingLib.MarketData;

namespace CStock
{
    public partial class TGongSi
    {

        ILog logger = LogManager.GetLogger("ChartPanel");
        double NA =double.MinValue;// -10000000.0;
        public static int SpaceWidth = 50;
        public string ErrorString;
        public TFunclist func;
       
        public List<TBian> mainbian;//保留原始数据
        //public Stock StockInfo=null;

        public MDSymbol Symbol = null;

        public List<QuanInfo> QuanInfo = new List<QuanInfo>();//权息集合表
        public QuanType QuanStyle = QuanType.qsNone;//不复权


        /// <summary>
        /// 技术参数
        /// </summary>
        public class tech
        {
            public string techname = "";//公式名称
            public string techtitle = "";
            public string password = "";
            public string filename = "";//公式文件名
            public TStringList outline = new TStringList();//输出数据
            public TStringList ls = new TStringList();//常数
            public TStringList pg = new TStringList();//原始公式
            public TStringList pg1 = new TStringList();//编译后的公式
            public List<TBian> namebian = new List<TBian>();//变量
            public List<Tinput> Input = new List<Tinput>();//可调参数
            public int[] param = new int[20];//参数
        }


        public List<tech> TechList = new List<tech>();//公式集合
        public tech CurTech = null;//当前公式

        public System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TGongSi));

        public MarkType marktype = MarkType.MarkA;//默认A股市

        public List<infolist> DL;
        string stkname, stklabel, week;
        public string ffilename;

        public string ftechname;
        public string ftechtitle;
        public string fpassword;
        public int showk;
        public string version;
        public string yStr, yValue;
        public int weilen;// 品种小数位数;//品种小数位数
        public int digit;


        public Boolean training = false;
        public int TrainEnd = 0;//最后位置


        Boolean comm, main;
        Boolean fsall, fsfull;
        Boolean showfs, showtop, showleft, showright, showbottom;
        public Boolean percent10;
        double prevclose;
        Boolean cursor = true, high, curgs;
        int ft;

        public int days = 1;//多日分时
        public int[] dayslist = new int[10];

        /// <summary>
        /// 默认字体
        /// </summary>
        Font font = new Font("宋体", 9);
        /// <summary>
        /// 默认Brush
        /// </summary>
        SolidBrush FBrush = new SolidBrush(Color.Black);
        /// <summary>
        /// 默认Pen
        /// </summary>
        Pen pen = new Pen(Color.Red, 1);

        public Color LineColor = Color.Maroon;
        public Color BackColor = Color.Black;

        public int leftYAxisWidth, rightYAxisWidth;

        TStock pCtrl = null;

        int _startIndex = -1;
        /// <summary>
        /// 当前窗口的K开始位置
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }

        int _endIndex = -1;
        /// <summary>
        /// 当前窗口的K结束位置
        /// </summary>
        public int EndIndex
        {
            get { return _endIndex; }
            private set { _endIndex = value; }
        }


        /// <summary>
        /// 当前光标所在位置数值
        /// </summary>
        public double FCurValue;
        /// <summary>
        /// 当前光标所在K线位置 
        /// </summary>
        public int FCurBar;
        /// <summary>
        /// 当前窗口显示的K线数量
        /// </summary>
        public int FCurWidth;
        public int FCurXX=0, FCurYY=0;

        public bool Focused = false;//焦点

        public int toph, both;
        
        public Rectangle Bounds=new Rectangle();
        public double MaxValue, MinValue;//窗口内最大价,最小价
        public bool showline=true;//是否显示自画线
        public List<XLine> Lines = new List<XLine>();
        public XLine CurLine = null;



        int curx, cury;
        public double FScale = 8;//默认值 8
        public String StkName
        {
            get { return stkname; }
            set
            {
                stkname = value;
            }
        }

        /// <summary>
        /// 合约名称
        /// </summary>
        public string SymbolName
        {
            get { return stkname; }
        }

        /// <summary>
        /// 合约代码
        /// </summary>
        public string SymbolCode
        {
            get { return stklabel; }
        }

        /// <summary>
        /// 数据周期名称
        /// </summary>
        public string DataCycleName
        {
            get { return "周期" + this.week; }
        }


        public String FuncList()
        {
            return func.GetFuncList();
        }
        public string StkCode
        {
            get { return stklabel; }
            set { stklabel = value; }
        }
        public string StkWeek
        {
            get { return week; }
            set { week = value; }
        }
        public Boolean ShowCursor
        {
            get { return cursor; }
            set { cursor = value; }
        }
        public double PreClose
        {
            get { return prevclose; }
            set { prevclose = value; }
        }
        public Boolean Main
        {
            get { return main; }
            set { main = value; }
        }

        public string ProgramFile
        {
            get { return ffilename; }
            set { this.loadprogram(value); }
        }

        /// <summary>
        /// 数据开始位置
        /// </summary>
        //public int LeftBar
        //{
        //    get { return StartIndex; }
        //    set { StartIndex = value; }
        //}

        /// <summary>
        /// 分时等高
        /// </summary>

        public Boolean FsAll
        {
            get { return fsall; }
            set { fsall = value; }
        }
        public Boolean FsFull
        {
            get { return fsfull; }
            set { fsfull = value; }
        }
        public Boolean ShowFs
        {
            get { return showfs; }
            set { showfs = value; }
        }
        private bool showcurwindow=false;
        public Boolean ShowCurWindow
        {
            get { return showcurwindow; }
            set { showcurwindow = value; }
        }

        public Boolean CurWindow
        {
            get { return curgs; }
            set { curgs = value; }
        }
        public Boolean ShowLeft
        {
            get { return showleft; }
            set { showleft = value; }
        }
        public Boolean ShowTop
        {
            get { return showtop; }
            set { showtop = value; }
        }
        public Boolean ShowRight
        {
            get { return showright; }
            set { showright = value; }
        }
        public Boolean ShowBottom
        {
            get { return showbottom; }
            set { showbottom = value; }
        }
        public Boolean High
        {
            get { return high; }
            set { high = value; }
        }

        public void SetCurxy(int Fcurx, int Fcury)
        {
            curx = Fcurx;
            cury = Fcury;
        }
        /// <summary>
        /// 加载内置公式
        /// </summary>
        /// <param name="wfc"></param>
        /// <returns></returns>
        public Boolean LoadWfc(string wfc)
        {
            FuncStr fs = new FuncStr();
            string s1 = fs.Get(wfc);
            if (s1.Length == 0)
                return false;
            ffilename = wfc.ToUpper() + ".WFC";
            return setprogtext(s1);
        }
        
        /// <summary>
        /// 主图初始化
        /// </summary>
        public TGongSi(TStock ctrl=null)
        {
            pCtrl = ctrl;
            mainbian = new List<TBian>();
            initdata();
        }
        /// <summary>
        /// 副图初始化--共用数据(作副图使用,内存数据共用,减少内存占用)
        /// </summary>
        /// <param name="gs"></param>
        public TGongSi(TGongSi gs, TStock ctrl = null)//List<TBian> maindata)
        {
            pCtrl = ctrl;
            if (gs.mainbian != null)
            {
                mainbian = gs.mainbian;
                Symbol = gs.Symbol; 
                comm = true;
            }
            else
            {
                mainbian = new List<TBian>();
                Symbol = null;
            }
            initdata();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void initdata()
        {
            func = new TFunclist();
            func.AddSub("year", null);
            func.AddSub("month", null);
            func.AddSub("day", null);
            func.AddSub("week", null);
            func.AddSub("hour", null);
            func.AddSub("minute", null);
            func.AddSub("fromopen", null);
            func.AddSub("high", null);
            func.AddSub("open", null);
            func.AddSub("close", null);
            func.AddSub("low", null);
            func.AddSub("vol", null);

            func.AddSub("ema", ema);
            func.AddSub("expma", ema);
            func.AddSub("expmema", expmema);
            func.AddSub("hhv", hhv);
            func.AddSub("llv", llv);
            func.AddSub("ma", ma);
            func.AddSub("sma", sma);
            func.AddSub("ref", ref1);
            func.AddSub("if", if1);
            func.AddSub("var", var11);
            func.AddSub("varp", varp);
            func.AddSub("std", std);
            func.AddSub("stdp", stdp);
            func.AddSub("devsq", devsq);
            func.AddSub("avedev", avedev);
            func.AddSub("max", max1);
            func.AddSub("min", min1);
            func.AddSub("sum", sum);
            func.AddSub("pow", pow1);
            func.AddSub("mod", mod1);
            func.AddSub("cross", cross);
            func.AddSub("sin", sin1);
            func.AddSub("abs", abs1);
            func.AddSub("cos", cos1);
            func.AddSub("sqrt", sqrt1);
            func.AddSub("log", log1);
            func.AddSub("tan", tan1);
            func.AddSub("exp", exp1);
            func.AddSub("revrrse", revrrse);
            func.AddSub("acos", acos1);
            func.AddSub("sgn", sgn1);
            func.AddSub("ln", ln1);
            func.AddSub("not", not1);
            func.AddSub("atan", atan1);
            func.AddSub("asin", asin1);
            func.AddSub("ployline", draw);
            func.AddSub("drawkline", draw);
            func.AddSub("drawicon", draw);
            func.AddSub("drawtext", draw);

            func.AddSub("partline", draw);
            func.AddSub("colorstick", draw);
            func.AddSub("volstick", draw);
            func.AddSub("stickline", draw);
            func.AddSub("twr", draw);
            func.AddSub("btx", draw);
            func.AddSub("box", draw);

            func.AddSub("backset", backset);
            func.AddSub("barscount", barscount);
            func.AddSub("barslast", barslast);
            func.AddSub("barssince", barssince);
            func.AddSub("count", count1);
            func.AddSub("hhvbars", hhvbars);
            func.AddSub("llvbars", llvbars);
            func.AddSub("filter", filter);
            func.AddSub("sumbars", sumbars);
            func.AddSub("range", range);
            func.AddSub("ceiling", ceiling);
            func.AddSub("floor", floor1);
            func.AddSub("intpart", intpart);
            func.AddSub("between", between);
            func.AddSub("input", input);
            func.AddSub("drawline", drawline1);
            func.AddSub("dynainfo", dynainfo);
            func.AddSub("finance", finance);
            ft = 0;
            prevclose = NA;

            ftechname = "";
            ftechtitle = "";
            fpassword = "";
            yStr = "";
            yValue = "";
            digit = 0;

            stkname = "";
            stklabel = "";
            week = "";

            showfs = false;

            showtop = true;
            showbottom = true;
            showleft = true;
            showright = false;

            fsall = false;
            fsfull = false;
            StartIndex = 0;
            cursor = false;
            comm = false;
            showk = -1;
            main = false;
            curgs = true;
            high = false;
            curx = -1;
            cury = -1;
            percent10 = false;

            TechList.Add(new tech());
            CurTech = TechList[0];

        }


       


        

        public string getprogtext()
        {
            return TechList[0].pg.Text;
        }
        /// <summary>
        /// 加载公式字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// 
        /// 
        public Boolean setprogtext(string text)
        {
          return  setprogtext(text, "");
        }
        public Boolean setprogtext(string text,string paramstr)
        {
            TStringList src, dst;
            Boolean result;
            Compile cp = new Compile();
            if ((text == null) || (text.Length == 0))
            {
                MessageBox.Show("程序长度不能为空!", "信息窗口", MessageBoxButtons.OK);
                return false;
            }
            ffilename = "";
            src = new TStringList();
            dst = new TStringList();
            src.Text = text;
            result = cp.S_Compile(src, dst);
            if ((result) && (dst.Count > 0))
            {
                clear();
                TechList[0].pg.Text = src.Text;
                TechList[0].pg1.Text = dst.Text;
                ft = 0;
                result = run(paramstr);
            }
            return result;
        }
        /// <summary>
        /// 加载公式文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Boolean loadprogram(string filename)
        {
            TStringList src, dst;
            Compile cp = new Compile();
            Boolean result;
            src = new TStringList();//   tstringlist.create;
            src.LoadFromFile(filename);
            if (src.Count == 0)
                return false;
            dst = new TStringList();
            result = cp.S_Compile(src, dst);
            if ((result) && (dst.Count > 0))
            {
                TechList[0].filename = filename.ToUpper();
                TechList[0].pg.Text = src.Text;
                TechList[0].pg1.Text = dst.Text;
                clear();
                ft = 0;
                result=run();
            }
            return result;
        }
        /// <summary>
        /// 叠加字符串公式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool AddTech(string str)
        {

            TStringList src, dst;
            Boolean result;
            Compile cp = new Compile();
            if ((str == null) || (str.Length == 0))
            {
                ErrorString = "程序长度不能为空!";
                MessageBox.Show(ErrorString, "信息窗口", MessageBoxButtons.OK);
                return false;
            }
            ffilename = "";
            src = new TStringList();
            dst = new TStringList();
            src.Text = str;
            result = cp.S_Compile(src, dst);
            if ((result) && (dst.Count > 0))
            {
                result =Compile.CheckGongSi(src.Text, ref ErrorString);
                if (result)
                {
                    clear();
                    tech th = new tech();
                    th.pg.Text = src.Text;
                    th.pg1.Text = dst.Text;
                    TechList.Add(th);
                    ft = 0;
                    run();
                }
            }
            return result;
        }


        /// <summary>
        /// 清除叠加公式
        /// </summary>
        public void ClearTech()
        {
            if (TechList.Count > 1)
                TechList.RemoveRange(1, TechList.Count - 1);
        }

        


        /// <summary>
        /// 绘图区域显示设置
        /// 顶部 值输出
        /// 左边 坐标轴
        /// 右侧 坐标
        /// 底部 日期坐标
        /// </summary>
        /// <param name="sleft"></param>
        /// <param name="stop"></param>
        /// <param name="sright"></param>
        /// <param name="sbottom"></param>
        public void SetType(Boolean sleft, Boolean stop, Boolean sright, Boolean sbottom)
        {
            showleft = sleft;
            showtop = stop;
            showright = sright;
            showbottom = sbottom;
        }

        

        
        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        //public TBian addall(string name, double[] f1, int len)
        //{
        //    TBian B1 = null;
        //    name = name.ToLower();
        //    B1 = GetBian(name);
        //    if (B1 == null)
        //    {
        //        //B1 = new TBian(name, len);
        //        //B1.SetDouble(f1, len, false);
        //        B1 = new TBian(name,0);
        //        B1.BInsert(f1, len);
        //        mainbian.Add(B1);
        //    }
        //    else
        //    {
        //        //B1.SetDouble(f1, len, true);
        //        B1.BInsert(f1, len);
        //    }
        //    return B1;
        //}

        


        private void showerror(string p)
        {
            MessageBox.Show(p, "信息窗口", MessageBoxButtons.OK);
        }

        /// <summary>
        /// 添加一个新数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f1"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        //public TBian addone(string name, double f1, Boolean app)
        //{
        //    TBian b1;

        //    name = name.ToLower();
        //    b1 = GetBian(name);
        //    if (b1 == null)
        //    {
        //        b1 = new TBian(name, 0);
        //        mainbian.Add(b1);
        //    }
        //    b1.AddOne(f1, app);
        //    return b1;
        //}



        //public TBian AppendValue(string name, double val)
        //{
        //    TBian data = GetBian(name.ToLower());
        //    if (data == null)
        //    {
        //        data = new TBian(name, 0);
        //        mainbian.Add(data);
        //    }
        //    data.AppendValue(val);

        //    return data;
        //}

        

        public void SaveToTdx(string filename)
        {
            SaveToTdx(filename, true); 
        }
        public void SaveToTdx(string filename,bool b1)
        {
            int len = RecordCount;
            if (len == 0)
                return;
            TStringList sl = new TStringList();
            String s1 = "序号,";
            for (int i = 0; i < mainbian.Count; i++)
                s1 = s1 + mainbian[i].name.ToLower() + ",";
            tech th = TechList[0];

            for (int i = 0; i < th.namebian.Count; i++)
            {
                if ((th.namebian[i].name[0] == '@') && b1)
                    continue;
                s1 = s1 + th.namebian[i].name.ToLower() + ",";
            }
            s1 = s1.Substring(0, s1.Length - 1);
            sl.Add(s1);

            for (int i = 0; i < len; i++)
            {
                s1 = i.ToString() + ",";
                for (int j = 0; j < mainbian.Count; j++)
                    s1 += mainbian[j].value[i].ToString("F3") + ",";
                for (int j = 0; j < th.namebian.Count; j++)
                {
                    if ((th.namebian[j].name[0] == '@') && b1)
                        continue;
                    double d1=th.namebian[j].value[i];
                    if (d1!=NA)
                    s1 = s1 +d1.ToString("F3") + ",";
                    else
                        s1 = s1 + "_,";
                }

                s1 = s1.Substring(0, s1.Length - 1);
                sl.Add(s1);
            }
            sl.SaveToFile(filename);
        }


        #region 清除数据
        public void clear() //清除所有临时数据,保留原始数据
        {
            for (int i = 0; i < TechList.Count; i++)
            {
                TechList[i].outline.Clear();
                TechList[i].namebian.Clear();
                TechList[i].ls.Clear();
            }
            showk = -1;
            ffilename = "";
            ftechname = "";
            ftechtitle = "";
            fpassword="";
            yStr="";
            yValue="";
            digit = 0;
        }
        public void cleardata() //清除所有数据
        {
            if (comm == false)
                mainbian.Clear();
            for (int i = 0; i < TechList.Count; i++)
            {
                TechList[i].outline.Clear();
                TechList[i].namebian.Clear();
                TechList[i].ls.Clear();
            }
            //QuanInfo.Clear();
            //QuanStyle = QuanType.qsNone;
            //PreClose = NA;
            Symbol = null;
            DL = null;
            showk = -1;
            ftechname = "";
            ftechtitle = "";
            fpassword = "";
            yStr = "";
            yValue = "";
            digit = 0;
        }
        #endregion



        /// <summary>
        /// 使用样式
        /// </summary>
        /// <param name="wei"></param>
        /// <returns></returns>
        public Boolean userwei(string wei)
        {
            string s1;
            int i;
            string[] ss;
            pen.DashStyle = DashStyle.Solid;
            pen.Color = Color.White;
            pen.Width = 1F;
            ss = wei.ToUpper().Split(',');
            for (i = 0; i < ss.Length; i++)
            {
                s1 = ss[i].Trim();//  fenjie(wei, ',', i);
                if (s1 == null)
                    break;
                if (s1 == "LINEDASH")
                    pen.DashStyle = DashStyle.Dash;
                if (s1 == "LINEDOT")
                    pen.DashStyle = DashStyle.Dot;
                if (s1 == "LINEDASHDOT")
                    pen.DashStyle = DashStyle.DashDot;
                if (s1 == "LINEDASHDOTDOT")
                    pen.DashStyle = DashStyle.DashDotDot;
                if (s1.IndexOf("COLOR") > -1)
                {
                    pen.Color = GetColor(s1);
                    FBrush.Color = GetColor(s1);
                }
                if (s1.IndexOf("LINETHICK") == 0)
                {
                    i = Convert.ToInt32(s1.Substring(9, 1));
                    pen.Width = i;
                }

            }
            return true;
        }

       






        /// <summary>
        /// 设置除权类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetQuanStyle(QuanType value)
        {
            if ((QuanInfo.Count == 0) || (RecordCount == 0))
            {
                QuanStyle = value;
                return false;
            }
            TBian d1 = check("Date");
            TBian b1 = check("high");
            TBian b2 = check("low");
            TBian b3 = check("close");
            TBian b4 = check("open");
            if ((d1 == null) || (b1 == null) || (b2 == null) || (b3 == null) || (b4 == null))
            {
                QuanStyle = value;
                return false;
            }

            TBian bb1 = check("q_high");
            TBian bb2 = check("q_low");
            TBian bb3 = check("q_close");
            TBian bb4 = check("q_open");
            if (bb1 == null)
            {
                bb4 = AddBian("q_open", b4.len);
                bb4.SetBian(b4);
                bb1 = AddBian("q_high", b1.len);
                bb1.SetBian(b1);
                bb2 = AddBian("q_low", b2.len);
                bb2.SetBian(b2);
                bb3 = AddBian("q_close", b3.len);
                bb3.SetBian(b3);
            }
            else
            { // 先还原用于计算
                b1.SetBian(bb1);
                b2.SetBian(bb2);
                b3.SetBian(bb3);
                b4.SetBian(bb4);
            }
            double f1, f2, v1, v2;
            if (value == QuanType.qsBefore)  // 向前复权
            {
                double vv1 = 0, vv2 = 1;
                int k = QuanInfo.Count - 1;
                int day1 = -1;
                int len = RecordCount;
                for (int i = len - 1; i > -1; i--)
                {
                    day1 = (int)(d1.value[i]);
                    if (k > -1)
                    {
                        if (day1 < QuanInfo[k].Date)
                        {
                            f1 = 0 - QuanInfo[k].Money / 10 + QuanInfo[k].PeiMoney * QuanInfo[k].PeiNumber / 10;
                            f2 = QuanInfo[k].Number / 10 + QuanInfo[k].PeiNumber / 10;
                            vv2 *= (1 + f2);
                            vv1 = f1 + vv1 * (1 + f2);
                            k -= 1;
                        }
                    }
                    v1 = b1.value[i];
                    v2 = (v1 + vv1) / vv2;
                    b1.value[i] = v2;
                    v1 = b2.value[i];
                    v2 = (v1 + vv1) / vv2;
                    b2.value[i] = v2;
                    v1 = b3.value[i];
                    v2 = (v1 + vv1) / vv2;
                    b3.value[i] = v2;
                    v1 = b4.value[i];
                    v2 = (v1 + vv1) / vv2;
                    b4.value[i] = v2;
                }
            }
            if (value == QuanType.qsBack)  // 向后复权
            {
                int len = RecordCount;
                double vv1 = 0, vv2 = 1;
                int k = 0;
                for (int i = 0; i < len; i++)
                {
                    int day = (int)(d1.value[i]);
                    if (k < QuanInfo.Count)
                    {
                        if (day >= QuanInfo[k].Date)
                        {
                            f1 = 0 - QuanInfo[k].Money / 10 + QuanInfo[k].PeiMoney * QuanInfo[k].PeiNumber / 10;
                            f2 = QuanInfo[k].Number / 10 + QuanInfo[k].PeiNumber / 10;
                            vv1 = vv1 + f1 * vv2;
                            vv2 *= (1 + f2);
                            k += 1;
                        }
                    }
                    v1 = b1.value[i];
                    v2 = v1 * vv2 - vv1;
                    b1.value[i] = v2;
                    v1 = b2.value[i];
                    v2 = v1 * vv2 - vv1;
                    b2.value[i] = v2;
                    v1 = b3.value[i];
                    v2 = v1 * vv2 - vv1;
                    b3.value[i] = v2;
                    v1 = b4.value[i];
                    v2 = v1 * vv2 - vv1;
                    b4.value[i] = v2;
                }
            }
            QuanStyle = value;
            return true;
        }

        public void GetBianList(TStringList sl)
        {
            sl.Clear();
            if (TechList.Count==0)
                return;
            tech ch=TechList[0];
            for (int i = 0; i < ch.namebian.Count; i++)
            {
                TBian b1 = ch.namebian[i];
                sl.Add(b1.name);
            }
        }

        public void ClearLine()
        {
            Lines.Clear();
         
        }

    }
}