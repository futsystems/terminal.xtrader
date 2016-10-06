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
            DataCoreService.DataClient.UnRegisterSymbol(new string[] { "*" });
        }

        void btnDebugForm_Click(object sender, EventArgs e)
        {
            debugForm.Show();
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            DataCoreService.InitClient("127.0.0.1", 5060);
            //mdClient = new TradingLib.MDClient.MDClient("127.0.0.1", 5060, 5060);
            DataCoreService.OnInitializedEvent += new Action(mdClient_OnInitializedEvent);
            DataCoreService.EventData.OnRtnTickEvent += new Action<Tick>(mdClient_OnRtnTickEvent);


            DataCoreService.DataClient.Start();
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
