using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlPosition : UserControl,TradingLib.API.IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctrlPosition");
        FGrid positionGrid = null;
        public ctrlPosition()
        {
            InitializeComponent();

          

            this.positionGrid = new FGrid();
            this.positionGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(positionGrid);


            InitTable();
            BindToTable();

            //this.positionGrid.SizeChanged += new EventHandler(positionGrid_SizeChanged);
            //this.positionGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(positionGrid_CellFormatting);
            this.Load += new EventHandler(ctPositionViewSTK_Load);

        }


        void ctPositionViewSTK_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._realview)
                {
                    CoreService.EventCore.RegIEventHandler(this);
                    //CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
                    //CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);

                    CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                    CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
                }

                positionGrid.DoubleClick += new EventHandler(positionGrid_DoubleClick);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 交易数据恢复结束后 查询每个持仓合约的行情快照
        /// </summary>
        void EventOther_OnResumeDataEnd()
        {
            //恢复持仓数据
            foreach (var pos in CoreService.TradingInfoTracker.PositionTracker)
            {
                this.GotPosition(pos);
                CoreService.TLClient.ReqXQryTickSnapShot(pos.oSymbol.Exchange, pos.oSymbol.Symbol);
            }
        }

        void EventOther_OnResumeDataStart()
        {
            this.Clear();
        }

        /// <summary>
        /// 获得当前选中持仓
        /// </summary>
        Position CurrentPositoin
        {
            get
            {
                if (positionGrid.SelectedRows.Count > 0)
                {
                    string sym = positionGrid.SelectedRows[0].Cells[SYMBOL].Value.ToString();
                    string account = positionGrid.SelectedRows[0].Cells[ACCOUNT].Value.ToString();
                    bool side = bool.Parse(positionGrid.SelectedRows[0].Cells[SIDE].Value.ToString());

                    return CoreService.TradingInfoTracker.PositionTracker[sym, account, side];
                }
                else
                {
                    return null;
                }
            }
        }


        void positionGrid_DoubleClick(object sender, EventArgs e)
        {
            Position pos = CurrentPositoin;
            if (pos == null) return;
            logger.Info(string.Format("Position:{0} selected", pos.GetPositionKey()));
            CoreService.EventUI.FireSymbolSelectedEvent(this, pos.oSymbol);
        }


        public void OnInit()
        {
            //恢复持仓数据
            foreach (var pos in CoreService.TradingInfoTracker.PositionTracker)
            {
                this.GotPosition(pos);
                CoreService.TLClient.ReqXQryTickSnapShot(pos.oSymbol.Exchange, pos.oSymbol.Symbol);
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


        string FormatPrice(Position pos, decimal val)
        {
            if (pos.oSymbol != null) return val.ToFormatStr(pos.oSymbol);
            SecurityFamily sec = null;// CoreService.BasicInfoTracker.GetSecurity(pos.SecurityCode);
            if (sec == null) return val.ToFormatStr();
            return val.ToFormatStr(sec);
        }



        //合约所对应的table id key为 account - symbol
        ConcurrentDictionary<string, int> poskeyRowIdxMap = new ConcurrentDictionary<string, int>();
        //通过account-symbol获得对应的tableid
        int PosiitonRowIdx(string poskey)
        {
            int idx = -1;
            if (poskeyRowIdxMap.TryGetValue(poskey, out idx))
            {
                return idx;
            }
            return -1;
        }



        /// <summary>
        /// 插入一条新的持仓对象
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int InsertNewRow(Position pos)
        {
            string key = pos.GetPositionKey();
            tb.Rows.Add(key);

            int i = tb.Rows.Count - 1;
            tb.Rows[i][POSKEY] = key;
            tb.Rows[i][SYMBOLKEY] = pos.oSymbol.GetUniqueKey();
            tb.Rows[i][SYMBOL] = pos.Symbol;
            tb.Rows[i][SIDE] = pos.DirectionType == QSEnumPositionDirectionType.Long;//??
            



            if (!poskeyRowIdxMap.ContainsKey(key))
                poskeyRowIdxMap.TryAdd(key, i);

            return i;
        }

        //获得某个持仓的可平数量
        int GetCanFlatSize(Position pos)
        {
            OrderTracker ot = CoreService.TradingInfoTracker.OrderTracker;
            return pos.isFlat ? 0 : (pos.UnsignedSize - ot.GetPendingExitSize(pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false));
        }

        public void GotPosition(Position pos)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Position>(GotPosition), new object[] { pos });
            }
            else
            {
                int posidx = PosiitonRowIdx(pos.GetPositionKey());
                if (posidx == -1) //不存在对应的持仓
                {
                    int i = InsertNewRow(pos);
                    tb.Rows[i][PROPERTY] = "";
                    tb.Rows[i][SIZE] = pos.UnsignedSize;
                    tb.Rows[i][SIZECANFLAT] = GetCanFlatSize(pos);
                    tb.Rows[i][AVGPRICE] = FormatPrice(pos,pos.AvgPrice);

                    tb.Rows[i][UNREALIZEDPL] = "";
                    tb.Rows[i][LOSSTARGET] = "";
                    tb.Rows[i][PROFITTARGET] = "";
                    tb.Rows[i][FLAG] = "投";

                }
                else
                {
                    int i = posidx;
                    tb.Rows[i][SIZE] = pos.UnsignedSize;
                    tb.Rows[i][SIZECANFLAT] = GetCanFlatSize(pos);
                    tb.Rows[i][AVGPRICE] = FormatPrice(pos, pos.AvgPrice);
                }
            }

        }


        #region 表格
        const string POSKEY = "持仓键";
        const string ACCOUNT = "ACCOUNT";
        const string SYMBOLKEY = "合约键";
        const string SYMBOL = "合约";
        const string SIDE = "方向";

        const string PROPERTY = "属性";
        const string SIZE = "持仓";

        const string SIZECANFLAT = "可用";
        const string AVGPRICE = "开仓均价";
        const string UNREALIZEDPL = "浮动盈亏";
        const string LOSSTARGET = "止损/数量";

        const string PROFITTARGET = "止盈/数量";
        const string FLAG = "投保";
        const string NAME = "名称";




        
        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(POSKEY);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(ACCOUNT);
            tb.Columns.Add(SYMBOLKEY);
            tb.Columns.Add(SIDE);

            tb.Columns.Add(PROPERTY);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(SIZECANFLAT);
            tb.Columns.Add(AVGPRICE);
            tb.Columns.Add(UNREALIZEDPL);
            tb.Columns.Add(LOSSTARGET);

            tb.Columns.Add(PROFITTARGET);
            tb.Columns.Add(FLAG);
            tb.Columns.Add(NAME);


        }

        //void ResetColumeSize()
        //{
        //    ComponentFactory.Krypton.Toolkit.KryptonDataGridView grid = positionGrid;

        //    grid.Columns[SYMBOL].Width = 100;
        //    grid.Columns[SYMBOLNAME].Width = 100;
        //    grid.Columns[TOTALSIZE].Width = 60;
        //    grid.Columns[AVABILESIZE].Width = 60;
        //    grid.Columns[FRONZENSIZE].Width = 60;

        //    grid.Columns[POSITIONPROFIT].Width = 80;
        //    grid.Columns[AVGPRICE].Width = 80;
        //    grid.Columns[LASTPRICE].Width = 80;
        //    grid.Columns[POSITIONPROFITPECT].Width = 80;
        //    grid.Columns[MARKETVALUE].Width = 120;




        //}

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid  = positionGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[POSKEY].Visible = false;
            grid.Columns[ACCOUNT].Visible = false;
            grid.Columns[SYMBOLKEY].Visible = false;
            
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ///ResetColumeSize();
        }

        /// <summary>
        /// 清空表格内容
        /// </summary>
        public void Clear()
        {
            poskeyRowIdxMap.Clear();
            positionGrid.DataSource = null;
            tb.Rows.Clear();
            BindToTable();
        }

        #endregion


    }
}
