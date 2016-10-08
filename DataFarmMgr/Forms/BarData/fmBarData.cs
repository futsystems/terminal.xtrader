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

        Symbol _symbol = null;
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

            barGrid.DoubleClick += new EventHandler(barGrid_DoubleClick);

            btnUpload.Click += new EventHandler(btnUpload_Click);
            DataCoreService.EventHub.OnRspBarEvent += new Action<RspQryBarResponseBin>(EventHub_OnRspBarEvent);

            cbExchange_SelectedIndexChanged(null, null);
        }

        BarUploader upload = new BarUploader();
        void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //MessageBox.Show(fd.FileName);
                upload.SetBarFile(fd.FileName);
                upload.Exchange = "NYMEX";
                upload.Symbol = "CL11";
                upload.IntervalType = BarInterval.CustomTime;
                upload.Interval = 60;

                upload.Start();

                //BarReader br = new BarReader(fd.FileName);
                //br.GotBar += new Action<BarImpl>(br_GotBar);
                //while (br.NextTick())
                //{

                //    Util.sleep(1000);
                    
                //}
            }

            //UploadBarDataRequest request = new UploadBarDataRequest();
            //request.Header.BarCount = 1;
            //request.Header.Exchange = "NYMEX";
            //request.Header.Symbol = "CL11";
            //request.Header.IntervalType = BarInterval.CustomTime;
            //request.Header.Interval = 60;

            //BarImpl b = new BarImpl();
            //request.Add(b);

            //DataCoreService.DataClient.SendPacket(request);
        }

        void br_GotBar(BarImpl obj)
        {
            UploadBarDataRequest request = new UploadBarDataRequest();
            request.Header.BarCount = 1;
            request.Header.Exchange = "NYMEX";
            request.Header.Symbol = "CL11";
            request.Header.IntervalType = BarInterval.CustomTime;
            request.Header.Interval = 60;


            request.Add(obj);
            DataCoreService.DataClient.SendPacket(request);

        }


        //得到当前选择的行号
        private int CurrentBarID
        {
            get
            {
                int row = barGrid.SelectedRows.Count > 0 ? barGrid.SelectedRows[0].Index : -1;
                if (row >= 0)
                {
                    return int.Parse(barGrid[0, row].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
        }



        //通过行号得该行的Security
        BarImpl GetVisibleBar(int id)
        {
            BarImpl bar = null;
            if (bardatamap.TryGetValue(id, out bar))
            {
                return bar;
            }
            else
            {
                return null;
            }

        }

        void barGrid_DoubleClick(object sender, EventArgs e)
        {
            BarImpl bar = GetVisibleBar(CurrentBarID);
            if (bar == null)
            {
                MessageBox.Show("请选择需要编辑的Bar");
                return;
            }
            Exchange exch = DataCoreService.DataClient.Exchanges.Where(ex => ex.ID == (int)cbExchange.SelectedValue).FirstOrDefault();
            if (exch == null)
            {
                MessageBox.Show("请选择交易所");
                return;
            }

            fmBarDataEdit fm = new fmBarDataEdit();
            fm.SetBar(bar);
            fm.SetSymbol(_symbol);
            fm.ShowDialog();
            fm.Close();
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
            _symbol = symbol;
            int sIdx = (int)startIndex.Value;
            int max = (int)maxCount.Value;
            DataCoreService.DataClient.QryBar(symbol.Exchange, symbol.Symbol, 60, DateTime.MinValue, DateTime.MaxValue, sIdx,max, fromEnd.Checked,havePartial.Checked);
        }

        Dictionary<int, BarImpl> bardatamap = new Dictionary<int, BarImpl>();
        void InvokeGotBar(BarImpl bar)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<BarImpl>(InvokeGotBar), new object[] { bar });
            }
            else
            {
                gt.Rows.Add(bar.ID);
                int i = gt.Rows.Count - 1;

                bardatamap.Add(bar.ID, bar);
                //securitymap.Add(sec.ID, sec);
                //securityidxmap.Add(sec.ID, i);

                gt.Rows[i][TRADINGDAY] = bar.TradingDay;
                gt.Rows[i][STARTTIME] = bar.EndTime;
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
        const string STARTTIME = "Bar时间";
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
                _symbol = null;
                bardatamap.Clear();
                barGrid.DataSource = null;
                gt.Rows.Clear();
                //BindToTable();
            }
        }


        #endregion
    }
}
