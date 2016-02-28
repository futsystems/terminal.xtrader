﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    public partial class TradingInfoTracker
    {
        /// <summary>
        /// 委托维护器
        /// </summary>
        public OrderTracker OrderTracker { get; set; }

        /// <summary>
        /// 当日持仓维护器
        /// </summary>
        public LSPositionTracker PositionTracker { get; set; }

        /// <summary>
        /// 昨日持仓维护器
        /// </summary>
        public LSPositionTracker HoldPositionTracker { get; set; }

        /// <summary>
        /// 成交维护器
        /// </summary>
        public ThreadSafeList<Trade> TradeTracker { get; set; }

        /// <summary>
        /// 新的持仓生成事件
        /// </summary>
        public event Action<Position> GotPositionEvent;

        public AccountLite Account { get; set; }

        public TradingInfoTracker()
        {
            OrderTracker = new OrderTracker();
            PositionTracker = new LSPositionTracker("");
            HoldPositionTracker = new LSPositionTracker("");
            TradeTracker = new ThreadSafeList<Trade>();
            Account = new AccountLite();

            CoreService.EventQry.OnRspXQryOrderResponse += new Action<Order, RspInfo, int, bool>(EventQry_OnRspXQryOrderResponse);
            CoreService.EventQry.OnRspXQryYDPositionResponse += new Action<PositionDetail, RspInfo, int, bool>(EventQry_OnRspXQryYDPositionResponse);
            CoreService.EventQry.OnRspXQryFillResponese += new Action<Trade, RspInfo, int, bool>(EventQry_OnRspXQryFillResponese);
            CoreService.EventQry.OnRspQryAccountInfoResponse += new Action<AccountInfo, RspInfo, int, bool>(EventQry_OnRspQryAccountInfoResponse);
            PositionTracker.NewPositionEvent += new Action<Position>(PositionTracker_NewPositionEvent);
        }


        int _qrypositionid = 0;
        /// <summary>
        /// 请求恢复日内交易记录
        /// </summary>
        public void ResumeData()
        {
            logger.Info("Start to resueme trading data");
            CoreService.EventOther.FireResumeDataStart();

            //清空当前交易记录维护器
            OrderTracker.Clear();
            PositionTracker.Clear();
            HoldPositionTracker.Clear();
            TradeTracker.Clear();

            //执行隔夜持仓查询 并按序触发后续查询
            _qrypositionid = CoreService.TLClient.ReqXQryYDPositon();
        }


        int _qryorderid = 0;
        void EventQry_OnRspXQryYDPositionResponse(PositionDetail arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qrypositionid) return;
            if (arg1 != null)
            {
                PositionTracker.GotPosition(arg1);
                HoldPositionTracker.GotPosition(arg1);
            }
            if (arg4)
            {
                Status("隔夜持仓查询完毕,查询委托");
                _qryorderid =  CoreService.TLClient.ReqXQryOrder();
            }
        }

        int _qrytradeid = 0;
        void EventQry_OnRspXQryOrderResponse(Order arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryorderid) return;
            if (arg1 != null)
            {
                OrderTracker.GotOrder(arg1);
            }
            if (arg4)
            {
                Status("委托查询完毕,查询成交");
                _qrytradeid =  CoreService.TLClient.ReqXQryTrade();
            }
        }

        int _qryaccountinfoid = 0;
        void EventQry_OnRspXQryFillResponese(Trade arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qrytradeid) return;
            if (arg1 != null)
            {
                bool accept = false;
                PositionTracker.GotFill(arg1, out accept);
                if (accept)
                {
                    OrderTracker.GotFill(arg1);
                    TradeTracker.Add(arg1);
                }
            }
            if (arg4)
            {
                Status("成交查询完毕,查询帐户信息");
                _qryaccountinfoid = CoreService.TLClient.ReqQryAccountInfo();
            }
        }

        void EventQry_OnRspQryAccountInfoResponse(AccountInfo arg1, RspInfo arg2, int arg3, bool arg4)
        {
            if (arg3 != _qryaccountinfoid) return;

            CoreService.AccountInfo = arg1;
            //登入第一次初始化过程中 查询完毕后需要启动行情连接并执行初始化事件
            if (!CoreService.Initialized)
            {
                if (arg1 == null)
                {
                    Status("帐户信息查询异常");
                    return;
                }

                if (arg4)
                {
                    Status("帐户信息查询完毕");
                    CoreService.TLClient.StartTick();
                    //核心服务完成初始化
                    CoreService.Initialize();
                    Status("触发初始化完毕事件");
                }
            }

            logger.Info("trading data resume finished");
            CoreService.EventOther.FireResumeDataEnd();
            
        }



        void PositionTracker_NewPositionEvent(Position obj)
        {
            if (GotPositionEvent != null)
                GotPositionEvent(obj);
        }

        

    }
}
