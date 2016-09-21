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

        void InitOtherView()
        {
            ctrlTickList.ExitView += new EventHandler(ctrlTickList_ExitView);
            ctrlPriceVolList.ExitView += new EventHandler(ctrlPriceVolList_ExitView);
            ctrlSymbolInfo.ExitView += new EventHandler(ctrlSymbolInfo_ExitView);

            ctrlTickList.DoubleClick += new EventHandler(ctrlTickList_ExitView);
            ctrlPriceVolList.DoubleClick += new EventHandler(ctrlPriceVolList_ExitView);
            ctrlSymbolInfo.DoubleClick += new EventHandler(ctrlSymbolInfo_DoubleClick);


            ctrlSymbolInfo.QrySymbolInfo += new EventHandler<TradingLib.XTrader.Control.QrySymbolInfoArgs>(ctrlSymbolInfo_QrySymbolInfo);
        }

        //查询合约信息
        void ctrlSymbolInfo_QrySymbolInfo(object sender, TradingLib.XTrader.Control.QrySymbolInfoArgs e)
        {
            MDService.DataAPI.QrySymbolInfo(e.Symbol.Exchange, e.Symbol.Symbol, e.Type);
        }

        void ctrlSymbolInfo_DoubleClick(object sender, EventArgs e)
        {
            RollBackView();
        }

        void ctrlSymbolInfo_ExitView(object sender, EventArgs e)
        {
            RollBackView();
        }

        void ctrlPriceVolList_ExitView(object sender, EventArgs e)
        {
            RollBackView();
        }

        void ctrlTickList_ExitView(object sender, EventArgs e)
        {
            RollBackView();
            
        }
    }
}
