﻿using System;
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
            SetCaretPos(1, 2);
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

        bool _txtMouseDown = false;

        bool TxtMouseDown { get { return _txtMouseDown; }
            set {
                _txtMouseDown = value;
                this.Invalidate();
            }
        }

        int _mouseDownX = 0;
        int _mouseDownY = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            
            mousedown = true;
            _mouseDownX = e.X;
            _mouseDownY = e.Y;

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

                //文本区域MouseDown
                if (e.X > 0 && e.X < this.Width - 15 - 1 && e.Y > 0 && e.Y < this.Height)
                {
                    if(!this.TxtMouseDown)
                        this.TxtMouseDown = true;
                }

                if (_selected)
                {
                    logger.Info("clear selected");
                    _selected = false;
                }
            }

            base.OnMouseDown(e);
            this.Focus();

        }


        decimal _increment = 1;
        decimal _mindval = 0;
        decimal _maxdval = 1000000;
        int _selectionStart = 0;//光标所处位置
        /// <summary>
        /// 设置当前光标位置
        /// </summary>
        /// <param name="start"></param>
        void InternalSetSelectionStart(int start)
        {
            if (start < 0) return;
            if (start > value.Length) return;
            _selectionStart = start;
        }

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

            if(this.TxtMouseDown)
                this.TxtMouseDown = false;

            mousedown = false;

            if (_charSelect)
            {
                logger.Info("exit chart select status");
                //设定光标位置为当前选中位置
                int v = _selectionStart;
                this.InternalSetSelectionStart(_SelectionEnd);
                _SelectionEnd = v;
                _charSelect = false;
                this.Invalidate();
            }

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
                    //value += e.KeyChar;
                    value = value.Substring(0, _selectionStart) + e.KeyChar + (_selectionStart >= value.Length ? "" : value.Substring(_selectionStart));
                    _selectionStart++;//插入字符后selectStart后移一位

                    this.Invalidate();
                }
                
            }
            if (keyData == Keys.Back)
            {
                if (value.Length > 0)
                {
                    if (_selectionStart - 1 >= 0)
                    {
                        //value = value.Substring(0, value.Length - 1);
                        value = value.Substring(0, _selectionStart - 1) + (_selectionStart >= value.Length ? "" : value.Substring(_selectionStart));
                        _selectionStart--;//删除一个字符后 selectStart前移一位;

                        this.Invalidate();
                    }
                }
            }
            
            //小数点
            if (e.KeyChar == '.')
            {
                if (!value.Contains('.'))
                {
                    value = value.Substring(0, _selectionStart) + e.KeyChar + (_selectionStart >= value.Length ? "" : value.Substring(_selectionStart));
                    _selectionStart++;//插入字符后selectStart后移一位
                    this.Invalidate();
                }
            }
            base.OnKeyPress(e);
        }



        int maxLen = 10;


        

        string value = string.Empty;

        
        int _currentX = 0;
        int _currentY = 0;


        bool _charSelect = false;
        int _SelectionEnd = 0;
        bool _selected = false;
        /// <summary>
        /// 通过鼠标移动来捕捉当前是否在按钮之上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FListBox_MouseMove(object sender, MouseEventArgs e)
        {
            _currentX = e.X;
            _currentY = e.Y;


            //位于按钮区域
            if (e.X > this.Width - 15 - 1 && e.Y > 0 && e.X < this.Width && e.Y < this.Height)
            {
                this.Cursor = Cursors.Default;
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
                this.Cursor = Cursors.IBeam;
                this.UpBtnMouseOver = false;
                this.DnBtnMouseOver = false;

                //处于TxtMouseDown状态 选择字符操作
                if (this.TxtMouseDown && Math.Abs(_currentX - _mouseDownX) > 3 && !_charSelect)
                {
                    logger.Info("start select chars");
                    _charSelect = true;
                    this.Invalidate();
                }
                if (_charSelect)
                {
                    this.Invalidate();
                }
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

        /// <summary>
        /// 获得X坐标对应的字符串位置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        int GetCurrentLocation(Graphics g, int x)
        {
            bool inTxt = false;
            int location = 0;
            SizeF size;
            for (int i = 1; i <= value.Length; i++)
            {
                string tmp = value.Substring(0, i);
                size = g.MeasureString(tmp, _font);
                if (size.Width >= _currentX)
                {
                    inTxt = true;
                    location = i - 1;
                    break;
                }
            }
            //当前坐标不在字符串中则为整个字符串长度
            if (!inTxt)
            {
                location = value.Length;
            }
            return location;
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
            //绘制外部边框
            g.DrawRectangle(_pen, rectBorder);

            //绘制右侧按钮
            g.DrawImage(GetUpArrowImg(), new Point(this.Width - 1 - 15, 1));//上箭头
            g.DrawImage(GetDnArrowImg(), new Point(this.Width - 1 - 15, 1 + 9));//下箭头

            
            //设定当前光标
            if (this.TxtMouseDown && !_charSelect)
            {

                int locatioin = GetCurrentLocation(g, _currentX);
                InternalSetSelectionStart(locatioin);
                logger.Info(string.Format("_currentX:{0} cursor location:{1}", _currentX, locatioin));
            }

            string ss = value.Substring(0, _selectionStart);
            size = g.MeasureString(ss, _font);

            int selectionStartX = (int)size.Width + 1;

            ss = value.Substring(0, _SelectionEnd);
            size = g.MeasureString(ss, _font);
            int selectionEndX = (int)size.Width + 1;

            if (this.Focused)
            {
                SetCaretPos(selectionStartX, 2);

            }

            //鼠标长按 拖动鼠标选择字符串
            if (_charSelect)
            {
                //获得当前坐标对应的字符串位置
                _SelectionEnd = GetCurrentLocation(g,_currentX);

                //根据位置绘制阴影
                if (_SelectionEnd < _selectionStart)
                {

                    Rectangle selectRect = new Rectangle(_currentX, 1, selectionStartX - _currentX, this.Height);
                    g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                }
                if (_SelectionEnd > _selectionStart)
                {
                    Rectangle selectRect = new Rectangle(selectionStartX, 1, _currentX - selectionStartX, this.Height);
                    g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);

                }
                
                logger.Info("SelectEnd:" + _SelectionEnd.ToString() +" SelectStart:"+_selectionStart.ToString());
                _selected = true;
            }

            //不在选择状态 则显示当前选中的
            if (!_charSelect && _selected)
            {
                if (selectionStartX > selectionEndX)
                {
                    Rectangle selectRect = new Rectangle(selectionEndX, 1, selectionStartX - selectionEndX, this.Height);
                    g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                }
                if (selectionStartX < selectionEndX)
                {
                    Rectangle selectRect = new Rectangle(selectionStartX, 1, selectionEndX - selectionStartX, this.Height);
                    g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                }
            }

            _brush.Color = _itemColor;
            g.DrawString(value, _font, _brush, 0, txtOffset);

        }
    }
}
