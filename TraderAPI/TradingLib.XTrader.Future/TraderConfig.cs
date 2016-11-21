using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.XTrader
{
    public class TraderConfig
    {
        static TraderConfig defaultinstance = null;

        static TraderConfig()
        {
            defaultinstance = new TraderConfig();
        }

        ConfigFile _cfgfile = null;
        string _cfgFN = "config//XTrader.Future.Setting.cfg";
        private TraderConfig()
        {
            bool exist =  File.Exists(_cfgFN);
            _cfgfile = new ConfigFile(_cfgFN);

            if(!exist)
            {
                _cfgfile.Set("ExSwitchSymbolOfMarketDataView", "true");//同步切换行情窗口合约
                _cfgfile.Set("ExDoubleOrderCancelIfNotFilled","true");//双击未完成委托 撤单
                _cfgfile.Set("ExDoubleOrderFilledEntryClosePosition","true");//双击已完成委托 进入平仓界面
                _cfgfile.Set("ExSwitchToOpenWhenCloseOrderSubmit","true");//平仓 平今委托发出后切换回开仓状态
                _cfgfile.Set("ExSendOrderDirect","false");//一键下单

                _cfgfile.Save();
            }
        }


        public static bool ExSwitchSymbolOfMarketDataView { get { return defaultinstance._cfgfile["ExSwitchSymbolOfMarketDataView"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSwitchSymbolOfMarketDataView",value.ToString()); } }

        public static bool ExDoubleOrderCancelIfNotFilled { get { return defaultinstance._cfgfile["ExDoubleOrderCancelIfNotFilled"].AsBool(); } set { defaultinstance._cfgfile.Set("ExDoubleOrderCancelIfNotFilled",value.ToString()); } }

        public static bool ExDoubleOrderFilledEntryClosePosition { get { return defaultinstance._cfgfile["ExDoubleOrderFilledEntryClosePosition"].AsBool(); } set { defaultinstance._cfgfile.Set("ExDoubleOrderFilledEntryClosePosition", value.ToString()); } }

        public static bool ExSwitchToOpenWhenCloseOrderSubmit { get { return defaultinstance._cfgfile["ExSwitchToOpenWhenCloseOrderSubmit"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSwitchToOpenWhenCloseOrderSubmit", value.ToString()); } }

        public static bool ExSendOrderDirect { get { return defaultinstance._cfgfile["ExSendOrderDirect"].AsBool(); } set { defaultinstance._cfgfile.Set("ExSendOrderDirect", value.ToString()); } }


        /// <summary>
        /// 保存当前设置
        /// </summary>
        public static void Save()
        {
            defaultinstance._cfgfile.Save();
        }

    }
}
