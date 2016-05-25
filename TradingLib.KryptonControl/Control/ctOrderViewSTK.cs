using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;

using ComponentFactory.Krypton.Toolkit;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;



namespace TradingLib.KryptonControl
{
    public partial class ctOrderViewSTK : UserControl, IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctOrderViewSTK");
        ConcurrentDictionary<long, int> orderRowIdxMap = new ConcurrentDictionary<long, int>();

        public ctOrderViewSTK()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();
            //控件创建时_realview为默认True 这里还没有设置成自定义参数
            //logger.Info("Control created, realview:" + _realview.ToString());
            ////显示实时委托 需要在核心初始化完毕后从TradingInfoTracker中加载所有委托 同时相应委托实时数据
            //if (_realview)
            //{
            //    CoreService.EventCore.RegIEventHandler(this);
            //    CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);
            //}
            this.orderGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(orderGrid_CellFormatting);
            this.orderGrid.SizeChanged += new EventHandler(orderGrid_SizeChanged);
            this.Load += new EventHandler(ctOrderViewSTK_Load);
        }

        void orderGrid_SizeChanged(object sender, EventArgs e)
        {
            this.ResetColumeSize();
        }

        void orderGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool side = false;
                    bool.TryParse(orderGrid[11, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? UIConstant.LongSideColor : UIConstant.ShortSideColor;
                }


            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctOrderViewSTK_Load(object sender, EventArgs e)
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
        }



        public void OnDisposed()
        {

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


        string GetSymbolName(Order o)
        {
            if (o.oSymbol != null) return o.oSymbol.GetName();
            return "";
        }

        string FormatPrice(Order o, decimal val)
        {
            if (o.oSymbol != null) val.ToFormatStr(o.oSymbol);
            return val.ToFormatStr();
        }

        const string _realDT = "HH:mm:ss";
        const string _histDT = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 获得委托时间
        /// 显示日内委托只显示时间 历史委托需要显示日期
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string GetDateTime(Order o)
        {
            return Util.ToDateTime(o.Date, o.Time).ToString(_realview ? _realDT : _histDT);
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
                        tb.Rows[i][OrderSeq] = o.OrderSeq.ToString();

                        
                        tb.Rows[i][SYMBOL] = o.Symbol;
                        tb.Rows[i][SYMBOLNAME] = GetSymbolName(o);
                        tb.Rows[i][SIZE] = Math.Abs(o.TotalSize);
                        tb.Rows[i][FILLED] = o.FilledSize;

                        tb.Rows[i][PRICE] = FormatPrice(o, o.LimitPrice);
                        tb.Rows[i][FILLAVGPRICE] = 0;
                        tb.Rows[i][OPERATION] = o.Side ? "买入" : "   卖出";
                        tb.Rows[i][DATETIME] = GetDateTime(o);
                        tb.Rows[i][COMMENT] = o.Comment;


                        tb.Rows[i][DIRECTION] = o.Side;
                        tb.Rows[i][OFFSETFLAG] = o.IsEntryPosition ? "开仓" : "平仓";
                        tb.Rows[i][STATUS] = o.Status;
                        tb.Rows[i][STATUSSTR] = Util.GetEnumDescription(o.Status);
                        
                    }
                    else
                    {
                        tb.Rows[i][FILLED] = o.FilledSize;
                        tb.Rows[i][FILLAVGPRICE] = 0;

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



        const string ID = "全局编号";
        const string OrderSeq = "编号";

        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";
        const string SIZE = "委托数量";
        const string FILLED = "成交数量";
        const string PRICE = "委托价格";
        const string FILLAVGPRICE = "成交均价";
        const string OPERATION = "操作";
        const string DATETIME = "时间";
        const string COMMENT = "备注";

        const string DIRECTION = "方向";
        const string OFFSETFLAG = "开平";
        const string STATUS = "挂单状态";
        const string STATUSSTR = "状态";
        

        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            KryptonDataGridView grid = orderGrid;

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 25;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            grid.Margin = new Padding(0);
        }

        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(ID);//0
            tb.Columns.Add(OrderSeq);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(FILLED);//5
            tb.Columns.Add(PRICE);
            tb.Columns.Add(FILLAVGPRICE);
            tb.Columns.Add(OPERATION);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(COMMENT);

            tb.Columns.Add(DIRECTION);
            tb.Columns.Add(OFFSETFLAG);
            tb.Columns.Add(STATUS);
            tb.Columns.Add(STATUSSTR);
            
        }

        void ResetColumeSize()
        {
            ComponentFactory.Krypton.Toolkit.KryptonDataGridView grid = orderGrid;
            grid.Columns[SYMBOL].Width = 100;
            grid.Columns[SYMBOLNAME].Width = 100;
            grid.Columns[SIZE].Width = 80;
            grid.Columns[FILLED].Width = 80;
            grid.Columns[PRICE].Width = 100;
            grid.Columns[PRICE].Width = 100;
            grid.Columns[FILLAVGPRICE].Width = 100;
            grid.Columns[OPERATION].Width = 80;
            grid.Columns[DATETIME].Width = _realview ? 80 : 160;
            
        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = orderGrid;
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[ID].Visible = false;
            grid.Columns[OrderSeq].Visible = false;

            grid.Columns[DIRECTION].Visible = false;
            grid.Columns[OFFSETFLAG].Visible = false;
            grid.Columns[STATUS].Visible = false;
            grid.Columns[STATUSSTR].Visible = false;


            //set width
            //grid.Columns[SYMBOL].Width = 80;
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

    }
}
