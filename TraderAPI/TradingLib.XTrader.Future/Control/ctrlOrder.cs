using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.XTrader.Future
{
    public partial class ctrlOrder : UserControl,TradingLib.API.IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctrlOrder");
        FGrid orderGrid = null;
        public ctrlOrder()
        {
            InitializeComponent();

            this.orderGrid = new FGrid();
            this.orderGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(orderGrid);


            InitTable();
            BindToTable();

            WireEvent();

            
        }

        

        void WireEvent()
        {
            this.SizeChanged += new EventHandler(ctrlOrder_SizeChanged);
            this.Load += new EventHandler(ctrlOrder_Load);

            tabAll.Click += new EventHandler(tabAll_Click);
            tabCancel.Click += new EventHandler(tabCancel_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnCancelAll.Click += new EventHandler(btnCancelAll_Click);

            orderGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(orderGrid_CellFormatting);
            orderGrid.MouseClick += new MouseEventHandler(orderGrid_MouseClick);
        }


        void orderGrid_MouseClick(object sender, MouseEventArgs e)
        {
            int rowid = GetRowIndexAt(e.Y);
            if (rowid == -1)
            {
                if (orderGrid.SelectedRows.Count > 0)
                {
                    orderGrid.SetSelectedBackground(false);
                }
            }
            else
            {
                orderGrid.SetSelectedBackground(true);
            }
        }
        int GetRowIndexAt(int mouseLocation_Y)
        {
            if (orderGrid.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;  // no rows.   
            }
            if (orderGrid.ColumnHeadersVisible == true && mouseLocation_Y <= orderGrid.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = orderGrid.FirstDisplayedScrollingRowIndex;
            int displayedCount = orderGrid.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount; )  // 因为行不能ReOrder，故只需要搜索显示的行   
            {
                if (orderGrid.Rows[index].Visible == true)
                {
                    Rectangle rect = orderGrid.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    k++;  // 只计数显示的行;   
                }
                index++;
            }
            return -1;
        }  


        void orderGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool side = false;
                    bool.TryParse(orderGrid[4, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? Constants.BuyColor : Constants.SellColor;
                }
                if (e.ColumnIndex == 6)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool open = orderGrid[6, e.RowIndex].Value.ToString() == "开";

                    e.CellStyle.ForeColor = open ? Color.Black : Color.Blue;
                }


            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }
        }

        void btnCancelAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认撤销所有委托？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Order o in CoreService.TradingInfoTracker.OrderTracker)
                {
                    if (o.IsPending())
                    {
                        System.Threading.Thread.Sleep(5);
                        CoreService.TLClient.ReqCancelOrder(o.id);
                    }
                }
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            long oid = SelectedOrderID;
            if (oid == -1)
            {
                MessageBox.Show("请选择需要撤单的委托", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Order o = CoreService.TradingInfoTracker.OrderTracker.SentOrder(oid);
                if (o.IsPending())
                {
                    CoreService.TLClient.ReqCancelOrder(oid);
                }
                else
                {
                    MessageBox.Show("该委托无法撤销", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        void tabCancel_Click(object sender, EventArgs e)
        {
            tabCancel.Checked = true;
            tabAll.Checked = false;
            string strFilter = DATETIME + " DESC";
            strFilter = String.Format(STATUS + " = '{0}' or " + STATUS + " = '{1}' or " + STATUS + " = '{2}'", "Placed", "Opened", "PartFilled");
            datasource.Filter = strFilter;
        }

        void tabAll_Click(object sender, EventArgs e)
        {
            tabCancel.Checked = false;
            tabAll.Checked = true;
            datasource.Filter = "";
        }
        void ctrlOrder_SizeChanged(object sender, EventArgs e)
        {

            this.tabAll.Height = (this.Height-2) / 2;
            this.tabCancel.Location = new Point(0, this.tabAll.Height+2);
            this.tabCancel.Height = this.Height - this.tabAll.Height - 2;
        }



        void ctrlOrder_Load(object sender, EventArgs e)
        {
            //logger.Info("Control load,realivew:" + _realview.ToString());
            //如果需要根据控件的参数设置动态的调整控件内部事件的注册 则在控件第一次显示时进行处理,此时控件的参数已经赋值完毕
            if (_realview)
            {
                CoreService.EventCore.RegIEventHandler(this);
                CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);

                CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            }
            if (orderGrid.SelectedRows.Count > 0)
            {
                orderGrid.SelectedRows[0].Selected = false;
            }
        }


        void EventOther_OnResumeDataEnd()
        {
            //加载初始化数据中的委托
            foreach (var order in CoreService.TradingInfoTracker.OrderTracker)
            {
                this.GotOrder(order);
            }
        }

        void EventOther_OnResumeDataStart()
        {
            this.Clear();
        }



        public void OnInit()
        {
            //加载初始化数据中的委托
            foreach (var order in CoreService.TradingInfoTracker.OrderTracker)
            {
                this.GotOrder(order);
            }

            if (orderGrid.SelectedRows.Count > 0)
            {
                orderGrid.SelectedRows[0].Selected = false;
            }
        }

        public void OnDisposed()
        { 
        
        }


        /// <summary>
        /// 当前选中委托ID
        /// </summary>
        long SelectedOrderID
        {
            get
            {
                if (orderGrid.SelectedRows.Count > 0)
                {
                    return long.Parse(orderGrid.SelectedRows[0].Cells[ID].Value.ToString());
                }
                else
                {
                    return -1;
                }

            }
        }


        [DefaultValue(true)]
        bool _realview = true;
        /// <summary>
        /// 控件是否在实时状态工作
        /// 实时状态显示实时交易回报
        /// 查询状态显示查询回报
        /// 该参数用于编制控件时在属性内进行调整，程序运行期间调整无效
        /// </summary>
        public bool RealView
        {
            get { return _realview; }
            set
            {
                _realview = value;
            }
        }

        ConcurrentDictionary<long, int> orderRowIdxMap = new ConcurrentDictionary<long, int>();
        /// <summary>
        /// 通过委托全局ID获得对应行编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int OrderID2RowIdx(long id)
        {
            int rowid = -1;
            if (orderRowIdxMap.TryGetValue(id, out rowid))
            {
                return rowid;
            }
            return -1;
        }

        const string _realDT = "HH:mm:ss";
        const string _histDT = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 获得委托时间
        /// 显示日内委托只显示时间 历史委托需要显示日期
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string GetDateTimeString(Order o)
        {
            return Util.ToDateTime(o.Date, o.Time).ToString(_realview ? _realDT : _histDT);
        }

        string FormatPrice(Order o, decimal val)
        {
            if (o.oSymbol != null) val.ToFormatStr(o.oSymbol);
            return val.ToFormatStr();
        }
        string GetSymbolName(Order o)
        {
            if (o.oSymbol != null) return o.oSymbol.GetName();
            return o.Symbol;
        }

        public void GotOrder(Order o)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Order>(GotOrder), new object[] { o });
            }
            else
            {
                try
                {
                    int i = OrderID2RowIdx(o.id);
                    if (i == -1)
                    {
                        DataRow r = tb.Rows.Add(o.id);
                        i = tb.Rows.Count - 1;//得到新建的Row号
                        orderRowIdxMap.TryAdd(o.id, i);

                        tb.Rows[i][ID] = o.id;
                        tb.Rows[i][DATETIME] = o.GetDateTime();
                        tb.Rows[i][TIME] = GetDateTimeString(o);
                        tb.Rows[i][SYMBOL] = o.Symbol;
                        tb.Rows[i][SIDE] = o.Side;
                        tb.Rows[i][SIDESTR] = o.Side ? "买" : "  卖";
                        tb.Rows[i][FLAG] = o.IsEntryPosition ? "开" : " 平";
                        tb.Rows[i][ORDERPRICE] = FormatPrice(o, o.LimitPrice);//o.LimitPrice.ToFormatStr(o.oSymbol.SecurityFamily.GetPriceFormat());


                        tb.Rows[i][TOTALSIZE] = Math.Abs(o.TotalSize);
                        tb.Rows[i][FILLSIZE] = o.FilledSize;


                        tb.Rows[i][STATUS] = o.Status;
                        tb.Rows[i][STATUSSTR] = Util.GetEnumDescription(o.Status);
                        tb.Rows[i][COMMENT] = o.Comment;
                        tb.Rows[i][SPECULATE] = "投";
                        tb.Rows[i][ORDERID] = o.OrderSysID;
                        tb.Rows[i][NAME] = GetSymbolName(o);

                    }
                    else
                    {
                        tb.Rows[i][FILLSIZE] = o.FilledSize;
                        tb.Rows[i][STATUS] = o.Status;
                        tb.Rows[i][STATUSSTR] = Util.GetEnumDescription(o.Status);
                        tb.Rows[i][COMMENT] = o.Comment;

                    }
                }
                catch (Exception ex)
                {
                    logger.Error("GotOrder Error:" + ex.ToString());
                }
            }
        }

        #region 表格
        const string ID = "委托键";
        const string DATETIME = "DATETIME";
        const string TIME = "委托时间";
        const string SYMBOL = "合约";
        const string SIDE = "SIDE";
        const string SIDESTR = "买卖";
        const string FLAG = "开平";

        const string ORDERPRICE = "委托价格";
        const string TOTALSIZE = "委手";
        const string FILLSIZE = "成手";
        const string STATUS = "STATUS";
        const string STATUSSTR = "状态";
        const string COMMENT = "备注";
        const string SPECULATE = "投保";
        const string ORDERID = "委托号";
        const string NAME = "名称";

        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(ID);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(TIME);
            tb.Columns.Add(SYMBOL);

            tb.Columns.Add(SIDE);
            tb.Columns.Add(SIDESTR);
            tb.Columns.Add(FLAG);
            tb.Columns.Add(ORDERPRICE);
            tb.Columns.Add(TOTALSIZE);
            tb.Columns.Add(FILLSIZE);

            tb.Columns.Add(STATUS);
            tb.Columns.Add(STATUSSTR);
            tb.Columns.Add(COMMENT);
            tb.Columns.Add(SPECULATE);
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(NAME);


        }

        void ResetColumeSize()
        {
            DataGridView grid = orderGrid;

            grid.Columns[TIME].Width = 100;
            grid.Columns[SYMBOL].Width = 120;
            grid.Columns[SIDE].Width = 40;
            grid.Columns[FLAG].Width = 40;
            grid.Columns[ORDERPRICE].Width = 75;

            grid.Columns[TOTALSIZE].Width = 47;
            grid.Columns[FILLSIZE].Width = 47;
            grid.Columns[STATUSSTR].Width = 100;
            grid.Columns[COMMENT].Width = 145;
            grid.Columns[SPECULATE].Width = 40;
            grid.Columns[ORDERID].Width = 80;
            grid.Columns[NAME].Width = 120;




        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid = orderGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[ID].Visible = false;
            grid.Columns[SIDE].Visible = false;
            grid.Columns[DATETIME].Visible = false;
            grid.Columns[STATUS].Visible = false;
            
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ResetColumeSize();
        }

        /// <summary>
        /// 清空表格内容
        /// </summary>
        public void Clear()
        {
            orderGrid.DataSource = null;
            orderRowIdxMap.Clear();
            tb.Rows.Clear();
            BindToTable();
        }

        #endregion


    }
}
