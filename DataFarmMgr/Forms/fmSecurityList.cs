using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.DataFarmManager
{
    public partial class fmSecurityList : Form
    {
        FGrid secGrid = null;

        public fmSecurityList()
        {
            InitializeComponent();

            secGrid = new FGrid();
            secGrid.Dock = DockStyle.Fill;
            panel2.Controls.Add(secGrid);

            InitTable();
            BindToTable();
        }

        #region 表格
        #region 显示字段

        const string ID = "全局ID";
        const string CODE = "字头";
        const string NAME = "名称";
        const string CURRENCY = "货币";
        const string TYPE = "证券品种";
        const string MULTIPLE = "乘数";
        const string PRICETICK = "价格变动";
        const string EXCHANGEID = "ExchangeID";
        const string EXCHANGE = "交易所";
        const string MARKETTIMEID = "MarketTimeID";
        const string MARKETTIME = "交易时间段";
        const string DATAFEED = "行情源";



        #endregion

        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(ID);//
            gt.Columns.Add(CODE);//
            gt.Columns.Add(NAME);
            gt.Columns.Add(CURRENCY);
            gt.Columns.Add(TYPE);
            gt.Columns.Add(MULTIPLE);
            gt.Columns.Add(PRICETICK);//
            gt.Columns.Add(EXCHANGEID);//
            gt.Columns.Add(EXCHANGE);
            gt.Columns.Add(MARKETTIMEID);
            gt.Columns.Add(MARKETTIME);
            gt.Columns.Add(DATAFEED);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            System.Windows.Forms.DataGridView grid = secGrid;

            datasource.DataSource = gt;
            grid.DataSource = datasource;

            //需要在绑定数据源后设定具体的可见性
            //grid.Columns[EXCHANGEID].IsVisible = false;
            //grid.Columns[ID].Visible = false;
            //grid.Columns[SECTYPE].Visible = false;
            //grid.Columns[SECID].Visible = false;

        }





        #endregion

    }
}
