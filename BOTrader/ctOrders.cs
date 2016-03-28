using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace BOTrader
{
    public partial class ctOrders : UserControl
    {
        public ctOrders()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();

        }

        const string ORDERID = "订单编号";//Trade ID
        const string ASSET = "商品名称";//Asset
        


        const string TIMESPAN = "时段";
        const string OPTIONSIDETYPE = "购买方向";//OptionType
        const string AMOUNT = "订单金额";//Investment
        const string PAYOUT = "回报率";//PayOut
        const string ENTRYTIME = "下单时间";//Trading Time
        const string EXPIRETIME = "到期时间";//Expire Time
        const string ENTRYPRICE = "下单价格";//Strike
        const string LASTPRICE = "最新价格";
        const string EXPIREPRICE = "到期价格";//Expire Price
        const string PL = "盈亏";
        const string ORDERSTATUS = "提交状态";
        const string RESULT = "期权状态";//status




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
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(ASSET);
            tb.Columns.Add(TIMESPAN);

            tb.Columns.Add(OPTIONSIDETYPE);
            tb.Columns.Add(AMOUNT);
            tb.Columns.Add(PAYOUT);
            tb.Columns.Add(ENTRYTIME);
            tb.Columns.Add(EXPIRETIME);

            tb.Columns.Add(ENTRYPRICE);
            tb.Columns.Add(LASTPRICE);
            tb.Columns.Add(EXPIREPRICE);
            tb.Columns.Add(PL);
            tb.Columns.Add(ORDERSTATUS);
            tb.Columns.Add(RESULT);

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
