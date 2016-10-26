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
    public partial class FNumberInput : System.Windows.Forms.Control, IPopupControlHost
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

        public FNumberInput()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;
            itemFormat.LineAlignment = StringAlignment.Center;

            this.Height = 20;
            this.MouseClick += new MouseEventHandler(FListBox_MouseClick);
            this.MouseMove += new MouseEventHandler(FListBox_MouseMove);

        }

        #region 外部属性
        public bool ShowTop { get; set; }
        public decimal MinVal { get { return _mindval; } set { _mindval = value; } }
        public decimal MaxVal { get { return _maxdval; } set { _maxdval = value; } }
        public int DecimalPlace { get { return _decimalplace; } set { _decimalplace = value; } }

        public string TxtValue { get { return _txtvalue; } set { _txtvalue = value; } }
        int _decimalplace = 2;
        decimal _increment = 1;
        decimal _mindval = 0;
        decimal _maxdval = 1000000;
        string _txtvalue = string.Empty;
        #endregion

        bool _txtMode = false;
        public void SetTxtVal(string txt)
        {
            _txtMode = true;
            _txtvalue = txt;
            _selectionStart = _txtvalue.Length;
            _SelectionEnd = 0;
            this.Invalidate();
        }

        #region 内部状态变量
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

        bool _txtMouseDown = false;

        bool TxtMouseDown
        {
            get { return _txtMouseDown; }
            set
            {
                _txtMouseDown = value;
                this.Invalidate();
            }
        }
        //记录鼠标点击时坐标
        int _mouseDownX = 0;
        int _mouseDownY = 0;

        //记录鼠标当前位置
        int _currentX = 0;
        int _currentY = 0;

        //是否处于字符选择中状态 按住鼠标拖动执行选择操作
        bool _charSelect = false;
        //是否处于字符选择完毕状态 选择完毕松开按键后被选中的字符处于高亮状态
        bool _selected = false;


        int _selectionStart = 0;//光标所处开始
        int _SelectionEnd = 0;//光标所处结束
        

        #endregion

        #region 覆写 鼠标 键盘操作处理
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
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

                if (!_txtMode)
                {
                    //文本区域MouseDown
                    if (e.X > 0 && e.X < this.Width - 15 - 1 && e.Y > 0 && e.Y < this.Height)
                    {
                        if (!this.TxtMouseDown)
                            this.TxtMouseDown = true;
                    }

                    if (_selected)
                    {
                        logger.Info("clear selected");
                        _selected = false;
                    }
                }
                else
                {
                    _selected = true;
                }
                ShowDropDown();
            }
            
            base.OnMouseDown(e);
            this.Focus();

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //鼠标up则所有按钮MoouseDonwn为False
            this.DnBtnMouseDown = false;
            this.UpBtnMouseDown = false;

            if (this.TxtMouseDown)
                this.TxtMouseDown = false;

            if (_charSelect)
            {
                logger.Info("exit chart select status");
                //设定光标位置为当前选中位置
                if (_selectionStart > _SelectionEnd)
                {
                    int v = _selectionStart;
                    this.InternalSetSelectionStart(_SelectionEnd);
                    _SelectionEnd = v;
                }
                _charSelect = false;
                this.Invalidate();
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //鼠标离开控件则所有按钮MouseOver为False
            this.UpBtnMouseOver = false;
            this.DnBtnMouseOver = false;

            //this.HideDropDown();
            base.OnMouseLeave(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            logger.Info("key:" + e.KeyChar);
            var keyData = (Keys)e.KeyChar;
            if ((keyData >= Keys.D0 && keyData <= Keys.D9) || (keyData >= Keys.NumPad0 && keyData <= Keys.NumPad9))
            {
                if (!_txtMode)//非文字模式可编辑数字 否则不接受字符
                {
                    if (_selected)
                    {
                        if (_selectionStart == 0 && _SelectionEnd == _txtvalue.Length)
                        {
                            _txtvalue = string.Empty + e.KeyChar;
                        }
                        else if (_selectionStart == 0 && _SelectionEnd < _txtvalue.Length)
                        {
                            _txtvalue = e.KeyChar + _txtvalue.Substring(_SelectionEnd, _txtvalue.Length - _SelectionEnd);
                        }
                        else if (_selectionStart > 0 && _SelectionEnd == _txtvalue.Length)
                        {
                            _txtvalue = _txtvalue.Substring(0, _selectionStart) + e.KeyChar;
                        }
                        else
                        {
                            _txtvalue = _txtvalue.Substring(0, _selectionStart) + e.KeyChar + _txtvalue.Substring(_SelectionEnd, _txtvalue.Length - _SelectionEnd);
                        }
                        _selectionStart++;//插入字符后selectStart后移一位
                        if (_selectionStart > _txtvalue.Length)
                            _selectionStart = _txtvalue.Length;
                        _selected = false;
                        this.Invalidate();
                    }
                    else
                    {
                        //value += e.KeyChar;
                        _txtvalue = _txtvalue.Substring(0, _selectionStart) + e.KeyChar + (_selectionStart >= _txtvalue.Length ? "" : _txtvalue.Substring(_selectionStart));
                        decimal dvalue = 0;
                        if (!decimal.TryParse(_txtvalue, out dvalue)) dvalue = 0;
                        if (dvalue > this.MaxVal)
                            _txtvalue = this.MaxVal.ToString();
                        _selectionStart++;//插入字符后selectStart后移一位
                        if (_selectionStart > _txtvalue.Length)
                            _selectionStart = _txtvalue.Length;

                        this.Invalidate();
                    }
                }


            }
            if (keyData == Keys.Back)
            {
                if (_txtvalue.Length > 0)
                {
                    if (!_txtMode)
                    {
                        if (_selected)//删除选中字符
                        {
                            if (_selectionStart == 0 && _SelectionEnd == _txtvalue.Length)
                            {
                                _txtvalue = string.Empty;
                            }
                            else if (_selectionStart == 0 && _SelectionEnd < _txtvalue.Length)
                            {
                                _txtvalue = _txtvalue.Substring(_SelectionEnd, _txtvalue.Length - _SelectionEnd);
                            }
                            else if (_selectionStart > 0 && _SelectionEnd == _txtvalue.Length)
                            {
                                _txtvalue = _txtvalue.Substring(0, _selectionStart);
                            }
                            else
                            {
                                _txtvalue = _txtvalue.Substring(0, _selectionStart) + _txtvalue.Substring(_SelectionEnd, _txtvalue.Length - _SelectionEnd);
                            }
                            //_selectionStart--;//删除一个字符后 selectStart前移一位;
                            //if (_selectionStart < 0)
                            //    _selectionStart = 0;
                            _selected = false;
                            this.Invalidate();
                        }
                        else //删除selectionStart前一位字符
                        {
                            if (_selectionStart - 1 >= 0)
                            {
                                _txtvalue = _txtvalue.Substring(0, _selectionStart - 1) + (_selectionStart >= _txtvalue.Length ? "" : _txtvalue.Substring(_selectionStart));
                                _selectionStart--;//删除一个字符后 selectStart前移一位;
                                if (_selectionStart < 0)
                                    _selectionStart = 0;
                                this.Invalidate();
                            }
                        }
                    }
                    else
                    {
                        _txtvalue = string.Empty;
                        _selectionStart = 0;
                        this.Invalidate();
                    }
                }
            }

            //小数点
            if (e.KeyChar == '.')
            {
                if (!_txtMode)//非文字模式可编辑数字 否则不接受字符
                {
                    if (_decimalplace > 0 && !_txtvalue.Contains('.'))
                    {
                        _txtvalue = _txtvalue.Substring(0, _selectionStart) + e.KeyChar + (_selectionStart >= _txtvalue.Length ? "" : _txtvalue.Substring(_selectionStart));
                        _selectionStart++;//插入字符后selectStart后移一位
                        this.Invalidate();
                    }
                }
            }
            base.OnKeyPress(e);
        }

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

        /*
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
            if (_startIdx > 0)
            {
                _startIdx--;
                this.Invalidate();
            }
        }**/

        #endregion

        #region 内部函数
        /// <summary>
        /// 设置当前光标位置
        /// </summary>
        /// <param name="start"></param>
        void InternalSetSelectionStart(int start)
        {
            if (start < 0) return;
            if (start > _txtvalue.Length) return;
            _selectionStart = start;
        }

        void OnUpArrowDown()
        {
            _txtMode = false;
            decimal dvalue=0;
            if (!decimal.TryParse(_txtvalue, out dvalue)) dvalue = 0;
            if (dvalue + _increment <= _maxdval)
            {
                dvalue += _increment;
                _txtvalue = dvalue.ToString();
                _selectionStart = _txtvalue.Length;
                this.Invalidate();
            }
        }



        void OnDnArrowDown()
        {
            _txtMode = false;
            decimal dvalue = 0;
            if (!decimal.TryParse(_txtvalue, out dvalue)) dvalue = 0;
            //默认为能小于0
            if (dvalue- _increment >= _mindval)
            {
                dvalue -= _increment;
                _txtvalue = dvalue.ToString();
                _selectionStart = _txtvalue.Length;
                this.Invalidate();
            }
        }
        #endregion


        #region IPopupControlHost

        /// <summary>
        /// Popup control.
        /// </summary>
        private PopupControl m_popupCtrl = new PopupControl();

        /// <summary>
        /// Actual drop-down control itself.
        /// </summary>
        System.Windows.Forms.Control m_dropDownCtrl;

        /// <summary>
        /// Indicates if drop-down is currently shown.
        /// </summary>
        bool m_bDroppedDown = false;
        /// <summary>
        /// Indicates current sizing mode.
        /// </summary>
        SizeMode m_sizeMode = SizeMode.UseComboSize;
        /// <summary>
        /// Time drop-down was last hidden.
        /// </summary>
        DateTime m_lastHideTime = DateTime.Now;

        /// <summary>
        /// Automatic focus timer helps make sure drop-down control is focused for user
        /// input upon drop-down.
        /// </summary>
        Timer m_timerAutoFocus;
        /// <summary>
        /// Original size of control dimensions when first assigned.
        /// </summary>
        Size m_sizeOriginal = new Size(1, 1);
        /// <summary>
        /// Original size of combo box dropdown when first assigned.
        /// </summary>
        Size m_sizeCombo;
        /// <summary>
        /// Indicates if drop-down is resizable.
        /// </summary>
        bool m_bIsResizable = true;

        private static DateTime m_sShowTime = DateTime.Now;


        /// <summary>
        /// Automatically resize drop-down from properties.
        /// </summary>
        protected void AutoSizeDropDown()
        {
            if (DropDownControl != null)
            {
                switch (DropDownSizeMode)
                {
                    case SizeMode.UseComboSize:
                        DropDownControl.Size = new Size(Width, m_sizeCombo.Height);
                        break;

                    case SizeMode.UseControlSize:
                        DropDownControl.Size = new Size(m_sizeOriginal.Width, m_sizeOriginal.Height);
                        break;

                    case SizeMode.UseDropDownSize:
                        DropDownControl.Size = m_sizeCombo;
                        break;
                }
            }
        }
        /// <summary>
        /// Assigns control to custom drop-down area of combo box.
        /// </summary>
        /// <param name="control">Control to be used as drop-down. Please note that this control must not be contained elsewhere.</param>
        protected virtual void AssignControl(System.Windows.Forms.Control control)
        {
            // If specified control is different then...
            if (control != DropDownControl)
            {
                // Preserve original container size.
                m_sizeOriginal = control.Size;

                // Reference the user-specified drop down control.
                m_dropDownCtrl = control;
            }
        }

        public SizeMode DropDownSizeMode
        {
            get { return this.m_sizeMode; }
            set
            {
                if (value != this.m_sizeMode)
                {
                    this.m_sizeMode = value;
                    AutoSizeDropDown();
                }
            }
        }

        public System.Windows.Forms.Control DropDownControl
        {
            get { return m_dropDownCtrl; }
            set { AssignControl(value); }
        }

        bool IsDroppedDown
        {
            get { return this.m_bDroppedDown /*&& m_popupCtrl.Visible*/; }
        }

        private void timerAutoFocus_Tick(object sender, EventArgs e)
        {
            if (m_popupCtrl.Visible && !DropDownControl.Focused)
            {
                DropDownControl.Focus();
                m_timerAutoFocus.Enabled = false;
            }

            //if (base.DroppedDown)
            //    base.DroppedDown = false;
        }

        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        public virtual void ShowDropDown()
        {
            if (m_popupCtrl != null && !IsDroppedDown)
            {
                // Raise drop-down event.
                //RaiseDropDownEvent();

                // Restore original control size.
                AutoSizeDropDown();

                Point location = PointToScreen(new Point(0, Height));

                // Actually show popup.
                PopupResizeMode resizeMode = PopupResizeMode.Bottom;// (this.m_bIsResizable ? PopupResizeMode.BottomRight : PopupResizeMode.None);
                m_popupCtrl.Show(this.DropDownControl, location.X, location.Y, Width, Height, resizeMode, this.ShowTop);
                m_bDroppedDown = true;

                m_popupCtrl.PopupControlHost = this;

                // Initialize automatic focus timer?
                //if (m_timerAutoFocus == null)
                //{
                //    m_timerAutoFocus = new Timer();
                //    m_timerAutoFocus.Interval = 10;
                //    m_timerAutoFocus.Tick += new EventHandler(timerAutoFocus_Tick);
                //}
                //// Enable the timer!
                //m_timerAutoFocus.Enabled = true;
                //m_sShowTime = DateTime.Now;
            }

        }



        /// <summary>
        /// Hides drop-down area of combo box, if shown.
        /// </summary>
        public virtual void HideDropDown()
        {
            if (m_popupCtrl != null && IsDroppedDown)
            {
                // Hide drop-down control.
                m_popupCtrl.Hide();
                m_bDroppedDown = false;

                // Disable automatic focus timer.
                if (m_timerAutoFocus != null && m_timerAutoFocus.Enabled)
                    m_timerAutoFocus.Enabled = false;

                // Raise drop-down closed event.
                //RaiseDropDownClosedEvent();
            }
        }

        #endregion





        Font _font = new Font("宋体", 10f,FontStyle.Bold);
        Color _itemColor = Color.FromArgb(4, 60, 109);
        SolidBrush _brush = new SolidBrush(Color.Black);
        SolidBrush _mouseOverBrush = new SolidBrush(Color.FromArgb(51, 153, 255));
        StringFormat itemFormat = new StringFormat();

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
            for (int i = 1; i <= _txtvalue.Length; i++)
            {
                string tmp = _txtvalue.Substring(0, i);
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
                location = _txtvalue.Length;
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

            SizeF size = g.MeasureString(_txtvalue, _font);
            Rectangle rectBorder = this.ClientRectangle;
            rectBorder.Height--;
            rectBorder.Width--;
            _pen.Color = Constants.BorderColor;
            //绘制外部边框
            g.DrawRectangle(_pen, rectBorder);

            //绘制右侧按钮
            g.DrawImage(GetUpArrowImg(), new Point(this.Width - 1 - 15, 1));//上箭头
            g.DrawImage(GetDnArrowImg(), new Point(this.Width - 1 - 15, 1 + 9));//下箭头
            string ss = string.Empty;

            if (!_txtMode)
            {
                //设定当前光标
                if (this.TxtMouseDown && !_charSelect)
                {

                    int locatioin = GetCurrentLocation(g, _currentX);
                    InternalSetSelectionStart(locatioin);
                    logger.Info(string.Format("_currentX:{0} cursor location:{1}", _currentX, locatioin));
                }


                ss = _txtvalue.Substring(0, _selectionStart);
                size = g.MeasureString(ss, _font);

                int selectionStartX = (int)size.Width + 1;



                if (this.Focused)
                {
                    SetCaretPos(selectionStartX, 2);

                }


                //鼠标长按 拖动鼠标选择字符串
                if (_charSelect)
                {
                    //获得当前坐标对应的字符串位置
                    _SelectionEnd = GetCurrentLocation(g, _currentX);

                    //根据位置绘制阴影
                    if (_SelectionEnd < _selectionStart)
                    {

                        Rectangle selectRect = new Rectangle(_currentX, txtOffset, selectionStartX - _currentX, _font.Height);
                        g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                    }
                    if (_SelectionEnd > _selectionStart)
                    {
                        Rectangle selectRect = new Rectangle(selectionStartX, txtOffset, _currentX - selectionStartX, _font.Height);
                        g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);

                    }

                    logger.Info("SelectEnd:" + _SelectionEnd.ToString() + " SelectStart:" + _selectionStart.ToString());
                    _selected = true;
                }

                //不在选择状态 则显示当前选中的
                if (!_charSelect && _selected)
                {
                    ss = _txtvalue.Substring(0, _SelectionEnd);
                    size = g.MeasureString(ss, _font);
                    int selectionEndX = (int)size.Width + 1;

                    if (selectionStartX > selectionEndX)
                    {
                        Rectangle selectRect = new Rectangle(selectionEndX, txtOffset, selectionStartX - selectionEndX, _font.Height);
                        g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                    }
                    if (selectionStartX < selectionEndX)
                    {
                        Rectangle selectRect = new Rectangle(selectionStartX, txtOffset, selectionEndX - selectionStartX, _font.Height);
                        g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                    }
                }
            }
            else
            {
                size = g.MeasureString(_txtvalue, _font);
                SetCaretPos((int)size.Width + 1, 2);
                //在文字模式下 鼠标点击文本框 全选所有文字 同时光标在最后一个位置闪烁
                //if (_selected)
                //{
                    
                //    Rectangle selectRect = new Rectangle(0, txtOffset, (int)size.Width+1, _font.Height);
                //    g.FillRectangle(new SolidBrush(Constants.ListMenuSelectedBGColor), selectRect);
                //}
            }

            _brush.Color = _itemColor;
            g.DrawString(_txtvalue, _font, _brush, 0, txtOffset);

        }
    }
}
