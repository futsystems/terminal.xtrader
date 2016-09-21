using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.XTrader.Control;

namespace XTraderLite
{
    public partial class MainForm
    {

        List<Control> viewList = new List<Control>();

        Control _curView = null;
        void SetViewType(EnumViewType type)
        {
            int index = (int)type;
            if (viewList[index].Visible) return;
            foreach (var v in viewList)
            {
                v.Visible = false;
            }
            _curView = viewList[index];
            _curView.Visible = true;
            _curView.Focus();//设置某个视图则将其获得焦点
        }
        /// <summary>
        /// 查看报价列表
        /// </summary>
        void ViewQuoteList()
        {
            SetViewType(EnumViewType.QuoteList);
            ctrlQuoteList.Focus();
            UpdateToolBarStatus();
        }

        /// <summary>
        /// 查看K线图
        /// </summary>
        void ViewBarChart()
        {
            SetViewType(EnumViewType.KChart);
            if (ctrlKChart.IsIntraView)
            {
                ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            }
            UpdateToolBarStatus();
        }


        /// <summary>
        /// 查看分时图
        /// </summary>
        void ViewIntraChart()
        {
            SetViewType(EnumViewType.KChart);
            if (ctrlKChart.IsBarView)
            {
                ctrlKChart.KChartViewType = CStock.KChartViewType.TimeView;
            }
            UpdateToolBarStatus();
        }

        /// <summary>
        /// 查看分笔明细
        /// </summary>
        void ViewTickList()
        {
            SetViewType(EnumViewType.TradeSplit);
        }


        /// <summary>
        /// 查看粉价明细
        /// </summary>
        void ViewPriceVolList()
        {
            SetViewType(EnumViewType.PriceVol);
        }

        /// <summary>
        /// 查看基础信息
        /// </summary>
        void ViewSymbolInfo()
        {
            SetViewType(EnumViewType.BasicInfo);
        }
    }
}
