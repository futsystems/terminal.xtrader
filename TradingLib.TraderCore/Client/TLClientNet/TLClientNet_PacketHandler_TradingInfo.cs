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
        void CliOnTickNotify(TickNotify response)
        {
            CoreService.TradingInfoTracker.NotifyTick(response.Tick);
            CoreService.EventIndicator.FireTick(response.Tick);
        }

        /// <summary>
        /// 响应委托回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnOrderNotify(OrderNotify response)
        {
            logger.Info("got order notify:" + response.Order.GetOrderInfo());
            Order o = response.Order;
            if (o != null)
            {
                o.oSymbol = CoreService.BasicInfoTracker.GetSymbol(o.Symbol);
            }
            //if (o.oSymbol == null)
            //{
            //    logger.Warn("Order's Symbol do no exist");
            //    return;
            //}
            CoreService.TradingInfoTracker.NotifyOrder(o);
            CoreService.EventIndicator.FireOrder(o);
        }

        /// <summary>
        /// 响应成交回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnTradeNotify(TradeNotify response)
        {
            logger.Info("Trade Notify:" + response.Trade.GetTradeInfo());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Symbol);
            }
            //if (f.oSymbol == null)
            //{
            //    logger.Warn("Trade's Symbol do no exist");
            //    return;
            //}
            CoreService.TradingInfoTracker.NotifyFill(f);
            CoreService.EventIndicator.FireFill(f);
        }

        /// <summary>
        /// 响应隔夜持仓查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryYDPosition(RspXQryYDPositionResponse response)
        {
            logger.Debug("got xqry yd position response:" + response.ToString());
            PositionDetail pd = response.YDPosition;
            if (pd != null)
            {
                pd.oSymbol = CoreService.BasicInfoTracker.GetSymbol(pd.Symbol);
                //if (pd.oSymbol == null)
                //{
                //    logger.Warn("PositionDetail's Symbol do no exist");
                //    return;
                //}
            }
            
            CoreService.EventQry.FireRspXQryYDPositionResponse(pd, response.RspInfo, response.RequestID, response.IsLast);
            //CoreService.TradingInfoTracker.GotYDPosition(response.YDPosition, response.IsLast);
        }

        /// <summary>
        /// 响应委托查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryOrder(RspXQryOrderResponse response)
        {
            logger.Debug("got xqry order response:" + response.ToString());
            Order o = response.Order;
            if (o != null)
            {
                o.oSymbol = CoreService.BasicInfoTracker.GetSymbol(o.Symbol);
                //if (o.oSymbol == null)
                //{
                //    logger.Warn("Order's Symbol do no exist");
                //    return;
                //}
            }
            
            CoreService.EventQry.FireRspXQryOrderResponse(o, response.RspInfo, response.RequestID, response.IsLast);
            //CoreService.TradingInfoTracker.GotOrder(response.Order, response.IsLast);
        }

        /// <summary>
        /// 响应成交查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryTrade(RspXQryTradeResponse response)
        {
            logger.Debug("got xqry trade response:" + response.ToString());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Symbol);
                //if (f.oSymbol == null)
                //{
                //    logger.Warn("Trade's Symbol do no exist");
                //    return;
                //}
            }
            
            CoreService.EventQry.FireRspXQryFillResponese(f, response.RspInfo, response.RequestID, response.IsLast);
            //CoreService.TradingInfoTracker.GotTrade(response.Trade, response.IsLast);
        }

        /// <summary>
        /// 响应帐户查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnQryAccountInfo(RspQryAccountInfoResponse response)
        {
            logger.Debug("got qry account info response:" + response.ToString());
            CoreService.TradingInfoTracker.GotAccountInfo(response.AccInfo, response.IsLast);
        }

        /// <summary>
        /// 响应行情快照查询
        /// </summary>
        /// <param name="response"></param>
        void CliOnXQryTickSnapShot(RspXQryTickSnapShotResponse response)
        {
            logger.Debug("got qry ticksnapshot response:" + response.ToString());
            if(response.Tick != null)
            {
                CoreService.EventIndicator.FireTick(response.Tick);
            }
        }

        /// <summary>
        /// 委托异常汇报
        /// </summary>
        /// <param name="response"></param>
        void CliOnErrorOrderNotify(ErrorOrderNotify response)
        {
            logger.Debug("got error order:" + response.ToString());
            CoreService.EventIndicator.FireErrorOrder(response.Order, response.RspInfo);

        }

        void CliOnErrorOrderActionNotify(ErrorOrderActionNotify response)
        {
            logger.Debug("got error order actoin:" + response.ToString());
            CoreService.EventIndicator.FireErrorOrderAction(response.OrderAction, response.RspInfo);
        }
    }
}
