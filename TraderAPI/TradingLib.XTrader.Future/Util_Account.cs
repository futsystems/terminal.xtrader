using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public static class Util_Account
    {
        /// <summary>
        /// 获得某个账户将对应品种货币转换成账户货币的汇率系数
        /// </summary>
        /// <param name="account"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static decimal GetExchangeRate(this AccountItem account, CurrencyType srcCurrency)
        {
            //品种货币与帐户货币一直则返回1
            if (srcCurrency == account.Currency) return 1;

            //帐户货币为主货币
            if (account.Currency == CurrencyType.RMB)
            {
                //获得品种货币对应的汇率 返回中间汇率
                ExchangeRate secRate =  CoreService.BasicInfoTracker.GetExchangeRate(srcCurrency);// BasicTracker.ExchangeRateTracker[TLCtxHelper.ModuleSettleCentre.Tradingday, sec.Currency];
                if (secRate == null) return 1;//没有找到品种汇率 则默认返回1
                return secRate.IntermediateRate;
            }
            else
            {
                ExchangeRate secRate = CoreService.BasicInfoTracker.GetExchangeRate(srcCurrency);
                ExchangeRate accRate = CoreService.BasicInfoTracker.GetExchangeRate(account.Currency);
                if (secRate == null || accRate == null) return 1;
                //将品种货币换算成系统基础货币然后再换算成帐户货币
                return secRate.IntermediateRate / accRate.IntermediateRate;
            }

        }
    }
}
