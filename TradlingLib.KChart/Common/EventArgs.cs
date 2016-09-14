using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CStock
{
    //定义事件参数类
    public class StockEventArgs : EventArgs
    {
        public readonly ClickStyle ClickType;
        public Double Date, PreClose;//日期，上一天收盘价
        public StockEventArgs(ClickStyle v)
        {
            Date = -1;
            PreClose = -1;
            ClickType = v;
        }
        public StockEventArgs(Double date, double preclose)
        {
            ClickType = ClickStyle.csDate;
            Date = date;
            PreClose = preclose;
        }
    }

    /// <summary>
    /// 信息面板Tab页切换事件参数
    /// </summary>
    public class TabSwitchEventArgs : EventArgs
    {
        public DetailBoardTabType TabType { get; set; }

        public TabSwitchEventArgs(DetailBoardTabType type)
        {
            this.TabType = type;
        }
    }

    /// <summary>
    /// 信息面板Tab页双击事件参数
    /// </summary>
    public class TabDoubleClickEventArgs : EventArgs
    {
        public DetailBoardTabType TabType { get; set; }

        public TabDoubleClickEventArgs(DetailBoardTabType type)
        {
            this.TabType = type;
        }
    }


    /// <summary>
    /// 绘图控件显示模式切换参数
    /// </summary>
    public class KChartModeChangeEventArgs : EventArgs
    {
        public KChartViewType ViewType { get; set; }

        public KChartModeChangeEventArgs(KChartViewType type)
        {
            this.ViewType = type;
        }
    }


    /// <summary>
    /// K线频率菜单事件参数
    /// 切换周期菜单点击后对外出发事件，然后在事件处理逻辑中请求对应的数据
    /// </summary>
    public class KFrequencyMenuClickEventAargs : EventArgs
    {
        public KFrequencyType KFrequencyType { get; set; }

        public KFrequencyMenuClickEventAargs(KFrequencyType type)
        {
            this.KFrequencyType = type;
        }
    }

    /// <summary>
    /// K线图加载更多数据时间参数
    /// TDX根据起始位置
    /// 0表示最近数据 默认最大800个Bar,第一次获取800个Bar后设定Start为800再获取800个
    /// 
    /// </summary>
    public class KViewLoadMoreDataEventArgs : EventArgs
    { 
        public int Count {get;set;}

        public KViewLoadMoreDataEventArgs(int count)
        {
            this.Count = count;
        }
    }
}
