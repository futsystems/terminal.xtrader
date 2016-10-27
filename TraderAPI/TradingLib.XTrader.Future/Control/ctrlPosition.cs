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

            WireEvent();

        }


        void WireEvent()
        {
            this.Load += new EventHandler(ctrlPosition_Load);
            positionGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(positionGrid_CellFormatting);
            positionGrid.MouseClick += new MouseEventHandler(positionGrid_MouseClick);
        }

        void positionGrid_MouseClick(object sender, MouseEventArgs e)
        {
            //orderGrid.SelectedRows[0].Selected = false;
            int rowid = GetRowIndexAt(e.Y);
            if (rowid == -1)
            {
                if (positionGrid.SelectedRows.Count > 0)
                {
                    positionGrid.SelectedRows[0].Selected = false;
                }
            }
            //MessageBox.Show(rowid.ToString());
        }
        int GetRowIndexAt(int mouseLocation_Y)
        {
            if (positionGrid.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;  // no rows.   
            }
            if (positionGrid.ColumnHeadersVisible == true && mouseLocation_Y <= positionGrid.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = positionGrid.FirstDisplayedScrollingRowIndex;
            int displayedCount = positionGrid.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount; )  // 因为行不能ReOrder，故只需要搜索显示的行   
            {
                if (positionGrid.Rows[index].Visible == true)
                {
                    Rectangle rect = positionGrid.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    k++;  // 只计数显示的行;   
                }
                index++;
            }
            return -1;
        }  



        void positionGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool side = false;
                    bool.TryParse(positionGrid[4, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? Constants.BuyColor : Constants.SellColor;
                }
                if (e.ColumnIndex == 6)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    //bool open = positionGrid[6, e.RowIndex].Value.ToString() == "开";

                    e.CellStyle.ForeColor = Color.Blue;
                }


            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }
        }
        void ctrlPosition_Load(object sender, EventArgs e)
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
            if (positionGrid.SelectedRows.Count > 0)
            {
                positionGrid.SelectedRows[0].Selected = false;
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
            tb.Rows[i][SIDESTR] = (pos.DirectionType == QSEnumPositionDirectionType.Long?"买":" 卖");



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

        /// 获得合约名称
        /// </summary>
        /// <param name="fill"></param>
        /// <returns></returns>
        string GetSymbolName(Position pos)
        {
            if (pos.oSymbol != null) return pos.oSymbol.GetName();
            return pos.Symbol;
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
                    tb.Rows[i][FLAG] = "投机";
                    tb.Rows[i][NAME] = GetSymbolName(pos);

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
        const string SIDE = "SIDE";
        const string SIDESTR = "方向";
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
            tb.Columns.Add(SIDESTR);
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

        void ResetColumeSize()
        {
            DataGridView grid = positionGrid;

            grid.Columns[SYMBOL].Width = 120;
            grid.Columns[SIDESTR].Width = 40;
            grid.Columns[PROPERTY].Width = 40;
            grid.Columns[SIZE].Width = 46;
            grid.Columns[SIZECANFLAT].Width = 46;

            grid.Columns[AVGPRICE].Width = 90;
            grid.Columns[UNREALIZEDPL].Width = 90;
            grid.Columns[LOSSTARGET].Width = 75;
            grid.Columns[PROFITTARGET].Width = 75;
            grid.Columns[FLAG].Width = 40;
            grid.Columns[NAME].Width = 120;



        }

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
            grid.Columns[SIDE].Visible = false;
            grid.Columns[SYMBOLKEY].Visible = false;
            
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
            poskeyRowIdxMap.Clear();
            positionGrid.DataSource = null;
            tb.Rows.Clear();
            BindToTable();
        }

        #endregion


    }
}
