using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using TradingLib.MarketData;
using Common.Logging;


namespace TradingLib.XTrader.Control
{
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class ctrlPriceVolList : System.Windows.Forms.Control,IView
    {

        EnumViewType vietype = EnumViewType.PriceVol;

        public EnumViewType ViewType { get { return vietype; } }

        ILog logger = LogManager.GetLogger("ctrlTickList");
        public event EventHandler ExitView;
        public ctrlPriceVolList()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseClick += new MouseEventHandler(ctrlTickList_MouseClick);
            this.MouseMove += new MouseEventHandler(ctrlTickList_MouseMove);

            this.GotFocus += new EventHandler(ctrlTickList_GotFocus);
            this.LostFocus += new EventHandler(ctrlTickList_LostFocus);

            rowCnt = (this.Height - topHeight - columHeight) / lineHeight;
            colCnt = this.Width / _defaultColumnWidth; 
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    {
                        ScrollPage(false);
                        break;
                    }
                case Keys.Down:
                    {
                        ScrollPage(true);
                        break;
                    }
                default:
                    break;
            }

            return base.ProcessDialogKey(keyData);
        }
       

        void ctrlTickList_LostFocus(object sender, EventArgs e)
        {
            logger.Info("lost focus");
        }

        void ctrlTickList_GotFocus(object sender, EventArgs e)
        {
            logger.Info("got focus");
        }


        
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScrollPage(false);
            }
            else
            {
                ScrollPage(true);
            }
        }

        


        void ScrollPage(bool after)
        {
            int rowCnt = (this.Height - topHeight - columHeight) / lineHeight;//计算每列可显示数据量
            int dataColCnt = pvList.Count / rowCnt;
            dataColCnt += pvList.Count % rowCnt > 0 ? 1 : 0;//计算所有数据的列数量

            //显示列数
            int columnCnt = this.Width / _defaultColumnWidth;//计算当前可显示的列数
            if (dataColCnt > columnCnt)
            {
                if (after)
                {
                    startDataColumn++;

                }
                else
                {
                    startDataColumn--;
                }
                if (startDataColumn >= dataColCnt - columnCnt)
                {
                    viewlast = true;
                    //logger.Info("view latest page !!!!!!!!!!!");
                    startDataColumn = dataColCnt - columnCnt;
                }
                else
                {
                    //logger.Info("view hist page !!!!!!!!!!!");
                    viewlast = false;
                }
                if (startDataColumn < 0)
                    startDataColumn = 0;
                this.Invalidate();
            }
        }
        bool viewlast = false;

        bool _mouseInClose = false;
        void ctrlTickList_MouseMove(object sender, MouseEventArgs e)
        {
            bool oldState = _mouseInClose;
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            Graphics g = this.CreateGraphics();
            if (e.X > p.X && e.X < p.X + btnSize && e.Y > p.Y && e.Y < p.Y + btnSize)
            {
                _mouseInClose = true;
                //DrawCloseBotton(g, Color.Orange, p, btnSize);
            }
            else
            {
                _mouseInClose = false;
            }
            if (_mouseInClose != oldState)
            {
                this.Invalidate();
            }
        }

        void ctrlTickList_MouseClick(object sender, MouseEventArgs e)
        {
            Focus();
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            if (e.X > p.X && e.X < p.X + btnSize && e.Y > p.Y && e.Y < p.Y + btnSize)
            {
                if (ExitView != null)
                {
                    ExitView(this, null);
                }
            }
        }




        public MDSymbol Symbol { get { return _symbol; } }
        MDSymbol _symbol = new MDSymbol();
        string _priceFormat = "{0:F2}";
        public void SetSymbol(MDSymbol symbol)
        {
            _symbol = symbol;
            _priceFormat = symbol.GetFormat();
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
        }


        int _defaultColumnWidth = 400;
        Pen _pen = new Pen(UIConstant.ColorLine, 1);

        int topHeight = 30;
        int columHeight = 22;
        int lineHeight = 22;
        int fontHeight = UIConstant.QuoteFont.Height;
        SolidBrush _brushTime = new SolidBrush(Color.Silver);
        SolidBrush _brushPrice = new SolidBrush(Color.Silver);

        Pen _btnPen = new Pen(Color.Red, 1);
        void DrawCloseBotton(Graphics g,Color color, Point p, int size)
        { 
            Point p1 = p;
            Point p2 = new Point(p1.X+size,p1.Y);
            Point p3 = new Point(p1.X,p1.Y+size);
            Point p4 = new Point(p1.X+size,p1.Y+size);
            _btnPen.Color = color;
            g.DrawLine(_btnPen, p1, p2);
            g.DrawLine(_btnPen, p1, p3);
            g.DrawLine(_btnPen, p3, p4);
            g.DrawLine(_btnPen, p2, p4);
            g.DrawLine(_btnPen, p1, p4);
            g.DrawLine(_btnPen, p2, p3);
        }

        /// <summary>
        /// 返回数据个数
        /// </summary>
        public int Count { get { return pvList.Count; } }
        //LinkedList<TradeSplit> tradeSplitList = new LinkedList<TradeSplit>();

        List<PriceVolPair> pvList = new List<PriceVolPair>();
        /// <summary>
        /// 添加到数据头部
        /// </summary>
        /// <param name="list"></param>
        public void Add(List<PriceVolPair> list)
        {
            pvList.AddRange(list);
        }

        /// <summary>
        /// 实时刷新时 请求最后一列数据
        /// 即序号从0开始 数量为每列显示的行数
        /// 然后再把数据进行拼接
        /// </summary>
        /// <param name="list"></param>
        //public void Update(List<TradeSplit> list)
        //{
        //    //this.Clear();
        //    //for (int i = list.Count - 1; i >= 0; i--)
        //    //{
        //    //    tradeSplitList.AddFirst(list[i]);
        //    //}

        //    int time = GetMinFinished(list);
        //    if (time <= 0) //没有获得完成的分钟数，无法准确更新
        //    {
        //        logger.Error("can not find minute finished,do not update");
        //        return;
        //    }

        //    LinkedListNode<TradeSplit> node = tradeSplitList.Last;
        //    LinkedListNode<TradeSplit> remove = null;
        //    if (node == null)
        //    {
        //        logger.Error("there is no data resumed,can not update");
        //    }
        //    else
        //    {
        //        //当前节点事件大于该时间 则往前移位
        //        while (node != null && node.Value.Time > time)
        //        {
        //            remove = node;
        //            node = node.Previous;
        //            tradeSplitList.Remove(remove);
        //        }

        //        if (node != null)
        //        {
        //            LinkedListNode<TradeSplit> current = node;

        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                if (list[i].Time > time)//如果
        //                {
        //                    tradeSplitList.AddAfter(current, new LinkedListNode<TradeSplit>(list[i]));//添加到该节点之后
        //                    current = current.Next;
        //                }
        //            }
        //        }

        //    }
            
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //int GetMinFinished(List<TradeSplit> list)
        //{
        //    if (list.Count == 0) return -1;
        //    int time = list.Last().Time;
        //    for (int i = list.Count - 1; i >= 0; i--)
        //    {
        //        if (time != list[i].Time)//比较时间分钟 分钟不一致则返回较老的时间
        //            return list[i].Time;//返回已经结束的分钟
        //    }
        //    return -1;
        //}

        ///// <summary>
        ///// 返回最近一分钟成交笔数
        ///// </summary>
        ///// <returns></returns>
        //int GetLastMinueTradeCount()
        //{
        //    int cnt = 1;
        //    LinkedListNode<TradeSplit> node = tradeSplitList.Last;
        //    if (node == null) return 0;
        //    //如果当前节点事件与上一个节点事件一样 则前移一位
        //    while (node.Previous!=null && node.Value.Time == node.Previous.Value.Time)
        //    {
        //        node = node.Previous;
        //        cnt++;
        //    }
        //    return cnt;
        //}



        public void Clear()
        {
            pvList.Clear();
            if (!update)
            {
                this.Invalidate();
            }

            
        }

        bool update = false;
        public void BeginUpdate()
        {
            update = true;
        
        }

        public void EndUpdate()
        {
            update = false;
            this.Invalidate();
        }


        /// <summary>
        /// 其实绘制数据列
        /// 数据太多 在一个屏幕无法显示时 需要进行翻页
        /// </summary>
        int startDataColumn = 0;


        int rowCnt = 0;
        /// <summary>
        /// 返回每列显示的数据行数
        /// 实时更新方法
        /// 1.以每列数据行数请求数据
        /// 2.按分钟作为节点进行更新
        /// </summary>
        public int RowCount { get { return rowCnt; } }

        int colCnt = 0;
        public int ColumnCount { get { return colCnt; } }

        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            logger.Info("paint ............");
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, this.ClientRectangle);

            colCnt = this.Width / _defaultColumnWidth;
            if (colCnt == 0)
            {
                g.DrawString("PriceVolList", UIConstant.LableFont, Brushes.Yellow, 10, 10);
                return;
            }
            int columnWidth = this.Width / colCnt;

            //string t = string.Format("{0} {1}",columnCnt,columnWidth);
            //g.DrawString(t, UIConstant.QuoteFont, Brushes.Red, 10, 10);

            Rectangle rect = new Rectangle();
            float tWidth = 0;
            System.Drawing.Font font = UIConstant.QuoteFont;

            //string d = string.Format("{0}-{1}", columnCnt, columnWidth);
            //g.DrawString(d, UIConstant.QuoteFont, Brushes.Red, 10, 10);
            g.DrawString(string.IsNullOrEmpty(_symbol.Symbol) ? "000000" : _symbol.Symbol, UIConstant.QuoteFont, Brushes.Yellow, 20, (topHeight - fontHeight) / 2);
            g.DrawString(string.IsNullOrEmpty(_symbol.Name) ? "中国平安" : _symbol.Name, UIConstant.LableFont, Brushes.Yellow, 70, (topHeight - fontHeight) / 2);

            g.DrawString("鼠标滚轮/上下键 翻页 ", UIConstant.HelpFont, Brushes.Gray, 140, (topHeight - fontHeight) / 2 + 2);

            g.DrawString("按Esc键返回", UIConstant.HelpFont, Brushes.Red, this.Width - 110, (topHeight - fontHeight) / 2 + 3);

            //绘制关闭按钮
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            DrawCloseBotton(g,_mouseInClose?Color.Orange: Color.Red, p, btnSize);

            rowCnt = (this.Height - topHeight - columHeight) / lineHeight;

            if (viewlast)
            {
                int dataColCnt = pvList.Count / rowCnt;
                dataColCnt += pvList.Count % rowCnt > 0 ? 1 : 0;//计算所有数据的列数量
                startDataColumn = dataColCnt - colCnt;
            }
            string text = string.Empty;
            int lw = (columnWidth - 90) / 2;
            int cnt = rowCnt*startDataColumn;//从0列开始则cnt起始为0，如果从第一列开始显示则对应的位为rowCnt则图形第一列显示的是数据第二列
            PriceVolPair split = null;

            double pr = 0;
            if (_symbol != null)
                pr = _symbol.GetYdPrice();//.GP.YClose;

            SizeF si;
            int maxvol = 1;
            int totalvol = 0;
            for (int j = 0; j < pvList.Count; j++)
            {
                PriceVolPair tk = pvList[j];
                if (tk.Vol > maxvol)
                    maxvol = tk.Vol;
                totalvol += tk.Vol;
            }


            for (int i = 0; i < colCnt; i++)
            {
                rect.X = i * columnWidth;

                if (i == colCnt - 1)
                    columnWidth = this.Width - (colCnt - 1) * columnWidth;

                

                //绘制顶部列标题
                g.DrawLine(_pen, rect.X, topHeight, rect.X + columnWidth, topHeight);
                g.DrawLine(_pen, rect.X, topHeight + columHeight, rect.X + columnWidth, topHeight + columHeight);
                g.DrawString("价格", UIConstant.LableFont, Brushes.White, rect.X + 50 - 30, topHeight + (lineHeight - fontHeight) / 2);
                g.DrawString("成交量", UIConstant.LableFont, Brushes.White, rect.X + 50 + lw -50 , topHeight + (lineHeight - fontHeight) / 2);
                //g.DrawString("比例", UIConstant.LableFont, Brushes.White, rect.X + 50 + lw +2, topHeight + (lineHeight - fontHeight) / 2);
                g.DrawString("比例", UIConstant.LableFont, Brushes.White, rect.X + columnWidth - 50, topHeight + (lineHeight - fontHeight) / 2);

                //绘制竖线
                if (i > 0)
                    g.DrawLine(_pen, rect.X, topHeight, rect.X, this.Height);
                
                for (int j = 0; j < rowCnt; j++)
                {
                    cnt++;
                    if (cnt > pvList.Count)
                        continue;
                    split = pvList[cnt - 1];
                    

                    rect.Y = (j) * lineHeight + topHeight + columHeight;



                    text = string.Format(_priceFormat, split.Price);
                    si = g.MeasureString(text,UIConstant.QuoteFont);
                    if (split.Price > pr)
                        g.DrawString(text, UIConstant.QuoteFont, Brushes.Red, rect.X + (int)(50 - si.Width), rect.Top + (lineHeight - fontHeight) / 2);
                    else
                        g.DrawString(text, UIConstant.QuoteFont, Brushes.Green, rect.X + (int)(50 - si.Width), rect.Top + (lineHeight - fontHeight) / 2);

                    text = string.Format("{0:D}", split.Vol);
                    si = g.MeasureString(text, UIConstant.QuoteFont);
                    g.DrawString(text, UIConstant.QuoteFont, Brushes.Yellow, rect.X + (int)(50 + 1 * lw - si.Width), rect.Top + (lineHeight - fontHeight) / 2);
                    int ww = (int)(split.Vol * (lw - 4) / maxvol);
                    if (ww == 0)
                        ww = 1;
                    g.FillRectangle(Brushes.Aqua, rect.X + (50 + lw + 2), rect.Top + 2, ww, lineHeight - 4);

                    text = string.Format("{0:P0}", (double)split.Vol / (double)totalvol);
                    si = g.MeasureString(text, UIConstant.QuoteFont);
                    g.DrawString(text, UIConstant.QuoteFont, Brushes.Silver, (int)(rect.X + columnWidth - si.Width), rect.Top + (lineHeight - fontHeight) / 2);
                }


            }
        }
    }
}
