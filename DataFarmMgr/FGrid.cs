﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TradingLib.DataFarmManager
{
    public class  FGrid:System.Windows.Forms.DataGridView
    {
        static Color lineColor = Color.FromArgb(127, 157, 185);
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
           

            this.EnableHeadersVisualStyles = false;//表头部适用系统样式

            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.BackgroundColor = Color.White;
            this.Margin = new Padding(0);
            //this.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;

            this.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(FPosition_CellPainting);
            this.Paint += new System.Windows.Forms.PaintEventHandler(FPosition_Paint);
        }



        

        void FPosition_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid,
                lineColor, 1, ButtonBorderStyle.Solid);
        }

        void FPosition_CellPainting(object sender, System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
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
