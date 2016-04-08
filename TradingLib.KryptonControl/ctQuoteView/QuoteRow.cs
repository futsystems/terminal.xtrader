﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.KryptonControl
{
    public class QuoteRow
    {
        public event IntDelegate SendRowLastPriceChangedEvent;//成交价格变动
        public event IntRetIntDel getColumnStartX;//得到列宽
        public event IntRetIntDel getColumnWidth;//得到列宽
        public event RetIntDel getRowWidth;//得到行宽
        public event RetIntDel getBeginIndex;

        //闪动最新价
        void FlashLoaction()
        {
            if (SendRowLastPriceChangedEvent != null)
                SendRowLastPriceChangedEvent(RowID);
        }
        /*
        public QuoteRow(string symbol, decimal last, int size, decimal ask, decimal bid, int asksize, int bidsize, decimal open, decimal high, decimal low, int preoi, int oi)
        {
            _symbol = symbol;
            _last = last;
            _size = size;
            _ask = ask;
            _bid = bid;
            _asksize = asksize;
            _bidsize = bidsize;
            _open = open;
            _high = high;
            _low = low;
            _preoi = preoi;
            _oi = oi;
        }
        public QuoteRow(Tick k)
        {
            _symbol = k.symbol;
            _last = k.trade;
            _size = k.size;
            _ask = k.ask;
            _bid = k.bid;
            _asksize = k.AskSize;
            _bidsize = k.BidSize;
        }**/
        //列名称,默认单元格样式
        //行编号,列名,列宽,默认quote样式,默认cell样式
        public ViewQuoteList _quotelist;
        public ViewQuoteList QuoteTable { get { return _quotelist; }}
       
        /// <summary>
        /// 初始化一个QuoteRow
        /// </summary>
        /// <param name="quotelist">用于传递quotelist控件引用,调用invaildate</param>
        /// <param name="rid">行序号</param>
        /// <param name="columes">列名</param>
        /// <param name="columnWidths">列起点/列宽</param>
        /// <param name="defaultQuoteStyle">默认的绘图配置</param>
        /// <param name="defaultCellStyle"></param>
        /// 
        //int[] _columnStartX;
        //int[] _columnWidth;
        //int _totalWidth;

        Symbol _symbol = null;
        public Symbol Symbol { get { return _symbol; } }
        EnumQuoteType _quoteType = EnumQuoteType.CNQUOTE;
        public EnumQuoteType QuoteType { get { return _quoteType; } set { _quoteType = value; } }
        public QuoteRow(ViewQuoteList quotelist,Symbol symbol,int rid, ref string[] columes,IntRetIntDel startfun,IntRetIntDel widthfun,RetIntDel rowwidthfun,RetIntDel beginIdxfun, QuoteStyle defaultQuoteStyle,string displayformat,EnumQuoteType type)//, CellStyle defaultCellStyle)
        {
            _quotelist = quotelist;
            _symbol = symbol;

            RowID = rid;
            _columesname = columes;
            _defaultQuoteStyle = defaultQuoteStyle;
            //_defaultCellStyle = defaultCellStyle;
            //_columnWidth = columnWidth;
            //_columnStartX = columnStartX;
            //_totalWidth = totalwidht;
            getColumnStartX = startfun;
            getColumnWidth = widthfun;
            getRowWidth = rowwidthfun;
            getBeginIndex = beginIdxfun;

            cellRectsArray = new Rectangle[columes.Length];//单元格的绘图区间
            PriceDispFormat = displayformat;
            _quoteType = type;

            //初始化行
            InitRow();

        }

        QuoteStyle _defaultQuoteStyle;
        //列名
        string[] _columesname;
        private string[] ColumnNames { get { return _columesname; } }
        //价格显示格式
        string _pricedispformat = "{0:F1}";
        public string PriceDispFormat { get { return _pricedispformat; } set { _pricedispformat = value; } }
        //{ SYMBOL, LAST, LASTSIZE, ASK, BID, ASKSIZE, BIDSIZE, VOL, CHANGE, OI, OICHANGE, SETTLEMENT, OPEN, HIGH, LOW, LASTSETTLEMENT };

        //模拟更新某单元格数据
        public void update(int col, decimal value)
        {
            if (value != this[col].Value)
            {
                this[col].Value = value;
                Rectangle c = getChangedCellRect(col);
                QuoteTable.Invalidate(c);
                debug("请求更新");
                debug("x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());
            }
        }
        //关于gottick后界面的更新
        //1.单条更新的,
        Tick _lastTick;//保存前一条Tick用于对比后进行数据显示
        Tick _nowtick;//用于更新的Tick
        public void GotTick(Tick k)
        {
            if (_lastTick == null)
            {
                _lastTick = k;
                _nowtick = k;
            }
            _lastTick = _nowtick;
            _nowtick = k;
            //debug("got a tick");
            //this[QuoteListConst.SYMBOL].symbol = k.symbol;
            //将数据更新到cell value中去
            if (k.IsTrade())
            {
                //1.更新最新价
                if (k.Trade != this[QuoteListConst.LAST].Value)
                {
                    if (RowID != QuoteTable.SelectedQuoteRow)
                    {
                        this[QuoteListConst.LAST].CellStyle.FontColor = k.Trade < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
                        cellChanged(QuoteListConst.LAST, k.Trade > this[QuoteListConst.LAST].Value ? Color.Tomato : Color.SpringGreen);
                        FlashLoaction();
                    }
                    this[QuoteListConst.LAST].Value = k.Trade;//更新单元各value
                    //Rectangle cellRectChanged = getChangedCellRect(QuoteListConst.LAST);//获得单元各更新区域
                    //QuoteTable.Invalidate(cellRectChanged);//请求quotelist更新该区域
                    //cellLocations.Write(RowID);
                }

                //更新涨跌
                decimal baseprice = _quoteType== EnumQuoteType.CNQUOTE?k.PreSettlement:k.PreClose;
                if ((k.Trade - baseprice) != this[QuoteListConst.CHANGE].Value)
                {
                    this[QuoteListConst.CHANGE].Value = k.Trade - baseprice;
                    this[QuoteListConst.CHANGE].CellStyle.FontColor = (k.Trade - k.PreSettlement) < 0 ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
                }

                this[QuoteListConst.LASTSIZE].Value = k.Size;
                //Rectangle cellRectChanged = getChangedCellRect(QuoteListConst.LASTSIZE);//获得单元各更新区域
               // QuoteTable.Invalidate(cellRectChanged);//请求quotelist更新该区域
            }

            
            //更新当前的Tick数据

            if (k.AskPrice != this[QuoteListConst.ASK].Value)
            {
                this[QuoteListConst.ASK].Value = k.AskPrice;
                this[QuoteListConst.ASK].CellStyle.FontColor = k.AskPrice < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }
            if (k.BidPrice != this[QuoteListConst.BID].Value)
            {
                this[QuoteListConst.BID].Value = k.BidPrice;
                this[QuoteListConst.BID].CellStyle.FontColor = k.BidPrice < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            this[QuoteListConst.BIDSIZE].Value = k.BidSize;
            this[QuoteListConst.ASKSIZE].Value = k.AskSize;


            this[QuoteListConst.VOL].Value = k.Vol;


            this[QuoteListConst.OI].Value = k.OpenInterest;

            this[QuoteListConst.OICHANGE].Value = k.OpenInterest != 0 ? (k.OpenInterest - k.PreOpenInterest) : 0;

            this[QuoteListConst.SETTLEMENT].Value = _quoteType == EnumQuoteType.CNQUOTE ? k.Settlement :0;

            if (k.Open != this[QuoteListConst.OPEN].Value)
            {
                this[QuoteListConst.OPEN].Value = k.Open;
                this[QuoteListConst.OPEN].CellStyle.FontColor = k.Open < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }
            if (k.Low != this[QuoteListConst.HIGH].Value)
            {
                this[QuoteListConst.HIGH].Value = k.High;
                this[QuoteListConst.HIGH].CellStyle.FontColor = k.High < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            if (k.Low != this[QuoteListConst.LOW].Value)
            {
                this[QuoteListConst.LOW].Value = k.Low;
                this[QuoteListConst.LOW].CellStyle.FontColor = k.Low < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            }

            this[QuoteListConst.LASTSETTLEMENT].Value = _quoteType == EnumQuoteType.CNQUOTE ? k.PreSettlement : k.PreClose;

            QuoteTable.Invalidate(this.Rect);
        }
        //RingBuffer<int> cellLocations = new RingBuffer<int>(1000);
        //改变某个单元格的背景颜色
        private void cellChanged(string col, Color c)
        {
                this[col].CellStyle.BackColor = c;
        }

        //序号对应的单元格
        public Dictionary<int, QuoteCell> _columeCellMap = new Dictionary<int, QuoteCell>();
        //colume名称对应的序号
        public Dictionary<string, int> _columeString2idx = new Dictionary<string, int>();

        //对应的行号
        private int _rowid;
        public int RowID { get { return _rowid; } set { _rowid = value; } }
        
       

        public event DebugDelegate SendDebutEvent;
        void debug(string msg)
        {
            if (SendDebutEvent != null)
                SendDebutEvent(msg);
        }

        int column2Idx(string column)
        {
            int idx = -1;
            if (_columeString2idx.TryGetValue(column, out idx))
                return idx;
            return idx;
        }

        /// <summary>
        /// 通过序号返回对应的Cell
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public QuoteCell this[int index]
        {
            get { return _columeCellMap[index]; }
        }


        /// <summary>
        /// 通过列名返回对应的Cell
        /// </summary>
        /// <param name="columnname"></param>
        /// <returns></returns>
        public QuoteCell this[string columnname]
        {

            get { return _columeCellMap[column2Idx(columnname)]; }
        }

        //public void AddCell(int index, QuoteCell cell)
        //{
        //    if (!_columeCellMap.ContainsKey(index))
        //        _columeCellMap.Add(index, cell);
        //    else
        //        _columeCellMap[index] = cell;
        //}

        //初始化行
        public void InitRow()
        {
            //根据行号得到列底色基本配置
            CellStyle cellstyle = new CellStyle(RowID % 2 == 0 ? _defaultQuoteStyle.QuoteBackColor1 : _defaultQuoteStyle.QuoteBackColor2,Color.DarkRed, _defaultQuoteStyle.QuoteFont,_defaultQuoteStyle.LineColor);
            //遍历所有的行名 并初始化单元格
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                string columnName = ColumnNames[i];
                QuoteCell c = new QuoteCell(columnName, cellstyle, 0M, PriceDispFormat);
                //if (columnName == QuoteListConst.SYMBOLNAME)
                //{
                //    c.symbol = _symbol.Name;
                //}
                //AddCell(i,c);
                c.SendDebutEvent += new DebugDelegate(debug);
                _columeCellMap.Add(i, c);
                _columeString2idx.Add(ColumnNames[i], i);
                //_columeCellValueMap.Add(i,0);
            }
            //将固定字段设置合约与名称
            this[QuoteListConst.SYMBOL].Symbol = _symbol.Symbol;
            this[QuoteListConst.SYMBOLNAME].Symbol = _symbol.GetName();
        }
       

        Rectangle  getChangedCellRect(string column)
        {
            return getChangedCellRect(_columeString2idx[column]);
        }
        //获得该行中某个单元格的区域
        private Dictionary<int, Rectangle> cellRectsMap = new Dictionary<int, Rectangle>();
        Rectangle[] cellRectsArray;
        //通过将rect计算后放入映射列表 可以避免每次更新都进行运算,但是当列宽改变的时候我们需要将
        //映射重置
        public void ResetCellRect()
        {
            lock (cellRectsMap)
            {
                cellRectsMap.Clear();
            }
            lock (cellRectsArray)
            {
                cellRectsArray = new Rectangle[_columesname.Length];
            }
        }
        Rectangle getChangedCellRect(int colindex)
        {
            int i = colindex;
            Rectangle cellRect;
            
            if (cellRectsMap.TryGetValue(i, out cellRect))
                return cellRect;

            //if (cellRectsArray[i] != null)
            //    return cellRectsArray[i];
            Point cellLocation = new Point(getColumnStartX(i), (RowID-getBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
            cellRect = new Rectangle(cellLocation.X, cellLocation.Y, getColumnWidth(i), _defaultQuoteStyle.RowHeight);
            cellRectsMap.Add(i, cellRect);
            //cellRectsArray[i] = cellRect;
            return cellRect;
        }

        //返回本row所在区域 每行起点就是从0-整个控件宽度
        private Rectangle _rowrect;
        private bool _rowrectsetted = false;

        /// <summary>
        /// 获得QuoteCell对应的绘图区域
        /// 当重置RowRect后 需要重新计算Quote对应的绘图区域
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                if (_rowrectsetted)
                    return _rowrect;

                Point cellLocation = new Point(0, (RowID - getBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
                Rectangle cellRect = new Rectangle(cellLocation.X, cellLocation.Y, getRowWidth(), _defaultQuoteStyle.RowHeight);
                _rowrect = cellRect;
                return cellRect;
            }
        }

        /// <summary>
        /// 重置Row绘图区域
        /// </summary>
        public void ResetRowRect()
        {
            _rowrectsetted = false;
        }

        //行的绘制函数
        //paint过程调用的函数要尽量减少运算,这样可以降低系统资源的消耗。
        //可以将一些运算通过一次运算下次取值的方式放入映射列表。这样可以有效的降低运算CPU消耗
        public void Paint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //检查需要更新的矩形区域与本单元格的矩形区域是否相交,如果相交则我们进行更新
            if (e.ClipRectangle.IntersectsWith(this.Rect))
            {
                //debug("更新Row: "+RowID.ToString());
                //Rectangle c = this.Rect;
                //debug("本行区域 x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());
                //c = e.ClipRectangle;
                //debug("更新区域：x:" + c.X.ToString() + " y:" + c.Y.ToString() + " w:" + c.Width.ToString() + " h:" + c.Height.ToString());

                //遍历每一个单元格并绘制0:Symbol不需要更新
                for (int i = 0; i < _columesname.Length; i++)
                {
                    //数值发生变化重绘
                    
                    //debug("cells:"+_columeCellMap.Count.ToString());
                    //PointF cellLocation = new PointF(getColumnStart(i), RowID * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
                    //RectangleF cellRect = new RectangleF(cellLocation.X, cellLocation.Y, getColumnWidth(i), _defaultQuoteStyle.RowHeight);
                    //debug("key:"+i.ToString()+_columeCellMap[i].ToString());
                    //debug(_columeCellMap[i].Value.ToString());
                    //debug("X:" + cellRect.X.ToString() + " Y:" + cellRect.Y.ToString());
                    Rectangle cellRect = getChangedCellRect(i);

                    _columeCellMap[i].Paint(e,cellRect , _defaultQuoteStyle);
                }
            }


        }
    }

}
