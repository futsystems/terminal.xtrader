using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI; 

using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;



namespace TradingLib.TraderControl
{
    public partial class ctPositionView : UserControl,IEventBinder
    {
       
        ILog logger = LogManager.GetLogger("ctTradeView");

        BindingSource datasource = new BindingSource();

        //委托事务辅助,用于执行反手等功能
        OrderTransactionHelper _ordTransHelper;

        ////获得止盈参数
        //ProfitArgs getDefaultprofitargs(string symbol)
        //{
        //    if (LookUpProfitArgsEvent != null)
        //        return LookUpProfitArgsEvent(symbol);
        //    return null;
        //}
        ////获得止损参数
        //StopLossArgs getDefaultlossargs(string symbol)
        //{
        //    if (LookUpLossArgsEvent != null)
        //        return LookUpLossArgsEvent(symbol);
        //    return null;
        //}


        public ctPositionView()
        {
            InitializeComponent();
            SetPreferences();
            InitTable();
            BindToTable();

            //初始化止盈止损设置窗口
            //frmPosOffset = new frmPositionOffset();
            //frmPosOffset.TopMost = true;
            //frmPosOffset.UpdatePostionOffsetEvent += (PositionOffsetArgs args) =>
            //{
            //    if (UpdatePostionOffsetEvent != null)
            //        UpdatePostionOffsetEvent(args);
            //};

            CoreService.EventCore.RegIEventHandler(this);
            WireEvent();
            

            this.Load +=new EventHandler(ctPositionView_Load);

            btnShowHold.IsChecked = true;
        }

        void WireEvent()
        {
            CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);
            CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);

            positiongrid.CellFormatting +=new CellFormattingEventHandler(positiongrid_CellFormatting);
            positiongrid.DoubleClick +=new EventHandler(positiongrid_DoubleClick);
            positiongrid.Click += new EventHandler(positiongrid_Click);
            //如果再load中加载 则会加载2次
            this.btnFlat.Click += new EventHandler(btnFlat_Click);
            this.btnFlatAll.Click += new EventHandler(btnFlatAll_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);


        }


        bool load = false;
        private void ctPositionView_Load(object sender, EventArgs e)
        {
            if (!load)
            {
                load = true;
                
            }            
        }



        public void OnInit()
        {
            //恢复持仓数据
            foreach (var pos in CoreService.TradingInfoTracker.PositionTracker)
            {
                this.GotPosition(pos);
            }
        }

        public void OnDisposed()
        { 
        
        }











        #region 止盈止损参数

        //frmPositionOffset frmPosOffset;
        //止损参数映射
        ConcurrentDictionary<string, PositionOffsetArgs> lossOffsetMap = new ConcurrentDictionary<string, PositionOffsetArgs>();
        //止盈参数映射
        ConcurrentDictionary<string, PositionOffsetArgs> profitOffsetMap = new ConcurrentDictionary<string, PositionOffsetArgs>();


        PositionOffsetArgs GetLossArgs(string key)
        {
            if (lossOffsetMap.Keys.Contains(key))
                return lossOffsetMap[key];
            else
                return null;
        }

        PositionOffsetArgs GetProfitArgs(string key)
        {
            if (profitOffsetMap.Keys.Contains(key))
                return profitOffsetMap[key];
            else
                return null;
        }

        //设置止盈止损参数时,弹出窗体,然后按相应操作触发 参数提交的操作
        //服务端获得参数设置成功后,回传对应的参数

        void ResetOffset(string key)
        {
            PositionOffsetArgs p = GetProfitArgs(key);
            if (p != null)
                p.Enable = false;
            PositionOffsetArgs l = GetLossArgs(key);
            if (l != null)
                l.Enable = false;
        }

        //获得显示的数值
        string GetGridOffsetText(Position pos, QSEnumPositionOffsetDirection direction)
        {
            string key = pos.Account + "_" + pos.Symbol;
            decimal price = direction == QSEnumPositionOffsetDirection.LOSS ? GetLossArgs(key).TargetPrice(pos) : GetProfitArgs(key).TargetPrice(pos);
            if (price == -1)
            {
                return "停止";
            }
            if (price == 0)
            {
                return "无持仓";
            }
            else
            {
                return string.Format(TraderHelper.GetDisplayFormat(pos.oSymbol), price);
            }

        }

        #endregion


        //合约所对应的table id key为 account - symbol
        ConcurrentDictionary<string, int> symRowMap = new ConcurrentDictionary<string, int>();
        //通过account-symbol获得对应的tableid
        int positionidx(string acc_sym)
        {
            if (symRowMap.Keys.Contains(acc_sym))
                return symRowMap[acc_sym];
            return -1;
        }


        //获得某个持仓的可平数量
        int GetCanFlatSize(Position pos)
        {
            //return pos.isFlat ? 0 : (pos.UnsignedSize - _ot.GetPendingExitSize(pos.Symbol, pos.DirectionType == QSEnumPositionDirectionType.Long ? true : false));
            OrderTracker ot = CoreService.TradingInfoTracker.OrderTracker;
            return pos.isFlat ? 0 : (pos.UnsignedSize - ot.GetPendingExitSize(pos.Symbol,pos.DirectionType== QSEnumPositionDirectionType.Long?true:false));
        }

        /// <summary>
        /// 插入一条新的持仓对象
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int InsertNewRow(Position pos)
        {
            gt.Rows.Add(pos.Symbol);
            int i = gt.Rows.Count - 1;
            string key = pos.GetPositionKey();

            gt.Rows[i][SIDE] = pos.DirectionType == QSEnumPositionDirectionType.Long;
            gt.Rows[i][DIRECTION] = pos.DirectionType== QSEnumPositionDirectionType.Long ? "多" : "空";
            gt.Rows[i][KEY] = key;
            gt.Rows[i][ACCOUNT] = pos.Account;

            if (!symRowMap.ContainsKey(key))
                symRowMap.TryAdd(key, i);
            //同时为该key准备positoinoffsetarg
            //lossOffsetMap[key] = new PositionOffsetArgs(account, symbol, QSEnumPositionOffsetDirection.LOSS);
            //profitOffsetMap[key] = new PositionOffsetArgs(account, symbol, QSEnumPositionOffsetDirection.PROFIT);

            return i;
        }

        #region 辅助功能函数

        void FlatPosition(Position pos)
        {
            logger.Info("FlatPositon:" + pos.GetPositionKey());
            if (pos == null || pos.isFlat) return;
            Order o = new MarketOrderFlat(pos);
            CoreService.TLClient.ReqOrderInsert(o);
        }


        Dictionary<string, string> secFromatMap = new Dictionary<string, string>();
        //通过symbil获得对应的价格显示格式
        string GetDisplayFormat(Symbol sym)
        {
            if (sym == null)
                return UIConstant.DefaultDecimalFormat;
            if (secFromatMap.Keys.Contains(sym.Symbol))
                return secFromatMap[sym.Symbol];

            else
            {
                string f = TraderHelper.GetDisplayFormat(sym.SecurityFamily.PriceTick);
                secFromatMap.Add(sym.Symbol, f);
                return f;
            }
        }

        string GetDisplayFormat(string sym)
        {
            if (secFromatMap.Keys.Contains(sym))
                return secFromatMap[sym];
            return UIConstant.DefaultDecimalFormat;
        }

        //获得当前选中持仓
        Position CurrentPositoin
        {
            get
            {
                if (positiongrid.SelectedRows.Count > 0)
                {
                    string sym = positiongrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[SYMBOL].Value.ToString();
                    string account = positiongrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[ACCOUNT].Value.ToString();
                    bool side = bool.Parse(positiongrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[SIDE].Value.ToString());
                    
                    return CoreService.TradingInfoTracker.PositionTracker[sym, account,side];
                }
                else
                {
                    return null;
                }
            }
        }

        //获得当前选中合约
        string CurrentSymbol
        {
            get
            {
                if (positiongrid.SelectedRows.Count > 0)
                {
                    string sym = positiongrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[SYMBOL].Value.ToString();
                    return sym;
                }
                else
                {
                    return null;
                }
                
            }
        }

        //获得当前key account-symbol
        string CurrentKey
        {
            get
            {
                if (positiongrid.SelectedRows.Count > 0)
                {
                    string key = positiongrid.CurrentRow.Cells[KEY].Value.ToString();//positiongrid.SelectedRows[0].ViewInfo.CurrentRow.Cells[SYMBOL].Value.ToString();
                    return key;
                }
                else
                {
                    return "";
                }
            }
        }

        
        #endregion


        #region 相应服务端数据回报
        //获得昨日隔夜持仓，作为基数累加后得到当前持仓数据
        public void GotPosition(Position pos)
        {
            if (InvokeRequired)
            {
                Invoke(new PositionDelegate(GotPosition), new object[] { pos });
            }
            else
            {
                string _format = GetDisplayFormat(pos.oSymbol);
                int posidx = positionidx(pos.GetPositionKey());
                if (posidx == -1) //不存在对应的持仓
                {
                    int i = InsertNewRow(pos);
                    gt.Rows[i][SIZE] = Math.Abs(pos.Size);
                    gt.Rows[i][YDSIZE] = pos.PositionDetailYdNew.Sum(p => p.Volume); 
                    gt.Rows[i][CANFLATSIZE] = GetCanFlatSize(pos);
                    gt.Rows[i][AVGPRICE] = string.Format(_format, pos.AvgPrice);
                    gt.Rows[i][REALIZEDPL] = string.Format(UIConstant.DefaultDecimalFormat, pos.ClosedPL * pos.oSymbol.Multiple);
                }
                else
                {
                    int i = posidx;
                    gt.Rows[i][SIZE] = Math.Abs(pos.Size);
                    gt.Rows[i][YDSIZE] = pos.PositionDetailYdNew.Sum(p => p.Volume);
                    gt.Rows[i][CANFLATSIZE] = GetCanFlatSize(pos);
                    gt.Rows[i][AVGPRICE] = string.Format(_format, pos.AvgPrice);
                    gt.Rows[i][REALIZEDPL] = string.Format(UIConstant.DefaultDecimalFormat, pos.ClosedPL * pos.oSymbol.Multiple);
                }
            }

        }

        public void GotTick(Tick t)
        {
            if (InvokeRequired)
            {
                Invoke(new TickDelegate(GotTick), new object[] { t });
            }
            else
            {
                try
                {
                    string _fromat = GetDisplayFormat(t.Symbol);
                    //数据列中如果是该symbol则必须全比更新
                    for (int i = 0; i < gt.Rows.Count; i++)
                    {
                        if (gt.Rows[i][SYMBOL].ToString() == t.Symbol)
                        {
                            //记录该仓位所属账户
                            string acc = gt.Rows[i][ACCOUNT].ToString();
                            bool posside = bool.Parse(gt.Rows[i][SIDE].ToString());
                            Position pos = CoreService.TradingInfoTracker.PositionTracker[t.Symbol, acc, posside];
                            string key = pos.GetPositionKey();
                            //logger.Debug("tick symbol:" + t.Symbol + " key:" + key.ToString());
                            decimal unrealizedpl = pos.UnRealizedPL;

                             //更新最新成交价
                            if (t.isTrade)
                            {
                                gt.Rows[i][LASTPRICE] = string.Format(_fromat, t.Trade);
                            }

                            //更新浮动盈亏
                            if (pos.isFlat)
                            {
                                gt.Rows[i][UNREALIZEDPL] = 0;
                            }
                            else
                            {
                                gt.Rows[i][UNREALIZEDPL] = string.Format(_fromat, unrealizedpl * pos.oSymbol.Multiple);
                            }

                            //更新止盈止损数值
                            //gt.Rows[i][STOPLOSS] = GetGridOffsetText(pos, QSEnumPositionOffsetDirection.LOSS);
                            //gt.Rows[i][PROFITTARGET] = GetGridOffsetText(pos, QSEnumPositionOffsetDirection.PROFIT);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("GotTick Error:" + ex.ToString());
                }
            }
        }


        /// <summary>
        /// 获得委托数据回报 用于更新可平数量
        /// </summary>
        /// <param name="o"></param>
        public void GotOrder(Order o)
        {
            //当有委托近来时候,我们需要重新计算我们所对应的可以平仓数量
            if (InvokeRequired)
            {
                Invoke(new OrderDelegate(GotOrder), new object[] { o });
            }
            else
            {
                try
                {
                    Position pos = CoreService.TradingInfoTracker.PositionTracker[o.Symbol, o.Account, o.PositionSide];
                    //通过account_symbol键对找到对应的行
                    int posidx = positionidx(pos.GetPositionKey());
                    //string _fromat = GetDisplayFormat(pos.oSymbol);
                    if ((posidx > -1) && (posidx < gt.Rows.Count))
                    {
                        gt.Rows[posidx][CANFLATSIZE] = GetCanFlatSize(pos);
                        //updateCurrentRowPositionNum();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("GotOrder Error:" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获得委托取消回报 获得委托取消回报 用于更新可平数量
        /// </summary>
        /// <param name="oid"></param>
        //public void GotCancel(long oid)
        //{
        //    //当有委托近来时候,我们需要重新计算我们所对应的可以平仓数量
        //    if (InvokeRequired)
        //    {
        //        Invoke(new LongDelegate(GotCancel), new object[] { oid });
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Order o = CoreService.TradingInfoTracker.OrderTracker.SentOrder(oid);
        //            if (o == null || !o.isValid) return;
        //            Position pos = CoreService.TradingInfoTracker.PositionTracker[o.Symbol, o.Account, o.PositionSide];
        //            //通过account_symbol键对找到对应的行
        //            int posidx = positionidx(pos.GetPositionKey());
        //            //string _fromat = getDisplayFormat(pos.Symbol);
        //            if ((posidx > -1) && (posidx < gt.Rows.Count))
        //            {
        //                gt.Rows[posidx][CANFLATSIZE] = GetCanFlatSize(pos);
        //                //updateCurrentRowPositionNum();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error("GotCancel Error:" + ex.ToString());
        //        }
        //    }
        //}

        public void GotFill(Trade t)
        {
            if (InvokeRequired)
            {
                Invoke(new FillDelegate(GotFill), new object[] { t });
            }
            else
            {
                try
                {
                    Position pos = CoreService.TradingInfoTracker.PositionTracker[t.Symbol, t.Account, t.PositionSide];//获得对应持仓数据
                    string key = pos.GetPositionKey();
                    int posidx = positionidx(key);
                    if ((posidx > -1) && (posidx < gt.Rows.Count))
                    {
                        int size = pos.Size;
                        gt.Rows[posidx][SIZE] = Math.Abs(size);
                        gt.Rows[posidx][YDSIZE] = pos.PositionDetailYdNew.Sum(p => p.Volume);

                        gt.Rows[posidx][CANFLATSIZE] = GetCanFlatSize(pos);
                        gt.Rows[posidx][AVGPRICE] = string.Format(TraderHelper.GetDisplayFormat(pos.oSymbol), pos.AvgPrice);
                        gt.Rows[posidx][REALIZEDPL] = string.Format(UIConstant.DefaultDecimalFormat, pos.ClosedPL * pos.oSymbol.Multiple);

                        if (pos.isFlat)
                        {
                            ResetOffset(key);
                        }
                        //gt.Rows[posidx][STOPLOSS] = GetGridOffsetText(pos, QSEnumPositionOffsetDirection.LOSS);
                        //gt.Rows[posidx][PROFITTARGET] = GetGridOffsetText(pos, QSEnumPositionOffsetDirection.PROFIT);

                        //updateCurrentRowPositionNum();
                    }
                    else
                    {
                        int i = InsertNewRow(pos);
                        gt.Rows[i][SIZE] = Math.Abs(pos.Size);
                        gt.Rows[i][CANFLATSIZE] = GetCanFlatSize(pos);
                        gt.Rows[i][AVGPRICE] = string.Format(TraderHelper.GetDisplayFormat(pos.oSymbol), pos.AvgPrice);
                        gt.Rows[i][REALIZEDPL] = string.Format(UIConstant.DefaultDecimalFormat, pos.ClosedPL * pos.oSymbol.Multiple);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("GotFill Error:" + ex.ToString());
                }
            }
            
        }

        #endregion


        #region 表格


        const string SYMBOL = "合约";
        const string SIDE = "方向Token";
        const string DIRECTION = "方向";
        const string SIZE = "总持仓";
        const string YDSIZE = "昨持仓";
        const string CANFLATSIZE = "可平量";//用于计算当前限价委托可以挂单数量
        const string LASTPRICE = "最新";//最新成交价
        const string AVGPRICE = "持仓均价";
        const string UNREALIZEDPL = "浮盈";
        const string REALIZEDPL = "平盈";
        const string ACCOUNT = "账户";
        const string PROFITTARGET = "止盈";
        const string STOPLOSS = "止损";
        const string KEY = "编号";
        //const string STRATEGY = "出场策略";
        DataTable gt = new DataTable();


        /// <summary>
        /// 设定表格控件的属性
        /// </summary>
        private void SetPreferences()
        {
            Telerik.WinControls.UI.RadGridView grid = positiongrid;
            grid.ShowRowHeaderColumn = false;//显示每行的头部
            grid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;//列的填充方式
            grid.ShowGroupPanel = false;//是否显示顶部的panel用于组合排序
            grid.MasterTemplate.EnableGrouping = false;//是否允许分组
            grid.EnableHotTracking = true;
            //this.radRadioDataReader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On; 

            grid.AllowAddNewRow = false;//不允许增加新行
            grid.AllowDeleteRow = false;//不允许删除行
            grid.AllowEditRow = false;//不允许编辑行
            grid.AllowRowResize = false;
            grid.EnableSorting = false;
            grid.TableElement.TableHeaderHeight = UIConstant.HeaderHeight;
            grid.TableElement.RowHeight = UIConstant.RowHeight;

            grid.EnableAlternatingRowColor = true;//隔行不同颜色
        }
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            gt.Columns.Add(SYMBOL);
            gt.Columns.Add(SIDE);
            gt.Columns.Add(DIRECTION);
            gt.Columns.Add(SIZE, typeof(int));
            gt.Columns.Add(YDSIZE, typeof(int));
            gt.Columns.Add(CANFLATSIZE, typeof(int));
            gt.Columns.Add(LASTPRICE);
            gt.Columns.Add(AVGPRICE);
            gt.Columns.Add(UNREALIZEDPL);
            gt.Columns.Add(REALIZEDPL);
            gt.Columns.Add(ACCOUNT);
            gt.Columns.Add(PROFITTARGET);
            gt.Columns.Add(STOPLOSS);
            gt.Columns.Add(KEY);
            //gt.Columns.Add(STRATEGY, typeof(Image));

        }
        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            Telerik.WinControls.UI.RadGridView grid = positiongrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = gt;
            grid.DataSource = datasource;
            grid.Columns[ACCOUNT].IsVisible = false;
            grid.Columns[KEY].IsVisible = false;
            grid.Columns[SIDE].IsVisible = false;

            //set width
            grid.Columns[SYMBOL].Width = 60;
            grid.Columns[SYMBOL].TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns[DIRECTION].Width = 45;
            grid.Columns[DIRECTION].TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns[SIZE].Width = 45;
            grid.Columns[SIZE].TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns[CANFLATSIZE].Width = 45;
            grid.Columns[CANFLATSIZE].TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns[LASTPRICE].Width = 65;
            grid.Columns[LASTPRICE].TextAlignment = ContentAlignment.MiddleRight;
            grid.Columns[AVGPRICE].Width = 65;
            grid.Columns[AVGPRICE].TextAlignment = ContentAlignment.MiddleRight;
            grid.Columns[UNREALIZEDPL].Width = 65;
            grid.Columns[UNREALIZEDPL].TextAlignment = ContentAlignment.MiddleRight;
            grid.Columns[REALIZEDPL].Width = 65;
            grid.Columns[REALIZEDPL].TextAlignment = ContentAlignment.MiddleRight;
            grid.Columns[PROFITTARGET].Width = 75;
            grid.Columns[PROFITTARGET].TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns[STOPLOSS].Width = 75;
            grid.Columns[STOPLOSS].TextAlignment = ContentAlignment.MiddleCenter;
            

        }



        private void positiongrid_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement.RowInfo is GridViewDataRowInfo)
                {
                    if (e.CellElement.ColumnInfo.Name == DIRECTION)
                    {
                        object direction = e.CellElement.RowInfo.Cells[DIRECTION].Value;
                        if (direction.ToString().Equals("看多"))
                        {
                            e.CellElement.ForeColor = UIConstant .LongSideColor;
                            e.CellElement.Font = UIConstant.BoldFont;
                        }
                        else if (direction.ToString().Equals("看空"))
                        {
                            e.CellElement.ForeColor = UIConstant.ShortSideColor;
                            e.CellElement.Font = UIConstant.BoldFont;
                        }
                        else
                        {
                            e.CellElement.ForeColor = Color.Black;
                            e.CellElement.Font = UIConstant.BoldFont;
                        }
                    }
                    else if (e.CellElement.ColumnInfo.Name == REALIZEDPL)
                    {
                        decimal v = decimal.Parse(e.CellElement.RowInfo.Cells[REALIZEDPL].Value.ToString());
                        if (v > 0)
                        {
                            e.CellElement.ForeColor = UIConstant.LongSideColor;
                        }
                        else if (v < 0)
                        {
                            e.CellElement.ForeColor = UIConstant.ShortSideColor;
                        }
                        else
                        {
                            e.CellElement.ForeColor = Color.Black;
                        }
                    }
                    else if (e.CellElement.ColumnInfo.Name ==UNREALIZEDPL)
                    {
                        decimal v = decimal.Parse(e.CellElement.RowInfo.Cells[UNREALIZEDPL].Value.ToString());
                        if (v > 0)
                        {
                            e.CellElement.ForeColor = UIConstant.LongSideColor;
                        }
                        else if (v < 0)
                        {
                            e.CellElement.ForeColor = UIConstant.ShortSideColor;
                        }
                        else
                        {
                            e.CellElement.ForeColor = Color.Black;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error("positionview cellformat error:" + ex.ToString());
            }
        }

        #endregion



        #region 界面操作事件
        private void btnShowAll_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            string strFilter = "";
            datasource.Filter = strFilter;
        }

        private void btnShowHold_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            string strFilter;
            strFilter = String.Format(SIZE + " > '{0}'","0");
            datasource.Filter = strFilter;
        }
        //平掉当前选中持仓
        private void btnFlat_Click(object sender, EventArgs e)
        {
            //TraderHelper.WindowMessage("btnflat operation");
            Position pos = CurrentPositoin;
            if (pos == null)
            {
                TraderHelper.WindowMessage("请选择持仓");
                return;
            }
            if (pos.isFlat)
            {
                TraderHelper.WindowMessage(string.Format("持仓:{0}无可平持仓",pos.GetPositionKey()));
                return;
            }
            FlatPosition(CurrentPositoin);
            
        }
        //平调所有持仓
        private void btnFlatAll_Click(object sender, EventArgs e)
        {
            //TraderHelper.WindowMessage("flat all");
            foreach (Position pos in CoreService.TradingInfoTracker.PositionTracker)
            {
                if (!pos.isFlat)
                {
                    FlatPosition(pos);
                }
            }
        }
        //撤单
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string sym = CurrentSymbol;
            if (string.IsNullOrEmpty(sym))
            {
                TraderHelper.WindowMessage("请选中持仓");
                return;
            }
            foreach (Order o in CoreService.TradingInfoTracker.OrderTracker)
            {
                if ((o.Symbol == sym) && (o.IsPending()))
                {
                    CoreService.TLClient.ReqCancelOrder(o.id);
                }
            }
        }
        //双击持仓某行
        //1.修改止盈止损参数
        private void positiongrid_DoubleClick(object sender, EventArgs e)
        {
            
            //int rownum = positiongrid.CurrentRow.Index+1;
            ////止盈 止损设置
            //if (positiongrid.CurrentColumn.Name == STOPLOSS)
            //{
            //    //MessageBox.Show("stoploss edit");
            //    //frmPositionOffset fm = new frmPositionOffset();
            //    //fm.TopMost = true;
            //    frmPosOffset.ParseOffset(GetLossArgs(CurrentKey),CurrentPositoin,findSecurity(CurrentSymbol));
            //    Point p = this.PointToScreen(positiongrid.Location);
            //    p.X = p.X + 250;
            //    p.Y = p.Y + UIGlobals.HeaderHeight + UIGlobals.RowHeight * rownum;
            //    frmPosOffset.Location = p;
            //    frmPosOffset.Show();
            //    return;
            //}
            //if (positiongrid.CurrentColumn.Name == PROFITTARGET)
            //{
            //    //frmPositionOffset fm = new frmPositionOffset();
            //    //fm.TopMost = true;
            //    frmPosOffset.ParseOffset(GetProfitArgs(CurrentKey), CurrentPositoin, findSecurity(CurrentSymbol));
            //    Point p = this.PointToScreen(positiongrid.Location);
            //    p.X = p.X + 250;
            //    p.Y = p.Y + UIGlobals.HeaderHeight + UIGlobals.RowHeight * rownum;
            //    frmPosOffset.Location = p;
            //    frmPosOffset.Show();
            //    return;
            //}

            ////非止盈止损列的双击并且设定为双击平仓,平调所选持仓
            if (isDoubleFlat.Checked)
            {
                FlatPosition(CurrentPositoin);
            }
            else
            { 
            
            }

        }

        /// <summary>
        /// 表格单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void positiongrid_Click(object sender, EventArgs e)
        {
            Position pos = CurrentPositoin;
            if (pos == null) return;
            CoreService.EventUI.FireSymbolselectedEvent(this,pos.oSymbol);

        }


        #endregion

        private void positiongrid_EditorRequired(object sender, EditorRequiredEventArgs e)
        {

        }
        // Fires when the cell changes its value 
        private void positiongrid_CellValueChanged(object sender, GridViewCellEventArgs e)
        {

        }
        // Fires when the editor changes its value. The value is stored only inside the editor. 
        private void positiongrid_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("eidtor value changed");
            //StopLossEditor edit = sender as StopLossEditor;
            //if (edit != null)
            //{
            //    GridCellElement cellstop = positiongrid.TableElement.GetCellElement(positiongrid.CurrentRow, positiongrid.Columns[STOPLOSS]);
            //    if (cellstop != null)
            //    {
            //        cellstop.Text = "i am here";
            //    }
            //}
        }

        

        

        


    }
}
