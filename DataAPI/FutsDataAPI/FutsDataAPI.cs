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

        MarketDataAPISetting setting = new MarketDataAPISetting();
        Dictionary<string, MDSymbol> symbolMap = new Dictionary<string, MDSymbol>();
        List<BlockInfo> blockInfoList = new List<BlockInfo>();
        public FutsDataAPI()
        {
            APISetting.TickMode = EnumMDTickMode.Register;
            APISetting.QryBarTimeSupport = true;
            APISetting.QryMinuteDataTimeSupport = true;

            DataCoreService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            DataCoreService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);
            DataCoreService.EventHub.OnLoginEvent += new Action<LoginResponse>(EventHub_OnLoginEvent);
            DataCoreService.EventHub.OnInitializedEvent += new Action(EventHub_OnInitializedEvent);

            DataCoreService.EventHub.OnRtnTickEvent += new Action<Tick>(EventHub_OnRtnTickEvent);

            //Bar数据查询
            DataCoreService.EventHub.OnRspBarEvent += new Action<RspQryBarResponseBin>(EventHub_OnRspBarEvent);
            //分笔成交数据查询
            DataCoreService.EventHub.OnRspTradeSplitEvent += new Action<RspXQryTradeSplitResponse>(EventHub_OnRspTradeSplitEvent);
            //价格成交量查询
            DataCoreService.EventHub.OnRspPriceVolEvent += new Action<RspXQryPriceVolResponse>(EventHub_OnRspPriceVolEvent);
            //分时数据查询
            DataCoreService.EventHub.OnRspMinuteDataEvent += new Action<RspXQryMinuteDataResponse>(EventHub_OnRspMinuteDataEvent);
        }



        

        

        

        void EventHub_OnRtnTickEvent(Tick tick)
        {
            string key = tick.GetSymbolUniqueKey();
            MDSymbol symbol = null;
            if (symbolMap.TryGetValue(key, out symbol))
            {
                symbol.TickSnapshot.Price = (double)tick.Trade;
                symbol.TickSnapshot.Size = tick.Size;
                symbol.TickSnapshot.Buy1 = (double)tick.BidPrice;
                symbol.TickSnapshot.BuyQTY1 = tick.BidSize;
                symbol.TickSnapshot.Sell1 = (double)tick.AskPrice;
                symbol.TickSnapshot.SellQTY1 = tick.AskSize;
                symbol.TickSnapshot.Time = tick.Time;
                symbol.TickSnapshot.Volume = tick.Vol;
                symbol.TickSnapshot.Open = (double)tick.Open;
                symbol.TickSnapshot.High = (double)tick.High;
                symbol.TickSnapshot.Low = (double)tick.Low;
                symbol.TickSnapshot.PreClose = (double)tick.PreClose;
                symbol.TickSnapshot.PreOI = tick.PreOpenInterest;
                symbol.TickSnapshot.PreSettlement = (double)tick.PreSettlement;
                //symbol.PreClose = (double)tick.PreClose;
                if (OnRtnTick != null)
                    OnRtnTick(symbol);
            }
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
                if (!target.Tradeable) continue;
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
                symbol.Precision = target.SecurityFamily.GetDecimalPlaces();
                symbol.Session = TradingSessionToMDSession(target.TradingSession);
                symbolMap.Add(symbol.UniqueKey, symbol);
            }

            foreach (var exchange in DataCoreService.DataClient.Exchanges)
            {
                string k = exchange.EXCode;
                blockInfoList.Add(new BlockInfo(exchange.Title, new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
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
        /// API工作模式参数
        /// </summary>
        public MarketDataAPISetting APISetting { get { return setting; } }

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

        /// <summary>
        /// 底部亮显合约
        /// </summary>
        public IEnumerable<SymbolHighLight> HightLightSymbols { get { return new List<SymbolHighLight>(); } }


        /// <summary>
        /// 所有板块列表
        /// </summary>
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
