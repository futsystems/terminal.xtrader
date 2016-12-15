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
        int requestid = 0;
        /// <summary>
        /// 发送业务数据包
        /// </summary>
        /// <param name="packet"></param>
        void SendPacket(IPacket packet)
        {
            //权限或者登入状态检查
            if (connecton != null && connecton.IsConnected)
            {
                connecton.TLSend(packet);
            }
        }


        #region API操作接口
        int _orderref = 1;
        object _orderrefobj = new object();
        string NextOrderRef
        {
            get
            {
                lock (_orderrefobj)
                {
                    string refstr = _orderref.ToString();
                    _orderref++;
                    return refstr;
                }
            }
        }

        /// <summary>
        /// 发送委托
        /// </summary>
        /// <param name="order"></param>
        public int  ReqOrderInsert(Order order)
        {
            logger.Info("Send Order:" + order.GetOrderInfo());

            order.Account = _account;

            OrderInsertRequest request = RequestTemplate<OrderInsertRequest>.CliSendRequest(++requestid);
            request.Order = order;
            request.Order.OrderRef = NextOrderRef;
            
            SendPacket(request);
            return requestid;
        }
        /// <summary>
        /// 提交委托操作
        /// </summary>
        /// <param name="action"></param>
        public int ReqOrderAction(OrderAction action)
        {
            logger.Info("Send OrderAction:" + action.ToString());

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
            logger.Info("Cancel Order:" + id.ToString());

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
        public int  ReqLogin(string loginid, string pass,string productInfo, int logintype = 1)
        {
            logger.Info(string.Format("Request Login Account:{0} Pass:{1}",loginid,pass));
            LoginRequest request = RequestTemplate<LoginRequest>.CliSendRequest(++requestid);
            request.LoginID = loginid;
            request.Passwd = pass;
            request.LoginType = 1;
            request.ProductInfo = productInfo;
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


        #region 基础数据接口
        /// <summary>
        /// 查询交易时间段
        /// </summary>
        public int ReqXQryMarketTime()
        {
            logger.Info("Qry MarketTime");

            XQryMarketTimeRequest request = RequestTemplate<XQryMarketTimeRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询交易所
        /// </summary>
        public int  ReqXQryExchange()
        {
            logger.Info("Qry Exchange");

            XQryExchangeRequuest request = RequestTemplate<XQryExchangeRequuest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询品种
        /// </summary>
        public int  ReqXQrySecurity()
        {
            logger.Info("Qry Security");

            XQrySecurityRequest request = RequestTemplate<XQrySecurityRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询合约
        /// </summary>
        public int  ReqXQrySymbol(string exchange,string symbol)
        {
            logger.Info("Qry Symbol Exchange:" + exchange + " Code:" + symbol);

            XQrySymbolRequest request = RequestTemplate<XQrySymbolRequest>.CliSendRequest(++requestid);
            request.Exchange = exchange;
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
            logger.Info("Qry YDPosition");
            XQryYDPositionRequest request = RequestTemplate<XQryYDPositionRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询委托
        /// </summary>
        public int  ReqXQryOrder(int start=0,int end=0,string symbol="")
        {
            logger.Info("Qry Order");
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
            logger.Info("Qry Trade");
            XQryTradeRequest request = RequestTemplate<XQryTradeRequest>.CliSendRequest(++requestid);
            request.Start = start;
            request.End = end;
            request.Symbol = symbol;
            SendPacket(request);
            return requestid;
        }



        #endregion


        /// <summary>
        /// 订阅合约实时行情
        /// </summary>
        public int ReqRegisterSymbol(Symbol sym)
        {
            logger.Info(string.Format("Subscribe market data for symbol:{0}",sym.Symbol));
            RegisterSymbolTickRequest request = RequestTemplate<RegisterSymbolTickRequest>.CliSendRequest(++requestid);
            request.Exchange = sym.Exchange;
            request.SymbolList.Add(sym.Symbol);
            this.SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 注销合约实时行情
        /// </summary>
        /// <param name="symbol"></param>
        public int ReqUnRegisterSymbol(Symbol sym)
        {
            logger.Info(string.Format("Unsubscribe market data for symbol:{0}",sym.Symbol));
            //int reqid = NextRequestID;
            UnregisterSymbolTickRequest request = RequestTemplate<UnregisterSymbolTickRequest>.CliSendRequest(++requestid);
            request.Exchange = sym.Exchange;
            request.SymbolList.Add(sym.Symbol);
            this.SendPacket(request);
            return requestid;
        }

        ///// <summary>
        ///// 请求注册行情数据
        ///// </summary>
        //public void ReqRegisterSymbol(string symbol)
        //{
        //    //if (!connecton.IsTickConnected) return;
        //    //connecton.Subscribe(symbol);
        //}

        ///// <summary>
        ///// 请求注销合约
        ///// </summary>
        ///// <param name="symbol"></param>
        //public void ReqUnRegisterSymbol(string symbol)
        //{
        //    //if (!connecton.IsTickConnected) return;
        //    //connecton.UnSubscribe(symbol);
        //}

        /// <summary>
        /// 请求修改密码
        /// </summary>
        /// <param name="oldpass"></param>
        /// <param name="newpass"></param>
        public void ReqChangePassowrd(string oldpass, string newpass)
        {
            logger.Info(string.Format("Request Change Password  Old:{0} New:{1}", oldpass, newpass));

            ReqChangePasswordRequest request = RequestTemplate<ReqChangePasswordRequest>.CliSendRequest(++requestid);
            request.Account = _account;
            request.OldPassword = oldpass;
            request.NewPassword = newpass;

            SendPacket(request);
        }


        /// <summary>
        /// 请求查询可开手数
        /// </summary>
        public int ReqXQryMaxOrderVol(string exchange,string symbol, bool side = true, QSEnumOffsetFlag offset = QSEnumOffsetFlag.UNKNOWN)
        {
            logger.Info("Qry Max Order Vol,Symbol:" + symbol);

            XQryMaxOrderVolRequest request = RequestTemplate<XQryMaxOrderVolRequest>.CliSendRequest(++requestid);
            request.Exchange = exchange;
            request.Symbol = symbol;
            request.Side = side;
            request.OffsetFlag = offset;
            request.Account = _account;

            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询行情快照
        /// </summary>
        /// <param name="symbol"></param>
        public void ReqXQryTickSnapShot(string exchange,string symbol)
        {
            logger.Info("Qry TickSnapshot:" + symbol);
            XQryTickSnapShotRequest request = RequestTemplate<XQryTickSnapShotRequest>.CliSendRequest(++requestid);
            request.Symbol = symbol;
            request.Exchange = exchange;

            SendPacket(request);
        }

        /// <summary>
        /// 查询交易账户
        /// </summary>
        public int ReqXQryAccount()
        {
            logger.Info("Qry AccountInfo");
            XQryAccountRequest request = RequestTemplate<XQryAccountRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }


        /// <summary>
        /// 请求帐户财务信息
        /// </summary>
        public int ReqXQryAccountFinance()
        {
            logger.Info("Qry Account Finance");
            XQryAccountFinanceRequest request = RequestTemplate<XQryAccountFinanceRequest>.CliSendRequest(++requestid);
            SendPacket(request);
            return requestid;
        }

        /// <summary>
        /// 查询结算单
        /// </summary>
        /// <param name="tradingday"></param>
        /// <returns></returns>
        public int ReqXQrySettlement(int tradingday)
        {
            logger.Info("Qry Settlement:" + tradingday.ToString());
            XQrySettleInfoRequest request = RequestTemplate<XQrySettleInfoRequest>.CliSendRequest(++requestid);
            request.Tradingday = tradingday;
            SendPacket(request);
            return requestid;

        }

        /// <summary>
        /// 查询汇率数据
        /// </summary>
        /// <returns></returns>
        public int ReqXQryExchangeRate()
        {
            logger.Info("Qry ExchangeRate");
            XQryExchangeRateRequest request = RequestTemplate<XQryExchangeRateRequest>.CliSendRequest(++requestid);

            SendPacket(request);
            return requestid;
        }


        /// <summary>
        /// 查询持仓明细
        /// </summary>
        /// <returns></returns>
        public int ReqXQryPositionDetail()
        {
            logger.Info("Qry PositionDetail");
            XQryPositionDetailRequest request = RequestTemplate<XQryPositionDetailRequest>.CliSendRequest(++requestid);

            SendPacket(request);
            return requestid;
        }
        #endregion


    }
}
