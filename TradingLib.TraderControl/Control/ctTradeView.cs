using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Telerik.WinControls;
using Telerik.WinControls.UI; 
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;


namespace TradingLib.TraderControl
{
    public partial class ctTradeView : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctTradeView");

        public ctTradeView()
        {
            InitializeComponent();

            SetPreferences();
            InitTable();
            BindToTable();

            CoreService.EventCore.RegIEventHandler(this);
            WireEvent();

            this.Load +=new EventHandler(ctTradeView_Load);
        }

        private void ctTradeView_Load(object sender, EventArgs e)
        {
            
        }

        private void WireEvent()
        {
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(EventIndicator_GotFillEvent);
        }

        void EventIndicator_GotFillEvent(Trade obj)
        {
            this.GotFill(obj);
        }

        public void OnInit()
        {
            foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
            {
                this.GotFill(f);
            }
        }

        public void OnDisposed()
        { 
            
        }
        public void GotFill(Trade t)
        {
            if (InvokeRequired)
                Invoke(new FillDelegate(GotFill), new object[] { t });
            else
            {
                DataRow r = tb.Rows.Add(t.id);
                int i = tb.Rows.Count - 1;//得到新建的Row号
                tb.Rows[i][ORDERID] = t.id;
                tb.Rows[i][TRADEID] = t.TradeID;
                tb.Rows[i][DATETIME] = Util.ToDateTime(t.xDate, t.xTime).ToString("HH:mm:ss");
                tb.Rows[i][SYMBOL] = t.Symbol;
                tb.Rows[i][SIDE] = (t.Side ? "买入" : "   卖出");
                tb.Rows[i][SIZE] = Math.Abs(t.xSize);
                tb.Rows[i][PRICE] = string.Format(TraderHelper.GetDisplayFormat(t.oSymbol), t.xPrice);
                tb.Rows[i][COMMISSION] = string.Format(UIConstant.DefaultDecimalFormat, t.Commission);
                tb.Rows[i][OPERATION] = t.IsEntryPosition ? "开仓" : "平仓";
                tb.Rows[i][PROFIT] = Util.FormatDecimal(t.Profit);
                
               
            }
        }

        public void Clear()
        {
            //kryptonDataGridView1.Enabled = false;
            //tradeGrid.Rows.Clear();
            //kryptonDataGridView1.Enabled = true;
            tradeGrid.DataSource = null;

            tb.Rows.Clear();
            BindToTable();
        }
        void toUpdateRow()
        {
            for (int i = 0; i < tradeGrid.Rows.Count; i++)
            {
                if (i == tradeGrid.Rows.Count - 1)
                {
                    tradeGrid.Rows[i].IsSelected = true;
                }
                else
                {
                    tradeGrid.Rows[i].IsSelected = false;
                }
            }
            //tradeGrid..FirstDisplayedScrollingRowIndex = tradeGrid.RowCount - 1;
        }


        const string ORDERID = "委托编号";
        const string TRADEID = "成交编号";
        const string DATETIME = "成交时间";
        const string SYMBOL = "合约";
        const string SIDE = "买卖";
        const string SIZE = "成交手数";
        const string PRICE = "成交价格";
        const string COMMISSION = "手续费";
        const string OPERATION = "开平";
        const string PROFIT = "盈亏";

        DataTable tb = new DataTable();

        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences() 
        {
            Telerik.WinControls.UI.RadGridView grid = tradeGrid;
            grid.ShowRowHeaderColumn = false;//显示每行的头部
            grid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;//列的填充方式
            grid.ShowGroupPanel = false;//是否显示顶部的panel用于组合排序
            grid.MasterTemplate.EnableGrouping = false;//是否允许分组
            grid.EnableHotTracking = true; 
            //this.radRadioDataReader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On; 

            grid.AllowAddNewRow = false;//不允许增加新行
            grid.AllowDeleteRow = false;//不允许删除行
            grid.AllowEditRow = false;//不允许编辑行
            grid.AllowRowResize = false;
            grid.EnableSorting = false;
            grid.TableElement.TableHeaderHeight = UIConstant.HeaderHeight;
            grid.TableElement.RowHeight = UIConstant.RowHeight;

            grid.EnableAlternatingRowColor = true;//隔行不同颜色
        }
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(TRADEID);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SIDE);
            tb.Columns.Add(OPERATION);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(COMMISSION);
            
            tb.Columns.Add(PROFIT);
        }

        BindingSource datasource = new BindingSource();

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        { 
            Telerik.WinControls.UI.RadGridView grid = tradeGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;
            grid.Columns[ORDERID].IsVisible = false;
        }


        private void tradeGrid_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement.RowInfo is GridViewDataRowInfo)
                {
                    
                    if (e.CellElement.ColumnInfo.Name == SIDE)
                    {
                        object side = e.CellElement.RowInfo.Cells[SIDE].Value;
                        if (side.ToString().Equals("买入"))
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
                }
            }
            catch (Exception ex)
            {
                logger.Error("Cellformating error:" + ex.ToString());
            }
        }

    }
}
