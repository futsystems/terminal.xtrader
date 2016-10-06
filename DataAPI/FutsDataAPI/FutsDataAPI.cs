using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;
using TradingLib.MarketData;
using Common.Logging;

namespace DataAPI.Futs
{
    public partial class FutsDataAPI : IMarketDataAPI
    {
        ILog logger = LogManager.GetLogger("FutsDataAPI");


        Dictionary<string, MDSymbol> symbolMap = new Dictionary<string, MDSymbol>();
        List<BlockInfo> blockInfoList = new List<BlockInfo>();
        public FutsDataAPI()
        {
            
            DataCoreService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            DataCoreService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);
            DataCoreService.EventHub.OnLoginEvent += new Action<LoginResponse>(EventHub_OnLoginEvent);
            DataCoreService.EventHub.OnInitializedEvent += new Action(EventHub_OnInitializedEvent);


        }

        void EventHub_OnLoginEvent(LoginResponse obj)
        {
            MDLoginResponse response = new MDLoginResponse();
            response.LoginSuccess = obj.Authorized;
            response.TradingDay = obj.Date;
            response.ErrorCode = obj.RspInfo.ErrorID.ToString();
            response.ErrorMessage = obj.RspInfo.ErrorMessage;
            MDService.EventHub.FireLoginEvent(response);

            //登入成功 且未初始化 则查询基础数据
            if (!DataCoreService.Initialized && obj.Authorized)
            {
                DataCoreService.DataClient.QryMarketTime();
            }
        }

        void EventHub_OnDisconnectedEvent()
        {
            MDService.EventHub.FireDisconnectedEvent();
        }

        void EventHub_OnConnectedEvent()
        {
            MDService.EventHub.FireConnectedEvent();
        }

        void EventHub_OnInitializedEvent()
        {
            foreach (var target in DataCoreService.DataClient.Symbols)
            {
                MDSymbol symbol = new MDSymbol();
                symbol.Symbol = target.Symbol;
                symbol.SecCode = target.SecurityFamily.Code;
                symbol.Name = target.GetName();
                symbol.Currency = MDCurrency.RMB;
                symbol.Exchange = target.Exchange;
                symbol.Multiple = target.Multiple;
                symbol.SecurityType = MDSecurityType.FUT;
                symbol.SizeRate = 1;
                symbol.NCode = 0;
                symbol.SortKey = target.Month;
                symbolMap.Add(symbol.UniqueKey, symbol);
            }

            foreach (var exchange in DataCoreService.DataClient.Exchanges)
            {
                string k = exchange.EXCode;
                blockInfoList.Add(new BlockInfo(exchange.EXCode, new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                    =>
                {
                    if (symbol.Exchange == k)
                    {
                        return true;
                    }
                    return false;
                }), 2));
            }


            MDService.EventHub.FireInitializeStatusEvent("基础数据初始化完毕");

            MDService.Initialize();//执行初始化完毕操作
        }


        /// <summary>
        /// 获得所有合约
        /// </summary>
        public IEnumerable<MDSymbol> Symbols 
        {
            get
            {
                return symbolMap.Values;
            }
        }

        public IEnumerable<BlockInfo> BlockInfos
        {
            get { return blockInfoList; }
        }

        public bool Connected
        {
            get { return true;
            }
        }

        public void Connect(string[] hosts, int port)
        {
            MDService.EventHub.FireInitializeStatusEvent("连接行情服务器");
            DataCoreService.InitClient(hosts[0], port);
            DataCoreService.DataClient.Start();
        }

        public void Disconnect()
        {
 

        }

        public void Login(string user, string pass)
        {
            MDService.EventHub.FireInitializeStatusEvent("登入行情服务器");
            DataCoreService.DataClient.Login(user, pass);
        }
    }
}
