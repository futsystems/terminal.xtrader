using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.MarketData;

namespace TradingLib.XTrader.Control
{
    public partial class ViewQuoteList
    {
        #region 添加 删除合约
        /// <summary>
        /// 增加一个合约到显示列表
        /// QuoteList初始为空 在添加合约过程中创建，且可以复用，避免视图切换时频繁创建该对象
        /// QuoteList维护了一个列表 用于存放QuoteRow
        /// _symbolIdxMap记录了合约->QuoteList下标的映射
        /// </summary>
        /// <param name="sec"></param>
        public void AddSymbol(MDSymbol symbol)
        {
            //1.检查是否存在该symbol,如果存在则直接返回
            if (_symbolIdxMap.Keys.Contains(symbol.UniqueKey)) return;
            try
            {
                int i = _count;
                _count++;
                //QuoteList没有足够的QuoteRow则新建
                if (_quoteList.Count < _count)
                {
                    QuoteRow qr = new QuoteRow(this, i, symbol);
                    //qr.SendDebutEvent += new DebugDelegate(debug);
                    _quoteList.Add(qr);
                }
                else//设定QuoteRow现实的合约对象
                {
                    _quoteList[i].Symbol = symbol;
                    _quoteList[i].ResetCellValue();
                    _quoteList[i].Update();
                }
                _symbolIdxMap.Add(symbol.UniqueKey, i);
                //更新当前的序号
                _endIdx = _count - 1;

                //如果当前没有默认选中某行 则选中第一行
                if (_selectedRow == -1)
                {
                    SelectRow(0);
                }

                //重新绘制窗口的某个特定部分
                if (_needInvalidate)
                {
                    Invalidate(_quoteList[i].Rect);
                    FireQuoteViewChange();
                }
            }
            catch (Exception ex)
            {
                debug(ex.ToString());
            }
        }

        ///// <summary>
        ///// 删除某个合约
        ///// </summary>
        ///// <param name="sec"></param>
        //public void RemoveSymbol(MDSymbol symbol)
        //{
        //    string sym = symbol.Symbol;
        //    int rid;
        //    //如果symbolIdx没有找到Symbol对应的行号 则直接返回
        //    if (!_symbolIdxMap.TryGetValue(sym, out rid))
        //    {
        //        return;
        //    }

        //    //删除某行对应合约 就是将下一行合约往上赋值
        //    for (int i = rid; i < _count - 1; i++)
        //    {
        //        QuoteRow now = _quoteList[i];
        //        QuoteRow next = _quoteList[i + 1];
        //        now.Symbol = next.Symbol;
        //        _symbolIdxMap[now.Symbol.Symbol] = i;
        //    }
        //    _count--;

        //    _symbolIdxMap.Remove(sym);

        //    UpdateBeginEndIdx();
        //    this.ResetRect();
        //    Invalidate();

        //}

        /// <summary>
        /// 添加一组合约
        /// 外部排序后添加对应的合约
        /// </summary>
        /// <param name="b"></param>
        public void AddSymbols(IEnumerable<MDSymbol> symbols)
        {
            foreach (MDSymbol s in symbols)
            {
                AddSymbol(s);
            }
        }

        public void Clear()
        {
            //清除上次选中行
            QuoteRow row = this[_selectedRow];
            if (row != null)
            {
                row.Selected = false;
            }
            _count = 0;
            _symbolIdxMap.Clear();
            _beginIdx = 0;
            _endIdx = 0;
            _selectedRow = -1;

        }
        #endregion
    }
}
