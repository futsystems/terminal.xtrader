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

            //第一次登入成功 请求基础数据
            if (_firstlogin && response.Authorized)
            {
                _firstlogin = false;
                //请求市场交易时间段
                CoreService.BasicInfoTracker.ResumeData();
            }
        }


        void CliOnXMarketTime(RspXQryMarketTimeResponse response)
        {
            logger.Debug("Got Markettime Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotMarketTime(response.MarketTime, response.IsLast);
        }

        void CliOnXExchange(RspXQryExchangeResponse response)
        {
            logger.Debug("Got Exchange Response:" + response.ToString());
            CoreService.BasicInfoTracker.GotExchange(response.Exchange, response.IsLast);
        }

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
            CoreService.EventQry.FireRspXQryPositionDetailResponse(response);
        }

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
            CoreService.EventQry.FireRspXQrySymbolResponse(target, response.RspInfo, response.RequestID, response.IsLast);

        }
    }
}
