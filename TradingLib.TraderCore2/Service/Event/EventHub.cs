using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    /// <summary>
    /// 查询事件中继
    /// </summary>
    public class EventHub
    {
        /// <summary>
        /// 查询成交回报
        /// </summary>
        public event Action<Trade,RspInfo,int,bool> OnRspXQryFillResponese;
        internal void FireRspXQryFillResponese(Trade trade, RspInfo rsp,int requestId, bool isLast)
        {
            if (OnRspXQryFillResponese != null)
            {
                OnRspXQryFillResponese(trade, rsp,requestId,isLast);
            }
        }

        /// <summary>
        /// 查询委托回报
        /// </summary>
        public event Action<Order, RspInfo,int,bool> OnRspXQryOrderResponse;
        internal void FireRspXQryOrderResponse(Order order, RspInfo rsp, int requestId, bool isLast)
        {
            if (OnRspXQryOrderResponse != null)
            {
                OnRspXQryOrderResponse(order, rsp, requestId, isLast);
            }
        }

        /// <summary>
        /// 查询隔夜持仓明细回报
        /// </summary>
        public event Action<PositionDetail, RspInfo, int, bool> OnRspXQryYDPositionResponse;
        internal void FireRspXQryYDPositionResponse(PositionDetail pd, RspInfo rsp, int requestId, bool isLast)
        {
            if (OnRspXQryYDPositionResponse != null)
            {
                OnRspXQryYDPositionResponse(pd, rsp, requestId, isLast);
            }
        }


        public event Action<AccountLite, RspInfo, int, bool> OnRspXQryAccountResponse;
        internal void FireRspXQryAccountResponse(AccountLite info, RspInfo rsp, int requestId, bool isLast)
        {
            if (OnRspXQryAccountResponse != null)
            {
                OnRspXQryAccountResponse(info, rsp, requestId, isLast);
            }
        }


        /// <summary>
        /// 查询合约对象
        /// </summary>
        public event Action<Symbol, RspInfo, int, bool> OnRspXQrySymbolResponse;
        internal void FireRspXQrySymbolResponse(Symbol symbol, RspInfo rsp, int requestId, bool isLast)
        {
            if (OnRspXQrySymbolResponse != null)
            {
                OnRspXQrySymbolResponse(symbol, rsp, requestId, isLast);
            }
        }





        /// <summary>
        /// 查询账户财务信息
        /// </summary>
        public event Action<RspXQryAccountFinanceResponse> OnRspXQryAccountFinanceEvent;
        internal void FireRspXQryAccountFinanceEvent(RspXQryAccountFinanceResponse response)
        {
            if (OnRspXQryAccountFinanceEvent != null)
                OnRspXQryAccountFinanceEvent(response);
        }

        /// <summary>
        /// 查询最大可开手数
        /// </summary>
        public event Action<RspXQryMaxOrderVolResponse> OnRspXQryMaxOrderVolResponse;
        internal void FireRspXQryMaxOrderVolResponse(RspXQryMaxOrderVolResponse response)
        {
            if (OnRspXQryMaxOrderVolResponse != null)
                OnRspXQryMaxOrderVolResponse(response);

        }

        /// <summary>
        /// 结算单查询回报
        /// </summary>
        public event Action<RspXQrySettleInfoResponse> OnRspXQrySettlementResponse;
        internal void FireRspXQrySettlementResponse(RspXQrySettleInfoResponse response)
        {
            if (OnRspXQrySettlementResponse != null)
                OnRspXQrySettlementResponse(response);
        }


        /// <summary>
        /// 查询持仓明细
        /// </summary>
        public event Action<RspXQryPositionDetailResponse> OnRspXQryPositionDetailResponse;
        internal void FireRspXQryPositionDetailResponse(RspXQryPositionDetailResponse response)
        {
            if (OnRspXQryPositionDetailResponse != null)
                OnRspXQryPositionDetailResponse(response);
        }

        



        public event Action<RspReqChangePasswordResponse> OnRspReqChangePasswordResponse;
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="response"></param>
        internal void FireRspReqChangePasswordResponse(RspReqChangePasswordResponse response)
        {
            if (OnRspReqChangePasswordResponse != null)
            {
                OnRspReqChangePasswordResponse(response);
            }
        }


        /// <summary>
        /// 交易记录开始恢复
        /// </summary>
        public event Action OnResumeDataStart;
        internal void FireResumeDataStart()
        {
            if (OnResumeDataStart != null)
            {
                OnResumeDataStart();
            }
        }

        /// <summary>
        /// 交易记录恢复完成
        /// </summary>
        public event Action OnResumeDataEnd;
        internal void FireResumeDataEnd()
        {
            if (OnResumeDataEnd != null)
            {
                OnResumeDataEnd();
            }
        }


        #region 合约 持仓 选中事件
        static Symbol _symbolSelected = null;
        public event Action<Object, Symbol> OnSymbolSelectedEvent;
        /// <summary>
        /// 触发合约选择事件
        /// </summary>
        /// <param name="symbol"></param>
        public void FireSymbolSelectedEvent(Object sender, Symbol symbol)
        {
            if (_symbolSelected != null && symbol != null)
            {
                FireSymbolUnSelectedEvent(sender, _symbolSelected);
                _symbolSelected = symbol;
            }

            if (OnSymbolSelectedEvent != null)
                OnSymbolSelectedEvent(sender, symbol);
        }

        public event Action<object, Symbol> OnSymbolUnSelectedEvent;
        /// <summary>
        /// 触发合约取消选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="symbol"></param>
        public void FireSymbolUnSelectedEvent(Object sender, Symbol symbol)
        {
            if (OnSymbolUnSelectedEvent != null)
                OnSymbolUnSelectedEvent(sender, symbol);
        }



        public event Action<object, Position> OnPositionSelectedEvent = delegate { };

        /// <summary>
        /// 触发持仓选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="position"></param>
        public void FirePositionSelectedEvent(object sender, Position position)
        {
            OnPositionSelectedEvent(sender, position);
        }
        #endregion
    }
}
