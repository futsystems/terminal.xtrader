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



            //moChannelColor = Color.FromArgb(51, 166, 3);
            //UpArrowImage =Properties.Resources.uparrow;
            //DownArrowImage = Properties.Resources.downarrow;


            //ThumbBottomImage = Properties.Resources.ThumbBottom;
            //ThumbBottomSpanImage = Properties.Resources.ThumbSpanBottom;
            //ThumbTopImage = Properties.Resources.ThumbTop;
            //ThumbTopSpanImage = Properties.Resources.ThumbSpanTop;
            //ThumbMiddleImage = Properties.Resources.ThumbMiddle;


            //scrollWidth = UpArrowImage.Width;

            itemFormat.LineAlignment = StringAlignment.Center;

            this.MouseClick += new MouseEventHandler(FListBox_MouseClick);
        }




        void FListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 0 && e.X < this.Width - 2 - scrollWidth)
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

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    this.moThumbDown = false;
        //    this.moThumbDragging = false;
        //}



        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Point ptPoint = this.PointToClient(Cursor.Position);//记录鼠标当前位置

            //int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));//游标总长度
            //float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            //int nThumbHeight = (int)fThumbHeight;

            //if (nThumbHeight > nTrackHeight)
            //{
            //    nThumbHeight = nTrackHeight;
            //    fThumbHeight = nTrackHeight;
            //}
            //if (nThumbHeight < 56)
            //{
            //    nThumbHeight = 56;
            //    fThumbHeight = 56;
            //}

            //int nTop = moThumbTop;
            //nTop += UpArrowImage.Height;

            //int scrollX = this.Width - scrollWidth;


            //Rectangle thumbrect = new Rectangle(new Point(scrollX + 1, nTop), new Size(ThumbMiddleImage.Width, nThumbHeight));
            //if (thumbrect.Contains(ptPoint))
            //{

            //    //hit the thumb
            //    nClickPoint = (ptPoint.Y - nTop);
            //    //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
            //    this.moThumbDown = true;
            //}

            ////判断鼠标按下 是否在某个区域内
            //Rectangle uparrowrect = new Rectangle(new Point(scrollX + 1, 0), new Size(UpArrowImage.Width, UpArrowImage.Height));
            //if (uparrowrect.Contains(ptPoint))
            //{

            //    int nRealRange = (Maximum - Minimum) - LargeChange;
            //    int nPixelRange = (nTrackHeight - nThumbHeight);
            //    if (nRealRange > 0)
            //    {
            //        if (nPixelRange > 0)
            //        {
            //            if ((moThumbTop - SmallChange) < 0)
            //                moThumbTop = 0;
            //            else
            //                moThumbTop -= SmallChange;

            //            //figure out value
            //            float fPerc = (float)moThumbTop / (float)nPixelRange;
            //            float fValue = fPerc * (Maximum - LargeChange);

            //            moValue = (int)fValue;
            //            Debug.WriteLine(moValue.ToString());

            //            if (ValueChanged != null)
            //                ValueChanged(this, new EventArgs());

            //            if (Scroll != null)
            //                Scroll(this, new EventArgs());

            //            Invalidate();
            //        }
            //    }
            //}

            //Rectangle downarrowrect = new Rectangle(new Point(scrollX + 1, UpArrowImage.Height + nTrackHeight), new Size(UpArrowImage.Width, UpArrowImage.Height));
            //if (downarrowrect.Contains(ptPoint))
            //{
            //    int nRealRange = (Maximum - Minimum) - LargeChange;
            //    int nPixelRange = (nTrackHeight - nThumbHeight);
            //    if (nRealRange > 0)
            //    {
            //        if (nPixelRange > 0)
            //        {
            //            if ((moThumbTop + SmallChange) > nPixelRange)
            //                moThumbTop = nPixelRange;
            //            else
            //                moThumbTop += SmallChange;

            //            //figure out value
            //            float fPerc = (float)moThumbTop / (float)nPixelRange;
            //            float fValue = fPerc * (Maximum - LargeChange);

            //            moValue = (int)fValue;
            //            Debug.WriteLine(moValue.ToString());

            //            if (ValueChanged != null)
            //                ValueChanged(this, new EventArgs());

            //            if (Scroll != null)
            //                Scroll(this, new EventArgs());

            //            Invalidate();
            //        }
            //    }
            //}
        }

        public int ItemHeight 
        { get { return lineHeight; } 
            set
            {
                lineHeight = value;
                this.Invalidate();
            } 
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


        //#region scroll
        //protected Color moChannelColor = Color.Empty;
        //protected Image moUpArrowImage = null;
        ////protected Image moUpArrowImage_Over = null;
        ////protected Image moUpArrowImage_Down = null;
        //protected Image moDownArrowImage = null;
        ////protected Image moDownArrowImage_Over = null;
        ////protected Image moDownArrowImage_Down = null;
        //protected Image moThumbArrowImage = null;

        //protected Image moThumbTopImage = null;
        //protected Image moThumbTopSpanImage = null;
        //protected Image moThumbBottomImage = null;
        //protected Image moThumbBottomSpanImage = null;
        //protected Image moThumbMiddleImage = null;

        //protected int moLargeChange = 10;
        //protected int moSmallChange = 1;
        //protected int moMinimum = 0;
        //protected int moMaximum = 100;
        //protected int moValue = 0;
        //private int nClickPoint;

        //protected int moThumbTop = 0;

        //protected bool moAutoSize = false;

        //private bool moThumbDown = false;
        //private bool moThumbDragging = false;

        //public new event EventHandler Scroll = null;
        //public event EventHandler ValueChanged = null;


        








        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        //public Color ChannelColor
        //{
        //    get { return moChannelColor; }
        //    set { moChannelColor = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image UpArrowImage
        //{
        //    get { return moUpArrowImage; }
        //    set { moUpArrowImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image DownArrowImage
        //{
        //    get { return moDownArrowImage; }
        //    set { moDownArrowImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image ThumbTopImage
        //{
        //    get { return moThumbTopImage; }
        //    set { moThumbTopImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image ThumbTopSpanImage
        //{
        //    get { return moThumbTopSpanImage; }
        //    set { moThumbTopSpanImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image ThumbBottomImage
        //{
        //    get { return moThumbBottomImage; }
        //    set { moThumbBottomImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image ThumbBottomSpanImage
        //{
        //    get { return moThumbBottomSpanImage; }
        //    set { moThumbBottomSpanImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        //public Image ThumbMiddleImage
        //{
        //    get { return moThumbMiddleImage; }
        //    set { moThumbMiddleImage = value; }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        //public int LargeChange
        //{
        //    get { return moLargeChange; }
        //    set
        //    {
        //        moLargeChange = value;
        //        Invalidate();
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        //public int SmallChange
        //{
        //    get { return moSmallChange; }
        //    set
        //    {
        //        moSmallChange = value;
        //        Invalidate();
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        //public int Minimum
        //{
        //    get { return moMinimum; }
        //    set
        //    {
        //        moMinimum = value;
        //        Invalidate();
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        //public int Maximum
        //{
        //    get { return moMaximum; }
        //    set
        //    {
        //        moMaximum = value;
        //        Invalidate();
        //    }
        //}

        //[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        //public int Value
        //{
        //    get { return moValue; }
        //    set
        //    {
        //        moValue = value;

        //        int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
        //        float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
        //        int nThumbHeight = (int)fThumbHeight;

        //        if (nThumbHeight > nTrackHeight)
        //        {
        //            nThumbHeight = nTrackHeight;
        //            fThumbHeight = nTrackHeight;
        //        }
        //        if (nThumbHeight < 56)
        //        {
        //            nThumbHeight = 56;
        //            fThumbHeight = 56;
        //        }

        //        //figure out value
        //        int nPixelRange = nTrackHeight - nThumbHeight;
        //        int nRealRange = (Maximum - Minimum) - LargeChange;
        //        float fPerc = 0.0f;
        //        if (nRealRange != 0)
        //        {
        //            fPerc = (float)moValue / (float)nRealRange;

        //        }

        //        float fTop = fPerc * nPixelRange;
        //        moThumbTop = (int)fTop;


        //        Invalidate();
        //    }
        //}


        //#endregion


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
        int scrollWidth = 10;
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
