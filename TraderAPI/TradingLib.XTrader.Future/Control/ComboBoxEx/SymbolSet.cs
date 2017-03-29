using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader.Future
{
    /// <summary>
    /// 合约集
    /// 合约集按品种或某个分类方式进行分类
    /// </summary>
    public class SymbolSet
    {
        public SymbolSet(string setTitle)
        {
            this.SetTitle = setTitle;
            this.Symbols = new Dictionary<string, Symbol>();
        }

        /// <summary>
        /// 向合约集中添加合约
        /// </summary>
        /// <param name="symbol"></param>
        public void AddSymbol(Symbol symbol)
        {
            this.Symbols.Add(symbol.GetSymbolTitle(), symbol);
        }
        /// <summary>
        /// 合约集名称
        /// </summary>
        public string SetTitle { get; set; }

        /// <summary>
        /// 合约列表
        /// </summary>
        public Dictionary<string,Symbol> Symbols { get; set; }


        public Symbol GetSymbol(string symbol)
        {
            Symbol target = null;

            if (this.Symbols.TryGetValue(symbol, out target))
            {
                return target;
            }
            return null;
           
            
        }
    }
}
