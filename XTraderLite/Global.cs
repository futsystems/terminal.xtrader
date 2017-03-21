using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTraderLite
{
    public class Global
    {
        public static bool IsXGJStyle = true;

        /// <summary>
        /// 经典登入
        /// </summary>
        public static bool ClassicLogin = false;
        /// <summary>
        /// 顶部标题名称
        /// </summary>
        public static string HeadTitle = "信管家";

        /// <summary>
        /// 显示左上角图标
        /// </summary>
        public static bool ShowCorner = false;


        /// <summary>
        /// 任务栏显示文字
        /// </summary>
        public static string TaskBarTitle = "测试";

        /// <summary>
        /// 是否显示长合约格式
        /// 长格式 HSI1701
        /// 短格式 HSI01
        /// </summary>
        public static bool LongSymbolName = true;

        /// <summary>
        /// 
        /// </summary>
        public static int BoardTitleSymbolStyle = 1;
        /// <summary>
        /// 行情插件
        /// </summary>
        //public static string PluginMarket = "";
        /// <summary>
        /// 交易插件
        /// </summary>
        //public static string PluginBroker = "";

        /// <summary>
        /// 在线出入金地址
        /// </summary>
        public static string PayUrl = "";
    }
}
