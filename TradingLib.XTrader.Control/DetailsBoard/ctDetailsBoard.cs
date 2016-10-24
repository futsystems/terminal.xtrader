using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;
using Common.Logging;
using TradingLib.XTrader;

namespace CStock
{
    /// <summary>
    /// 盘口信息 根据品种不同有多种显示方式
    /// </summary>
    public enum EnumQuoteInfoType
    { 
        /// <summary>
        /// 国内股票
        /// </summary>
        StockCN,
        /// <summary>
        /// 国外期货
        /// </summary>
        FutureOverSea,
    }
    public partial class ctDetailsBoard : UserControl
    {

        ILog logger = LogManager.GetLogger("ctDetailsBoard");
        //盘口信息下面Tab面板
        string[] ws = { "笔", "价"};//, "指", "配", "值" };
        Control[] PBox = new Control[5];

        Color mBackColor = Color.Black;
        Color mBoardColor = Color.Maroon;


        public int myTabIndex = 0;
        public int TabValue
        {
            get { return myTabIndex; }
        }


        public int DefaultHeight { get { return 202; } }
        /// <summary>
        /// Tab页切换
        /// </summary>
        public event Action<object,TabSwitchEventArgs> TabSwitch;

        /// <summary>
        /// Tab页双击
        /// </summary>
        public event Action<object, TabDoubleClickEventArgs> TabDoubleClick = null;
        
        

        MDSymbol _symbol = null;

        public MDSymbol Symbol { get { return _symbol; } }

        Dictionary<EnumQuoteInfoType, IQuoteInfo> quoteInfoMap = new Dictionary<EnumQuoteInfoType, IQuoteInfo>();

        public ctDetailsBoard()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            
            int i = 0;


            QuoteInfoBox.ForeColor = Color.Black;
            DetailTabBox.BackColor = Color.Black;

            PBox[0] = pbox1;
            PBox[1] = pbox2 as Control;
            PBox[2] = pbox3 as Control;
            PBox[3] = pbox4 as Control;
            PBox[4] = pbox5 as Control;
            for (i = 0; i < 5; i++)
            {
                PBox[i].BackColor = Color.Black;
                PBox[i].Dock = DockStyle.Fill;
            }

            DetailTabBox.Dock = DockStyle.Fill;

            ctrlStockQuoteInfo1.Dock = DockStyle.Fill;
            ctrlQuoteInfo1.Dock = DockStyle.Fill;

            quoteInfoMap.Add(ctrlStockQuoteInfo1.QuoteInfoType, ctrlStockQuoteInfo1);
            quoteInfoMap.Add(ctrlQuoteInfo1.QuoteInfoType, ctrlQuoteInfo1);


        }


        IQuoteInfo _currentQuoteInfo = null;

        IQuoteInfo CurrentQuoteInfo { get { return _currentQuoteInfo; } }

        void SelectQuoteInfo(EnumQuoteInfoType type)
        {
            foreach (var v in quoteInfoMap.Values)
            {
                v.Visible = false;
            }

            if (!quoteInfoMap.TryGetValue(type, out _currentQuoteInfo))
            {
                _currentQuoteInfo = null;
            }

            if (this.CurrentQuoteInfo != null)
            {
                //this.CurrentQuoteInfo.Height = this.CurrentQuoteInfo.DefaultHeight;
                this.QuoteInfoBox.Height = this.CurrentQuoteInfo.DefaultHeight;
                this.CurrentQuoteInfo.Visible = true;
                
                //MessageBox.Show(this.CurrentQuoteInfo.Height.ToString());
            }

        }
        EnumQuoteInfoType quoteInfoType = EnumQuoteInfoType.StockCN;

        /// <summary>
        /// 后期根据品种的完善逐渐增加与调整
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        EnumQuoteInfoType GetQuoteInfoType(MDSymbol symbol)
        {
            if (symbol == null) return EnumQuoteInfoType.StockCN;
            switch (symbol.SecurityType)
            {
                case MDSecurityType.STK:
                    {
                        return EnumQuoteInfoType.StockCN;
                    }
                case MDSecurityType.FUT:
                    {
                        return EnumQuoteInfoType.FutureOverSea;
                    }
                default :
                    return EnumQuoteInfoType.FutureOverSea;
            }
        }

        /// <summary>
        /// 设置Stock用于更新当前最新盘口数据
        /// </summary>
        /// <param name="symbol"></param>
        public void SetStock(MDSymbol symbol)
        {
            if (symbol == null)
                return;
           
            _symbol = symbol;
            quoteInfoType = GetQuoteInfoType(symbol);
            //设定当前显示的QuoteInfo
            SelectQuoteInfo(quoteInfoType);

            if (CurrentQuoteInfo != null)
            {
                CurrentQuoteInfo.SetSymbol(symbol);
            }


            pbox1.SetSymbol(symbol);
            this.Update(symbol);
           
            //this.Update(symbol);

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
        public void Update(MDSymbol symbol)
        {
            if (CurrentQuoteInfo != null)
            {
                CurrentQuoteInfo.OnTick(symbol);
            }
        }

        /// <summary>
        /// 返回分笔成交明细可显示行数
        /// </summary>
        public int TradesRowCount
        {
            get
            {
                return pbox1.RowCount;
            }
        }



        public void AddTrade(TradeSplit trade,bool update)
        {
            pbox1.AddTrade(trade, update);
        }
        public void AddTrade(List<TradeSplit> trades, bool update)
        {
            pbox1.AddTrade(trades, update);
        }
        public void ClearFenbi()
        {
            pbox1.Clear();
        }



        public void AddPriceVol(List<PriceVolPair> pvs, bool update)
        {
            pbox2.AddPriceVol(pvs, update);
        }
        public void ClearJia()
        {
            pbox2.Clear();
        }
      



        public void ClearData()
        {

            ctrlStockQuoteInfo1.ClearData();
            pbox1.Clear();
            pbox2.Clear();
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
            for (int i = 0; i < 2; i++)
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
            int ButtonWidth = TabBox.Width / 5;// ws.Length;
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

        private void pbox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.TabDoubleClick != null)
            {
                this.TabDoubleClick(this, new TabDoubleClickEventArgs(DetailBoardTabType.TradeDetails));
            }
        }





        #region pbox2事件
        private void pbox2_DoubleClick(object sender, EventArgs e)
        {
            if (this.TabDoubleClick != null)
            {
                this.TabDoubleClick(this, new TabDoubleClickEventArgs(DetailBoardTabType.PriceDistribution));
            }
        }

        //private void pbox2_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics cv = e.Graphics;
        //    Rectangle r1 = pbox2.ClientRectangle;
        //    cv.FillRectangle(Brushes.Black, r1);

        //    if (JiaList.Count == 0)
        //        return;

        //    string ss;
        //    float lw = (pbox2.Width - 52) / 2; ;
        //    double pr = 0;
        //    //if (PreClose != NA)
        //    //    pr = PreClose;
        //    if (_FCurStock != null)
        //        pr = _FCurStock.PreClose;//.GP.YClose;

        //    SizeF si;
        //    int maxvol = 1;
        //    for (int j = 0; j < JiaList.Count; j++)
        //    {
        //        jialist tk = JiaList[j];
        //        if (tk.vol > maxvol)
        //            maxvol = tk.vol;
        //    }
        //    for (int j = JiaList.Count - 1; j > -1; j--)
        //    {
        //        jialist tk = JiaList[j];
        //        r1.Y = (JiaList.Count - 1 - j) * LineHeight + 2;

        //        ss = string.Format("{0:F2}", tk.value);
        //        si = cv.MeasureString(ss, Constants.QuoteFont);
        //        if (tk.value > pr)
        //            cv.DrawString(ss, Constants.QuoteFont, Brushes.Red, (int)(50 - si.Width), r1.Top);
        //        else
        //            cv.DrawString(ss, Constants.QuoteFont, Brushes.Green, (int)(50 - si.Width), r1.Top);

        //        ss = string.Format("{0:D}", tk.vol);
        //        si = cv.MeasureString(ss, Constants.QuoteFont);
        //        cv.DrawString(ss, Constants.QuoteFont, Brushes.Yellow, (int)(50 + 1 * lw - si.Width), r1.Top);
        //        int ww = (int)(tk.vol * (lw - 4) / maxvol);
        //        if (ww == 0)
        //            ww = 1;
        //        cv.FillRectangle(Brushes.Aqua, (50 + lw + 2), r1.Top + 2, ww, LineHeight - 4);

        //        if (r1.Y + LineHeight > pbox2.Height)
        //            break;
        //    }
        //}

        //private void pbox2_Resize(object sender, EventArgs e)
        //{
        //    pbox2.Invalidate();
        //}
        #endregion


    }
}
