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

namespace TradingLib.XTrader.Future
{
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class FListBox : System.Windows.Forms.Control
    {

        ILog logger = LogManager.GetLogger("ctrlTickList");
        


        public FListBox()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;

            itemFormat.LineAlignment = StringAlignment.Center;

            this.MouseClick += new MouseEventHandler(FListBox_MouseClick);
        }




        void FListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 0 && e.X < this.Width - 2)
            {
                int rowIdx = e.Y / lineHeight;

                if ((_startIdx + rowIdx) < this.Items.Count)
                {
                    string val = this.Items[rowIdx + _startIdx];
                    if (ItemSelected != null)
                    {
                        ItemSelected(val);
                    }
                }
            }
        }

        public int ItemHeight 
        { get { return lineHeight; } 
            set
            {
                lineHeight = value;
                this.Invalidate();
            } 
        }


        public void Clear()
        {
            _startIdx = 0;
            _rowIdMouseOver = 0;
            _itemIdxMouseOver = 0;
            this.Items.Clear();
        }

        /// <summary>
        /// 显示的总行数
        /// </summary>
        public int ShowCount { get {return this.Height / lineHeight; } }

        int _rowIdMouseOver = 0;
        int _itemIdxMouseOver = 0;
        protected override void OnMouseMove(MouseEventArgs e)
        {
           
            int rowIdx = e.Y / lineHeight;
            if (rowIdx != _rowIdMouseOver)
            {
                _rowIdMouseOver = rowIdx;
                _itemIdxMouseOver = _startIdx + _rowIdMouseOver;
                this.Invalidate();
            }
            
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScrollRowDown();
            }
            else
            {
                ScrollRowUp();
            }
        }


        public int StartIndex {
            get { return _startIdx; }
            set 
            { 
                _startIdx = value;
                this.Invalidate();
            }
        }

        void ScrollRowUp()
        {
            int totalNum = this.Height / lineHeight;
            if ((_startIdx + totalNum) < this.Items.Count)
            {
                _startIdx++;
                this.Invalidate();
            }
        }
        void ScrollRowDown()
        {
            int totalNum = this.Height / lineHeight;
            if (_startIdx>0)
            {
                _startIdx--;
                this.Invalidate();
            }
        }


        public event Action<string> ItemSelected;

        string _selectedItem = string.Empty;
        public string SelectedItem
        {
            get { return _selectedItem; }
        }
        List<string> _items = new List<string>();

        public List<string> Items { get { return _items; } }

        int lineHeight = 16;
        Font _font = new Font("宋体", 9f,FontStyle.Bold);
        Color _itemColor = Color.FromArgb(4, 60, 109);
        SolidBrush _brush = new SolidBrush(Color.Black);
        SolidBrush _mouseOverBrush = new SolidBrush(Color.FromArgb(51, 153, 255));
        StringFormat itemFormat = new StringFormat();

        int _startIdx = 0;
        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, this.ClientRectangle);

            Point location = new Point(0,0);
            location.Y +=1;

            int fheight = _font.Height;
            int offset = (lineHeight-fheight)/2;
            int totalNum = this.Height / lineHeight;
            totalNum = Math.Min(totalNum, this.Items.Count);


            for (int i = _startIdx; i < _startIdx + totalNum;i++ )
            {
                string val = this.Items[i];

                if (i == _itemIdxMouseOver)
                {
                    g.FillRectangle(_mouseOverBrush, new Rectangle(0 + 1, location.Y, this.Width - 2, lineHeight));
                }

                if (i == _itemIdxMouseOver)
                {
                    _brush.Color = Color.White;
                }
                else
                {
                    _brush.Color = _itemColor;
                }
                g.DrawString(val, _font, _brush, location.X, location.Y + offset);
                location.Y += lineHeight;
            }
        }
    }
}
