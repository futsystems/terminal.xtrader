using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STOCKCHARTXLib;
using AxSTOCKCHARTXLib;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.KryptonControl
{
    public class ChartPanel
    {
        AxStockChartX _StockChartX;
        int _panelIdx = 0;

        public string Name { get;set; }

        public int PanelIdx { get { return _panelIdx; } }

        public ChartPanel(string name,AxStockChartX stockchart)
        {
            this.Name = name;
            _StockChartX = stockchart;
            _panelIdx = _StockChartX.AddChartPanel();
        }

        /// <summary>
        /// 在ChartPanel中添加一个数据序列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void AddSeries(string name, SeriesType type = SeriesType.stCandleChart)
        {
            _StockChartX.AddSeries(name, type, _panelIdx);
        }
    }
}
