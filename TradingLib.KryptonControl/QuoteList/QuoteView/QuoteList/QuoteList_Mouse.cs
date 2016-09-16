using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;
using TradingLib.MarketData;

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {
        bool CanChangeMoveState = true;
        bool CanMoveColumnWidth = false;
        /// 正在拖动的ColLine已拖动距离
        /// </summary>
        int CurrentYLineMoveWidth = 0;
        /// <summary>
        /// 当前正在移动的ColLine
        /// </summary>
        int CurrentMoveYLIneID = -1;
        /// <summary>
        /// 鼠标移动事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewQuoteList_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                int ylineID;
                //判断鼠标在x y的那条线
                ylineID = MouseIInYLineIdentity(e);
                if (CanChangeMoveState)
                {
                    if (ylineID != -1)
                    {
                        //记录当前列序号
                        CurrentMoveYLIneID = ylineID;
                        this.Cursor = Cursors.SizeWE;//更改鼠标
                        CanMoveColumnWidth = true;//打开移动列开关 可以移动列
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        CanMoveColumnWidth = false;
                    }
                }

                if (!CanChangeMoveState && CanMoveColumnWidth && CurrentMoveYLIneID != -1)
                {
                    MoveChangeColWidthLine(e, ylineID);
                }

            }
            catch (Exception ex)
            {
                debug("quoteview list mouse move error");
            }
        }


        /// <summary>
        /// 鼠标点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewQuoteList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this.Focus();

                if (e.Button == MouseButtons.Left)
                {

                    if (CanMoveColumnWidth)
                    {
                        CanChangeMoveState = false;
                    }
                    else
                    {
                        //debug("click row:" + mouseX2RowID(e).ToString());
                        //当我们不处于选择状态我们单击 选择某行报价
                        int i = mouseX2RowID(e);
                        SelectRow(i);
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (_cmenu != null)
                        _cmenu.Show(new Point(MousePosition.X, MousePosition.Y));
                }
            }
            catch (Exception ex)
            {
                logger.Error("MouseDown Error:" + ex.ToString());
            }
        }

        void ViewQuoteList_MouseUp(object sender, MouseEventArgs e)
        {

            if (!CanChangeMoveState)
            {
                if (CanMoveColumnWidth)
                {
                    ChangeColWidth();
                }
            }
            CanChangeMoveState = true;
            //Invalidate();
            //this.Refresh();
        }


        private int mouseX2RowID(MouseEventArgs e)
        {
            //鼠标Y位置扣除标题高度/行高 就得到对应的行数 需要加上我们的起始现实序号
            return Convert.ToInt16((e.Y - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight) + _beginIdx;
        }

        /// <summary>
        /// 判断鼠标当前所在列
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private int MouseIInYLineIdentity(MouseEventArgs e)
        {
            if (e.Y > 0 && e.Y < this.HeaderHeight)//在标题栏进行鼠标位置判定
            {
                for (int i = 0; i < quoteColumns.Count; i++)
                {
                    if (e.X > quoteColumns[i].StartX - 3 && e.X < quoteColumns[i].StartX + 3)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 通过拖动改变列宽时显示的虚线
        /// </summary>
        /// <param name="e"></param>
        private void MoveChangeColWidthLine(MouseEventArgs e, int ylineID)
        {
            //debug("moving column");
            //输出当前鼠标坐标
            _mouseX = e.X;
            _mouseY = e.Y;

            CurrentYLineMoveWidth = (e.X - quoteColumns[CurrentMoveYLIneID].StartX);//计算移动值
            ChangeColWidth();//计算新的列宽
            columnWidthChanged();//重新计算绘制表格需要的列宽 列起点 总宽数据
            this.Refresh();//刷新
        }
        private int _mouseX;
        private int _mouseY;






        void ViewQuoteList_MouseClick(object sender, MouseEventArgs e)
        {
            MDSymbol symbol = GetVisibleSecurity(SelectedQuoteRow);
            if (SymbolSelectedEvent != null)
            {
                SymbolSelectedEvent(symbol);
            }
            //CoreService.EventUI.FireSymbolSelectedEvent(this, symbol);
            debug("Symbol:" + symbol.ToString() + " Selected");
        }

        //触发选择某个symbol的事件
        void ViewQuoteList_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MDSymbol symbol = GetVisibleSecurity(SelectedQuoteRow);
            if (SymbolSelectedEvent != null)
            {
                SymbolSelectedEvent(symbol);
            }
            //CoreService.EventUI.FireSymbolSelectedEvent(this, symbol);
            debug("Symbol:" + symbol.ToString() + " Selected");
        }

        /// <summary>
        /// 滚动鼠标轮 上下移动选行光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewQuoteList_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                RowUp();
            else
                RowDown();
        }

    }
}
