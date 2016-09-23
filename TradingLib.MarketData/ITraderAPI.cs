using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.MarketData
{

    public enum EnumTraderWindowOperation
    { 
        Min,
        Max,
        Close,
    }


    /// <summary>
    /// 交易控件接口
    /// </summary>
    public interface ITraderAPI
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// 获得焦点
        /// </summary>
        /// <returns></returns>
        bool Focus();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();


        /// <summary>
        /// 显示
        /// </summary>
        void Show();


        #region 控件最小化 最大化 关闭等操作


        event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

        #endregion

    }
}
