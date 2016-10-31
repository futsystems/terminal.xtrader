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
using System.Drawing.Drawing2D;

namespace TradingLib.XTrader.Future
{
    public class MenuItem
    {
        public MenuItem(string title, Bitmap img, bool canSelect, Action action)
        {
            this.Title = title;
            this.Image = img;
            this.Point = new Point();
            this.Width = 0;
            this.Selected = false;
            this.CanSelect = canSelect;
            this.EventHandler = action;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图像
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// 菜单左上角位置
        /// </summary>
        public Point Point;

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 是否处于选中状态
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 是否允许选中
        /// </summary>
        public bool CanSelect { get; set; }
        /// <summary>
        /// 事件处理
        /// </summary>
        public Action EventHandler { get; set; }
        
    }
    /// <summary>
    /// 通过Panel绘制以及Control控件直接绘制 比较发现原生控件效率高
    /// </summary>
    public partial class ctrlListMenu : System.Windows.Forms.Control
    {

      

        ILog logger = LogManager.GetLogger("ctrlTickList");
        public ctrlListMenu()
        {
            
            InitializeComponent();
            this.DoubleBuffered = true;
            linePen.DashStyle = DashStyle.Dot;

            this.SizeChanged += new EventHandler(ctrlListMenu_SizeChanged);
            this.MouseClick += new MouseEventHandler(ctrlListMenu_MouseClick);
        }

        void ctrlListMenu_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        MenuItem _selected = null;
        void ctrlListMenu_MouseClick(object sender, MouseEventArgs e)
        {
            MenuItem selectedItem = null;
            foreach (var menu in menulist)
            {
                if (e.X >= menu.Point.X && e.X <= menu.Point.X + menu.Width && e.Y >= menu.Point.Y && e.Y <= menu.Point.Y + iconSize)
                {
                    if (menu.CanSelect)
                    {
                        selectedItem = menu;
                    }
                    
                }
            }
            if (selectedItem != null)
            {
                selectedItem.Selected = true;

                if (_selected != null)
                {
                    _selected.Selected = false;
                }
                _selected = selectedItem;
                //对外输出事件
                Invalidate();

                if (_selected.EventHandler != null)
                {
                    _selected.EventHandler();
                }
            }

        }



        Pen pen = new Pen(Constants.BorderColor);
        Pen linePen = new Pen(Color.Black);
        int treeLineWidth = 12;
        int iconSize = 16;
        Font font = new Font("宋体", 10f);
        SolidBrush strbrush = new SolidBrush(Color.Black);
        SolidBrush bgbrush = new SolidBrush(Constants.ListMenuSelectedBGColor);

        public void AddMenu(MenuItem item)
        {
            if (_selected == null)
            {
                _selected = item;
                item.Selected = true;
            }
            menulist.Add(item);
        }

        List<MenuItem> menulist = new List<MenuItem>();

        private void GDIControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, this.ClientRectangle);
            Rectangle rect = this.ClientRectangle;
            rect.Height -=1;
            rect.Width -=1;
            g.DrawRectangle(pen,rect);


            Point p = new System.Drawing.Point(0,0);
            p.X += 10;
            p.Y += 10;

            int i = 0;
            SizeF size;
            foreach (var menu in menulist)
            {
                if (i > 0)
                {
                    g.DrawLine(linePen, p.X, p.Y-iconSize, p.X, p.Y);
                }

                g.DrawLine(linePen, p.X, p.Y, p.X + treeLineWidth, p.Y);
                g.DrawImage(menu.Image, p.X + treeLineWidth, p.Y - iconSize / 2);
                size = g.MeasureString(menu.Title, font);
                if (menu.Selected)
                {

                    g.FillRectangle(bgbrush, new Rectangle(p.X + treeLineWidth + iconSize + 2, p.Y - iconSize / 2,(int)size.Width+1, iconSize));
                    strbrush.Color = Color.White;
                    g.DrawString(menu.Title, font, strbrush, p.X + treeLineWidth + iconSize + 2, p.Y - 6);
                    strbrush.Color = Color.Black;
                }
                else
                {
                    g.DrawString(menu.Title, font, strbrush, p.X + treeLineWidth + iconSize + 2, p.Y - 6);
                }
                menu.Point.X = p.X;
                menu.Point.Y = p.Y - iconSize / 2;
                menu.Width = treeLineWidth + iconSize + 2 + (int)size.Width;
                p.Y += iconSize;
                i++;
                
            }
            
        }
    }
}
