using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

            btnFlat.Click += new EventHandler(btnFlat_Click);
            btnFlatAll.Click += new EventHandler(btnFlatAll_Click);
            btnReserve.Click += new EventHandler(btnReserve_Click);
            btnLockPos.Click += new EventHandler(btnLockPos_Click);
            
            positionGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(positionGrid_CellFormatting);
            positionGrid.MouseClick += new MouseEventHandler(positionGrid_MouseClick);
            positionGrid.MouseDoubleClick += new MouseEventHandler(positionGrid_MouseDoubleClick);
            CoreService.EventHub.OnResumeDataStart += new Action(EventHub_OnResumeDataStart);
            CoreService.EventHub.OnResumeDataEnd += new Action(EventHub_OnResumeDataEnd);
        }

        void EventHub_OnResumeDataEnd()
        {
            btnFlat.Enabled = true;
            btnFlatAll.Enabled = true;
            btnReserve.Enabled = true;
            btnLockPos.Enabled = true;
        }

        void EventHub_OnResumeDataStart()
        {
            btnFlat.Enabled = false;
            btnFlatAll.Enabled = false;
            btnReserve.Enabled = false;
            btnLockPos.Enabled = false;

        }

        void btnLockPos_Click(object sender, EventArgs e)
        {
            Position pos = this.CurrentPositoin;
            if (pos == null)
            {
                MessageBox.Show("请选择持仓");
                return;
            }
            if (pos.isFlat)
            {
                MessageBox.Show("持仓数量为零");
                return;
            }
            LockPosition(pos);
        }

        void btnReserve_Click(object sender, EventArgs e)
        {
            Position pos = this.CurrentPositoin;
            if (pos == null)
            {
                MessageBox.Show("请选择持仓");
                return;
            }
            if(pos.isFlat)
            {
                MessageBox.Show("持仓数量为零，无可平持仓");
                return;
            }

            tasklist.Add(ExTask.CreateReserveTask(pos));
            System.Threading.ThreadPool.QueueUserWorkItem((o) => { ProcessTask(); });
        }



        ThreadSafeList<ExTask> tasklist = new ThreadSafeList<ExTask>();
        void ProcessTask()
        {
            while (tasklist.Count > 0)
            {
                List<ExTask> removelist = new List<ExTask>();
                foreach (var task in tasklist)
                {
                    switch (task.TaskType)
                    {
                        case EnumExTaskType.TaskReserve:
                            {
                                if (task.FlatSentCount == 0)
                                {
                                    this.FlatPosition(task.Position);
                                    task.FlatSentCount = 1;
                                    task.StartTime = DateTime.Now;
                                }
                                else
                                {
                                    if (task.Position.Size == 0 && task.ReverseSentcount == 0)
                                    {
                                        CoreService.TLClient.ReqOrderInsert(task.Order);
                                        task.ReverseSentcount = 1;
                                    }

                                }

                                if (task.FlatSentCount > 0 && task.ReverseSentcount > 0)
                                {
                                    bool side = task.OrigSize > 0 ? false : true;
                                    Position newPos = CoreService.TradingInfoTracker.PositionTracker[task.Position.Symbol, task.Position.Account, side];
                                    if (newPos != null && newPos.Size + task.OrigSize == 0)
                                    {
                                        logger.Info("反手任务执行成功");
                                        removelist.Add(task);
                                    }
                                }
                                if (DateTime.Now.Subtract(task.StartTime).TotalSeconds > 10)
                                {
                                    logger.Info("反手任务执行超时");
                                    removelist.Add(task);
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }

                foreach (var task in removelist)
                {
                    tasklist.Remove(task);
                }

                System.Threading.Thread.Sleep(200);
            }
        }

        void btnFlatAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认平掉所有持仓?", "确认全平", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                    (object o) =>
                    {
                        foreach (Position pos in CoreService.TradingInfoTracker.PositionTracker)
                        {
                            DateTime now = DateTime.Now;
                            if (!pos.isFlat)
                            {
                                while (!CoreService.TLClient.LastOrderNotified)
                                {
                                    System.Threading.Thread.Sleep(200);
                                    if (DateTime.Now.Subtract(now).TotalSeconds > 5)
                                    {
                                        MessageBox.Show("上次提交委托未回报，请重启交易端，避免重复下单");
                                        return;
                                    }
                                }

                                FlatPosition(pos);
                            }
                        }
                    }
                ));
            }
        }

        void btnFlat_Click(object sender, EventArgs e)
        {
            Position pos = this.CurrentPositoin;
            if (pos == null)
            {
                MessageBox.Show("请选择持仓");
                return;
            }
            this.FlatPosition(pos);
        }

        void FlatPosition(Position pos)
        {
            logger.Info("FlatPositon:" + pos.GetPositionKey());
            if (pos == null || pos.isFlat) return;

            bool side = pos.isLong ? true : false;
            //上期所区分平今平昨
            if (pos.oSymbol.SecurityFamily.Exchange.EXCode == "SHFE")
            {
                int voltd = pos.PositionDetailTodayNew.Sum(p => p.Volume);//今日持仓
                int volyd = pos.PositionDetailYdNew.Sum(p => p.Volume);//昨日持仓
                //Tick snapshot = TLCtxHelper.ModuleDataRouter.GetTickSnapshot(pos.Account);
                if (volyd != 0)
                {
                    Order oyd = new OrderImpl(pos.Symbol, volyd * (side ? 1 : -1) * -1);
                    oyd.OffsetFlag = QSEnumOffsetFlag.CLOSE;

                    CoreService.TLClient.ReqOrderInsert(oyd);
                }
                if (voltd != 0)
                {
                    Order otd = new OrderImpl(pos.Symbol, voltd * (side ? 1 : -1) * -1);
                    otd.OffsetFlag = QSEnumOffsetFlag.CLOSETODAY;

                    CoreService.TLClient.ReqOrderInsert(otd);
                }
            }
            else
            {
                if (!CoreService.TLClient.LastOrderNotified)
                {
                    MessageBox.Show("上次提交委托未回报，请重启交易端，避免重复下单");
                    return;
                }

                Order o = new MarketOrderFlat(pos);
                o.Symbol = pos.oSymbol.Symbol;
                o.Exchange = pos.oSymbol.Exchange;
                CoreService.TLClient.ReqOrderInsert(o);
            }
        }

        void LockPosition(Position pos)
        {
            if (!CoreService.TLClient.LastOrderNotified)
            {
                MessageBox.Show("上次提交委托未回报，请重启交易端，避免重复下单");
                return;
            }

            logger.Info("LockPositon:" + pos.GetPositionKey());
            if (pos == null || pos.isFlat) return;

            bool side = pos.isLong ? true : false;

            Order o = new MarketOrder(pos.Symbol,pos.Size *-1);
            o.Exchange = pos.oSymbol.Exchange;
            o.OffsetFlag = QSEnumOffsetFlag.OPEN;
            CoreService.TLClient.ReqOrderInsert(o);
            
        }



        void positionGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Position pos = CurrentPositoin;
            if (pos != null)
            {
                //UIService.EventUI.FireSymbolSelectedEvent(this, pos.oSymbol);
                UIService.EventUI.FirePositionSelectedEvent(this, pos);
            }
        }

        void positionGrid_MouseClick(object sender, MouseEventArgs e)
        {
            int rowid = GetRowIndexAt(e.Y);
            if (rowid == -1)
            {
                if (positionGrid.SelectedRows.Count > 0)
                {
                    positionGrid.SetSelectedBackground(false);
                }
            }
            else
            {
                positionGrid.SetSelectedBackground(true);
            }

            if (rowid != -1)
            {
                Position current_pos = CurrentPositoin;
                if (current_pos != null)
                {
                    int rownum = positionGrid.CurrentRow.Index + 1;
                    if (positionGrid.CurrentCell.ColumnIndex == 14)
                    {
                        PositionOffsetArgSet args = CoreService.PositionWatcher.GetPositionOffsetArgs(current_pos);
                        if (args == null) return;

                        frmPositionOffset fm = new frmPositionOffset();
                        fm.Height = 180;
                        fm.TopMost = true;
                        fm.ParseOffset(current_pos, args.LossArg);
                        Point p = this.PointToScreen(positionGrid.Location);
                        p.X = p.X + 300;
                        p.Y = p.Y - 200;
                        fm.Location = p;
                        fm.Show();
                        return;
                    }
                    else if (positionGrid.CurrentCell.ColumnIndex == 15)
                    {
                        PositionOffsetArgSet args = CoreService.PositionWatcher.GetPositionOffsetArgs(current_pos);
                        if (args == null) return;

                        frmPositionOffset fm = new frmPositionOffset();
                        fm.Height = 180;
                        fm.TopMost = true;
                        fm.ParseOffset(current_pos, args.ProfitArg);
                        Point p = this.PointToScreen(positionGrid.Location);
                        p.X = p.X + 300;
                        p.Y = p.Y - 200;
                        fm.Location = p;
                        fm.Show();
                        return;
                    }
                    else
                    {
                        UIService.EventUI.FireSymbolSelectedEvent(this, current_pos.oSymbol);
                    }
                }
            }
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
                if (e.ColumnIndex == 6)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    bool side = false;
                    bool.TryParse(positionGrid[5, e.RowIndex].Value.ToString(), out side);
                    e.CellStyle.ForeColor = side ? Constants.BuyColor : Constants.SellColor;
                }
                if (e.ColumnIndex == 7)
                {
                    //e.CellStyle.Font = UIConstant.BoldFont;
                    //bool open = positionGrid[6, e.RowIndex].Value.ToString() == "开";

                    e.CellStyle.ForeColor = Color.Blue;
                }

                if (e.ColumnIndex == 11 || e.ColumnIndex == 12)
                {
                    decimal val = 0;
                    if (!decimal.TryParse(positionGrid[12, e.RowIndex].Value.ToString(), out val)) val = 0;
                    if (val == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (val > 0)
                    {
                        e.CellStyle.ForeColor = Constants.BuyColor;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Constants.SellColor;
                    }

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

                    CoreService.EventIndicator.GotTickEvent += new Action<Tick>(GotTick);
                    CoreService.EventIndicator.GotFillEvent += new Action<Trade>(GotFill);
                    CoreService.EventIndicator.GotOrderEvent += new Action<Order>(GotOrder);

                    CoreService.EventHub.OnResumeDataStart += new Action(EventOther_OnResumeDataStart);
                    CoreService.EventHub.OnResumeDataEnd += new Action(EventOther_OnResumeDataEnd);
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
            }

            positionGrid.ClearSelection();
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
            UIService.EventUI.FireSymbolSelectedEvent(this, pos.oSymbol);
        }


        public void OnInit()
        {
            //恢复持仓数据
            foreach (var pos in CoreService.TradingInfoTracker.PositionTracker)
            {
                this.GotPosition(pos);
            }
            positionGrid.ClearSelection();
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


        string GetSymbolTitle(Position pos)
        {
            if (pos.oSymbol != null) return pos.oSymbol.GetSymbolTitle();
            return pos.Symbol;
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
            tb.Rows[i][SYMBOLKEY] = pos.oSymbol.GetUniqueKey();
            tb.Rows[i][SYMBOL] = pos.Symbol;
            tb.Rows[i][SYMBOLTITLE] = GetSymbolTitle(pos);
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
            if (pos.oSymbol != null) return pos.oSymbol.GetTitleName(Constants.SymbolNameStyle == 0);
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

                    tb.Rows[i][UNREALIZEDPL] = 0;
                    tb.Rows[i][UNREALIZEDPOINT] = 0;
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
                    Position pos = CoreService.TradingInfoTracker.PositionTracker[f.Symbol,f.Account, f.PositionSide];//获得对应持仓数据

                    PositionOffsetArgSet args = CoreService.PositionWatcher.GetPositionOffsetArgs(pos);
                    if (args.ProfitArg.Size > pos.UnsignedSize) args.ProfitArg.Size = pos.UnsignedSize;
                    if (args.LossArg.Size > pos.UnsignedSize) args.LossArg.Size = pos.UnsignedSize;

                    string key = pos.GetPositionKey();
                    int posidx = PosiitonRowIdx(key);
                    if ((posidx > -1) && (posidx < tb.Rows.Count))
                    {
                        int i = posidx;
                        tb.Rows[i][SIZE] = pos.UnsignedSize;
                        tb.Rows[i][SIZECANFLAT] = GetCanFlatSize(pos); ;
                        tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);

                        //更新浮动盈亏
                        if (pos.isFlat)
                        {
                            tb.Rows[i][UNREALIZEDPL] = 0;
                        }
                        else
                        {
                            tb.Rows[i][UNREALIZEDPL] = (pos.UnRealizedPL * pos.oSymbol.Multiple).ToFormatStr();
                        }

                        tb.Rows[i][LOSSTARGET] = "";
                        tb.Rows[i][PROFITTARGET] = "";
                        tb.Rows[i][FLAG] = "投机";
                        tb.Rows[i][NAME] = GetSymbolName(pos);

                    }
                    else
                    {
                        int i = InsertNewRow(pos);
                        tb.Rows[i][SIZE] = pos.UnsignedSize;
                        tb.Rows[i][SIZECANFLAT] = GetCanFlatSize(pos); ;
                        tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);
                        tb.Rows[i][NAME] = GetSymbolName(pos);
                        //更新浮动盈亏
                        if (pos.isFlat)
                        {
                            tb.Rows[i][UNREALIZEDPL] = 0;
                            tb.Rows[i][UNREALIZEDPLACCTCURRENCY] = 0;
                            tb.Rows[i][UNREALIZEDPOINT] = 0;
                        }
                        else
                        {
                            tb.Rows[i][UNREALIZEDPL] = (pos.UnRealizedPL * pos.oSymbol.Multiple).ToFormatStr() + " " + pos.oSymbol.SecurityFamily.Currency.ToString(); ;
                            tb.Rows[i][UNREALIZEDPOINT] = pos.UnRealizedPL.ToFormatStr();
                            tb.Rows[i][UNREALIZEDPLACCTCURRENCY] = (pos.UnRealizedPL * pos.oSymbol.Multiple*CoreService.TradingInfoTracker.Account.GetExchangeRate(pos.oSymbol.SecurityFamily.Currency)).ToFormatStr();

                            //更新止盈止损数值
                            string lossstr = GetGridPositionOffsetText(pos, QSEnumPositionOffsetDirection.LOSS);
                            string profitstr = GetGridPositionOffsetText(pos, QSEnumPositionOffsetDirection.PROFIT);
                            tb.Rows[i][LOSSTARGET] = lossstr;
                            tb.Rows[i][PROFITTARGET] = profitstr;
                        }

                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Got Fill Error:" + ex.ToString());
                }
            }
        }

        void GotOrder(Order order)
        {
            if (InvokeRequired)
            {
                Invoke(new OrderDelegate(GotOrder), new object[] { order });
            }
            else
            {
                try
                {
                    Position pos = CoreService.TradingInfoTracker.PositionTracker[order.Symbol,order.Account, order.PositionSide];//获得对应持仓数据
                    
                    string key = pos.GetPositionKey();
                    int posidx = PosiitonRowIdx(key);
                    if ((posidx > -1) && (posidx < tb.Rows.Count))
                    {
                        int i = posidx;
                        tb.Rows[i][SIZE] = pos.UnsignedSize;
                        tb.Rows[i][SIZECANFLAT] = GetCanFlatSize(pos); ;
                        tb.Rows[i][AVGPRICE] = pos.FormatPrice(pos.AvgPrice);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Got Order Error:" + ex.ToString());
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
                    if (k.UpdateType != "X" && k.UpdateType !="S") return;

                    //logger.Info("tick:" + k.ToString());
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        if (tb.Rows[i][SYMBOLKEY].ToString() == k.GetSymbolUniqueKey())
                        {
                            //记录该仓位所属账户 持仓方向 
                            string account = tb.Rows[i][ACCOUNT].ToString();
                            bool posside = bool.Parse(tb.Rows[i][SIDE].ToString());
                            Position pos = CoreService.TradingInfoTracker.PositionTracker[k.Symbol, account, posside];

                            decimal unrealizedpl = pos.UnRealizedPL;

                            //更新浮动盈亏
                            if (pos.isFlat)
                            {
                                tb.Rows[i][UNREALIZEDPL] = 0;
                                tb.Rows[i][UNREALIZEDPLACCTCURRENCY] = 0;
                                tb.Rows[i][UNREALIZEDPOINT] = 0;
                            }
                            else
                            {
                                tb.Rows[i][UNREALIZEDPL] = (unrealizedpl * pos.oSymbol.Multiple).ToFormatStr() +" "+pos.oSymbol.SecurityFamily.Currency.ToString();
                                tb.Rows[i][UNREALIZEDPOINT] = unrealizedpl;
                                tb.Rows[i][UNREALIZEDPLACCTCURRENCY] = (pos.UnRealizedPL * pos.oSymbol.Multiple*CoreService.TradingInfoTracker.Account.GetExchangeRate(pos.oSymbol.SecurityFamily.Currency)).ToFormatStr();

                                //更新止盈止损数值
                                string lossstr = GetGridPositionOffsetText(pos, QSEnumPositionOffsetDirection.LOSS);
                                string profitstr = GetGridPositionOffsetText(pos, QSEnumPositionOffsetDirection.PROFIT);
                                tb.Rows[i][LOSSTARGET] = lossstr;
                                tb.Rows[i][PROFITTARGET] = profitstr;

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


        string GetGridPositionOffsetText(Position pos, QSEnumPositionOffsetDirection direction)
        {
            PositionOffsetArgSet args = CoreService.PositionWatcher.GetPositionOffsetArgs(pos);
            if (args == null) return "无";
            decimal targetprice = 0;
            //logger.Info("it is here");
            //return "x";
            string sizestr = string.Empty;
            if (direction == QSEnumPositionOffsetDirection.LOSS)
            {
                targetprice = args.LossArg.TargetPrice(pos);
                sizestr = args.LossArg.Size==0?"所有":args.LossArg.Size.ToString();
            }
            else
            {
                targetprice = args.ProfitArg.TargetPrice(pos);
                sizestr = args.ProfitArg.Size == 0 ? "所有" : args.ProfitArg.Size.ToString();
            }

            if (targetprice == -1)
            {
                return "停止";
            }
            if (targetprice == 0)
            {
                return "无持仓";
            }
            else
            {
                return string.Format("{0}/{1}",pos.FormatPrice(targetprice), sizestr);
            }
        }

        #region 表格
        const string POSKEY = "持仓键";
        const string ACCOUNT = "ACCOUNT";
        const string SYMBOLKEY = "合约键";
        const string SYMBOL = "合约STD";
        const string SYMBOLTITLE = "合约";
        const string SIDE = "SIDE";
        const string SIDESTR = "方向";
        const string PROPERTY = "属性";
        const string SIZE = "持仓";

        const string SIZECANFLAT = "可用";
        const string AVGPRICE = "开仓均价";
        const string UNREALIZEDPL = "浮动盈亏";
        const string UNREALIZEDPLACCTCURRENCY = "浮盈(基币)";
        const string UNREALIZEDPOINT = "浮盈(点)";
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
            tb.Columns.Add(POSKEY);//0
            tb.Columns.Add(SYMBOL);//1
            tb.Columns.Add(SYMBOLTITLE);
            tb.Columns.Add(ACCOUNT);//2
            tb.Columns.Add(SYMBOLKEY);//3
            tb.Columns.Add(SIDE);//4
            tb.Columns.Add(SIDESTR);//5
            tb.Columns.Add(PROPERTY);//6
            tb.Columns.Add(SIZE);//7
            tb.Columns.Add(SIZECANFLAT);//8
            tb.Columns.Add(AVGPRICE);//9
            tb.Columns.Add(UNREALIZEDPL);//10
            tb.Columns.Add(UNREALIZEDPLACCTCURRENCY);//11
            tb.Columns.Add(UNREALIZEDPOINT);//12

            tb.Columns.Add(LOSSTARGET);//13
            tb.Columns.Add(PROFITTARGET);//14
            tb.Columns.Add(FLAG);//15
            tb.Columns.Add(NAME);//16


        }

        void ResetColumeSize()
        {
            DataGridView grid = positionGrid;

            grid.Columns[SYMBOLTITLE].Width = 120;
            grid.Columns[SIDESTR].Width = 40;
            grid.Columns[PROPERTY].Width = 40;
            grid.Columns[SIZE].Width = 46;
            grid.Columns[SIZECANFLAT].Width = 46;

            grid.Columns[AVGPRICE].Width = 90;
            grid.Columns[UNREALIZEDPL].Width = 120;
            grid.Columns[UNREALIZEDPLACCTCURRENCY].Width = 90;
            grid.Columns[LOSSTARGET].Width = 75;
            grid.Columns[PROFITTARGET].Width = 75;
            grid.Columns[FLAG].Width = 40;
            grid.Columns[NAME].Width = 120;

           
            positionGrid.CalcRowWidth();
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
            datasource.Filter = SIZE + " > 0";
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[POSKEY].Visible = false;
            grid.Columns[ACCOUNT].Visible = false;
            grid.Columns[SIDE].Visible = false;
            grid.Columns[SYMBOLKEY].Visible = false;
            grid.Columns[UNREALIZEDPOINT].Visible = false;
            grid.Columns[SYMBOL].Visible = false;
            grid.Columns[FLAG].Visible = Constants.HedgeFieldVisible;

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
