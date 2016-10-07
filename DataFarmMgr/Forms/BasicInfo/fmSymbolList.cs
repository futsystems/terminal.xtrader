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
    public partial class fmSymbolList : Form
    {

        FGrid symGrid = null;
        public fmSymbolList()
        {
            InitializeComponent();
            symGrid = new FGrid();
            symGrid.Dock = DockStyle.Fill;
            panel2.Controls.Add(symGrid);

            InitTable();
            BindToTable();

            ManagerHelper.AdapterToIDataSource(cbExchange).BindDataSource(ManagerHelper.GetExchangeCombList());
            ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(0));

            DataCoreService.EventManager.OnMGRUpdateSymbolResponse += new Action<RspMGRUpdateSymbolResponse>(EventManager_OnMGRUpdateSymbolResponse);
            

            this.Load += new EventHandler(fmSymbolList_Load);
        }

        void EventManager_OnMGRUpdateSymbolResponse(RspMGRUpdateSymbolResponse obj)
        {
            SecurityFamilyImpl sec = DataCoreService.DataClient.GetSecurity(obj.Symbol.security_fk);
            SymbolImpl symbol = DataCoreService.DataClient.GetSymbol(sec.Exchange.EXCode, obj.Symbol.Symbol);
            InvokeGotSymbol(symbol);
        }

        void fmSymbolList_Load(object sender, EventArgs e)
        {

            WireEvent();

            foreach (var symbol in DataCoreService.DataClient.Symbols)
            {
                InvokeGotSymbol(symbol);
            }
        }


        void WireEvent()
        {
            cbSecurity.SelectedIndexChanged += new EventHandler(cbSecurity_SelectedIndexChanged);
            cbExchange.SelectedIndexChanged += new EventHandler(cbExchange_SelectedIndexChanged);

            symGrid.DoubleClick += new EventHandler(symGrid_DoubleClick);
            btnAddSymbol.Click += new EventHandler(btnAddSymbol_Click);
        }

        void btnAddSymbol_Click(object sender, EventArgs e)
        {
            fmSymbolEdit fm = new fmSymbolEdit();
            fm.ShowDialog();
        }

        //得到当前选择的行号
        private int CurrentSymbolID
        {
            get
            {
                int row = symGrid.SelectedRows.Count > 0 ? symGrid.SelectedRows[0].Index : -1;
                if (row >= 0)
                {
                    return int.Parse(symGrid[0, row].Value.ToString());
                }
                else
                {
                    return 0;
                }
            }
        }


        //通过行号得该行的Security
        SymbolImpl GetVisibleSymbol(int id)
        {
            SymbolImpl sym = null;
            if (symbolmap.TryGetValue(id, out sym))
            {
                return sym;
            }
            else
            {
                return null;
            }

        }

        void symGrid_DoubleClick(object sender, EventArgs e)
        {
            SymbolImpl symbol = GetVisibleSymbol(CurrentSymbolID);
            if (symbol != null)
            {
                
                fmSymbolEdit fm = new fmSymbolEdit();
                fm.Symbol = symbol;
                fm.ShowDialog();
            }
        }

        void cbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSymbols();
        }

        void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshSymbols();
            int exid = (int)cbExchange.SelectedValue;
            ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(exid,true));
          
        }
        string GetMonth(SymbolImpl sym)
        {
            switch (sym.SecurityFamily.Type)
            {
                case SecurityType.FUT:
                    return sym.Month;
                case SecurityType.STK:
                    return "";
                default:
                    return "";
            }
        }

        string GetExpireDate(SymbolImpl sym)
        {
            switch (sym.SecurityFamily.Type)
            {
                case SecurityType.FUT:
                    return sym.ExpireDate.ToString();
                case SecurityType.STK:
                    return "";
                default:
                    return "";
            }
        }


        Dictionary<int, SymbolImpl> symbolmap = new Dictionary<int, SymbolImpl>();
        Dictionary<int, int> symbolidxmap = new Dictionary<int, int>();

        int SymbolIdx(int id)
        {
            int rowid = -1;
            if (symbolidxmap.TryGetValue(id, out rowid))
            {
                return rowid;
            }
            else
            {
                return -1;
            }
        }

        void InvokeGotSymbol(SymbolImpl sym)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<SymbolImpl>(InvokeGotSymbol), new object[] { sym });
            }
            else
            {
                if (string.IsNullOrEmpty(sym.Symbol))
                    return;
                int r = SymbolIdx(sym.ID);
                if (r == -1)
                {
                    gt.Rows.Add(sym.ID);
                    int i = gt.Rows.Count - 1;

                    symbolmap.Add(sym.ID, sym);
                    symbolidxmap.Add(sym.ID, i);

                    gt.Rows[i][SYMBOL] = sym.Symbol;
                    gt.Rows[i][NAME] = sym.GetName();

                    gt.Rows[i][SECID] = sym.SecurityFamily != null ? (sym.SecurityFamily as SecurityFamilyImpl).ID : 0;
                    gt.Rows[i][SECCODE] = sym.SecurityFamily != null ? sym.SecurityFamily.Code : "未设置";
                    gt.Rows[i][SECTYPE] = sym.SecurityType;
                    gt.Rows[i][EXCHANGEID] = sym.SecurityFamily != null ? (sym.SecurityFamily.Exchange as Exchange).ID : 0;
                    gt.Rows[i][EXCHANGE] = sym.SecurityFamily != null ? sym.SecurityFamily.Exchange.Title : "未设置";

               
                    gt.Rows[i][MONTH] = GetMonth(sym);//sym.Month;
                    gt.Rows[i][EXPIREDATE] = GetExpireDate(sym);//sym.ExpireDate;
                    gt.Rows[i][SYMBOLTYPE] = Util.GetEnumDescription(sym.SymbolType);

                }
                else
                {
                    int i = r;
                    gt.Rows[i][SYMBOL] = sym.Symbol;
                    gt.Rows[i][NAME] = sym.GetName();
                  
                    gt.Rows[i][SECID] = sym.SecurityFamily != null ? (sym.SecurityFamily as SecurityFamilyImpl).ID : 0;
                    gt.Rows[i][SECCODE] = sym.SecurityFamily != null ? sym.SecurityFamily.Code : "未设置";
                    gt.Rows[i][SECTYPE] = sym.SecurityType;
                    gt.Rows[i][EXCHANGEID] = sym.SecurityFamily != null ? (sym.SecurityFamily.Exchange as Exchange).ID : 0;
                    gt.Rows[i][EXCHANGE] = sym.SecurityFamily != null ? sym.SecurityFamily.Exchange.Title : "未设置";

                    gt.Rows[i][MONTH] = GetMonth(sym);//sym.Month;
                    gt.Rows[i][EXPIREDATE] = GetExpireDate(sym);//sym.ExpireDate;
                 
                    gt.Rows[i][SYMBOLTYPE] = Util.GetEnumDescription(sym.SymbolType);
                }
            }
            
        }


        void RefreshSymbols()
        {

            string strFilter = string.Format(SECTYPE + " > '{0}'", "*");
            

            //通过交易所过滤
            if (cbExchange.SelectedIndex != 0)
            {
                strFilter = string.Format(strFilter + " and " + EXCHANGEID + " = '{0}'", cbExchange.SelectedValue);
            }

            
            //通过交易所过滤
            if (cbSecurity.SelectedIndex != 0)
            {
                strFilter = string.Format(strFilter + " and " + SECID + " = '{0}'", cbSecurity.SelectedValue);
            }


            datasource.Filter = strFilter;
        }

        #region 表格
        #region 显示字段

        const string ID = "全局ID";
        const string SYMBOL = "合约";
        const string NAME = "名称";
        const string SECID = "SecID";//品种编号
        const string SECCODE = "品种";
        const string SECTYPE = "SecType";
        const string EXCHANGEID = "ExchangeID";
        const string EXCHANGE = "交易所";
        const string MONTH = "月份";
        const string EXPIREDATE = "到期日";
        const string SYMBOLTYPE = "类别";



        #endregion

        DataTable gt = new DataTable();
        BindingSource datasource = new BindingSource();

        //初始化Account显示空格
        private void InitTable()
        {
            gt.Columns.Add(ID);//
            gt.Columns.Add(SYMBOL);//
            gt.Columns.Add(NAME);
            gt.Columns.Add(SECID);
            gt.Columns.Add(SECCODE);
            gt.Columns.Add(SECTYPE);
            gt.Columns.Add(EXCHANGEID);//
            gt.Columns.Add(EXCHANGE);//
            gt.Columns.Add(MONTH);
            gt.Columns.Add(EXPIREDATE);
            gt.Columns.Add(SYMBOLTYPE);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            System.Windows.Forms.DataGridView grid = symGrid;

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
