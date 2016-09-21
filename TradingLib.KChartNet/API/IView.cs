using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Control
{
    /// <summary>
    /// 行情软件视图接口
    /// 报价,K线/分时,分笔列表,分价列表,基本信息等都是一个View
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 视图类别
        /// </summary>
        EnumViewType ViewType { get;}

        /// <summary>
        /// 是否可见
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();


        /// <summary>
        /// 获得焦点
        /// </summary>
        bool Focus();
    }
}
