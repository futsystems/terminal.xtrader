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
    public class QrySymbolInfoArgs:EventArgs
    {

        public QrySymbolInfoArgs(MDSymbol symbol,SymbolInfoType type)
        {
            this.Symbol = symbol;
            this.Type = type;
        }
        public MDSymbol Symbol {get;set;}

        public SymbolInfoType Type {get;set;}
    }
    internal class Cell
    {
        public Cell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        public int Row { get; set; }
        public int Col { get; set; }
    }
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class ctrlSymbolInfo : System.Windows.Forms.Control, IView
    {

        public event EventHandler<QrySymbolInfoArgs> QrySymbolInfo;

        EnumViewType vietype = EnumViewType.BasicInfo;

        public EnumViewType ViewType { get { return vietype; } }

        ILog logger = LogManager.GetLogger("ctrlTickList");
        public event EventHandler ExitView;


        public ctrlSymbolInfo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseClick += new MouseEventHandler(ctrlTickList_MouseClick);
            this.MouseMove += new MouseEventHandler(ctrlTickList_MouseMove);

            this.GotFocus += new EventHandler(ctrlTickList_GotFocus);
            this.LostFocus += new EventHandler(ctrlTickList_LostFocus);

            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            for (int i = 0; i < 13; i++)
            {
                typeList.Add(new SymbolInfoType("00001", "基本信息", 0, 0,0));
            }
        }

        int _startline = 0;
        void ctrlSymbolInfo_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                _startline += 2;
            }
            else
            {
                _startline -=2;
            }
            if (_startline < 0) _startline = 0;

            this.Invalidate();

        }

        List<SymbolInfoType> typeList = new List<SymbolInfoType>();
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    {
                        Scroll(false);
                        break;
                    }
                case Keys.Down:
                    {
                        Scroll(true);
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
                Scroll(false);
            }
            else
            {
                Scroll(true);
            }

            this.Invalidate();
        }

        void Scroll(bool up)
        {
            if (up)
            {
                _startline +=2;
            }
            else
            {
                _startline -=2;
            }
            if (_startline < 0) _startline = 0;
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

            //判定当前选择的类别
            Point symbolPoint = new Point(10, 5);
            Point btnPoint = new Point(symbolPoint.X + btnWidth + 10, 5);

            btnCol = (this.Width - btnWidth * 2) / btnWidth;
            btnCol = btnCol <= 8 ? btnCol : 8;

            if (e.X > btnPoint.X+5 && e.X < btnPoint.X + btnCol * btnWidth -5  && e.Y > btnPoint.Y+2  && e.Y < btnPoint.Y + 2 * btnHeight-2)
            {
                int col = (e.X - btnPoint.X) / btnWidth + 1;
                int row = (e.Y - btnPoint.Y) / btnHeight + 1;

                int location = (row - 1) * btnCol + col;
                this.Invalidate();
            }

        }

        Cell selectedCell = new Cell(0, 0);
        void ctrlTickList_MouseClick(object sender, MouseEventArgs e)
        {
            Focus();

            //点击退出按钮
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            if (e.X > p.X && e.X < p.X + btnSize && e.Y > p.Y && e.Y < p.Y + btnSize)
            {
                if (ExitView != null)
                {
                    ExitView(this, null);
                }
            }

            //判定当前选择的类别
            Point symbolPoint = new Point(10, 5);
            Point btnPoint = new Point(symbolPoint.X + btnWidth + 10, 5);

            btnCol = (this.Width - btnWidth * 2) / btnWidth;
            btnCol = btnCol <= 8 ? btnCol : 8;

            if (e.X > btnPoint.X + 5 && e.X < btnPoint.X + btnCol * btnWidth - 5 && e.Y > btnPoint.Y + 2 && e.Y < btnPoint.Y + 2 * btnHeight - 2)
            {
                int col = (e.X - btnPoint.X) / btnWidth + 1;
                int row = (e.Y - btnPoint.Y) / btnHeight + 1;

                int location = (row - 1) * btnCol + col;
                //msg = string.Format("row:{0} col:{1} location:{2}", row, col, location);
                if (location <=typeList.Count)
                {
                    SelectType(location);
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 序号从1开始
        /// 1-16
        /// </summary>
        /// <param name="location"></param>
        public void SelectType(int location)
        {
            if (location > typeList.Count) return;

            btnCol = (this.Width - btnWidth * 2) / btnWidth;
            btnCol = btnCol <= 8 ? btnCol : 8;
            if (btnCol <= 0) return;

            int inrow = location / btnCol;
            int incol = location % btnCol;
            inrow = inrow + (incol > 0 ? 1 : 0);
            incol = incol == 0 ? btnCol : incol;//为0标示整除 在最后一列

            
            SymbolInfoType type = typeList[location-1];
            if(type != null)
            {
                selectedCell = new Cell(inrow, incol);
                if (QrySymbolInfo != null)
                {
                    QrySymbolInfo(this, new QrySymbolInfoArgs(_symbol, type));
                }
            }
        }





        MDSymbol _symbol = new MDSymbol();
        public void SetSymbol(MDSymbol symbol)
        {
            _symbol = symbol;
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
        }

        string info = string.Empty;
        List<string> infolist = new List<string>();
        public void SetInfo(string msg)
        {
            _startline = 0;
            info = msg;
            infolist = new List<string>(msg.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            Invalidate();
        }



        Pen _pen = new Pen(UIConstant.ColorLine, 1);

        int topHeight = 30;
        //int columHeight = 22;
        //int lineHeight = 22;
        int fontHeight = UIConstant.QuoteFont.Height;
        //SolidBrush _brushTime = new SolidBrush(Color.Silver);
        SolidBrush _brush = new SolidBrush(Color.Silver);

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

       

        public void Clear()
        {
            typeList.Clear();
            info = string.Empty;
            infolist.Clear();
            _startline = 0;
            if (!update)
            {
                this.Invalidate();
            }
        }

        public void AddType(List<SymbolInfoType> types)
        {
            typeList.AddRange(types);
            SelectType(1);
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





        int btnWidth = 100;
        int btnHeight = 25;


        int btnCol = 0;

        StringFormat format = null;
        Color _lineColor = Color.FromArgb(255, 255, 150);
        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {

           
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, this.ClientRectangle);

            btnCol = (this.Width - btnWidth * 2) / btnWidth;
            btnCol = btnCol <= 8 ? btnCol : 8;
            if (btnCol <= 0)
            {
                g.DrawString("SymbolInfo", UIConstant.LableFont, Brushes.Yellow, 10, 10);
                return;
            }

            System.Drawing.Font font = UIConstant.QuoteFont;

            Point symbolPoint = new Point(10, 5);
            Point btnPoint = new Point(symbolPoint.X + btnWidth + 10, 5);

            _pen.Color = _lineColor;

            //绘制按钮框
            g.DrawRectangle(_pen, symbolPoint.X, symbolPoint.Y, btnWidth, btnHeight * 2 +1);
            g.DrawLine(_pen, symbolPoint.X, symbolPoint.Y + btnHeight, symbolPoint.X + btnWidth, symbolPoint.Y + btnHeight);

            g.DrawRectangle(_pen, btnPoint.X, btnPoint.Y, btnWidth * btnCol, btnHeight * 2 + 1);
            g.DrawLine(_pen, btnPoint.X, btnPoint.Y + btnHeight, btnPoint.X + btnWidth * btnCol, btnPoint.Y + btnHeight);

            for (int i = 1; i < btnCol; i++)
            {
                g.DrawLine(_pen, btnPoint.X + btnWidth * i, btnPoint.Y, btnPoint.X + btnWidth * i, btnPoint.Y + btnHeight * 2 + 1);
            }

            //绘制合约
            g.DrawString(string.IsNullOrEmpty(_symbol.Symbol) ? "000000" : _symbol.Symbol, UIConstant.QuoteFont, Brushes.Yellow, symbolPoint.X+btnWidth/2, symbolPoint.Y + (btnHeight-fontHeight)/2, format );
            g.DrawString(string.IsNullOrEmpty(_symbol.Name) ? "中国平安" : _symbol.Name, UIConstant.LableFont, Brushes.Yellow, symbolPoint.X + btnWidth / 2, symbolPoint.Y + btnHeight + (btnHeight - fontHeight) / 2, format);


            //绘制关闭按钮
            int btnSize = 14;
            Point p = new Point(this.Width - (btnSize + (topHeight - btnSize) / 2), (topHeight - btnSize) / 2);
            DrawCloseBotton(g, _mouseInClose ? Color.Orange : Color.Red, p, btnSize);

            int rows = typeList.Count / btnCol;//需要几行输出
            rows = rows + typeList.Count % btnCol > 0 ? 1 : 0;
            //绘制按钮信息
            for (int i = 0; i < typeList.Count; i++)
            {
                SymbolInfoType type = typeList[i];
                int inrow = (i + 1) / btnCol;
                int incol = (i + 1) % btnCol;
                inrow = inrow + (incol > 0 ? 1 : 0);
                incol = incol == 0 ? btnCol : incol;//为0标示整除 在最后一列
                if (inrow > 2)
                    continue;
                if (selectedCell.Row == inrow && selectedCell.Col == incol)
                {
                    _brush.Color = Color.Red;
                }
                else
                {
                    _brush.Color = _lineColor;
                }
                g.DrawString(type.Title/* + string.Format("{0} {1}", inrow, incol)*/, UIConstant.LableFont, _brush, btnPoint.X + (incol - 1) * btnWidth + btnWidth / 2, btnPoint.Y + (inrow - 1) * btnHeight + (btnHeight - fontHeight) / 2, format);

            }

            g.DrawLine(_pen, 0, btnPoint.Y + 2 * btnHeight + 10, this.Width, btnPoint.Y + 2 * btnHeight + 10);

            //绘制文字
            int txtWidth = 700;
            txtWidth = txtWidth < this.Width ? txtWidth : this.Width;
            Rectangle textRect = new Rectangle(0, btnPoint.Y + 2 * btnHeight + 20, txtWidth, this.Height);
            SizeF size = new SizeF();
            int current = 0;
            _brush.Color = _lineColor;
            foreach (var line in infolist)
            {
                if (current < _startline)
                {
                    current++;
                    continue;
                }
                
                size = g.MeasureString(line, UIConstant.LableFont);
                int r = (int)size.Width / txtWidth +1;

                g.DrawString(line, UIConstant.LableFont,_brush, textRect);
                textRect.Y = textRect.Y + r * (int)size.Height;
                if (textRect.Y > this.Height)
                {
                    //logger.Info("no need to drawstring");
                    return;
                }
            }
        }
    }
}
