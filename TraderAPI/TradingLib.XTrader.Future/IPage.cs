using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader
{
    /// <summary>
    /// IPage接口 用于在某个Panel的Controls中显示多种类别的控件
    /// 在不同的状态或选择下将某个控件置余最前
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// 名字
        /// </summary>
        string PageName { get; }

        /// <summary>
        /// 显示页面
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏页面
        /// </summary>
        void Hide();

        /// <summary>
        /// 获得焦点
        /// </summary>
        bool Focus();
    }
}
