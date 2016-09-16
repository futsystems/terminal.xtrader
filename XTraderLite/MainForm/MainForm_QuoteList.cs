using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    public partial class MainForm
    {

        void InitQuoteList()
        {
            quoteList.AddBlock("所有A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" || symbol.BlockType == "5" || symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("中小版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "5")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("创业版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("沪市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange=="SH")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("深市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange == "SZ")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("基金", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "4")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("指数", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "7")
                    {
                        return true;
                    }
                    return false;
                }));

            quoteList.AddBlock("债券", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "3")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("三板", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteList.AddBlock("自选", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }));


        }
    }
}
