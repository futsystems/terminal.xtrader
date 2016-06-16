using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {
        #region API操作接口



        /// <summary>
        /// 发送委托
        /// </summary>
        /// <param name="order"></param>
        public int  ReqOrderInsert(Order order)
        {
            logger.Info(PROGRAME + " send order:" + order.GetOrderInfo());

            order.Account = _account;

            OrderInsertRequest request = RequestTemplate<OrderInsertRequest>.CliSendRequest(++requestid);
            request.Order = order;

            SendPacket(request);
            return requestid;
        }
        /// <summary>
        /// 提交委托操作
        /// </summary>
        /// <param name="action"></param>
        public int ReqOrderAction(OrderAction action)
        {
            logger.Info(PROGRAME + " send orderaction:" + action.ToString());

            OrderActionRequest requets = RequestTemplate<OrderActionRequest>.CliSendRequest(++requestid);
            action.Account = _account;
            requets.OrderAction = action;

            SendPacket(requets);
            return requestid;
        }

        /// <summary>
        /// 直接取消委托
        /// </summary>
        /// <param name="id"></param>
        public int ReqCancelOrder(long id)
        {
            logger.Info(PROGRAME + " cancel order:" + id.ToString());

            OrderAction action = new OrderActionImpl();
            action.Account = "";
            action.ActionFlag = QSEnumOrderActionFlag.Delete;
            action.OrderID = id;
            return ReqOrderAction(action);
        }




        /// <summary>
        /// 请求登入
        /// </summary>
        /// <param name="loginid"></param>
        /// <param name="pass"></param>
        public int  ReqLogin(string loginid, string pass, int logintype = 1)
        {
            logger.Info(PROGRAME + " request login to server, account:" + loginid + " pass:" + pass);
            LoginRequest request = RequestTemplate<LoginRequest>.CliSendRequest(++requestid);
            request.LoginID = loginid;
            request.Passwd = pass;
            request.LoginType = 1;
            request.ProductInfo = Constants.ProductInfo;
            request.IPAddress = "";// info.IP;
            
            //request.IPAddress = "22.22.22.22";
            //request.MAC = "wwwwww";
            //request.MAC = mac;
            SendPacket(request);

            Func<LocationInfo> del = new Func<LocationInfo>(Util.GetLocationInfo);
            del.BeginInvoke(QryLocaltionInfoCallback, null);
            return requestid;
        }

        void QryLocaltionInfoCallback(IAsyncResult async)
        {
            Func<LocationInfo> proc = ((AsyncResult)async).AsyncDelegate as Func<LocationInfo>;
            LocationInfo info = proc.EndInvoke(async);
            //InvokeGotGLocation(location);
            UpdateLocationInfoRequest request = RequestTemplate<UpdateLocationInfoRequest>.CliSendRequest(++requestid);
            request.LocationInfo = info;
            SendPacket(request);
        }


        /// <summary>
        /// 请求帐户信息
        /// </summary>
        public int  ReqQryAccountInfo()
        {
            logger.Info(PROGRAME + " qry account info");
            QryAccountInfoRequest request = RequestTemplate<QryAccountInfoRequest>.CliSendRequest(++requestid);
            request.Account = _account;

            SendPacket(request);
            return requestid;
        }


        /// <summary>
        /// 请求查询可开手数
        /// </summary>
        public int  ReqQryMaxOrderVol(string symbol,bool side = true,QSEnumOffsetFlag offset = QSEnumOffsetFlag.UNKNOWN)
        {
            logger.Info(PROGRAME + " qry max order vol,symbol:" + symbol);

            QryMaxOrderVolRequest request = RequestTemplate<QryMaxOrderVolRequest>.CliSendRequest(++requestid);
            request.Symbol = symbol;
            request.Side = side;
            request.OffsetFlag = offset;
            request.Account = _account;

            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询交易者信息
        /// </summary>
        public int  ReqQryInvestor()
        {
            logger.Info(PROGRAME + " qry investror info");

            QryInvestorRequest request = RequestTemplate<QryInvestorRequest>.CliSendRequest(++requestid);
            request.Account = _account;
            SendPacket(request);
            return requestid;
        }


        #region 基础数据接口
        /// <summary>
        /// 查询交易时间段
        /// </summary>
        public int ReqXQryMarketTime()
        {
            logger.Info(PROGRAME + " qry markettime");

            XQryMarketTimeRequest request = RequestTemplate<XQryMarketTimeRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询交易所
        /// </summary>
        public int  ReqXQryExchange()
        {
            logger.Info(PROGRAME + " qry exchange");

            XQryExchangeRequuest request = RequestTemplate<XQryExchangeRequuest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询品种
        /// </summary>
        public int  ReqXQrySecurity()
        {
            logger.Info(PROGRAME + " qry security");

            XQrySecurityRequest request = RequestTemplate<XQrySecurityRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询合约
        /// </summary>
        public int  ReqXQrySymbol(string symbol="")
        {
            logger.Info(PROGRAME + " qry symbol");

            XQrySymbolRequest request = RequestTemplate<XQrySymbolRequest>.CliSendRequest(++requestid);
            request.Symbol = symbol;
            SendPacket(request);
            return requestid;
        }

        #endregion


        #region 交易记录
        /// <summary>
        /// 查询隔夜持仓
        /// </summary>
        public int  ReqXQryYDPositon()
        {
            logger.Info(PROGRAME +" qry yd position");
            XQryYDPositionRequest request = RequestTemplate<XQryYDPositionRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询委托
        /// </summary>
        public int  ReqXQryOrder(int start=0,int end=0,string symbol="")
        {
            logger.Info(PROGRAME + "qry order");
            XQryOrderRequest request = RequestTemplate<XQryOrderRequest>.CliSendRequest(++requestid);
            request.Start = start;
            request.End = end;
            request.Symbol = symbol;
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询成交
        /// </summary>
        public int ReqXQryTrade(int start = 0, int end = 0, string symbol = "")
        {
            logger.Info(PROGRAME + "qry trade");
            XQryTradeRequest request = RequestTemplate<XQryTradeRequest>.CliSendRequest(++requestid);
            request.Start = start;
            request.End = end;
            request.Symbol = symbol;
            SendPacket(request);
            return requestid;
        }



        #endregion


        /// <summary>
        /// 请求注册行情数据
        /// </summary>
        public void ReqRegisterSymbol(string symbol)
        {
            //logger.Info(PROGRAME + " register symbols:" + string.Join(",", symbols));

            //RegisterSymbolTickRequest request = RequestTemplate<RegisterSymbolTickRequest>.CliSendRequest(++requestid);

            //request.SymbolList.AddRange(symbols);
            //SendPacket(request);
            //connecton.Subscribe(symbols);
            //return requestid;

            connecton.Subscribe(symbol);
        }

        public void ReqUnRegisterSymbol(string symbol)
        {
            connecton.UnSubscribe(symbol);
        }
        ///// <summary>
        ///// 请求注销行情数据
        ///// </summary>
        //public void ReqUnRegisterSymbols()
        //{
        //    UnregisterSymbolTickRequest request = RequestTemplate<UnregisterSymbolTickRequest>.CliSendRequest(++requestid);
        //    SendPacket(request);
        //}



        public void ReqContribRequest(string moduleid, string cmdstr, string args)
        {
            ContribRequest request = RequestTemplate<ContribRequest>.CliSendRequest(++requestid);
            request.ModuleID = moduleid;
            request.CMDStr = cmdstr;
            request.Parameters = args;

            SendPacket(request);
        }

        public void ReqChangePassowrd(string oldpass, string newpass)
        {
            logger.Info(PROGRAME + " req change password");

            ReqChangePasswordRequest request = RequestTemplate<ReqChangePasswordRequest>.CliSendRequest(++requestid);
            request.Account = _account;
            request.OldPassword = oldpass;
            request.NewPassword = newpass;

            SendPacket(request);
        }


        public void ReqQryOpenSize(string symbol)
        {
            QryMaxOrderVolRequest request = RequestTemplate<QryMaxOrderVolRequest>.CliSendRequest(++requestid);
            request.Symbol = symbol;
            request.Account = _account;

            SendPacket(request);
        }

        public void ReqXQryTickSnapShot(string symbol = "")
        {
            XQryTickSnapShotRequest request = RequestTemplate<XQryTickSnapShotRequest>.CliSendRequest(++requestid);
            request.Symbol = symbol;

            SendPacket(request);
        }

        /// <summary>
        /// 查询交易账户
        /// </summary>
        public int ReqXQryAccount()
        {
            XQryAccountRequest request = RequestTemplate<XQryAccountRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        #endregion


    }
}
