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
        /// 品牌名称
        /// </summary>
        public static string BrandName = "";

        /// <summary>
        /// 公司名称
        /// </summary>
        public static string BrandCompany = "";

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
        public static string NewsUrl = "";

        public static int NewsBtn = 0;

        public static bool RiskPrompt = false;

        /// <summary>
        /// 行情服务器分配版本
        /// </summary>
        public static int DataFarmGroup = 0;

        /// <summary>
        /// 显示行情IP地址
        /// </summary>
        public static bool ShowMDIP = false;

        /// <summary>
        /// 默认行情用户
        /// </summary>
        public static string DefaultMarketUser = string.Empty;

        /// <summary>
        /// 默认开启 停留的页面
        /// </summary>
        public static string DefaultBlock = string.Empty;

        public static int XGJCTRL_R = 0;
        public static int XGJCTRL_B = 0;
        public static int XGJCTRL_G = 0;

        /// <summary>
        /// 配置服务器版本
        /// </summary>
        //public static string AppServerVer { get; set; }

        //public static AppConfig AppConfig { get; set; }

        /// <summary>
        /// 组设定
        /// </summary>
        //public static DataFarmConfig GroupConfig { get; set; }

        /// <summary>
        /// 部署设定
        /// </summary>
        //public static DataFarmConfig DeoplyConfig { get; set; }


        public static DeployConfig Config { get; set; }
    }
}
