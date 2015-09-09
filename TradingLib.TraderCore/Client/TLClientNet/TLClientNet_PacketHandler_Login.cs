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
        bool _firstlogin = true;
        /// <summary>
        /// 响应底层暴露上来的登入回报事件
        /// </summary>
        /// <param name="response"></param>
        void connecton_OnLoginResponse(LoginResponse response)
        {
            logger.Info(" got loginresponse:" + response.ToString());
            if (response.Authorized)
            {
                _account = response.Account;
                _tradingday = response.Date;
                CoreService.Account = response.Account;
            }
            CoreService.EventCore.FireLoginEvent(response);

            //第一层成功登入 需要请求基础数据
            if (_firstlogin && response.Authorized)
            {
                _firstlogin = false;
                //请求市场交易时间段
                this.ReqXQryMarketTime();
            }

        }

    }
}
