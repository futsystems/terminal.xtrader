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
        CursorType _cursorType = CursorType.NONE;

        //当前鼠标坐标
        private int _mouseX;
        private int _mouseY;

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
                _mouseX = e.X;
                _mouseY = e.Y;

                if (_cursorType == CursorType.NONE)
                {
                    int colIdx = MouseIInYLineIdentity(e);
                    if (colIdx != -1)
                    {
                        this.Cursor = Cursors.SizeWE;
                        CurrentMoveYLIneID = colIdx;
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                    }
                    
                }

                if (_cursorType == CursorType.CHANGEWIDTH)
                {
                    MoveChangeColWidthLine(e, CurrentMoveYLIneID);
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
                    if (e.Y > 0 && e.Y < this.HeaderHeight)//在标题栏进行鼠标位置判定
                    {
                        int currentColumn = MouseIInYLineIdentity(e);
                        if (currentColumn !=-1)
                        {
                            logger.Info("can chagne width column:" + currentColumn.ToString());
                            _cursorType = CursorType.CHANGEWIDTH;
                            CurrentMoveYLIneID = currentColumn;
                        }
                    }
                    else //在非标题区域 则选择某行
                    {
                        int rowId = Convert.ToInt16((e.Y - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight) + _beginIdx;
                        SelectRow(rowId);
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
            if (_cursorType == CursorType.CHANGEWIDTH)
            {
                _cursorType = CursorType.NONE;
                this.Refresh();
            }
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
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    if (e.X > visibleColumns[i].StartX - 3 && e.X < visibleColumns[i].StartX + 3)
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
            CurrentYLineMoveWidth = (e.X - visibleColumns[CurrentMoveYLIneID].StartX);//计算移动值
            visibleColumns[CurrentMoveYLIneID - 1].Width = visibleColumns[CurrentMoveYLIneID - 1].Width + CurrentYLineMoveWidth;
            CalcColunmStartX();
            ResetRect();
            Refresh();
        }

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
            if (MouseEvent != null && symbol!=null)
            {
                MouseEvent(symbol, QuoteMouseEventType.SymbolDoubleClick);
            }
            //if (SymbolSelectedEvent != null)
            //{
            //    SymbolSelectedEvent(symbol);
            //}
            ////CoreService.EventUI.FireSymbolSelectedEvent(this, symbol);
            //debug("Symbol:" + symbol.ToString() + " Selected");
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
