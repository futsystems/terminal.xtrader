using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.XTrader
{
    public class TraderConfig
    {
        static TraderConfig defaultinstance = null;
        static ILog logger = LogManager.GetLogger("TraderConfig");
        static TraderConfig()
        {
            defaultinstance = new TraderConfig();
        }

        ConfigFile _cfgfile = null;
        string _cfgFN = "config//XTrader.Future.Setting.cfg";
        private TraderConfig()
        {
            //try
            {
                bool exist = File.Exists(_cfgFN);
                _cfgfile = new ConfigFile(_cfgFN);

                if (!_cfgfile.ContainsKey("ExSwitchSymbolOfMarketDataView")) _cfgfile.Set("ExSwitchSymbolOfMarketDataView", "true");//同步切换行情窗口合约
                if (!_cfgfile.ContainsKey("ExDoubleOrderCancelIfNotFilled")) _cfgfile.Set("ExDoubleOrderCancelIfNotFilled", "true");//双击未完成委托 撤单
                if (!_cfgfile.ContainsKey("ExDoubleOrderFilledEntryClosePosition")) _cfgfile.Set("ExDoubleOrderFilledEntryClosePosition", "true");//双击已完成委托 进入平仓界面
                if (!_cfgfile.ContainsKey("ExSwitchToOpenWhenCloseOrderSubmit")) _cfgfile.Set("ExSwitchToOpenWhenCloseOrderSubmit", "true");//平仓 平今委托发出后切换回开仓状态
                if (!_cfgfile.ContainsKey("ExPositionLine")) _cfgfile.Set("ExPositionLine", "true");//显示持仓线
                if (!_cfgfile.ContainsKey("ExSendOrderDirect")) _cfgfile.Set("ExSendOrderDirect", "false");//一键下单
                if (!_cfgfile.ContainsKey("ExFlagAuto")) _cfgfile.Set("ExFlagAuto", "true");//默认下单状态自动
                _cfgfile.Save();
            }
            //catch (Exception ex)
            {
                //logger.Error("Config Init Error:" + ex.ToString());
            }
            
        }


        public static bool ExSwitchSymbolOfMarketDataView { get { return defaultinstance._cfgfile["ExSwitchSymbolOfMarketDataView"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSwitchSymbolOfMarketDataView",value.ToString()); } }

        public static bool ExDoubleOrderCancelIfNotFilled { get { return defaultinstance._cfgfile["ExDoubleOrderCancelIfNotFilled"].AsBool(); } set { defaultinstance._cfgfile.Set("ExDoubleOrderCancelIfNotFilled",value.ToString()); } }

        public static bool ExDoubleOrderFilledEntryClosePosition { get { return defaultinstance._cfgfile["ExDoubleOrderFilledEntryClosePosition"].AsBool(); } set { defaultinstance._cfgfile.Set("ExDoubleOrderFilledEntryClosePosition", value.ToString()); } }

        public static bool ExSwitchToOpenWhenCloseOrderSubmit { get { return defaultinstance._cfgfile["ExSwitchToOpenWhenCloseOrderSubmit"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSwitchToOpenWhenCloseOrderSubmit", value.ToString()); } }

        public static bool ExSendOrderDirect { get { return defaultinstance._cfgfile["ExSendOrderDirect"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSendOrderDirect", value.ToString()); } }

        public static bool ExPositionLine { get { return defaultinstance._cfgfile["ExPositionLine"].AsBool(); } set { defaultinstance._cfgfile.Set("ExPositionLine", value.ToString()); } }

        public static bool ExFlagAuto { get { return defaultinstance._cfgfile["ExFlagAuto"].AsBool(); } set { defaultinstance._cfgfile.Set("ExFlagAuto", value.ToString()); } }

        /// <summary>
        /// 保存当前设置
        /// </summary>
        public static void Save()
        {
            defaultinstance._cfgfile.Save();
        }

    }
}
