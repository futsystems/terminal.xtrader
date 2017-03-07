using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static int ReqWithdraw(this TLClientNet client, decimal val)
        {
            return client.ReqContribRequest("APIService", "Withdraw", val.ToString());
        }
    }
}
