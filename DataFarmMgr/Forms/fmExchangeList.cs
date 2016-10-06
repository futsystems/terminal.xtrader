using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public partial class fmExchangeList : Form
    {
        FGrid exchangeGrid = null;
        public fmExchangeList()
        {
            InitializeComponent();

            exchangeGrid = new FGrid();
            exchangeGrid.Dock = DockStyle.Fill;
            panel1.Controls.Add(exchangeGrid);

            InitTable();
            BindToTable();

            this.Load += new EventHandler(fmExchangeList_Load);
        }

        void fmExchangeList_Load(object sender, EventArgs e)
        {
            //exchangeGrid.RowPrePaint += new DataGridViewRowPrePaintEventHandler(exchangegrid_RowPrePaint);
            //exchangeGrid.DoubleClick += new EventHandler(exchangegrid_DoubleClick);
            exchangeGrid.DoubleClick += new EventHandler(exchangeGrid_DoubleClick);
            //btnAddExchange.Click += new EventHandler(btnAddExchange_Click);
            foreach (Exchange ex in DataCoreService.MDClient.Exchanges)
            {
                this.InvokeGotExchange(ex);
            }
        }

        void exchangeGrid_DoubleClick(object sender, EventArgs e)
        {
            Exchange current = GetExchangeSelected(CurrentExchangeID);
            if (current == null)
            {
                MessageBox.Show("请选择要编辑的交易所");
                return;
            }
            fmExchangeEdit fm = new fmExchangeEdit();
            fm.SetExchange(current);
            fm.ShowDialog();
            fm.Close();
        }

        private int CurrentExchangeID
        {
            get
            {
                int row = exchangeGrid.SelectedRows.Count > 0 ? exchangeGrid.SelectedRows[0].Index : -1; ;
                if (row >= 0)
                {
                    return int.Parse(exchangeGrid[0, row].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
        }

        



        //通过行号得该行的Security
        Exchange GetExchangeSelected(int id)
        {
            Exchange ex = null;
            if (exchangemap.TryGetValue(id, out ex))
            {
                return ex;
            }
            else
            {
                return null;
            }

        }

        Dictionary<int, int> exchangeidmap = new Dictionary<int, int>();
        Dictionary<int, Exchange> exchangemap = new Dictionary<int, Exchange>();

        int ExchangeIdx(int id)
        {
            int rowid = -1;
            if (exchangeidmap.TryGetValue(id, out rowid))
            {
                return rowid;
            }
            else
            {
                return -1;
            }
        }


       
        void InvokeGotExchange(Exchange ex)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Exchange>(InvokeGotExchange), new object[] { ex });
            }
            else
            {
                int r = ExchangeIdx(ex.ID);
                if (r == -1)
                {
                    gt.Rows.Add(ex.ID);
                    int i = gt.Rows.Count - 1;
                    gt.Rows[i][EXNAME] = ex.Name;
                    gt.Rows[i][EXCODE] = ex.EXCode;
                    gt.Rows[i][EXCOUNTRY] = Util.GetEnumDescription(ex.Country);
                    gt.Rows[i][TITLE] = ex.Title;

                    gt.Rows[i][TIMEZONE] = ex.TimeZoneID;//ex.TimeZoneInfo != null ? ex.TimeZoneInfo.DisplayName : "";
                    gt.Rows[i][CALENDAR] = ex.Calendar;
                    gt.Rows[i][SETTLETIME] = Util.ToDateTime(Util.ToTLDate(), ex.CloseTime).ToString("HH:mm:ss");
                    gt.Rows[i][SETTLETYPE] = Util.GetEnumDescription(ex.SettleType);

                    exchangemap.Add(ex.ID, ex);
                    exchangeidmap.Add(ex.ID, i);

                }
                else
                {
                    int i = r;
                    gt.Rows[i][EXNAME] = ex.Name;
                    gt.Rows[i][EXCOUNTRY] = Util.GetEnumDescription(ex.Country);
                    gt.Rows[i][TITLE] = ex.Title;
                    gt.Rows[i][TIMEZONE] = ex.TimeZoneID;// ex.TimeZoneInfo != null ? ex.TimeZoneInfo.DisplayName : "";
                    gt.Rows[i][CALENDAR] = ex.Calendar;
                    gt.Rows[i][SETTLETIME] = Util.ToDateTime(Util.ToTLDate(), ex.CloseTime).ToString("HH:mm:ss");
                    gt.Rows[i][SETTLETYPE] = Util.GetEnumDescription(ex.SettleType);
                }
            }
        }



        #region 表格
        const string EXID = "全局ID";

        const string EXCODE = "编号";
        const string EXCOUNTRY = "国家";
        const string EXNAME = "名称";
        const string TITLE = "简称";
        const string TIMEZONE = "时区";
        const string CALENDAR = "交易日历";
        const string SETTLETIME = "结算时间";
        const string SETTLETYPE = "结算方式";

        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

      

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(EXID);//

            gt.Columns.Add(EXCODE);//
            gt.Columns.Add(EXCOUNTRY);//
            gt.Columns.Add(EXNAME);//
            gt.Columns.Add(TITLE);
            gt.Columns.Add(TIMEZONE);
            gt.Columns.Add(CALENDAR);
            gt.Columns.Add(SETTLETIME);
            gt.Columns.Add(SETTLETYPE);
        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            System.Windows.Forms.DataGridView grid = exchangeGrid;

            datasource.DataSource = gt;
            grid.DataSource = datasource;

            grid.Columns[EXID].Width = 60;

            grid.Columns[EXCODE].Width = 60;
            grid.Columns[EXCOUNTRY].Width = 60;
            grid.Columns[EXNAME].Width = 120;
            grid.Columns[CALENDAR].Width = 60;
            grid.Columns[SETTLETIME].Width = 60;
            grid.Columns[SETTLETYPE].Width = 60;


        }





        #endregion
    }
}
