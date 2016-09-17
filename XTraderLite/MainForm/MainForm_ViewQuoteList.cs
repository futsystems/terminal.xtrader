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


namespace XTraderLite
{
    public partial class MainForm
    {
        /// <summary>
        /// 当前合约
        /// </summary>
        MDSymbol _currentSymbol = null;

        void InitQuoteList()
        {
            quoteView.AddBlock("所有A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" || symbol.BlockType == "5" || symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("中小版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "5")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("创业版", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "6")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("沪市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange==Exchange.EXCH_SSE)
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("深市A股", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "1" && symbol.Exchange == Exchange.EXCH_SZE)
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("基金", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "4")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("指数", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "7")
                    {
                        return true;
                    }
                    return false;
                }));

            quoteView.AddBlock("债券", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "3")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("三板", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }));
            quoteView.AddBlock("自选", new Predicate<TradingLib.MarketData.MDSymbol>((symbol)
                =>
                {
                    if (symbol.BlockType == "8")
                    {
                        return true;
                    }
                    return false;
                }));



            //绑定对外事件
            quoteView.MouseEvent += new Action<TradingLib.MarketData.MDSymbol, TradingLib.KryptonControl.QuoteMouseEventType>(quoteView_MouseEvent);
        }

        void quoteView_MouseEvent(TradingLib.MarketData.MDSymbol arg1, TradingLib.KryptonControl.QuoteMouseEventType arg2)
        {
            switch (arg2)
            {
                case TradingLib.KryptonControl.QuoteMouseEventType.SymbolDoubleClick:
                    {
                        
                        logger.Info("QuoteView Select Symbol:" + arg1.Symbol);
                        SelectCurrentSymbol(arg1);
                        break;
                    }
            }
        }

        /// <summary>
        /// 选中当前合约
        /// </summary>
        /// <param name="symbol"></param>
        void SelectCurrentSymbol(MDSymbol symbol)
        {
            bool changeSymbol = false;
            if (_currentSymbol != symbol)
            {
                changeSymbol = true;
            }
            _currentSymbol = symbol;

            //设定当前视图类型
            SetViewType(EnumTraderViewType.KChart);

            
            kChartView.Focus();
            kChartView.ClearData();
           
            //GP.SetQuan(sk.qu);
            //GP.PreClose = sk.GP.YClose;
            kChartView.StkCode = symbol.Symbol;
            kChartView.StkName = symbol.Name;
            kChartView.SetStock(symbol);

            //如果是分时模式 则请求分时数据
            if (kChartView.IsIntraView)
            {
                //多日分时
                if ((kChartView.DaysForIntradayView > 1) && changeSymbol)
                {
                    minuteData.Clear();
                    //for (int i = 0; i < 10; i++)
                    //    dateList[i] = -1;

                    //多日分时 由于不知道交易日信息 因此先查询日线 获得有效日期，然后再按此日期进行历史分时查询
                    //int reqid = dataApi.QrySeurityBars(FCurStock.mark, FCurStock.codes, 4, 0, 10);
                    //reqSender_TimeView.TryAdd(reqid, GP);
                }
                else //MDService.DataAPI
                {
                    MDService.DataAPI.QryMinuteDate(symbol.Exchange,symbol.Symbol,0);
                }
            }

            //如果是K线模式则请求K线数据
            if (kChartView.IsBarView)
            {

                //if (zq == 12)
                //{
                //    //Ticks.Clear();
                //    //GetFenBiLine(sk, 0, 2000);
                //}
                //else
                {
                    MDService.DataAPI.QrySeurityBars(symbol.Exchange, symbol.Symbol, ConstFreq.Freq_Day, 0, 800);
                }
            }

        }
    }
}
