using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TradingLib.XTrader
{
    public class FButton:System.Windows.Forms.Button
    {
        private bool mouseover = false;//鼠标经过
        private bool mousedown = false;
        StringFormat _orderStringFormat = null;
        StringFormat _normalStrFormat = null;
        public FButton()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.BackColor = Color.Transparent;
            this.BackColor = Color.White;
            //this.Cursor = System.Windows.Forms.Cursors.Hand;
            _orderStringFormat = new StringFormat();
            _orderStringFormat.Alignment = StringAlignment.Center;
            _orderStringFormat.LineAlignment = StringAlignment.Far;
            _normalStrFormat = new StringFormat();
            _normalStrFormat.Alignment = StringAlignment.Center;
            _normalStrFormat.LineAlignment = StringAlignment.Center;


        }

        Color _background = Color.White;
        protected override void OnPaint(PaintEventArgs e)
        {

            //在这里用自己的方法来绘制Button的外观(其实也就是几个框框)
            Graphics g = e.Graphics;
            g.Clear(_background);

            Rectangle rect = this.ClientRectangle;
            if (mouseover)
            {
                if (Focused && !mousedown)
                {
                    DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonFocuseAndMouseOver);
                    return;
                }
                if (mousedown)
                {
                    DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonDown);
                    return;
                }
                DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonMouseOver);
                return;
            }
            if (Focused)
            {
                DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonFocuse);
                return;
            }
            DrawRoundButton(this.Text, g, rect, buttonStyle.ButtonNormal);
        }


        static Pen _borderPen = new Pen(Color.FromArgb(0, 60, 116));

        static Pen _dashPen = new Pen(Color.FromArgb(35, 39, 48));
        static Pen _splitPen = new Pen(Color.Gray);

        static SolidBrush _clickBrush = new SolidBrush(Color.FromArgb(227, 226, 218));
        static SolidBrush _txtBrush = new SolidBrush(Color.FromArgb(189, 9, 9));

        static Color _shadowEnd = Color.FromArgb(249, 181, 53);
        static Color _shadownStart = Color.FromArgb(253, 232, 187);
        static Color _bgStart = Color.FromArgb(252, 252, 252);
        static Color _bgEnd = Color.FromArgb(241, 241, 241);


        static Pen _shadownPen = new Pen(_shadownStart, 2);
        static Font _priceFont = new Font("宋体", 12, FontStyle.Bold);
        static Font _sideFont = new Font("宋体", 18, FontStyle.Bold);

        //decimal _price = 1224.00M;

        //string _priceFormat = "{0:F2}";



        bool _orderEntryButton = false;
        /// <summary>
        /// 是否是买入 卖出下单按钮
        /// 下单按钮在按钮上输出价格信息
        /// </summary>
        public bool OrderEntryButton
        {
            get { return _orderEntryButton; }
            set { _orderEntryButton = value; }
        }

        bool _checkButton = false;
        public bool CheckButton
        {
            get { return _checkButton; }
            set { _checkButton = value; }
        }

        bool _checked = false;
        public bool Checked
        {
            get { return _checked; }
            set 
            { 
                _checked = value;
                Invalidate();
            }
        }
        //public decimal Price
        //{
        //    get { return _price; }
        //    set
        //    {
        //        _price = value;
        //        this.Invalidate();
        //    }
        //}
        /// <summary> 
        /// 绘制圆角按钮
        /// </summary> 
        /// <param name="Text">要绘制的文字</param>
        /// <param name="g">Graphics 对象</param> 
        /// <param name="rect">要填充的矩形</param> 
        /// <param name="btnStyle"></param>
        public void DrawRoundButton(string Text, Graphics g, Rectangle rect, buttonStyle btnStyle)
        {
            Rectangle rectBorder = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

            

            if (this.Checked)
            {
                g.FillRectangle(_clickBrush, rect);
            }
            else
            {
                //渐变填充背景
                LinearGradientBrush bgbrush = new LinearGradientBrush(rect, _bgStart, _bgEnd, 90);
                g.FillRectangle(bgbrush, rect);
            }

            //绘制按钮边框
            g.DrawPath(_borderPen, GetRoundRectangle(rectBorder, 3));
            Rectangle rectShadow = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4 - 1, rect.Height - 4 - 1);
            Rectangle rectDash = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4 - 1, rect.Height - 4 - 1);

            if (this.CheckButton)
            {
                if (!this.Checked)
                {
                    if (btnStyle == buttonStyle.ButtonFocuseAndMouseOver || btnStyle == buttonStyle.ButtonMouseOver)
                    {
                        //绘制渐变阴影
                        _shadownPen.Color = _shadownStart;
                        g.DrawLine(_shadownPen, rectShadow.X, rectShadow.Y, rectShadow.X + rectShadow.Width, rectShadow.Y);

                        using (LinearGradientBrush brush = new LinearGradientBrush(new Point(rectShadow.X, rectShadow.Y), new Point(rectShadow.X, rectShadow.Y + rectShadow.Height), _shadownStart, _shadowEnd))
                        {
                            Pen pen = new Pen(brush);
                            pen.Width = 2;
                            //绘制左右渐变阴影
                            g.DrawLine(pen, rectShadow.X, rectShadow.Y, rectShadow.X, rectShadow.Y + rectShadow.Height);
                            g.DrawLine(pen, rectShadow.X + rectShadow.Width, rectShadow.Y, rectShadow.X + rectShadow.Width, rectShadow.Y + rectShadow.Height);
                        }
                        _shadownPen.Color = _shadowEnd;
                        g.DrawLine(_shadownPen, rectShadow.X, rectShadow.Y + rectShadow.Height, rectShadow.X + rectShadow.Width, rectShadow.Y + rectShadow.Height);
                    }
                }

                //居中输出文字
                if (this.Checked)
                {
                    _txtBrush.Color = Color.FromArgb(64, 64, 64);
                }
                else
                {
                    _txtBrush.Color = Color.FromArgb(160,160,160);
                }
                g.DrawString(this.Text, this.Font, _txtBrush, rect, _normalStrFormat);
            }
            else
            {
               

                if (btnStyle == buttonStyle.ButtonFocuseAndMouseOver || btnStyle == buttonStyle.ButtonMouseOver)
                {
                    //绘制渐变阴影
                    _shadownPen.Color = _shadownStart;
                    g.DrawLine(_shadownPen, rectShadow.X, rectShadow.Y, rectShadow.X + rectShadow.Width, rectShadow.Y);

                    using (LinearGradientBrush brush = new LinearGradientBrush(new Point(rectShadow.X, rectShadow.Y), new Point(rectShadow.X, rectShadow.Y + rectShadow.Height), _shadownStart, _shadowEnd))
                    {
                        Pen pen = new Pen(brush);
                        pen.Width = 2;
                        //绘制左右渐变阴影
                        g.DrawLine(pen, rectShadow.X, rectShadow.Y, rectShadow.X, rectShadow.Y + rectShadow.Height);
                        g.DrawLine(pen, rectShadow.X + rectShadow.Width, rectShadow.Y, rectShadow.X + rectShadow.Width, rectShadow.Y + rectShadow.Height);
                    }
                    _shadownPen.Color = _shadowEnd;
                    g.DrawLine(_shadownPen, rectShadow.X, rectShadow.Y + rectShadow.Height, rectShadow.X + rectShadow.Width, rectShadow.Y + rectShadow.Height);
                }

                //按钮焦点 按钮焦点鼠标悬停 绘制虚线
                if (btnStyle == buttonStyle.ButtonFocuse || btnStyle == buttonStyle.ButtonFocuseAndMouseOver)
                {
                    _dashPen.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(_dashPen, rectDash);
                }

                //按下按钮 绘制虚线 凸显背景
                if (btnStyle == buttonStyle.ButtonDown)
                {
                    g.FillRectangle(_clickBrush, rectShadow);

                    _dashPen.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(_dashPen, rectDash);//虚线框
                }

                if (this.OrderEntryButton)
                {
                    //输出价格与文字
                    int ly = (int)(this.Height * 0.4);
                    g.DrawLine(_splitPen, 4, ly, this.Width - 4, ly);
                    Rectangle txtRect = new Rectangle(0, ly, rect.Width, rect.Height - ly);
                    Rectangle priceRect = new Rectangle(0, 0, rect.Width, ly);

                    //string pricestr = string.Format(_priceFormat, this.Price);
                    _txtBrush.Color = this.ForeColor;
                    if (this.IsPriceOn && !string.IsNullOrEmpty(this.PriceStr))
                    {
                        g.DrawString(this.PriceStr, _priceFont, _txtBrush, priceRect, _orderStringFormat);
                    }
                    g.DrawString(Text, _sideFont, _txtBrush, txtRect, _orderStringFormat);
                }
                else
                {
                    //居中输出文字
                    _txtBrush.Color = this.ForeColor;
                    g.DrawString(this.Text, this.Font, _txtBrush, rect, _normalStrFormat);
                }
            }
        }

        string _pricestr = string.Empty;
        public string PriceStr { get { return _pricestr; } set { _pricestr = value; this.Invalidate(); } }
        /// <summary>
        /// 是否输出价格信息
        /// </summary>
        public bool IsPriceOn { get; set; }

        



        static GraphicsPath GetRoundRectangle(Rectangle rectangle, int r)
        {
            int l = 2 * r;
            // 把圆角矩形分成八段直线、弧的组合，依次加到路径中  
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(rectangle.X + r, rectangle.Y), new Point(rectangle.Right - r, rectangle.Y));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Y, l, l), 270F, 90F);

            gp.AddLine(new Point(rectangle.Right, rectangle.Y + r), new Point(rectangle.Right, rectangle.Bottom - r));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Bottom - l, l, l), 0F, 90F);

            gp.AddLine(new Point(rectangle.Right - r, rectangle.Bottom), new Point(rectangle.X + r, rectangle.Bottom));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Bottom - l, l, l), 90F, 90F);

            gp.AddLine(new Point(rectangle.X, rectangle.Bottom - r), new Point(rectangle.X, rectangle.Y + r));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Y, l, l), 180F, 90F);
            return gp;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            mousedown = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            mousedown = false;
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseover = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            mouseover = false;
            base.OnMouseLeave(e);   
        }
    
    }
}
