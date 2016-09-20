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
        void SwitchTradingBox()
        {
            panelBroker.Visible ^= true;
            //隐藏交易面板时 将当前行情视图获取焦点
            if (!panelBroker.Visible)
            {
                if (_curView != null) _curView.Focus();
            }
        }

        /// <summary>
        /// 回退
        /// </summary>
        void RollBackView()
        {

            //报价表 回退不执行任何操作
            if (ctrlQuoteList.Visible)
            {
                return;
            }
            //K线图 回退到报价表
            if (ctrlKChart.Visible)
            {
                ViewQuoteList();
                return;
            }
            else
            {
                SetViewType(EnumTraderViewType.KChart);
                UpdateToolBarStatus();
                SetKChartSymbol(CurrentKChartSymbol);
            }

            
        }
        
        void  SwitchMainView()
        {
            //当前为报价表状态 则进入分时
            if (ctrlQuoteList.Visible)
            {
                MDSymbol tmp = null;
                if (ctrlQuoteList.Visible)
                {
                    tmp = ctrlQuoteList.SymbolSelected;
                    if (tmp == null) return;
                }

                if (tmp == null)
                {
                    tmp = this.CurrentKChartSymbol;
                    if (tmp == null)
                        return;
                }


                SetViewType(EnumTraderViewType.KChart);
                UpdateToolBarStatus();
                SetKChartSymbol(tmp);
                return;
            }

            if (ctrlKChart.Visible)
            {
                if (ctrlKChart.IsIntraView)
                {
                    ViewBarChart();
                    return;
                }
                if (ctrlKChart.IsBarView)
                {
                    //if (backQuote)
                    //{
                    //    ViewQuoteList();
                    //    return false;
                    //}
                    //else
                    //{
                        ViewIntraChart();
                        return;
                    //}
                  
                }
            }
            //return false;
        }


        
        void menuTrading_Click(object sender, EventArgs e)
        {
            SwitchTradingBox();
        }

        void menuSwitchKchart_Click(object sender, EventArgs e)
        {
            SwitchMainView();
        }
    }
}
