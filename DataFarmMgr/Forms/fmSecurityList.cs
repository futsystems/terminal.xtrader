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

            ManagerHelper.AdapterToIDataSource(cbExchange).BindDataSource(ManagerHelper.GetExchangeCombList(true));

            this.Load += new EventHandler(fmSecurityList_Load);
  
        }

        void fmSecurityList_Load(object sender, EventArgs e)
        {
            foreach (SecurityFamilyImpl sec in DataCoreService.DataClient.Securities)
            {
                InvokGotSecurity(sec);
            }

            cbExchange.SelectedIndexChanged += new EventHandler(cbExchange_SelectedIndexChanged);
            secGrid.DoubleClick += new EventHandler(secGrid_DoubleClick);
            btnAddSecurity.Click += new EventHandler(btnAddSecurity_Click);

            DataCoreService.EventManager.OnMGRUpdateSecurityResponse += new Action<RspMGRUpdateSecurityResponse>(EventManager_OnMGRUpdateSecurityResponse);
        }

        void EventManager_OnMGRUpdateSecurityResponse(RspMGRUpdateSecurityResponse obj)
        {
            SecurityFamilyImpl sec = DataCoreService.DataClient.GetSecurity(obj.SecurityFaimly.Code);
            InvokGotSecurity(sec);
        }

        void btnAddSecurity_Click(object sender, EventArgs e)
        {
            fmSecurityEdit fm = new fmSecurityEdit();
            fm.ShowDialog();

        }

        void secGrid_DoubleClick(object sender, EventArgs e)
        {
            SecurityFamilyImpl sec = GetVisibleSecurity(CurrentSecurityID);
            if (sec != null)
            {
                fmSecurityEdit fm = new fmSecurityEdit();
                fm.Security = sec;
                fm.ShowDialog();
            }
        }

        void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSecurityQuery();
        }

        void RefreshSecurityQuery()
        {
            string sectype = string.Empty;
            sectype = "*";
            string strFilter = string.Empty;

            strFilter = string.Format(TYPE + " > '{0}'", sectype);

            if (cbExchange.SelectedIndex != 0)
            {
                strFilter = string.Format(strFilter + " and " + EXCHANGEID + " = '{0}'", cbExchange.SelectedValue);
            }
            datasource.Filter = strFilter;
        }

        //得到当前选择的行号
        private int CurrentSecurityID
        {
            get
            {
                int row = secGrid.SelectedRows.Count > 0 ? secGrid.SelectedRows[0].Index : -1;
                if (row >= 0)
                {
                    return int.Parse(secGrid[0, row].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
        }



        //通过行号得该行的Security
        SecurityFamilyImpl GetVisibleSecurity(int id)
        {
            SecurityFamilyImpl sec = null;
            if (securitymap.TryGetValue(id, out sec))
            {
                return sec;
            }
            else
            {
                return null;
            }

        }




        Dictionary<int, SecurityFamilyImpl> securitymap = new Dictionary<int, SecurityFamilyImpl>();
        Dictionary<int, int> securityidxmap = new Dictionary<int, int>();
        int SecurityFamilyIdx(int id)
        {

            int rowid = -1;
            if (securityidxmap.TryGetValue(id, out rowid))
            {
                return rowid;
            }
            else
            {
                return -1;
            }
        }


        void InvokGotSecurity(SecurityFamilyImpl sec)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<SecurityFamilyImpl>(InvokGotSecurity), new object[] { sec });
            }
            else
            {
                int r = SecurityFamilyIdx(sec.ID);
                if (r == -1)
                {
                    gt.Rows.Add(sec.ID);
                    int i = gt.Rows.Count - 1;

                    securitymap.Add(sec.ID, sec);
                    securityidxmap.Add(sec.ID, i);

                    gt.Rows[i][CODE] = sec.Code;
                    gt.Rows[i][NAME] = sec.Name;
                    gt.Rows[i][CURRENCY] = Util.GetEnumDescription(sec.Currency);
                    gt.Rows[i][TYPE] = sec.Type;
                    gt.Rows[i][MULTIPLE] = sec.Multiple;
                    gt.Rows[i][PRICETICK] = sec.PriceTick;
                   
                    gt.Rows[i][EXCHANGEID] = sec.exchange_fk;
                    gt.Rows[i][EXCHANGE] = sec.Exchange != null ? sec.Exchange.Title : "未设置";
                    
                    gt.Rows[i][MARKETTIMEID] = sec.mkttime_fk;
                    gt.Rows[i][MARKETTIME] = sec.MarketTime != null ? sec.MarketTime.Name : "未设置";

                    gt.Rows[i][DATAFEED] = sec.DataFeed;
                }
                else
                {
                    int i = r;
                    //获得当前实例

                    SecurityFamilyImpl target = DataCoreService.DataClient.GetSecurity(sec.ID);
                    //MessageBox.Show("got security target code:" + target.Code + " seccode:" + sec.Code);
                    if (target != null)
                    {
                        
                        gt.Rows[i][CODE] = target.Code;
                        gt.Rows[i][NAME] = target.Name;
                        gt.Rows[i][CURRENCY] = Util.GetEnumDescription(sec.Currency);
                        gt.Rows[i][TYPE] = target.Type;
                        gt.Rows[i][MULTIPLE] = target.Multiple;
                        gt.Rows[i][PRICETICK] = target.PriceTick;
                      
                        gt.Rows[i][EXCHANGEID] = target.exchange_fk;
                        gt.Rows[i][EXCHANGE] = target.Exchange != null ? target.Exchange.Title : "未设置";
                       
                        gt.Rows[i][MARKETTIMEID] = target.mkttime_fk;
                        gt.Rows[i][MARKETTIME] = target.MarketTime != null ? target.MarketTime.Name : "未设置";

                        gt.Rows[i][DATAFEED] = sec.DataFeed;
                    }


                }
            }
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
