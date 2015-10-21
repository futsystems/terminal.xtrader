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
        void connecton_OnPacketEvent(IPacket packet)
        {
            switch (packet.Type)
            {
                //Tick数据
                case MessageTypes.TICKNOTIFY:
                    CliOnTickNotify(packet as TickNotify);
                    break;
                //昨日持仓数据
                case MessageTypes.OLDPOSITIONNOTIFY:
                    CliOnOldPositionNotify(packet as HoldPositionNotify);
                    break;
                //委托回报
                case MessageTypes.ORDERNOTIFY:
                    CliOnOrderNotify(packet as OrderNotify);
                    break;
                case MessageTypes.ERRORORDERNOTIFY:
                    CliOnErrorOrderNotify(packet as ErrorOrderNotify);
                    break;
                //成交回报
                case MessageTypes.EXECUTENOTIFY:
                    CliOnTradeNotify(packet as TradeNotify);
                    break;
                //持仓更新回报
                case MessageTypes.POSITIONUPDATENOTIFY:
                    CliOnPositionUpdateNotify(packet as PositionNotify);
                    break;
                //委托操作回报
                case MessageTypes.ORDERACTIONNOTIFY:
                    CliOnOrderAction(packet as OrderActionNotify);
                    break;

                case MessageTypes.ERRORORDERACTIONNOTIFY:
                    CliOnErrorOrderActionNotify(packet as ErrorOrderActionNotify);
                    break;

                case MessageTypes.CHANGEPASSRESPONSE:
                    CliOnChangePass(packet as RspReqChangePasswordResponse);
                    break;



                #region 查询
                case MessageTypes.ORDERRESPONSE://查询委托回报
                    CliOnRspQryOrderResponse(packet as RspQryOrderResponse);
                    break;
                case MessageTypes.TRADERESPONSE://查询成交回报
                    CliOnRspQryTradeResponse(packet as RspQryTradeResponse);
                    break;
                case MessageTypes.POSITIONRESPONSE://查询持仓回报
                    CliOnRspQryPositionResponse(packet as RspQryPositionResponse);
                    break;

                case MessageTypes.ACCOUNTINFORESPONSE://帐户信息回报
                    CliOnQryAccountInfo(packet as RspQryAccountInfoResponse);
                    break;
                case MessageTypes.INVESTORRESPONSE:
                    CliOnRspQryInvestorResponse(packet as RspQryInvestorResponse);
                    break;

                case MessageTypes.MAXORDERVOLRESPONSE: //最大可开数量回报
                    CliOnMaxOrderVol(packet as RspQryMaxOrderVolResponse);
                    break;
                case MessageTypes.SETTLEINFOCONFIRMRESPONSE://结算确认回报
                    CliOnSettleInfoConfirm(packet as RspQrySettleInfoConfirmResponse);
                    break;

                case MessageTypes.SETTLEINFORESPONSE://结算信息会回报
                    CliOnSettleInfo(packet as RspQrySettleInfoResponse);
                    break;


                case MessageTypes.XMARKETTIMERESPONSE://交易时间段回报
                    CliOnXMarketTime(packet as RspXQryMarketTimeResponse);
                    break;

                case MessageTypes.XEXCHANGERESPNSE://交易所回报
                    CliOnXExchange(packet as RspXQryExchangeResponse);
                    break;

                case MessageTypes.XSECURITYRESPONSE://品种回报
                    CliOnXSecurity(packet as RspXQrySecurityResponse);
                    break;

                case MessageTypes.XSYMBOLRESPONSE://合约回报
                    CliOnXSymbol(packet as RspXQrySymbolResponse);
                    break;
                case MessageTypes.XYDPOSITIONRESPONSE://隔夜持仓回报
                    CliOnXQryYDPosition(packet as RspXQryYDPositionResponse);
                    break;
                case MessageTypes.XORDERRESPONSE://委托查询回报
                    CliOnXQryOrder(packet as RspXQryOrderResponse);
                    break;
                case MessageTypes.XTRADERESPONSE://成交查询回报
                    CliOnXQryTrade(packet as RspXQryTradeResponse);
                    break;
                case MessageTypes.XTICKSNAPSHOTRESPONSE://行情快照回报
                    CliOnXQryTickSnapShot(packet as RspXQryTickSnapShotResponse);
                    break;
                #endregion

                default:
                    logger.Error("Packet Handler Not Set, Packet:" + packet.ToString());
                    break;
            }
        }
    }
}
