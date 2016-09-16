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

    public delegate int IntRetIntDel(int idx);
    public delegate int RetIntDel();

    public partial class ViewQuoteList :System.Windows.Forms.Control
    {
        ILog logger = LogManager.GetLogger("ViewQuoteList");
        //控件事件
        //public event SecurityDelegate EOpenChart;

        public event DebugDelegate SendDebugEvent;
        //public event SymDelegate SendRegisterSymbols;
        /// <summary>
        /// 双击合约时触发选择了某个合约
        /// </summary>
        public event Action<MDSymbol> SymbolSelectedEvent;

        /// <summary>
        /// 捕捉到控件上左右移动事件
        /// </summary>
        public event Action<PreviewKeyDownEventArgs> RightLeftMoveEvent;

        /// <summary>
        /// 报价单 小下单面板行情事件
        /// </summary>
        private event TickDelegate spillTick;

        public event OrderDelegate SendOrderEvent;//发送Order
        public event SymbolDelegate SendOpenTimeSalesEvent;//打开盘口窗口

        public event Action<MDSymbol> OpenKChartEvent;//打开图标窗口

        RingBuffer<int> cellLocations = new RingBuffer<int>(1000);//用于记录闪动的最新价格位置
        System.Threading.Timer _timer;

        void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        //初始
        CellStyle _cellstyle;
        public CellStyle DefaultCellStyle { get { return _cellstyle; } }
        QuoteStyle _quotestyle;
        public QuoteStyle DefaultQuoteStyle { get { return _quotestyle; } }

        
        public ViewQuoteList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            //设置单元格样式
            _cellstyle = new CellStyle(QuoteBackColor1, Color.Red, QuoteFont,SymbolFont,TableLineColor);
            _quotestyle = new QuoteStyle(QuoteBackColor1, QuoteBackColor2, QuoteFont,SymbolFont,TableLineColor,UPColor,DNColor,HeaderHeight, RowHeight);

            _timer = new System.Threading.Timer(ChangeColorBack, null, 800, 1500);
           
            
            this.MouseMove +=new MouseEventHandler(ViewQuoteList_MouseMove);
            this.MouseDown += new MouseEventHandler(ViewQuoteList_MouseDown);
            this.MouseUp += new MouseEventHandler(ViewQuoteList_MouseUp);
            this.MouseWheel += new MouseEventHandler(ViewQuoteList_MouseWheel);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(ViewQuoteList_MouseDoubleClick);
            this.MouseClick += new MouseEventHandler(ViewQuoteList_MouseClick);



            this.GotFocus += new EventHandler(ViewQuoteList_GotFocus);
            this.LostFocus += new EventHandler(ViewQuoteList_LostFocus);
            //this.PreviewKeyDown += new PreviewKeyDownEventHandler(ViewQuoteList_PreviewKeyDown);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyDown);
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ViewQuoteList_KeyPress);
            //this.KeyUp += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyUp);

            this.SizeChanged += new EventHandler(ViewQuoteList_SizeChanged);

            //初始化右键菜单
            if(MenuEnable)
                initMenu();
            //计算列起点 总宽等参数
            columnWidthChanged();

            
        }

        void ViewQuoteList_LostFocus(object sender, EventArgs e)
        {
            logger.Info("lost focus");
        }

        void ViewQuoteList_GotFocus(object sender, EventArgs e)
        {
            logger.Info("got focus");
        }

        /// <summary>
        /// 尺寸变化重新计算开始与结束行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewQuoteList_SizeChanged(object sender, EventArgs e)
        {
            UpdateBeginEndIdx();
        }

        
        //void ViewQuoteList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    logger.Info("PreviewKeyDown:" + e.KeyCode.ToString());
        //    switch(e.KeyCode)
        //    {
        //        case Keys.Up:
        //            this.RowUp();
        //            break;
        //        case Keys.Down:
        //            this.RowDown();
        //            break;
        //        case Keys.Left:
        //        case Keys.Right:
        //            {
        //                if (RightLeftMoveEvent != null)
        //                {
        //                    RightLeftMoveEvent(e);
        //                }
        //                break;
        //            }
        //        default:
        //            break;
        //    }
        //}


       
        /// <summary>
        /// 定时将闪亮的单元格 改回原来的颜色
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeColorBack(object obj)
        {
            try
            {
                while (cellLocations.hasItems)
                {
                    int r = cellLocations.Read();
                    if (r % 2 == 0)
                    {
                        this[r][QuoteListConst.LAST].CellStyle.BackColor = QuoteBackColor1;
                        Invalidate(this[r].Rect);
                    }
                    else
                    {
                        this[r][QuoteListConst.LAST].CellStyle.BackColor = QuoteBackColor1;
                        Invalidate(this[r].Rect);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ChangeColorBack Error:" + ex.ToString());
            }
            
        }



        private SolidBrush _brush = new SolidBrush(Color.Black);

        #region 计算我们需要显示的行
        
        int _beginIdx = 0;
        int _endIdx = 0;
        /// <summary>
        /// 更新我们需要显示的起点idx与终点idx 这个运算不需要每次都调用当移动光标使得显示的行改变的时候才需要进行
        /// </summary>
        void UpdateBeginEndIdx()
        {
            int[] ids = CalcRowsBeginEndIdx();
            //debug("caculate:" + _beginIdx.ToString() + "|" + _endIdx.ToString());
            int oldbegin = _beginIdx;
            _beginIdx = ids[0];
            _endIdx = ids[1];
            //如果更新后的起点与终点发生了变化,我们需要重新计算每个单元格的rect
            if (oldbegin != _beginIdx)
                this.ResetAllRect();
        }

        /// <summary>
        /// 计算控件显示报价行的开始与结束序号
        /// </summary>
        /// <returns></returns>
        int[] CalcRowsBeginEndIdx()
        {
            int rows = CalcRowsCanShow();//计算我们显示的总行数
            //当选择的行小于我们可以显示的行 则我们显示可以显示的行数与总行数中最小的一个数字
            if (SelectedQuoteRow + 1 <= rows)
                return new int[] { 0, Math.Min(rows - 1, Count - 1) };
            else
            {   //如果大于可以显示的行了 则我们需要移动表格,把起点与终点都同步向上移动
                //selectedrows会对行数进行变化,当移动到没有足够的行的时候它会自己跳到首行进行显示
                return new int[] { 0 + (SelectedQuoteRow - rows + 1), rows + (SelectedQuoteRow - rows + 1) - 1 };
            }
        }


        /// <summary>
        /// 计算当前控件高度可以显示的报价行数
        /// </summary>
        /// <returns></returns>
        int CalcRowsCanShow()
        {
            int i = ClientSize.Height;
            //得到可以显示的行数
            int rnum = Convert.ToInt32((i - DefaultQuoteStyle.HeaderHeight) / DefaultQuoteStyle.RowHeight);
            //debug("可以显示:"+rnum.ToString());
            return rnum;
        }
        
        #endregion

        int _count = 0;
        List<QuoteRow> _quoteList = new List<QuoteRow>();
        //记录symbol代码与对应Row序号的映射
        Dictionary<string, int> _symbolIdxMap = new Dictionary<string, int>();
        
        //symbol到本地idx的转换
        int symbol2idx(string symbol)
        {
            int idx;
            if (_symbolIdxMap.TryGetValue(symbol, out idx))
                return idx;
            return -1;
        }

        //通过本地idx得到quoterow
        public QuoteRow this[int idx]{
                get{
                    if (idx > _count) return null;
                    if (idx < 0) return null;
                    return _quoteList[idx];
                }
        }

        //通过symbol得到quoterow
        public QuoteRow this[string symbol] { get { return this[symbol2idx(symbol)]; } }

        
        /// <summary>
        /// 记录改变最新价颜色的行号
        /// 定时器会将颜色修改会原来的标准颜色
        /// </summary>
        /// <param name="val"></param>
        internal void BookLocation(int val)
        {
            cellLocations.Write(val);
        }

        bool _needInvalidate = true;
        public void BeginUpdate()
        {
            _needInvalidate = false;
        }

        public void EndUpdate()
        {
            _needInvalidate = true;
            UpdateBeginEndIdx();
            Invalidate();
        }

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
            string sym = symbol.Symbol;
            //1.检查是否存在该symbol,如果存在则直接返回
            if (_symbolIdxMap.Keys.Contains(sym)) return;
            try
            {
                int i = _count;
                _count++;
                //QuoteList没有足够的QuoteRow则新建
                if (_quoteList.Count < _count)
                {
                    QuoteRow qr = new QuoteRow(this, i, symbol, _quoteType);
                    qr.SendDebutEvent += new DebugDelegate(debug);
                    _quoteList.Add(qr);
                }
                else//设定QuoteRow现实的合约对象
                {
                    _quoteList[i].Symbol = symbol;
                }
                _symbolIdxMap.Add(sym, i);
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
                }
            }
            catch(Exception ex)
            {
                debug(ex.ToString());
            }
        }
        
        /// <summary>
        /// 删除某个合约
        /// </summary>
        /// <param name="sec"></param>
        public void RemoveSymbol(MDSymbol symbol)
        {
            string sym = symbol.Symbol;
            int rid;
            //如果symbolIdx没有找到Symbol对应的行号 则直接返回
            if (!_symbolIdxMap.TryGetValue(sym, out rid))
            {
                return;
            }

            //删除某行对应合约 就是将下一行合约往上赋值
            for (int i = rid; i < _count-1; i++)
            {
                QuoteRow now = _quoteList[i];
                QuoteRow next = _quoteList[i+1];
                now.Symbol = next.Symbol;
                _symbolIdxMap[now.Symbol.Symbol] = i;
            }
            _count--;

            _symbolIdxMap.Remove(sym);

            UpdateBeginEndIdx();
            this.ResetAllRect();
            Invalidate();
  
        }

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

            _count = 0;
            _symbolIdxMap.Clear();
            _beginIdx = 0;
            _endIdx = 0;
            _selectedRow = -1;

        }
        #endregion


        //报价表总行数
        public int Count { get { return _count; } }//_idxQuoteRowMap.Count; } }
        //得到新的Tick通过索引直接调用QuoteRow进行gottick更新单元格,然后按行进行数据更新
        public void GotTick(Tick k)
        {
            int idx = symbol2idx(k.Symbol);
            if ((idx == -1) || (idx > Count)) return;
            if (spillTick != null)
                spillTick(k);
            this[idx].GotTick(k); 
        }

        #region 表宽,表名

        internal string[] Columns { get { return columns; } }
        //设定列的表头名称
        private string[] columns = new string[] { QuoteListConst.SYMBOLNAME, QuoteListConst.LAST, QuoteListConst.LASTSIZE, QuoteListConst.BIDSIZE, QuoteListConst.ASKSIZE, QuoteListConst.BID,QuoteListConst.ASK, QuoteListConst.VOL,QuoteListConst.OI, QuoteListConst.CHANGE, QuoteListConst.OICHANGE, QuoteListConst.SETTLEMENT, QuoteListConst.OPEN, QuoteListConst.HIGH, QuoteListConst.LOW, QuoteListConst.LASTSETTLEMENT };
        //设定每列的宽度
        int[] columnWidth = new int[] { 100, 70, 40, 40, 40, 70, 70, 80, 80, 70, 70, 70, 70, 70, 70, 70 };
        //记录每行的起点X值
        int[] columnStartX;
        
        //报价列表总宽
        int totalWidth;
        public int QuoteViewWidth { get { return totalWidth; } set { totalWidth = value; } }

        //获得某个序号列的起点
        internal int GetColumnStarX(int i)
        {
            return columnStartX[i];
        }
        //获得某个序号列的宽度
        internal int GetColumnWidth(int i)
        {
            return columnWidth[i];
        }
        //获得行总宽
        internal int GetRowWidth()
        {
            return totalWidth;
        }
        //开始显示的序号
        internal int GetBeginIndex()
        {
            return _beginIdx;
        }

        //计算行总宽
        void CalcColunmTotalWidth()
        {
            //int w = 0;
            //for (int i = 0; i < columnWidth.Length; i++)
            //{
            //    w += columnWidth[i];
            //}
            totalWidth = columnWidth.Sum();
        }
        //根据列宽重新计算列的起点
        void CalcColunmStartX()
        {
            columnStartX = new int[columnWidth.Length];
            for (int i = 0; i < columnWidth.Length; i++)
            {
                columnStartX[i] = 0;
                //循环计算起点
                for (int j = 0; j < i; j++)
                {
                    columnStartX[i] += columnWidth[j];
                }
            }
        }
        //用于将单元格所缓存的Rect清楚 重新计算 主要用于当列宽差生变化后进行更新,
        //平时更新数据的时候进行这些计算是没有意义 浪费CPU资源
        void ResetAllRect()
        {
            for (int i = 0; i < _count;i++)
            {
                _quoteList[i].ResetRowRect();
            }
                //foreach (QuoteRow qr in _idxQuoteRowMap.Values)
                //{
                //    qr.ResetRowRect();
                //}
        }
        //当列宽发生变化时候,我们需要重新计算更新列的起点 以及 总宽等数据编译QuoteRow cell进行调用
        void columnWidthChanged()
        {
            CalcColunmTotalWidth();
            CalcColunmStartX();
            ResetAllRect();
        }

        /// <summary>
        /// 标题高度
        /// </summary>
        private int HeaderHeight { get { return _headFont.Height + 4; } }
        /// <summary>
        /// 报价行高度
        /// </summary>
        private int RowHeight { get { return _symbolFont.Height + 4; } }

        #endregion



        #region 鼠标修改列宽

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

        int _selectedRow=-1;
        public int SelectedQuoteRow { get { return _selectedRow; } set { _selectedRow = value; } }

        /// <summary>
        /// 当前选中的合约
        /// </summary>
        public MDSymbol SelectedSymbol
        {
            get
            {
                return CurrentSymbol;
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

        /// <summary>
        /// 选中某个行，高亮显示该报价行
        /// </summary>
        /// <param name="i"></param>
        void SelectRow(int i)
        {
            //debug("选择行:"+i.ToString());
            if (i < 0) i =  Count - 1;//如果选择的行小于0 则返回最后一行
            if (i > (Count - 1)) i = 0;//如果选择的行 超过当总行数,则返回到第一行
            int old = SelectedQuoteRow;
            if (i != old)//两次选择的行步一致
            {
                ChangeRowBackColor(i);//高亮被我们选中的那行
                //debug("改回原来的颜色");
                ResetRowBackColor(old);//将原来的行改成原来的样式
                SelectedQuoteRow = i;//保存我们选定的行
            }
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
            if (SymbolSelectedEvent != null)
            {
                SymbolSelectedEvent(symbol);
            }
            //CoreService.EventUI.FireSymbolSelectedEvent(this, symbol);
            debug("Symbol:" + symbol.ToString() + " Selected");
        }


        //获得某个行的security
        MDSymbol GetVisibleSecurity(int row)
        {
            if ((row < 0) || (row >= Count)) return new MDSymbol();
            return this[row].Symbol;
        }

        MDSymbol CurrentSymbol
        { 
            get
            {
                if (_selectedRow > 0 && _selectedRow < this.Count)
                {
                    return this[_selectedRow].Symbol;
                }
                if (this.Count > 0)
                {
                    return this[0].Symbol;
                }
                return null;
            }
        
        }

        //将某行颜色重置
        void ResetRowBackColor(int rid)
        {
            if (rid >= 0)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    Color c = rid % 2 == 0 ? QuoteBackColor1 : QuoteBackColor2;
                    this[rid][i].CellStyle.BackColor = c;
                    this[rid][i].CellStyle.LineColor = TableLineColor;
                }
                Invalidate(this[rid].Rect);
            }
        }

        //更改某行颜色
        void ChangeRowBackColor(int rid)
        {
            //if (rid == SelectedQuoteRow) return;
            for (int i = 0; i < columns.Length; i++)
            {
                this[rid][i].CellStyle.BackColor = this.SelectedColor;
                this[rid][i].CellStyle.LineColor = this.SelectedColor;
            }
            Invalidate(this[rid].Rect);
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

        /// <summary>
        /// 拖拽停止后改变列宽
        /// </summary>
        private void ChangeColWidth()
        {
            try
            {
                //debug("改变列宽");
                columnWidth[CurrentMoveYLIneID - 1] = columnWidth[CurrentMoveYLIneID - 1] + CurrentYLineMoveWidth;
                columnWidthChanged();
                this.Refresh();
            }
            catch (Exception ex)
            {
                debug("changecolwidth error");
            }
        }
        private bool MouseIsInTableArea(MouseEventArgs e)
        {
            //return true;
            return e.X >= 20 && e.X <= (totalWidth + 20) && e.Y >= 50 && e.Y <= (DefaultQuoteStyle.HeaderHeight + 10);
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
            if (e.Y > 0 && e.Y < this.HeaderHeight)
            {
                for (int i = 0; i < columnStartX.Length; i++)
                {
                    if (e.X > columnStartX[i] - 3 && e.X < columnStartX[i] + 3)
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
            
            CurrentYLineMoveWidth = (e.X - columnStartX[CurrentMoveYLIneID]);//计算移动值
            ChangeColWidth();//计算新的列宽
            columnWidthChanged();//重新计算绘制表格需要的列宽 列起点 总宽数据
            this.Refresh();//刷新
        }
        private int _mouseX;
        private int _mouseY;

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

        #endregion

        #region 右键菜单

        ContextMenuStrip _cmenu;
        private void initMenu()
        {
            _cmenu =new ContextMenuStrip();
            _cmenu.Items.Add("K线图", null, new EventHandler(menuOpenKChart));
            _cmenu.Items.Add("分时数据", null, new EventHandler(rightTimeSales));
            _cmenu.Items.Add("下单板", null, new EventHandler(rightticket));

        }

        void rightTimeSales(object sender, EventArgs e)
        {
            openTimeSales();
        }
        void openTimeSales()
        {
            MDSymbol sec = GetVisibleSecurity(SelectedQuoteRow);
            //if (!sec.isValid)
            //    return;
            //if (SendOpenTimeSalesEvent != null)
            //    SendOpenTimeSalesEvent(sec);
        }
        void menuOpenKChart(object sender, EventArgs e)
        {
            MDSymbol symbol = GetVisibleSecurity(_selectedRow);
            //if (!sec.isValid)
            //    return;
            //if (OpenKChartEvent != null)
            //    OpenKChartEvent(symbol);
        }


        //简易下单
        void rightticket(object sender, EventArgs e)
        {
            openTicket();
        }
        void openTicket()
        {
            /*
            Security s = GetVisibleSecurity(SelectedQuoteRow);
            if (s.Type == SecurityType.IDX) return;
            string sym = s.Symbol;
            if ((s.FullName == string.Empty) || (sym == string.Empty))
            {
                return;
            }
            Order o = new OrderImpl(s.Symbol, 0);
            o.ex = s.DestEx;
            o.Security = s.Type;
            o.LocalSymbol = sym;
            //简易下单面板
            Ticket t = new Ticket(o);
            //考虑用什么方式去调用对应的函数
            t.SendOrder += new OrderDelegate(SendOrder);
            spillTick += new TickDelegate(t.newTick);
            //orderStatus += new OrderStatusDel(t.orderStatus);

            System.Drawing.Point p = new System.Drawing.Point(MousePosition.X, MousePosition.Y);
            p.Offset(-200,5);
            t.SetDesktopLocation(p.X, p.Y);
            t.Show();**/
        }
        void SendOrder(Order o)
        {
            if (SendOrderEvent != null)
                SendOrderEvent(o);
        }


        #endregion

        #region 键盘响应

       
        /// <summary>
        /// 上移行
        /// </summary>
        public void RowUp()
        {
            int rid = SelectedQuoteRow -1;
            SelectRow(rid);
            if (rid > _endIdx || rid < _beginIdx)
            {
                UpdateBeginEndIdx();
                Invalidate();
            }
        }
        /// <summary>
        /// 下移行
        /// </summary>
        public void RowDown()
        {
            int rid = SelectedQuoteRow + 1;
            SelectRow(rid);//选择某行带有更新该行显示的语句
            logger.Info("begin:{0} end:{1}".Put(_beginIdx, _endIdx));
            if (rid > _endIdx || rid < _beginIdx)//当选择的行超出我们显示的start end区间之后我们需要重新计算对应的起始
            {
                UpdateBeginEndIdx();
                Invalidate();
            }
        }
        void ViewQuoteList_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Up)
            //{
            //    RowUpside();
            //}
            //if (e.KeyCode == Keys.Down)
            //{
            //    RowDownside();
            //}
            
        }

        #endregion

    }
}