//////////////////////////////////////////////////////////////////////////
/// 定义了客户端接口使用的业务数据结构
///
/////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace TradingLib.XLProtocol.V1
{

    /// <summary>
    /// 用户登入请求
    /// </summary>
    public struct XLReqLoginField : IXLField
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;

        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;

        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;

        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;

        /// <summary>
        /// 终端IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string ClientIPAddress;

        /// <summary>
        /// 域类别
        /// </summary>
        public ushort FieldID { get { return (ushort)XLFieldType.F_REQ_LOGIN; } }
    }
}
