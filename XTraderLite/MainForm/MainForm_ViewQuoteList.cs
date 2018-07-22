using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;
using TradingLib.XTrader.Control;


namespace XTraderLite
{
    public partial class MainForm
    {

        
        void InitQuoteList()
        {
            //绑定对外事件
            ctrlQuoteList.MouseEvent += new Action<TradingLib.MarketData.MDSymbol, TradingLib.XTrader.Control.QuoteMouseEventType>(quoteView_MouseEvent);
            ctrlQuoteList.SymbolVisibleChanged += new EventHandler<TradingLib.XTrader.Control.SymbolVisibleChangeEventArgs>(ctrlQuoteList_SymbolVisibleChanged);
            ctrlQuoteList.WatchSymbolEvent += new Action<MDSymbol>(ctrlQuoteList_WatchSymbolEvent);
            ctrlQuoteList.UnWatchSymbolEvent += new Action<MDSymbol>(ctrlQuoteList_UnWatchSymbolEvent);
            ctrlQuoteList.WatchManagerEvent += new Action(ctrlQuoteList_WatchManagerEvent);
        }

        void ctrlQuoteList_WatchManagerEvent()
        {
            var fm = new fmWatchMgr(watchList);
            fm.ShowDialog();
        }

        void ctrlQuoteList_UnWatchSymbolEvent(MDSymbol obj)
        {
            watchList.UnWatchSymbol(obj.Symbol);
        }

        void ctrlQuoteList_WatchSymbolEvent(MDSymbol obj)
        {
            //throw new NotImplementedException();
            watchList.WatchSymbol(obj.Symbol);
        }

        void ctrlQuoteList_SymbolVisibleChanged(object sender, TradingLib.XTrader.Control.SymbolVisibleChangeEventArgs e)
        {
            logger.Info("QuoteList Symbol visible Changed");
            if (e.Symbols != null && e.Symbols.Length > 0)
            {
                if (MDService.DataAPI.APISetting.TickMode == EnumMDTickMode.FreqQry)
                {
                    MDService.DataAPI.QryTickSnapshot(e.Symbols);
                }
                else
                {
                    if (symbolRegister.Count > 0)
                    {
                        MDService.DataAPI.UnregisterSymbol(symbolRegister.ToArray());
                        symbolRegister.Clear();
                    }
                    IEnumerable<MDSymbol> symlist = GetSymbolsNeeded();
                    MDService.DataAPI.RegisterSymbol(symlist.ToArray());
                    symbolRegister.AddRange(symlist);//记录当前QuoteList所注册合约 用于视图变化时注销合约行情
                }
            }
        }

        void quoteView_MouseEvent(MDSymbol arg1,QuoteMouseEventType arg2)
        {
            switch (arg2)
            {
                case QuoteMouseEventType.SymbolDoubleClick:
                    {
                        if (!string.IsNullOrEmpty(arg1.Symbol))
                        {
                            logger.Info("QuoteView Select Symbol:" + arg1.Symbol);
                            ViewKChart(arg1);
                            TraderAPI_SelectSymbol(arg1);
                        }
                        break;
                    }
                case QuoteMouseEventType.SymbolBuyClick:
                    {
                        ViewKChart(arg1);
                        EntryOrderPanel(true, arg1);
                        break;
                    }
                case QuoteMouseEventType.SymbolSellClick:
                    {
                        ViewKChart(arg1);
                        EntryOrderPanel(false, arg1);
                        break;
                    }
            }
        }

        
    }
}
