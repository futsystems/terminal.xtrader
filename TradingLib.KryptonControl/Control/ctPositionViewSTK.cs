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


namespace TradingLib.NevronControl.Control
{
    public partial class ctPositionViewSTK : UserControl
    {
        public ctPositionViewSTK()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();

        }


       

        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";
        const string TOTALSIZE = "股票余额";
        const string AVABILESIZE = "可用余额";
        const string FRONZENSIZE = "冻结数量";

        const string POSITIONPROFIT = "盈亏";
        const string AVGPRICE = "成本价";
        const string POSITIONPROFITPECT = "盈亏比例(%)";
        const string LASTPRICE="市价";
        const string MARKETVALUE = "市值";
        const string EXCHANGE = "交易市场";



        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            KryptonDataGridView grid = positionGrid;

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
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(TOTALSIZE);
            tb.Columns.Add(AVABILESIZE);
            tb.Columns.Add(FRONZENSIZE);

            tb.Columns.Add(POSITIONPROFIT);
            tb.Columns.Add(AVGPRICE);
            tb.Columns.Add(POSITIONPROFITPECT);
            tb.Columns.Add(LASTPRICE);
            tb.Columns.Add(MARKETVALUE);
            tb.Columns.Add(EXCHANGE);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = positionGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            //grid.Columns[ORDERID].Visible = false;
            //grid.Columns[TRADEID].Visible = false;
            //grid.Columns[DATETIME].Visible = false;

            //set width
            //grid.Columns[SYMBOL].Width = 80;
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}
