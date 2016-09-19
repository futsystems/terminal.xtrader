using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;

namespace TradingLib.KryptonControl
{
    public class BlockTabClickEvent : EventArgs
    {
        public BlockButton TargtButton { get; set; }

        public BlockTabClickEvent(BlockButton btn)
        {
            this.TargtButton = btn;
        }
    }


    public partial class BlockTab : Control
    {

        public event EventHandler<BlockTabClickEvent> BlockTabClick;
        List<BlockButton> _btnList = new List<BlockButton>();
        public BlockTab()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            this.Paint += new PaintEventHandler(BlockButton_Paint);
            this.MouseMove += new MouseEventHandler(BlockButton_MouseMove);
            this.MouseLeave += new EventHandler(BlockButton_MouseLeave);
            this.MouseClick += new MouseEventHandler(BlockButton_MouseClick);

            AddBlock("所有", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    return true;
                })
                
                ,EnumQuoteListType.FUTURE_CN);
           
        }

        BlockButton this[int index]
        {
            get
            {
                if (index < 0 || index > _btnList.Count - 1) return null;
                return _btnList[index];
            }
        }

        public void SelectTab(int index)
        {
            BlockButton btn = this[index];
            if (btn != null)
            {
                foreach(var b in _btnList)
                {
                    b.Selected = false;
                }
                btn.Selected = true;



                this.Invalidate();
                if (BlockTabClick != null)
                {
                    BlockTabClick(this, new BlockTabClickEvent(btn));
                }

            }
        }

        void BlockButton_MouseClick(object sender, MouseEventArgs e)
        {
            bool change = false;
            BlockButton target = null;
            foreach (var btn in _btnList)
            {
                bool oldStatus = btn.Selected;
                if (e.X > btn.StartX && e.X < btn.EndX)
                {
                    btn.Selected = true;
                    target = btn;
                }
                else
                {
                    btn.Selected = false;
                }
                if (btn.Selected != oldStatus)
                {
                    change = true;
                }
            }
            if (change)
            {
                this.Invalidate();
            }
            if (change && target != null)
            {
                if (BlockTabClick != null)
                {
                    BlockTabClick(this, new BlockTabClickEvent(target));
                }
            }
        }

        void BlockButton_MouseLeave(object sender, EventArgs e)
        {
            foreach (var btn in _btnList)
            {
                if (btn.MouseOver)
                {
                    btn.MouseOver = false;
                    this.Invalidate();
                    return;
                }
            }
        }

        void BlockButton_MouseMove(object sender, MouseEventArgs e)
        {
            bool change = false;
            foreach (var btn in _btnList)
            {
                bool oldStatus = btn.MouseOver;
                if (e.X > btn.StartX && e.X < btn.EndX)
                {
                    btn.MouseOver = true;
                }
                else
                {
                    btn.MouseOver = false;
                }
                if (btn.MouseOver != oldStatus)
                {
                    change = true;
                }
            }
            if (change)
            {
                this.Invalidate();
            }
        }


        /// <summary>
        /// 测量文字宽度
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="font"></param>
        /// <param name="s1"></param>
        /// <returns></returns>
        static int TextWidth(Graphics canvas, System.Drawing.Font font, string s1)
        {
            return (int)canvas.MeasureString(s1, font).Width;
        }


        SolidBrush _brush = new SolidBrush(Color.Red);
        Pen _pen = new Pen(Color.Red, 1);
        Font _font = new Font("宋体", 9);

        int _toph = 4;
        int _space = 4;
        
        void BlockButton_Paint(object sender, PaintEventArgs e)
        {

            //e.Graphics.FillRectangle(_brush,e.ClipRectangle);
            Graphics canvas = e.Graphics;

            
            int w = 0;
            int totalWidth=0;
            foreach (var btn in _btnList)
            {
                btn.StartX = totalWidth;
                totalWidth = totalWidth + _space;
                w = TextWidth(canvas, _font, btn.Title);

                //首先在按钮文字结束位置 绘制分割竖线 如果按钮选中或者鼠标悬停，则填充背景会将该分割线覆盖
                _pen.Color = Color.Gray;
                canvas.DrawLine(_pen, btn.StartX + _space + w + _space, 0 + 3, btn.StartX + _space + w + _space, this.Height - 3);

                if (!btn.MouseOver && !btn.Selected)
                {
                    _brush.Color = Color.Black;
                    canvas.DrawString(btn.Title, _font, _brush, totalWidth, _toph);
                }
                else
                {
                    //如果鼠标悬停 则填充背景
                    _brush.Color = Color.Black;
                    canvas.FillRectangle(_brush, btn.StartX>0?(btn.StartX - 1):btn.StartX, 0, _space + w + _space + 1 + 1, this.Height);
                    _brush.Color = Color.White;
                    canvas.DrawString(btn.Title, _font, _brush, totalWidth, _toph);
                }
                totalWidth = totalWidth + w + _space + 1;
                btn.EndX = totalWidth;
            }

            this.Width = totalWidth+200;
        }

        

        public void AddBlock(string title,Predicate<MDSymbol> filter,EnumQuoteListType type)
        {
            BlockButton btn = new BlockButton(title,filter,type);
            _btnList.Add(btn);
        }





        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    //e.Graphics.FillRectangle(_brush, e.ClipRectangle);
        //}
        
    }
}
