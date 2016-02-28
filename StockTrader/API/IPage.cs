using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockTrader.API
{
    public interface IPage
    {
        /// <summary>
        /// 页面类型
        /// </summary>
        EnumPageType PageType { get;  }

        /// <summary>
        /// 显示
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();
    }
}
