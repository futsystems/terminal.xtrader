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
    /// 行情接口工作参数
    /// </summary>
    public class TraderAPISetting
    {

        /// <summary>
        /// 交易框最小高度
        /// </summary>
        public int TradingBoxMinHeight { get; set; }
    }



    /// <summary>
    /// 交易控件接口
    /// </summary>
    public interface ITraderAPI
    {
        TraderAPISetting APISetting { get; }
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

        /// <summary>
        /// 进入委托提交状态
        /// </summary>
        /// <param name="side"></param>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        void EntryOrder(bool side, string exchange, string symbol);

        /// <summary>
        /// 选中某合约
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        void SelectSymbol(string exchange, string symbol);

        /// <summary>
        /// 提交委托
        /// </summary>
        /// <param name="side"></param>
        /// <param name="exchagne"></param>
        /// <param name="symbol"></param>
        /// <param name="size"></param>
        /// <param name="price"></param>
        void PlaceOrder(bool side, string exchagne, string symbol, int size, double price);

        /// <summary>
        /// 查看某合约的分时/K线
        /// </summary>
        event Action<string, string,int> ViewKChart;

        /// <summary>
        /// 窗口最小化 最大化 关闭操作
        /// </summary>
        event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

        /// <summary>
        /// 持仓更新事件
        /// </summary>
        event Action<string, string,bool, int, decimal> PositionNotify;

        /// <summary>
        /// 交易数据重置
        /// 刷新交易数据时 需要将行情组件内的持仓信息重置 然后再讲最新数据填充到行情组件
        /// </summary>
        event Action TradingInfoRest;


        /// <summary>
        /// 交易组件所需合约注册列表
        /// </summary>
        IEnumerable<string> SymbolRegisters { get; }

        /// <summary>
        /// 合约注册集合发生变动
        /// </summary>
        event Action SymbolRegisterChanged;

        /// <summary>
        /// 合约更新行情数据
        /// </summary>
        /// <param name="symbol"></param>
        void NotifyTick(MDSymbol symbol);


    }
}
