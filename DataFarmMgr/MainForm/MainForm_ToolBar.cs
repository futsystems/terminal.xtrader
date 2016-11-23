using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {

        void InitToolBar()
        {
            btnConnect.Click += new EventHandler(btnConnect_Click);

            btnDebug1.Click += new EventHandler(btnDebug1_Click);

            btnRegister.Click += new EventHandler(btnRegister_Click);
            btnUnregister.Click += new EventHandler(btnUnregister_Click);
            btnDebugForm.Click += new EventHandler(btnDebugForm_Click);

            btnStopFeedTick.Click += new EventHandler(btnStopFeedTick_Click);
            btnStartFeedTick.Click += new EventHandler(btnStartFeedTick_Click);
            btnFunctionForm.Click += new EventHandler(btnFunctionForm_Click);
        }

        void btnFunctionForm_Click(object sender, EventArgs e)
        {
            fmFunction fm = new fmFunction();
            fm.ShowDialog();
            fm.Close();
        }

        void btnStartFeedTick_Click(object sender, EventArgs e)
        {
            DataCoreService.DataClient.ReqContribRequest("DataFarm", "StartFeedTick", "");
        }

        void btnStopFeedTick_Click(object sender, EventArgs e)
        {
            DataCoreService.DataClient.ReqContribRequest("DataFarm", "StopFeedTick", "");
        }

        void btnUnregister_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnRegister_Click(object sender, EventArgs e)
        {

            foreach (var g in ctrlQuoteList.SymbolVisible.GroupBy(s => s.Exchange))
            {
                List<string> symlist = new List<string>();
                string exchange = g.Key;
                foreach (var val in g)
                {
                    symlist.Add(val.Symbol);
                }
                DataCoreService.DataClient.RegisterSymbol(exchange, symlist.ToArray());
            }
        }

        void btnDebug1_Click(object sender, EventArgs e)
        {
            //DataCoreService.DataClient.UnRegisterSymbol(new string[] { "*" });
        }

        void btnDebugForm_Click(object sender, EventArgs e)
        {
            debugForm.Show();
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            //DataCoreService.InitClient("127.0.0.1", 5060);
            DataCoreService.InitClient(new string[] { "121.41.76.214" }, 5060);
            //mdClient = new TradingLib.MDClient.MDClient("127.0.0.1", 5060, 5060);
            DataCoreService.EventHub.OnInitializedEvent += new Action(mdClient_OnInitializedEvent);
            DataCoreService.EventHub.OnRtnTickEvent += new Action<Tick>(mdClient_OnRtnTickEvent);
            DataCoreService.EventHub.OnConnectedEvent += new Action(EventHub_OnConnectedEvent);
            DataCoreService.EventHub.OnDisconnectedEvent += new Action(EventHub_OnDisconnectedEvent);
            DataCoreService.EventHub.OnLoginEvent += new Action<LoginResponse>(EventHub_OnLoginEvent);
            DataCoreService.DataClient.Start();
        }

        void mdClient_OnInitializedEvent()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(mdClient_OnInitializedEvent), new object[] { });
            }
            else
            {
                logger.Info("MDClient Inited");

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
                    symbol.Precision = Util.GetDecimalPlace(target.SecurityFamily.PriceTick);
                    mdsymbolmap.Add(symbol.UniqueKey, symbol);
                }

                ctrlQuoteList.SetSymbols(mdsymbolmap.Values);
                ctrlQuoteList.SelectTab(0);

                foreach (var exchange in DataCoreService.DataClient.Exchanges)
                {
                    string k = exchange.EXCode;

                    ctrlQuoteList.AddBlock(k, new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                        =>
                    {
                        if (symbol.Exchange == k)
                        {
                            return true;
                        }
                        return false;
                    }), TradingLib.XTrader.Control.EnumQuoteListType.FUTURE_IQFeed);
                }
            }
        }

        void EventHub_OnLoginEvent(LoginResponse obj)
        {
            if (obj.Authorized)
            {
                logger.Info("login success ~~~~~~~~~~ qry basic data");
                DataCoreService.DataClient.QryMarketTime();
            }
        }

        void EventHub_OnDisconnectedEvent()
        {
            
        }

        void EventHub_OnConnectedEvent()
        {
            logger.Info("Connected and login");
            DataCoreService.DataClient.Login("", "");
        }

        void mdClient_OnRtnTickEvent(Tick tick)
        {
            string key = tick.GetSymbolUniqueKey();
            MDSymbol symbol = GetMDSymbol(key);
            if (symbol == null) return;

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
            //symbol.TickSnapshot.settlement = tick.PreSettlement; ;


            if (InvokeRequired)
            {
                Invoke(new Action<Tick>(mdClient_OnRtnTickEvent), new object[] { tick });
            }
            else
            {
                ctrlQuoteList.Update(symbol);  
            }



            //保存LastTick
            if (symbol.TickSnapshot.Time != symbol.LastTickSnapshot.Time)
            {
                symbol.LastTickSnapshot = symbol.TickSnapshot;
            }
        }


    }
}
