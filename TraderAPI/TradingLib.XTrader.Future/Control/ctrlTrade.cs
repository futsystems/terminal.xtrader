using System;
using System.Collections.Generic;
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
    public partial class ctrlTrade : UserControl,TradingLib.API.IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctrlPosition");
        FGrid tradeGrid = null;
        public ctrlTrade()
        {
            InitializeComponent();

            this.tradeGrid = new FGrid();
            this.tradeGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(tradeGrid);


            InitTable();
            BindToTable();

            WireEvent();
        }

        void WireEvent()
        {
            this.Load += new EventHandler(ctrlTrade_Load);
            tradeGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(tradeGrid_CellFormatting);
            btnDetail.CheckedChanged += new EventHandler(btnDetail_CheckedChanged);
            btnByOrder.CheckedChanged += new EventHandler(btnByOrder_CheckedChanged);
            btnBySymbol.CheckedChanged += new EventHandler(btnBySymbol_CheckedChanged);
            tradeGrid.MouseClick += new MouseEventHandler(tradeGrid_MouseClick);
        }

        void ctrlTrade_Load(object sender, EventArgs e)
        {
            if (this._realview)
            {
                CoreService.EventCore.RegIEventHandler(this);
                CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);

                CoreService.EventHub.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                CoreService.EventHub.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
            }
            tradeGrid.ClearSelection();
        }



        void tradeGrid_MouseClick(object sender, MouseEventArgs e)
        {
            int rowid = GetRowIndexAt(e.Y);
            if (rowid == -1)
            {
                if (tradeGrid.SelectedRows.Count > 0)
                {
                    tradeGrid.SetSelectedBackground(false);
                }
            }
            else
            {
                tradeGrid.SetSelectedBackground(true);
            }
        }

          
        int GetRowIndexAt(int mouseLocation_Y)
        {
            if (tradeGrid.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;  // no rows.   
            }
            if (tradeGrid.ColumnHeadersVisible == true && mouseLocation_Y <= tradeGrid.ColumnHeadersHeight)
            {
                return -1;
            }
            int index = tradeGrid.FirstDisplayedScrollingRowIndex;
            int displayedCount = tradeGrid.DisplayedRowCount(true);
            for (int k = 1; k <= displayedCount; )  // 因为行不能ReOrder，故只需要搜索显示的行   
            {
                if (tradeGrid.Rows[index].Visible == true)
                {
                    Rectangle rect = tradeGrid.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域   
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

        enum EnumTradeViewType
        { 
            Detail,
            ByOrder,
            BySymbol,
        }
        EnumTradeViewType _tradeViewType = EnumTradeViewType.Detail;

        void btnBySymbol_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBySymbol.Checked && _tradeViewType != EnumTradeViewType.BySymbol)
            {
                _tradeViewType = EnumTradeViewType.BySymbol;
                LoadTrade();
            }
        }

        void btnByOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (btnByOrder.Checked && _tradeViewType != EnumTradeViewType.ByOrder)
            {
                _tradeViewType = EnumTradeViewType.ByOrder;
                LoadTrade();
            }
        }

        void btnDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (btnDetail.Checked && _tradeViewType != EnumTradeViewType.Detail)
            {
                _tradeViewType = EnumTradeViewType.Detail;
                LoadTrade();
            }
        }

        void LoadTrade()
        {
            this.Clear();
            switch (_tradeViewType)
            {
                case EnumTradeViewType.Detail:
                    {
                        foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
                        {
                            this.GotFill(f);
                        }
                        break;
                    }
                case EnumTradeViewType.ByOrder:
                    {
                        Dictionary<string, Trade> fillmap = new Dictionary<string, Trade>();
                        foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
                        {
                            string key = string.Format("{0}-{1}", f.Symbol,f.id);
                            Trade target = null;
                            if (!fillmap.TryGetValue(key, out target))
                            {
                                target = new TradeImpl(f);
                                fillmap.Add(key, target);
                            }
                            else
                            {
                                decimal amount = Math.Abs(target.xSize) * target.xPrice + Math.Abs(f.xSize) * f.xPrice;
                                target.xSize += f.xSize;
                                target.xPrice = amount / Math.Abs(target.xSize);
                            }
                        }

                        foreach (var fill in fillmap.Values)
                        {
                            this.GotFill(fill);
                        }
                        break;
                    }
                case EnumTradeViewType.BySymbol:
                    {
                        Dictionary<string, Trade> fillmap = new Dictionary<string, Trade>();
                        foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
                        {
                            string key = string.Format("{0}-{1}-{2}", f.Symbol, f.Side, f.IsEntryPosition);
                            Trade target = null;
                            if (!fillmap.TryGetValue(key, out target))
                            {
                                target = new TradeImpl(f);
                                fillmap.Add(key, target);
                            }
                            else
                            {
                                decimal amount = Math.Abs(target.xSize) * target.xPrice + Math.Abs(f.xSize) * f.xPrice;
                                target.xSize += f.xSize;
                                target.xPrice = amount / Math.Abs(target.xSize);
                            }
                        }

                        foreach (var fill in fillmap.Values)
                        {
                            this.GotFill(fill);
                        }

                        break;
                    }
                default:
                    break;
            }
        }
        

        void tradeGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    bool side = false;
                    bool.TryParse(tradeGrid[2, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? Constants.BuyColor : Constants.SellColor;
                }
                if (e.ColumnIndex == 4)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool open = tradeGrid[4, e.RowIndex].Value.ToString() == "开";

                    e.CellStyle.ForeColor = open ? Color.Black : Color.Blue;
                }

            }
            catch (Exception ex)
            {
                logger.Error("cell format error:" + ex.ToString());
            }
        }

        

        void EventOther_OnResumeDataEnd()
        {
            foreach (var f in CoreService.TradingInfoTracker.TradeTracker)
            {
                this.GotFill(f);
            }
            tradeGrid.ClearSelection();
        }

        void EventOther_OnResumeDataStart()
        {
            this.Clear();
        }

        public void OnInit()
        {
            _tradeViewType = EnumTradeViewType.Detail;
            LoadTrade();

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
            if (fill.oSymbol != null) return fill.oSymbol.GetName(BrokerAPIConstants.IsLongSymbolName);
            return fill.Symbol;
        }

        string FormatPrice(Trade fill, decimal val)
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
            return Util.ToDateTime(t.xDate, t.xTime).ToString(_realview ? _realDT : _histDT);
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
                tb.Rows[i][DATETIME] = Util.ToTLDateTime(fill.xDate, fill.xTime);
                tb.Rows[i][TIME] = GetDateTime(fill);
                tb.Rows[i][SYMBOL] = fill.Symbol;
                tb.Rows[i][SIDE] = fill.Side;
                tb.Rows[i][SIDESTR] = fill.Side?"买":"卖";
                tb.Rows[i][FLAG] = (fill.IsEntryPosition ? "开" : " 平");
                tb.Rows[i][PRICE] = FormatPrice(fill, fill.xPrice);

                tb.Rows[i][SIZE] = Math.Abs(fill.xSize);
                tb.Rows[i][SPECULATE] = "投";
                tb.Rows[i][ORDERSYSID] = fill.OrderSysID;
                tb.Rows[i][NAME] = GetSymbolName(fill);
                tb.Rows[i][TRADERID] = fill.TradeID;
                

            }
        }

        #region 表格

        const string DATETIME = "DATEIME";
        const string TIME = "成交时间";
        const string SYMBOL = "合约";
        const string SIDE = "SIDE";
        const string SIDESTR = "买卖";
        const string FLAG = "开平";

        const string PRICE = "成交价格";
        const string SIZE = "手数";
        const string SPECULATE = "投保";
        const string ORDERSYSID = "委托号";
        const string NAME = "名称";
        const string TRADERID = "成交号";




        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            
            tb.Columns.Add(TIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SIDE);
            tb.Columns.Add(SIDESTR);
            tb.Columns.Add(FLAG);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(SPECULATE);
            tb.Columns.Add(ORDERSYSID);
            tb.Columns.Add(NAME);
            tb.Columns.Add(TRADERID);
            tb.Columns.Add(DATETIME);
        }

        void ResetColumeSize()
        {
            DataGridView grid = tradeGrid;

            grid.Columns[TIME].Width = 100;
            grid.Columns[SYMBOL].Width = 120;
            grid.Columns[SIDESTR].Width = 40;
            grid.Columns[FLAG].Width = 40;
            grid.Columns[PRICE].Width = 75;

            grid.Columns[SIZE].Width = 45;
            grid.Columns[SPECULATE].Width = 40;
            grid.Columns[ORDERSYSID].Width = 80;
            grid.Columns[NAME].Width = 120;
            grid.Columns[TRADERID].Width = 80;


            tradeGrid.CalcRowWidth();

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid = tradeGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[SIDE].Visible = false;
            grid.Columns[DATETIME].Visible = false;

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

        #endregion

    }
}
