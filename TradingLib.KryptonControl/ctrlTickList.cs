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

namespace TradingLib.KryptonControl
{
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class ctrlTickList : System.Windows.Forms.Control
    {

        ILog logger = LogManager.GetLogger("ctrlTickList");
        public event EventHandler ExitView;
        public ctrlTickList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseClick += new MouseEventHandler(ctrlTickList_MouseClick);
            this.MouseMove += new MouseEventHandler(ctrlTickList_MouseMove);
            this.MouseWheel += new MouseEventHandler(ctrlTickList_MouseWheel);

            this.GotFocus += new EventHandler(ctrlTickList_GotFocus);
            this.LostFocus += new EventHandler(ctrlTickList_LostFocus);
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
            int dataColCnt = tradeSplitList.Count / rowCnt; 
            dataColCnt += tradeSplitList.Count % rowCnt > 0 ? 1 : 0;//计算所有数据的列数量

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
                if (startDataColumn > dataColCnt - columnCnt)
                    startDataColumn = dataColCnt - columnCnt;
                if (startDataColumn < 0)
                    startDataColumn = 0;
                this.Invalidate();
            }
        }
        void ctrlTickList_MouseWheel(object sender, MouseEventArgs e)
        {
            int rowCnt = (this.Height - topHeight - columHeight) / lineHeight;
            int dataColCnt = tradeSplitList.Count / rowCnt;
            dataColCnt += tradeSplitList.Count % rowCnt > 0 ? 1 : 0;

            //显示列数
            int columnCnt = this.Width / _defaultColumnWidth;
            if (dataColCnt > columnCnt)
            {
                if (e.Delta > 0)
                {
                    startDataColumn++;
                    
                }
                else
                {
                    startDataColumn--;
                }
                if (startDataColumn > dataColCnt - columnCnt)
                    startDataColumn = dataColCnt - columnCnt;
                if (startDataColumn < 0)
                    startDataColumn = 0;
                this.Invalidate();
            }

            //throw new NotImplementedException();
        }

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





        MDSymbol _symbol = new MDSymbol();
        public MDSymbol Symbol
        {
            get { return _symbol; }

            set {
                _symbol = value;
                this.Invalidate();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
        }


        int _defaultColumnWidth = 220;
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

        List<TradeSplit> tradeSplitList = new List<TradeSplit>();

        public void AddTrade(TradeSplit trade)
        {
            tradeSplitList.Add(trade);
            if (!update)
            {
                this.Invalidate();
            }
        }

        public void Clear()
        {
            tradeSplitList.Clear();
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

        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, this.ClientRectangle);

            int columnCnt = this.Width / _defaultColumnWidth;
            if (columnCnt == 0)
            {
                g.DrawString("控件太窄", UIConstant.QuoteFont, Brushes.Red, 10, 10);
                return;
            }
            int columnWidth = this.Width / columnCnt;

            //string t = string.Format("{0} {1}",columnCnt,columnWidth);
            //g.DrawString(t, UIConstant.QuoteFont, Brushes.Red, 10, 10);

            Rectangle rect = new Rectangle();
            float tWidth = 0;
            System.Drawing.Font font = UIConstant.QuoteFont;

            //string d = string.Format("{0}-{1}", columnCnt, columnWidth);
            //g.DrawString(d, UIConstant.QuoteFont, Brushes.Red, 10, 10);
            g.DrawString(string.IsNullOrEmpty(_symbol.Symbol) ? "000000" : _symbol.Symbol, UIConstant.QuoteFont, Brushes.Yellow, 20, (topHeight - fontHeight) / 2);
            g.DrawString(string.IsNullOrEmpty(_symbol.Name) ? "中国平安" : _symbol.Name, UIConstant.LableFont, Brushes.Yellow, 70, (topHeight - fontHeight) / 2);
            g.DrawString("请使用鼠标滚轮或上下键翻页", UIConstant.HelpFont, Brushes.Gray, 140, (topHeight - fontHeight) / 2 + 2);

            g.DrawString("按Esc键返回", UIConstant.HelpFont, Brushes.Red, this.Width - 110, (topHeight - fontHeight) / 2 + 3);

            //绘制关闭按钮
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            DrawCloseBotton(g,_mouseInClose?Color.Orange: Color.Red, p, btnSize);

            int rowCnt = (this.Height - topHeight - columHeight) / lineHeight;

            string text = string.Empty;
            int lw = (columnWidth - 90) / 2;
            int cnt = rowCnt*startDataColumn;//从0列开始则cnt起始为0，如果从第一列开始显示则对应的位为rowCnt则图形第一列显示的是数据第二列
            TradeSplit split = null;
            for (int i = 0; i < columnCnt; i++)
            {
                rect.X = i * columnWidth;

                if (i == columnCnt - 1)
                    columnWidth = this.Width - (columnCnt - 1) * columnWidth;

                

                //绘制顶部列标题
                g.DrawLine(_pen, rect.X, topHeight, rect.X + columnWidth, topHeight);
                g.DrawLine(_pen, rect.X, topHeight + columHeight, rect.X + columnWidth, topHeight + columHeight);
                g.DrawString("时间", UIConstant.LableFont, Brushes.White, rect.X + 60 - 40, topHeight + (lineHeight - fontHeight) / 2);
                g.DrawString("价格", UIConstant.LableFont, Brushes.White, rect.X + 60 + lw - 40, topHeight + (lineHeight - fontHeight) / 2);
                g.DrawString("数量", UIConstant.LableFont, Brushes.White, rect.X + columnWidth - 50, topHeight + (lineHeight - fontHeight) / 2);

                //绘制竖线
                if (i > 0)
                    g.DrawLine(_pen, rect.X, topHeight, rect.X, this.Height);
                
                for (int j = 0; j < rowCnt; j++)
                {
                    cnt++;
                    if (cnt >tradeSplitList.Count)
                        continue;
                    split = tradeSplitList[cnt-1];
                    

                    rect.Y = (j) * lineHeight + topHeight + columHeight;



                    text = string.Format("{0:D2}:{1:D2}:{2:D2}", split.Time / 100, split.Time % 100, 1);
                    tWidth = g.MeasureString(text, font).Width;
                    g.DrawString(text, UIConstant.QuoteFont, _brushTime, rect.X + 60 - tWidth, rect.Y + (lineHeight - fontHeight) / 2);


                    text = string.Format("{0:F2}", split.Price);
                    tWidth = g.MeasureString(text, font).Width;
                    if (split.Price == _symbol.PreClose)
                    {
                        _brushPrice.Color = Color.White;
                    }
                    else if (split.Price > _symbol.PreClose)
                    {
                        _brushPrice.Color = UIConstant.ColorUp;
                    }
                    else
                    {
                        _brushPrice.Color = UIConstant.ColorDown;
                    }
                    g.DrawString(text, font, _brushPrice, rect.X + 60 + lw - tWidth, rect.Y + (lineHeight - fontHeight) / 2);


                    text = string.Format("{0:D}", split.Vol);
                    tWidth = g.MeasureString(text, font).Width;
                    g.DrawString(text, font, Brushes.Gray, rect.X + 60 + 2 * lw - tWidth, rect.Y + (lineHeight - fontHeight) / 2);

                    if (split.Flag == 1)
                        text = "B";
                    else
                        text = "S";
                    tWidth = g.MeasureString(text, font).Width;
                    if (split.Flag == 1)
                        g.DrawString(text, font, Brushes.Red, rect.X + columnWidth - 25, rect.Y + (lineHeight - fontHeight) / 2);
                    else
                        g.DrawString(text, font, Brushes.Lime, rect.X + columnWidth - 25, rect.Y + (lineHeight - fontHeight) / 2);

                }


            }
        }
    }
}
