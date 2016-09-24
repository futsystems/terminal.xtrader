using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Stock
{
    public partial class PageSTKOrderEntry : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_ORDER_ENTRY;
        public string PageName { get { return _pageName; } }

       

        public PageSTKOrderEntry()
        {
            InitializeComponent();
            this.Mode = 0;
        }

        /// <summary>
        /// 设置某个合约
        /// 下单面板会根据交易所及合约 进行内部处理
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        public void SetSymbol(string exchange, string symbol)
        {
            ctOrderSenderSTK1.SetSymbol(exchange, symbol);
        }

        [DefaultValue(0)]
        int _mode = 0;
        public int Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode < 0 || _mode > 2) return;
                _mode = value;
                //买入
                if (_mode == 0)
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = true;
                    //ctOrderSenderSTK2.Visible = false;
                }
                //卖出
                else if (_mode == 1)
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = false;
                    //ctOrderSenderSTK2.Visible = false;
                }
                //双向
                else
                {
                    ctOrderSenderSTK1.Visible = true;
                    ctOrderSenderSTK1.Side = true;
                   //ctOrderSenderSTK2.Visible = true;
                    //ctOrderSenderSTK2.Side = false;
                    
                }

                Invalidate();
            }
        }
    }
}
