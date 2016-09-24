using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {

        void CliOnMaxOrderVol(RspQryMaxOrderVolResponse response)
        {
            logger.Debug("Got XQry MaxOrderVol Response:" + response.ToString());
            CoreService.EventOther.FireRspQryMaxOrderVolResponse(response);
        }


        /// <summary>
        /// 响应修改密码回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnChangePass(RspReqChangePasswordResponse response)
        {
            logger.Debug("Got ChangePass Response:" + response.ToString());
            CoreService.EventOther.FireRspReqChangePasswordResponse(response);
            if (IsRspInfoError(response.RspInfo))
            {
                PromptMessage msg = new PromptMessage("修改密码错误", "{0},ErrorCode[{1}]".Put(response.RspInfo.ErrorMessage, response.RspInfo.ErrorID));
                CoreService.EventCore.FirePromptMessageEvent(msg);
            }
            else
            {
                PromptMessage msg = new PromptMessage("修改密码成功", "密码修改成功，下次交易请用新密码登入。");
                CoreService.EventCore.FirePromptMessageEvent(msg);
            }
        }
        
    }
}
