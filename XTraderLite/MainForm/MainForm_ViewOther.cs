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
            ctrlTickList.ExitView += new EventHandler(Exit_Handler);
            ctrlPriceVolList.ExitView += new EventHandler(Exit_Handler);
            ctrlSymbolInfo.ExitView += new EventHandler(Exit_Handler);

            ctrlTickList.DoubleClick += new EventHandler(Exit_Handler);
            ctrlPriceVolList.DoubleClick += new EventHandler(Exit_Handler);
            ctrlSymbolInfo.DoubleClick += new EventHandler(Exit_Handler);


            ctrlSymbolInfo.QrySymbolInfo += new EventHandler<TradingLib.XTrader.Control.QrySymbolInfoArgs>(ctrlSymbolInfo_QrySymbolInfo);
        }

        //查询合约信息
        void ctrlSymbolInfo_QrySymbolInfo(object sender, TradingLib.XTrader.Control.QrySymbolInfoArgs e)
        {
            MDService.DataAPI.QrySymbolInfo(e.Symbol.Exchange, e.Symbol.Symbol, e.Type);
        }

        void Exit_Handler(object sender, EventArgs e)
        {
            RollBackView();
        }

        
    }
}
