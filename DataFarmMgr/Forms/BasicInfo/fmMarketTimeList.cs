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
    public partial class fmMarketTimeList : Form
    {
        FGrid mktimeGrid = null;
        public fmMarketTimeList()
        {
            InitializeComponent();

            mktimeGrid = new FGrid();
            mktimeGrid.Dock = DockStyle.Fill;
            panel1.Controls.Add(mktimeGrid);

            InitTable();
            BindToTable();
            mktimeGrid.DoubleClick += new EventHandler(mktimeGrid_DoubleClick);
            this.Load += new EventHandler(fmMarketTimeList_Load);
        }

        void mktimeGrid_DoubleClick(object sender, EventArgs e)
        {
            
            MarketTimeImpl mt = CurrentMarketTime;
            if (mt == null)
            {
                MessageBox.Show("请选择要编辑的交易时间段");
                return;
            }
            fmMarketTimeEdit fm = new fmMarketTimeEdit();
            fm.SetMarketTime(mt);
            fm.ShowDialog();
            fm.Dispose();
            
        }

        void fmMarketTimeList_Load(object sender, EventArgs e)
        {
            foreach (MarketTimeImpl mt in DataCoreService.DataClient.MarketTimes)
            {
                InvokeGotMarketTime(mt);
            }
        }


        private MarketTimeImpl CurrentMarketTime
        {
            get
            {
                int row = mktimeGrid.SelectedRows.Count > 0 ? mktimeGrid.SelectedRows[0].Index : -1; ;
                if (row >= 0)
                {
                    return mktimeGrid[TAG, row].Value as MarketTimeImpl;
                }
                else
                {
                    return null;
                }
            }
        }


        Dictionary<int, int> markettimeidxmap = new Dictionary<int, int>();
        int MarketTimeIdx(int id)
        {
            int rowid = -1;
            if (markettimeidxmap.TryGetValue(id, out rowid))
            {
                return rowid;
            }
            else
            {
                return -1;
            }
        }



        void InvokeGotMarketTime(MarketTimeImpl mt)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<MarketTimeImpl>(InvokeGotMarketTime), new object[] { mt });
            }
            else
            {
                int r = MarketTimeIdx(mt.ID);
                if (r == -1)
                {
                    gt.Rows.Add(mt.ID);
                    int i = gt.Rows.Count - 1;
                    gt.Rows[i][MTNAME] = mt.Name;
                    gt.Rows[i][MTDESC] = mt.Description;
                    gt.Rows[i][CLOSETIME] = Util.ToDateTime(Util.ToTLDate(), mt.CloseTime).ToString("HH:mm:ss");
                    gt.Rows[i][TAG] = mt;

                    markettimeidxmap.Add(mt.ID, i);

                }
                else
                {
                    int i = r;
                    gt.Rows[i][MTNAME] = mt.Name;
                    gt.Rows[i][MTDESC] = mt.Description;
                    gt.Rows[i][CLOSETIME] = Util.ToDateTime(Util.ToTLDate(), mt.CloseTime).ToString("HH:mm:ss");

                }
            }
        }


        #region 表格

        const string MTID = "全局ID";
        const string MTNAME = "名称";
        const string MTDESC = "描述";
        const string CLOSETIME = "收盘时间";
        const string TAG = "TAG";

        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            System.Windows.Forms.DataGridView grid = mktimeGrid;

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

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(MTID);//
            gt.Columns.Add(MTNAME);//
            gt.Columns.Add(MTDESC);//
            gt.Columns.Add(CLOSETIME);
            gt.Columns.Add(TAG,typeof(MarketTimeImpl));
        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            System.Windows.Forms.DataGridView grid = mktimeGrid;
            datasource.DataSource = gt;
            grid.DataSource = datasource;

            grid.Columns[TAG].Visible = false;
            grid.Columns[MTID].Width = 60;
            //grid.Columns[MTNAME].Width = 200;
            grid.Columns[CLOSETIME].Width = 60;

        }
        #endregion

    }
}
