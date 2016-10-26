using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using TradingLib.MarketData;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    enum MouseStatus
    { 
        UPBtnMouseOver,
        UpBtnMouseClick,
    }
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class FPriceInput : System.Windows.Forms.Control
    {

        ILog logger = LogManager.GetLogger("FPriceInput");

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CreateCaret(IntPtr hWnd, IntPtr hBmp, int w, int h);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyCaret();

        public FPriceInput()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;
            itemFormat.LineAlignment = StringAlignment.Center;

            this.Height = 20;
            this.MouseClick += new MouseEventHandler(FListBox_MouseClick);
            this.MouseMove += new MouseEventHandler(FListBox_MouseMove);

        }

        bool _upBtnMouseDown = false;
        bool _dnBtnMouseDown = false;

        bool _upBtnMouseOver = false;
        bool _dnBtnMouseOver = false;

        bool UpBtnMouseDown
        {
            get { return _upBtnMouseDown; }
            set
            {
                bool r = value != _upBtnMouseDown;
                _upBtnMouseDown = value;
                if (r) this.Invalidate();
            }
        }
        bool DnBtnMouseDown
        {
            get { return _dnBtnMouseDown; }
            set
            {
                bool r = value != _dnBtnMouseDown;
                _dnBtnMouseDown = value;
                if (r) this.Invalidate();
            }
        }
        bool UpBtnMouseOver
        {
            get { return _upBtnMouseOver; }
            set
            {
                bool r = value != _upBtnMouseOver;
                _upBtnMouseOver = value;
                if (r) this.Invalidate();
            }
        }
        bool DnBtnMouseOver
        {
            get { return _dnBtnMouseOver; }
            set
            {
                bool r = value != _dnBtnMouseOver;
                _dnBtnMouseOver = value;
                if (r) this.Invalidate();
            }
        }


        bool mousedown = false;

        protected override void OnGotFocus(EventArgs e)
        {
            logger.Info("got focus cursor x:"+Cursor.Position.X.ToString() + " y:"+Cursor.Position.Y.ToString());

            CreateCaret(this.Handle, IntPtr.Zero, 1, this.Height - 4);
            SetCaretPos(2, 2);
            ShowCaret(this.Handle);
            base.OnGotFocus(e);
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            logger.Info("lost focus");
            DestroyCaret();
            base.OnLostFocus(e);
            this.Invalidate();
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            
            mousedown = true;
            //在特定区域内 检查按钮是否被按下 其余区域按钮被弹起
            if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < this.Height)
            {
                if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < 10)
                {
                    this.UpBtnMouseDown = true;
                    this.OnUpArrowDown();
                }
                else
                {
                    this.UpBtnMouseDown = false;
                }
                if (e.X > this.Width - 15 - 1 && e.Y < 20 && e.X < this.Width && e.Y > 10)
                {
                    this.DnBtnMouseDown = true;
                    this.OnDnArrowDown();
                }
                else
                {
                    this.DnBtnMouseDown = false;
                }
            }
            else
            {
                this.DnBtnMouseDown = false;
                this.UpBtnMouseDown = false;
            }

            base.OnMouseDown(e);
            this.Focus();

        }


        decimal _increment = 1;
        decimal _mindval = 0;
        decimal _maxdval = 1000000;
        int _selectionStart = 0;//光标所处位置


        void OnUpArrowDown()
        { 
            decimal dvalue=0;
            if (!decimal.TryParse(value, out dvalue)) dvalue = 0;
            if (dvalue + _increment <= _maxdval)
            {
                dvalue += _increment;
                value = dvalue.ToString();

                this.Invalidate();
            }
        }


        void OnDnArrowDown()
        {
            decimal dvalue = 0;
            if (!decimal.TryParse(value, out dvalue)) dvalue = 0;
            //默认为能小于0
            if (dvalue- _increment >= _mindval)
            {
                dvalue -= _increment;
                value = dvalue.ToString();

                this.Invalidate();
            }
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            //鼠标up则所有按钮MoouseDonwn为False
            this.DnBtnMouseDown = false;
            this.UpBtnMouseDown = false;

            mousedown = false;



            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            //鼠标离开控件则所有按钮MouseOver为False
            this.UpBtnMouseOver = false;
            this.DnBtnMouseOver = false;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            base.OnMouseLeave(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            logger.Info("key:" + e.KeyChar);
            var keyData = (Keys)e.KeyChar;
            if ((keyData >= Keys.D0 && keyData <= Keys.D9) || (keyData >= Keys.NumPad0 && keyData <= Keys.NumPad9))
            {
                if (value.Length < maxLen)
                {
                    value += e.KeyChar;


                    this.Invalidate();
                }
                
            }
            if (keyData == Keys.Back)
            {
                if (value.Length > 0)
                {
                    value = value.Substring(0, value.Length - 1);

                    this.Invalidate();
                }
            }
            
            //小数点
            if (e.KeyChar == '.')
            {
                if (!value.Contains('.'))
                {
                    value += e.KeyChar;
                    this.Invalidate();
                }
            }
            base.OnKeyPress(e);
        }



        int maxLen = 10;


        

        string value = string.Empty;

        /// <summary>
        /// 通过鼠标移动来捕捉当前是否在按钮之上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FListBox_MouseMove(object sender, MouseEventArgs e)
        {
            //位于按钮区域
            if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < this.Height)
            {
                if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < 10)
                {
                    this.UpBtnMouseOver = true;
                }
                else
                {
                    this.UpBtnMouseOver = false;
                }
                if (e.X > this.Width - 15 - 1 && e.Y < 20 && e.X < this.Width && e.Y > 10)
                {
                    this.DnBtnMouseOver = true;
                }
                else
                {
                    this.DnBtnMouseOver = false;
                }
            }
            else
            {
                this.UpBtnMouseOver = false;
                this.DnBtnMouseOver = false;
            }

        }

        void FListBox_MouseClick(object sender, MouseEventArgs e)
        {
            //位于按钮区域
            if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < this.Height)
            {

            }
            else
            {
                //判定当前光标位置
                for (int i = 0; i < value.Length; i++)
                { 
                    
                }
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
        Font _font = new Font("宋体", 10f,FontStyle.Bold);
        Color _itemColor = Color.FromArgb(4, 60, 109);
        SolidBrush _brush = new SolidBrush(Color.Black);
        SolidBrush _mouseOverBrush = new SolidBrush(Color.FromArgb(51, 153, 255));
        StringFormat itemFormat = new StringFormat();

        int _startIdx = 0;
        Pen _pen = new Pen(Color.Black);


        Bitmap GetUpArrowImg()
        {
            if (this.UpBtnMouseDown) return Properties.Resources.arrow_up_mouse_down;
            if (this.UpBtnMouseOver) return Properties.Resources.arrow_up_mouse_over;
            return Properties.Resources.arrow_up;
        }

        Bitmap GetDnArrowImg()
        {
            if (this.DnBtnMouseDown) return Properties.Resources.arrow_down_mouse_down;
            if (this.DnBtnMouseOver) return Properties.Resources.arrow_down_mouse_over;
            return Properties.Resources.arrow_down;
        }

        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, this.ClientRectangle);

            int txtOffset = (this.Height - _font.Height) / 2;
            Point location = new Point(0,0);
            location.Y += 5;

            SizeF size = g.MeasureString(value, _font);
            Rectangle rectBorder = this.ClientRectangle;
            rectBorder.Height--;
            rectBorder.Width--;
            _pen.Color = Constants.BorderColor;
            g.DrawRectangle(_pen, rectBorder);

            
            //绘制右侧按钮
            g.DrawImage(GetUpArrowImg(), new Point(this.Width - 1 - 15, 1));//上箭头
            g.DrawImage(GetDnArrowImg(), new Point(this.Width - 1 - 15, 1 + 9));//下箭头

            _brush.Color = _itemColor;
            g.DrawString(value, _font, _brush, 0, txtOffset);

            if (this.Focused)
            {
                SetCaretPos((int)size.Width + 2, 2);
                
            }
        }
    }
}
