using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.MarketData;

namespace TradingLib.KryptonControl
{
    public partial class ctrlQuoteList : UserControl
    {
        string[] cmd = { "中金所", "大商所" ,"上海证券交易所"};
        IEnumerable<MDSymbol> symbolMap = new List<MDSymbol>();
        ILog logger = LogManager.GetLogger("Quote");
        public ctrlQuoteList()
        {
            InitializeComponent();
            blockTab.BlockTabClick += new EventHandler<BlockTabClickEvent>(blockTab_BlockTabClick);
        }

        void blockTab_BlockTabClick(object sender, BlockTabClickEvent e)
        {
            if (e.TargtButton != null)
            {
                if (e.TargtButton.SymbolFilter != null && symbolMap.Count()>0)
                {
                    quotelist.Clear();
                    quotelist.BeginUpdate();
                    quotelist.AddSymbols(symbolMap.Where(sym=>e.TargtButton.SymbolFilter(sym)));
                    quotelist.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 设置合约数据集
        /// 通过板块按钮进行合约数据过滤
        /// </summary>
        public IEnumerable<MDSymbol> Symbols
        {
            get { return symbolMap; }
            set { symbolMap = value; }
        }

        /// <summary>
        /// 添加板块按钮
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        public void AddBlock(string title,Predicate<MDSymbol> filter)
        {
            blockTab.AddBlock(title, filter);
        }
    }
}
