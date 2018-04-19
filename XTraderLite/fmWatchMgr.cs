using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class fmWatchMgr : Form
    {
        WatchList watchlist = null;
        public fmWatchMgr(WatchList val)
        {
            InitializeComponent();

            watchlist = val;

            foreach (var sym in watchlist.GetWatchSymbolList())
            {
                symbolList.Items.Add(sym);
            }
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUp.Click += new EventHandler(btnUp_Click);
            btnDown.Click += new EventHandler(btnDown_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            if (symbolList.SelectedItem != null)
            {
                string val = symbolList.SelectedItem.ToString();
                watchlist.UnWatchSymbol(val);
                symbolList.Items.Remove(val);
            }
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            if (symbolList.SelectedItem != null)
            {
                string val = symbolList.SelectedItem.ToString();
                var idx = watchlist.DownSymbol(val);

                symbolList.Items.Clear();
                foreach (var sym in watchlist.GetWatchSymbolList())
                {
                    symbolList.Items.Add(sym);
                }

                symbolList.SelectedIndex = idx;
            }


        }

        void btnUp_Click(object sender, EventArgs e)
        {
            if (symbolList.SelectedItem != null)
            {
                string val = symbolList.SelectedItem.ToString();
                var idx = watchlist.UpSymbol(val);

                symbolList.Items.Clear();
                foreach (var sym in watchlist.GetWatchSymbolList())
                {
                    symbolList.Items.Add(sym);
                }

                symbolList.SelectedIndex = idx;

            }
            
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(symbol.Text))
            {
                MessageBox.Show("请输入合约");
                return;
            }

            if (watchlist.IsWatched(symbol.Text))
            {
                MessageBox.Show(string.Format("合约:{0}已在自选列表", symbol.Text));
                return;
            }

            MDSymbol tmp = MDService.DataAPI.Symbols.Where(s => s.Symbol == symbol.Text).FirstOrDefault();
            if (tmp == null)
            {
                MessageBox.Show(string.Format("合约:{0}已过期或不存在", symbol.Text));
                return;
            }

            watchlist.WatchSymbol(symbol.Text);

            this.symbolList.Items.Add(symbol.Text);
         

        }
    }
}
