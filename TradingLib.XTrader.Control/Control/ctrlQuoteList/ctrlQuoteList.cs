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

namespace TradingLib.XTrader.Control
{
    public partial class ctrlQuoteList : UserControl,IView
    {

        EnumViewType vietype = EnumViewType.QuoteList;

        public EnumViewType ViewType { get { return vietype; } }


        string[] cmd = { "中金所", "大商所" ,"上海证券交易所"};
        IEnumerable<MDSymbol> symbolMap = new List<MDSymbol>();
        ILog logger = LogManager.GetLogger("Quote");

        public override bool Focused
        {
            get
            {
                return quotelist.Focused;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            quotelist.Focus();
        }

        /// <summary>
        /// 聚合鼠标事件
        /// </summary>
        public event Action<MDSymbol, QuoteMouseEventType> MouseEvent;

        /// <summary>
        /// 可视合约发生变化
        /// </summary>
        public event EventHandler<SymbolVisibleChangeEventArgs> SymbolVisibleChanged;

        public ctrlQuoteList()
        {
            InitializeComponent();
            quotelist.QuoteViewChanged += new EventHandler<QuoteViewChangedArgs>(quotelist_QuoteViewChanged);
            quotelist.MouseEvent += new Action<MDSymbol, QuoteMouseEventType>(quotelist_MouseEvent);
            quotelist.SymbolVisibleChanged += new EventHandler<SymbolVisibleChangeEventArgs>(quotelist_SymbolVisibleChanged);
            blockTab.BlockTabClick += new EventHandler<BlockTabClickEvent>(blockTab_BlockTabClick);
            scrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
            scrollBar.ValueChanged += new EventHandler(scrollBar_ValueChanged);
        }


        void quotelist_SymbolVisibleChanged(object sender, SymbolVisibleChangeEventArgs e)
        {
            if (SymbolVisibleChanged != null)
            {
                SymbolVisibleChanged(this, e);
            }
        }

        void quotelist_MouseEvent(MDSymbol arg1, QuoteMouseEventType arg2)
        {
            if (MouseEvent != null)
            {
                MouseEvent(arg1, arg2);
            }
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
                    quotelist.ApplyConfig(e.TargtButton.QuoteType);
                    quotelist.EndUpdate();
                }
            }
        }


        /// <summary>
        /// 设置合约数据集
        /// 通过板块按钮进行合约数据过滤
        /// </summary>
        public void SetSymbols(IEnumerable<MDSymbol> symbols)
        {
            symbolMap = symbols;
        }

        /// <summary>
        /// 当前选中合约
        /// </summary>
        public MDSymbol SymbolSelected
        {
            get
            {
                return quotelist.SymbolSelected;
            }
        }


        /// <summary>
        /// 所有可见合约
        /// </summary>
        public List<MDSymbol> SymbolVisible
        {
            get
            {
                return quotelist.SymbolVisible;
            }
        }


        /// <summary>
        /// 添加板块按钮
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        public void AddBlock(string title,Predicate<MDSymbol> filter,EnumQuoteListType type)
        {
            blockTab.AddBlock(title, filter,type);
        }
        /// <summary>
        /// 选中某个Tab
        /// </summary>
        /// <param name="index"></param>
        public void SelectTab(int index)
        {
            blockTab.SelectTab(index);
        }

        public void Update(MDSymbol symbol)
        {
            quotelist.Update(symbol);
        }
    }
}
