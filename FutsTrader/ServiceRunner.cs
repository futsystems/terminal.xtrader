using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace FutsTrader
{
    public partial class MainForm 
    {
        private void InitTradingTrackerCentre()
        {
            debug("1.初始化交易信息记录中心");
            //_tradingtrackerCentre = new TradingTrackerCentre();
            //_tradingtrackerCentre.SendDebugEvet += new DebugDelegate(debug);
           // _defaultBasket = _tradingtrackerCentre.DefaultBasket;

        }

        private void InitCoreCentre()
        {
            debug("2.初始化CoreCentre组件");
            //通过我们选定的通道优先类别，生成服务端列表，按照优先访问顺序

            //_coreCentre = new CoreCentre2(_tradingtrackerCentre,new string[]{"127.0.0.1"},5570);
            //_coreCentre.SendDebugEvent += new DebugDelegate(debug);

            
            //_coreCentre.SendPopMessageEvent += new TradingLib.API.PopMessageDel(PopMessage);
            //_coreCentre.GotConnectEvent += new ConnectDel(OnConnect);//连接建立
            //_coreCentre.GotDisconnectEvent += new DisconnectDel(OnDisconnect);//连接断开

            //_coreCentre.DataPubConnectEvent += new DataPubConnectDel(OnDataPubConnectEvent);
            //_coreCentre.DataPubDisconnectEvent += new DataPubDisconnectDel(OnDataPubDisconnectEvent);

            //_coreCentre.gotLoginRep += new LoginResponseDel(OnLogin);
            //_coreCentre.resumeFinish += new VoidDelegate(OnResumeFinish);
            //_coreCentre.resumeStart += new VoidDelegate(OnResumeStart);

            //_coreCentre.gotTick += new TickDelegate(OnTick);//行情数据达到
            //_coreCentre.gotPosition += new PositionDelegate(OnPosition);
            //_coreCentre.gotOrder += new OrderDelegate(OnOrder);//委托达到
            //_coreCentre.gotOrderCancel += new LongDelegate(OnCancel);//取消达到
            //_coreCentre.gotFill += new FillDelegate(OnFill);//成交达到

            //_coreCentre.GotAccountInfoEvent += new IAccountInfoDel(OnAccountInfo);//账户信息达到
            //_coreCentre.GotCanOpenPositionEvent += new IntDelegate(OnCanOpenPositionRep);//可开仓数据达到
            //_coreCentre.GotRaceInfoEvent += new IRaceInfoDel(OnRaceInfo);//比赛信息达到
            //_coreCentre.gotRuleSetInfo += new StringParamDelegate(OnRuleSetInfo);


        }

        private void InitPositionCheckCentre()
        {
            debug("3.初始化仓位监控中心");
            //_poscheckCentre = new PositionCheckCentre(_tradingtrackerCentre as ITradingTrackerCentre);
            //_poscheckCentre.SendDebugEvent += new DebugDelegate(debug);

            //_poscheckCentre.SendOrderEvent += new OrderDelegate(_coreCentre.SendOrder);
           // _poscheckCentre.CancelOrderEvent += new LongDelegate(_coreCentre.CancelOrder);

        }


        /// <summary>
        /// 绑定控件时间
        /// </summary>
        void InitControl()
        {
            debug("4.初始化组件");
            //初始化报价面板

            //ctQuoteView1.SendDebugEvent += new DebugDelegate(debug);
            //ctQuoteView1.SymbolSelectedEvent += new SecurityDelegate(ctOrderSender1.SetSecurity);
            //ctQuoteView1.SetBasket(_defaultBasket);

            /*
            //初始化positionview
            ctPositionView1.SendDebugEvent += new DebugDelegate(debug);
            ctPositionView1.PositionCheckCentre = _poscheckCentre;

            ctPositionView1.PositionTracker = _tradingtrackerCentre.PositionTracker;
            ctPositionView1.OrderTracker = _tradingtrackerCentre.OrderTracker;

            ctPositionView1.SendOrderEvent += new OrderDelegate(SendOrder);
            ctPositionView1.SendCancelEvent += new LongDelegate(CancelOrder);
            ctPositionView1.FindSecurityEvent += new FindSecurity(_tradingtrackerCentre.getSecurity);
            ctPositionView1.PositionSelectedEvent += new PositionDelegate(ctOrderSender1.SetPosition);

            ctPositionView1.LookUpLossArgsEvent += new LookUpLossArgs(GetStopLossArgs);
            ctPositionView1.LookUpProfitArgsEvent += new LookUpProfitArgs(GetProfitArgs);

            //初始化orderview
            ctOrderView1.SendDebugEvent += new DebugDelegate(debug);
            ctOrderView1.SendOrderCancel += new LongDelegate(CancelOrder);
            ctOrderView1.OrderTracker = _tradingtrackerCentre.OrderTracker;

             * **/
            //初始化下单面板
            //ctOrderSender1.DefaultBasket = _defaultBasket;
            //ctOrderSender1.OnBasketChange();

            //ctOrderSender1.PositionTracker = _tradingtrackerCentre.PositionTracker;
            //ctOrderSender1.OrderTracker = _tradingtrackerCentre.OrderTracker;

            //ctOrderSender1.SendDebugEvent += new DebugDelegate(debug);
            //ctOrderSender1.SendOrderEvent += new OrderDelegate(SendOrder);
            //ctOrderSender1.SendCancelEvent += new LongDelegate(CancelOrder);

            //ctOrderSender1.QryCanOpenPositionEvent += new QryCanOpenPosition(_coreCentre.QryCanOpenPosition);
            //ctOrderSender1.SendReserveSymbolPositionEvent += new SymDelegate(positionView1.ReserverPosition);
            //ctOrderSender1.SendQryDefaultSizeEvent += new IntStringDelegate(GetDefaultSize);

            //ctOrderSender1.SendEntrySecEvent += new VoidDelegate(onEntrySecSelection);
            //ctOrderSender1.SendLeaveSecEvent += new VoidDelegate(onLeaveSecSelection);
            //ctOrderSender1.SendSecuritySelectedEvent += new VoidDelegate(AdjuestSymbolSubscribe);//改变选中合约调整订阅列表
            
            //初始化tradeview
            //ctTradeView1.FindSecurityEvent += new FindSecurity(_tradingtrackerCentre.getSecurity);

            //绑定修改密码事件
            //ctChangePassward2.ChangeAccountPassEvent += new ChangeAccountPassDel(_coreCentre.ChangeAccountPass);

            //SetRaceControl();
        }

        #region corecentre事件回调

        
        void OnConnect()
        {
            //linkLabel.Image = (System.Drawing.Image)Properties.Resources.link;
            //provider.Text = _coreCentre.Provider.ToString();
            
        }

        void OnDisconnect()
        {
            //linkLabel.Image = (System.Drawing.Image)Properties.Resources.link_break;
            //dataLable.Image = (System.Drawing.Image)Properties.Resources.link_break;
            //loginLable.Image = (System.Drawing.Image)Properties.Resources.logout;
        }

        void OnDataPubConnectEvent()
        {
            //dataLable.Image = (System.Drawing.Image)Properties.Resources.link;
            //if (en_autologin)
            //    autoLogin();
        }

        void OnDataPubDisconnectEvent()
        {
            //dataLable.Image = (System.Drawing.Image)Properties.Resources.link_break;
        }

        string title = string.Empty;
        void OnLogin(bool f)
        {
            if (f)
            {
                /*
                loginLable.Image = (System.Drawing.Image)Properties.Resources.login;
                _coreCentre.QryAccountInfo();
                title = "盈晶投资[" + LiteGlobals.Ver + "]  交易账号:" + _loginID + "@" + _srvlabel + "  ";
                notifyIcon.Text = "盈晶交易客户端" + " [" + _loginID + "]";
                Text = title;
                flatsend = false;**/

            }
        }

        //获得规则信息
        void OnRuleSetInfo(string param)
        {
            //_fmriskcontrol.GotRuleSetInfo(param);

        }

        //交易数据恢复完毕后执行的操作
        void OnResumeFinish()
        {
            //debug("~~~~~onresumefinish");
            //positionView1.OnFinishResume();
            //this.pageholder.Enabled = true;
            //resumelabel.Text = "";
        }

        void OnResumeStart()
        {
            //debug("~~~~~onresumestart");
            //pageholder.SelectedPage = quotepanel;
            //this.pageholder.Enabled = false;
            //锁定当前状态
            //positionView1.OnStartResume();
            //orderView1.OnStartResume();
            //本地数据情况
            //this.Clear();
            //resumelabel.Text = "正在恢复当日交易数据....";

        }


        void OnPosition(Position p)
        {
            try
            {
                //Trade f = p.ToTrade();
                //positionView1.GotFill(f);
            }
            catch (Exception ex)
            {
                //debug(ex.ToString());
            }
        }
        void OnTick(Tick k)
        {
            try
            {
                //debug("got tick:" + k.ToString());
                //_poscheckCentre.GotTick(k);
                ctQuoteView1.GotTick(k);
                ctOrderSender1.GotTick(k);
                //positionView1.GotTick(k);
            }
            catch (Exception ex)
            {
                debug(ex.ToString());
            }
        }
        void OnOrder(Order o)
        {
            //positionView1.GotOrder(o);
            //orderView1.GotOrder(o);
            //if (!_coreCentre.IsInResume && o.Status == QSEnumOrderStatus.Opened)
            //    ctOrderSender1.QryCanOpen(o.symbol);
        }
        void OnCancel(long oid)
        {
            //_poscheckCentre.GotCancel(oid);
            //positionView1.GotOrderCancel(oid);
            //orderView1.GotOrderCancel(oid);
            //if (!_coreCentre.IsInResume)
            //{
            //    Order o = _tradingtrackerCentre.SentOrder(oid);
            //    if (o != null && o.isValid)
            //        ctOrderSender1.QryCanOpen(o.symbol);
            //}
        }
        void OnFill(Trade fill)
        {
            /*
            _poscheckCentre.GotFill(fill);
            orderView1.GotFill(fill);
            positionView1.GotFill(fill);
            tradeView1.GotFill(fill);
            if (!_coreCentre.IsInResume)
            {
                ctOrderSender1.GotTrade(fill);
                ctOrderSender1.QryCanOpen(fill.symbol);
            }**/

        }


        //void OnRaceInfo(IRaceInfo ri)
        //{
        //    //_fmracestatus.GotRaceInfo(ri);
        //    //racestatus.Text = LibUtil.GetEnumDescription(ri.RaceStatus);
        //}
        //void OnCanOpenPositionRep(int size)
        //{
        //    //ctOrderSender1.setPositionSize(size);
        //}

        bool israce = false;
        //获得账户信息
        //void OnAccountInfo(IAccountInfo accinfo)
        //{
        //    /*

        //    ctAccountFinanceInfo2.RefreshAccountInfo(accinfo);

        //    accCantrade.Text = accinfo.Execute ? "正常" : "禁止";
        //    if (accinfo.Execute)
        //    {
        //        accCantrade.StateCommon.ShortText.Color1 = Color.DarkGreen;
        //    }
        //    else
        //    {
        //        accCantrade.StateCommon.ShortText.Color1 = Color.DarkRed;
        //    }
        //    accCategory.Text = LibUtil.GetEnumDescription(accinfo.Category);
        //    accIntraday.Text = accinfo.IntraDay ? "日内" : "可隔夜";

        //    //当返回账户信息时，表明已经登入成功，我们激活风控中心
        //    _fmriskcontrol.LastEquity = accinfo.LastEquity;
        //    _fmriskcontrol.Enabled = true;
        //    israce = accinfo.Category == QSEnumAccountCategory.DEALER ? true : false;
        //    SetRaceControl();
        //     * */
        //}

        void SetRaceControl()
        {
            /*
            if (!israce)
            {
                racelabel.Visible = false;
                racestatus.Visible = false;
                raceStatusToolStripMenuItem.Enabled = false;
            }
            else
            {
                racelabel.Visible = true;
                racestatus.Visible = true;
                raceStatusToolStripMenuItem.Enabled = true;
            }**/
        }



        #endregion
    }
}
