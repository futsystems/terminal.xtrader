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

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {
        int _beginIdx = 0;
        int _endIdx = 0;

        public int StartIndex
        {
            get { return _beginIdx; }
            set
            {
                int showCount = VisibleRowCount;
                int maxStartIndex = _count - showCount;
                _beginIdx = Math.Min(value, maxStartIndex);

                _endIdx = _beginIdx + showCount -1;

                this.ResetRect();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 更新我们需要显示的起点idx与终点idx 这个运算不需要每次都调用当移动光标使得显示的行改变的时候才需要进行
        /// </summary>
        void UpdateBeginEndIdx()
        {

            int[] ids = CalcRowsBeginEndIdx();
            //debug("caculate:" + _beginIdx.ToString() + "|" + _endIdx.ToString());
            int oldbegin = _beginIdx;
            int oldend = _endIdx;
            _beginIdx = ids[0];
            _endIdx = ids[1];
            //如果更新后的起点与终点发生了变化,我们需要重新计算每个单元格的rect
            if (oldbegin != _beginIdx || oldend != _endIdx)
            {
                this.ResetRect();
                if (SymbolVisibleChanged != null)
                {
                    SymbolVisibleChanged(this, new SymbolVisibleChangeEventArgs(_beginIdx, _endIdx, this.SymbolVisible.ToArray()));
                }
                //logger.Info(string.Format("start:{0} end:{1} selectd:{2}", _beginIdx, _endIdx, _selectedRow));
            }
        }

        /// <summary>
        /// 计算控件显示报价行的开始与结束序号
        /// </summary>
        /// <returns></returns>
        int[] CalcRowsBeginEndIdx()
        {
            int rows = VisibleRowCount;//计算我们显示的总行数
            //当选择的行小于我们可以显示的行 则我们显示可以显示的行数与总行数中最小的一个数字
            if (_selectedRow <= rows-1)
                return new int[] { 0, Math.Min(rows - 1, _count - 1) };
            else
            {   //如果大于可以显示的行了 则我们需要移动表格,把起点与终点都同步向上移动
                //selectedrows会对行数进行变化,当移动到没有足够的行的时候它会自己跳到首行进行显示
                return new int[] { 0 + (SelectedQuoteRow - rows + 1), rows + (SelectedQuoteRow - rows + 1) - 1 };
            }
        }

        /// <summary>
        /// 可见最大行数
        /// </summary>
        int VisibleRowCount
        {
            get { return Convert.ToInt32((this.Height - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight); }
        }
    }
}
