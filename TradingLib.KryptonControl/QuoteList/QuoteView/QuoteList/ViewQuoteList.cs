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

    public class QuoteViewChangedArgs:EventArgs
    {
        public QuoteViewChangedArgs()
        {
            this.Count = 0;
            this.MaxShowCount = 0;
        }
        /// <summary>
        /// 数据集数量
        /// </summary>
        public int Count{get;set;}

        /// <summary>
        /// 最大可显示数量
        /// </summary>
        public int MaxShowCount{get;set;}
    }

    public enum QuoteMouseEventType
    { 
        /// <summary>
        /// 合约双击 用于进入KChart图标
        /// </summary>
        SymbolDoubleClick,
    }

    

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

        public event EventHandler<QuoteViewChangedArgs> QuoteViewChanged;

        public event EventHandler<SymbolVisibleChangeEventArgs> SymbolVisibleChanged;

        /// <summary>
        /// 聚合鼠标事件
        /// </summary>
        public event Action<MDSymbol, QuoteMouseEventType> MouseEvent;

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

            InitQuoteColumns();

            InitConfig();

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

            //this.SizeChanged +=new EventHandler(ViewQuoteList_SizeChanged);

            this.GotFocus += new EventHandler(ViewQuoteList_GotFocus);
            this.LostFocus += new EventHandler(ViewQuoteList_LostFocus);
            
            //this.PreviewKeyDown += new PreviewKeyDownEventHandler(ViewQuoteList_PreviewKeyDown);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyDown);
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ViewQuoteList_KeyPress);
            //this.KeyUp += new System.Windows.Forms.KeyEventHandler(ViewQuoteList_KeyUp);

            this.SizeChanged += new EventHandler(ViewQuoteList_SizeChanged);

            ApplyConfig(EnumQuoteListType.STOCK_CN);
            //初始化右键菜单
            if(MenuEnable)
                initMenu();
            //计算列起点 总宽等参数
            CalcColunmStartX();
            this.ResetRect();

            
        }

        public IEnumerable<Symbol> SymbolsShow
        {
            get
            {
                return null;
            }
        }

        
        void FireQuoteViewChange()
        {
            if (QuoteViewChanged != null)
            {
                QuoteViewChangedArgs arg = new QuoteViewChangedArgs();
                arg.Count = _count;
                arg.MaxShowCount = VisibleRowCount;
                QuoteViewChanged(this, arg);
            }
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
            logger.Info("Size Changed");
            UpdateBeginEndIdx();
            ResetRect();
            Refresh();
            logger.Info("selected row:" + _selectedRow.ToString());
        }

       
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
                        this[r][EnumFileldType.LAST].CellStyle.BackColor = QuoteBackColor1;
                        Invalidate(this[r].Rect);
                    }
                    else
                    {
                        this[r][EnumFileldType.LAST].CellStyle.BackColor = QuoteBackColor1;
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

        private Pen _pen = new Pen(Color.Green, 1);
        int _count = 0;
        List<QuoteRow> _quoteList = new List<QuoteRow>();
        //记录symbol代码与对应Row序号的映射
        Dictionary<string, int> _symbolIdxMap = new Dictionary<string, int>();
        
        //symbol到本地idx的转换
        int symbol2idx(string symbolkey)
        {
            int idx;
            if (_symbolIdxMap.TryGetValue(symbolkey, out idx))
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
        //public QuoteRow this[string symbol] { get { return this[symbol2idx(symbol)]; } }

        
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
            FireQuoteViewChange();
            Invalidate();
        }

 


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

        /// <summary>
        /// 更新某个合约数据
        /// </summary>
        /// <param name="symbol"></param>
        public void Update(MDSymbol symbol)
        {

            int idx = symbol2idx(symbol.UniqueKey);
            QuoteRow row = this[idx];
            if (row != null)
            {
                row.Update();
            }

        }

        #region 表宽,表名

        /// <summary>
        /// 所有列
        /// </summary>
        internal List<QuoteColumn> Columns { get { return totalColumns; } }
        List<QuoteColumn> totalColumns = new List<QuoteColumn>();

        /// <summary>
        /// 所有可视列
        /// </summary>
        internal List<QuoteColumn> VisibleColumns { get { return visibleColumns; } }
        List<QuoteColumn> visibleColumns = new List<QuoteColumn>();

        void InitQuoteColumns()
        {
            foreach (int code in Enum.GetValues(typeof(EnumFileldType)))
            {
                totalColumns.Add(new QuoteColumn((EnumFileldType)code));
                
            }
        }


        //开始显示的序号
        internal int GetBeginIndex()
        {
            return _beginIdx;
            
        }

     
        /// <summary>
        /// 根据Column宽度 计算每个Column的StartX
        /// </summary>
        void CalcColunmStartX()
        {
            //QuoteColumn[] columns = quoteColumns.Where(c => c.Visible).OrderBy(c => c.Index).ToArray();//获得需要显示的列并按index排列
            for(int i=0;i<visibleColumns.Count;i++)
            {
                visibleColumns[i].StartX = 0;
                //循环计算起点
                for (int j = 0; j < i; j++)
                {
                    visibleColumns[i].StartX += visibleColumns[j].Width;
                }
            }

            //for (int i = 0; i < columns.Count(); i++)
            //{
            //    quoteColumns[i].StartX = 0;
            //    //循环计算起点
            //    for (int j = 0; j < i; j++)
            //    {
            //        quoteColumns[i].StartX += quoteColumns[j].Width;
            //    }
            //}
        }

        //用于将单元格所缓存的Rect清楚 重新计算 主要用于当列宽差生变化后进行更新,
        //平时更新数据的时候进行这些计算是没有意义 浪费CPU资源
        /// <summary>
        /// 重置绘图区域缓存
        /// 将缓存的Rect清除，需要区域数据时进行重新计算 比如列宽变化,控件大小发生变化等
        /// 当尺寸未变时 使用缓存数据 避免不需要的计算
        /// </summary>
        void ResetRect()
        {
            foreach (var row in _quoteList)
            {
                row.ResetRect();
            }
        }

        #endregion

        int _selectedRow=-1;
        public int SelectedQuoteRow 
        { 
            get { return _selectedRow; }
            //set
            //{
            //    _selectedRow = value;

            //    SelectRow(_selectedRow);

            //    if (_selectedRow > _endIdx || _selectedRow < _beginIdx)
            //    {
            //        UpdateBeginEndIdx();
            //        Invalidate();
            //    }
            //} 
        
        }

        /// <summary>
        /// 当前选中合约
        /// </summary>
        public MDSymbol SymbolSelected
        {
            get
            {
                if (_selectedRow < 0 || _selectedRow >= _count) return null;
                return this[_selectedRow].Symbol;
            }
        }

        /// <summary>
        /// 所有可见合约列表
        /// </summary>
        public List<MDSymbol> SymbolVisible
        {
            get
            {
                List<MDSymbol> list = new List<MDSymbol>();
                for (int i = _beginIdx; i <= _endIdx; i++)
                {
                    list.Add(this[i].Symbol);
                }
                return list;
            }
        }

        /// <summary>
        /// 选中某个行，高亮显示该报价行
        /// </summary>
        /// <param name="i"></param>
        void SelectRow(int i)
        {
            if (i < 0) i =  _count - 1;//如果选择的行小于0 则返回最后一行
            if (i > (_count - 1)) i = 0;//如果选择的行 超过当总行数,则返回到第一行
            int old = _selectedRow;
            if (i != old)
            {
                QuoteRow row = null;
                row = this[i];
                if (row != null)
                {
                    row.Selected = true;
                    Invalidate(row.Rect);
                }
                row = this[old];
                if (row != null)
                {
                    Invalidate(row.Rect);
                    row.Selected = false;
                }
                _selectedRow = i;

            }
        }

        


        //获得某个行的security
        MDSymbol GetVisibleSecurity(int row)
        {
            if ((row < 0) || (row >= Count)) return new MDSymbol();
            return this[row].Symbol;
        }

        public MDSymbol SelectedSymbol
        { 
            get
            {
                if (_selectedRow > 0 && _selectedRow < this.Count)
                {
                    return this[_selectedRow].Symbol;
                }
                //_selected 不在选择范围内 返回第一个值
                if (this.Count > 0)
                {
                    return this[0].Symbol;
                }
                return null;
            }
        
        }

        private bool MouseIsInTableArea(MouseEventArgs e)
        {
            //return true;
            return e.X >= 20 && e.X <= (this.Width + 20) && e.Y >= 50 && e.Y <= (DefaultQuoteStyle.HeaderHeight + 10);
        }

        
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


        /// <summary>
        /// 上移行
        /// </summary>
        public void RowUp()
        {
            int rid = SelectedQuoteRow -1;
            SelectRow(rid);
            //当选择的行超出我们显示的start end区间之后我们需要重新计算对应的起始
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
            SelectRow(rid);
            //当选择的行超出我们显示的start end区间之后我们需要重新计算对应的起始
            if (rid > _endIdx || rid < _beginIdx)
            {
                UpdateBeginEndIdx();
                Invalidate();
            }
        }
    }
}