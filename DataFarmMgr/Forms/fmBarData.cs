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
    public partial class fmBarData : Form
    {
        FGrid barGrid = null;
        public fmBarData()
        {
            InitializeComponent();

            barGrid = new FGrid();

            barGrid.Dock = DockStyle.Fill;
            panel2.Controls.Add(barGrid);

            InitTable();
            BindToTable();

            ManagerHelper.AdapterToIDataSource(cbExchange).BindDataSource(ManagerHelper.GetExchangeCombList(false));
            ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(0));
            ManagerHelper.AdapterToIDataSource(cbSymbol).BindDataSource(ManagerHelper.GetSymbolCombListViaSecurity(0));


            

            this.Load += new EventHandler(fmBarData_Load);
        }

        void fmBarData_Load(object sender, EventArgs e)
        {
            btnQry.Click += new EventHandler(btnQry_Click);
            cbExchange.SelectedIndexChanged += new EventHandler(cbExchange_SelectedIndexChanged);
            cbSecurity.SelectedIndexChanged += new EventHandler(cbSecurity_SelectedIndexChanged);

            DataCoreService.EventHub.OnRspBarEvent += new Action<RspQryBarResponseBin>(EventHub_OnRspBarEvent);

            cbExchange_SelectedIndexChanged(null, null);
        }

        void EventHub_OnRspBarEvent(RspQryBarResponseBin obj)
        {
            foreach(var bar in obj.Bars)
            {
                InvokeGotBar(bar);
            }
            //最后一个数据 执行界面数据绑定 提高显示效率
            if (obj.IsLast)
            {
                BindToTable();
                barGrid.FirstDisplayedScrollingRowIndex = barGrid.Rows.Count - barGrid.DisplayedRowCount(true);
            }
        }

        
        void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            int exid = (int)cbExchange.SelectedValue;
            ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(exid));
        }

        void cbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int secid = (int)cbSecurity.SelectedValue;
            ManagerHelper.AdapterToIDataSource(cbSymbol).BindDataSource(ManagerHelper.GetSymbolCombListViaSecurity(secid));
        }


        void btnQry_Click(object sender, EventArgs e)
        {
            Clear();

            Symbol symbol = (Symbol)cbSymbol.SelectedValue;
            if (symbol == null)
            {
                MessageBox.Show("请选择需要查询的合约");
                return;
            }

            DataCoreService.DataClient.QryBar(symbol.Exchange, symbol.Symbol, 60, DateTime.MinValue, DateTime.MaxValue, 1000, true);
        }

        void InvokeGotBar(Bar bar)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Bar>(InvokeGotBar), new object[] { bar });
            }
            else
            {
                gt.Rows.Add(0);
                int i = gt.Rows.Count - 1;

                //securitymap.Add(sec.ID, sec);
                //securityidxmap.Add(sec.ID, i);

                gt.Rows[i][TRADINGDAY] = bar.TradingDay;
                gt.Rows[i][STARTTIME] = bar.StartTime;
                gt.Rows[i][SYMBOL] = bar.Symbol;
                gt.Rows[i][OPEN] = bar.Open;
                gt.Rows[i][HIGH] = bar.High;
                gt.Rows[i][LOW] = bar.Low;

                gt.Rows[i][CLOSE] = bar.Close;
                gt.Rows[i][VOL] = bar.Volume;

                gt.Rows[i][OI] = bar.OpenInterest;
                gt.Rows[i][XCOUNT] = bar.TradeCount;

            }
        }

        #region 表格

        const string ID = "数据库ID";
        const string TRADINGDAY = "交易日";
        const string STARTTIME = "开始时间";
        const string SYMBOL = "合约";
        const string OPEN = "Open";
        const string HIGH = "High";
        const string LOW = "Low";
        const string CLOSE = "Close";
        const string VOL = "Vol";
        const string OI = "OI";
        const string XCOUNT = "XCount";




        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(ID);//
            gt.Columns.Add(TRADINGDAY);//
            gt.Columns.Add(STARTTIME);
            gt.Columns.Add(SYMBOL);
            gt.Columns.Add(OPEN);
            gt.Columns.Add(HIGH);
            gt.Columns.Add(LOW);//
            gt.Columns.Add(CLOSE);//
            gt.Columns.Add(VOL);
            gt.Columns.Add(OI);
            gt.Columns.Add(XCOUNT);

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


        #endregion
    }
}
