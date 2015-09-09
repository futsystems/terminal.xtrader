using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI; 
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;
using System.Threading;


namespace TradingLib.TraderControl
{
    public partial class ctOrderView : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctOrderView");

        public ctOrderView()
        {
            InitializeComponent();

            SetPreferences();
            InitTable();
            BindToTable();

            CoreService.EventCore.RegIEventHandler(this);
            WireEvent();

            this.Load +=new EventHandler(ctOrderView_Load);

        }

        void WireEvent()
        {
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);
        }
        bool load = false;
        private void ctOrderView_Load(object sender, EventArgs e)
        {
            if (!load)
            {
                load = true;
                
            }
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

        

        const string ID = "全局编号";
        const string OrderSeq = "编号";
        const string DATETIME = "时间";
        const string SYMBOL = "合约";
        const string DIRECTION = "方向";
        const string OPERATION = "买卖";
        const string OFFSETFLAG = "开平";
        const string SIZE = "报手";
        const string PRICE = "价格";
        const string FILLED = "成手";
        const string STATUS = "挂单状态";
        const string STATUSSTR = "状态";
        const string COMMENT = "备注";
        //const string ACCOUNT = "账户";

        DataTable tb = new DataTable();
        ConcurrentDictionary<long, int> orderidxmap = new ConcurrentDictionary<long, int>();
        int OrderID2Idx(long id)
        {
            int idx = -1;
            if (orderidxmap.TryGetValue(id, out idx))
            {
                return idx;
            }
            return -1;
        }
        string GetOrderPrice(Order o)
        {
            if (o.isMarket)
            {
                return "市价";
            }
            if(o.isLimit)
            {
                return "限价:" + o.LimitPrice.ToString();
            }
            if (o.isStop)
            {
                return "Stop:" + o.StopPrice.ToString();
            }
            return "未知";
        }
        public void GotOrder(Order o)
        {
            if (InvokeRequired)
                Invoke(new OrderDelegate(GotOrder), new object[] { o });
            else
            {
                try
                {
                    int i = OrderID2Idx(o.id);
                    if (i == -1)
                    {
                        DataRow r = tb.Rows.Add(o.id);
                        i = tb.Rows.Count - 1;//得到新建的Row号
                        orderidxmap.TryAdd(o.id, i);

                        tb.Rows[i][ID] = o.id;
                        tb.Rows[i][OrderSeq] = o.OrderSeq.ToString();
                        tb.Rows[i][DATETIME] = Util.ToDateTime(o.Date, o.Time).ToString("HH:mm:ss");
                        tb.Rows[i][SYMBOL] = o.Symbol;
                        tb.Rows[i][DIRECTION] = o.Side ? "1" : "-1";
                        tb.Rows[i][OPERATION] = o.Side ? "买入" : "   卖出";
                        tb.Rows[i][OFFSETFLAG] = o.IsEntryPosition ? "开仓" : "平仓";
                        tb.Rows[i][SIZE] = Math.Abs(o.TotalSize);
                        tb.Rows[i][PRICE] = GetOrderPrice(o);
                        tb.Rows[i][FILLED] = o.FilledSize;
                        tb.Rows[i][STATUS] = o.Status;
                        tb.Rows[i][STATUSSTR] = Util.GetEnumDescription(o.Status);
                        tb.Rows[i][COMMENT] = o.Comment;
                    }
                    else
                    {
                        tb.Rows[i][FILLED] = o.FilledSize;
                        tb.Rows[i][STATUS] = o.Status;
                        tb.Rows[i][STATUSSTR] = Util.GetEnumDescription(o.Status);
                        tb.Rows[i][COMMENT] = o.Comment;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("OrderView got order error:" + ex.ToString());
                }
            }
        }


        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            Telerik.WinControls.UI.RadGridView grid = orderGrid;
            grid.ShowRowHeaderColumn = false;//显示每行的头部
            grid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;//列的填充方式
            grid.ShowGroupPanel = false;//是否显示顶部的panel用于组合排序
            grid.MasterTemplate.EnableGrouping = false;//是否允许分组
            grid.EnableHotTracking = true;

            grid.AllowAddNewRow = false;//不允许增加新行
            grid.AllowDeleteRow = false;//不允许删除行
            grid.AllowEditRow = false;//不允许编辑行
            grid.AllowRowResize = false;
            grid.EnableSorting = false;
            grid.TableElement.TableHeaderHeight = UIConstant.HeaderHeight;
            grid.TableElement.RowHeight = UIConstant.RowHeight;
            
            grid.EnableAlternatingRowColor = true;//隔行不同颜色
            //this.radRadioDataReader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On; 
        }
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(ID);
            tb.Columns.Add(OrderSeq);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(DIRECTION);
            tb.Columns.Add(OPERATION);
            tb.Columns.Add(OFFSETFLAG);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(FILLED);
            tb.Columns.Add(STATUS);
            tb.Columns.Add(STATUSSTR);
            tb.Columns.Add(COMMENT);
        }
        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            Telerik.WinControls.UI.RadGridView grid = orderGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[ID].IsVisible = false;
            grid.Columns[DIRECTION].IsVisible = false;
            grid.Columns[STATUS].IsVisible = false;
            
            //set width
            //grid.Columns[SYMBOL].Width = 80;

            
        }


        #region 界面事件
        private void btnFilterAll_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            string strFilter ="";
            datasource.Filter = strFilter;
        }

        private void btnFilterPlaced_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            string strFilter = DATETIME + " DESC";
            strFilter = String.Format(STATUS + " = '{0}' or " + STATUS + " = '{1}'", "Placed", "Opened");
            datasource.Filter = strFilter;
        }

        private void btnFilterFilled_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            string strFilter = DATETIME + " DESC";
            strFilter = String.Format(STATUS + " = '{0}' ", "Filled");
            datasource.Filter = strFilter;
        }

        private void btnFilterCancelError_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            string strFilter = DATETIME + " DESC";
            strFilter = String.Format(STATUS + " = '{0}' or " + STATUS + " = '{1}' or " + STATUS + " = '{2}'", "Canceled", "Reject", "Unknown");
            datasource.Filter = strFilter;
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            long oid = SelectedOrderID;
            if (oid == -1)
            {
                MessageForm.Show("请选择要撤销的委托");
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
                    MessageForm.Show("该委托无法撤销");
                }
            }
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            foreach (Order o in CoreService.TradingInfoTracker.OrderTracker)
            {
                if (o.IsPending())
                {
                    Thread.Sleep(5);
                    CoreService.TLClient.ReqCancelOrder(o.id);
                }
            }
        }
        private void orderGrid_DoubleClick(object sender, EventArgs e)
        {
            long oid = SelectedOrderID;
            if (oid == -1)
            {
                MessageForm.Show("请选择要撤销的委托");
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
                    MessageForm.Show("该委托无法撤销");
                }
            }
        }
        #endregion


        long SelectedOrderID
        {
            get
            {
                if (orderGrid.SelectedRows.Count > 0)
                {
                    return long.Parse(orderGrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[ID].Value.ToString());
                }
                else
                {
                    return -1;
                }
            
            }
        }


        //格式化输出
        private void orderGrid_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement.RowInfo is GridViewDataRowInfo)
                {
                    if (e.CellElement.ColumnInfo.Name == OPERATION)
                    {
                        object direction = e.CellElement.RowInfo.Cells[DIRECTION].Value;
                        if (direction.ToString().Equals("1"))
                        {
                            e.CellElement.ForeColor = UIConstant.LongSideColor;
                            e.CellElement.Font = UIConstant.BoldFont;
                        }
                        else
                        {
                            e.CellElement.ForeColor = UIConstant.ShortSideColor;
                            e.CellElement.Font = UIConstant.BoldFont;
                        }
                    }

                    if (e.CellElement.ColumnInfo.Name == SYMBOL)
                    {
                        //e.CellElement.Font = UIGlobals.BoldFont;
                    }

                }


            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }

        }

        
    }
}
