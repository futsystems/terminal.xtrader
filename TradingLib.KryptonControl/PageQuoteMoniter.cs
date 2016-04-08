using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


namespace TradingLib.KryptonControl
{
    public partial class PageQuoteMoniter : UserControl,IPage
    {
        string _pageName = "QUOTEMONITER";
        public string PageName { get { return _pageName; } }


        public event Action<IExchange> ExchangeChangedEvent;
        public event Action<Symbol> OpenChartEvent;

        ILog logger = LogManager.GetLogger("QuoteMoniter");

        public PageQuoteMoniter()
        {

            InitializeComponent();
            navigator.SelectedPageChanged += new EventHandler(navigator_SelectedPageChanged);
        }

        void navigator_SelectedPageChanged(object sender, EventArgs e)
        {
            logger.Info("Current Page:" + navigator.SelectedPage.Name);
            if (ExchangeChangedEvent != null)
            {
                IExchange exchange = navigator.SelectedPage.Tag as IExchange;
                if (exchange != null)
                {
                    ExchangeChangedEvent(exchange);
                }
            }
        }


        ConcurrentDictionary<string, ViewQuoteList> exchangeQuoteMap = new ConcurrentDictionary<string, ViewQuoteList>();
        ConcurrentDictionary<string, IExchange> exchangeMap = new ConcurrentDictionary<string, IExchange>();
        ConcurrentDictionary<string, ComponentFactory.Krypton.Navigator.KryptonPage> pageMap = new ConcurrentDictionary<string, ComponentFactory.Krypton.Navigator.KryptonPage>();

        ViewQuoteList GetQuoteList(string exchange)
        {
            ViewQuoteList target = null;
            if (exchangeQuoteMap.TryGetValue(exchange, out target))
            {
                return target;
            }
            return null;
        }


        /// <summary>
        /// 获得所有交易所
        /// </summary>
        public IEnumerable<IExchange> Exchanges { get { return exchangeMap.Values; } }

        EnumQuoteType GetQuoteType(IExchange ex)
        {
            switch (ex.EXCode)
            { 
                case "CFFEX":
                case "SHFE":
                case "CZCE":
                case "DCE":
                    return EnumQuoteType.CNQUOTE;
                default:
                    return EnumQuoteType.FOREIGNQUOTE;
            }
        }
        /// <summary>
        /// 添加一个交易所
        /// </summary>
        /// <param name="ex"></param>
        public void AddExchange(IExchange ex)
        {
            ViewQuoteList quote = new ViewQuoteList();
            quote.HeaderBackColor = Color.FromArgb(0, 0, 0);
            quote.BackColor = Color.FromArgb(0, 0, 0);
            quote.TableLineColor = Color.FromArgb(0, 0, 0);
            quote.HeaderFontColor = Color.FromArgb(0, 255, 255);
            quote.QuoteBackColor1 = Color.FromArgb(0, 0, 0);
            quote.QuoteBackColor2 = Color.FromArgb(0, 0, 0);
            quote.QuoteType = GetQuoteType(ex);
            quote.SelectedColor = Color.FromArgb(75, 75, 75);
            quote.MenuEnable = true;
            quote.OpenKChartEvent += new Action<Symbol>(OnOpenKChartEvent);

            ComponentFactory.Krypton.Navigator.KryptonPage page = new ComponentFactory.Krypton.Navigator.KryptonPage(ex.Title);
            page.Tag = ex;
            page.Controls.Add(quote);
            quote.Dock = DockStyle.Fill;
            navigator.Pages.Add(page);

            exchangeQuoteMap.TryAdd(ex.EXCode, quote);
            exchangeMap.TryAdd(ex.EXCode, ex);
            pageMap.TryAdd(ex.EXCode, page);

        }

        void OnOpenKChartEvent(Symbol symbol)
        {
            if (OpenChartEvent != null)
            {
                OpenChartEvent(symbol);
            }
        }

        /// <summary>
        /// 添加单个合约
        /// </summary>
        /// <param name="symbol"></param>
        public void AddSymbol(Symbol symbol)
        {
            ViewQuoteList target = GetQuoteList(symbol.Exchange);
            if (target != null)
                target.AddSymbol(symbol);
        }

        /// <summary>
        /// 添加一组合约
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbols"></param>
        public void AddSymbols(IExchange exchange, IEnumerable<Symbol> symbols)
        {
            ViewQuoteList target = GetQuoteList(exchange.EXCode);
            if (target != null)
                target.AddSymbols(symbols);
        }


        public void ShowQuote(Symbol symbol)
        {
            ComponentFactory.Krypton.Navigator.KryptonPage target = null;
            string exchange = symbol.Exchange;
            if (pageMap.TryGetValue(exchange, out target))
            {
                navigator.SelectedPage = target;
                ViewQuoteList quote = null;
                if (exchangeQuoteMap.TryGetValue(exchange, out quote))
                {
                    quote.Focus();
                }
            }



        }
        public void GotTick(Symbol symbol, Tick k)
        {
            ViewQuoteList target = GetQuoteList(symbol.Exchange);
            if (target != null)
                target.GotTick(k);
        }
    }
}
