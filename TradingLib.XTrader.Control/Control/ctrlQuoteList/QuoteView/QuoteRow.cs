using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using TradingLib.MarketData;


namespace TradingLib.XTrader.Control
{
    public class QuoteRow
    {
        /// <summary>
        /// QuoteList报价对象
        /// </summary>
        public ViewQuoteList _quotelist;

        /// <summary>
        /// Quote样式
        /// </summary>
        QuoteStyle _defaultQuoteStyle;

        /// <summary>
        /// 报价显示样式
        /// </summary>
        string _pricedispformat = "{0:F1}";

        private int _rowid;
        /// <summary>
        /// 行号
        /// </summary>
        public int RowID { get { return _rowid; } set { _rowid = value; } }


        MDSymbol _symbol = null;
        /// <summary>
        /// 合约
        /// </summary>
        public MDSymbol Symbol 
        { 
            get { return _symbol; }
            set {
                _symbol = value;
                _pricedispformat = _symbol.GetFormat();
                this.SetUnchangedCell();
            }
        
        }


        //EnumQuoteType _quoteType = EnumQuoteType.CNQUOTE;
        ///// <summary>
        ///// 报价类别
        ///// </summary>
        //public EnumQuoteType QuoteType { get { return _quoteType; } set { _quoteType = value; } }


        bool _selected = false;
        public bool Selected {
            get { return _selected; }

            set {
                _selected = value;

                foreach (var cell in _columeCellMap.Values)
                {
                    if (_selected)
                    {
                        cell.CellStyle.BackColor = _quotelist.SelectedColor;
                        cell.CellStyle.LineColor = _quotelist.SelectedColor;
                    }
                    else
                    {
                        cell.CellStyle.BackColor = (_rowid % 2 == 0 ? _quotelist.QuoteBackColor1 : _quotelist.QuoteBackColor2);
                        cell.CellStyle.LineColor = _quotelist.TableLineColor;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quotelist">用于传递quotelist控件引用,调用invaildate</param>
        /// <param name="rid">行序号</param>
        /// <param name="symbol">合约</param>
        /// <param name="type">报价类别</param>
        public QuoteRow(ViewQuoteList quotelist, int rid, MDSymbol symbol)
        {
            _quotelist = quotelist;
            _symbol = symbol;
            _rowid = rid;

            _defaultQuoteStyle = quotelist.DefaultQuoteStyle;
            _pricedispformat = symbol.GetFormat();

            //初始化行
            InitRow();

        }

        /// <summary>
        /// 初始化QuoteRow
        /// </summary>
        public void InitRow()
        {
            //根据行号得到列底色基本配置
            CellStyle cellstyle = new CellStyle(RowID % 2 == 0 ? _defaultQuoteStyle.QuoteBackColor1 : _defaultQuoteStyle.QuoteBackColor2, Color.DarkRed, _defaultQuoteStyle.QuoteFont, _defaultQuoteStyle.SymbolFont, _defaultQuoteStyle.LineColor);
            //遍历所有的行名 并初始化单元格
            //for (int i = 0; i < _quotelist.Columns.Length; i++)
            foreach(var column in _quotelist.Columns)
            {
                QuoteCell cell = new QuoteCell(this,column, cellstyle,double.NaN, _pricedispformat);
                //cell.SendDebutEvent += new DebugDelegate(debug);
                //添加Cell到数据结构
                _columeCellMap.Add(column.FieldType, cell);
                //_columeCellMap.Add(i, cell);
                //_columeName2idx.Add(_quotelist.Columns[i], i);
            }

            this.SetUnchangedCell();
        }

        /// <summary>
        /// 重置单元格值
        /// </summary>
        public void ResetCellValue()
        {
            foreach (var cell in _columeCellMap.Values)
            {
                if (cell.Column.FieldType == EnumFileldType.INDEX || cell.Column.FieldType == EnumFileldType.SYMBOL || cell.Column.FieldType == EnumFileldType.SYMBOLNAME)
                {
                    continue;
                }
                cell.Value = double.NaN;
            }
        }
        void SetUnchangedCell()
        {
            this[EnumFileldType.SYMBOL].Symbol = _symbol.Symbol;
            this[EnumFileldType.SYMBOLNAME].Symbol = _symbol.Name;
            //设置合约/名称字段
            //if (_quotelist.Columns.Contains(QuoteListConst.SYMBOL))
            //{
            //    this[QuoteListConst.SYMBOL].Symbol = _symbol.Symbol;
            //}
            //if (_quotelist.Columns.Contains(QuoteListConst.SYMBOLNAME))
            //{
            //    this[QuoteListConst.SYMBOLNAME].Symbol = _symbol.Name;
            //}
        }

        /// <summary>
        /// 更新数值
        /// </summary>
        public void Update()
        {
            //没有有效行情直接返回
            if (_symbol.TickSnapshot.Time <= 0)
                return;
            //遍历所有单元格 按单元格类型进行数据更新 同时设定样式，行情快照 与单元格数据 一一对应
            foreach (var cell in _columeCellMap.Values)
            {
                //行情没有最新价 则该合约处于停盘或其他情况
                if(!_symbol.TickSnapshot.IsValid())
                {
                    if (cell.Column.FieldType != EnumFileldType.PRECLOSE)//
                    {
                        continue;
                    }

                }
                switch (cell.Column.FieldType)
                {
                    case EnumFileldType.PRECLOSE:
                        cell.Value = _symbol.TickSnapshot.last;
                        break;

                    case EnumFileldType.LAST:
                        cell.Value = _symbol.TickSnapshot.Price;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Price, _symbol.TickSnapshot.last);
                        if (_symbol.LastTickSnapshot.Price > 0 && _symbol.TickSnapshot.Price != _symbol.LastTickSnapshot.Price)
                        {
                            cell.CellStyle.BackColor = _symbol.TickSnapshot.Price > _symbol.LastTickSnapshot.Price ? Color.Tomato : Color.SpringGreen;
                            _quotelist.BookLocation(_rowid);
                        }
                        break;
                    case EnumFileldType.LASTSIZE:
                        cell.Value = _symbol.TickSnapshot.Size;
                        break;
                    case EnumFileldType.BIDSIZE:
                        cell.Value = _symbol.TickSnapshot.BuyQTY1;
                        break;
                    case EnumFileldType.BID:
                        cell.Value = _symbol.TickSnapshot.Buy1;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Buy1, _symbol.TickSnapshot.last);
                        break;
                    case EnumFileldType.ASK:
                        cell.Value = _symbol.TickSnapshot.Sell1;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Sell1, _symbol.TickSnapshot.last);
                        break;
                    case EnumFileldType.ASKSIZE:
                        cell.Value = _symbol.TickSnapshot.SellQTY1;
                        break;
                    case EnumFileldType.VOL:
                        cell.Value = _symbol.TickSnapshot.Volume;
                        break;
                    case EnumFileldType.CHANGE:
                        cell.Value = _symbol.TickSnapshot.Price - _symbol.TickSnapshot.last;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Price - _symbol.TickSnapshot.last, 0);
                        break;
                    case EnumFileldType.CHANGEPECT:
                        cell.Value = (_symbol.TickSnapshot.Price - _symbol.TickSnapshot.last) / _symbol.TickSnapshot.last * 100;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Price - _symbol.TickSnapshot.last, 0);
                        break;
                    case EnumFileldType.OI:
                    case EnumFileldType.OICHANGE:
                    case EnumFileldType.SETTLEMENT:
                        break;
                    case EnumFileldType.OPEN:
                        cell.Value = _symbol.TickSnapshot.Open;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Open, _symbol.TickSnapshot.last);
                        break;
                    case EnumFileldType.HIGH:
                        cell.Value = _symbol.TickSnapshot.High;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.High, _symbol.TickSnapshot.last);
                        break;
                    case EnumFileldType.LOW:
                        cell.Value = _symbol.TickSnapshot.Low;
                        cell.CellStyle.FontColor = _quotelist.GetUpDownColor(_symbol.TickSnapshot.Low, _symbol.TickSnapshot.last);
                        break;
                    case EnumFileldType.PRESETTLEMENT:
                        break;
                    
                    case EnumFileldType.PREOI:
                        break;
                    case EnumFileldType.EXCHANGE:
                        break;
                    case EnumFileldType.AVGPRICE:
                        break;
                    case EnumFileldType.BSIDE:
                        cell.Value = _symbol.TickSnapshot.B;
                        break;
                    case EnumFileldType.SSIDE:
                        cell.Value = _symbol.TickSnapshot.S;
                        break;
                    default:
                        break;
                }
            }
            _quotelist.Invalidate(this.Rect);
        }
        
        //{ SYMBOL, LAST, LASTSIZE, ASK, BID, ASKSIZE, BIDSIZE, VOL, CHANGE, OI, OICHANGE, SETTLEMENT, OPEN, HIGH, LOW, LASTSETTLEMENT };

        //public void GotTick(Tick k)
        //{
            //将数据更新到cell value中去
            //if (k.IsTrade())
            //{
            //    //1.更新最新价
            //    if (k.Trade != this[EnumFileldType.LAST].Value)
            //    {
            //        //选中的行 不执行闪亮操作
            //        if (RowID != _quotelist.SelectedQuoteRow)
            //        {
            //            this[EnumFileldType.LAST].CellStyle.FontColor = k.Trade < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //            CellChangeColor(QuoteListConst.LAST, k.Trade > this[EnumFileldType.LAST].Value ? Color.Tomato : Color.SpringGreen);
            //        }
            //        this[EnumFileldType.LAST].Value = k.Trade;
            //    }

            //    //更新涨跌
            //    decimal baseprice = _quoteType== EnumQuoteType.CNQUOTE?k.PreSettlement:k.PreClose;
            //    if ((k.Trade - baseprice) != this[EnumFileldType.CHANGE].Value)
            //    {
            //        this[EnumFileldType.CHANGE].Value = k.Trade - baseprice;
            //        this[EnumFileldType.CHANGE].CellStyle.FontColor = (k.Trade - k.PreSettlement) < 0 ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //    }

            //    this[EnumFileldType.LASTSIZE].Value = k.Size;
            //}

            
            ////更新当前的Tick数据
            //if (k.AskPrice != this[EnumFileldType.ASK].Value)
            //{
            //    this[EnumFileldType.ASK].Value = k.AskPrice;
            //    this[EnumFileldType.ASK].CellStyle.FontColor = k.AskPrice < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //}
            //if (k.BidPrice != this[EnumFileldType.BID].Value)
            //{
            //    this[EnumFileldType.BID].Value = k.BidPrice;
            //    this[EnumFileldType.BID].CellStyle.FontColor = k.BidPrice < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //}

            //this[EnumFileldType.BIDSIZE].Value = k.BidSize;
            //this[EnumFileldType.ASKSIZE].Value = k.AskSize;

            //this[EnumFileldType.VOL].Value = k.Vol;

            //this[EnumFileldType.OI].Value = _quoteType == EnumQuoteType.CNQUOTE ? k.OpenInterest : k.PreOpenInterest;

            //this[EnumFileldType.OICHANGE].Value = k.OpenInterest != 0 ? (k.OpenInterest - k.PreOpenInterest) : 0;

            //this[EnumFileldType.SETTLEMENT].Value = _quoteType == EnumQuoteType.CNQUOTE ? k.Settlement : 0;
            //this[EnumFileldType.PRESETTLEMENT].Value = _quoteType == EnumQuoteType.CNQUOTE ? k.PreSettlement : k.PreClose;


            //if (k.Open != this[EnumFileldType.OPEN].Value)
            //{
            //    this[EnumFileldType.OPEN].Value = k.Open;
            //    this[EnumFileldType.OPEN].CellStyle.FontColor = k.Open < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //}
            //if (k.Low != this[EnumFileldType.HIGH].Value)
            //{
            //    this[EnumFileldType.HIGH].Value = k.High;
            //    this[EnumFileldType.HIGH].CellStyle.FontColor = k.High < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //}

            //if (k.Low != this[EnumFileldType.LOW].Value)
            //{
            //    this[EnumFileldType.LOW].Value = k.Low;
            //    this[EnumFileldType.LOW].CellStyle.FontColor = k.Low < k.PreSettlement ? _defaultQuoteStyle.DNColor : _defaultQuoteStyle.UPColor;
            //}

            //重绘该行
        //    _quotelist.Invalidate(this.Rect);
        //}



        
        //改变某个单元格的背景颜色
        private void CellChangeColor(string col, Color c)
        {
            //this[col].CellStyle.BackColor = c;
            _quotelist.BookLocation(_rowid);
        }


        #region 数据结构 用于存放序号与Cell
        //序号对应的单元格
        Dictionary<EnumFileldType, QuoteCell> _columeCellMap = new Dictionary<EnumFileldType, QuoteCell>();
        //colume名称对应的序号
        //Dictionary<string, int> _columeName2idx = new Dictionary<string, int>();


        //int column2Idx(string column)
        //{
        //    int idx = -1;
        //    if (_columeName2idx.TryGetValue(column, out idx))
        //        return idx;
        //    return idx;
        //}

        /// <summary>
        /// 通过序号返回对应的Cell
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public QuoteCell this[EnumFileldType type]
        {
            get { return _columeCellMap[type]; }
        }


        /// <summary>
        /// 通过列名返回对应的Cell
        /// </summary>
        /// <param name="columnname"></param>
        /// <returns></returns>
        //public QuoteCell this[string columnname]
        //{

        //    get { return _columeCellMap[column2Idx(columnname)]; }
        //}

        #endregion


        #region 序号与RectangleMap
        //获得该行中某个单元格的区域
       // private Dictionary<int, Rectangle> cellRectsMap = new Dictionary<int, Rectangle>();
        //通过将rect计算后放入映射列表 可以避免每次更新都进行运算,但是当列宽改变的时候我们需要将
        //public void ResetCellRect()
        //{
            
        //}

        /// <summary>
        /// 获得某个Cell的绘图区域
        /// </summary>
        /// <param name="colindex"></param>
        /// <returns></returns>
        //Rectangle GetCellRect(int colindex)
        //{
        //    int i = colindex;
        //    Rectangle cellRect;
        //    if (cellRectsMap.TryGetValue(i, out cellRect))
        //        return cellRect;
        //    //没有缓存单元格序号对应的绘图区域 需要计算该绘图区域
        //    Point cellLocation = new Point(_quotelist.GetColumnStarX(i), (RowID - _quotelist.GetBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
        //    cellRect = new Rectangle(cellLocation.X, cellLocation.Y, _quotelist.GetColumnWidth(i), _defaultQuoteStyle.RowHeight);
        //    cellRectsMap.Add(i, cellRect);
        //    return cellRect;
        //}


        /// <summary>
        /// 重置Row绘图区域
        /// </summary>
        public void ResetRect()
        {
            _rectSetted = false;
            foreach (var cell in _columeCellMap.Values)
            {
                cell.ResetRect();
            }
        }

        //返回本row所在区域 每行起点就是从0-整个控件宽度
        private Rectangle _rowrect;
        private bool _rectSetted = false;

        /// <summary>
        /// 获得QuoteRow对应的绘图区域
        /// 当重置RowRect后 需要重新计算Quote对应的绘图区域
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                if (_rectSetted)
                    return _rowrect;

                Point cellLocation = new Point(0, (RowID - _quotelist.GetBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
                _rowrect = new Rectangle(cellLocation.X, cellLocation.Y, _quotelist.VisibleColumns.Sum(column=>column.Width), _defaultQuoteStyle.RowHeight);
                _rectSetted = true;
                return _rowrect;
            }
        }

        #endregion

        

        //行的绘制函数
        //paint过程调用的函数要尽量减少运算,这样可以降低系统资源的消耗。
        //可以将一些运算通过一次运算下次取值的方式放入映射列表。这样可以有效的降低运算CPU消耗
        public void Paint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //检查需要更新的矩形区域与本单元格的矩形区域是否相交,如果相交则我们进行更新
            if (e.ClipRectangle.IntersectsWith(this.Rect))
            {
                foreach (var cell in _columeCellMap.Values.Where(cell=>cell.Column.Visible))
                {
                    if (cell.NeedCalcRect)
                    { 
                         //缓存单元格区域失效 重新计算
                        Point cellLocation = new Point(cell.Column.StartX, (RowID - _quotelist.GetBeginIndex()) * _defaultQuoteStyle.RowHeight + _defaultQuoteStyle.HeaderHeight);
                        cell.CellRect =  new Rectangle(cellLocation.X, cellLocation.Y,cell.Column.Width, _defaultQuoteStyle.RowHeight);
                    }
                    cell.Paint(e, _defaultQuoteStyle);
                }
            }


        }

        //public event DebugDelegate SendDebutEvent;
        void debug(string msg)
        {
            //if (SendDebutEvent != null)
            //    SendDebutEvent(msg);
        }

    }

}
