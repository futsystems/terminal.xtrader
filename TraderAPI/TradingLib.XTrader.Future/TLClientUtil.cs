using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public static class TLClientUtil
    {
        public static int  ReqUpdateAccountProfile(this TLClientNet client, AccountProfile profile)
        {
            return client.ReqContribRequest("AccountManager", "UpdateAccountProfile", profile);
        }

        public static int ReqQryAccountProfile(this TLClientNet client)
        {
            return client.ReqContribRequest("AccountManager", "QryAccountProfile", CoreService.TradingInfoTracker.Account.Account);
        }

        public static int ReqQryContractBank(this TLClientNet client)
        {
            return client.ReqContribRequest("MsgExch", "QryContractBank", "");
        }

        public static int ReqDeposit(this TLClientNet client,decimal val)
        {
            return client.ReqContribRequest("APIService", "Deposit", val.ToString());
        }

        public static int ReqDeposit2(this TLClientNet client, decimal val,EnumBusinessType type)
        {
            return client.ReqContribRequest("APIService", "Deposit2", new { amount=val,business_type=type});
        }

        public static int ReqWithdraw(this TLClientNet client, decimal val)
        {
            return client.ReqContribRequest("APIService", "Withdraw", val.ToString());
        }

        public static int ReqWithdraw2(this TLClientNet client, decimal val, EnumBusinessType type)
        {
            return client.ReqContribRequest("APIService", "Withdraw2", new { amount = val, business_type = type });
        }
    }

    public enum EnumBusinessType
    { 
        /// <summary>
        /// 普通类别
        /// </summary>
        [Description("普通出入金")]
        Normal,

        /// <summary>
        /// 配资入金 资金会根据账户资金按杠杆比例自动调整优先资金
        /// </summary>
        [Description("配资入金")]
        LeverageDeposit,

        /// <summary>
        /// 减少配资 减少账户优先资金 用于降低风险
        /// </summary>
        [Description("减少配资")]
        CreditWithdraw
    }
}
