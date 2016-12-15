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
        /// <summary>
        /// 响应登入回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnLogin(LoginResponse response)
        {
            logger.Info("Got Login Response:" + response.ToString());
            if (response.Authorized)
            {
                _account = response.Account;
                _tradingday = response.TradingDay;
                _clientID = response.ClientID;
                _frontID = response.FrontIDi;
                _sessionID = response.SessionIDi;
            }
            CoreService.EventCore.FireLoginEvent(response);
            if (response.Authorized && !CoreService.BasicInfoTracker.Inited)
            {
                //请求市场交易时间段
                CoreService.BasicInfoTracker.ResumeData();
            }
        }

        /// <summary>
        /// 响应交易时间段查询回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXMarketTime(RspXQryMarketTimeResponse response)
        {
            logger.Debug("Got Markettime Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotMarketTime(response.MarketTime, response.IsLast);
        }

        /// <summary>
        /// 响应交易所查询回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXExchange(RspXQryExchangeResponse response)
        {
            logger.Debug("Got Exchange Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotExchange(response.Exchange, response.IsLast);
        }

        /// <summary>
        /// 响应品种查询回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXSecurity(RspXQrySecurityResponse response)
        {
            logger.Debug("Got Security Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotSecurity(response.SecurityFaimly, response.IsLast);
        }

        /// <summary>
        /// 查询汇率回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryExchangeRate(RspXQryExchangeRateResponse response)
        {
            logger.Debug("Got XQry ExchangeRate Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotExchagneRate(response.ExchangeRate, response.IsLast);
            
        }

        /// <summary>
        /// 查询持仓明细回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryPositionDetails(RspXQryPositionDetailResponse response)
        {
            logger.Debug("Got XQry PositionDetail Response:" + response.ToString());
            CoreService.EventHub.FireRspXQryPositionDetailResponse(response);
        }

        /// <summary>
        /// 查询合约回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXSymbol(RspXQrySymbolResponse response)
        {
            logger.Debug("Got Symbol Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotSymbol(response.Symbol, response.IsLast);

            //触发查询回调
            Symbol target = null;
            if (response.Symbol != null)
            {
                //不能使用Exchange + Symbol来查找 初始化查询过程中 合约可能没有被正常初始化
                target = CoreService.BasicInfoTracker.GetSymbol(response.Symbol.ID);
            }
            CoreService.EventHub.FireRspXQrySymbolResponse(target, response.RspInfo, response.RequestID, response.IsLast);

        }
    }
}
