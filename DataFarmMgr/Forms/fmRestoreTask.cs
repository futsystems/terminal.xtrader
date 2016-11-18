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
    public partial class fmRestoreTask : Form
    {
        FGrid barGrid = null;

        public fmRestoreTask()
        {
            InitializeComponent();


            barGrid = new FGrid();

            barGrid.Dock = DockStyle.Fill;
            panel2.Controls.Add(barGrid);

            InitTable();
            BindToTable();


            btnQryTaskStatus.Click += new EventHandler(btnQryTaskStatus_Click);
            btnResetTask.Click += new EventHandler(btnResetTask_Click);

            this.Load += new EventHandler(fmRestoreTask_Load);
            this.FormClosing += new FormClosingEventHandler(fmRestoreTask_FormClosing);
        }

        void btnResetTask_Click(object sender, EventArgs e)
        {
            int row = barGrid.SelectedRows.Count > 0 ? barGrid.SelectedRows[0].Index : -1;
            if (row >= 0)
            {
                string exchange = barGrid.SelectedRows[0].Cells[EXCHANGE].Value.ToString();
                string symbol = barGrid.SelectedRows[0].Cells[SYMBOL].Value.ToString();

                MessageBox.Show(exchange + " " + symbol);
                DataCoreService.DataClient.ReqContribRequest("DataFarm", "ResetRestoreTask", TradingLib.Mixins.Json.JsonMapper.ToJson(new { exchange = exchange, symbol = symbol }));
            }
            else
            {
                MessageBox.Show("请选择需要重置的任务");
            }
        }

        void fmRestoreTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataCoreService.EventContrib.UnRegisterCallback("DataFarm", "QryRestoreTask", OnTaskStatus);
        }

        void fmRestoreTask_Load(object sender, EventArgs e)
        {
            DataCoreService.EventContrib.RegisterCallback("DataFarm", "QryRestoreTask", OnTaskStatus);
        }

        void btnQryTaskStatus_Click(object sender, EventArgs e)
        {
            Clear();

            DataCoreService.DataClient.ReqContribRequest("DataFarm", "QryRestoreTask", "");
        }

        void OnTaskStatus(string json, bool islast)
        {
            RestoreTask[] items = ManagerHelper.ParseJsonResponse<RestoreTask[]>(json);
            if (items == null) return;
            foreach (var item in items)
            {
                InvokeGotTask(item);
            }
            if (islast)
            {
                BindToTable();
            }
        }

        void InvokeGotTask( RestoreTask task)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RestoreTask>(InvokeGotTask), new object[] { task });
            }
            else
            {
                gt.Rows.Add(task.Symbol);
                int i = gt.Rows.Count - 1;

                //bardatamap.Add(bar.ID, bar);
                //securitymap.Add(sec.ID, sec);
                //securityidxmap.Add(sec.ID, i);
                gt.Rows[i][EXCHANGE] = task.Exchange;
                gt.Rows[i][CREATEDTIME] = task.CreatedTime;
                gt.Rows[i][TICKFILLED] = task.IsTickFilled;
                gt.Rows[i][TICKFILLSUCCESS] = task.IsTickFillSuccess;
                gt.Rows[i][EODFILL] = task.IsEODRestored;
                gt.Rows[i][EODFILLSUCCESS] = task.IsEODRestoreSuccess;
                gt.Rows[i][INTRADAY1MINBAREND] = task.Intraday1MinHistBarEnd;

                gt.Rows[i][FEED1MINROUND] = task.DataFeed1MinRoundTime;
                gt.Rows[i][EXCHANGE1MINROUND] = task.Exchange1MinRoundtime;
                gt.Rows[i][EODHISTBAREND] = task.EodHistBarEndTradingDay;

                gt.Rows[i][COMPLETE] = task.Complete;
                gt.Rows[i][ENDTIME] = task.CompleteTime;

            }
        }



        const string SYMBOL = "合约";
        const string EXCHANGE = "交易所";
        const string CREATEDTIME = "创建时间";
        
        const string TICKFILLED = "Tick回补";
        const string TICKFILLSUCCESS = "Tick回补状态";
        const string EODFILL = "EOD回补";
        const string EODFILLSUCCESS = "EOD状态";
        const string INTRADAY1MINBAREND = "1分钟加载End";
        const string FEED1MINROUND = "行情源1分钟Round";
        const string EXCHANGE1MINROUND = "交易所1分钟Round";
        const string EODHISTBAREND = "日线加载End";
        
        const string COMPLETE = "任务状态";
        const string ENDTIME = "结束时间";




        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(SYMBOL);//
            gt.Columns.Add(EXCHANGE);//
            gt.Columns.Add(CREATEDTIME);//
            gt.Columns.Add(TICKFILLED);
            gt.Columns.Add(TICKFILLSUCCESS);
            gt.Columns.Add(EODFILL);
            gt.Columns.Add(EODFILLSUCCESS);
            gt.Columns.Add(INTRADAY1MINBAREND);//
            gt.Columns.Add(FEED1MINROUND);//
            gt.Columns.Add(EXCHANGE1MINROUND);//
            gt.Columns.Add(EODHISTBAREND);
            gt.Columns.Add(COMPLETE);
            gt.Columns.Add(ENDTIME);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(BindToTable), new object[] { });
            }
            else
            {
                System.Windows.Forms.DataGridView grid = barGrid;

                datasource.DataSource = gt;
                grid.DataSource = datasource;

                //需要在绑定数据源后设定具体的可见性
                //grid.Columns[EXCHANGEID].IsVisible = false;
                //grid.Columns[ID].Visible = false;
                //grid.Columns[SECTYPE].Visible = false;
                //grid.Columns[SECID].Visible = false;
            }
        }



        /// <summary>
        /// 清空表格内容
        /// </summary>
        public void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Clear), new object[] { });
            }
            else
            {
                barGrid.DataSource = null;
                gt.Rows.Clear();
                //BindToTable();
            }
        }
    }
}
