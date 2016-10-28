using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public class  FGrid:System.Windows.Forms.DataGridView
    {
        static Color lineColor = Color.FromArgb(127, 157, 185);
        ILog logger = LogManager.GetLogger("FGrid");
        static Pen pen = new Pen(lineColor, 1);
        public FGrid()
        {


            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//如果禁止Resize则表头高度会无法修改
            this.ColumnHeadersHeight = 25;
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.Font = new Font("宋体", 9.5f);
            this.EnableHeadersVisualStyles = false;//表头部适用系统样式

            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.BackgroundColor = Color.White;
            this.Margin = new Padding(0);
            //this.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;

            _dashPen.DashStyle = DashStyle.Dot;
        }



        protected override void OnLostFocus(EventArgs e)
        {
            if (this.SelectedRows.Count > 0)
            {
                this.SetSelectedBackground(false);
            }
            base.OnLostFocus(e);
        }



        public void SetSelectedBackground(bool blue)
        {

            if (blue)
            {
                this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
                this.DefaultCellStyle.SelectionForeColor = Color.White;
            }
            else
            {
                //根据Row奇偶来定颜色
                if (this.SelectedRows.Count > 0)
                {

                    if (this.SelectedRows[0].Index % 2 == 0) //根据奇偶来设定选择行背景色 以保持和原来的色调一致
                    {
                        this.DefaultCellStyle.SelectionBackColor = Color.White;
                    }
                    else
                    {
                        this.DefaultCellStyle.SelectionBackColor = this.AlternatingRowsDefaultCellStyle.BackColor;
                    }
                    
                    this.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                
            }
        }


        Pen _dashPen = new Pen(Color.Black, 1);
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (Rows[e.RowIndex].Selected)
            {
                int x = e.RowBounds.Left;
                int y = e.RowBounds.Top;
                int width = e.RowBounds.Width;
                int height = e.RowBounds.Height - 1;

                e.Graphics.DrawRectangle(_dashPen, x, y, width, height);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(e.CellBounds, Color.LightGray,
                    Color.White, LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    Rectangle border = e.CellBounds;
                    border.Offset(new Point(-1, -1));
                    e.Graphics.DrawRectangle(Pens.Gray, border);
                }
                e.PaintContent(e.CellBounds);
                e.Handled = true;
            }
            else if (e.RowIndex == -1)
            {
                //标题行
                using (LinearGradientBrush brush = new LinearGradientBrush(e.CellBounds, Color.LightGray,
                    Color.White, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                    //e.Graphics.FillRectangle(brush, e.CellBounds);
                    Rectangle border = e.CellBounds;
                    border.Offset(new Point(-1, -1));
                    e.Graphics.DrawRectangle(pen, border);
                }
                e.PaintContent(e.CellBounds);
                e.Handled = true;
            }
            else if (e.ColumnIndex == -1)
            {
                //标题列
                using (LinearGradientBrush brush = new LinearGradientBrush(e.CellBounds, Color.LightGray,
                    Color.White, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    Rectangle border = e.CellBounds;
                    border.Offset(new Point(-1, -1));
                    e.Graphics.DrawRectangle(Pens.Gray, border);
                }
                e.PaintContent(e.CellBounds);
                e.Handled = true;
            }
        }
    }
}
