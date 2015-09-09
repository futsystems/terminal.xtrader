using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {
        #region 客户端暴露的操作



        /// <summary>
        /// 发送委托
        /// </summary>
        /// <param name="order"></param>
        public void ReqOrderInsert(Order order)
        {
            logger.Info(PROGRAME + " send order:" + order.GetOrderInfo());

            order.Account = _account;

            OrderInsertRequest request = RequestTemplate<OrderInsertRequest>.CliSendRequest(requestid++);
            request.Order = order;

            SendPacket(request);
        }
        /// <summary>
        /// 提交委托操作
        /// </summary>
        /// <param name="action"></param>
        public void ReqOrderAction(OrderAction action)
        {
            logger.Info(PROGRAME + " send orderaction:" + action.ToString());

            OrderActionRequest requets = RequestTemplate<OrderActionRequest>.CliSendRequest(requestid++);
            action.Account = _account;
            requets.OrderAction = action;

            SendPacket(requets);
        }
        /// <summary>
        /// 直接取消委托
        /// </summary>
        /// <param name="id"></param>
        public void ReqCancelOrder(long id)
        {
            logger.Info(PROGRAME + " cancel order:" + id.ToString());

            OrderAction action = new OrderActionImpl();
            action.Account = "";
            action.ActionFlag = QSEnumOrderActionFlag.Delete;
            action.OrderID = id;
            ReqOrderAction(action);
        }

        /// <summary>
        /// 请求注销行情数据
        /// </summary>
        public void ReqUnRegisterSymbols()
        {
            UnregisterSymbolsRequest request = RequestTemplate<UnregisterSymbolsRequest>.CliSendRequest(requestid++);

            SendPacket(request);
        }

        /// <summary>
        /// 请求登入
        /// </summary>
        /// <param name="loginid"></param>
        /// <param name="pass"></param>
        public void ReqLogin(string loginid, string pass, int logintype = 1)
        {
            logger.Info(PROGRAME + " request login to server, account:" + loginid + " pass:" + pass);

            LoginRequest request = RequestTemplate<LoginRequest>.CliSendRequest(requestid++);
            request.LoginID = loginid;
            request.Passwd = pass;
            request.LoginType = 1;
            //request.MAC = mac;
            SendPacket(request);
        }

        /// <summary>
        /// 请求帐户信息
        /// </summary>
        public void ReqQryAccountInfo()
        {
            logger.Info(PROGRAME + " qry account info");
            QryAccountInfoRequest request = RequestTemplate<QryAccountInfoRequest>.CliSendRequest(requestid++);
            request.Account = _account;

            SendPacket(request);
        }
        /// <summary>
        /// 请求查询可开手数
        /// </summary>
        public void ReqQryMaxOrderVol(string symbol)
        {
            logger.Info(PROGRAME + " qry max order vol,symbol:" + symbol);

            QryMaxOrderVolRequest request = RequestTemplate<QryMaxOrderVolRequest>.CliSendRequest(requestid++);
            request.Symbol = symbol;
            request.OffsetFlag = QSEnumOffsetFlag.UNKNOWN;
            request.Account = _account;

            SendPacket(request);
        }

        /// <summary>
        /// 查询交易者信息
        /// </summary>
        public void ReqQryInvestor()
        {
            logger.Info(PROGRAME + " qry investror info");

            QryInvestorRequest request = RequestTemplate<QryInvestorRequest>.CliSendRequest(requestid++);
            request.Account = _account;
            SendPacket(request);
        }

        #region 基础信息
        /// <summary>
        /// 查询交易时间段
        /// </summary>
        public void ReqXQryMarketTime()
        {
            logger.Info(PROGRAME + " qry markettime");

            XQryMarketTimeRequest request = RequestTemplate<XQryMarketTimeRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        /// <summary>
        /// 查询交易所
        /// </summary>
        public void ReqXQryExchange()
        {
            logger.Info(PROGRAME + " qry exchange");

            XQryExchangeRequuest request = RequestTemplate<XQryExchangeRequuest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        /// <summary>
        /// 查询品种
        /// </summary>
        public void ReqXQrySecurity()
        {
            logger.Info(PROGRAME + " qry security");

            XQrySecurityRequest request = RequestTemplate<XQrySecurityRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        /// <summary>
        /// 查询合约
        /// </summary>
        public void ReqXQrySymbol()
        {
            logger.Info(PROGRAME + " qry symbol");

            XQrySymbolRequest request = RequestTemplate<XQrySymbolRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        #endregion


        #region 交易记录
        /// <summary>
        /// 查询隔夜持仓
        /// </summary>
        public void ReqXQryYDPositon()
        {
            logger.Info(PROGRAME +" qry yd position");
            XQryYDPositionRequest request = RequestTemplate<XQryYDPositionRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        /// <summary>
        /// 查询委托
        /// </summary>
        public void ReqXQryOrder()
        {
            logger.Info(PROGRAME + "qry order");
            XQryOrderRequest request = RequestTemplate<XQryOrderRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }

        /// <summary>
        /// 查询成交
        /// </summary>
        public void ReqXQryTrade()
        {
            logger.Info(PROGRAME + "qry trade");
            XQryTradeRequest request = RequestTemplate<XQryTradeRequest>.CliSendRequest(requestid++);
            SendPacket(request);
        }



        #endregion
        /// <summary>
        /// 查询结算信息
        /// </summary>
        public void ReqQrySettleInfo(int tradingday = 0)
        {
            logger.Info(PROGRAME + " qry settleinfo");

            QrySettleInfoRequest request = RequestTemplate<QrySettleInfoRequest>.CliSendRequest(requestid++);
            request.Account = _account;
            request.Tradingday = tradingday;
            SendPacket(request);
        }

        /// <summary>
        /// 查询结算确认
        /// </summary>
        public void ReqQrySettleInfoConfirm()
        {
            logger.Info(PROGRAME + " qry settleinfoconfigm");

            QrySettleInfoConfirmRequest request = RequestTemplate<QrySettleInfoConfirmRequest>.CliSendRequest(requestid++);
            request.Account = _account;

            SendPacket(request);
        }

        /// <summary>
        /// 确认结算单
        /// </summary>
        public void ReqConfirmSettlement()
        {
            logger.Info(PROGRAME + " confirm settlement");

            ConfirmSettlementRequest request = RequestTemplate<ConfirmSettlementRequest>.CliSendRequest(requestid++);
            request.Account = _account;

            SendPacket(request);
        }
        /// <summary>
        /// 查询委托
        /// </summary>
        //public void ReqQryOrder(string symbol = "", long orderid = 0)
        //{
        //    logger.Info(PROGRAME + " qry order");

        //    QryOrderRequest request = RequestTemplate<QryOrderRequest>.CliSendRequest(requestid++);
        //    request.Account = _account;
        //    request.Symbol = symbol;
        //    request.OrderID = orderid;
        //    SendPacket(request);
        //}

        /// <summary>
        /// 查询成交
        /// </summary>
        //public void ReqQryTrade(string symbol = "")
        //{
        //    logger.Info(PROGRAME + " qry trade");

        //    QryTradeRequest request = RequestTemplate<QryTradeRequest>.CliSendRequest(requestid++);
        //    request.Account = _account;
        //    request.Symbol = symbol;

        //    SendPacket(request);
        //}

        /// <summary>
        /// 查询持仓
        /// </summary>
        //public void ReqQryPosition(string symbol = "")
        //{
        //    logger.Info(PROGRAME + " qry position");

        //    QryPositionRequest request = RequestTemplate<QryPositionRequest>.CliSendRequest(requestid++);
        //    request.Account = _account;
        //    request.Symbol = symbol;

        //    SendPacket(request);
        //}


        /// <summary>
        /// 请求注册行情数据
        /// </summary>
        public void ReqRegisterSymbols(string[] symbols)
        {
            logger.Info(PROGRAME + " register symbols:" + string.Join(",", symbols));

            RegisterSymbolsRequest request = RequestTemplate<RegisterSymbolsRequest>.CliSendRequest(requestid++);
            request.SetSymbols(symbols);

            SendPacket(request);
            connecton.Subscribe(symbols);

        }

        public void ReqContribRequest(string moduleid, string cmdstr, string args)
        {
            ContribRequest request = RequestTemplate<ContribRequest>.CliSendRequest(requestid++);
            request.ModuleID = moduleid;
            request.CMDStr = cmdstr;
            request.Parameters = args;

            SendPacket(request);
        }

        public void ReqChangePassowrd(string oldpass, string newpass)
        {
            logger.Info(PROGRAME + " req change password");

            ReqChangePasswordRequest request = RequestTemplate<ReqChangePasswordRequest>.CliSendRequest(requestid++);
            request.Account = _account;
            request.OldPassword = oldpass;
            request.NewPassword = newpass;

            SendPacket(request);
        }


        public void ReqQrySymbol(string symbol)
        {
            QrySymbolRequest request = RequestTemplate<QrySymbolRequest>.CliSendRequest(requestid++);
            request.Symbol = symbol;

            SendPacket(request);
        }

        public void ReqQryOpenSize(string symbol)
        {
            QryMaxOrderVolRequest request = RequestTemplate<QryMaxOrderVolRequest>.CliSendRequest(requestid++);
            request.Symbol = symbol;
            request.Account = _account;

            SendPacket(request);
        }
        #endregion


    }
}
