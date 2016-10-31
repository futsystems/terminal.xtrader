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
    public class EventQry
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



    }
}
