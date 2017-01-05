﻿using System.Collections.Generic;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.TraderCore
{
    public partial class TLClientNet
    {

        /// <summary>
        /// 响应行情
        /// </summary>
        /// <param name="response"></param>
        //void CliOnTickNotify(TickNotify response)
        //{
        //    CoreService.EventIndicator.FireTick(response.Tick);
        //}

        /// <summary>
        /// 响应委托回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnOrderNotify(OrderNotify response)
        {
            logger.Info("Got Order Notify:" + response.Order.GetOrderInfo());
            Order o = response.Order;
            if (o != null)
            {
                o.oSymbol = CoreService.BasicInfoTracker.GetSymbol(o.Exchange,o.Symbol);
            }
            CoreService.EventIndicator.FireOrder(o);
        }

        /// <summary>
        /// 响应成交回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnTradeNotify(TradeNotify response)
        {
            logger.Info("Got Trade Notify:" + response.Trade.GetTradeInfo());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Exchange,f.Symbol);
            }
            
            CoreService.EventIndicator.FireFill(f);
        }

        /// <summary>
        /// 响应持仓更新回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnPositionUpdateNotify(PositionNotify response)
        {
            logger.Info("Got Postion Notify:" + response.Position.ToString());
            CoreService.EventIndicator.FirePositionNotify(response.Position);
            
        }

        /// <summary>
        /// 获得委托操作会回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnOrderAction(OrderActionNotify response)
        {
            logger.Info("Got Order Action Notify:" + response.ToString());
        }



        /// <summary>
        /// 委托异常汇报
        /// </summary>
        /// <param name="response"></param>
        void CliOnErrorOrderNotify(ErrorOrderNotify response)
        {
            logger.Debug("Got Order Error Notify:" + response.ToString());
            CoreService.EventIndicator.FireErrorOrder(response.Order, response.RspInfo);
            if (IsRspInfoError(response.RspInfo))
            {
                PromptMessage msg = new PromptMessage("提交委托异常", "{0},ErrorCode[{1}]".Put(response.RspInfo.ErrorMessage, response.RspInfo.ErrorID));
                CoreService.EventCore.FirePromptMessageEvent(msg);
            }
        }

        void CliOnErrorOrderActionNotify(ErrorOrderActionNotify response)
        {
            logger.Debug("Got Order Actoin Error Notify:" + response.ToString());
            CoreService.EventIndicator.FireErrorOrderAction(response.OrderAction, response.RspInfo);
            if (IsRspInfoError(response.RspInfo))
            {
                PromptMessage msg = new PromptMessage("提交委托操作异常", "{0},ErrorCode[{1}]".Put(response.RspInfo.ErrorMessage, response.RspInfo.ErrorID));
                CoreService.EventCore.FirePromptMessageEvent(msg);
            }
        }




        /// <summary>
        /// 响应隔夜持仓查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryYDPosition(RspXQryYDPositionResponse response)
        {
            logger.Debug("Got XQry YDPosition Response:" + response.ToString());
            PositionDetail pd = response.YDPosition;
            if (pd != null)
            {
                pd.oSymbol = CoreService.BasicInfoTracker.GetSymbol(pd.Exchange,pd.Symbol);
            }

            CoreService.EventHub.FireRspXQryYDPositionResponse(pd, response.RspInfo, response.RequestID, response.IsLast);
        }

        /// <summary>
        /// 响应委托查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryOrder(RspXQryOrderResponse response)
        {
            logger.Debug("Got XQry Order Response:" + response.ToString());
            Order o = response.Order;
            if (o != null)
            {
                o.oSymbol = CoreService.BasicInfoTracker.GetSymbol(o.Exchange,o.Symbol);
            }

            CoreService.EventHub.FireRspXQryOrderResponse(o, response.RspInfo, response.RequestID, response.IsLast);
        }

        /// <summary>
        /// 响应成交查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryTrade(RspXQryTradeResponse response)
        {
            logger.Debug("Got XQry Trade Response:" + response.ToString());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Exchange,f.Symbol);
            }

            CoreService.EventHub.FireRspXQryFillResponese(f, response.RspInfo, response.RequestID, response.IsLast);
        }

        /// <summary>
        /// 响应行情快照查询
        /// 这里行情快照直接按照新到行情的处理方式进行处理的 感觉不是很合理，会导致出发某些由行情触发的事件
        /// 在需要行情快照数据的地方应该自行填充数据
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryTickSnapShot(RspXQryTickSnapShotResponse response)
        {
            logger.Debug("Got XQry TickSnapshot Response:" + response.ToString());
            if(response.Tick != null)
            {
                CoreService.EventIndicator.FireTick(response.Tick);
            }
        }

        /// <summary>
        /// 查询账户对象
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryAccount(RspXQryAccountResponse response)
        {
            logger.Debug("Got XQry AccountInfo Response:" + response.ToString());
            CoreService.EventHub.FireRspXQryAccountResponse(response.Account, response.RspInfo, response.RequestID, response.IsLast);
        }

        /// <summary>
        /// 查询最大可开手数量
        /// </summary>
        /// <param name="response"></param>
        void CliOnMaxOrderVol(RspXQryMaxOrderVolResponse response)
        {
            logger.Debug("Got XQry MaxOrderVol Response:" + response.ToString());
            CoreService.EventHub.FireRspXQryMaxOrderVolResponse(response);
        }

        /// <summary>
        /// 查询账户财务信息
        /// </summary>
        /// <param name="response"></param>
        void CliOnAccountFinance(RspXQryAccountFinanceResponse response)
        {
            logger.Debug("Got XQry Account Finance Response:" + response.ToString());
            CoreService.EventHub.FireRspXQryAccountFinanceEvent(response);
        }

        /// <summary>
        /// 查询结算单回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQrySettlement(RspXQrySettleInfoResponse response)
        {
            logger.Debug("Got XQry Settlement Response:" + response.ToString());
            CoreService.EventHub.FireRspXQrySettlementResponse(response);
        }

        /// <summary>
        /// 响应修改密码回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnChangePass(RspReqChangePasswordResponse response)
        {
            logger.Debug("Got ChangePass Response:" + response.ToString());
            CoreService.EventHub.FireRspReqChangePasswordResponse(response);
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