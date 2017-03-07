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
        /// 连接断开
        /// </summary>
        void connecton_OnDisconnectEvent()
        {
            CoreService.EventCore.FireDisconnectedEvent();
        }

        /// <summary>
        /// 连接建立
        /// </summary>
        void connecton_OnConnectEvent()
        {
            CoreService.EventCore.FireConnectedEvent();
        }

        /// <summary>
        /// 数据包到达
        /// </summary>
        /// <param name="packet"></param>
        void connecton_OnPacketEvent(IPacket packet)
        {
            switch (packet.Type)
            {
                //Tick数据
                //case MessageTypes.TICKNOTIFY:
                //    CliOnTickNotify(packet as TickNotify);
                //    break;
                //登入回报
                case MessageTypes.LOGINRESPONSE:
                    CliOnLogin(packet as LoginResponse);
                    break;
                //委托回报
                case MessageTypes.ORDERNOTIFY:
                    CliOnOrderNotify(packet as OrderNotify);
                    break;
                //委托异常回报
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
                //委托操作异常回报
                case MessageTypes.ERRORORDERACTIONNOTIFY:
                    CliOnErrorOrderActionNotify(packet as ErrorOrderActionNotify);
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
                
                case MessageTypes.XACCOUNTRESPONSE://交易账户回报
                    CliOnXQryAccount(packet as RspXQryAccountResponse);
                    break;

                case MessageTypes.XQRYMAXORDERVOLRESPONSE: //最大可开数量回报
                    CliOnMaxOrderVol(packet as RspXQryMaxOrderVolResponse);
                    break;

                case MessageTypes.XQRYACCOUNTFINANCERESPONSE://账户财务数据回报
                    CliOnAccountFinance(packet as RspXQryAccountFinanceResponse);
                    break;

                case MessageTypes.CHANGEPASSRESPONSE://修改密码回报
                    CliOnChangePass(packet as RspReqChangePasswordResponse);
                    break;

                case MessageTypes.XSETTLEINFORESPONSE://结算单回报
                    CliOnXQrySettlement(packet as RspXQrySettleInfoResponse);
                    break;

                case MessageTypes.XQRYEXCHANGERATERESPONSE://查询汇率信息回报
                    CliOnXQryExchangeRate(packet as RspXQryExchangeRateResponse);
                    break;
                case MessageTypes.XPOSITIONDETAILRESPONSE://查询持仓明细回报
                    CliOnXQryPositionDetails(packet as RspXQryPositionDetailResponse);
                    break;
                case MessageTypes.CONTRIBRESPONSE:
                    CliOnContribResponse(packet as RspContribResponse);
                    break;
                default:
                    logger.Error("Packet Handler Not Set, Packet:" + packet.ToString());
                    break;
            }
        }
    }
}
