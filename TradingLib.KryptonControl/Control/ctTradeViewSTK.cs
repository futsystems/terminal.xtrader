using System;
using System.Collections.Generic;
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
    public partial class ctTradeViewSTK : UserControl, IEventBinder
    {
        public ctTradeViewSTK()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();
            CoreService.EventCore.RegIEventHandler(this);
            
            //实时状态 响应实时成交回报
            if (this._realview)
            {
                CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);
            }
            else
            { 
                
            }
        }

        public void OnInit()
        {
            //实时状态从交易数据维护器中恢复当日交易数据
            if (this._realview)
            {
                foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
                {
                    this.GotFill(f);
                }
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
        /// </summary>
        public bool RealView
        {
            get { return _realview; }
            set
            {
                _realview = value;
            }
        }

        public void GotFill(Trade fill)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Trade>(GotFill), new object[] { fill });
            }
            else
            {
                DataRow r = tb.Rows.Add(fill.id);
                int i = tb.Rows.Count - 1;//得到新建的Row号
                tb.Rows[i][ORDERID] = fill.id;
                tb.Rows[i][TRADEID] = fill.TradeID;
                tb.Rows[i][DATETIME] = Util.ToDateTime(fill.xDate, fill.xTime).ToString("HH:mm:ss");
                tb.Rows[i][FILLDATE] = fill.xDate;
                tb.Rows[i][FILLTIME] = fill.xTime;

                tb.Rows[i][SYMBOL] = fill.Symbol;
                tb.Rows[i][SYMBOLNAME] = fill.oSymbol.GetName();

                tb.Rows[i][OPERATEION] = (fill.Side ? "买入" : "   卖出");
                tb.Rows[i][PRICE] = fill.oSymbol.FormatPrice(fill.xPrice);
                tb.Rows[i][SIZE] = Math.Abs(fill.xSize);
                tb.Rows[i][AMMOUNT] = fill.xSize * fill.xPrice;
                
            }
        }

        const string ORDERID = "委托编号";
        const string TRADEID = "成交编号";
        const string DATETIME = "日期时间";
        const string FILLDATE = "成交日期"; 
        const string FILLTIME = "成交时间";
        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";

        const string OPERATEION = "操作";
        const string PRICE = "成交均价";
        const string SIZE = "成交数量";
        const string AMMOUNT = "成交金额";


        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            KryptonDataGridView grid = tradeGrid;

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
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(TRADEID);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(FILLDATE);
            tb.Columns.Add(FILLTIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(OPERATEION);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(AMMOUNT);
        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = tradeGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[ORDERID].Visible = false;
            grid.Columns[TRADEID].Visible = false;
            grid.Columns[DATETIME].Visible = false;

            //set width
            //grid.Columns[SYMBOL].Width = 80;
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
