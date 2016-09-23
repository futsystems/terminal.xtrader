using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.KryptonControl
{
    public partial class ctPositionViewSTK : UserControl,IEventBinder
    {
        ILog logger = LogManager.GetLogger("ctPositionViewSTK");
        public ctPositionViewSTK()
        {
            InitializeComponent();
            InitTable();
            BindToTable();

            this.positionGrid.SizeChanged += new EventHandler(positionGrid_SizeChanged);
            this.Load += new EventHandler(ctPositionViewSTK_Load);
        }

        void positionGrid_SizeChanged(object sender, EventArgs e)
        {
            ResetColumeSize();
        }

        void ctPositionViewSTK_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._realview)
                {
                    CoreService.EventCore.RegIEventHandler(this);
                    CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
                    //CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);
                    CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);

                    CoreService.EventOther.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                    CoreService.EventOther.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
                }

                positionGrid.Click += new EventHandler(positionGrid_Click);
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
                CoreService.TLClient.ReqXQryTickSnapShot(pos.Symbol);
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


        void positionGrid_Click(object sender, EventArgs e)
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
                CoreService.TLClient.ReqXQryTickSnapShot(pos.Symbol);
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
        string GetSymbolName(Position pos)
        {
            if (pos.oSymbol != null) return pos.oSymbol.GetName();
            return pos.Symbol;
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
            tb.Rows[i][ACCOUNT] = pos.Account;
            tb.Rows[i][SIDE] = pos.DirectionType == QSEnumPositionDirectionType.Long;//??
            tb.Rows[i][SYMBOL] = pos.Symbol;
            tb.Rows[i][SYMBOLNAME] = GetSymbolName(pos);


            if (!poskeyRowIdxMap.ContainsKey(key))
                poskeyRowIdxMap.TryAdd(key, i);

            return i;
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
                    int ydsize = pos.PositionDetailYdNew.Sum(p => p.Volume); //隔夜仓可平
                    int i = InsertNewRow(pos);
                    tb.Rows[i][TOTALSIZE] = pos.UnsignedSize;
                    tb.Rows[i][AVABILESIZE] = ydsize;
                    tb.Rows[i][FRONZENSIZE] = pos.UnsignedSize - ydsize;
                    tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);

                    tb.Rows[i][LASTPRICE] = pos.FormatPrice(pos.LastPrice);
                    tb.Rows[i][MARKETVALUE] = pos.CalcPositionMarketValue().ToFormatStr();
                    

                }
                else
                {
                    int ydsize = pos.PositionDetailYdNew.Sum(p => p.Volume); //隔夜仓可平
                    int i = posidx;
                    tb.Rows[i][TOTALSIZE] = pos.UnsignedSize;
                    tb.Rows[i][AVABILESIZE] = ydsize;
                    tb.Rows[i][FRONZENSIZE] = pos.UnsignedSize - ydsize;
                    tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);
                }
            }
            
        }


        /// <summary>
        /// 响应行情数据
        /// </summary>
        /// <param name="k"></param>
        public void GotTick(Tick k)
        {
            if (InvokeRequired)
            {
                Invoke(new TickDelegate(GotTick), new object[] { k });
            }
            else
            {
                try
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        if (tb.Rows[i][SYMBOL].ToString() == k.Symbol)
                        {
                            //记录该仓位所属账户 持仓方向 
                            string account = tb.Rows[i][ACCOUNT].ToString();
                            bool posside = bool.Parse(tb.Rows[i][SIDE].ToString());
                            Position pos = CoreService.TradingInfoTracker.PositionTracker[k.Symbol, account, posside];

                            decimal unrealizedpl = pos.UnRealizedPL;


                            //更新最新成交价
                            if (k.IsTrade())
                            {
                                tb.Rows[i][LASTPRICE] = pos.FormatPrice(k.Trade);

                            }

                            //更新浮动盈亏
                            if (pos.isFlat)
                            {
                                tb.Rows[i][POSITIONPROFIT] = 0;
                            }
                            else
                            {
                                tb.Rows[i][POSITIONPROFIT] = (unrealizedpl * pos.oSymbol.Multiple).ToFormatStr();
                                //计算市值
                                tb.Rows[i][MARKETVALUE] = pos.CalcPositionMarketValue().ToFormatStr();
                                tb.Rows[i][POSITIONPROFITPECT] = ((pos.CalcPositionMarketValue() - pos.CalcPositionCostValue()) * 100 / pos.CalcPositionCostValue()).ToFormatStr();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Got Tick Error:" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 响应成交数据
        /// </summary>
        /// <param name="f"></param>
        public void GotFill(Trade f)
        {
            if (InvokeRequired)
            {
                Invoke(new FillDelegate(GotFill), new object[] { f });
            }
            else
            {
                try
                {
                    Position pos = CoreService.TradingInfoTracker.PositionTracker[f.Symbol, f.Account, f.PositionSide];//获得对应持仓数据
                    string key = pos.GetPositionKey();
                    int posidx = PosiitonRowIdx(key);
                    if ((posidx > -1) && (posidx < tb.Rows.Count))
                    {
                        int ydsize = pos.PositionDetailYdNew.Sum(p => p.Volume); //隔夜仓可平
                        int i = posidx;
                        tb.Rows[i][TOTALSIZE] = pos.UnsignedSize;
                        tb.Rows[i][AVABILESIZE] = ydsize;
                        tb.Rows[i][FRONZENSIZE] = pos.UnsignedSize - ydsize;
                        tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);
                    }
                    else
                    {
                        int i = InsertNewRow(pos);
                        int ydsize = pos.PositionDetailYdNew.Sum(p => p.Volume); //隔夜仓可平

                        tb.Rows[i][TOTALSIZE] = pos.UnsignedSize;
                        tb.Rows[i][AVABILESIZE] = ydsize;
                        tb.Rows[i][FRONZENSIZE] = pos.UnsignedSize - ydsize;
                        tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);

                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Got Fill Error:" + ex.ToString());
                }
            }
        }



        const string POSKEY = "持仓键";
        const string ACCOUNT = "交易帐户";
        const string SIDE = "持仓方向";

        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";

        const string TOTALSIZE = "股票余额";
        const string AVABILESIZE = "可用余额";
        const string FRONZENSIZE = "冻结数量";

        const string POSITIONPROFIT = "盈亏";
        const string AVGPRICE = "成本价";
        const string POSITIONPROFITPECT = "盈亏比例(%)";
        const string LASTPRICE="市价";
        const string MARKETVALUE = "市值";
        const string EXCHANGE = "交易市场";



        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();

        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(POSKEY);
            tb.Columns.Add(ACCOUNT);
            tb.Columns.Add(SIDE);

            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(TOTALSIZE);
            tb.Columns.Add(AVABILESIZE);
            tb.Columns.Add(FRONZENSIZE);

            tb.Columns.Add(POSITIONPROFIT);
            tb.Columns.Add(AVGPRICE);
            tb.Columns.Add(POSITIONPROFITPECT);
            tb.Columns.Add(LASTPRICE);
            tb.Columns.Add(MARKETVALUE);
            tb.Columns.Add(EXCHANGE);

        }

        void ResetColumeSize()
        {
            DataGridView grid = positionGrid;
           
            grid.Columns[SYMBOL].Width = 100;
            grid.Columns[SYMBOLNAME].Width = 100;
            grid.Columns[TOTALSIZE].Width = 60;
            grid.Columns[AVABILESIZE].Width = 60;
            grid.Columns[FRONZENSIZE].Width = 60;

            grid.Columns[POSITIONPROFIT].Width = 80;
            grid.Columns[AVGPRICE].Width = 80;
            grid.Columns[LASTPRICE].Width = 80;
            grid.Columns[POSITIONPROFITPECT].Width = 80;
            grid.Columns[MARKETVALUE].Width = 120;




        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid = positionGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[POSKEY].Visible = false;
            grid.Columns[ACCOUNT].Visible = false;
            grid.Columns[SIDE].Visible = false;

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
            poskeyRowIdxMap.Clear();
            positionGrid.DataSource = null;
            tb.Rows.Clear();
            BindToTable();
        }
    }
}
