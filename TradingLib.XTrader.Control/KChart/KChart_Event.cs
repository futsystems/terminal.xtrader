using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStock
{
    /// <summary>
    /// 绘图控件外部事件
    /// </summary>
    public partial class TStock
    {


        //用event 关键字声明事件对象
        public event Action<object, TabDoubleClickEventArgs> TabDoubleClick;


        /// <summary>
        /// 盘口信息面板底部Tab切换操作
        /// 通过参数int获得切换面板 从而实现对数据的请求
        /// TStock监听该事件 通过int判断所需数据并向服务端发起对应请求
        /// </summary>
        public event Action<object, TabSwitchEventArgs> TabSwitch;


        /// <summary>
        /// 分时图表显示日期改变事件
        /// </summary>
        public event Action<object, int> TimeViewDaysChanged;

        /// <summary>
        /// 显示模式切换事件
        /// </summary>
        public event Action<object, KChartModeChangeEventArgs> KChartModeChange;


        /// <summary>
        /// K线周期切换事件
        /// </summary>
        public event Action<object, KFrequencyMenuClickEventAargs> KFrequencyMenuClick;

        /// <summary>
        /// K线图加载更多数据
        /// </summary>
        public event Action<object, KViewLoadMoreDataEventArgs> KViewLoadMoreData;
    }
}
