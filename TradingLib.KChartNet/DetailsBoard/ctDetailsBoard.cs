using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;

namespace CStock
{
    public partial class ctDetailsBoard : UserControl
    {
        //盘口信息下面Tab面板
        string[] ws = { "笔", "价", "指", "配", "值" };
        Panel[] PBox = new Panel[5];

        Color mBackColor = Color.Black;
        Color mBoardColor = Color.Maroon;


        public int myTabIndex = 0;
        public int TabValue
        {
            get { return myTabIndex; }
        }

        /// <summary>
        /// Tab页切换
        /// </summary>
        public event Action<object,TabSwitchEventArgs> TabSwitch;

        //点击位置事件
        //public delegate void StockEventHandler(object sender, StockEventArgs e);
        //用event 关键字声明事件对象
        //public event StockEventHandler StockClick = null;

        /// <summary>
        /// Tab页双击
        /// </summary>
        public event Action<object, TabDoubleClickEventArgs> TabDoubleClick = null;
        
        int LineHeight = 18;//输出行高

        MDSymbol FCurStock = null;

        Label[] SellValue = new Label[10];
        Label[] SellVol = new Label[10];
        Label[] BuyValue = new Label[10];
        Label[] BuyVol = new Label[10];

        Label[] Cell = new Label[28];
        Color volc = Color.FromArgb(192, 192, 0);

        //笔--列表
        public List<Tick> FenBiList = new List<Tick>();

        //价--列表
        //public SortedList<double,jia> jialist = new SortedList<double,jia>();
        public List<jialist> JiaList = new List<jialist>();

        /// <summary>
        /// 合约标题
        /// </summary>
        public string StockLabel
        {
            get { return StkLabel.Text; }
            set
            {
                StkLabel.Text = value;
            }
        }
        public ctDetailsBoard()
        {
            InitializeComponent();
            
            int i = 0;

            DetailTabBox.BackColor = Color.Black;
            PBox[0] = pbox1;
            PBox[1] = pbox2;
            PBox[2] = pbox3;
            PBox[3] = pbox4;
            PBox[4] = pbox5;
            for (i = 0; i < 5; i++)
            {
                PBox[i].BackColor = Color.Black;
                PBox[i].Dock = DockStyle.Fill;
            }

            DetailTabBox.Dock = DockStyle.Fill;
            Board.ForeColor = Color.Black;

            weibi.Font = Constants.QuoteFont;
            weica.Font = Constants.QuoteFont;

            //添加买盘label到layoutpanel
            for (i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();// "";
                SellValue[i] = l;
                Sell.Controls.Add(l, 1, 4 - i);

                Label l11 = new Label();
                l11.AutoSize = false;
                l11.TextAlign = ContentAlignment.MiddleRight;
                l11.Dock = DockStyle.Top;
                l11.Height = 18;
                l11.ForeColor = Constants.ColorSize;
                l11.Font = Constants.QuoteFont;
                l11.Text = i.ToString();
                SellVol[i] = l11;
                Sell.Controls.Add(l11, 2, 4 - i);
            }

            //添加卖盘label到layoutpanel
            for (i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();// "";
                BuyValue[i] = l;
                Buy.Controls.Add(l, 1, i);

                Label l11 = new Label();
                l11.AutoSize = false;
                l11.TextAlign = ContentAlignment.MiddleRight;
                l11.Dock = DockStyle.Top;
                l11.Height = 18;
                l11.ForeColor = Constants.ColorSize;
                l11.Font = Constants.QuoteFont;
                l11.Text = i.ToString();
                BuyVol[i] = l11;
                Buy.Controls.Add(l11, 2, i);
            }

            //初始化其他参数标签并加入到layoutpanel
            for (i = 0; i < 28; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();
                Cell[i] = l;
            }
            cell4.Controls.Add(Cell[0], 1, 0);
            cell4.Controls.Add(Cell[1], 1, 1);
            cell4.Controls.Add(Cell[2], 1, 2);
            cell4.Controls.Add(Cell[3], 1, 3);
            cell4.Controls.Add(Cell[4], 1, 4);
            cell4.Controls.Add(Cell[5], 1, 5);
            cell4.Controls.Add(Cell[6], 1, 6);

            cell4.Controls.Add(Cell[7], 3, 0);
            cell4.Controls.Add(Cell[8], 3, 1);
            cell4.Controls.Add(Cell[9], 3, 2);
            cell4.Controls.Add(Cell[10], 3, 3);
            cell4.Controls.Add(Cell[11], 3, 4);
            cell4.Controls.Add(Cell[12], 3, 5);
            cell4.Controls.Add(Cell[13], 3, 6);


            cell5.Controls.Add(Cell[14], 1, 0);
            cell5.Controls.Add(Cell[15], 1, 1);
            cell5.Controls.Add(Cell[16], 1, 2);
            cell5.Controls.Add(Cell[17], 3, 0);
            cell5.Controls.Add(Cell[18], 3, 1);
            cell5.Controls.Add(Cell[19], 3, 2);

            Cell[21].ForeColor = volc;
            Cell[23].ForeColor = volc;
        }

        /// <summary>
        /// 刷新指定的Tab窗口0~4
        /// </summary>
        /// <param name="Index"></param>
        public void TabPaint(int Index)
        {
            if ((Index > -1) && (Index < PBox.Length))
            {
                PBox[Index].Invalidate();
            }

        }


        #region 数据操作
        /// <summary>
        /// 设置Stock用于更新当前最新盘口数据
        /// </summary>
        /// <param name="symbol"></param>
        public void SetStock(MDSymbol symbol)
        {
            if (symbol == null)
                return;
            if (BuyValue[0] == null)
                return;
            FCurStock = symbol;

            if (symbol.BlockType == "7")
            {
                if (Buy.Visible)
                {
                    Buy.Visible = false;
                    BuySp.Visible = false;
                    Sell.Visible = false;
                    SellSp.Visible = false;
                }

            }
            else
            {
                if (!Buy.Visible)
                {
                    Buy.Visible = true;
                    Sell.Visible = true;
                    BuySp.Visible = true;
                    SellSp.Visible = true;
                }
            }

            double f1 = symbol.TickSnapshot.SellQTY1 + symbol.TickSnapshot.SellQTY2 + symbol.TickSnapshot.SellQTY3 + symbol.TickSnapshot.SellQTY4 + symbol.TickSnapshot.SellQTY5;
            double f2 = symbol.TickSnapshot.BuyQTY1 + symbol.TickSnapshot.BuyQTY2 + symbol.TickSnapshot.BuyQTY3 + symbol.TickSnapshot.BuyQTY4 + symbol.TickSnapshot.BuyQTY5;
            weibi.Text = "";
            weica.Text = "";

            if ((f1 + f2) > 0)
            {
                weibi.Text = string.Format("{0:F2}%", (f1 - f2) * 100 / (f1 + f2));
                weibi.ForeColor = f2 > f1 ? Constants.ColorUp : Constants.ColorDown;
            }
            else
            {
                weibi.Text = "0%";
                weibi.ForeColor = Color.White;
            }
            if ((f2 + f1) > 0)
            {
                weica.Text = string.Format("{0:F0}%", f2 - f1);
                weica.ForeColor = f2 > f1 ? Constants.ColorUp : Constants.ColorDown;
            }
            else
            {
                weica.Text = "";
            }


            for (int i = 0; i < 5; i++)
            {
                SellValue[i].Text = "";
                SellVol[i].Text = "";
                BuyValue[i].Text = "";
                BuyVol[i].Text = "";

            }
            for (int i = 0; i < 24; i++)
                Cell[i].Text = "";
            if (symbol.TickSnapshot.BuyQTY1 > 0)
            {
                BuyValue[0].Text = String.Format("{0:F3}", symbol.TickSnapshot.Buy1);
                BuyVol[0].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY1);
                BuyValue[0].ForeColor = symbol.TickSnapshot.Buy1 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY2 > 0)
            {
                BuyValue[1].Text = String.Format("{0:F3}", symbol.TickSnapshot.Buy2);
                BuyVol[1].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY2);
                BuyValue[1].ForeColor = symbol.TickSnapshot.Buy2 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY3 > 0)
            {
                BuyValue[2].Text = String.Format("{0:F3}", symbol.TickSnapshot.Buy3);
                BuyVol[2].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY3);
                BuyValue[2].ForeColor = symbol.TickSnapshot.Buy3 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY4 > 0)
            {
                BuyValue[3].Text = String.Format("{0:F3}", symbol.TickSnapshot.Buy4);
                BuyVol[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY4);
                BuyValue[3].ForeColor = symbol.TickSnapshot.Buy4 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }
            if (symbol.TickSnapshot.BuyQTY5 > 0)
            {
                BuyValue[4].Text = String.Format("{0:F3}", symbol.TickSnapshot.Buy5);
                BuyVol[4].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY5);
                BuyValue[4].ForeColor = symbol.TickSnapshot.Buy5 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }


            if (symbol.TickSnapshot.SellQTY1 > 0)
            {
                SellValue[0].Text = String.Format("{0:F3}", symbol.TickSnapshot.Sell1);
                SellVol[0].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY1);
                SellValue[0].ForeColor = symbol.TickSnapshot.Sell1 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY2 > 0)
            {
                SellValue[1].Text = String.Format("{0:F3}", symbol.TickSnapshot.Sell2);
                SellVol[1].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY2);
                SellValue[1].ForeColor = symbol.TickSnapshot.Sell2 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY3 > 0)
            {
                SellValue[2].Text = String.Format("{0:F3}", symbol.TickSnapshot.Sell3);
                SellVol[2].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY3);
                SellValue[2].ForeColor = symbol.TickSnapshot.Sell3 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY4 > 0)
            {
                SellValue[3].Text = String.Format("{0:F3}", symbol.TickSnapshot.Sell4);
                SellVol[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY4);
                SellValue[3].ForeColor = symbol.TickSnapshot.Sell4 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }
            if (symbol.TickSnapshot.SellQTY5 > 0)
            {
                SellValue[4].Text = String.Format("{0:F3}", symbol.TickSnapshot.Sell5);
                SellVol[4].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY5);
                SellValue[4].ForeColor = symbol.TickSnapshot.Sell5 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }


            Cell[0].Text = String.Format("{0:F3}", symbol.TickSnapshot.Price);
            Cell[0].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;

            Cell[7].Text = String.Format("{0:F3}", symbol.TickSnapshot.Open);
            Cell[7].ForeColor = symbol.TickSnapshot.Open > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;

            Cell[1].Text = String.Format("{0:F3}", symbol.TickSnapshot.Price - symbol.TickSnapshot.last);
            Cell[1].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;

            Cell[8].Text = String.Format("{0:F3}", symbol.TickSnapshot.High);
            Cell[8].ForeColor = symbol.TickSnapshot.High > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;

            if (symbol.TickSnapshot.last != 0)
            {
                Cell[2].Text = String.Format("{0:F2}%", (symbol.TickSnapshot.Price - symbol.TickSnapshot.last) * 100 / symbol.TickSnapshot.last);
                Cell[2].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }
            Cell[9].Text = String.Format("{0:F3}", symbol.TickSnapshot.Low);
            Cell[9].ForeColor = symbol.TickSnapshot.Low > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;

            Cell[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.Size);
            double d1 = symbol.TickSnapshot.Volume;
            if (d1 > 0)
            {
                double d2 = (symbol.TickSnapshot.Amount / symbol.TickSnapshot.Volume) / 100.0;
                Cell[10].Text = String.Format("{0:F3}", d2);
                Cell[10].ForeColor = d2 > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            }
            double d3 = symbol.TickSnapshot.Amount;
            if (d3 > 100000000)
            {
                Cell[4].Text = String.Format("{0:F2}亿", symbol.TickSnapshot.Amount / 100000000);
            }
            else
                Cell[4].Text = String.Format("{0:F0}万", symbol.TickSnapshot.Amount / 10000);
            if (d1 > 1000000)
            {
                d1 = d1 / 10000;
                Cell[11].Text = String.Format("{0:F1}万", d1);
            }
            else
                Cell[11].Text = String.Format("{0:F0}", d1);

            f2 = 0.1;
            if (symbol.Symbol.IndexOf("ST") > -1)
                f2 = 0.05;
            Cell[5].Text = String.Format("{0:F3}", symbol.TickSnapshot.last * (1 + f2));
            Cell[5].ForeColor = Constants.ColorUp;
            Cell[12].Text = String.Format("{0:F3}", symbol.TickSnapshot.last * (1 - f2));
            Cell[12].ForeColor = Constants.ColorDown;
            Cell[6].Text = String.Format("{0:F0}", symbol.TickSnapshot.S);
            Cell[6].ForeColor = Color.Red;
            Cell[13].Text = String.Format("{0:F0}", symbol.TickSnapshot.B);
            Cell[13].ForeColor = Constants.ColorDown;
            if (symbol.FinanceData.LTG > 0)
            {
                Cell[14].Text = String.Format("{0:F2}%", symbol.TickSnapshot.Volume / symbol.FinanceData.LTG);
                Cell[18].Text = String.Format("{0:F1}亿", symbol.FinanceData.LTG / 10000);
            }
            else
            {
                Cell[14].Text = "";
                Cell[18].Text = "";
            }

            if (symbol.FinanceData.zl != null)
            {
                Cell[17].Text = String.Format("{0:F1}亿", symbol.FinanceData.zl[0] / 10000);
                Cell[15].Text = String.Format("{0:F2}", symbol.FinanceData.zl[15] / symbol.FinanceData.zl[0] / 10);
                f2 = 0;
                if (symbol.FinanceData.zl[26] > 0)
                    f2 = symbol.FinanceData.zl[26] / symbol.FinanceData.zl[0] / 10;
                Cell[16].Text = String.Format("{0:F3}", f2);

                if ((f2 > 0) && (symbol.FinanceData.zl[29] > 0))
                {
                    f2 = symbol.TickSnapshot.Price / (f2 / symbol.FinanceData.zl[29] * 12);
                    Cell[19].Text = String.Format("{0:F1}", f2);
                }
            }
            else
            {
                Cell[15].Text = "";
                Cell[16].Text = "";
                Cell[17].Text = "";
                Cell[19].Text = "";
            }


            Cell[20].Text = String.Format("{0:F3}", symbol.TickSnapshot.sellall);
            Cell[20].ForeColor = symbol.TickSnapshot.sellall > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            Cell[21].Text = String.Format("{0:F0}", symbol.TickSnapshot.sellQTYall);

            Cell[22].Text = String.Format("{0:F3}", symbol.TickSnapshot.buyall);
            Cell[22].ForeColor = symbol.TickSnapshot.buyall > symbol.TickSnapshot.last ? Constants.ColorUp : Constants.ColorDown;
            Cell[23].Text = String.Format("{0:F0}", symbol.TickSnapshot.buyQTYall);
            //FSGS[0].PreClose = symbol.TickSnapshot.last;
            this.Invalidate();

        }

        /// <summary>
        /// 增加一行分笔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <param name="vol"></param>
        /// <param name="tick"></param>
        public void AddTick(int time, double value, int vol, int tick, int tickcount,bool update=false)
        {
            Tick tk = new Tick();
            tk.time = time;
            tk.value = value;
            tk.vol = vol;
            tk.tick = tick;
            tk.tickcount = tickcount;
            FenBiList.Add(tk);
            if (update)
            {
                pbox1.Invalidate();
            }
        }

        /// <summary>
        /// 返回分笔成交明细可显示行数
        /// </summary>
        public int TradesRowCount
        {
            get
            {
                return (pbox1.Height - 2) / LineHeight;
            }
        }

        public void ClearFenbi()
        {
            FenBiList.Clear();
            pbox1.Invalidate();
        }

        public void ClearJia()
        {
            JiaList.Clear();
            pbox2.Invalidate();
        }
        public void AddJia(double value, int vol,bool update=false)
        {

            jialist tk = new jialist();
            tk.value = value;
            tk.vol = vol;
            JiaList.Add(tk);
            if (update)
            {
                pbox2.Invalidate();//是否可以考虑数据添加完毕后统一Invalidate
            }
        }

        public void ClearData()
        {
            if (SellValue[0] != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    SellValue[i].Text = "";
                    SellVol[i].Text = "";
                    BuyValue[i].Text = "";
                    BuyVol[i].Text = "";

                }
            }

            JiaList.Clear();
            FenBiList.Clear();
        }
        #endregion



        #region 盘口底部Tab事件
        private void TabBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            Rectangle TabArea = TabBox.ClientRectangle;

            Brush br = new SolidBrush(mBackColor);
            cv.FillRectangle(br, TabArea);
            br.Dispose();

            Pen border = new Pen(mBoardColor);
            cv.DrawLine(border, TabArea.Left, TabArea.Top, TabArea.Right, TabArea.Top);
            border.Dispose();

            Brush br1 = new SolidBrush(Color.FromArgb(28, 28, 28));
            Brush bh1 = new SolidBrush(Color.FromArgb(178, 178, 178));
            Brush br2 = new SolidBrush(Color.FromArgb(90, 0, 0));
            Brush bh2 = new SolidBrush(Color.Yellow);
            Pen pn = new Pen(Color.FromArgb(155, 0, 0));

            Point[] pt = new Point[5];
            Rectangle r1 = new Rectangle();
            int ww = TabBox.Width / 5;
            r1.Width = ww;
            r1.Height = TabBox.Height;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    r1.Width = TabBox.Width - ww * (ws.Length - 1) - 1;
                }
                r1.X = TabArea.Left + i * ww;
                pt[0] = new Point(r1.Left, r1.Top);
                pt[1] = new Point(r1.Left, r1.Bottom - 1);
                pt[2] = new Point(r1.Right - 5, r1.Bottom - 1);
                pt[3] = new Point(r1.Right, r1.Bottom - 6);
                pt[4] = new Point(r1.Right, r1.Top);

                if (myTabIndex != i)
                {
                    cv.FillPolygon(br1, pt);
                    cv.DrawString(ws[i], Font, bh1, r1, stringFormat);
                }
                else
                {
                    cv.FillPolygon(br2, pt);
                    cv.DrawString(ws[i], Font, bh2, r1, stringFormat);
                }
                cv.DrawPolygon(pn, pt);
            }
        }

        private void TabBox_MouseClick(object sender, MouseEventArgs e)
        {
            int ButtonWidth = TabBox.Width / ws.Length;
            int ii = e.X / ButtonWidth;
            if (ii != myTabIndex)
            {
                PBox[myTabIndex].Visible = false;
                myTabIndex = ii;
                PBox[myTabIndex].Visible = true; ;
                TabBox.Invalidate();
                if (TabSwitch != null)
                {
                    //切换Tab时候 清除原有数据 并对外触发事件 用于向服务端请求最新数据
                    if (myTabIndex == 0)
                    {
                        this.ClearFenbi();
                    }
                    if (myTabIndex == 1)
                    {
                        this.ClearJia();
                    }
                    TabSwitch(this, new TabSwitchEventArgs(GetTabType(myTabIndex)));
                }
            }
        }


        DetailBoardTabType GetTabType(int index)
        {
            if (myTabIndex == 0)
            {
                return DetailBoardTabType.TradeDetails;
            }
            if (myTabIndex == 1)
            {
                return DetailBoardTabType.PriceDistribution;
            }

            return DetailBoardTabType.Unknown;

        }


        #endregion





        #region pbox1事件
        private void pbox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.TabDoubleClick != null)
            {
                this.TabDoubleClick(this, new TabDoubleClickEventArgs(DetailBoardTabType.TradeDetails));
            }
        }

        private void pbox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            Rectangle r1 = pbox1.ClientRectangle;
            Brush br = new SolidBrush(Color.Black);
            Pen p = new Pen(Color.Maroon);
            cv.FillRectangle(br, r1);
            br.Dispose();
            p.Dispose();

            if (FenBiList.Count == 0)
                return;
            int i = 0;
            int h = (pbox1.Height - 2) / LineHeight;
            if (FenBiList.Count > h)
                i = FenBiList.Count - h;

            string ss;
            int time = -1, jj = -1;
            float lw;
            SizeF si;
            Tick tk = FenBiList[0];
            System.Drawing.Font font = Constants.QuoteFont;
            if (FCurStock.BlockType == "7")// tk.value > 300) //为指数
            {
                lw = (pbox1.Width - 52) / 2;
                for (int j = i; j < FenBiList.Count; j++)
                {
                    tk = FenBiList[j];
                    ss = "";
                    if (time == -1)
                    {
                        jj = 1;
                        ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.time / 100, tk.time % 100, jj);
                        time = tk.time;
                    }
                    else
                    {
                        if (tk.time == time)
                        {
                            jj += 1;
                            ss = string.Format(":{0:D2}", jj);
                        }

                        if (tk.time > time)// (tk.time - time) > 100)
                        {
                            jj = 1;
                            ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.time / 100, tk.time % 100, jj);
                            time = tk.time;
                        }
                    }
                    r1.Y = (j - i) * LineHeight + 2;
                    si = cv.MeasureString(ss, font);
                    if (jj == 1)
                        cv.DrawString(ss, font, Brushes.Gray, (int)(52 - si.Width), r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.Gray, (int)(52 - si.Width - 1), r1.Top);

                    ss = string.Format("{0:F2}", tk.value);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Red, (int)(50 + lw - si.Width), r1.Top);

                    ss = string.Format("{0:D}", tk.vol);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.YellowGreen, (int)(50 + 2 * lw - si.Width), r1.Top);
                }
            }
            else
            {
                lw = (pbox1.Width - 92) / 2;
                for (int j = i; j < FenBiList.Count; j++)
                {
                    tk = FenBiList[j];
                    ss = "";
                    if (time == -1)
                    {
                        jj = 1;
                        ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.time / 100, tk.time % 100, jj);
                        time = tk.time;
                    }
                    else
                    {
                        if (tk.time == time)
                        {
                            jj += 1;
                            ss = string.Format(":{0:D2}", jj);
                        }

                        if (tk.time > time)// (tk.time - time) > 100)
                        {
                            jj = 1;
                            ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.time / 100, tk.time % 100, jj);
                            time = tk.time;
                        }
                    }
                    r1.Y = (j - i) * LineHeight + 2;
                    si = cv.MeasureString(ss, font);
                    if (jj == 1)
                        cv.DrawString(ss, font, Brushes.White, (int)(52 - si.Width), r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.White, (int)(52 - si.Width - 1), r1.Top);

                    ss = string.Format("{0:F2}", tk.value);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Red, (int)(50 + lw - si.Width), r1.Top);

                    ss = string.Format("{0:D}", tk.vol);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Yellow, (int)(50 + 2 * lw - si.Width), r1.Top);

                    if (tk.tick == 1)
                        ss = "B";
                    else
                        ss = "S";
                    si = cv.MeasureString(ss, font);
                    if (tk.tick == 1)
                        cv.DrawString(ss, font, Brushes.Red, pbox1.Width - 40, r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.Lime, pbox1.Width - 40, r1.Top);
                    ss = tk.tickcount.ToString();
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Gray, pbox1.Width - si.Width, r1.Top);
                }
            }
        }

        private void pbox1_Resize(object sender, EventArgs e)
        {
            pbox1.Invalidate();
        }

        #endregion




        #region pbox2事件
        private void pbox2_DoubleClick(object sender, EventArgs e)
        {
            if (this.TabDoubleClick != null)
            {
                this.TabDoubleClick(this, new TabDoubleClickEventArgs(DetailBoardTabType.PriceDistribution));
            }
        }

        private void pbox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            Rectangle r1 = pbox2.ClientRectangle;
            cv.FillRectangle(Brushes.Black, r1);

            if (JiaList.Count == 0)
                return;

            string ss;
            float lw = (pbox2.Width - 52) / 2; ;
            double pr = 0;
            //if (PreClose != NA)
            //    pr = PreClose;
            if (FCurStock != null)
                pr = FCurStock.PreClose;//.GP.YClose;

            SizeF si;
            int maxvol = 1;
            for (int j = 0; j < JiaList.Count; j++)
            {
                jialist tk = JiaList[j];
                if (tk.vol > maxvol)
                    maxvol = tk.vol;
            }
            for (int j = JiaList.Count - 1; j > -1; j--)
            {
                jialist tk = JiaList[j];
                r1.Y = (JiaList.Count - 1 - j) * LineHeight + 2;

                ss = string.Format("{0:F2}", tk.value);
                si = cv.MeasureString(ss, Constants.QuoteFont);
                if (tk.value > pr)
                    cv.DrawString(ss, Constants.QuoteFont, Brushes.Red, (int)(50 - si.Width), r1.Top);
                else
                    cv.DrawString(ss, Constants.QuoteFont, Brushes.Green, (int)(50 - si.Width), r1.Top);

                ss = string.Format("{0:D}", tk.vol);
                si = cv.MeasureString(ss, Constants.QuoteFont);
                cv.DrawString(ss, Constants.QuoteFont, Brushes.Yellow, (int)(50 + 1 * lw - si.Width), r1.Top);
                int ww = (int)(tk.vol * (lw - 4) / maxvol);
                if (ww == 0)
                    ww = 1;
                cv.FillRectangle(Brushes.Aqua, (50 + lw + 2), r1.Top + 2, ww, LineHeight - 4);

                if (r1.Y + LineHeight > pbox2.Height)
                    break;
            }
        }

        private void pbox2_Resize(object sender, EventArgs e)
        {
            pbox2.Invalidate();
        }
        #endregion


    }
}
