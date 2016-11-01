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

        List<MDSymbol> quoteListRegister = new List<MDSymbol>();
        void InitQuoteList()
        {
            //绑定对外事件
            ctrlQuoteList.MouseEvent += new Action<TradingLib.MarketData.MDSymbol, TradingLib.XTrader.Control.QuoteMouseEventType>(quoteView_MouseEvent);
            ctrlQuoteList.SymbolVisibleChanged += new EventHandler<TradingLib.XTrader.Control.SymbolVisibleChangeEventArgs>(ctrlQuoteList_SymbolVisibleChanged);
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
                    if (quoteListRegister.Count > 0)
                    {
                        MDService.DataAPI.UnregisterSymbol(quoteListRegister.ToArray());
                        quoteListRegister.Clear();
                    }
                    MDService.DataAPI.RegisterSymbol(e.Symbols);
                    quoteListRegister.AddRange(e.Symbols);//记录当前QuoteList所注册合约 用于视图变化时注销合约行情
                }
            }
        }

        void quoteView_MouseEvent(MDSymbol arg1,QuoteMouseEventType arg2)
        {
            switch (arg2)
            {
                case QuoteMouseEventType.SymbolDoubleClick:
                    {
                        logger.Info("QuoteView Select Symbol:" + arg1.Symbol);
                        ViewKChart();
                        TraderAPI_SelectSymbol(arg1);
                        break;
                    }
                case QuoteMouseEventType.SymbolBuyClick:
                    {
                        ViewKChart();
                        EntryOrderPanel(true, arg1);
                        break;
                    }
                case QuoteMouseEventType.SymbolSellClick:
                    {
                        ViewKChart();
                        EntryOrderPanel(false, arg1);
                        break;
                    }
            }
        }

        
    }
}
