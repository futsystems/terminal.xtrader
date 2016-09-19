using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    public partial class MainForm
    {

        List<Control> viewList = new List<Control>();


        void SetViewType(EnumTraderViewType type)
        {
            int index = (int)type;
            if (viewList[index].Visible) return;
            foreach (var v in viewList)
            {
                v.Visible = false;
            }
            viewList[index].Visible = true;
        }
        /// <summary>
        /// 查看报价列表
        /// </summary>
        void ViewQuoteList()
        {
            SetViewType(EnumTraderViewType.QuoteList);
            ctrlQuoteList.Focus();
            UpdateToolBarStatus();
        }

        /// <summary>
        /// 查看K线图
        /// </summary>
        void ViewBarChart()
        {
            SetViewType(EnumTraderViewType.KChart);
            if (ctrlKChart.IsIntraView)
            {
                ctrlKChart.ViewType = CStock.KChartViewType.KView;
            }
            UpdateToolBarStatus();
        }


        /// <summary>
        /// 查看分时图
        /// </summary>
        void ViewIntraChart()
        {
            SetViewType(EnumTraderViewType.KChart);
            if (ctrlKChart.IsBarView)
            {
                ctrlKChart.ViewType = CStock.KChartViewType.TimeView;
            }
            UpdateToolBarStatus();
        }

        void ViewTickList()
        {
            SetViewType(EnumTraderViewType.TradeSplit);
        }
    }
}
