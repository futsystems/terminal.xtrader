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
    public partial class ctTradeViewSTK : UserControl, IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctOrderViewSTK");

        public ctTradeViewSTK()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();

            this.tradeGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(tradeGrid_CellFormatting);
            this.tradeGrid.SizeChanged += new EventHandler(tradeGrid_SizeChanged);
            this.Load += new EventHandler(ctTradeViewSTK_Load);
        }

        void tradeGrid_SizeChanged(object sender, EventArgs e)
        {
            ResetColumeSize();
        }

        void tradeGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool side = false;
                    bool.TryParse(tradeGrid[7, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? UIConstant.LongSideColor : UIConstant.ShortSideColor;
                }


            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }
        }

        void ctTradeViewSTK_Load(object sender, EventArgs e)
        {
            if (this._realview)
            {
                CoreService.EventCore.RegIEventHandler(this);
                CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);

                CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            }
        }

        void EventOther_OnResumeDataEnd()
        {
            foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
            {
                this.GotFill(f);
            }
        }

        void EventOther_OnResumeDataStart()
        {
            this.Clear();
        }

        public void OnInit()
        {
            foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
            {
                this.GotFill(f);
            }

        }

        public void OnDisposed()
        {

        }

        [DefaultValue(true)]
        bool _realview = true;
        /// <summary>
        /// 控件是否在实时状态工作
        /// 实时状态显示实时交易回报
        /// 查询状态显示查询回报
        /// </summary>
        public bool RealView
        {
            get { return _realview; }
            set
            {
                _realview = value;
            }
        }

        /// <summary>
        /// 获得合约名称
        /// </summary>
        /// <param name="fill"></param>
        /// <returns></returns>
        string GetSymbolName(Trade fill)
        {
            if (fill.oSymbol != null) return fill.oSymbol.GetName();
            return fill.Symbol;
        }

        string FormatPrice(Trade fill,decimal val)
        {
            if (fill.oSymbol != null) return val.ToFormatStr(fill.oSymbol);
            SecurityFamily sec = CoreService.BasicInfoTracker.GetSecurity(fill.SecurityCode);
            if (sec == null) return val.ToFormatStr();
            return val.ToFormatStr(sec);
        }

        const string _realDT = "HH:mm:ss";
        const string _histDT = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 获得委托时间
        /// 显示日内委托只显示时间 历史委托需要显示日期
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string GetDateTime(Trade t)
        {
            return Util.ToDateTime(t.xDate,t.xTime).ToString(_realview ? _realDT : _histDT);
        }


        public void GotFill(Trade fill)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Trade>(GotFill), new object[] { fill });
            }
            else
            {
                
                DataRow r = tb.Rows.Add(fill.id);
                int i = tb.Rows.Count - 1;//得到新建的Row号
                tb.Rows[i][ORDERID] = fill.id;
                tb.Rows[i][TRADEID] = fill.TradeID;
                tb.Rows[i][DATETIME] = GetDateTime(fill);
                tb.Rows[i][FILLDATE] = fill.xDate;
                tb.Rows[i][FILLTIME] = fill.xTime;

                tb.Rows[i][SYMBOL] = fill.Symbol;
                tb.Rows[i][SYMBOLNAME] = GetSymbolName(fill);
                tb.Rows[i][SIDE] = fill.Side;
                tb.Rows[i][OPERATION] = (fill.Side ? "买入" : "   卖出");
                tb.Rows[i][PRICE] = FormatPrice(fill, fill.xPrice);
                tb.Rows[i][SIZE] = Math.Abs(fill.xSize);
                tb.Rows[i][AMMOUNT] = fill.GetAmount();//fill.xSize * fill.xPrice;
                
            }
        }

        const string ORDERID = "委托编号";
        const string TRADEID = "成交编号";
        const string DATETIME = "日期时间";
        const string FILLDATE = "成交日期"; 
        const string FILLTIME = "成交时间";
        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";

        const string SIDE = "方向";
        const string OPERATION = "操作";
        const string PRICE = "成交均价";
        const string SIZE = "成交数量";
        const string AMMOUNT = "成交金额";


        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            KryptonDataGridView grid = tradeGrid;

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
            tb.Columns.Add(ORDERID);//0
            tb.Columns.Add(TRADEID);
            tb.Columns.Add(DATETIME);
            tb.Columns.Add(FILLDATE);
            tb.Columns.Add(FILLTIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(SIDE);
            tb.Columns.Add(OPERATION);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(AMMOUNT);
        }

        void ResetColumeSize()
        {
            ComponentFactory.Krypton.Toolkit.KryptonDataGridView grid = tradeGrid;
            grid.Columns[DATETIME].Width = _realview?80:160;
            grid.Columns[SYMBOL].Width = 100;
            grid.Columns[SYMBOLNAME].Width = 100;
            grid.Columns[OPERATION].Width = 80;
            
            grid.Columns[PRICE].Width = 100;
            grid.Columns[SIZE].Width = 80;
            
            

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            KryptonDataGridView grid = tradeGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[SIDE].Visible = false;
            grid.Columns[ORDERID].Visible = false;
            grid.Columns[TRADEID].Visible = false;
            grid.Columns[FILLDATE].Visible = false;
            grid.Columns[FILLTIME].Visible = false;

            //set width
            //grid.Columns[SYMBOL].Width = 80;
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ResetColumeSize();
        }


        /// <summary>
        /// 清空表格内容
        /// </summary>
        public void Clear()
        {

            tradeGrid.DataSource = null;
            tb.Rows.Clear();
            BindToTable();
        }

    }
}
