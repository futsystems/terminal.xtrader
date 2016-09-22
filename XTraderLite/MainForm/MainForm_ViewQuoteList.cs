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
            ctrlQuoteList.AddBlock("所有A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" || symbol.BlockType == "5" || symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("中小版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "5")
                    {
                        return true;
                    }
                    return false;
                }),EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("创业版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("沪市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange==Exchange.EXCH_SSE)
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("深市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange == Exchange.EXCH_SZE)
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("基金", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "4")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("指数", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "7")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);

            ctrlQuoteList.AddBlock("债券", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "3")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("三板", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);
            ctrlQuoteList.AddBlock("自选", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }), EnumQuoteListType.STOCK_CN);



            //绑定对外事件
            ctrlQuoteList.MouseEvent += new Action<TradingLib.MarketData.MDSymbol, TradingLib.XTrader.Control.QuoteMouseEventType>(quoteView_MouseEvent);
            ctrlQuoteList.SymbolVisibleChanged += new EventHandler<TradingLib.XTrader.Control.SymbolVisibleChangeEventArgs>(ctrlQuoteList_SymbolVisibleChanged);
        }

        void ctrlQuoteList_SymbolVisibleChanged(object sender, TradingLib.XTrader.Control.SymbolVisibleChangeEventArgs e)
        {
            if (e.Symbols != null && e.Symbols.Length > 0)
            {
                MDService.DataAPI.QryTickSnapshot(e.Symbols);
            }
        }

        void quoteView_MouseEvent(TradingLib.MarketData.MDSymbol arg1, TradingLib.XTrader.Control.QuoteMouseEventType arg2)
        {
            switch (arg2)
            {
                case TradingLib.XTrader.Control.QuoteMouseEventType.SymbolDoubleClick:
                    {
                        logger.Info("QuoteView Select Symbol:" + arg1.Symbol);
                        ViewKChart();
                        break;
                    }
            }
        }

        
    }
}
