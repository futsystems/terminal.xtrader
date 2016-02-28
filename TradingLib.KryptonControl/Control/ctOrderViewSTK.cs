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

            CoreService.EventCore.RegIEventHandler(this);
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);
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
                        tb.Rows[i][SYMBOLNAME] = o.oSymbol.GetName();
                        tb.Rows[i][SIZE] = Math.Abs(o.TotalSize);
                        tb.Rows[i][FILLED] = o.FilledSize;

                        tb.Rows[i][PRICE] = o.oSymbol.FormatPrice(o.LimitPrice);
                        tb.Rows[i][FILLAVGPRICE] = 0;
                        tb.Rows[i][OPERATION] = o.Side ? "买入" : "   卖出";
                        tb.Rows[i][DATETIME] = Util.ToDateTime(o.Date, o.Time).ToString("HH:mm:ss");
                        tb.Rows[i][COMMENT] = o.Comment;


                        tb.Rows[i][DIRECTION] = o.Side ? "1" : "-1";
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
            tb.Columns.Add(ID);
            tb.Columns.Add(OrderSeq);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(FILLED);
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

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = orderGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
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
        }

    }
}
