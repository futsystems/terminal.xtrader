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
            quotelist.QuoteViewChanged += new EventHandler<QuoteViewChangedArgs>(quotelist_QuoteViewChanged);
            blockTab.BlockTabClick += new EventHandler<BlockTabClickEvent>(blockTab_BlockTabClick);
            scrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
            scrollBar.ValueChanged += new EventHandler(scrollBar_ValueChanged);

        }

        void quotelist_QuoteViewChanged(object sender, QuoteViewChangedArgs e)
        {
            if (e.Count <= e.MaxShowCount)
            {
                scrollBar.Visible = false;
            }
            else
            {
                scrollBar.Visible = true;
                scrollBar.Value = 0;
                scrollBar.Maximum = e.Count - 1;
            }
        }

        void scrollBar_ValueChanged(object sender, EventArgs e)
        {
            logger.Info("value changed:" + scrollBar.Value.ToString());
            //quotelist.SelectRow(scrollBar.Value);
            //quotelist.SelectedQuoteRow = scrollBar.Value;
            quotelist.StartIndex = scrollBar.Value;
        }

        void scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            logger.Info("scroll:" + e.ScrollOrientation + " v:" + e.NewValue.ToString());
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
