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
    public partial class ctDeliveryViewSTK : UserControl
    {
        public ctDeliveryViewSTK()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();

        }

        const string FILLDATE = "成交日期";
        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";
        const string REMARK = "摘要";
        const string SIZE = "成交数量";
        const string PRICE = "成交均价";
        const string AMOUNT = "成交金额";
        const string AMOUNT2 = "发生金额";
        const string COMMISSION = "手续费";
        const string STAMPTAX = "印花税";
        const string TRANSFERFEE = "过户费";
        const string CONTRACTID = "合同编号";



        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            KryptonDataGridView grid = deliveryGrid;

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

        }

        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(FILLDATE);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(REMARK);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(PRICE);

            tb.Columns.Add(AMOUNT);
            tb.Columns.Add(AMOUNT2);
            tb.Columns.Add(COMMISSION);
            tb.Columns.Add(STAMPTAX);
            tb.Columns.Add(TRANSFERFEE);
            tb.Columns.Add(CONTRACTID);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = deliveryGrid;
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
