using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;
using TradingLib.MarketData;
using Common.Logging;
using NodaTime;
using System.Net;

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
                symbol.TickSnapshot.Date = tick.Date;
                symbol.TickSnapshot.Time = tick.Time;
                symbol.TickSnapshot.Volume = tick.Vol;
                symbol.TickSnapshot.Open = (double)tick.Open;
                symbol.TickSnapshot.High = (double)tick.High;
                symbol.TickSnapshot.Low = (double)tick.Low;
                symbol.TickSnapshot.PreClose = (double)tick.PreClose;
                symbol.TickSnapshot.PreOI = tick.PreOpenInterest;
                symbol.TickSnapshot.PreSettlement = (double)tick.PreSettlement;
                //symbol.PreClose = (double)tick.PreClose;
                if (symbol.LongPosition.Size != 0)
                {
                    symbol.LongPosition.UnRealizedPL = (symbol.LastTickSnapshot.Price - symbol.LongPosition.PositionCost) * symbol.Multiple * symbol.LongPosition.Size;
                }
                if (symbol.ShortPosition.Size != 0)
                {
                    symbol.ShortPosition.UnRealizedPL = -1 * (symbol.LastTickSnapshot.Price - symbol.ShortPosition.PositionCost) * symbol.Multiple * symbol.ShortPosition.Size;
                }
                if (OnRtnTick != null)
                    OnRtnTick(symbol);
            }
        }

        void EventHub_OnLoginEvent(LoginResponse obj)
        {
            MDLoginResponse response = new MDLoginResponse();
            response.LoginSuccess = obj.Authorized;
            response.TradingDay = obj.TradingDay;
            response.ErrorCode = obj.RspInfo.ErrorID.ToString();
            response.ErrorMessage = obj.RspInfo.ErrorMessage;
            MDService.EventHub.FireLoginEvent(response);

            ////登入成功 且未初始化 则查询基础数据
            //if (!DataCoreService.Initialized && obj.Authorized)
            //{
            //    DataCoreService.DataClient.QryMarketTime();
            //}
        }

        void EventHub_OnDisconnectedEvent()
        {
            MDService.EventHub.FireDisconnectedEvent();
        }

        void EventHub_OnConnectedEvent()
        {
            MDService.EventHub.FireConnectedEvent();
        }

        /// <summary>
        /// 从交易小节设置信息 解析出Session对象
        /// 93000-113000,130000-150000
        /// </summary>
        /// <param name="sessionStr"></param>
        /// <returns></returns>
        List<MDSession> ParseSession(string sessionStr)
        {
            List<MDSession> list = new List<MDSession>();
            string[] rec = sessionStr.Split(',');
            foreach (var str in rec)
            {
                list.Add(MDSession.Deserialize(str));
            }
            return list;
        }

        void EventHub_OnInitializedEvent()
        {
            Instant now = SystemClock.Instance.Now;//标准UTC时间
            //DateTimeZone localZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();//本地市区
            DateTimeZone localZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Asia/Shanghai");
            Offset localOffset = localZone.GetUtcOffset(now);

            DateTimeZone exZone = null;
            Offset zoneOffset;
            foreach (var target in DataCoreService.DataClient.Symbols)
            {
                if (!target.Tradeable) continue;
                MDSymbol symbol = new MDSymbol();
                symbol.Symbol = target.Symbol;
                symbol.SecCode = target.SecurityFamily.Code;
                symbol.Name = target.GetTitleName(true);

                symbol.NameLong = target.GetTitleName(true);
                symbol.NameShort = target.GetTitleName(false);
                symbol.TitleLong = target.GetAlphabetName(true);
                symbol.TitleShort = target.GetAlphabetName(false);

                symbol.Currency = MDCurrency.RMB;
                symbol.Exchange = target.Exchange;
                symbol.Multiple = target.Multiple;
                symbol.SecurityType = MDSecurityType.FUT;
                symbol.SizeRate = 1;
                symbol.Key = symbol.Symbol;
                symbol.NCode = 0;
                symbol.SortKey = target.Month;
                symbol.Precision = target.SecurityFamily.GetDecimalPlaces();
                symbol.Session = TradingSessionToMDSession(target.TradingSession);

                
                exZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(target.SecurityFamily.Exchange.TimeZoneID);
                if (exZone != null)
                {
                    zoneOffset = exZone.GetUtcOffset(now);
                    symbol.TimeZoneOffset = (localOffset.Milliseconds - zoneOffset.Milliseconds) / 1000;
                    List<MDSession> list = ParseSession(symbol.Session);
                    if (symbol.TimeZoneOffset != 0)
                    {
                        int today = DateTime.Now.ToTLDate();
                        int tomorrow = DateTime.Now.AddDays(1).ToTLDate();
                        DateTime start;
                        DateTime end;

                        foreach (var session in list)
                        {
                            start = symbol.GetLocalDateTime(today, session.Start);
                            end = symbol.GetLocalDateTime(session.EndInNextDay ? tomorrow : today, session.End);

                            session.Start = start.ToTLTime();
                            session.End = end.ToTLTime();
                            session.EndInNextDay = start.ToTLDate() != end.ToTLDate();
                        }

                        symbol.Session = string.Join(",", list.Select(s => MDSession.Serialize(s)).ToArray());
                    }

                    //获得开盘事件
                    if (!string.IsNullOrEmpty(symbol.Session))
                    {
                        symbol.OpenTime = list.FirstOrDefault().Start;
                    }
                }

                symbolMap.Add(symbol.UniqueKey, symbol);
            }

            foreach (var exchange in DataCoreService.DataClient.Exchanges)
            {
                string k = exchange.EXCode;
                blockInfoList.Add(new BlockInfo(exchange.Title,exchange.EXCode, new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                    =>
                {
                    if (symbol.Exchange == k)
                    {
                        return true;
                    }
                    return false;
                }), 2));
            }

            //查询市场快照
            DataCoreService.DataClient.QryTickSnapshot(string.Empty, string.Empty);

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
            get 
            {
                return DataCoreService.DataClient.Connected;
            }
        }

        public void Connect(string[] hosts, int port)
        {
            MDService.EventHub.FireInitializeStatusEvent("连接行情服务器");
            DataCoreService.InitClient(hosts, port);
            DataCoreService.DataClient.Start();
        }

        public void Disconnect()
        {
            if (DataCoreService.DataClient != null)
            {
                DataCoreService.DataClient.Stop();
            }

        }

        public void Login(string user, string pass)
        {
            MDService.EventHub.FireInitializeStatusEvent("登入行情服务器");
            DataCoreService.DataClient.Login(user, pass);
        }

        /// <summary>
        /// 返回当前连接服务器地址
        /// </summary>
        public IPEndPoint CurrentServer 
        {
            get{
                if (DataCoreService.DataClient != null)
                {
                    return DataCoreService.DataClient.CurrentServer;
                }
                return null;
            }
        }
    }
}
