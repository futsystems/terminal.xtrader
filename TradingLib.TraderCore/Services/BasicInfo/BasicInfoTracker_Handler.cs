using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.TraderCore
{
    public partial class BasicInfoTracker
    {
        public void GotMarketTime(MarketTime mt, bool islast)
        {
            if (mt != null)
            {
                MarketTime target = null;
                MarketTime notify = null;
                if (markettimemap.TryGetValue(mt.ID, out target))
                {
                    //更新
                    target.Name = mt.Name;
                    target.Description = mt.Description;
                    notify = target;
                }
                else
                {
                    markettimemap.Add(mt.ID, mt);
                    notify = mt;
                }
            }
            if (islast)
            {
                Status("交易时间查询完毕,查询交易所信息");
                CoreService.TLClient.ReqXQryExchange();
            }

        }

        public void GotExchange(Exchange ex, bool islast)
        {
            if (ex != null)
            {
                Exchange target = null;
                Exchange notify = null;
                if (exchangemap.TryGetValue(ex.ID, out target))
                {
                    //更新
                    target.Name = ex.Name;
                    target.EXCode = ex.EXCode;
                    target.Country = ex.Country;
                    notify = target;
                }
                else
                {
                    exchangemap.Add(ex.ID, ex);
                    notify = ex;
                }
            }
            if (islast)
            {
                Status("交易所查询完毕,查询品种信息");
                CoreService.TLClient.ReqXQrySecurity();
            }
        }

        /// <summary>
        /// 获得品种信息
        /// </summary>
        /// <param name="sec"></param>
        public void GotSecurity(SecurityFamilyImpl sec, bool islast)
        {
            if (sec != null)
            {
                SecurityFamilyImpl target = null;
                SecurityFamilyImpl notify = null;
                if (securitymap.TryGetValue(sec.ID, out target))
                {
                    //更新
                    target.Code = sec.Code;
                    target.Name = sec.Name;
                    target.Currency = sec.Currency;
                    target.Type = sec.Type;

                    target.exchange_fk = sec.exchange_fk;
                    target.Exchange = this.GetExchange(target.exchange_fk);

                    target.mkttime_fk = sec.mkttime_fk;
                    target.MarketTime = this.GetMarketTime(target.mkttime_fk);

                    target.underlaying_fk = sec.underlaying_fk;
                    target.UnderLaying = this.GetSecurity(target.underlaying_fk);

                    target.Multiple = sec.Multiple;
                    target.PriceTick = sec.PriceTick;
                    target.EntryCommission = sec.EntryCommission;
                    target.ExitCommission = sec.ExitCommission;
                    target.Margin = sec.Margin;
                    target.ExtraMargin = sec.ExtraMargin;
                    target.MaintanceMargin = sec.MaintanceMargin;
                    target.Tradeable = sec.Tradeable;
                    notify = target;
                }
                else
                {
                    securitymap.Add(sec.ID, sec);
                    notify = sec;
                }
            }

            if (islast)
            {
                Status("品种查询完毕,查询合约信息");
                CoreService.TLClient.ReqXQrySymbol();
            }
        }

        /// <summary>
        /// 获得合约信息
        /// </summary>
        /// <param name="symbol"></param>
        public void GotSymbol(SymbolImpl symbol, bool islast)
        {
            if (symbol != null)
            {
                SymbolImpl target = null;
                SymbolImpl notify = null;
                if (symbolmap.TryGetValue(symbol.ID, out target))
                {
                    //更新
                    target.Symbol = symbol.Symbol;
                    target.EntryCommission = symbol._entrycommission;
                    target.ExitCommission = symbol._exitcommission;
                    target.Margin = symbol._margin;
                    target.ExtraMargin = symbol._extramargin;
                    target.MaintanceMargin = symbol._maintancemargin;
                    target.Strike = symbol.Strike;
                    target.OptionSide = symbol.OptionSide;
                    target.ExpireDate = symbol.ExpireDate;

                    target.security_fk = symbol.security_fk;
                    target.SecurityFamily = this.GetSecurity(target.security_fk);

                    target.underlaying_fk = symbol.underlaying_fk;
                    target.ULSymbol = this.GetSymbol(target.underlaying_fk);

                    target.underlayingsymbol_fk = symbol.underlayingsymbol_fk;
                    target.UnderlayingSymbol = this.GetSymbol(target.underlayingsymbol_fk);
                    target.Tradeable = symbol.Tradeable;

                    notify = target;
                }
                else //添加
                {
                    symbolmap.Add(symbol.ID, symbol);
                    symbol.SecurityFamily = this.GetSecurity(symbol.security_fk);
                    symbol.ULSymbol = this.GetSymbol(symbol.underlaying_fk);
                    symbol.UnderlayingSymbol = this.GetSymbol(symbol.underlayingsymbol_fk);
                    symbolnamemap[symbol.Symbol] = symbol;
                    notify = symbol;
                }
            }
            if (islast)
            {
                Status("合约查询完毕,查询隔夜持仓");
                BindData();
                CoreService.TLClient.ReqXQryYDPositon();
            }

        }

        /// <summary>
        /// 绑定对象数据
        /// </summary>
        void BindData()
        {
            foreach (SecurityFamilyImpl target in securitymap.Values)
            {
                target.Exchange = this.GetExchange(target.exchange_fk);
                target.MarketTime = this.GetMarketTime(target.mkttime_fk);
                target.UnderLaying = this.GetSecurity(target.underlaying_fk);
            }

            foreach (SymbolImpl target in symbolmap.Values)
            {
                target.SecurityFamily = this.GetSecurity(target.security_fk);
                target.ULSymbol = this.GetSymbol(target.underlaying_fk);
                target.UnderlayingSymbol = this.GetSymbol(target.underlayingsymbol_fk);
            }
        }

    }
}
