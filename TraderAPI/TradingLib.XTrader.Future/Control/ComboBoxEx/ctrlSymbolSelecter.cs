﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TradingLib.XTrader.Future
{

    public enum SizeMode
    {
        UseComboSize,
        UseControlSize,
        UseDropDownSize,
    }

    /// <summary>
    /// <c>CustomComboBox</c> is an extension of <c>ComboBox</c> which provides drop-down customization.
    /// 自定义下拉合约选择器
    /// </summary>
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [Designer(typeof(ctrlSymbolSelecterDesigner))]
    public class ctrlSymbolSelecter : ComboBox, IPopupControlHost
    {
        #region 控件自绘
        #region Fields

        private IntPtr _editHandle;
        private ControlState _buttonState;
        private Color _baseColor = Color.FromArgb(51, 161, 224);
        private Color _borderColor = Color.FromArgb(127, 157, 185);
        private Color _arrowColor = Color.FromArgb(19, 88, 128);
        private bool _bPainting;

        #endregion

        #region Properties

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                if (_baseColor != value)
                {
                    _baseColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "127, 157, 185")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "19, 88, 128")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set
            {
                if (_arrowColor != value)
                {
                    _arrowColor = value;
                    base.Invalidate();
                }
            }
        }

        internal ControlState ButtonState
        {
            get { return _buttonState; }
            set
            {
                if (_buttonState != value)
                {
                    _buttonState = value;
                    Invalidate(ButtonRect);
                }
            }
        }

        internal Rectangle ButtonRect
        {
            get
            {
                return GetDropDownButtonRect();
            }
        }

        internal bool ButtonPressed
        {
            get
            {
                if (IsHandleCreated)
                {
                    return GetComboBoxButtonPressed();
                }
                return false;
            }
        }

        internal IntPtr EditHandle
        {
            get { return _editHandle; }
        }

        internal Rectangle EditRect
        {
            get
            {
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Rectangle rect = new Rectangle(
                        3, 3, Width - ButtonRect.Width - 6, Height - 6);
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        rect.X += ButtonRect.Right;
                    }
                    return rect;
                }
                if (IsHandleCreated && EditHandle != IntPtr.Zero)
                {
                    NativeMethods.RECT rcClient = new NativeMethods.RECT();
                    NativeMethods.GetWindowRect(EditHandle, ref rcClient);
                    return RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }

        #endregion

        #region Render Methods

        private void RenderComboBox(ref Message m)
        {
            Rectangle rect = new Rectangle(Point.Empty, Size);
            Rectangle buttonRect = ButtonRect;
            ControlState state = ButtonPressed ?
                ControlState.Pressed : ButtonState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd))
            {
                g.Clear(Color.White);
                RenderComboBoxBackground(g, rect, buttonRect);
                RenderConboBoxDropDownButton(g, ButtonRect, state);
                RenderConboBoxBorder(g, rect);
            }
        }

        private void RenderConboBoxBorder(
            Graphics g, Rectangle rect)
        {
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            using (Pen pen = new Pen(borderColor))
            {
                rect.Width--;
                rect.Height--;
                g.DrawRectangle(pen, rect);
            }
        }

        private void RenderComboBoxBackground(
            Graphics g,
            Rectangle rect,
            Rectangle buttonRect)
        {
            
            Color backColor = base.Enabled ?
                base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                buttonRect.Inflate(-1, -1);
                rect.Inflate(-1, -1);
                using (Region region = new Region(rect))
                {
                    region.Exclude(buttonRect);
                    region.Exclude(EditRect);
                    g.FillRegion(brush, region);
                }
            }
        }

        Bitmap GetBtnImg(ControlState state)
        {
            if (state == ControlState.Pressed) return Properties.Resources.combox_btn_mouse_down;
            if (state == ControlState.Hover) return Properties.Resources.combox_btn_mouse_over;
            
            return Properties.Resources.combox_btn_normal;
        }
        private void RenderConboBoxDropDownButton(
            Graphics g,
            Rectangle buttonRect,
            ControlState state)
        {

            g.DrawImage(GetBtnImg(state), new Point(buttonRect.X+1, buttonRect.Y+1));
        }
        #endregion

        #region Help Methods

        private NativeMethods.ComboBoxInfo GetComboBoxInfo()
        {
            NativeMethods.ComboBoxInfo cbi = new NativeMethods.ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            NativeMethods.GetComboBoxInfo(base.Handle, ref cbi);
            return cbi;
        }

        private bool GetComboBoxButtonPressed()
        {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.stateButton ==
                NativeMethods.ComboBoxButtonState.STATE_SYSTEM_PRESSED;
        }

        private Rectangle GetDropDownButtonRect()
        {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();

            return cbi.rcButton.Rect;
        }

        #endregion

        #region Windows Message Methods

        private void WmPaint(ref Message m)
        {
            if (base.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.WndProc(ref m);
                return;
            }

            if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (!_bPainting)
                {
                    NativeMethods.PAINTSTRUCT ps =
                        new NativeMethods.PAINTSTRUCT();

                    _bPainting = true;
                    NativeMethods.BeginPaint(m.HWnd, ref ps);

                    RenderComboBox(ref m);

                    NativeMethods.EndPaint(m.HWnd, ref ps);
                    _bPainting = false;
                    m.Result = NativeMethods.TRUE;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
                RenderComboBox(ref m);
            }
        }

        #endregion


        protected override void OnDropDown(EventArgs e)
        {
            //base.OnDropDown(e);
            
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            _editHandle = cbi.hwndEdit;
        }

        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
            else
            {
                ButtonState = ControlState.Normal;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Point point = PointToClient(Cursor.Position);
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            ButtonState = ControlState.Normal;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ButtonState = ControlState.Normal;
        }

        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case NativeMethods.WM_PAINT:
        //            WmPaint(ref m);
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}
        #endregion

        #region Construction and destruction

        public ctrlSymbolSelecter()
            : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

            m_sizeCombo = new Size(base.DropDownWidth, base.DropDownHeight);
            this.ForeColor = Color.FromArgb(4, 60, 109);
            this.Font = new Font("宋体", 9f, FontStyle.Bold);
            m_popupCtrl.Closing += new ToolStripDropDownClosingEventHandler(m_dropDown_Closing);
            
            m_titleList.Height = 200;
            m_titleList.Width = 1;
            m_symbolList.Height = 80;
            m_symbolList.Width = 1;

            
            //m_titleList.BorderStyle = BorderStyle.None;
            //m_symbolList.BorderStyle = BorderStyle.None;

            m_titleList.Items.Add("IF  股指1");
            m_titleList.Items.Add("HSI 恒指2");
            m_titleList.Items.Add("CL  原油3");
            m_titleList.Items.Add("IF  股指4");
            m_titleList.Items.Add("HSI 股指5");
            m_titleList.Items.Add("CL  股指6");
            m_titleList.Items.Add("IF  股指7");
            m_titleList.Items.Add("HSI 股指8");
            m_titleList.Items.Add("IF  股指9");
            m_titleList.Items.Add("HSI 股指10");
            m_titleList.Items.Add("CL11");
            m_titleList.Items.Add("IF12");
            m_titleList.Items.Add("HSI13");
            m_titleList.Items.Add("CL14");
            m_titleList.Items.Add("IF15");
            m_titleList.Items.Add("HSI16");
            m_titleList.Items.Add("CL17");
            m_titleList.Items.Add("IF18");
            m_titleList.Items.Add("HSI19");
            m_titleList.Items.Add("CL20");
            m_titleList.Items.Add("CL21");

            m_symbolList.Items.Add("CLX622");
            m_symbolList.Items.Add("CLZ623");

            //m_symbolList.SelectedValueChanged += new EventHandler(m_symbolList_SelectedValueChanged);

            //m_titleList.SelectedValueChanged += new EventHandler(m_titleList_SelectedValueChanged);
            m_titleList.ItemSelected += new Action<string>(m_titleList_ItemSelected);
            m_symbolList.ItemSelected += new Action<string>(m_symbolList_ItemSelected);
        }

        void m_symbolList_ItemSelected(string obj)
        {
            if (m_popupCtrl != null && this.IsDroppedDown)
            {
                //关闭合约列表
                m_popupCtrl.Hide();
            }
            this.Text = obj;
        }

        void m_titleList_ItemSelected(string obj)
        {
            ShowSymbolList(obj);


        }

        void m_titleList_SelectedValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("sec:" + m_titleList.SelectedItem.ToString());
            //根据选择的品种显示合约列表
            string sectitle = m_titleList.SelectedItem.ToString();
            ShowSymbolList(sectitle);
        }

        void m_symbolList_SelectedValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("symbol:" + m_symbolList.SelectedItem.ToString());

            string symbol = m_symbolList.SelectedItem.ToString();
            if (m_popupCtrl != null && this.IsDroppedDown)
            {
                //关闭合约列表
                m_popupCtrl.Hide();
            }
            this.Text = symbol;
        }

        void m_dropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            m_lastHideTime = DateTime.Now;
        }

        public ctrlSymbolSelecter(System.Windows.Forms.Control dropControl)
            : this()
        {
            DropDownControl = dropControl;
        }

        #endregion

        #region ComboBox overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(m_timerAutoFocus != null)
                {
                    m_timerAutoFocus.Dispose();
                    m_timerAutoFocus = null;
                }
            }
            base.Dispose(disposing);
        }




        #endregion

        #region Event handlers

        private void timerAutoFocus_Tick(object sender, EventArgs e)
        {
            //if (m_popupCtrl.Visible && !DropDownControl.Focused)
            //{
            //    DropDownControl.Focus();
            //    m_timerAutoFocus.Enabled = false;
            //}

            if (base.DroppedDown)
                base.DroppedDown = false;
        }

        private void m_dropDown_LostFocus(object sender, EventArgs e)
        {
            m_lastHideTime = DateTime.Now;
        }

        #endregion

        #region Events

        public new event EventHandler DropDown;
        public new event EventHandler DropDownClosed;

        public new event OldNewEventHandler<object> SelectedValueChanged;

        public void RaiseDropDownEvent()
        {
            EventHandler eventHandler = this.DropDown;
            if (eventHandler != null)
                this.DropDown(this, EventArgs.Empty);
        }

        public void RaiseDropDownClosedEvent()
        {
            EventHandler eventHandler = this.DropDownClosed;
            if (eventHandler != null)
                this.DropDownClosed(this, EventArgs.Empty);
        }

        public void RaiseSelectedValueChangedEvent(object oldValue, object newValue)
        {
            OldNewEventHandler<object> eventHandler = this.SelectedValueChanged;
            if (eventHandler != null)
                this.SelectedValueChanged(this, new OldNewEventArgs<object>(oldValue, newValue));
        }

        #endregion

        #region 下拉List控件

        enum EnumSymbolSelectStatus
        { 
            /// <summary>
            /// 下拉品种
            /// </summary>
            DropDownSecurity,
            /// <summary>
            /// 下拉合约
            /// </summary>
            DropDownSymbol,
        }
        ctrlListBox m_titleList = new ctrlListBox();

        ctrlListBox m_symbolList = new ctrlListBox();

        #endregion

        #region IPopupControlHost Members


        void ShowSymbolList(string title)
        {
            if (m_popupCtrl != null && this.IsDroppedDown)
            {
                Point location = PointToScreen(new Point(0, Height));

                // Actually show popup.
                PopupResizeMode resizeMode = (this.m_bIsResizable ? PopupResizeMode.BottomRight : PopupResizeMode.None);
                m_popupCtrl.Show(this.m_symbolList, location.X, location.Y, Width, Height,PopupResizeMode.Top);
                m_popupCtrl.PopupControlHost = this;

            }
        }
        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        public virtual void ShowDropDown()
        {
            if (m_popupCtrl != null && !IsDroppedDown)
            {
                // Raise drop-down event.
                RaiseDropDownEvent();

                // Restore original control size.
                AutoSizeDropDown();

                Point location = PointToScreen(new Point(0, Height));

                // Actually show popup.
                PopupResizeMode resizeMode = PopupResizeMode.Bottom;// (this.m_bIsResizable ? PopupResizeMode.BottomRight : PopupResizeMode.None);
                m_popupCtrl.Show(this.m_titleList, location.X, location.Y, Width, Height, resizeMode,true);
                m_bDroppedDown = true;

                m_popupCtrl.PopupControlHost = this;

                // Initialize automatic focus timer?
                if (m_timerAutoFocus == null)
                {
                    m_timerAutoFocus = new Timer();
                    m_timerAutoFocus.Interval = 10;
                    m_timerAutoFocus.Tick += new EventHandler(timerAutoFocus_Tick);
                }
                // Enable the timer!
                m_timerAutoFocus.Enabled = true;
                m_sShowTime = DateTime.Now;
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
                RaiseDropDownClosedEvent();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获得某个分组下的合约集
        /// </summary>
        /// <param name="setTitle"></param>
        /// <returns></returns>
        SymbolSet GetSymbolSet(string setTitle)
        {
            SymbolSet target = null;
            if (m_SymbolSetMap.TryGetValue(setTitle, out target))
            {
                return target;
            }
            return null;
        }

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

        #endregion

        #region Win32 message handlers 通过截获Win32消息来屏蔽 下拉按钮点击时 显示自定义下拉框

        public const uint WM_COMMAND = 0x0111;
        public const uint WM_USER = 0x0400;
        public const uint WM_REFLECT = WM_USER + 0x1C00;
        public const uint WM_LBUTTONDOWN = 0x0201;

        public const uint CBN_DROPDOWN = 7;
        public const uint CBN_CLOSEUP = 8;
        
        public static uint HIWORD(int n)
        {
            return (uint)(n >> 16) & 0xffff;
        }

        public override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg == (WM_REFLECT + WM_COMMAND))
            {
                if (HIWORD((int)m.WParam) == CBN_DROPDOWN)
                    return false;
            }
            return base.PreProcessMessage(ref m);
        }

        private static DateTime m_sShowTime = DateTime.Now;

        private void AutoDropDown()
        {
            if (m_popupCtrl != null && m_popupCtrl.Visible)
                HideDropDown();
            else if ((DateTime.Now - m_lastHideTime).Milliseconds > 50)
                ShowDropDown();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                AutoDropDown();
                return;
            }

            if (m.Msg == (WM_REFLECT + WM_COMMAND))
            {
                switch (HIWORD((int)m.WParam))
                {
                    case CBN_DROPDOWN:
                        AutoDropDown();
                        return;

                    case CBN_CLOSEUP:
                        if ((DateTime.Now - m_sShowTime).Seconds > 1)
                            HideDropDown();
                        return;
                }
            }
            if (m.Msg == NativeMethods.WM_PAINT)
            {
                WmPaint(ref m);
                return;
            }

            base.WndProc(ref m);
        }

        #endregion

        #region Enumerations



        #endregion

        #region Properties

        /// <summary>
        /// Actual drop-down control itself.
        /// </summary>
        [Browsable(false)]
        public System.Windows.Forms.Control DropDownControl
        {
            get { return m_dropDownCtrl; }
            set { AssignControl(value); }
        }

        /// <summary>
        /// Indicates if drop-down is currently shown.
        /// </summary>
        [Browsable(false)]
        public bool IsDroppedDown
        {
            get { return this.m_bDroppedDown /*&& m_popupCtrl.Visible*/; }
        }

        /// <summary>
        /// Indicates if drop-down is resizable.
        /// </summary>
        [Category("Custom Drop-Down"), Description("Indicates if drop-down is resizable.")]
        public bool AllowResizeDropDown
        {
            get { return this.m_bIsResizable; }
            set { this.m_bIsResizable = value; }
        }

        /// <summary>
        /// Indicates current sizing mode.
        /// </summary>
        [Category("Custom Drop-Down"), Description("Indicates current sizing mode."), DefaultValue(SizeMode.UseComboSize)]
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

        [Category("Custom Drop-Down")]
        public Size DropSize
        {
            get { return m_sizeCombo; }
            set
            {
                m_sizeCombo = value;
                if (DropDownSizeMode == SizeMode.UseDropDownSize)
                    AutoSizeDropDown();
            }
        }

        [Category("Custom Drop-Down"), Browsable(false)]
        public Size ControlSize
        {
            get { return m_sizeOriginal; }
            set
            {
                m_sizeOriginal = value;
                if (DropDownSizeMode == SizeMode.UseControlSize)
                    AutoSizeDropDown();
            }
        }

        #endregion

        #region Hide some unwanted properties 隐藏不需要的属性

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int MaxDropDownItems
        {
            get { return base.MaxDropDownItems; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new string DisplayMember
        {
            get { return base.DisplayMember; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int DropDownWidth
        {
            get { return base.DropDownWidth; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), ReadOnly(true)]
        public new bool Sorted
        {
            get { return base.Sorted; }
            set { }
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Popup control.
        /// </summary>
        private PopupControl m_popupCtrl = new PopupControl(true);//dropdown自动关闭

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

        /// <summary>
        /// 合约集列表
        /// </summary>
        SortedDictionary<string, SymbolSet> m_SymbolSetMap = new SortedDictionary<string, SymbolSet>();
        #endregion
    }

    internal class ctrlSymbolSelecterDesigner : ParentControlDesigner
    {
        #region ParentControlDesigner Overrides

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("DropDownStyle");
            properties.Remove("Items");
            properties.Remove("ItemHeight");
            properties.Remove("MaxDropDownItems");
            properties.Remove("DisplayMember");
            properties.Remove("ValueMember");
            properties.Remove("DropDownWidth");
            properties.Remove("DropDownHeight");
            properties.Remove("IntegralHeight");
            properties.Remove("Sorted");
        }

        #endregion
    }
}
