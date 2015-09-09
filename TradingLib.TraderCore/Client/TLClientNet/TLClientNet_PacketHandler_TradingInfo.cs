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
            CoreService.TradingInfoTracker.NotifyOrder(o);
            CoreService.EventIndicator.FireOrder(o);
        }

        /// <summary>
        /// 响应成交回报
        /// </summary>
        /// <param name="response"></param>
        void CliOnTradeNotify(TradeNotify response)
        {
            logger.Info("got trade notify:" + response.Trade.GetTradeInfo());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Symbol);
            }
            CoreService.TradingInfoTracker.NotifyFill(f);
            CoreService.EventIndicator.FireFill(f);
        }

        void CliOnXQryYDPosition(RspXQryYDPositionResponse response)
        {
            logger.Debug("got xqry yd position response:" + response.ToString());
            PositionDetail pd = response.YDPosition;
            if (pd != null)
            {
                pd.oSymbol = CoreService.BasicInfoTracker.GetSymbol(pd.Symbol);
            }
            CoreService.TradingInfoTracker.GotYDPosition(response.YDPosition, response.IsLast);
        }

        void CliOnXQryOrder(RspXQryOrderResponse response)
        {
            logger.Debug("got xqry order response:" + response.ToString());
            Order o = response.Order;
            if (o != null)
            {
                o.oSymbol = CoreService.BasicInfoTracker.GetSymbol(o.Symbol);
            }
            CoreService.TradingInfoTracker.GotOrder(response.Order, response.IsLast);
        }

        void CliOnXQryTrade(RspXQryTradeResponse response)
        {
            logger.Debug("got xqry trade response:" + response.ToString());
            Trade f = response.Trade;
            if (f != null)
            {
                f.oSymbol = CoreService.BasicInfoTracker.GetSymbol(f.Symbol);
            }
            CoreService.TradingInfoTracker.GotTrade(response.Trade, response.IsLast);
        }
    }
}
