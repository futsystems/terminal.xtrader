using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.MarketData;

namespace XTraderLite
{
    public class Global
    {
        /// <summary>
        /// 板块列表
        /// </summary>
        public static List<string> QuoteBlockList = new List<string>();

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
        /// 部署编号
        /// </summary>
        public static string DeployID = string.Empty;

        /// <summary>
        /// 配置服务器
        /// </summary>
        public static string AppServer = string.Empty;

        /// <summary>
        /// 在线出入金地址
        /// </summary>
        public static string PayUrl = "";


        public static AppConfig AppConfig { get; set; }
    }
}
