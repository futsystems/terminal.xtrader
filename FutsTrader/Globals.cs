using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace FutsTrader
{
    public class Globals
    {
        public static LoginStatus LoginStatus = new LoginStatus(false, false, false, "");

        public static Log logger = new Log("FutsTrader_Crash", true, true, "log", true);//日志组件

        public static string CashIn = string.Empty;
        public static string CashOut = string.Empty;

        public static void Debug(string msg)
        {
            logger.GotDebug(msg);
        }
    }

    public struct LoginStatus
    {
        public LoginStatus(bool isreport, bool isfailed, bool issucess, string reason)
        {
            IsReported = false;
            IsLoginFailed = false;
            IsLoginSuccess = false;
            LoginFaliedReason = "";
        }
        public bool IsReported;
        public bool IsLoginFailed;
        public bool IsLoginSuccess;
        public string LoginFaliedReason;
    }
}
