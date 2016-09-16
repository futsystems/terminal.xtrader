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
            InitQuoteColumns();

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
            //logger.Info("Size changed,totlal width:" + totalWidth.ToString());
            UpdateBeginEndIdx();
            //CalcColunmTotalWidth();
            //CalcColunmStartX();
            this.ResetRect();
            //logger.Info("2Size changed,totlal width:" + totalWidth.ToString());
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

        internal List<QuoteColumn> Columns { get { return quoteColumns; } }

        List<QuoteColumn> quoteColumns = new List<QuoteColumn>();
        void InitQuoteColumns()
        {
            foreach (int code in Enum.GetValues(typeof(EnumFileldType)))
            {
                quoteColumns.Add(new QuoteColumn((EnumFileldType)code));
                
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
            for (int i = 0; i < quoteColumns.Count; i++)
            {
                quoteColumns[i].StartX = 0;
                //循环计算起点
                for (int j = 0; j < i; j++)
                {
                    quoteColumns[i].StartX += quoteColumns[j].Width;
                }
            }
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
        //当列宽发生变化时候,我们需要重新计算更新列的起点 以及 总宽等数据编译QuoteRow cell进行调用
        void columnWidthChanged()
        {
            //CalcColunmTotalWidth();
            CalcColunmStartX();
            this.ResetRect();
        }



        #endregion



        #region 鼠标修改列宽

        

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
            QuoteRow row = this[rid];
            if (row != null)
            {
                row.Selected = false;
                Invalidate(row.Rect);
            }
        }

        //更改某行颜色
        void ChangeRowBackColor(int rid)
        {
             QuoteRow row = this[rid];
             if (row != null)
             {
                 row.Selected = true;
                 Invalidate(row.Rect);
             }
        }

       

        /// <summary>
        /// 拖拽停止后改变列宽
        /// </summary>
        private void ChangeColWidth()
        {
            try
            {
                //debug("改变列宽");
                quoteColumns[CurrentMoveYLIneID - 1].Width = quoteColumns[CurrentMoveYLIneID - 1].Width + CurrentYLineMoveWidth;
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
            return e.X >= 20 && e.X <= (this.Width + 20) && e.Y >= 50 && e.Y <= (DefaultQuoteStyle.HeaderHeight + 10);
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

        #endregion

    }
}