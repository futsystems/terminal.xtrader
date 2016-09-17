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

        bool _splitterMoved = false;
        void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _splitterMoved = true;
        }


        bool _tradingBoxShow = false;
        void SwitchTradingBox()
        {
            if (!_tradingBoxShow)
            {
                //如果splitter没有移动过则交易面板宽度为最小宽度 设置splitterDistance为最大值 是的panel2为最小值
                if (!_splitterMoved)
                {
                    splitContainer.SplitterDistance = this.Height;// -splitContainer.Panel2MinSize;
                    //splitContainer.Panel2.Width = splitContainer.Panel2MinSize;
                }
                splitContainer.Panel2Collapsed = false;
                _tradingBoxShow = true;
            }
            else
            {
                splitContainer.Panel2Collapsed = true;
                _tradingBoxShow = false;
            }
        }


        void SwitchMainView(bool backQuote)
        {
            //当前为报价表状态 则进入分时
            if (panelQuoteList.Visible)
            {
                ViewIntraChart();
                return;
            }

            if (panelKChart.Visible)
            {
                if (kChartView.IsIntraView)
                {
                    ViewBarChart();
                    return;
                }
                if (kChartView.IsBarView)
                {
                    if (backQuote)
                    {
                        ViewQuoteList();
                    }
                    else
                    {
                        ViewIntraChart();
                    }
                    return;
                }
            }
        }


        
        void menuTrading_Click(object sender, EventArgs e)
        {
            SwitchTradingBox();
        }

        void menuSwitchKchart_Click(object sender, EventArgs e)
        {
            SwitchMainView(false);
        }
    }
}
